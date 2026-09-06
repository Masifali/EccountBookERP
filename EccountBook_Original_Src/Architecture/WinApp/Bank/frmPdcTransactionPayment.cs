using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.BankSchema;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.BankSchema;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.ChequePrinting;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Bank;

public class frmPdcTransactionPayment : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private bool UpdateMode;

	private int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private DataTable dtHistory = new DataTable();

	private DataTable dtAccountAll = new DataTable();

	private bool Approved;

	private bool ChequeBookEnableStatus;

	private bool ChequePrintingEnable;

	private bool FormInitialized;

	public DateTime FromDate;

	public DateTime ToDate;

	public bool RequestedByOtherDocument;

	public int CreditAccountId;

	public int DebitAccountId;

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private ToolStripButton btnattachment;

	private Label label23;

	private Panel panel4;

	private Label label33;

	private DateTimePicker txtDocDateMain;

	private Label label17;

	private TextBox txtDocNoMain;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	private CheckBox ChkBox;

	private ToolStrip toolStrip2;

	private ToolStripButton btnDelete;

	private ToolStripButton btnSaveAs;

	private GroupBox groupBox1;

	private Panel panel10;

	private ToolStripButton BtnShortCutkeys;

	private Panel panel8;

	private GroupBox groupBox2;

	private Button btnShowMain;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	private TextBox ToDocNoHistoryMain;

	private TextBox FromDocNoHistoryMain;

	private DateTimePicker ToDateHistoryMain;

	private DateTimePicker FromDateHistoryMain;

	private Panel panel9;

	private Panel panel12;

	private ToolStripButton btnResetHistoryMain;

	private ToolStripButton btnRefreshHistoryMain;

	private Label label55;

	private RadioButton rdapproveddateMain;

	private RadioButton rdmodifydateMain;

	private RadioButton drdocdateMain;

	private RadioButton rdentrydateMain;

	private Panel panel14;

	private Label label5;

	private TextBox txtRemarksMain;

	private Panel panel6;

	private ToolStripButton BtnRefPartyForm;

	private Label label10;

	private Label label7;

	private Label label3;

	private Label label19;

	private Label label9;

	internal UltraCombo CmbDebitAccount;

	internal UltraCombo CmbCreditAccount;

	internal UltraCombo CmbChequeBank;

	internal UltraCombo CmbChequeNo;

	private DateTimePicker txtChequeDate;

	private Label label8;

	private Label label4;

	private Label label1;

	private Label label18;

	private Label label6;

	private TextBox txtChequeAmount;

	private Label label14;

	private Label label25;

	private Label label27;

	internal UltraCombo CmbPayeeTitle;

	internal UltraCombo CmbChequeType;

	private Label label20;

	private Label label26;

	private Label label11;

	private FlowLayoutPanel FlPanelChequeTemplate;

	private GroupBox groupBox3;

	private DateTimePicker ToDateHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnShow;

	private Label label2;

	private Label label12;

	private Label label13;

	private Label label15;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker FromDateHistory;

	private Label label16;

	internal UltraCombo CmbDebitAccountHistory;

	internal UltraCombo CmbCreditAccountHistory;

	internal UltraCombo CmbChequeBankHistory;

	private Label label28;

	private Label label29;

	private Label label30;

	internal UltraCombo CmbDebitAccountHistoryMain;

	private Label label24;

	internal UltraCombo CmbCreditAccountHistoryMain;

	internal UltraCombo CmbChequeBankHistoryMain;

	private Label label21;

	private Label label22;

	private TextBox txtDrBalance;

	private TextBox txtCrBalance;

	private Button BtnNewHistory;

	private Button btnRefreshHistory;

	private CheckBox cbPrintOnSave;

	private RadioButton rdAll;

	private RadioButton rdComplete;

	private RadioButton rdPending;

	public frmPdcTransactionPayment(UserAccount user)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
	}

	private void frmPdcTransactionPayment_Load(object sender, EventArgs e)
	{
		try
		{
			InitializeForm();
			InitializeDataAsync();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error during load: " + ex.Message);
		}
	}

	private void InitializeForm()
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(5, 5);
		base.AutoScrollMinSize = new Size(1200, 500);
		btnupdate.Visible = false;
		btnDelete.Visible = false;
		btnsave.Visible = true;
		btnSaveAs.Visible = false;
		txtDocDateMain.Focus();
	}

	private async void InitializeDataAsync()
	{
		try
		{
			ApplyDataToUI(await Task.Run(delegate
			{
				Rightsobjects formRight = CommonServices.SetRightsValueInRightsObject(base.Name);
				List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate").ToList();
				if (list.Count > 0)
				{
					DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)list[0].ConfigKey);
				}
				bool chequeBookEnableStatus = clsGlobalVariables.configrationsAllocation.Any((ConfigrationsAllocation x) => x.ConfigDescription == "CheqBook Enabled" && !string.IsNullOrEmpty(x.ConfigKey));
				string docNo = DocumentNoFill();
				bool chequePrinting = clsGlobalVariables.configrationsAllocation.Any((ConfigrationsAllocation x) => x.ConfigDescription == "ChequePrintingEnable" && !string.IsNullOrEmpty(x.ConfigKey));
				bool chequeNoCompulsoryOnBpv = clsGlobalVariables.configrationsAllocation.Any((ConfigrationsAllocation x) => x.ConfigDescription == "ChequeNoCompulsoryOnBpv" && !string.IsNullOrEmpty(x.ConfigKey));
				return new
				{
					formRight = formRight,
					docNo = docNo,
					DefaultDaysToLessFromHistoryFromDate = DefaultDaysToLessFromHistoryFromDate,
					ChequeBookEnableStatus = chequeBookEnableStatus,
					ChequePrinting = chequePrinting,
					ChequeNoCompulsoryOnBpv = chequeNoCompulsoryOnBpv
				};
			}));
			InitializeHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error during data initialization: " + ex.Message);
		}
	}

	private void InitializeHistory()
	{
		try
		{
			DateTime StartDate = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			HistoryComboFill();
			if (RequestedByOtherDocument)
			{
				FromDateHistory.Value = FromDate;
				ToDateHistory.Value = ToDate;
				FromDateHistoryMain.Value = FromDate;
				ToDateHistoryMain.Value = ToDate;
				if (CreditAccountId > 0)
				{
					CmbCreditAccountHistoryMain.Value = CreditAccountId;
				}
				if (DebitAccountId > 0)
				{
					CmbDebitAccountHistoryMain.Value = DebitAccountId;
				}
				FillGridHistoryMain();
				tabControl1.SelectedIndex = 1;
			}
			else
			{
				FromDateHistory.Value = StartDate;
				ToDateHistory.Value = DateTime.Now;
				FromDateHistoryMain.Value = StartDate;
				ToDateHistoryMain.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApplyDataToUI(dynamic data)
	{
		try
		{
			formright = data.formRight;
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnSaveAs.Enabled = formright.DoHaveSaveRight;
			btnprint.Enabled = formright.DoHavePrintRights;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			ChkBox.Checked = formright.DoHavePrintRights;
			ChequePrintingEnable = data.ChequePrinting;
			cbPrintOnSave.Visible = ChequePrintingEnable;
			UpdateControlGroupRights(ctrlGrdBar1, formright);
			UpdateControlGroupRights(ctrlGrdBar2, formright);
			UpdateDocumentNoUI(data.docNo);
			DefaultDaysToLessFromHistoryFromDate = data.DefaultDaysToLessFromHistoryFromDate;
			ChequeBookEnableStatus = data.ChequeBookEnableStatus;
			if (data.ChequeNoCompulsoryOnBpv)
			{
				CmbChequeNo.LimitToList = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateControlGroupRights(dynamic ctrlGrdBar, dynamic rights)
	{
		try
		{
			ctrlGrdBar.mGridChooseFielder.Enabled = rights.DoHaveFieldChooserRights;
			ctrlGrdBar.mGridSaveLayouts.Enabled = rights.DoHaveSaveLayoutRights;
			ctrlGrdBar.mGridPrint.Enabled = rights.DoHaveGridPrintRights;
			ctrlGrdBar.mGridExport.Enabled = rights.DoHaveGridExportRights;
			ctrlGrdBar.GroupCollaps.Enabled = rights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar.GroupExpand.Enabled = rights.DoHaveGridGroupExpandRights;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentCustom()
	{
		try
		{
			FormInitialized = true;
			BankNameFill();
			AllAccountFill();
			CreditAccountComboFill();
			DebitAccountComboFill();
			ChequeTypeFill();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private string DocumentNoFill()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		string Code = "";
		try
		{
			Code = Conversion.ToString((object)pdcTransaction.GenerateCode(new pdcTransaction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				documentTypeId = 250,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return Code.ToString();
	}

	private void UpdateDocumentNoUI(string DocNo)
	{
		txtDocNoMain.Text = DocNo;
	}

	private void BankNameFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbChequeBank.Value);
			DataTable dt = BankName.FormHistory();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbChequeBank, "bankNameId", "bankName", "bankName", ZeroIndex: true);
				((UltraGridBase)CmbChequeBank).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbChequeBank).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("bankNameId") == ID))
					{
						CmbChequeBank.Value = ID;
					}
					else
					{
						((Control)(object)CmbChequeBank).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbChequeBank).Text = string.Empty;
				((UltraGridBase)CmbChequeBank).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ChequeTypeFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbChequeType.Value);
			DataTable dt = ChequeType.FormHistory();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbChequeType, "ChequeTypeId", "CheqTypeName", "CheqType", true);
				((UltraGridBase)CmbChequeType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("ChequeTypeId") == ID))
					{
						CmbChequeType.Value = ID;
					}
					else
					{
						((Control)(object)CmbChequeType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbChequeType).Text = string.Empty;
				((UltraGridBase)CmbChequeType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void payeeTitleFill(int AccountId)
	{
		try
		{
			long ID = Conversion.ToInt64(CmbPayeeTitle.Value);
			DataTable dt = PayeeTitle.payeeTitle_ByGlAccountId(UserAccount.OrganizationId, UserAccount.CompanyId, AccountId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbPayeeTitle, "payeeTitleId", "payeeTitle", "payeeTitle", false);
				((UltraGridBase)CmbPayeeTitle).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<long>("payeeTitleId") == ID))
					{
						CmbPayeeTitle.Value = ID;
					}
					else
					{
						((Control)(object)CmbPayeeTitle).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbPayeeTitle).Text = string.Empty;
				((UltraGridBase)CmbPayeeTitle).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CheqNoFill(int CreditAccountId)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		try
		{
			if (!ChequeBookEnableStatus)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CheqNo");
			string ID = Conversion.ToString((object)((Control)(object)CmbChequeNo).Text);
			((Control)(object)CmbChequeNo).Text = string.Empty;
			DataTable dtCheq = CheqBookHeader.OutstandingCheqNo(new CheqBookHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BankId = CreditAccountId
			});
			if (dtCheq.Rows.Count > 0)
			{
				dt.Clear();
				for (int i = 0; i < dtCheq.Rows.Count; i++)
				{
					dt.Rows.Add(dtCheq.Rows[i]["Id"], dtCheq.Rows[i]["CheqNo"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbChequeNo, "Id", "CheqNo", "CheqNo", ZeroIndex: false);
				if (!string.IsNullOrEmpty(ID))
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("CheqNo") == ID))
					{
						CmbChequeNo.Value = ID;
					}
					else
					{
						((Control)(object)CmbChequeNo).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbChequeNo).Text = string.Empty;
				((UltraGridBase)CmbChequeNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllAccountFill()
	{
		try
		{
			dtAccountAll.Clear();
			dtAccountAll = CommonServices.CoaAllocationGetAllServiceBind();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CreditAccountComboFill()
	{
		try
		{
			DataTable dtAccountInCombo = new DataTable();
			dtAccountInCombo.Columns.Add("Id", typeof(int));
			dtAccountInCombo.Columns.Add("AccountTitle");
			dtAccountInCombo.Columns.Add("AccountCode");
			int CrId = Conversion.ToInt(CmbCreditAccount.Value);
			if (dtAccountAll != null && dtAccountAll.Rows.Count > 0)
			{
				foreach (DataRow r in dtAccountAll.Rows)
				{
					if (Convert.ToInt32(r["AccountTypeId"]) == 15)
					{
						dtAccountInCombo.Rows.Add(r["Id"], r["AccountTitle"], r["AccountCode"]);
					}
				}
			}
			if (dtAccountInCombo.Rows.Count > 0)
			{
				DDL.BindDDL(dtAccountInCombo, CmbCreditAccount, "Id", "AccountTitle", "Credit Account", ZeroIndex: true);
				if (CrId > 0)
				{
					if (dtAccountInCombo.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == CrId))
					{
						CmbCreditAccount.Value = CrId;
					}
					else
					{
						((Control)(object)CmbCreditAccount).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCreditAccount).Text = string.Empty;
				((UltraGridBase)CmbCreditAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DebitAccountComboFill()
	{
		try
		{
			bool ExpenseAccountAllowOnPaymentVoucher = clsGlobalVariables.configrationsAllocation.Any((ConfigrationsAllocation x) => x.ConfigDescription == "ExpenseAccountAllowOnPaymentVoucher" && Conversion.ToBool((object)x.ConfigKey.ToString()));
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AccountTitle");
			dt.Columns.Add("AccountCode");
			int DrId = Conversion.ToInt(CmbDebitAccount.Value);
			if (dtAccountAll != null && dtAccountAll.Rows.Count > 0)
			{
				HashSet<int> excludedAccountTypes = (ExpenseAccountAllowOnPaymentVoucher ? new HashSet<int> { 2, 15, 4, 10, 12 } : new HashSet<int> { 2, 15, 4, 10, 11, 12, 13, 14, 20, 21 });
				foreach (DataRow r in dtAccountAll.Rows)
				{
					int accountTypeId = Convert.ToInt32(r["AccountTypeId"]);
					if (!excludedAccountTypes.Contains(accountTypeId))
					{
						dt.Rows.Add(r["Id"], r["AccountTitle"], r["AccountCode"]);
					}
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbDebitAccount, "Id", "AccountTitle", "Debit Account", ZeroIndex: true);
				if (DrId > 0 && dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DrId))
				{
					CmbDebitAccount.Value = DrId;
				}
				else
				{
					((Control)(object)CmbDebitAccount).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbDebitAccount).Text = string.Empty;
				((UltraGridBase)CmbDebitAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception("An error occurred while processing the payment voucher: " + ex.Message, ex);
		}
	}

	private void DetailAccountCurrentBalance(int AccountId, TextBox lbl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = txtDocDateMain.Value,
				RefAccountId = AccountId
			});
			if (detail.Rows.Count > 0)
			{
				lbl.Text = Conversion.ToDouble(detail.Rows[0]["Balance"]).ToString("#,#;(#,#);0");
			}
			else
			{
				lbl.Text = 0.ToString("#,#;(#,#);0");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbChequeBank_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbChequeBank.Value) > 0)
			{
				string TemplateName = Conversion.ToString(((UltraDropDownBase)CmbChequeBank).SelectedRow.Cells["TemplateName"].Value);
				GeneratingCard(TemplateName);
			}
			else
			{
				FlPanelChequeTemplate.Controls.Clear();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbCreditAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			DetailAccountCurrentBalance(Conversion.ToInt(CmbCreditAccount.Value), txtCrBalance);
			CheqNoFill(Conversion.ToInt(CmbCreditAccount.Value));
			if (FlPanelChequeTemplate.Controls.Count > 0)
			{
				dynamic dynamicCard = FlPanelChequeTemplate.Controls[0];
				dynamicCard.ChequeTypeId = Conversion.ToInt(CmbChequeType.Value);
				dynamicCard.CheqType = Conversion.ToString((object)((Control)(object)CmbChequeType).Text);
				dynamicCard.ChequeNumber = Conversion.ToString((object)((Control)(object)CmbChequeNo).Text);
				dynamicCard.PayeeTitle = Conversion.ToString((object)((Control)(object)CmbPayeeTitle).Text);
				double chequeAmount = Conversion.ToDouble((object)txtChequeAmount.Text);
				dynamicCard.Amount = chequeAmount.ToString("##,#.####");
				dynamicCard.AmountInWords = ConvertAmountToWords(chequeAmount);
				DateTime CheqDate = Conversion.ToDateTime((object)txtChequeDate.Value);
				SetDateProperties(dynamicCard, CheqDate);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtDocNoMain.Text.Trim() == string.Empty || txtDocNoMain.Text.Trim() == "0" || txtDocNoMain.Text.Trim() == null)
		{
			MessageBox.Show("Doc No Field is Required");
			txtDocNoMain.Focus();
			return false;
		}
		if (((UltraGridBase)CmbChequeBank).ActiveRow == null || Conversion.ToInt(CmbChequeBank.Value) == 0)
		{
			MessageBox.Show("Cheque Bank Field is Required");
			CmbChequeBank.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCreditAccount).ActiveRow == null || Conversion.ToInt(CmbCreditAccount.Value) == 0)
		{
			MessageBox.Show("Credit Account Field is Required");
			CmbCreditAccount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDebitAccount).ActiveRow == null || Conversion.ToInt(CmbDebitAccount.Value) == 0)
		{
			MessageBox.Show("Debit Account Field is Required");
			CmbDebitAccount.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(((Control)(object)CmbPayeeTitle).Text.Trim()) || ((Control)(object)CmbPayeeTitle).Text.Trim() == "0")
		{
			MessageBox.Show("Payee Title Field is Required");
			CmbPayeeTitle.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(((Control)(object)CmbChequeNo).Text.Trim()) || ((Control)(object)CmbChequeNo).Text.Trim() == "0")
		{
			MessageBox.Show("ChequeNo Field is Required");
			CmbChequeNo.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(txtChequeAmount.Text.Trim()) || Conversion.ToDouble((object)txtChequeAmount.Text) == 0.0)
		{
			MessageBox.Show("ChequeAmount Field is Required");
			txtChequeAmount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbChequeType).ActiveRow == null || Conversion.ToInt(CmbChequeType.Value) == 0)
		{
			MessageBox.Show("ChequeType Field is Required");
			CmbChequeType.Focus();
			return false;
		}
		return true;
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Delete because Record has approved");
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					pdcTransaction.DeleteByID(RecId, UserAccount.ID);
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
			}
			else
			{
				MessageBox.Show("Record Not Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveAs_Click(object sender, EventArgs e)
	{
		try
		{
			Approved = false;
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
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			pdcTransaction po = new pdcTransaction();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				po.pdcTransactionId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			DateTime chequeDate = Convert.ToDateTime(txtChequeDate.Value);
			if ((DateTime.Now - chequeDate).Days > 7 && MessageBox.Show("The cheque date is older than 7 days. Are you sure you want to proceed?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.BranchId = UserAccount.BranchesId;
			po.ProjectId = 0;
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.documentTypeId = 250;
			po.EntryUserId = UserAccount.ID;
			po.ModifyUserId = UserAccount.ID;
			po.ApprovedUserId = UserAccount.ID;
			po.ApprovedDate = DateTime.Now;
			po.voucherDate = DateTime.Now;
			po.docDate = txtDocDateMain.Value;
			po.docNo = Conversion.ToInt((object)txtDocNoMain.Text);
			po.BankNameId = Conversion.ToInt(CmbChequeBank.Value);
			po.creditAccountId = Conversion.ToInt(CmbCreditAccount.Value);
			po.debitAccountId = Conversion.ToInt(CmbDebitAccount.Value);
			po.payeeTitleId = Conversion.ToInt(CmbPayeeTitle.Value);
			po.payeeTitle = Conversion.ToString((object)((Control)(object)CmbPayeeTitle).Text);
			po.CheqBookDetailId = Conversion.ToInt(CmbChequeNo.Value);
			po.cheqNo = Conversion.ToString((object)((Control)(object)CmbChequeNo).Text);
			po.cheqDate = Conversion.ToDateTime((object)txtChequeDate.Value);
			po.cheqAmount = Conversion.ToDouble((object)txtChequeAmount.Text);
			po.cheqTypeId = Conversion.ToInt(CmbChequeType.Value);
			po.remarks = Conversion.ToString((object)txtRemarksMain.Text);
			po.AttachmentsList = new List<DMSAttachments>();
			po.AttachmentsList = new List<DMSAttachments>();
			po.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<pdcTransaction>(po, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			long success = (po.pdcTransactionId = pdcTransaction.Save(po));
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + po.docNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 250, 0);
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + po.docNo);
			}
			if (ChequePrintingEnable && cbPrintOnSave.Checked)
			{
				new Architecture.WinApp.ChequePrinting.ChequePrinting(UserAccount).ShowDialog();
			}
			Reset();
			if (ChkBox.Checked)
			{
				CommonServices.PdcPaymentTransactionsSlip250(Conversion.ToInt((object)success));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Approved = false;
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Not Update because RecId Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			RecId = Conversion.ToInt((object)ID);
			pdcTransaction po = pdcTransaction.GetByID(RecId);
			if (po == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			txtDocDateMain.Value = po.docDate;
			txtDocNoMain.Text = Conversion.ToString((object)po.docNo);
			CmbChequeBank.Value = Conversion.ToInt((object)po.BankNameId);
			CmbCreditAccount.Value = Conversion.ToInt((object)po.creditAccountId);
			CmbDebitAccount.Value = Conversion.ToInt((object)po.debitAccountId);
			CmbCreditAccount_Leave(null, null);
			((Control)(object)CmbPayeeTitle).Text = Conversion.ToString((object)po.payeeTitle);
			CmbChequeBank_Leave(null, null);
			((Control)(object)CmbChequeNo).Text = Conversion.ToString((object)po.cheqNo);
			txtChequeDate.Value = Conversion.ToDateTime((object)po.cheqDate);
			txtChequeAmount.Text = Conversion.ToString((object)po.cheqAmount);
			CmbChequeType.Value = Conversion.ToInt((object)po.cheqTypeId);
			txtRemarksMain.Text = Conversion.ToString((object)po.remarks);
			Approved = Conversion.ToBool((object)po.IsApproved);
			List<DMSAttachments> byID = DMSAttachments.GetByID(RecId, base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.ActionId = 2;
				TA.Path = Attachment.Attachment;
				TA.CustomFileName = Attachment.UploadedFileCustomName;
				AT.lst.Add(TA);
			}
			btnsave.Visible = false;
			btnupdate.Visible = true;
			btnDelete.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadbyIdForSaveAsRecord(int CurrId)
	{
		try
		{
			Reset();
			RecId = CurrId;
			ReadById(RecId);
			btnsave.Visible = false;
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			btnSaveAs.Visible = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			BankNameFill();
			AllAccountFill();
			CreditAccountComboFill();
			DebitAccountComboFill();
			ChequeTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			Approved = false;
			RecId = 0;
			((Control)(object)CmbChequeBank).Text = string.Empty;
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			((Control)(object)CmbDebitAccount).Text = string.Empty;
			((Control)(object)CmbPayeeTitle).Text = string.Empty;
			((UltraGridBase)CmbPayeeTitle).DataSource = null;
			((Control)(object)CmbChequeNo).Text = string.Empty;
			((UltraGridBase)CmbChequeNo).DataSource = null;
			txtChequeAmount.Text = string.Empty;
			txtRemarksMain.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			btnSaveAs.Visible = false;
			txtDocNoMain.Text = DocumentNoFill();
			txtDocDateMain.Focus();
			FlPanelChequeTemplate.Controls.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierDispatch_FormClosing(object sender, FormClosingEventArgs e)
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

	private void ConfigureNumericalColumn(GridEXColumn column)
	{
		if (column.Key.Contains("Amount"))
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.stringFormatsingle;
			column.TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		else if (column.Key.Contains("Rate"))
		{
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.DecimalRateFormate;
		}
		else
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = "#,##0.##";
			column.TotalFormatString = "#,##0.##";
		}
	}

	private void HistoryComboFill()
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		try
		{
			DataTable dtBankName = new DataTable();
			dtBankName.Columns.Add("Id", typeof(int));
			dtBankName.Columns.Add("name", typeof(string));
			DataTable dtCreditAccount = new DataTable();
			dtCreditAccount.Columns.Add("Id", typeof(int));
			dtCreditAccount.Columns.Add("name", typeof(string));
			DataTable dtDebitAccount = new DataTable();
			dtDebitAccount.Columns.Add("Id", typeof(int));
			dtDebitAccount.Columns.Add("name", typeof(string));
			DataTable dt = pdcTransaction.GetDataForDropDownFrompdcTransaction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "BankName")
					{
						dtBankName.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "CreditAccount")
					{
						dtCreditAccount.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "DebitAccount")
					{
						dtDebitAccount.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtBankName, CmbChequeBankHistoryMain, "Id", "name", "Bank Name", ZeroIndex: false);
			DDL.BindDDL(dtBankName, CmbChequeBankHistory, "Id", "name", "Bank Name", ZeroIndex: false);
			DDL.BindDDL(dtCreditAccount, CmbCreditAccountHistoryMain, "Id", "name", "CreditAccount", ZeroIndex: false);
			DDL.BindDDL(dtCreditAccount, CmbCreditAccountHistory, "Id", "name", "CreditAccount", ZeroIndex: false);
			DDL.BindDDL(dtDebitAccount, CmbDebitAccountHistoryMain, "Id", "name", "DebitAccount", ZeroIndex: false);
			DDL.BindDDL(dtDebitAccount, CmbDebitAccountHistory, "Id", "name", "DebitAccount", ZeroIndex: false);
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
				FromDateHistoryMain.Focus();
			}
			else
			{
				txtDocDateMain.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetHistoryMain_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistoryMain.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistoryMain.Value = DateTime.Now;
			FromDocNoHistoryMain.Clear();
			ToDocNoHistoryMain.Clear();
			((Control)(object)CmbChequeBankHistoryMain).Text = string.Empty;
			((Control)(object)CmbCreditAccountHistoryMain).Text = string.Empty;
			((Control)(object)CmbDebitAccountHistoryMain).Text = string.Empty;
			grdhistory.DataSource = null;
			drdocdateMain.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistoryMain_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowMain_Click(object sender, EventArgs e)
	{
		try
		{
			FillGridHistoryMain();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FillGridHistoryMain()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.DocumentTypeId = 250;
			reports.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			reports.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			if (drdocdateMain.Checked)
			{
				if (FromDateHistoryMain.Checked)
				{
					reports.FromDate = FromDateHistoryMain.Value;
				}
				if (ToDateHistoryMain.Checked)
				{
					reports.ToDate = ToDateHistoryMain.Value;
				}
			}
			else if (rdentrydateMain.Checked)
			{
				if (FromDateHistoryMain.Checked)
				{
					reports.EntryDateFrom = FromDateHistoryMain.Value;
				}
				if (ToDateHistoryMain.Checked)
				{
					reports.EntryDateTo = ToDateHistoryMain.Value;
				}
			}
			else if (rdmodifydateMain.Checked)
			{
				if (FromDateHistoryMain.Checked)
				{
					reports.ModifyDateFrom = FromDateHistoryMain.Value;
				}
				if (ToDateHistoryMain.Checked)
				{
					reports.ModifyDateTo = ToDateHistoryMain.Value;
				}
			}
			else if (rdapproveddateMain.Checked)
			{
				if (FromDateHistoryMain.Checked)
				{
					reports.ApprovedDateFrom = FromDateHistoryMain.Value;
				}
				if (ToDateHistoryMain.Checked)
				{
					reports.ApprovedDateTo = ToDateHistoryMain.Value;
				}
			}
			reports.FromDocNo = Conversion.ToInt((object)FromDocNoHistoryMain.Text);
			reports.ToDocNo = Conversion.ToInt((object)ToDocNoHistoryMain.Text);
			reports.BankId = Conversion.ToInt(CmbChequeBankHistoryMain.Value);
			reports.CreditAccountId = Conversion.ToInt(CmbCreditAccountHistoryMain.Value);
			reports.DebitAccountId = Conversion.ToInt(CmbDebitAccountHistoryMain.Value);
			reports.StatusId = ((!rdAll.Checked) ? (rdPending.Checked ? 1 : 2) : 0);
			DataTable dtGridRegister = pdcTransaction.FormHistory(reports);
			if (dtGridRegister.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(string));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DebitAccountId", typeof(int));
				dtcol.Columns.Add("DebitAccountTitle", typeof(string));
				dtcol.Columns.Add("CreditAccountId", typeof(int));
				dtcol.Columns.Add("CreditAccountTitle", typeof(string));
				dtcol.Columns.Add("BankNameId", typeof(int));
				dtcol.Columns.Add("BankName", typeof(string));
				dtcol.Columns.Add("ChequeBookDetailId", typeof(int));
				dtcol.Columns.Add("ChequeNo", typeof(string));
				dtcol.Columns.Add("ChequeDate", typeof(DateTime));
				dtcol.Columns.Add("payeeTitleId", typeof(int));
				dtcol.Columns.Add("payeeTitle", typeof(string));
				dtcol.Columns.Add("ChequeTypeId", typeof(int));
				dtcol.Columns.Add("ChequeAmount", typeof(decimal));
				dtcol.Columns.Add("seqNo", typeof(int));
				dtcol.Columns.Add("remarks", typeof(string));
				dtcol.Columns.Add("voucherId", typeof(int));
				dtcol.Columns.Add("voucherCode", typeof(string));
				dtcol.Columns.Add("voucherDate", typeof(DateTime));
				dtcol.Columns.Add("IsApproved", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(string));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("ApprovedDate", typeof(string));
				dtcol.Columns.Add("ApprovedUser", typeof(string));
				dtcol.Columns.Add("PdcStatus", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow row in dtGridRegister.Rows)
				{
					dtcol.Rows.Add(row["pdcTransactionId"], row["documentTypeId"], row["docNo"], row["docDate"], row["debitAccountId"], row["DebitAccountTitle"], row["creditAccountId"], row["CreditAccountTitle"], row["BankNameId"], row["bankName"], row["CheqBookDetailId"], row["cheqNo"], row["cheqDate"], row["payeeTitleId"], row["payeeTitle"], row["cheqTypeId"], row["cheqAmount"], row["seqNo"], row["remarks"], row["voucherId"], row["voucherCode"], row["voucherDate"], Conversion.ToBool(row["IsApproved"]) ? "Approved" : "Not Approved", row["EntryDate"], row["EntryUser"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(row["ModifyDate"])) ? "" : Conversion.ToDateTime(row["ModifyDate"]).ToShortTimeString(), row["ModifyUser"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(row["ApprovedDate"])) ? "" : Conversion.ToDateTime(row["ApprovedDate"]).ToShortTimeString(), row["ApprovedUser"], row["PdcStatus"], row["NoOfAttachments"]);
				}
				grdhistory.DataSource = dtcol;
				grdhistory.RetrieveStructure();
				GridHistoryMainSettings();
			}
			else
			{
				grdhistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridHistoryMainSettings()
	{
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdhistory.RootTable.Columns["DebitAccountId"].Visible = false;
			grdhistory.RootTable.Columns["CreditAccountId"].Visible = false;
			grdhistory.RootTable.Columns["BankNameId"].Visible = false;
			grdhistory.RootTable.Columns["ChequeBookDetailId"].Visible = false;
			grdhistory.RootTable.Columns["payeeTitleId"].Visible = false;
			grdhistory.RootTable.Columns["ChequeTypeId"].Visible = false;
			grdhistory.RootTable.Columns["voucherId"].Visible = false;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdhistory.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			grdhistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["DebitAccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grdhistory.RootTable.Columns["CreditAccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grdhistory.RootTable.Columns["BankName"].Width = Constants.InventoryConstants.BranchName;
			grdhistory.RootTable.Columns["ChequeNo"].Width = Constants.InventoryConstants.ContractNoConstant;
			grdhistory.RootTable.Columns["ChequeDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["payeeTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grdhistory.RootTable.Columns["ChequeAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdhistory.RootTable.Columns["seqNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdhistory.RootTable.Columns["voucherCode"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["voucherDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdhistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdhistory.RootTable.Columns["PdcStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 40;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 40;
			grdhistory.RootTable.Columns.Add("SaveAs");
			grdhistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].Width = 60;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns["SaveAs"].Position = 1;
			grdhistory.RootTable.Columns["Print"].Position = 2;
			grdhistory.RootTable.Columns.Remove("Confirm");
			if (rdPending.Checked)
			{
				grdhistory.RootTable.Columns.Add("Confirm");
				grdhistory.RootTable.Columns["Confirm"].Caption = "Confirm";
				grdhistory.RootTable.Columns["Confirm"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdhistory.RootTable.Columns["Confirm"].ButtonStyle = (ButtonStyle)5;
				grdhistory.RootTable.Columns["Confirm"].ButtonText = "Confirm";
				grdhistory.RootTable.Columns["Confirm"].Width = 60;
				grdhistory.RootTable.Columns["Confirm"].Position = 3;
			}
			grdhistory.FrozenColumns = 4;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					Reset();
					RecId = CurrId;
					ReadById(RecId);
				}
				else if (e.Column.Key == "Print")
				{
					CommonServices.PdcPaymentTransactionsSlip250(RecId);
				}
				else if (e.Column.Key == "SaveAs")
				{
					ReadbyIdForSaveAsRecord(CurrId);
				}
				else if (e.Column.Key == "Confirm")
				{
					string CreditAccount = Conversion.ToString(r.Cells["CreditAccountTitle"].Value);
					string DebitAccount = Conversion.ToString(r.Cells["DebitAccountTitle"].Value);
					pdcTransaction obj = pdcTransaction.GetByID(CurrId);
					MakeVoucher(obj, CreditAccount, DebitAccount);
					FillGridHistoryMain();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 250);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				RecId = CurrId;
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbChequeBankHistory).Text = string.Empty;
			((Control)(object)CmbCreditAccountHistory).Text = string.Empty;
			((Control)(object)CmbDebitAccountHistory).Text = string.Empty;
			grd.DataSource = null;
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			FillGridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FillGridHistory()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.DocumentTypeId = 250;
			reports.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			reports.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			reports.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			reports.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			reports.BankId = Conversion.ToInt(CmbChequeBankHistory.Value);
			reports.CreditAccountId = Conversion.ToInt(CmbCreditAccountHistory.Value);
			reports.DebitAccountId = Conversion.ToInt(CmbDebitAccountHistory.Value);
			DataTable dtGridRegister = pdcTransaction.FormHistory(reports);
			if (dtGridRegister.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(string));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DebitAccountId", typeof(int));
				dtcol.Columns.Add("DebitAccountTitle", typeof(string));
				dtcol.Columns.Add("CreditAccountId", typeof(int));
				dtcol.Columns.Add("CreditAccountTitle", typeof(string));
				dtcol.Columns.Add("BankNameId", typeof(int));
				dtcol.Columns.Add("BankName", typeof(string));
				dtcol.Columns.Add("ChequeBookDetailId", typeof(int));
				dtcol.Columns.Add("ChequeNo", typeof(string));
				dtcol.Columns.Add("ChequeDate", typeof(DateTime));
				dtcol.Columns.Add("payeeTitleId", typeof(int));
				dtcol.Columns.Add("payeeTitle", typeof(string));
				dtcol.Columns.Add("ChequeTypeId", typeof(int));
				dtcol.Columns.Add("ChequeAmount", typeof(decimal));
				dtcol.Columns.Add("seqNo", typeof(int));
				dtcol.Columns.Add("remarks", typeof(string));
				dtcol.Columns.Add("voucherId", typeof(int));
				dtcol.Columns.Add("voucherCode", typeof(string));
				dtcol.Columns.Add("voucherDate", typeof(DateTime));
				dtcol.Columns.Add("IsApproved", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(string));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("ApprovedDate", typeof(string));
				dtcol.Columns.Add("ApprovedUser", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow row in dtGridRegister.Rows)
				{
					dtcol.Rows.Add(row["pdcTransactionId"], row["documentTypeId"], row["docNo"], row["docDate"], row["debitAccountId"], row["DebitAccountTitle"], row["creditAccountId"], row["CreditAccountTitle"], row["BankNameId"], row["bankName"], row["CheqBookDetailId"], row["cheqNo"], row["cheqDate"], row["payeeTitleId"], row["payeeTitle"], row["cheqTypeId"], row["cheqAmount"], row["seqNo"], row["remarks"], row["voucherId"], row["voucherCode"], row["voucherDate"], Conversion.ToBool(row["IsApproved"]) ? "Approved" : "Not Approved", row["EntryDate"], row["EntryUser"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(row["ModifyDate"])) ? "" : Conversion.ToDateTime(row["ModifyDate"]).ToShortDateString(), row["ModifyUser"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(row["ApprovedDate"])) ? "" : Conversion.ToDateTime(row["ApprovedDate"]).ToShortDateString(), row["ApprovedUser"], row["NoOfAttachments"]);
				}
				grd.DataSource = dtcol;
				grd.RetrieveStructure();
				GridHistorySettings();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridHistorySettings()
	{
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["DebitAccountId"].Visible = false;
			grd.RootTable.Columns["CreditAccountId"].Visible = false;
			grd.RootTable.Columns["BankNameId"].Visible = false;
			grd.RootTable.Columns["ChequeBookDetailId"].Visible = false;
			grd.RootTable.Columns["payeeTitleId"].Visible = false;
			grd.RootTable.Columns["ChequeTypeId"].Visible = false;
			grd.RootTable.Columns["voucherId"].Visible = false;
			grd.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grd.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grd.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			grd.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["DebitAccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grd.RootTable.Columns["CreditAccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grd.RootTable.Columns["BankName"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns["ChequeNo"].Width = Constants.InventoryConstants.ContractNoConstant;
			grd.RootTable.Columns["ChequeDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["payeeTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grd.RootTable.Columns["ChequeAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["seqNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["voucherCode"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["voucherDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			grd.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grd.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grd.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grd.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 40;
			grd.RootTable.Columns.Add("Print");
			grd.RootTable.Columns["Print"].Caption = "Print";
			grd.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Print"].ButtonText = "Print";
			grd.RootTable.Columns["Print"].Width = 40;
			grd.RootTable.Columns.Add("SaveAs");
			grd.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			grd.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			grd.RootTable.Columns["SaveAs"].Width = 60;
			grd.RootTable.Columns["Edit"].Position = 0;
			grd.RootTable.Columns["SaveAs"].Position = 1;
			grd.RootTable.Columns["Print"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					Reset();
					RecId = CurrId;
					ReadById(RecId);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.PdcPaymentTransactionsSlip250(CurrId);
				}
				if (e.Column.Key == "SaveAs")
				{
					ReadbyIdForSaveAsRecord(CurrId);
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 250);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				RecId = CurrId;
				ReadById(RecId);
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
			if (!FormInitialized)
			{
				return;
			}
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs == null)
			{
				return;
			}
			string fileName = fs.Name;
			grd.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(fileName);
			int count = ((JanusCollectionBase)grd.RootTable.Columns).Count;
			int TableColumnsCount = dtHistory.Columns.Count + 1;
			if (count == TableColumnsCount)
			{
				return;
			}
			List<string> missingColumns = CommonServices.GetMissingColumns(dtHistory, xmlDoc);
			foreach (string column in missingColumns)
			{
				string d = CommonServices.GetDataTypeString(dtHistory.Columns[column].DataType);
				CommonServices.AddNewColumn(xmlDoc, fileName, column, d);
			}
			if (missingColumns.Count > 0)
			{
				FileStream fsNew = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
				grd.LoadLayoutFile((Stream)fsNew);
				fsNew.Close();
				fsNew.Dispose();
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
			if (!FormInitialized)
			{
				return;
			}
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
			if (fs == null)
			{
				return;
			}
			string fileName = fs.Name;
			grdhistory.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(fileName);
			int count = ((JanusCollectionBase)grdhistory.RootTable.Columns).Count;
			int TableColumnsCount = dtHistory.Columns.Count + 3;
			if (count == TableColumnsCount)
			{
				return;
			}
			List<string> missingColumns = CommonServices.GetMissingColumns(dtHistory, xmlDoc);
			foreach (string column in missingColumns)
			{
				string d = CommonServices.GetDataTypeString(dtHistory.Columns[column].DataType);
				CommonServices.AddNewColumn(xmlDoc, fileName, column, d);
			}
			if (missingColumns.Count > 0)
			{
				FileStream fsNew = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
				grdhistory.LoadLayoutFile((Stream)fsNew);
				fsNew.Close();
				fsNew.Dispose();
			}
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

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.PdcPaymentTransactionsSlip250(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbChequeNo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void FromDocNoHistoryMain_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ToDocNoHistoryMain_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtChequeAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRefPartyForm_Click(object sender, EventArgs e)
	{
		try
		{
			new frmpayeeTitle(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmPdcTransactionPayment_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtDocDateMain.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistoryMain.Focus();
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F1)
				{
					BtnRefPartyForm_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDocDateMain.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F12 && btnSaveAs.Visible && btnSaveAs.Enabled)
				{
					btnSaveAs_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtDocDateMain.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
				{
					btnprint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1))
				{
					if (formright.DoHavePrintRights)
					{
						btnprint_Click(null, null);
					}
				}
				else if (e.Control && e.Shift && e.KeyCode == Keys.Delete && btnDelete.Visible && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowMain_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnResetHistoryMain_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshHistoryMain_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistoryMain.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdhistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDateHistoryMain.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Save in Form Tab and For Show Records in History Tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print 273");
			dt.Rows.Add("Alt+1", "For Print 273");
			dt.Rows.Add("Ctrl+F1", "For Open Payee Title Form");
			dt.Rows.Add("Ctrl+F2", "For Open Bank Define Form");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+F12", "For SaveAs");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Edit Record");
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
			int CuurID = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				ReadById(CuurID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Edit")
				{
					Reset();
					ReadById(CuurID);
				}
				else if (grd.CurrentColumn.Key == "Print")
				{
					CommonServices.PdcPaymentTransactionsSlip250(CuurID);
				}
				else if (grd.CurrentColumn.Key == "SaveAs")
				{
					ReadbyIdForSaveAsRecord(CuurID);
				}
				else if (grd.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CuurID, 250);
				}
			}
			if (e.Control && e.KeyCode == Keys.F12)
			{
				ReadbyIdForSaveAsRecord(CuurID);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int CuurID = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				ReadById(CuurID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdhistory.CurrentColumn.Key == "Edit")
				{
					Reset();
					ReadById(CuurID);
				}
				else if (grdhistory.CurrentColumn.Key == "Print")
				{
					CommonServices.PdcPaymentTransactionsSlip250(CuurID);
				}
				else if (grdhistory.CurrentColumn.Key == "SaveAs")
				{
					ReadbyIdForSaveAsRecord(CuurID);
				}
				else if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CuurID, 250);
				}
			}
			if (e.Control && e.KeyCode == Keys.F12)
			{
				ReadbyIdForSaveAsRecord(CuurID);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GeneratingCard(string ChequeTemplate)
	{
		try
		{
			if (string.IsNullOrEmpty(ChequeTemplate))
			{
				FlPanelChequeTemplate.Controls.Clear();
				return;
			}
			Dictionary<string, Func<UserControl>> cardCreators = new Dictionary<string, Func<UserControl>>
			{
				{
					"UBL",
					() => new ChequePrint()
				},
				{
					"BankAlFalah",
					() => new BankAlFalah()
				},
				{
					"ABL",
					() => new ABL()
				},
				{
					"AskariBank",
					() => new AskariBank()
				},
				{
					"BankAlHabib",
					() => new BankAlHabib()
				},
				{
					"FaysalBank",
					() => new FaysalBank()
				},
				{
					"HabibMetro",
					() => new HabibMetro()
				},
				{
					"HBL",
					() => new HBL()
				},
				{
					"MCB",
					() => new MCB()
				},
				{
					"MeezanBank",
					() => new MeezanBank()
				},
				{
					"NBP",
					() => new NBP()
				},
				{
					"NIB",
					() => new NIB()
				},
				{
					"SaadiqStandardCharteredBank",
					() => new SaadiqStandardCharteredBank()
				},
				{
					"SilkBank",
					() => new SilkBank()
				},
				{
					"SoneriBank",
					() => new SoneriBank()
				}
			};
			if (FlPanelChequeTemplate.Controls.Count > 0)
			{
				if (FlPanelChequeTemplate.Controls[0].GetType().Name != ChequeTemplate)
				{
					FlPanelChequeTemplate.Controls.Clear();
					if (cardCreators.ContainsKey(ChequeTemplate))
					{
						UserControl card = cardCreators[ChequeTemplate]();
						SetChequeCardProperties(card, SetPadding: true);
						FlPanelChequeTemplate.Controls.Add(card);
					}
					else
					{
						FlPanelChequeTemplate.Controls.Clear();
					}
				}
				else
				{
					UserControl card2 = cardCreators[ChequeTemplate]();
					SetChequeCardProperties(card2, SetPadding: false);
				}
			}
			else
			{
				FlPanelChequeTemplate.Controls.Clear();
				if (cardCreators.ContainsKey(ChequeTemplate))
				{
					UserControl card3 = cardCreators[ChequeTemplate]();
					SetChequeCardProperties(card3, SetPadding: true);
					FlPanelChequeTemplate.Controls.Add(card3);
				}
				else
				{
					FlPanelChequeTemplate.Controls.Clear();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SetChequeCardProperties(UserControl card, bool SetPadding)
	{
		try
		{
			if (SetPadding)
			{
				card.Margin = new Padding(15, 5, 5, 5);
			}
			((dynamic)card).Id = 0;
			((dynamic)card).ChequeTypeId = Conversion.ToInt(CmbChequeType.Value);
			((dynamic)card).CheqType = Conversion.ToString((object)((Control)(object)CmbChequeType).Text);
			((dynamic)card).ChequeNumber = Conversion.ToString((object)((Control)(object)CmbChequeNo).Text);
			((dynamic)card).PayeeTitle = Conversion.ToString((object)((Control)(object)CmbPayeeTitle).Text);
			double chequeAmount = Conversion.ToDouble((object)txtChequeAmount.Text);
			((dynamic)card).Amount = chequeAmount.ToString("##,#.####");
			((dynamic)card).AmountInWords = ConvertAmountToWords(chequeAmount);
			DateTime chequeDate = Conversion.ToDateTime((object)txtChequeDate.Value);
			SetDateProperties((dynamic)card, chequeDate);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SetDateProperties(dynamic card, DateTime CheqDate)
	{
		string day = CheqDate.Day.ToString("00");
		string month = CheqDate.Month.ToString("00");
		string year = CheqDate.Year.ToString();
		card.D1 = day[0].ToString();
		card.D2 = day[1].ToString();
		card.M1 = month[0].ToString();
		card.M2 = month[1].ToString();
		card.Y1 = year[0].ToString();
		card.Y2 = year[1].ToString();
		card.Y3 = year[2].ToString();
		card.Y4 = year[3].ToString();
	}

	private void txtChequeDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime CheqDate = Conversion.ToDateTime((object)txtChequeDate.Value);
			if (FlPanelChequeTemplate.Controls.Count > 0)
			{
				dynamic dynamicCard = FlPanelChequeTemplate.Controls[0];
				SetDateProperties(dynamicCard, CheqDate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private string ConvertAmountToWords(double amount)
	{
		if (amount == 0.0)
		{
			return "Zero";
		}
		long integerPart = (long)Math.Floor(amount);
		long decimalPart = (long)((amount - (double)integerPart) * 100.0);
		string obj = NumberToWords(integerPart);
		string decimalPartWords = ((decimalPart > 0) ? (" and " + NumberToWords(decimalPart) + " Paisa") : "");
		return (obj + " Rupees" + decimalPartWords).Trim();
	}

	private string NumberToWords(long number)
	{
		if (number == 0L)
		{
			return "Zero";
		}
		string[] unitsMap = new string[20]
		{
			"Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",
			"Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
		};
		string[] tensMap = new string[10] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
		if (number < 20)
		{
			return unitsMap[number];
		}
		if (number < 100)
		{
			return tensMap[number / 10] + ((number % 10 > 0) ? (" " + unitsMap[number % 10]) : "");
		}
		if (number < 1000)
		{
			return unitsMap[number / 100] + " Hundred" + ((number % 100 > 0) ? (" and " + NumberToWords(number % 100)) : "");
		}
		if (number < 1000000)
		{
			return NumberToWords(number / 1000) + " Thousand" + ((number % 1000 > 0) ? (" " + NumberToWords(number % 1000)) : "");
		}
		if (number < 1000000000)
		{
			return NumberToWords(number / 1000000) + " Million" + ((number % 1000000 > 0) ? (" " + NumberToWords(number % 1000000)) : "");
		}
		return NumberToWords(number / 1000000000) + " Billion" + ((number % 1000000000 > 0) ? (" " + NumberToWords(number % 1000000000)) : "");
	}

	private void CmbPayeeTitle_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (FlPanelChequeTemplate.Controls.Count > 0)
			{
				dynamic dynamicCard = FlPanelChequeTemplate.Controls[0];
				dynamicCard.PayeeTitle = Conversion.ToString((object)((Control)(object)CmbPayeeTitle).Text);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbChequeType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (FlPanelChequeTemplate.Controls.Count > 0)
			{
				dynamic dynamicCard = FlPanelChequeTemplate.Controls[0];
				dynamicCard.ChequeTypeId = Conversion.ToInt(CmbChequeType.Value);
				dynamicCard.CheqType = Conversion.ToString((object)((Control)(object)CmbChequeType).Text);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtChequeAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (FlPanelChequeTemplate.Controls.Count > 0)
			{
				dynamic dynamicCard = FlPanelChequeTemplate.Controls[0];
				double chequeAmount = Conversion.ToDouble((object)txtChequeAmount.Text);
				dynamicCard.Amount = chequeAmount.ToString("##,#.####");
				dynamicCard.AmountInWords = ConvertAmountToWords(chequeAmount);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void CmbChequeNo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (FlPanelChequeTemplate.Controls.Count > 0)
			{
				dynamic dynamicCard = FlPanelChequeTemplate.Controls[0];
				dynamicCard.ChequeNumber = Conversion.ToString((object)((Control)(object)CmbChequeNo).Text);
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
			DetailAccountCurrentBalance(Conversion.ToInt(CmbDebitAccount.Value), txtDrBalance);
			payeeTitleFill(Conversion.ToInt(CmbDebitAccount.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeVoucher(pdcTransaction obj, string CreditAccount, string DebitAccount)
	{
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Expected O, but got Unknown
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Expected O, but got Unknown
		try
		{
			int CurrencyId = 0;
			double BaseCurrencyRate = 0.0;
			ConfigrationsAllocation BaseCurrency = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency");
			if (BaseCurrency != null)
			{
				CurrencyId = Conversion.ToInt((object)BaseCurrency.ConfigKey);
			}
			ConfigrationsAllocation BaseRate = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate");
			if (BaseRate != null)
			{
				BaseCurrencyRate = Conversion.ToDouble((object)BaseRate.ConfigKey);
			}
			bool AutoRemarksForPaymentThroughBank = clsGlobalVariables.configrationsAllocation.Any((ConfigrationsAllocation x) => x.ConfigDescription == "AutoRemarksForPaymentThroughBank" && Conversion.ToBool((object)x.ConfigKey));
			clsGlobalVariables.configrationsAllocation.Any((ConfigrationsAllocation x) => x.ConfigDescription == "AutoRemarksIncludeHeaderRemarks" && Conversion.ToBool((object)x.ConfigKey));
			bool AutoRemarksIncludeChqDateAndNum = clsGlobalVariables.configrationsAllocation.Any((ConfigrationsAllocation x) => x.ConfigDescription == "AutoRemarksIncludeChequeDateAndNumber" && Conversion.ToBool((object)x.ConfigKey));
			bool AutoRemarksIncludeChqNumber = clsGlobalVariables.configrationsAllocation.Any((ConfigrationsAllocation x) => x.ConfigDescription == "AutoRemarksIncludeChequeNumber" && Conversion.ToBool((object)x.ConfigKey));
			bool AutoRemarksIncludePayeeTitle = clsGlobalVariables.configrationsAllocation.Any((ConfigrationsAllocation x) => x.ConfigDescription == "AutoRemarksIncludePayeeTitle" && Conversion.ToBool((object)x.ConfigKey));
			clsGlobalVariables.configrationsAllocation.Any((ConfigrationsAllocation x) => x.ConfigDescription == "AutoRemarksIncludeDetailRemarks" && Conversion.ToBool((object)x.ConfigKey));
			VoucherHead vh = new VoucherHead();
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				vh.Type = (VoucherType)0;
				vh.DocumentTypeId = 2;
				vh.ProjectId = 0;
				vh.RefDocumentTypeId = obj.documentTypeId;
				vh.RefDocNoId = Conversion.ToInt((object)obj.pdcTransactionId);
				vh.VoucherCode = obj.docNo;
				vh.VoucherDate = obj.cheqDate;
				vh.Remarks = Conversion.ToString((object)obj.remarks);
				vh.RefAccountId = obj.creditAccountId;
				vh.AgainstAccountId = obj.debitAccountId;
				vh.PayTitle = obj.payeeTitle;
				vh.CheqId = obj.CheqBookDetailId;
				vh.ChequeNo = obj.cheqNo;
				vh.ChequeDate = obj.cheqDate;
				vh.IncludeWHT = false;
				vh.BranchId = UserAccount.BranchesId;
				vh.OrganizationId = UserAccount.OrganizationId;
				vh.CompanyId = UserAccount.CompanyId;
				vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				vh.EntryUser = UserAccount.ID;
				vh.ModifyUser = UserAccount.ID;
				vh.VoucherAmount = obj.cheqAmount;
				vh.MultiCurrencyId = Conversion.ToInt((object)CurrencyId);
				vh.ExchangeCurrencyRate = Conversion.ToDouble((object)BaseCurrencyRate);
				vh.FcAmount = Conversion.ToDouble((object)vh.VoucherAmount);
				vh.voucherDetailList = new List<VoucherDetail>();
				vh.AttachmentsList = new List<DMSAttachments>();
				int LineId = 1;
				string RemarksAuto = "";
				string Remarks = "";
				VoucherDetail vd = new VoucherDetail();
				if (AutoRemarksForPaymentThroughBank)
				{
					RemarksAuto = (vd.CommentsOtherLingo = (vd.Comments = ((AutoRemarksIncludeChqDateAndNum && AutoRemarksIncludePayeeTitle) ? (" CHEQUE DATE: " + Conversion.ToDateTime((object)vh.ChequeDate).ToShortDateString() + " CHEQUE NO: " + Conversion.ToString((object)vh.ChequeNo) + " PayTitle: " + Conversion.ToString((object)vh.PayTitle) + " Credit Account: " + CreditAccount + " PAID TO: " + DebitAccount) : (AutoRemarksIncludeChqDateAndNum ? (" CHEQUE DATE: " + Conversion.ToDateTime((object)vh.ChequeDate).ToShortDateString() + " CHEQUE NO: " + Conversion.ToString((object)vh.ChequeNo) + " Credit Account: " + CreditAccount + " PAID TO: " + DebitAccount) : ((AutoRemarksIncludeChqNumber && AutoRemarksIncludePayeeTitle) ? (" CHEQUE NO: " + Conversion.ToString((object)vh.ChequeNo) + " PayTitle: " + Conversion.ToString((object)vh.PayTitle) + " Credit Account: " + CreditAccount + " PAID TO: " + DebitAccount) : ((!AutoRemarksIncludePayeeTitle) ? (" Credit Account: " + CreditAccount + " PAID TO: " + DebitAccount) : (" PayTitle: " + vh.PayTitle + " Credit Account: " + CreditAccount + " PAID TO: " + DebitAccount)))))));
					vh.RemarksOtherLingo = ((vh.Remarks != "") ? vh.Remarks : "");
					vh.Remarks = RemarksAuto;
				}
				else if (vh.ChequeNo != string.Empty)
				{
					Remarks = "Cheq/Ref#: " + vh.ChequeNo + ", " + vh.Remarks;
					vd.Comments = Remarks;
					vd.CommentsOtherLingo = vh.Remarks;
				}
				else
				{
					vd.Comments = vh.Remarks;
					vd.CommentsOtherLingo = vh.Remarks;
				}
				vd.AdvanceAmount = 0.0;
				vd.LineId = LineId;
				vd.PaymentType = "Regular";
				vd.AccountId = obj.debitAccountId;
				vd.AccountTitle = DebitAccount;
				vd.AgainstAccountId = obj.creditAccountId;
				vd.JobLotId = 0;
				vd.DebitAmount = obj.cheqAmount;
				vd.IsTaxable = "False";
				vd.InvoiceNoRefId = Conversion.ToInt((object)obj.CheqBookDetailId);
				vd.InstrumentTypeId = 1;
				vd.CheqNoDetail = vh.ChequeNo;
				vd.DCheqDate = Conversion.ToDateTime((object)obj.cheqDate);
				vd.PayeeTitle = Conversion.ToString((object)obj.payeeTitle);
				vd.ChequeTypeId = 1;
				vd.BranchesId = UserAccount.BranchesId;
				vd.DMultiCurrencyId = CurrencyId;
				vd.DExchangeCurrencyRate = Conversion.ToDouble((object)BaseCurrencyRate);
				vd.DCurrencyAmount = Conversion.ToDouble((object)obj.cheqAmount);
				if (vh.Remarks.Trim() == string.Empty)
				{
					vh.RemarksOtherLingo = ((vh.Remarks.Trim() != "") ? vh.Remarks.Trim() : null);
					vh.Remarks = vd.Comments;
				}
				vh.voucherDetailList.Add(vd);
				LineId++;
				VoucherDetail vd2 = new VoucherDetail();
				if (AutoRemarksForPaymentThroughBank)
				{
					vd2.Comments = RemarksAuto;
					vd2.CommentsOtherLingo = vh.Remarks;
				}
				else if (vh.Remarks.Trim() != null && vh.Remarks.Trim() != string.Empty)
				{
					vd2.Comments = vh.Remarks.Trim();
					vd2.CommentsOtherLingo = vd.CommentsOtherLingo;
				}
				else
				{
					vd2.Comments = vd.Comments;
					vd2.CommentsOtherLingo = vh.Remarks;
				}
				vd2.AdvanceAmount = 0.0;
				vd2.LineId = LineId;
				vd2.PaymentType = vd.PaymentType;
				vd2.AccountId = Conversion.ToInt((object)obj.creditAccountId);
				vd2.AccountTitle = Conversion.ToString((object)CreditAccount);
				vd2.AgainstAccountId = Conversion.ToInt((object)obj.debitAccountId);
				vd2.JobLotId = 0;
				vd2.CreditAmount = vd.DebitAmount;
				vd2.DebitAmount = 0.0;
				vd2.IsTaxable = "False";
				vd2.InvoiceNoRefId = vd.InvoiceNoRefId;
				vd2.InstrumentTypeId = vd.InstrumentTypeId;
				vd2.ChequeTypeId = vd.ChequeTypeId;
				vd2.BranchesId = UserAccount.BranchesId;
				vd2.CheqNoDetail = vd.CheqNoDetail;
				vd2.DCheqDate = vd.DCheqDate;
				vd2.PayeeTitle = vd.PayeeTitle;
				vd2.DMultiCurrencyId = vd.DMultiCurrencyId;
				vd2.DExchangeCurrencyRate = vd.DExchangeCurrencyRate;
				vd2.DCurrencyAmount = vd.DCurrencyAmount;
				vh.voucherDetailList.Add(vd2);
				VoucherHead.Save(vh);
				MessageBox.Show("Record save successfully....");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Expected O, but got Unknown
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Expected O, but got Unknown
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Expected O, but got Unknown
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Expected O, but got Unknown
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a7: Expected O, but got Unknown
		//IL_082c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0836: Expected O, but got Unknown
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_084c: Expected O, but got Unknown
		//IL_0858: Unknown result type (might be due to invalid IL or missing references)
		//IL_0862: Expected O, but got Unknown
		//IL_0dbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc6: Expected O, but got Unknown
		//IL_0dd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddd: Expected O, but got Unknown
		//IL_5f71: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f7b: Expected O, but got Unknown
		//IL_5f88: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f92: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance35 = new Appearance();
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
		Appearance appearance78 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
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
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Bank.frmPdcTransactionPayment));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbDebitAccountHistory = new UltraCombo();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.CmbCreditAccountHistory = new UltraCombo();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.CmbChequeBankHistory = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label29 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.label30 = new System.Windows.Forms.Label();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.BtnNewHistory = new System.Windows.Forms.Button();
		this.btnRefreshHistory = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.FlPanelChequeTemplate = new System.Windows.Forms.FlowLayoutPanel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbDebitAccount = new UltraCombo();
		this.CmbCreditAccount = new UltraCombo();
		this.CmbChequeBank = new UltraCombo();
		this.CmbPayeeTitle = new UltraCombo();
		this.CmbChequeType = new UltraCombo();
		this.CmbChequeNo = new UltraCombo();
		this.txtChequeDate = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtDocDateMain = new System.Windows.Forms.DateTimePicker();
		this.txtChequeAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtRemarksMain = new System.Windows.Forms.TextBox();
		this.txtDrBalance = new System.Windows.Forms.TextBox();
		this.txtCrBalance = new System.Windows.Forms.TextBox();
		this.txtDocNoMain = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.cbPrintOnSave = new System.Windows.Forms.CheckBox();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.BtnRefPartyForm = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label55 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.rdComplete = new System.Windows.Forms.RadioButton();
		this.rdPending = new System.Windows.Forms.RadioButton();
		this.label5 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.ToDateHistoryMain = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistoryMain = new System.Windows.Forms.DateTimePicker();
		this.CmbDebitAccountHistoryMain = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbCreditAccountHistoryMain = new UltraCombo();
		this.rdapproveddateMain = new System.Windows.Forms.RadioButton();
		this.CmbChequeBankHistoryMain = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.rdmodifydateMain = new System.Windows.Forms.RadioButton();
		this.drdocdateMain = new System.Windows.Forms.RadioButton();
		this.label22 = new System.Windows.Forms.Label();
		this.rdentrydateMain = new System.Windows.Forms.RadioButton();
		this.btnShowMain = new System.Windows.Forms.Button();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.ToDocNoHistoryMain = new System.Windows.Forms.TextBox();
		this.FromDocNoHistoryMain = new System.Windows.Forms.TextBox();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistoryMain = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistoryMain = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccountHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccountHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeBankHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPayeeTitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeNo).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel14.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccountHistoryMain).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccountHistoryMain).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeBankHistoryMain).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1144, 561);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1144, 561);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1136, 532);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(2, 395);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1132, 135);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel6.Controls.Add(this.groupBox3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(2, 323);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1132, 72);
		this.panel6.TabIndex = 0;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistory);
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChequeBankHistory);
		this.groupBox3.Controls.Add(this.label28);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.label29);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.label30);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnShow);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Controls.Add(this.label13);
		this.groupBox3.Controls.Add(this.label15);
		this.groupBox3.Controls.Add(this.ToDocNoHistory);
		this.groupBox3.Controls.Add(this.FromDocNoHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1114, 70);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.CmbDebitAccountHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAccountHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAccountHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAccountHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAccountHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistory).Location = new System.Drawing.Point(699, 14);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistory).Name = "CmbDebitAccountHistory";
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistory).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistory).TabIndex = 6;
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(68, 43);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(137, 23);
		this.ToDateHistory.TabIndex = 1;
		this.CmbCreditAccountHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccountHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccountHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccountHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccountHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAccountHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistory).Location = new System.Drawing.Point(427, 41);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistory).Name = "CmbCreditAccountHistory";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistory).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistory).TabIndex = 5;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1010, 45);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 11;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.CmbChequeBankHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbChequeBankHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChequeBankHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChequeBankHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChequeBankHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChequeBankHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChequeBankHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChequeBankHistory).Location = new System.Drawing.Point(427, 14);
		((System.Windows.Forms.Control)(object)this.CmbChequeBankHistory).Name = "CmbChequeBankHistory";
		((System.Windows.Forms.Control)(object)this.CmbChequeBankHistory).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbChequeBankHistory).TabIndex = 4;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(350, 20);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(76, 15);
		this.label28.TabIndex = 222;
		this.label28.Text = "Cheque Bank";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1010, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 10;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(639, 20);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(58, 15);
		this.label29.TabIndex = 223;
		this.label29.Text = "Debit A/c";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(935, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 8;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(350, 47);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(60, 15);
		this.label30.TabIndex = 224;
		this.label30.Text = "Credit A/c";
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(935, 45);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 9;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(870, 42);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 25);
		this.btnShow.TabIndex = 7;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(208, 47);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(45, 15);
		this.label2.TabIndex = 9;
		this.label2.Text = "To Doc";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(208, 20);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 7;
		this.label12.Text = "From Doc";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(4, 47);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 5;
		this.label13.Text = "To Date";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(4, 20);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(63, 15);
		this.label15.TabIndex = 3;
		this.label15.Text = "From Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(269, 43);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(269, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(68, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label16);
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Controls.Add(this.BtnNewHistory);
		this.panel10.Controls.Add(this.btnRefreshHistory);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 298);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1132, 25);
		this.panel10.TabIndex = 2;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(2, 2);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(67, 19);
		this.label16.TabIndex = 0;
		this.label16.Text = "History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1091, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.BtnNewHistory.BackColor = System.Drawing.Color.White;
		this.BtnNewHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.ForeColor = System.Drawing.Color.White;
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.Location = new System.Drawing.Point(82, 2);
		this.BtnNewHistory.Margin = new System.Windows.Forms.Padding(2);
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(26, 22);
		this.BtnNewHistory.TabIndex = 7;
		this.BtnNewHistory.Text = " ";
		this.BtnNewHistory.UseVisualStyleBackColor = false;
		this.BtnNewHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.BackColor = System.Drawing.Color.White;
		this.btnRefreshHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.ForeColor = System.Drawing.Color.White;
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.Location = new System.Drawing.Point(111, 2);
		this.btnRefreshHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(30, 22);
		this.btnRefreshHistory.TabIndex = 7;
		this.btnRefreshHistory.UseVisualStyleBackColor = false;
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.FlPanelChequeTemplate);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 53);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1132, 245);
		this.panel4.TabIndex = 0;
		this.FlPanelChequeTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.FlPanelChequeTemplate.Location = new System.Drawing.Point(587, 0);
		this.FlPanelChequeTemplate.Name = "FlPanelChequeTemplate";
		this.FlPanelChequeTemplate.Size = new System.Drawing.Size(545, 245);
		this.FlPanelChequeTemplate.TabIndex = 1;
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAccount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChequeBank);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPayeeTitle);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChequeType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChequeNo);
		this.groupBox1.Controls.Add(this.txtChequeDate);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtDocDateMain);
		this.groupBox1.Controls.Add(this.txtChequeAmount);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.txtRemarksMain);
		this.groupBox1.Controls.Add(this.txtDrBalance);
		this.groupBox1.Controls.Add(this.txtCrBalance);
		this.groupBox1.Controls.Add(this.txtDocNoMain);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(587, 245);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Red;
		this.label10.Location = new System.Drawing.Point(74, 101);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(14, 17);
		this.label10.TabIndex = 218;
		this.label10.Text = "*";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Red;
		this.label7.Location = new System.Drawing.Point(74, 73);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(14, 17);
		this.label7.TabIndex = 218;
		this.label7.Text = "*";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.Red;
		this.label11.Location = new System.Drawing.Point(74, 17);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(14, 17);
		this.label11.TabIndex = 218;
		this.label11.Text = "*";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Red;
		this.label3.Location = new System.Drawing.Point(74, 45);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(14, 17);
		this.label3.TabIndex = 218;
		this.label3.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(434, 73);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 218;
		this.label19.Text = "*";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Red;
		this.label25.Location = new System.Drawing.Point(74, 129);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(14, 17);
		this.label25.TabIndex = 218;
		this.label25.Text = "*";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Red;
		this.label27.Location = new System.Drawing.Point(434, 101);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(14, 17);
		this.label27.TabIndex = 218;
		this.label27.Text = "*";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Red;
		this.label9.Location = new System.Drawing.Point(433, 17);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(14, 17);
		this.label9.TabIndex = 218;
		this.label9.Text = "*";
		this.CmbDebitAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance84).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Location = new System.Drawing.Point(88, 96);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Name = "CmbDebitAccount";
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Size = new System.Drawing.Size(197, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccount).Leave += new System.EventHandler(CmbDebitAccount_Leave);
		this.CmbCreditAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance115).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Location = new System.Drawing.Point(88, 68);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Name = "CmbCreditAccount";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Size = new System.Drawing.Size(197, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Leave += new System.EventHandler(CmbCreditAccount_Leave);
		this.CmbChequeBank.AllowNull = (DefaultableBoolean)1;
		this.CmbChequeBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChequeBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChequeBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChequeBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChequeBank).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChequeBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChequeBank).Location = new System.Drawing.Point(88, 40);
		((System.Windows.Forms.Control)(object)this.CmbChequeBank).Name = "CmbChequeBank";
		((System.Windows.Forms.Control)(object)this.CmbChequeBank).Size = new System.Drawing.Size(273, 26);
		((System.Windows.Forms.Control)(object)this.CmbChequeBank).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbChequeBank).Leave += new System.EventHandler(CmbChequeBank_Leave);
		this.CmbPayeeTitle.AllowNull = (DefaultableBoolean)1;
		this.CmbPayeeTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPayeeTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPayeeTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPayeeTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPayeeTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbPayeeTitle).Location = new System.Drawing.Point(88, 124);
		((System.Windows.Forms.Control)(object)this.CmbPayeeTitle).Name = "CmbPayeeTitle";
		((System.Windows.Forms.Control)(object)this.CmbPayeeTitle).Size = new System.Drawing.Size(495, 26);
		((System.Windows.Forms.Control)(object)this.CmbPayeeTitle).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbPayeeTitle).TextChanged += new System.EventHandler(CmbPayeeTitle_TextChanged);
		this.CmbChequeType.AllowNull = (DefaultableBoolean)1;
		this.CmbChequeType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChequeType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChequeType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance110;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChequeType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChequeType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChequeType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChequeType).Location = new System.Drawing.Point(450, 96);
		((System.Windows.Forms.Control)(object)this.CmbChequeType).Name = "CmbChequeType";
		((System.Windows.Forms.Control)(object)this.CmbChequeType).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.CmbChequeType).TabIndex = 10;
		this.CmbChequeType.ValueChanged += new System.EventHandler(CmbChequeType_ValueChanged);
		this.CmbChequeNo.AllowNull = (DefaultableBoolean)1;
		this.CmbChequeNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChequeNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChequeNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance100).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChequeNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Location = new System.Drawing.Point(450, 12);
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Name = "CmbChequeNo";
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).TextChanged += new System.EventHandler(CmbChequeNo_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).KeyPress += new System.Windows.Forms.KeyPressEventHandler(CmbChequeNo_KeyPress);
		this.txtChequeDate.CustomFormat = "dd-MMM-yy";
		this.txtChequeDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtChequeDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtChequeDate.Location = new System.Drawing.Point(450, 42);
		this.txtChequeDate.Name = "txtChequeDate";
		this.txtChequeDate.Size = new System.Drawing.Size(133, 23);
		this.txtChequeDate.TabIndex = 8;
		this.txtChequeDate.ValueChanged += new System.EventHandler(txtChequeDate_ValueChanged);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(1, 102);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(58, 15);
		this.label8.TabIndex = 34;
		this.label8.Text = "Debit A/c";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(1, 74);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(60, 15);
		this.label4.TabIndex = 34;
		this.label4.Text = "Credit A/c";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(1, 46);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(76, 15);
		this.label1.TabIndex = 34;
		this.label1.Text = "Cheque Bank";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(1, 130);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(64, 15);
		this.label20.TabIndex = 34;
		this.label20.Text = "Payee Title";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(362, 74);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(51, 15);
		this.label18.TabIndex = 34;
		this.label18.Text = "Amount";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(362, 102);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(75, 15);
		this.label26.TabIndex = 34;
		this.label26.Text = "Cheque Type";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(362, 18);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(66, 15);
		this.label6.TabIndex = 34;
		this.label6.Text = "Cheque No";
		this.txtDocDateMain.CustomFormat = "dd-MMM-yy";
		this.txtDocDateMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocDateMain.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDateMain.Location = new System.Drawing.Point(210, 14);
		this.txtDocDateMain.Name = "txtDocDateMain";
		this.txtDocDateMain.Size = new System.Drawing.Size(151, 23);
		this.txtDocDateMain.TabIndex = 1;
		this.txtDocDateMain.TabStop = false;
		this.txtChequeAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtChequeAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtChequeAmount.Location = new System.Drawing.Point(450, 70);
		this.txtChequeAmount.Name = "txtChequeAmount";
		this.txtChequeAmount.Size = new System.Drawing.Size(133, 23);
		this.txtChequeAmount.TabIndex = 9;
		this.txtChequeAmount.TextChanged += new System.EventHandler(txtChequeAmount_TextChanged);
		this.txtChequeAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtChequeAmount_KeyPress);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(362, 46);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(75, 15);
		this.label14.TabIndex = 3;
		this.label14.Text = "Cheque Date";
		this.txtRemarksMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarksMain.Location = new System.Drawing.Point(88, 152);
		this.txtRemarksMain.Multiline = true;
		this.txtRemarksMain.Name = "txtRemarksMain";
		this.txtRemarksMain.Size = new System.Drawing.Size(495, 69);
		this.txtRemarksMain.TabIndex = 6;
		this.txtDrBalance.BackColor = System.Drawing.Color.White;
		this.txtDrBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDrBalance.Enabled = false;
		this.txtDrBalance.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDrBalance.Location = new System.Drawing.Point(286, 99);
		this.txtDrBalance.Name = "txtDrBalance";
		this.txtDrBalance.Size = new System.Drawing.Size(75, 21);
		this.txtDrBalance.TabIndex = 0;
		this.txtDrBalance.TabStop = false;
		this.txtDrBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCrBalance.BackColor = System.Drawing.Color.White;
		this.txtCrBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCrBalance.Enabled = false;
		this.txtCrBalance.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCrBalance.Location = new System.Drawing.Point(286, 71);
		this.txtCrBalance.Name = "txtCrBalance";
		this.txtCrBalance.Size = new System.Drawing.Size(75, 21);
		this.txtCrBalance.TabIndex = 0;
		this.txtCrBalance.TabStop = false;
		this.txtCrBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDocNoMain.BackColor = System.Drawing.Color.White;
		this.txtDocNoMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoMain.Enabled = false;
		this.txtDocNoMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocNoMain.Location = new System.Drawing.Point(88, 14);
		this.txtDocNoMain.Name = "txtDocNoMain";
		this.txtDocNoMain.Size = new System.Drawing.Size(120, 23);
		this.txtDocNoMain.TabIndex = 0;
		this.txtDocNoMain.TabStop = false;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(1, 18);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(76, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No,Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(1, 156);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(52, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.cbPrintOnSave);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1132, 26);
		this.panel3.TabIndex = 0;
		this.cbPrintOnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.cbPrintOnSave.AutoSize = true;
		this.cbPrintOnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cbPrintOnSave.ForeColor = System.Drawing.Color.White;
		this.cbPrintOnSave.Location = new System.Drawing.Point(822, 3);
		this.cbPrintOnSave.Name = "cbPrintOnSave";
		this.cbPrintOnSave.Size = new System.Drawing.Size(183, 20);
		this.cbPrintOnSave.TabIndex = 44;
		this.cbPrintOnSave.Text = "Cheque Print On Save";
		this.cbPrintOnSave.UseVisualStyleBackColor = true;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(120, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "PDC Payment";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1008, 3);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(122, 20);
		this.ChkBox.TabIndex = 0;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1132, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.btnRefresh, this.btnsave, this.btnattachment, this.btnSaveAs, this.btnupdate, this.btnDelete, this.btnprint, this.BtnShortCutkeys, this.BtnRefPartyForm });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1132, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "250-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.BtnRefPartyForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefPartyForm.Image = Architecture.WinApp.Properties.Resources.Export_Reports;
		this.BtnRefPartyForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefPartyForm.Name = "BtnRefPartyForm";
		this.BtnRefPartyForm.Size = new System.Drawing.Size(105, 24);
		this.BtnRefPartyForm.Text = "Payee Title";
		this.BtnRefPartyForm.Click += new System.EventHandler(BtnRefPartyForm_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1136, 532);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel12);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1132, 528);
		this.panel7.TabIndex = 0;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 153);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1132, 375);
		this.panel12.TabIndex = 47;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1132, 375);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label55);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 131);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1132, 22);
		this.panel9.TabIndex = 47;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(1, 1);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(132, 18);
		this.label55.TabIndex = 0;
		this.label55.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1087, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel8.Controls.Add(this.panel14);
		this.panel8.Controls.Add(this.groupBox2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 27);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1132, 104);
		this.panel8.TabIndex = 0;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.rdAll);
		this.panel14.Controls.Add(this.rdComplete);
		this.panel14.Controls.Add(this.rdPending);
		this.panel14.Controls.Add(this.label5);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1132, 25);
		this.panel14.TabIndex = 50;
		this.rdAll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdAll.AutoSize = true;
		this.rdAll.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAll.ForeColor = System.Drawing.Color.White;
		this.rdAll.Location = new System.Drawing.Point(1080, 2);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(43, 21);
		this.rdAll.TabIndex = 57;
		this.rdAll.Text = "All";
		this.rdAll.UseVisualStyleBackColor = true;
		this.rdComplete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdComplete.AutoSize = true;
		this.rdComplete.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdComplete.ForeColor = System.Drawing.Color.White;
		this.rdComplete.Location = new System.Drawing.Point(990, 2);
		this.rdComplete.Name = "rdComplete";
		this.rdComplete.Size = new System.Drawing.Size(85, 21);
		this.rdComplete.TabIndex = 57;
		this.rdComplete.Text = "Complete";
		this.rdComplete.UseVisualStyleBackColor = true;
		this.rdPending.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdPending.AutoSize = true;
		this.rdPending.Checked = true;
		this.rdPending.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdPending.ForeColor = System.Drawing.Color.White;
		this.rdPending.Location = new System.Drawing.Point(910, 2);
		this.rdPending.Name = "rdPending";
		this.rdPending.Size = new System.Drawing.Size(77, 21);
		this.rdPending.TabIndex = 57;
		this.rdPending.TabStop = true;
		this.rdPending.Text = "Pending";
		this.rdPending.UseVisualStyleBackColor = true;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(3, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(183, 19);
		this.label5.TabIndex = 0;
		this.label5.Text = "PDC Payment History";
		this.groupBox2.Controls.Add(this.ToDateHistoryMain);
		this.groupBox2.Controls.Add(this.FromDateHistoryMain);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistoryMain);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistoryMain);
		this.groupBox2.Controls.Add(this.rdapproveddateMain);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChequeBankHistoryMain);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.rdmodifydateMain);
		this.groupBox2.Controls.Add(this.drdocdateMain);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.rdentrydateMain);
		this.groupBox2.Controls.Add(this.btnShowMain);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.ToDocNoHistoryMain);
		this.groupBox2.Controls.Add(this.FromDocNoHistoryMain);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 25);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1122, 74);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.ToDateHistoryMain.CustomFormat = "dd-MMM-yy";
		this.ToDateHistoryMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistoryMain.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistoryMain.Location = new System.Drawing.Point(68, 44);
		this.ToDateHistoryMain.Name = "ToDateHistoryMain";
		this.ToDateHistoryMain.ShowCheckBox = true;
		this.ToDateHistoryMain.Size = new System.Drawing.Size(141, 23);
		this.ToDateHistoryMain.TabIndex = 1;
		this.FromDateHistoryMain.CustomFormat = "dd-MMM-yy";
		this.FromDateHistoryMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistoryMain.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistoryMain.Location = new System.Drawing.Point(68, 16);
		this.FromDateHistoryMain.Name = "FromDateHistoryMain";
		this.FromDateHistoryMain.ShowCheckBox = true;
		this.FromDateHistoryMain.Size = new System.Drawing.Size(141, 23);
		this.FromDateHistoryMain.TabIndex = 0;
		this.CmbDebitAccountHistoryMain.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAccountHistoryMain.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAccountHistoryMain.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAccountHistoryMain.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAccountHistoryMain).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistoryMain).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAccountHistoryMain.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistoryMain).Location = new System.Drawing.Point(700, 14);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistoryMain).Name = "CmbDebitAccountHistoryMain";
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistoryMain).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistoryMain).TabIndex = 6;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(641, 20);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(58, 15);
		this.label24.TabIndex = 55;
		this.label24.Text = "Debit A/c";
		this.CmbCreditAccountHistoryMain.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccountHistoryMain.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccountHistoryMain.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccountHistoryMain.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccountHistoryMain).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistoryMain).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAccountHistoryMain.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistoryMain).Location = new System.Drawing.Point(431, 42);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistoryMain).Name = "CmbCreditAccountHistoryMain";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistoryMain).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccountHistoryMain).TabIndex = 5;
		this.rdapproveddateMain.AutoSize = true;
		this.rdapproveddateMain.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddateMain.Location = new System.Drawing.Point(1012, 46);
		this.rdapproveddateMain.Name = "rdapproveddateMain";
		this.rdapproveddateMain.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddateMain.TabIndex = 11;
		this.rdapproveddateMain.Text = "Approved Date";
		this.rdapproveddateMain.UseVisualStyleBackColor = true;
		this.CmbChequeBankHistoryMain.AllowNull = (DefaultableBoolean)1;
		this.CmbChequeBankHistoryMain.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChequeBankHistoryMain.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChequeBankHistoryMain.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChequeBankHistoryMain).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChequeBankHistoryMain).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChequeBankHistoryMain.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChequeBankHistoryMain).Location = new System.Drawing.Point(431, 14);
		((System.Windows.Forms.Control)(object)this.CmbChequeBankHistoryMain).Name = "CmbChequeBankHistoryMain";
		((System.Windows.Forms.Control)(object)this.CmbChequeBankHistoryMain).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbChequeBankHistoryMain).TabIndex = 4;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(357, 20);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(76, 15);
		this.label21.TabIndex = 54;
		this.label21.Text = "Cheque Bank";
		this.rdmodifydateMain.AutoSize = true;
		this.rdmodifydateMain.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydateMain.Location = new System.Drawing.Point(1012, 18);
		this.rdmodifydateMain.Name = "rdmodifydateMain";
		this.rdmodifydateMain.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydateMain.TabIndex = 10;
		this.rdmodifydateMain.Text = "Modify Date";
		this.rdmodifydateMain.UseVisualStyleBackColor = true;
		this.drdocdateMain.AutoSize = true;
		this.drdocdateMain.Checked = true;
		this.drdocdateMain.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdateMain.Location = new System.Drawing.Point(933, 18);
		this.drdocdateMain.Name = "drdocdateMain";
		this.drdocdateMain.Size = new System.Drawing.Size(75, 19);
		this.drdocdateMain.TabIndex = 8;
		this.drdocdateMain.TabStop = true;
		this.drdocdateMain.Text = "Doc Date";
		this.drdocdateMain.UseVisualStyleBackColor = true;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(357, 48);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(60, 15);
		this.label22.TabIndex = 56;
		this.label22.Text = "Credit A/c";
		this.rdentrydateMain.AutoSize = true;
		this.rdentrydateMain.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydateMain.Location = new System.Drawing.Point(933, 46);
		this.rdentrydateMain.Name = "rdentrydateMain";
		this.rdentrydateMain.Size = new System.Drawing.Size(80, 19);
		this.rdentrydateMain.TabIndex = 9;
		this.rdentrydateMain.Text = "Entry Date";
		this.rdentrydateMain.UseVisualStyleBackColor = true;
		this.btnShowMain.BackColor = System.Drawing.Color.Teal;
		this.btnShowMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowMain.ForeColor = System.Drawing.Color.White;
		this.btnShowMain.Location = new System.Drawing.Point(870, 43);
		this.btnShowMain.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowMain.Name = "btnShowMain";
		this.btnShowMain.Size = new System.Drawing.Size(58, 25);
		this.btnShowMain.TabIndex = 7;
		this.btnShowMain.Text = "Show";
		this.btnShowMain.UseVisualStyleBackColor = false;
		this.btnShowMain.Click += new System.EventHandler(btnShowMain_Click);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(213, 48);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(45, 15);
		this.label51.TabIndex = 9;
		this.label51.Text = "To Doc";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(213, 20);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(60, 15);
		this.label52.TabIndex = 7;
		this.label52.Text = "From Doc";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(5, 48);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 5;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(5, 20);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 3;
		this.label54.Text = "From Date";
		this.ToDocNoHistoryMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistoryMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistoryMain.Location = new System.Drawing.Point(275, 44);
		this.ToDocNoHistoryMain.Name = "ToDocNoHistoryMain";
		this.ToDocNoHistoryMain.ShortcutsEnabled = false;
		this.ToDocNoHistoryMain.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistoryMain.TabIndex = 3;
		this.ToDocNoHistoryMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistoryMain_KeyPress);
		this.FromDocNoHistoryMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistoryMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistoryMain.Location = new System.Drawing.Point(275, 16);
		this.FromDocNoHistoryMain.Name = "FromDocNoHistoryMain";
		this.FromDocNoHistoryMain.ShortcutsEnabled = false;
		this.FromDocNoHistoryMain.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistoryMain.TabIndex = 2;
		this.FromDocNoHistoryMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistoryMain_KeyPress);
		this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistoryMain, this.btnRefreshHistoryMain });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1132, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistoryMain.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistoryMain.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistoryMain.Name = "btnResetHistoryMain";
		this.btnResetHistoryMain.Size = new System.Drawing.Size(68, 24);
		this.btnResetHistoryMain.Text = "Reset";
		this.btnResetHistoryMain.Click += new System.EventHandler(btnResetHistoryMain_Click);
		this.btnRefreshHistoryMain.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistoryMain.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistoryMain.Name = "btnRefreshHistoryMain";
		this.btnRefreshHistoryMain.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistoryMain.Text = "Refresh";
		this.btnRefreshHistoryMain.Click += new System.EventHandler(btnRefreshHistoryMain_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1144, 561);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmPdcTransactionPayment";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SupplierDispatch_FormClosing);
		base.Load += new System.EventHandler(frmPdcTransactionPayment_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPdcTransactionPayment_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccountHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccountHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeBankHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPayeeTitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeNo).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccountHistoryMain).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccountHistoryMain).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeBankHistoryMain).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
