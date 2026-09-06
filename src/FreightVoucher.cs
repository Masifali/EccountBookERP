// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.BLL.Main;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.Account_Definition
{

public class FreightVoucher : Form
{
	private DataTable table = new DataTable();

	private DataTable dtRegister = new DataTable();

	private DataTable dtRegisterGrid = new DataTable();

	private DataTable dtFreightPaymentBreakUp = new DataTable();

	private int VoucherHeadId;

	private int DocumentTypeId = 28;

	private int RecId;

	private int DriverBioId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private int DefaultFreightVoucherCreditAccountId;

	private string ScreenName = "FreightVoucher";

	private bool FormInitialized;

	private bool IncludeBankAccountsInFreightVoucherCreditAccount;

	private bool ChequeBookEnableStatus;

	private double TolerancePercentForDiscountonFreightVoucher;

	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ToolStrip miniToolStrip;

	private ToolStripButton btn;

	private ToolStripButton btnE;

	private ToolStripButton btns;

	private ToolStripButton btnPrint;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton bt;

	private ToolStripButton btnCopy;

	private ToolStripButton btnPaste;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton helpToolStripButton;

	private ToolStrip toolStrip1;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage Form;

	private Panel panel4;

	private ToolStrip TsForm;

	private ToolStripButton New;

	private new ToolStripButton Update;

	private ToolStripButton Save;

	private ToolStripButton Print;

	private ToolStripButton BtnAttachtment;

	private Label PaymentVoucherLable;

	private Panel panel5;

	private GroupBox groupBox2;

	private Label label2;

	private Label label15;

	private DateTimePicker voucherdatetime;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo CmbCashAccount;

	private TextBox txtvoucherno;

	private TextBox txtremarksmain;

	private Label label1;

	private BackgroundWorker backgroundWorker1;

	private Timer timer1;

	private CheckBox ChkBox;

	private TextBox txtAllowShortage;

	private Label label23;

	private TextBox txtBiltyFreight;

	private Label label21;

	private TextBox txtToleranceWeight;

	private Label label18;

	private TextBox txtShortWeight;

	private Label label17;

	private TextBox txtFactoryWeight;

	private Label label13;

	private TextBox txtSupplierWeight;

	private Label label12;

	private TextBox txtBiltyNo;

	private Label label9;

	private TextBox txtVehicleNo;

	private Label label8;

	private TextBox txtGPNo;

	private Label label4;

	private Label label6;

	private DateTimePicker txtGPDate;

	private GroupBox groupBox1;

	private TextBox txtGpId;

	private TabPage Register;

	private ToolStrip toolStrip3;

	private Button btnshow;

	private DateTimePicker datToDate;

	private TextBox txtGpNoTo;

	private TextBox txtGpNoFrom;

	private Label label10;

	private DateTimePicker datFromDate;

	private Label label20;

	private TextBox txtVehicleRegister;

	private Label label3;

	private ToolStripButton btnRegister;

	private ToolStripButton btnResetRegister;

	internal GridEX grdRegister;

	private ToolStripButton btnSlip241;

	private CtrlGrdBar ctrlGrdBar4;

	internal UltraCombo CmbSupplierName;

	private Label label5;

	private Label lbldrcr;

	private Label label7;

	internal UltraCombo CmbCity;

	private Panel panel15;

	private Label label11;

	private Panel panel21;

	private Label label14;

	private Panel panel23;

	private Label label24;

	private GroupBox groupBox3;

	internal UltraCombo CmbDebitAcRegister;

	private Label label22;

	internal UltraCombo CmbCreditAcRegister;

	private Label label19;

	private ToolStripButton btnRefreshRegister;

	private ToolStripButton btnShortCutKey;

	private Label label25;

	private TextBox txtbalance;

	internal GridEX grdPreviousInformation;

	private Panel panel6;

	private Label label26;

	private Panel panel3;

	private TextBox txtPartyName;

	private Label label27;

	private ToolStripButton BtnRefresh;

	private Label label16;

	private TextBox txtShortageApply;

	private TextBox txtWeightToleranceAllowPerMTon;

	private Label label28;

	public GridEX grdPendingRecord;

	private Panel panel2;

	private Panel panel7;

	private GroupBox groupBox4;

	private MaskedTextBox txtCNIC;

	private Label label29;

	private TextBox txtDriverName;

	private Label label30;

	private MaskedTextBox txtDriverCellNo;

	private Label label31;

	private MaskedTextBox txtAlternateCellNo;

	private Label label32;

	private Label label33;

	private TextBox txtFatherName;

	private MaskedTextBox txtWhatsAppNo;

	private Label label34;

	private MaskedTextBox txtFatherCNIC;

	private Label label35;

	private PictureBox btnResetDriverInfo;

	private CheckBox chkFreightAuditbyCity;

	private CheckBox chkOnlyDiscountedRows;

	private Label label36;

	private Label label50;

	private Button btnPaymentBreakUp;

	private TextBox txtManualNumber;

	private Label label37;

	private RadioButton rdCreditAccount;

	private RadioButton rdCashAccount;

	public event Action FormInitializedAction;

	public FreightVoucher(UserAccount puserAccount)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			if (table.Columns.Count == 0)
			{
				table.Columns.Add("Id");
				table.Columns.Add("FreightCustomAccountsGroupId", typeof(int));
				table.Columns.Add("DebitAccountId");
				table.Columns.Add("AccountTitle");
				table.Columns.Add("BiltyFreight", typeof(double));
				table.Columns.Add("FreightPer100Kg", typeof(double));
				table.Columns.Add("FreightPerMton", typeof(double));
				table.Columns.Add("WbCharges", typeof(double));
				table.Columns.Add("AdvanceByParty", typeof(double));
				table.Columns.Add("AdvanceByFactory", typeof(double));
				table.Columns.Add("OtherDeduction", typeof(double));
				table.Columns.Add("TotalFreight", typeof(double));
				table.Columns.Add("ShortageAmount", typeof(double));
				table.Columns.Add("DiscountAmount", typeof(double));
				table.Columns.Add("RemainingAmount", typeof(double));
				table.Columns.Add("DeductionAmount", typeof(double));
				table.Columns.Add("ChargeToParty", typeof(double));
				table.Columns.Add("PaidAmount", typeof(double));
				table.Columns.Add("Remarks");
				table.Columns.Add("VarietyName");
				table.Columns.Add("ItemQty", typeof(double));
				table.Columns.Add("RateForShortage", typeof(double));
			}
			if (dtFreightPaymentBreakUp.Columns.Count == 0)
			{
				dtFreightPaymentBreakUp.Columns.Add("Id");
				dtFreightPaymentBreakUp.Columns.Add("TransactionTypeId", typeof(int));
				dtFreightPaymentBreakUp.Columns.Add("TransactionType");
				dtFreightPaymentBreakUp.Columns.Add("InstrumentTypeId", typeof(int));
				dtFreightPaymentBreakUp.Columns.Add("InstrumentType");
				dtFreightPaymentBreakUp.Columns.Add("AccountTitleId", typeof(int));
				dtFreightPaymentBreakUp.Columns.Add("AccountTitle");
				dtFreightPaymentBreakUp.Columns.Add("ChequeId", typeof(int));
				dtFreightPaymentBreakUp.Columns.Add("ChequeNo", typeof(string));
				dtFreightPaymentBreakUp.Columns.Add("ChequeDate", typeof(DateTime));
				dtFreightPaymentBreakUp.Columns.Add("Amount", typeof(double));
				dtFreightPaymentBreakUp.Columns.Add("PayeeTitle", typeof(string));
				dtFreightPaymentBreakUp.Columns.Add("Remarks", typeof(string));
			}
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
			DataTable dataForDropDownFromFreightVoucher = InvFreightVoucher.GetDataForDropDownFromFreightVoucher(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null);
			string documentNoData = VoucherNoDbCall();
			DataTable pendingRecordsData = PendingRecordDbCall();
			return new
			{
				documentNoData = documentNoData,
				PendingRecordsData = pendingRecordsData,
				HistoryComboData = dataForDropDownFromFreightVoucher
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
				Save.Enabled = formright.DoHaveSaveRight;
				Print.Enabled = formright.DoHavePrintRights;
				Update.Enabled = formright.DoHaveUpdateRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar4, formright);
				if (RecId == 0)
				{
					txtvoucherno.Text = result.documentNoData;
				}
				GetConfigurationsFromGlobal();
				CashAccountFillFromGlobal();
				CityDtFillFromGlobalAndBind();
				DataTable dataTable = table;
				if (dataTable != null && dataTable.Columns.Count > 0)
				{
					grd.DataSource = table;
					grd.RetrieveStructure();
					grdSettings();
				}
				PendingRecordBind(result.PendingRecordsData);
				HistoryComboBind(result.HistoryComboData);
				datFromDate.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				datToDate.Value = DateTime.Now;
				FormInitialized = true;
				this.FormInitializedAction?.Invoke();
				CmbCashAccount.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultFreightVoucherCreditAccountId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultFreightVoucherCreditAccountId"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			TolerancePercentForDiscountonFreightVoucher = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("TolerancePercentForDiscountonFreightVoucher"));
			IncludeBankAccountsInFreightVoucherCreditAccount = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IncludeBankAccountsInFreightVoucherCreditAccount"));
			ChequeBookEnableStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CheqBook Enabled"));
			if (grd.RowCount > 0)
			{
				grd.RootTable.Columns["DiscountAmount"].EditType = (EditType)(TolerancePercentForDiscountonFreightVoucher != 0.0);
			}
			if (DefaultFreightVoucherCreditAccountId > 0)
			{
				CmbCashAccount.Value = DefaultFreightVoucherCreditAccountId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dt)
	{
		try
		{
			DataTable dtSupp = new DataTable();
			dtSupp.Columns.Add("Id", typeof(int));
			dtSupp.Columns.Add("Name", typeof(string));
			DataTable dtCrAc = dtSupp.Clone();
			DataTable dtDrAc = dtSupp.Clone();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
				{
					dtSupp.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "CreditAccount")
				{
					dtCrAc.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "DebitAccount")
				{
					dtDrAc.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbSupplierName, dtSupp, "Id", "Name", "Party Name", AllColumns: true, Conversion.ToInt(CmbSupplierName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCreditAcRegister, dtCrAc, "Id", "Name", "Credit Account", AllColumns: true, Conversion.ToInt(CmbCreditAcRegister.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDebitAcRegister, dtDrAc, "Id", "Name", "Debit Account", AllColumns: true, Conversion.ToInt(CmbDebitAcRegister.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string VoucherNoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			return Conversion.ToString((object)InvFreightVoucher.GenerateCode(new InvFreightVoucher
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId
			}));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CashAccountFillFromGlobal()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AccountTitle", typeof(string));
			dt.Columns.Add("AccountCode", typeof(string));
			dt.Columns.Add("ParentAccountTitle", typeof(string));
			dt.Columns.Add("AccountTypeId", typeof(int));
			List<getGlobalAllAccountsWithCustomGroup> allAccounts = clsGlobalVariables.AllAccountsWithCustomGroupId;
			if (allAccounts == null)
			{
				return;
			}
			IEnumerable<getGlobalAllAccountsWithCustomGroup> filteredAccounts = allAccounts.AsEnumerable();
			filteredAccounts = ((!rdCashAccount.Checked) ? filteredAccounts.Where((getGlobalAllAccountsWithCustomGroup a) => new int[3] { 3, 6, 8 }.Contains(a.AccountTypeId)) : filteredAccounts.Where((getGlobalAllAccountsWithCustomGroup a) => ((!IncludeBankAccountsInFreightVoucherCreditAccount) ? new int[1] { 2 } : new int[2] { 2, 15 }).Contains(a.AccountTypeId)));
			HashSet<int> seenIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup account in filteredAccounts)
			{
				int id = Conversion.ToInt((object)account.ChartOfAccountId);
				if (seenIds.Add(id))
				{
					dt.Rows.Add(id, account.AccountTitle, account.AccountCode, account.ParentAccountTitle, account.AccountTypeId);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCashAccount, dt, "Id", "AccountTitle", "Credit Account", AllColumns: true, Conversion.ToInt(CmbCashAccount.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DebitAccountFillFromGlobal(int CustomGroupId)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AccountTitle", typeof(string));
			dt.Columns.Add("AccountCode", typeof(string));
			dt.Columns.Add("ParentAccountTitle", typeof(string));
			dt.Columns.Add("AccountTypeId", typeof(int));
			List<getGlobalAllAccountsWithCustomGroup> allAccounts = clsGlobalVariables.AllAccountsWithCustomGroupId;
			if (allAccounts == null)
			{
				return;
			}
			IEnumerable<getGlobalAllAccountsWithCustomGroup> enumerable = from a in allAccounts.AsEnumerable()
				where new int[1] { CustomGroupId }.Contains(a.CustomGroupId)
				select a;
			HashSet<int> seenIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup account in enumerable)
			{
				int id = Conversion.ToInt((object)account.ChartOfAccountId);
				if (seenIds.Add(id))
				{
					dt.Rows.Add(id, account.AccountTitle, account.AccountCode, account.ParentAccountTitle, account.AccountTypeId);
				}
			}
			GridEX_Helper.ComboBind(grd.RootTable.Columns["DebitAccountId"], dt, "Id", "AccountTitle", "Debit Account");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityDtFillFromGlobalAndBind()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalAllCities, (GlobalAllCity x) => x.Id, (GlobalAllCity x) => x.CityName);
			int Id = Conversion.ToInt(CmbCity.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbCity, dt, "Id", "Description", "City Name", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combcreditac_Leave(object sender, EventArgs e)
	{
		try
		{
			AccountCurrentBalance();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountCurrentBalance()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = DateTime.Now,
				RefAccountId = Conversion.ToInt(CmbCashAccount.Value)
			});
			if (detail.Rows.Count > 0)
			{
				double ClosingBalance = Conversion.ToDouble(detail.Rows[0]["Balance"]);
				txtbalance.Text = ClosingBalance.ToString("#,#;(#,#);0");
				if (ClosingBalance > 0.0)
				{
					lbldrcr.Text = "Dr";
				}
				else
				{
					lbldrcr.Text = "Cr";
				}
				if (ClosingBalance == 0.0)
				{
					lbldrcr.Text = "Nill";
				}
				lbldrcr.Visible = true;
			}
			else
			{
				txtbalance.Text = "0";
				lbldrcr.Text = "Nill";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count == 0)
			{
				return;
			}
			string columnName = e.Column.Key;
			double shortageAmount = Conversion.ToDouble(item.Cells["ShortageAmount"].Value);
			if (columnName == "ShortageAmount")
			{
				item.Cells["RemainingAmount"].Value = shortageAmount;
			}
			double discountAmount = Conversion.ToDouble(item.Cells["DiscountAmount"].Value);
			double chargeToParty = Conversion.ToDouble(item.Cells["ChargeToParty"].Value);
			double totalFreight = Conversion.ToDouble(item.Cells["TotalFreight"].Value);
			double deductionAmount = Conversion.ToDouble(item.Cells["DeductionAmount"].Value);
			double RemainingAmount = Conversion.ToDouble(item.Cells["RemainingAmount"].Value);
			if (columnName == "OtherDeduction")
			{
				double num = Conversion.ToDouble(item.Cells["BiltyFreight"].Value);
				double WbCharges = Conversion.ToDouble(item.Cells["WbCharges"].Value);
				double AdvanceByParty = Conversion.ToDouble(item.Cells["AdvanceByParty"].Value);
				double AdvanceByFactory = Conversion.ToDouble(item.Cells["AdvanceByFactory"].Value);
				double OtherDeduction = Conversion.ToDouble(item.Cells["OtherDeduction"].Value);
				double num2 = num + WbCharges - (AdvanceByFactory + AdvanceByParty);
				double NetpaidAmount = num2 - deductionAmount;
				if (OtherDeduction > NetpaidAmount)
				{
					OtherDeduction = NetpaidAmount;
					item.Cells["OtherDeduction"].Value = OtherDeduction;
					MessageBox.Show("OtherDeduction can not be greater than PaidAmount.");
				}
				totalFreight = num2 - OtherDeduction;
				item.Cells["TotalFreight"].Value = totalFreight;
				if (deductionAmount > totalFreight)
				{
					deductionAmount = totalFreight;
					item.Cells["DeductionAmount"].Value = deductionAmount;
				}
			}
			if (columnName == "DiscountAmount" && shortageAmount == 0.0)
			{
				item.Cells["DiscountAmount"].Value = 0;
				item.Cells["ChargeToParty"].Value = 0;
				item.Cells["DeductionAmount"].Value = 0;
				MessageBox.Show("There is no shortage.");
				return;
			}
			if (columnName == "DeductionAmount" && deductionAmount > totalFreight)
			{
				deductionAmount = totalFreight;
				item.Cells["DeductionAmount"].Value = deductionAmount;
				MessageBox.Show("DeductionAmount can not be greater than totalFreight.");
			}
			if (columnName == "DeductionAmount" && deductionAmount > RemainingAmount)
			{
				deductionAmount = RemainingAmount;
				item.Cells["DeductionAmount"].Value = deductionAmount;
				MessageBox.Show("DeductionAmount can not be greater than RemainingAmount.");
			}
			if (columnName == "DiscountAmount" || columnName == "ShortageAmount")
			{
				if (discountAmount > shortageAmount)
				{
					discountAmount = shortageAmount;
					item.Cells["DiscountAmount"].Value = discountAmount;
					MessageBox.Show("DiscountAmount can not be greater than ShortageAmount.");
				}
				RemainingAmount = shortageAmount - discountAmount;
				item.Cells["RemainingAmount"].Value = RemainingAmount;
				if (deductionAmount > RemainingAmount)
				{
					deductionAmount = RemainingAmount;
					item.Cells["DeductionAmount"].Value = deductionAmount;
				}
				chargeToParty = RemainingAmount - deductionAmount;
				item.Cells["ChargeToParty"].Value = chargeToParty;
			}
			if (columnName == "DeductionAmount" || columnName == "ShortageAmount")
			{
				chargeToParty = RemainingAmount - deductionAmount;
				item.Cells["ChargeToParty"].Value = chargeToParty;
			}
			if (columnName != "Remarks")
			{
				double paidAmount = totalFreight - deductionAmount;
				item.Cells["PaidAmount"].Value = paidAmount;
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings(int RefDocumentTypeId = 0, double Rate = 0.0, int CustomGroupId = 0)
	{
		try
		{
			grd.RootTable.Columns["AccountTitle"].Visible = true;
			List<string> hiddenColumns = new List<string> { "Id", "DebitAccountId", "FreightCustomAccountsGroupId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"DiscountAmount",
					(EditType)1
				},
				{
					"OtherDeduction",
					(EditType)1
				},
				{
					"DeductionAmount",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			if ((RefDocumentTypeId == 105 || RefDocumentTypeId == 106) && Rate == 0.0)
			{
				editableColumns.Add("ShortageAmount", (EditType)1);
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns, editableColumns);
			if (CustomGroupId > 0)
			{
				grd.RootTable.Columns["AccountTitle"].Visible = false;
				grd.RootTable.Columns["DebitAccountId"].Visible = true;
				DebitAccountFillFromGlobal(CustomGroupId);
			}
			grd.RootTable.Columns["AccountTitle"].Caption = "Account Title (Dr)";
			grd.RootTable.Columns["VarietyName"].Caption = "Item Name";
			grd.RootTable.Columns["AdvanceByParty"].Caption = "Advance By Supplier";
			grd.RootTable.Columns["WbCharges"].Caption = "Scale Charges";
			grd.RootTable.Columns["PaidAmount"].Caption = "Net Payable Amount";
			grd.RootTable.Columns["ChargeToParty"].Caption = "Charge To Supplier";
			grd.RootTable.Columns["DeductionAmount"].Caption = "Deduction From Freight";
			grd.RootTable.Columns["RateForShortage"].Caption = "Rate For Shortage / 40Kg";
			grd.RootTable.Columns["DiscountAmount"].EditType = (EditType)(TolerancePercentForDiscountonFreightVoucher != 0.0);
			grd.RootTable.Columns["BiltyFreight"].Width = 70;
			grd.RootTable.Columns["FreightPerMton"].Width = 70;
			grd.RootTable.Columns["WbCharges"].Width = 65;
			grd.RootTable.Columns["AdvanceByParty"].Width = 80;
			grd.RootTable.Columns["AdvanceByFactory"].Width = 80;
			grd.RootTable.Columns["OtherDeduction"].Width = 80;
			grd.RootTable.Columns["TotalFreight"].Width = 80;
			grd.RootTable.Columns["ShortageAmount"].Width = 80;
			grd.RootTable.Columns["DiscountAmount"].Width = 80;
			grd.RootTable.Columns["RemainingAmount"].Width = 80;
			grd.RootTable.Columns["DeductionAmount"].Width = 80;
			grd.RootTable.Columns["ChargeToParty"].Width = 80;
			grd.RootTable.Columns["PaidAmount"].Width = 80;
			GridEX_Helper.ChangeColorOfColumn(grd, "PaidAmount");
			grd.RootTable.Columns["Remarks"].Width = 110;
			grd.RootTable.Columns["VarietyName"].Width = 100;
			grd.RootTable.Columns["ItemQty"].Width = 60;
			grd.RootTable.Columns["RateForShortage"].Width = 90;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PreviousCityDataBind(int CityId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dt = GatePassInward.getLastFreightInfoByCityId(UserAccount.CompanyId, CityId, 5, RecId);
			if (dt.Rows.Count > 0)
			{
				DataTable pending = new DataTable
				{
					Columns = 
					{
						{
							"GpNo",
							typeof(int)
						},
						{
							"GpDate",
							typeof(DateTime)
						},
						{
							"SupplierName",
							typeof(string)
						},
						{
							"SupplierWeight",
							typeof(double)
						},
						{
							"FactoryWeight",
							typeof(double)
						},
						{
							"DiffWeight",
							typeof(double)
						},
						{
							"Freight",
							typeof(double)
						},
						{
							"FreightPerMton",
							typeof(double)
						},
						{
							"NoOfBags",
							typeof(double)
						},
						{
							"FreightPer40Kg",
							typeof(double)
						}
					}
				};
				foreach (DataRow row in dt.Rows)
				{
					pending.Rows.Add(Conversion.ToInt(row["GatePassNo"]), Conversion.ToDateTime(row["GatePassDate"]), row["SupplierName"], Conversion.ToDouble(row["SupplierWeight"]), Conversion.ToDouble(row["FactoryWeight"]), Conversion.ToDouble(row["DiffWeight"]), Conversion.ToDouble(row["Freight"]), Conversion.ToDouble(row["PerMtonFreight"]), Conversion.ToDouble(row["NoOfPackages"]), Conversion.ToDouble(row["FreightPer40Kg"]));
				}
				grdPreviousInformation.DataSource = pending;
				grdPreviousInformation.RetrieveStructure();
				PreviousCityDataSetting();
			}
			else
			{
				grdPreviousInformation.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PreviousCityDataSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "GpNo" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdPreviousInformation, 2, 3, hiddenColumns);
			grdPreviousInformation.RootTable.Columns["GpDate"].Width = 70;
			grdPreviousInformation.RootTable.Columns["SupplierName"].Width = 90;
			grdPreviousInformation.RootTable.Columns["SupplierWeight"].Width = 70;
			grdPreviousInformation.RootTable.Columns["FactoryWeight"].Width = 70;
			grdPreviousInformation.RootTable.Columns["DiffWeight"].Width = 50;
			grdPreviousInformation.RootTable.Columns["Freight"].Width = 70;
			grdPreviousInformation.RootTable.Columns["FreightPer40Kg"].Width = 60;
			grdPreviousInformation.RootTable.Columns["NoOfBags"].Width = 54;
			grdPreviousInformation.RootTable.Columns["FreightPerMton"].Width = 63;
			grdPreviousInformation.TotalRow = (InheritableBoolean)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable PendingRecordDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return GatePassInward.GetGpDataForFreightVoucher(new GatePassInward
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

	private void PendingRecordBind(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				DataTable pending = new DataTable();
				pending.Columns.Add("Id", typeof(int));
				pending.Columns.Add("GpDate", typeof(DateTime));
				pending.Columns.Add("GpNo", typeof(int));
				pending.Columns.Add("VehicleNo", typeof(string));
				pending.Columns.Add("BiltyNo", typeof(string));
				pending.Columns.Add("BiltyFreight", typeof(double));
				pending.Columns.Add("WbCharges", typeof(double));
				pending.Columns.Add("AdvanceByParty", typeof(double));
				pending.Columns.Add("AdvanceByFactory", typeof(double));
				pending.Columns.Add("TotalFreight", typeof(double));
				pending.Columns.Add("ItemQty", typeof(double));
				pending.Columns.Add("SupplierWeight", typeof(double));
				pending.Columns.Add("NetWbWeight", typeof(double));
				pending.Columns.Add("ToleranceWeight", typeof(double));
				pending.Columns.Add("ShortWeight", typeof(double));
				pending.Columns.Add("AllowShortWeight", typeof(double));
				pending.Columns.Add("ShortageApplyWeight", typeof(double));
				pending.Columns.Add("ShortageAmount", typeof(double));
				pending.Columns.Add("Status", typeof(string));
				pending.Columns.Add("CityId", typeof(int));
				pending.Columns.Add("CityName", typeof(string));
				pending.Columns.Add("VarietyName", typeof(string));
				pending.Columns.Add("SupplierName", typeof(string));
				pending.Columns.Add("FreightCustomAccountsGroupId", typeof(int));
				pending.Columns.Add("DebitAccountId", typeof(int));
				pending.Columns.Add("AccountTitle", typeof(string));
				pending.Columns.Add("OrderRate", typeof(double));
				pending.Columns.Add("Remarks", typeof(string));
				pending.Columns.Add("WeightToleranceAllowPerMTon", typeof(double));
				pending.Columns.Add("PolicyTypeId", typeof(int));
				pending.Columns.Add("RefDocumentTypeId", typeof(int));
				pending.Columns.Add("DriverName", typeof(string));
				pending.Columns.Add("CnicNo", typeof(string));
				pending.Columns.Add("DriverCellNo", typeof(string));
				pending.Columns.Add("whatsappNo", typeof(string));
				pending.Columns.Add("AlternateCellNo", typeof(string));
				pending.Columns.Add("FatherName", typeof(string));
				pending.Columns.Add("fatherCnicNo", typeof(string));
				pending.Columns.Add("driverBiodataId", typeof(int));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					pending.Rows.Add(dt.Rows[i]["Id"], Conversion.ToDateTime(dt.Rows[i]["GpDate"]).ToShortDateString(), dt.Rows[i]["GpSrNo"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["BiltyNo"], dt.Rows[i]["BiltyFreight"], dt.Rows[i]["WbCharges"], dt.Rows[i]["AdvanceByParty"], dt.Rows[i]["AdvanceByFactory"], dt.Rows[i]["TotalFreight"], dt.Rows[i]["ItemQty"], dt.Rows[i]["SupplierWeight"], dt.Rows[i]["NetWbWeight"], dt.Rows[i]["ToleranceWeight"], dt.Rows[i]["ShortWeight"], dt.Rows[i]["PolicyShortWeight"], dt.Rows[i]["ShortWeightAfterDeductionPolicy"], dt.Rows[i]["ShortageAmount"], dt.Rows[i]["Status"], dt.Rows[i]["CityId"], dt.Rows[i]["CityName"], dt.Rows[i]["VarietyName"], dt.Rows[i]["SupplierName"], dt.Rows[i]["FreightCustomAccountsGroupId"], dt.Rows[i]["DebitAccountId"], dt.Rows[i]["AccountTitle"], dt.Rows[i]["OrderRate"], dt.Rows[i]["Remarks"], dt.Rows[i]["WeightToleranceAllowPerMTon"], dt.Rows[i]["PolicyTypeId"], dt.Rows[i]["RefDocumentTypeId"], dt.Rows[i]["DriverName"], dt.Rows[i]["CnicNo"], dt.Rows[i]["DriverCellNo"], dt.Rows[i]["whatsappNo"], dt.Rows[i]["AlternateCellNo"], dt.Rows[i]["FatherName"], dt.Rows[i]["fatherCnicNo"], dt.Rows[i]["driverBiodataId"]);
				}
				grdPendingRecord.DataSource = pending;
				grdPendingRecord.RetrieveStructure();
				PendingGridSetting();
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

	private void PendingGridSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "CityId", "DebitAccountId", "FreightCustomAccountsGroupId", "ToleranceWeight", "WeightToleranceAllowPerMTon", "PolicyTypeId", "RefDocumentTypeId", "driverBiodataId", "CnicNo",
				"DriverCellNo", "whatsappNo", "AlternateCellNo", "FatherName", "fatherCnicNo"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdPendingRecord, 3, 3, hiddenColumns);
			grdPendingRecord.RootTable.Columns["VarietyName"].Caption = "Item Name";
			grdPendingRecord.RootTable.Columns["ToleranceWeight"].Caption = "WeighBridge Tolerance";
			grdPendingRecord.RootTable.Columns["VehicleNo"].Width = 80;
			grdPendingRecord.RootTable.Columns["CityName"].Width = 100;
			grdPendingRecord.RootTable.Columns["VarietyName"].Width = 150;
			grdPendingRecord.RootTable.Columns["SupplierName"].Width = 150;
			grdPendingRecord.RootTable.Columns["AccountTitle"].Width = 150;
			GridEX_Helper.AddButton(grdPendingRecord, "Load", "Load", 40, 0);
			grdPendingRecord.FrozenColumns = 1;
			((Control)(object)grdPendingRecord).Refresh();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingRecord_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow item = grdPendingRecord.CurrentRow;
			if (item != null && (int)item.RowType <= 0 && item.Cells.Count != 0 && e.Column.Key == "Load")
			{
				LoadRecord(item);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadRecord(GridEXRow item)
	{
		try
		{
			if (RecId > 0)
			{
				throw new Exception("Record can't be loaded because another record is in update mode.");
			}
			table.Rows.Clear();
			dtFreightPaymentBreakUp.Rows.Clear();
			int debitAccountId = Conversion.ToInt(item.Cells["DebitAccountId"].Value);
			if (debitAccountId == 0)
			{
				throw new Exception("FreightAccount not found. Please check!");
			}
			txtGpId.Text = Conversion.ToString(item.Cells["Id"].Value);
			txtGPDate.Value = Conversion.ToDateTime(item.Cells["GpDate"].Value);
			txtGPNo.Text = Conversion.ToString(item.Cells["GpNo"].Value);
			txtVehicleNo.Text = Conversion.ToString(item.Cells["VehicleNo"].Value);
			txtBiltyNo.Text = Conversion.ToString(item.Cells["BiltyNo"].Value);
			txtBiltyFreight.Text = Conversion.ToString(item.Cells["BiltyFreight"].Value);
			txtAllowShortage.Text = Conversion.ToString(item.Cells["AllowShortWeight"].Value);
			txtShortageApply.Text = Conversion.ToString(item.Cells["ShortageApplyWeight"].Value);
			txtWeightToleranceAllowPerMTon.Text = Conversion.ToString(item.Cells["WeightToleranceAllowPerMTon"].Value);
			txtSupplierWeight.Text = Conversion.ToString(item.Cells["SupplierWeight"].Value);
			txtFactoryWeight.Text = Conversion.ToString(item.Cells["NetWbWeight"].Value);
			txtToleranceWeight.Text = Conversion.ToString(item.Cells["ToleranceWeight"].Value);
			txtShortWeight.Text = Conversion.ToString(item.Cells["ShortWeight"].Value);
			label17.Text = ((Conversion.ToDouble((object)txtShortWeight.Text) < 0.0) ? "Excess Weight" : "Short Weight");
			txtremarksmain.Text = Conversion.ToString(item.Cells["Remarks"].Value);
			int cityId = Conversion.ToInt(item.Cells["CityId"].Value);
			CmbCity.Value = cityId;
			string PartyName = Conversion.ToString(item.Cells["SupplierName"].Value);
			txtPartyName.Text = PartyName;
			double biltyFreight = Conversion.ToDouble(item.Cells["BiltyFreight"].Value);
			double AdvanceByParty = Conversion.ToDouble(item.Cells["AdvanceByParty"].Value);
			double AdvanceByFactory = Conversion.ToDouble(item.Cells["AdvanceByFactory"].Value);
			double FreightPerMTon = ((Conversion.ToDouble(item.Cells["NetWbWeight"].Value) > 0.0) ? (biltyFreight / Conversion.ToDouble(item.Cells["NetWbWeight"].Value) * 1000.0) : 0.0);
			double FreightPer100Kg = ((Conversion.ToDouble(item.Cells["NetWbWeight"].Value) > 0.0) ? (biltyFreight / Conversion.ToDouble(item.Cells["NetWbWeight"].Value) * 100.0) : 0.0);
			double wbCharges = Conversion.ToDouble(item.Cells["WbCharges"].Value);
			double totalFreightAmount = biltyFreight + wbCharges - AdvanceByFactory - AdvanceByParty;
			double shortageAmount = Conversion.ToDouble(item.Cells["ShortageAmount"].Value);
			double netAmount = totalFreightAmount;
			int PolicyTypeId = Conversion.ToInt(item.Cells["PolicyTypeId"].Value);
			int RefDocumentTypeId = Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value);
			double Rate = Conversion.ToDouble(item.Cells["OrderRate"].Value);
			int FreightCustomAccountsGroupId = Conversion.ToInt(item.Cells["FreightCustomAccountsGroupId"].Value);
			DriverBioId = Conversion.ToInt(item.Cells["driverBiodataId"].Value);
			if (DriverBioId > 0)
			{
				txtCNIC.Text = Conversion.ToString(item.Cells["CnicNo"].Value);
				txtDriverCellNo.Text = Conversion.ToString(item.Cells["DriverCellNo"].Value);
				txtWhatsAppNo.Text = Conversion.ToString(item.Cells["whatsappNo"].Value);
				txtAlternateCellNo.Text = Conversion.ToString(item.Cells["AlternateCellNo"].Value);
				txtDriverName.Text = Conversion.ToString(item.Cells["DriverName"].Value);
				txtFatherName.Text = Conversion.ToString(item.Cells["FatherName"].Value);
				txtFatherCNIC.Text = Conversion.ToString(item.Cells["fatherCnicNo"].Value);
				DriverFieldsDisableOrEnable(flag: false);
			}
			table.Rows.Add(0, FreightCustomAccountsGroupId, debitAccountId, item.Cells["AccountTitle"].Value, biltyFreight, FreightPer100Kg, FreightPerMTon, wbCharges, AdvanceByParty, AdvanceByFactory, 0, totalFreightAmount, shortageAmount, 0, shortageAmount, 0, shortageAmount, netAmount, "", item.Cells["VarietyName"].Value, item.Cells["ItemQty"].Value, item.Cells["OrderRate"].Value);
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings(RefDocumentTypeId, Rate, FreightCustomAccountsGroupId);
			PreviousCityDataBind(cityId);
			if (PolicyTypeId == 3)
			{
				txtAllowShortage.ReadOnly = false;
				txtAllowShortage.TabStop = true;
			}
			else
			{
				txtAllowShortage.ReadOnly = true;
				txtAllowShortage.TabStop = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(InvFreightVoucherDetail detail, GridEXRow r)
	{
		int FreightCustomAccountsGroupId = Conversion.ToInt(r.Cells["FreightCustomAccountsGroupId"].Value);
		int debitAccountId = Conversion.ToInt(r.Cells["DebitAccountId"].Value);
		string accountTitle = Conversion.ToString(r.Cells["AccountTitle"].Value);
		int biltyFreight = Conversion.ToInt(r.Cells["BiltyFreight"].Value);
		decimal scaleCharges = Conversion.ToDecimal(r.Cells["WbCharges"].Value);
		double AdvanceByParty = Conversion.ToDouble(r.Cells["AdvanceByParty"].Value);
		double AdvanceByFactory = Conversion.ToDouble(r.Cells["AdvanceByFactory"].Value);
		decimal shortageAmount = Conversion.ToDecimal(r.Cells["ShortageAmount"].Value);
		double discountAmount = Conversion.ToDouble(r.Cells["DiscountAmount"].Value);
		decimal chargeToParty = Conversion.ToDecimal(r.Cells["ChargeToParty"].Value);
		decimal deductedAmount = Conversion.ToDecimal(r.Cells["DeductionAmount"].Value);
		decimal paidAmount = Conversion.ToDecimal(r.Cells["PaidAmount"].Value);
		decimal rateForShortage = Conversion.ToDecimal(r.Cells["RateForShortage"].Value);
		detail.CustomGroupId = FreightCustomAccountsGroupId;
		detail.DebitAccountId = debitAccountId;
		detail.AccountTitle = accountTitle;
		detail.BiltyFreight = biltyFreight;
		detail.ScaleCharges = scaleCharges;
		detail.AdvanceByParty = AdvanceByParty;
		detail.AdvanceByFactory = AdvanceByFactory;
		detail.ShortageAmount = shortageAmount;
		detail.AddLessAmount = discountAmount;
		detail.ChargeToParty = chargeToParty;
		detail.DeductedAmount = deductedAmount;
		detail.TotalPayableAmount = paidAmount;
		detail.RateForShortage = rateForShortage;
		detail.OtherDeduction = Conversion.ToDouble(r.Cells["OtherDeduction"].Value);
		StringBuilder remarks = new StringBuilder();
		remarks.Append("Freight of ").Append(Conversion.ToString(r.Cells["ItemQty"].Value)).Append(" Bags of ")
			.Append(Conversion.ToString(r.Cells["VarietyName"].Value))
			.Append(" GpNo ")
			.Append(txtGPNo.Text)
			.Append(" VehicleNo ")
			.Append(txtVehicleNo.Text)
			.Append(" BiltyNo ")
			.Append(txtBiltyNo.Text);
		if (Conversion.ToInt((object)txtBiltyFreight.Text) > 0)
		{
			remarks.Append(" BiltyFreight ").Append(txtBiltyFreight.Text);
		}
		if (Conversion.ToDecimal((object)txtSupplierWeight.Text) > 0m)
		{
			remarks.Append(" SupplierWeight ").Append(txtSupplierWeight.Text).Append(" WeighBridgeWeight ")
				.Append(txtFactoryWeight.Text);
		}
		if (Conversion.ToDecimal((object)txtShortWeight.Text) > 0m)
		{
			remarks.Append(" ShortWeight ").Append(txtShortWeight.Text).Append(" KG");
		}
		else if (Conversion.ToDecimal((object)txtShortWeight.Text) < 0m)
		{
			remarks.Append(" ExcessWeight ").Append(txtShortWeight.Text).Append(" KG");
		}
		if (Conversion.ToInt((object)detail.LessAny) > 0)
		{
			remarks.Append(" ShortageAmount ").Append(detail.LessAny);
		}
		if (Conversion.ToInt((object)detail.OtherDeduction) > 0)
		{
			remarks.Append(" OtherDeduction ").Append(detail.OtherDeduction);
		}
		if (Conversion.ToInt((object)detail.AddLessAmount) > 0)
		{
			remarks.Append(" AddLessAmount ").Append(detail.AddLessAmount).Append(" PaidAmount ")
				.Append(detail.TotalPayableAmount);
		}
		if (!string.IsNullOrEmpty(txtPartyName.Text))
		{
			remarks.Append("\nFor Party ").Append(txtPartyName.Text);
		}
		if (!string.IsNullOrWhiteSpace(txtremarksmain.Text))
		{
			remarks.Append("\n ").Append(txtremarksmain.Text);
		}
		detail.RemarksDetail = remarks.ToString();
	}

	private void Insert()
	{
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Expected O, but got Unknown
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Expected O, but got Unknown
		//IL_09d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09df: Expected O, but got Unknown
		//IL_0872: Unknown result type (might be due to invalid IL or missing references)
		//IL_0879: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (txtDriverCellNo.Text.Trim() == string.Empty || !txtDriverCellNo.MaskFull)
			{
				MessageBox.Show("Cell No. Field Required");
				txtDriverCellNo.Focus();
			}
			else if (txtDriverName.Text.Trim() == string.Empty)
			{
				MessageBox.Show("Driver Name  Field Required");
				txtDriverName.Focus();
			}
			else if (dtFreightPaymentBreakUp.Rows.Count == 0 && Conversion.ToInt(CmbCashAccount.Value) == 0)
			{
				MessageBox.Show("Cash Account Field Required");
				CmbCashAccount.Focus();
			}
			else
			{
				if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
				{
					(txtGPNo, "GpNo", FormHelper.ValidType.INT),
					(txtBiltyFreight, "BiltyFreight", FormHelper.ValidType.INT),
					(txtSupplierWeight, "SupplierWeight", FormHelper.ValidType.Double),
					(txtFactoryWeight, "FactoryWeight", FormHelper.ValidType.Double)
				}) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
				{
					return;
				}
				double SumDiscount = GridEX_Helper.GetColumnSum(grd, "DiscountAmount");
				double ShortageAmount = GridEX_Helper.GetColumnSum(grd, "ShortageAmount");
				double ToleranceAmount = ShortageAmount * TolerancePercentForDiscountonFreightVoucher / 100.0;
				if (SumDiscount > ToleranceAmount)
				{
					throw new Exception($"Total Discount Amount {SumDiscount} cannot be greater than Tolerance Amount {ToleranceAmount} for Shortage Amount {ShortageAmount}.");
				}
				double allowshortage = Conversion.ToDouble((object)txtAllowShortage.Text);
				double shortweight = Conversion.ToDouble((object)txtShortWeight.Text);
				double Toleranceweight = shortweight * TolerancePercentForDiscountonFreightVoucher / 100.0;
				if (allowshortage > Toleranceweight)
				{
					throw new Exception($"Total Allow shortage {allowshortage} cannot be greater than Tolerance Weight {Toleranceweight} for Short weight {shortweight}.");
				}
				double sumFreightPerMetricTon = GridEX_Helper.GetColumnSum(grd, "FreightPerMTon");
				GridEXRow obj2 = (from r in grdPreviousInformation.GetRows()
					orderby Conversion.ToDateTime(r.Cells["GpDate"].Value) descending
					select r).FirstOrDefault();
				double firstRowFreightPerMetricTon = Conversion.ToDouble((obj2 != null) ? obj2.Cells["FreightPerMton"].Value : null);
				if (sumFreightPerMetricTon > 0.0 && firstRowFreightPerMetricTon > 0.0 && sumFreightPerMetricTon > firstRowFreightPerMetricTon + 100.0 && !FormHelper.ConfirmAction($"Total Freight Per Metric Ton ({sumFreightPerMetricTon:0.##}) exceeds the previous freight rate ({firstRowFreightPerMetricTon:0.##}) by more than 100." + " Do you want to continue?"))
				{
					return;
				}
				InvFreightVoucher obj = new InvFreightVoucher();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.BranchId = UserAccount.BranchesId;
				obj.ProjectId = UserAccount.BranchesId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.DocumentTypeId = DocumentTypeId;
				obj.EntryUserId = UserAccount.ID;
				obj.ModifyUserId = UserAccount.ID;
				obj.EntryDate = DateTime.Now;
				obj.ModifyDate = DateTime.Now;
				obj.ApprovedDate = DateTime.Now;
				obj.Id = RecId;
				obj.DocumentNo = Conversion.ToInt((object)txtvoucherno.Text.Trim());
				obj.DocDate = voucherdatetime.Value;
				obj.FreightOn = (rdCreditAccount.Checked ? 1 : 0);
				bool hasDifferentAccountIds = (from r in dtFreightPaymentBreakUp.AsEnumerable()
					where !r.IsNull("AccountTitleId")
					select r.Field<int>("AccountTitleId")).Distinct().Skip(1).Any();
				obj.CashAccountId = ((dtFreightPaymentBreakUp.Rows.Count == 0) ? Conversion.ToInt(CmbCashAccount.Value) : ((!hasDifferentAccountIds) ? Conversion.ToInt(dtFreightPaymentBreakUp.Rows[0]["AccountTitleId"]) : 0));
				obj.GatePassId = Conversion.ToInt((object)txtGpId.Text);
				obj.GatePassNo = Conversion.ToInt((object)txtGPNo.Text);
				obj.GatePassDate = Conversion.ToDateTime((object)txtGPDate.Value);
				obj.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text);
				obj.ManualNo = Conversion.ToString((object)txtManualNumber.Text);
				obj.BiltyNo = Conversion.ToString((object)txtBiltyNo.Text);
				obj.SupplierWeight = Conversion.ToDecimal((object)txtSupplierWeight.Text);
				obj.FactoryWeight = Conversion.ToDecimal((object)txtFactoryWeight.Text);
				obj.ToleranceWeight = Conversion.ToDecimal((object)txtToleranceWeight.Text);
				obj.DifferenceWeight = Conversion.ToDecimal((object)txtShortWeight.Text);
				obj.AllowShortage = ((obj.DifferenceWeight > 0m) ? Conversion.ToDouble((object)txtAllowShortage.Text) : 0.0);
				obj.ShortageApply = Conversion.ToDouble((object)txtShortageApply.Text);
				obj.CityId = Conversion.ToInt(CmbCity.Value);
				obj.RemarksHeader = txtremarksmain.Text.Trim();
				obj.TotalPaidAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "PaidAmount"));
				obj.driverBiodataId = DriverBioId;
				obj.CnicNo = txtCNIC.Text;
				obj.DriverCellNo = txtDriverCellNo.Text;
				obj.AlternateCellNo = txtAlternateCellNo.Text;
				obj.whatsappNo = txtWhatsAppNo.Text;
				obj.DriverName = txtDriverName.Text;
				obj.FatherName = txtFatherName.Text;
				obj.fatherCnicNo = txtFatherCNIC.Text;
				obj.InvFreightVoucherDetailslist = new List<InvFreightVoucherDetail>();
				obj.InvFreightVoucherPaymentBreakupList = new List<InvFreightVoucherPaymentBreakup>();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					InvFreightVoucherDetail vd = new InvFreightVoucherDetail();
					vd.Id = ((RecId != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
					vd.SortNo = r2.RowIndex + 1;
					FillDetailListCommonForInsertAndDelete(vd, r2);
					FormHelper.ValidateField(vd.DebitAccountId, "Debit Account", r2.RowIndex);
					FormHelper.ValidateField(vd.BiltyFreight, "BiltyFreight", r2.RowIndex);
					if (Conversion.ToDecimal((object)Math.Round(vd.AddLessAmount)) + Math.Round(vd.ChargeToParty) + Math.Round(vd.DeductedAmount) > Math.Round(vd.ShortageAmount))
					{
						throw new Exception($"Sum of 'DiscountAmount,ChargeToParty and DeductionAmount' cannot exceed 'ShortageAmount'. in Row No : {r2.RowIndex + 1}");
					}
					if (vd.TotalPayableAmount < 0m)
					{
						throw new Exception($"TotalPayable Amount can not less than 0 in Row No : {r2.RowIndex + 1}");
					}
					obj.InvFreightVoucherDetailslist.Add(vd);
				}
				if (dtFreightPaymentBreakUp.Rows.Count > 0)
				{
					decimal BreakUpAmount = default(decimal);
					foreach (DataRow row in dtFreightPaymentBreakUp.Rows)
					{
						InvFreightVoucherPaymentBreakup d = new InvFreightVoucherPaymentBreakup();
						d.TransTypeId = Conversion.ToInt(row["TransactionTypeId"]);
						d.InstrumentTypeId = Conversion.ToInt(row["InstrumentTypeId"]);
						d.AccountId = Conversion.ToInt(row["AccountTitleId"]);
						d.CheqId = (ChequeBookEnableStatus ? Conversion.ToInt(row["ChequeId"]) : 0);
						d.CheqNo = Conversion.ToString(row["ChequeNo"]);
						d.CheqDate = Conversion.ToDateTime(row["ChequeDate"]);
						d.Amount = Conversion.ToDecimal(row["Amount"]);
						BreakUpAmount += d.Amount;
						d.PayeeTitle = Conversion.ToString(row["PayeeTitle"]);
						d.Remarks = Conversion.ToString(row["Remarks"]);
						obj.InvFreightVoucherPaymentBreakupList.Add(d);
					}
					if (BreakUpAmount != obj.TotalPaidAmount)
					{
						throw new Exception($"BreakUp Amount:{BreakUpAmount} not equal to TotalPaidAmount:{obj.TotalPaidAmount}");
					}
				}
				else
				{
					InvFreightVoucherPaymentBreakup d2 = new InvFreightVoucherPaymentBreakup();
					int AccountTypeId = Conversion.ToInt(((UltraDropDownBase)CmbCashAccount).SelectedRow.Cells["AccountTypeId"].Value);
					bool otherparty = ((AccountTypeId == 3 || AccountTypeId == 6 || AccountTypeId == 8) ? true : false);
					d2.TransTypeId = AccountTypeId switch
					{
						15 => 2, 
						2 => 1, 
						_ => otherparty ? 3 : 0, 
					};
					d2.InstrumentTypeId = ((AccountTypeId == 15) ? 3 : 0);
					d2.AccountId = obj.CashAccountId;
					d2.CheqId = 0;
					d2.CheqNo = "";
					d2.CheqDate = DateTime.Now;
					d2.Amount = obj.TotalPaidAmount;
					obj.InvFreightVoucherPaymentBreakupList.Add(d2);
				}
				obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<InvFreightVoucher>(obj, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = InvFreightVoucher.Save(obj);
				if (RecId == 0)
				{
					MessageBox.Show("Record Save Successfully...[" + obj.DocumentNo + "]");
				}
				else
				{
					MessageBox.Show("Record Update Successfully...[" + obj.DocumentNo + "]");
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, obj.DocumentTypeId, ScreenName);
				}
				Reset();
				if (ChkBox.Checked)
				{
					FreightSlip241(success);
				}
			}
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
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Update_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Id not found please check");
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
		try
		{
			Reset();
			RecId = ID;
			InvFreightVoucher obj = InvFreightVoucher.GetByID(ID);
			if (obj != null)
			{
				Save.Visible = false;
				Update.Visible = true;
				tabControl1.SelectedIndex = 0;
				voucherdatetime.Value = Conversion.ToDateTime((object)obj.DocDate);
				txtvoucherno.Text = Conversion.ToString((object)obj.DocumentNo);
				if (obj.CashAccountId > 0)
				{
					CmbCashAccount.Value = Conversion.ToInt((object)obj.CashAccountId);
				}
				txtremarksmain.Text = Conversion.ToString((object)obj.RemarksHeader);
				txtGpId.Text = Conversion.ToString((object)obj.GatePassId);
				txtGPNo.Text = Conversion.ToString((object)obj.GatePassNo);
				txtGPDate.Value = Conversion.ToDateTime((object)obj.GatePassDate);
				txtManualNumber.Text = Conversion.ToString((object)obj.ManualNo);
				txtVehicleNo.Text = Conversion.ToString((object)obj.VehicleNo);
				txtBiltyNo.Text = Conversion.ToString((object)obj.BiltyNo);
				txtSupplierWeight.Text = Conversion.ToString((object)obj.SupplierWeight);
				txtFactoryWeight.Text = Conversion.ToString((object)obj.FactoryWeight);
				txtToleranceWeight.Text = Conversion.ToString((object)obj.ToleranceWeight);
				txtShortWeight.Text = Conversion.ToString((object)obj.DifferenceWeight);
				txtAllowShortage.Text = Conversion.ToString((object)obj.AllowShortage);
				txtShortageApply.Text = Conversion.ToString((object)obj.ShortageApply);
				if (obj.FreightOn == 1)
				{
					rdCreditAccount.Checked = true;
				}
				else
				{
					rdCashAccount.Checked = true;
				}
				label17.Text = ((Conversion.ToDouble((object)txtShortWeight.Text) < 0.0) ? "Excess Weight" : "Short Weight");
				CmbCity.Value = Conversion.ToInt((object)obj.CityId);
				txtPartyName.Text = obj.PartyName;
				DriverBioId = obj.driverBiodataId;
				if (DriverBioId > 0)
				{
					txtCNIC.Text = obj.CnicNo;
					txtDriverCellNo.Text = obj.DriverCellNo;
					txtWhatsAppNo.Text = obj.whatsappNo;
					txtAlternateCellNo.Text = obj.AlternateCellNo;
					txtDriverName.Text = obj.DriverName;
					txtFatherName.Text = obj.FatherName;
					txtFatherCNIC.Text = obj.fatherCnicNo;
					DriverFieldsDisableOrEnable(flag: false);
				}
				VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, DocumentTypeId);
				table.Rows.Clear();
				int Freight = 0;
				int CustomGroupId = 0;
				decimal RateForShortage = default(decimal);
				decimal WbCharges = default(decimal);
				for (int j = 0; j < obj.InvFreightVoucherDetailslist.Count; j++)
				{
					InvFreightVoucherDetail item = obj.InvFreightVoucherDetailslist[j];
					RateForShortage = obj.InvFreightVoucherDetailslist[j].RateForShortage;
					CustomGroupId = obj.InvFreightVoucherDetailslist[j].CustomGroupId;
					decimal FreightPerMTon = ((obj.FactoryWeight > 0m) ? (Conversion.ToDecimal((object)obj.InvFreightVoucherDetailslist[j].BiltyFreight) / obj.FactoryWeight * 1000m) : 0m);
					decimal FreightPer100Kg = ((obj.FactoryWeight > 0m) ? (Conversion.ToDecimal((object)obj.InvFreightVoucherDetailslist[j].BiltyFreight) / obj.FactoryWeight * 100m) : 0m);
					double totalFreightAmount = (double)item.BiltyFreight + Conversion.ToDouble((object)item.ScaleCharges) - item.AdvanceByFactory - item.AdvanceByParty - item.OtherDeduction;
					table.Rows.Add(obj.InvFreightVoucherDetailslist[j].Id, obj.InvFreightVoucherDetailslist[j].CustomGroupId, obj.InvFreightVoucherDetailslist[j].DebitAccountId, obj.InvFreightVoucherDetailslist[j].AccountTitle, obj.InvFreightVoucherDetailslist[j].BiltyFreight, FreightPer100Kg, FreightPerMTon, obj.InvFreightVoucherDetailslist[j].ScaleCharges, obj.InvFreightVoucherDetailslist[j].AdvanceByParty, obj.InvFreightVoucherDetailslist[j].AdvanceByFactory, obj.InvFreightVoucherDetailslist[j].OtherDeduction, totalFreightAmount, obj.InvFreightVoucherDetailslist[j].ShortageAmount, obj.InvFreightVoucherDetailslist[j].AddLessAmount, Conversion.ToDouble((object)obj.InvFreightVoucherDetailslist[j].ShortageAmount) - obj.InvFreightVoucherDetailslist[j].AddLessAmount, obj.InvFreightVoucherDetailslist[j].DeductedAmount, obj.InvFreightVoucherDetailslist[j].ChargeToParty, obj.InvFreightVoucherDetailslist[j].TotalPayableAmount, obj.InvFreightVoucherDetailslist[j].RemarksDetail, obj.VarietyName, obj.GpQty, obj.InvFreightVoucherDetailslist[j].RateForShortage);
					Freight += obj.InvFreightVoucherDetailslist[j].BiltyFreight;
					WbCharges += obj.InvFreightVoucherDetailslist[j].ScaleCharges;
				}
				txtBiltyFreight.Text = Freight.ToString();
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings(obj.RefDocumentTypeId, Conversion.ToDouble((object)RateForShortage), CustomGroupId);
				dtFreightPaymentBreakUp.Clear();
				for (int i = 0; i < obj.InvFreightVoucherPaymentBreakupList.Count; i++)
				{
					dtFreightPaymentBreakUp.Rows.Add(obj.InvFreightVoucherPaymentBreakupList[i].Id, obj.InvFreightVoucherPaymentBreakupList[i].TransTypeId, obj.InvFreightVoucherPaymentBreakupList[i].TransactionType, obj.InvFreightVoucherPaymentBreakupList[i].InstrumentTypeId, obj.InvFreightVoucherPaymentBreakupList[i].InstrumentType, obj.InvFreightVoucherPaymentBreakupList[i].AccountId, obj.InvFreightVoucherPaymentBreakupList[i].AccountTitle, obj.InvFreightVoucherPaymentBreakupList[i].CheqId, obj.InvFreightVoucherPaymentBreakupList[i].CheqNo, obj.InvFreightVoucherPaymentBreakupList[i].CheqDate, obj.InvFreightVoucherPaymentBreakupList[i].Amount, obj.InvFreightVoucherPaymentBreakupList[i].PayeeTitle, obj.InvFreightVoucherPaymentBreakupList[i].Remarks);
				}
				FormHelper.LoadAttachmentsForObject(AT, RecId, ScreenName);
				PreviousCityDataBind(obj.CityId);
				if (obj.PolicyTypeId == 3)
				{
					txtAllowShortage.ReadOnly = false;
					txtAllowShortage.TabStop = true;
				}
				else
				{
					txtAllowShortage.ReadOnly = true;
					txtAllowShortage.TabStop = false;
				}
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
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RecId = 0;
			VoucherHeadId = 0;
			Save.Visible = true;
			Update.Visible = false;
			txtremarksmain.Text = string.Empty;
			txtManualNumber.Text = string.Empty;
			txtGpId.Text = string.Empty;
			txtGPNo.Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			txtBiltyNo.Text = string.Empty;
			txtBiltyFreight.Text = string.Empty;
			txtAllowShortage.Text = string.Empty;
			txtSupplierWeight.Text = string.Empty;
			txtFactoryWeight.Text = string.Empty;
			txtToleranceWeight.Text = string.Empty;
			txtShortWeight.Text = string.Empty;
			label17.Text = ((Conversion.ToDouble((object)txtShortWeight.Text) < 0.0) ? "Excess Weight" : "Short Weight");
			((Control)(object)CmbCity).Text = string.Empty;
			txtPartyName.Text = string.Empty;
			table.Rows.Clear();
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			dtFreightPaymentBreakUp.Rows.Clear();
			grdPreviousInformation.DataSource = null;
			txtvoucherno.Text = VoucherNoDbCall();
			PendingRecordBind(PendingRecordDbCall());
			CmbCashAccount.Focus();
			txtCNIC.Text = string.Empty;
			txtDriverCellNo.Text = string.Empty;
			txtWhatsAppNo.Text = string.Empty;
			txtAlternateCellNo.Text = string.Empty;
			txtDriverName.Text = string.Empty;
			txtFatherName.Text = string.Empty;
			txtFatherCNIC.Text = string.Empty;
			txtDriverCellNo.Text = "0092-3-";
			txtWhatsAppNo.Text = "0092-3-";
			txtAlternateCellNo.Text = "0092-3-";
			DriverFieldsDisableOrEnable(flag: true);
			clsGlobalVariables.globalAllDriverBioInfo = driverBiodata.ReadAll(UserAccount.OrganizationId, UserAccount.CompanyId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
		((Control)(object)CmbCashAccount).Text = string.Empty;
		lbldrcr.Visible = false;
	}

	private void FreightVoucher_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "AllCities" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			CashAccountFillFromGlobal();
			CityDtFillFromGlobalAndBind();
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
			_ = tabControl1.SelectedIndex;
			_ = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetRegister_Click(object sender, EventArgs e)
	{
		try
		{
			txtGpNoFrom.Text = string.Empty;
			txtGpNoTo.Text = string.Empty;
			txtVehicleRegister.Text = string.Empty;
			((Control)(object)CmbSupplierName).Text = string.Empty;
			dtRegister.Rows.Clear();
			grdRegister.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshRegister_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind(InvFreightVoucher.GetDataForDropDownFromFreightVoucher(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtRegister.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = datFromDate.Value;
			obj.ToDate = datToDate.Value;
			obj.GpSrNoF = Conversion.ToInt((object)txtGpNoFrom.Text);
			obj.GpSrNoT = Conversion.ToInt((object)txtGpNoTo.Text);
			obj.VehicleNo = Conversion.ToString((object)txtVehicleRegister.Text);
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplierName.Value);
			obj.CreditAccountId = Conversion.ToInt(CmbCreditAcRegister.Value);
			obj.DebitAccountId = Conversion.ToInt(CmbDebitAcRegister.Value);
			if (chkOnlyDiscountedRows.Checked)
			{
				obj.FreightDiscountAmount = 1.0;
			}
			if (chkFreightAuditbyCity.Checked)
			{
				obj.ActionId = 1;
			}
			DataSet ds = InvFreightVoucher.FreightVoucherSlipAndRegister(obj);
			dtRegister = ds?.Tables[0].Copy();
			if (dtRegister.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtRegister, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				if (dtRegisterGrid.Columns.Count == 0)
				{
					dtRegisterGrid.Columns.Add("Id", typeof(int));
					dtRegisterGrid.Columns.Add("VoucherHeadId", typeof(int));
					dtRegisterGrid.Columns.Add("DocDate", typeof(DateTime));
					dtRegisterGrid.Columns.Add("DocNo", typeof(int));
					dtRegisterGrid.Columns.Add("GpDate", typeof(DateTime));
					dtRegisterGrid.Columns.Add("GpNo", typeof(int));
					dtRegisterGrid.Columns.Add("SupplierName", typeof(string));
					dtRegisterGrid.Columns.Add("VehicleNo", typeof(string));
					dtRegisterGrid.Columns.Add("BiltyNo", typeof(string));
					dtRegisterGrid.Columns.Add("BiltyDate", typeof(DateTime));
					dtRegisterGrid.Columns.Add("DeliveryTerm", typeof(string));
					dtRegisterGrid.Columns.Add("SupplierWeight", typeof(double));
					dtRegisterGrid.Columns.Add("FactoryWeight", typeof(double));
					dtRegisterGrid.Columns.Add("ShortWeight", typeof(double));
					dtRegisterGrid.Columns.Add("AllowShortage", typeof(double));
					dtRegisterGrid.Columns.Add("ShortageApply", typeof(double));
					dtRegisterGrid.Columns.Add("ShortageAmount", typeof(double));
					dtRegisterGrid.Columns.Add("BiltyFreight", typeof(double));
					dtRegisterGrid.Columns.Add("DeductedAmount", typeof(double));
					dtRegisterGrid.Columns.Add("DiscountAmount", typeof(double));
					dtRegisterGrid.Columns.Add("ToleranceWeight", typeof(string));
					dtRegisterGrid.Columns.Add("WbCharges", typeof(double));
					dtRegisterGrid.Columns.Add("AdvanceByParty", typeof(double));
					dtRegisterGrid.Columns.Add("AdvanceByFactory", typeof(double));
					dtRegisterGrid.Columns.Add("TotalFreight", typeof(double));
					dtRegisterGrid.Columns.Add("ChargeToParty", typeof(double));
					dtRegisterGrid.Columns.Add("PaidAmount", typeof(double));
					dtRegisterGrid.Columns.Add("RemainingAmount", typeof(double));
					dtRegisterGrid.Columns.Add("PerMtonFreight", typeof(double));
					dtRegisterGrid.Columns.Add("PerMaundFreight", typeof(double));
					dtRegisterGrid.Columns.Add("Per100KgFreight", typeof(double));
					dtRegisterGrid.Columns.Add("CityName", typeof(string));
					dtRegisterGrid.Columns.Add("CashAccountCode", typeof(string));
					dtRegisterGrid.Columns.Add("CashAccountTitle", typeof(string));
					dtRegisterGrid.Columns.Add("DebitAccountCode", typeof(string));
					dtRegisterGrid.Columns.Add("DebitAccountTitle", typeof(string));
					dtRegisterGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtRegisterGrid.Columns.Add("EntryUser", typeof(string));
					dtRegisterGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtRegisterGrid.Columns.Add("ModifyUser", typeof(string));
					dtRegisterGrid.Columns.Add("ApprovedDate", typeof(DateTime));
					dtRegisterGrid.Columns.Add("ApprovalUser", typeof(string));
					dtRegisterGrid.Columns.Add("ApprovalStatus", typeof(string));
					dtRegisterGrid.Columns.Add("NoOfAttachments", typeof(int));
					dtRegisterGrid.Columns.Add("RemarksDetail", typeof(string));
					dtRegisterGrid.Columns.Add("RemarksHeader", typeof(string));
				}
				dtRegisterGrid.Clear();
				for (int i = 0; i < dtRegister.Rows.Count; i++)
				{
					dtRegisterGrid.Rows.Add(dtRegister.Rows[i]["Id"], dtRegister.Rows[i]["VoucherHeadId"], Conversion.ToDateTime(dtRegister.Rows[i]["DocDate"]).ToShortDateString(), dtRegister.Rows[i]["DocNo"], Conversion.ToDateTime(dtRegister.Rows[i]["GatePassDate"]).ToShortDateString(), dtRegister.Rows[i]["GatePassNo"], dtRegister.Rows[i]["SupplierName"], dtRegister.Rows[i]["VehicleNo"], dtRegister.Rows[i]["BiltyNo"], dtRegister.Rows[i]["BiltyDate"], dtRegister.Rows[i]["DeliveryTerm"], dtRegister.Rows[i]["SupplierWeight"], dtRegister.Rows[i]["FactoryWeight"], dtRegister.Rows[i]["ShortWeight"], dtRegister.Rows[i]["AllowShortage"], dtRegister.Rows[i]["ShortageApply"], dtRegister.Rows[i]["ShortageAmount"], dtRegister.Rows[i]["BiltyFreight"], dtRegister.Rows[i]["DeductedAmount"], dtRegister.Rows[i]["DiscountAmount"], dtRegister.Rows[i]["ToleranceWeight"], dtRegister.Rows[i]["ScaleCharges"], dtRegister.Rows[i]["AdvanceByParty"], dtRegister.Rows[i]["AdvanceByFactory"], dtRegister.Rows[i]["TotalFreight"], dtRegister.Rows[i]["ChargeToParty"], dtRegister.Rows[i]["PaidAmount"], dtRegister.Rows[i]["RemainingAmount"], dtRegister.Rows[i]["PerMtonFreight"], Conversion.ToDouble(dtRegister.Rows[i]["BiltyFreight"]) / Conversion.ToDouble(dtRegister.Rows[i]["FactoryWeight"]) * 40.0, Conversion.ToDouble(dtRegister.Rows[i]["BiltyFreight"]) / Conversion.ToDouble(dtRegister.Rows[i]["FactoryWeight"]) * 100.0, dtRegister.Rows[i]["CityName"], dtRegister.Rows[i]["CashAcCode"], dtRegister.Rows[i]["CashAcTitle"], dtRegister.Rows[i]["DebAcCode"], dtRegister.Rows[i]["DebAcTitle"], Conversion.ToDateTime(dtRegister.Rows[i]["EntryDate"]), dtRegister.Rows[i]["EntryUser"], dtRegister.Rows[i]["ModifyDate"], dtRegister.Rows[i]["ModifyUser"], dtRegister.Rows[i]["ApprovedDate"], dtRegister.Rows[i]["ApprovalUser"], dtRegister.Rows[i]["ApprovalStatus"], dtRegister.Rows[i]["NoOfAttachments"], dtRegister.Rows[i]["RemarksDetail"], dtRegister.Rows[i]["RemarksHeader"]);
				}
				grdRegister.DataSource = dtRegisterGrid;
				grdRegister.RetrieveStructure();
				RegisterGridSettings();
			}
			else
			{
				grdRegister.DataSource = null;
				grdRegister.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RegisterGridSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "VoucherHeadId", "RemainingAmount", "CashAccountCode", "DebitAccountCode" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdRegister, 3, 3, hiddenColumns);
			grdRegister.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdRegister.RootTable.Columns["EntryDate"].Width = 145;
			grdRegister.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdRegister.RootTable.Columns["ModifyDate"].Width = 145;
			grdRegister.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdRegister.RootTable.Columns["ApprovedDate"].Width = 145;
			grdRegister.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdRegister.RootTable.Columns["DocDate"].Width = 80;
			grdRegister.RootTable.Columns["DocNo"].Width = 55;
			grdRegister.RootTable.Columns["GpDate"].Width = 80;
			grdRegister.RootTable.Columns["GpNo"].Width = 55;
			grdRegister.RootTable.Columns["DeliveryTerm"].Width = 65;
			grdRegister.RootTable.Columns["VehicleNo"].Width = 75;
			grdRegister.RootTable.Columns["BiltyNo"].Width = 70;
			grdRegister.RootTable.Columns["SupplierName"].Width = 200;
			grdRegister.RootTable.Columns["SupplierWeight"].Width = 70;
			grdRegister.RootTable.Columns["FactoryWeight"].Width = 70;
			grdRegister.RootTable.Columns["ToleranceWeight"].Width = 70;
			grdRegister.RootTable.Columns["BiltyFreight"].Width = 70;
			grdRegister.RootTable.Columns["PerMtonFreight"].Width = 75;
			grdRegister.RootTable.Columns["WbCharges"].Width = 60;
			grdRegister.RootTable.Columns["AdvanceByParty"].Width = 70;
			grdRegister.RootTable.Columns["AdvanceByFactory"].Width = 70;
			grdRegister.RootTable.Columns["TotalFreight"].Width = 80;
			grdRegister.RootTable.Columns["ShortWeight"].Width = 65;
			grdRegister.RootTable.Columns["AllowShortage"].Width = 100;
			grdRegister.RootTable.Columns["ShortageAmount"].Width = 85;
			grdRegister.RootTable.Columns["ShortageApply"].Width = 65;
			grdRegister.RootTable.Columns["DeductedAmount"].Width = 80;
			grdRegister.RootTable.Columns["DiscountAmount"].Width = 80;
			grdRegister.RootTable.Columns["ChargeToParty"].Width = 80;
			grdRegister.RootTable.Columns["PaidAmount"].Width = 80;
			grdRegister.RootTable.Columns["CashAccountTitle"].Width = 150;
			grdRegister.RootTable.Columns["DebitAccountTitle"].Width = 150;
			grdRegister.RootTable.Columns["RemarksDetail"].Width = 150;
			grdRegister.RootTable.Columns["NoOfAttachments"].Width = 90;
			grdRegister.RootTable.Columns["ToleranceWeight"].Caption = "Weigh Bridge Tolerance";
			grdRegister.RootTable.Columns["ToleranceWeight"].TextAlignment = (TextAlignment)3;
			grdRegister.RootTable.Columns["DeductedAmount"].Caption = "Deduction From Freight";
			grdRegister.RootTable.Columns["PaidAmount"].Caption = "Net Payable Amount";
			grdRegister.RootTable.Columns["AdvanceByParty"].Caption = "Advance By Supplier";
			grdRegister.RootTable.Columns["ChargeToParty"].Caption = "Shortage Charge To Supplier";
			grdRegister.RootTable.Columns["AllowShortage"].Caption = "Against Policy Discount/Allowed Weight";
			grdRegister.RootTable.Columns["ShortageApply"].Caption = "Shortage Applied Weight";
			grdRegister.RootTable.Columns["ShortageAmount"].Caption = "Shortage Applied Amount";
			GridEX_Helper.AddButton(grdRegister, "Edit", "Edit", 50, 0);
			GridEX_Helper.AddButton(grdRegister, "Print", "Print", 50, 1);
			GridEX_Helper.AddButton(grdRegister, "Voucher", "Voucher", 70, 2);
			GridEX_Helper.AddButton(grdRegister, "AddAttachment", "Add Attachment", 105);
			grdRegister.FrozenColumns = 3;
			((Control)(object)grdRegister).Refresh();
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdRegister_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdRegister.CurrentRow;
			if (item != null && (int)item.RowType == 0 && Conversion.ToInt(item.Cells["NoOfAttachments"].Value) > 0)
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(item.Cells["Id"].Value), base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdRegister_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdRegister.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Edit")
				{
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ReadById(RecId);
				}
				else if (e.Column.Key == "Print")
				{
					FreightSlip241(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Voucher")
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Conversion.ToInt(r.Cells["VoucherHeadId"].Value));
				}
				else if (e.Column.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId, base.Name);
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

	private void grdRegister_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdRegister.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				RecId = Conversion.ToInt(item.Cells["Id"].Value);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grd, table);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdRegister, dtRegisterGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void toolStripButton10_Click(object sender, EventArgs e)
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
			CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip241_Click(object sender, EventArgs e)
	{
		try
		{
			FreightSlip241(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightSlip241(int PrintId)
	{
		try
		{
			CommonServices.FreightVoucherSlip241(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRegister_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtRegister == null || dtRegister.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtRegister, "240-FreightVoucherRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtGpNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGpNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAllowShortage_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			txtAllowShortage.Tag = txtAllowShortage.Text;
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentVoucherNew_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.U && Update.Visible && Update.Enabled)
			{
				Update_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnResetRegister_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.R && tabControl1.SelectedIndex == 1)
			{
				btnRefreshRegister_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					voucherdatetime.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					datFromDate.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdPendingRecord).Focus();
					}
					else if (((Control)(object)grdPendingRecord).Focused)
					{
						((Control)(object)grd).Focus();
					}
					else
					{
						((Control)(object)grdPendingRecord).Focus();
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)grdRegister).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					voucherdatetime.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					datFromDate.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				voucherdatetime.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				toolStripButton10_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formright.DoHavePrintRights)
			{
				btnSlip241_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2) && formright.DoHavePrintRights)
			{
				Print_Click(null, null);
			}
			if (e.Control && e.Alt)
			{
				btnShortCutKey_Click(null, null);
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
			dt.Rows.Add("Ctrl+S", "For Save in Form Tab And For Show History in History Tabs");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+1", "For Print Slip");
			dt.Rows.Add("Alt+2", "For Print Voucher");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Doc Date in Form tab and on From Date in history tab");
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

	private void grdPendingRecord_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdPendingRecord.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Control && e.KeyCode == Keys.Space && grdPendingRecord.CurrentColumn.Key == "Load")
			{
				LoadRecord(item);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdRegister_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			int ID = 0;
			GridEXRow item = grdRegister.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (grdRegister.GetRows().Count() > 0)
			{
				ID = Conversion.ToInt(item.Cells["Id"].Value);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				ReadById(ID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdRegister.CurrentColumn.Key == "Edit")
				{
					ReadById(ID);
				}
				else if (grdRegister.CurrentColumn.Key == "Print")
				{
					FreightSlip241(ID);
				}
				else if (grdRegister.CurrentColumn.Key == "Voucher")
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(ID);
				}
				else if (grdRegister.CurrentColumn.Key == "Attachment")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(ID, DocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightVoucher_Load(object sender, EventArgs e)
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(20, 20);
		base.AutoScrollMinSize = new Size(1200, 640);
	}

	private void txtAllowShortage_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtAllowShortage.ReadOnly)
			{
				return;
			}
			double ShortWeight = Conversion.ToDouble((object)txtShortWeight.Text);
			double AllowShortage = Conversion.ToDouble((object)txtAllowShortage.Text);
			if (ShortWeight > 0.0)
			{
				if (AllowShortage > ShortWeight)
				{
					txtAllowShortage.Text = "0";
					MessageBox.Show("Allow shortage cannot be greater than short weight please check....", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else
				{
					txtShortageApply.Text = (ShortWeight - AllowShortage).ToString("#,##0.###");
					RecalculateShortageAmount();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RecalculateShortageAmount()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow item in dataRows)
			{
				if (item != null && (int)item.RowType == 0)
				{
					double num = Conversion.ToDouble((object)txtShortageApply.Text);
					double OrderRate = Conversion.ToDouble(item.Cells["RateForShortage"].Value);
					double DeductionAmount = Conversion.ToDouble(item.Cells["DeductionAmount"].Value);
					double TotalFreight = Conversion.ToDouble(item.Cells["TotalFreight"].Value);
					double ShortageAmount = Math.Round(num / 40.0 * OrderRate, 0);
					item.BeginEdit();
					item.Cells["ShortageAmount"].Value = ShortageAmount;
					if (ShortageAmount == 0.0)
					{
						item.Cells["DiscountAmount"].Value = 0;
						item.Cells["RemainingAmount"].Value = 0;
						item.Cells["DeductionAmount"].Value = 0;
					}
					double Discount = Conversion.ToDouble(item.Cells["DiscountAmount"].Value);
					double Remaining = ShortageAmount - Discount;
					item.Cells["RemainingAmount"].Value = Remaining;
					item.Cells["ChargeToParty"].Value = Remaining - DeductionAmount;
					item.Cells["PaidAmount"].Value = TotalFreight - DeductionAmount;
					item.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DriverFieldsDisableOrEnable(bool flag)
	{
		try
		{
			txtCNIC.Enabled = flag;
			txtDriverCellNo.Enabled = flag;
			txtWhatsAppNo.Enabled = flag;
			txtAlternateCellNo.Enabled = flag;
			txtDriverName.Enabled = flag;
			txtFatherName.Enabled = flag;
			txtFatherCNIC.Enabled = flag;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ResetDriverFields()
	{
		try
		{
			DriverBioId = 0;
			txtCNIC.Text = string.Empty;
			txtDriverCellNo.Text = string.Empty;
			txtWhatsAppNo.Text = string.Empty;
			txtAlternateCellNo.Text = string.Empty;
			txtDriverName.Text = string.Empty;
			txtFatherName.Text = string.Empty;
			txtFatherCNIC.Text = string.Empty;
			txtDriverCellNo.Text = "0092-3-";
			txtWhatsAppNo.Text = "0092-3-";
			txtAlternateCellNo.Text = "0092-3-";
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtCNIC_Leave(object sender, EventArgs e)
	{
		try
		{
			DriverBioGlobal obj = clsGlobalVariables.globalAllDriverBioInfo.Find((DriverBioGlobal row) => row.CnicNo == txtCNIC.Text.Trim());
			if (obj != null)
			{
				DriverBioId = obj.Id;
				txtDriverCellNo.Text = obj.DriverCellNo;
				txtWhatsAppNo.Text = obj.WhatsappNo;
				txtAlternateCellNo.Text = obj.AlternateCellNo;
				txtDriverName.Text = obj.DriverName;
				txtFatherName.Text = obj.FatherName;
				txtFatherCNIC.Text = obj.FatherCnicNo;
				DriverFieldsDisableOrEnable(flag: false);
			}
			else
			{
				DriverFieldsDisableOrEnable(flag: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDriverCellNo_Leave(object sender, EventArgs e)
	{
		try
		{
			DriverBioGlobal obj = clsGlobalVariables.globalAllDriverBioInfo.Find((DriverBioGlobal row) => row.DriverCellNo == txtDriverCellNo.Text.Trim());
			if (obj != null)
			{
				DriverBioId = obj.Id;
				txtCNIC.Text = obj.CnicNo;
				txtWhatsAppNo.Text = obj.WhatsappNo;
				txtAlternateCellNo.Text = obj.AlternateCellNo;
				txtDriverName.Text = obj.DriverName;
				txtFatherName.Text = obj.FatherName;
				txtFatherCNIC.Text = obj.FatherCnicNo;
				DriverFieldsDisableOrEnable(flag: false);
			}
			else
			{
				DriverFieldsDisableOrEnable(flag: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetDriverInfo_Click(object sender, EventArgs e)
	{
		try
		{
			clsGlobalVariables.globalAllDriverBioInfo = driverBiodata.ReadAll(UserAccount.OrganizationId, UserAccount.CompanyId);
			ResetDriverFields();
			DriverFieldsDisableOrEnable(flag: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCNIC_Click(object sender, EventArgs e)
	{
		txtCNIC.Focus();
	}

	private void txtDriverCellNo_Click(object sender, EventArgs e)
	{
		txtDriverCellNo.Focus();
	}

	private void txtWhatsAppNo_Click(object sender, EventArgs e)
	{
		txtWhatsAppNo.Focus();
	}

	private void txtAlternateCellNo_Click(object sender, EventArgs e)
	{
		txtAlternateCellNo.Focus();
	}

	private void btnPaymentBreakUp_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount > 0)
			{
				double TotalPaidAmount = GridEX_Helper.GetColumnSum(grd, "PaidAmount");
				using FreightVoucherPaymentBreakUp frm = new FreightVoucherPaymentBreakUp(UserAccount, dtFreightPaymentBreakUp, TotalPaidAmount, VoucherHeadId);
				frm.BringToFront();
				frm.ShowDialog();
				return;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdCashAccount_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			CashAccountFillFromGlobal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdCreditAccount_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			CashAccountFillFromGlobal();
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
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Expected O, but got Unknown
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Expected O, but got Unknown
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Expected O, but got Unknown
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Expected O, but got Unknown
		//IL_11c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ce: Expected O, but got Unknown
		//IL_13ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1409: Expected O, but got Unknown
		//IL_529d: Unknown result type (might be due to invalid IL or missing references)
		//IL_52a7: Expected O, but got Unknown
		//IL_52b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_52be: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.FreightVoucher));
		GridEXLayout grdPendingRecord_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPreviousInformation_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		GridEXLayout grdRegister_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btn = new System.Windows.Forms.ToolStripButton();
		this.btnE = new System.Windows.Forms.ToolStripButton();
		this.btns = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.bt = new System.Windows.Forms.ToolStripButton();
		this.btnCopy = new System.Windows.Forms.ToolStripButton();
		this.btnPaste = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.grdPendingRecord = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.btnResetDriverInfo = new System.Windows.Forms.PictureBox();
		this.txtFatherCNIC = new System.Windows.Forms.MaskedTextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtWhatsAppNo = new System.Windows.Forms.MaskedTextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtAlternateCellNo = new System.Windows.Forms.MaskedTextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtFatherName = new System.Windows.Forms.TextBox();
		this.txtDriverCellNo = new System.Windows.Forms.MaskedTextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.txtDriverName = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.txtCNIC = new System.Windows.Forms.MaskedTextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdPreviousInformation = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label26 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtSupplierWeight = new System.Windows.Forms.TextBox();
		this.txtGpId = new System.Windows.Forms.TextBox();
		this.txtBiltyFreight = new System.Windows.Forms.TextBox();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.txtPartyName = new System.Windows.Forms.TextBox();
		this.txtShortWeight = new System.Windows.Forms.TextBox();
		this.txtShortageApply = new System.Windows.Forms.TextBox();
		this.txtAllowShortage = new System.Windows.Forms.TextBox();
		this.txtGPNo = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.txtBiltyNo = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtGPDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtManualNumber = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.btnPaymentBreakUp = new System.Windows.Forms.Button();
		this.txtbalance = new System.Windows.Forms.TextBox();
		this.txtvoucherno = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.voucherdatetime = new System.Windows.Forms.DateTimePicker();
		this.lbldrcr = new System.Windows.Forms.Label();
		this.txtremarksmain = new System.Windows.Forms.TextBox();
		this.CmbCashAccount = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.rdCreditAccount = new System.Windows.Forms.RadioButton();
		this.rdCashAccount = new System.Windows.Forms.RadioButton();
		this.PaymentVoucherLable = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.CmbCity = new UltraCombo();
		this.txtWeightToleranceAllowPerMTon = new System.Windows.Forms.TextBox();
		this.txtToleranceWeight = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.TsForm = new System.Windows.Forms.ToolStrip();
		this.New = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.Update = new System.Windows.Forms.ToolStripButton();
		this.Save = new System.Windows.Forms.ToolStripButton();
		this.btnSlip241 = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.BtnAttachtment = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.Register = new System.Windows.Forms.TabPage();
		this.grdRegister = new GridEX();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.chkFreightAuditbyCity = new System.Windows.Forms.CheckBox();
		this.chkOnlyDiscountedRows = new System.Windows.Forms.CheckBox();
		this.CmbDebitAcRegister = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbCreditAcRegister = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.CmbSupplierName = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtVehicleRegister = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtGpNoFrom = new System.Windows.Forms.TextBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtGpNoTo = new System.Windows.Forms.TextBox();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.panel23 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetRegister = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshRegister = new System.Windows.Forms.ToolStripButton();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingRecord).BeginInit();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel21.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnResetDriverInfo).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPreviousInformation).BeginInit();
		this.panel6.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCashAccount).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		this.TsForm.SuspendLayout();
		this.Register.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdRegister).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAcRegister).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAcRegister).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).BeginInit();
		this.panel23.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(718, 1);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1830, 27);
		this.miniToolStrip.TabIndex = 46;
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btn, this.btnE, this.btns, this.btnPrint, this.toolStripSeparator, this.bt, this.btnCopy, this.btnPaste, this.toolStripSeparator1, this.toolStripButton1,
			this.toolStripButton2, this.helpToolStripButton
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1830, 27);
		this.toolStrip1.TabIndex = 46;
		this.toolStrip1.Text = "toolStrip1";
		this.btn.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(55, 24);
		this.btn.Text = "&New";
		this.btnE.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnE.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnE.Name = "btnE";
		this.btnE.Size = new System.Drawing.Size(51, 24);
		this.btnE.Text = "&Edit";
		this.btns.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btns.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btns.Name = "btns";
		this.btns.Size = new System.Drawing.Size(55, 24);
		this.btns.Text = "&Save";
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(56, 24);
		this.btnPrint.Text = "&Print";
		this.bt.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.bt.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.bt.Name = "bt";
		this.bt.Size = new System.Drawing.Size(64, 24);
		this.bt.Text = "&Delete";
		this.btnCopy.Image = Architecture.WinApp.Properties.Resources.icons8_copy_16;
		this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCopy.Name = "btnCopy";
		this.btnCopy.Size = new System.Drawing.Size(59, 24);
		this.btnCopy.Text = "&Copy";
		this.btnPaste.Image = (System.Drawing.Image)resources.GetObject("btnPaste.Image");
		this.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPaste.Name = "btnPaste";
		this.btnPaste.Size = new System.Drawing.Size(59, 24);
		this.btnPaste.Text = "&Paste";
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(94, 24);
		this.toolStripButton1.Text = "&Attachment";
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(54, 24);
		this.toolStripButton2.Text = "&SMS";
		this.helpToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.helpToolStripButton.Name = "helpToolStripButton";
		this.helpToolStripButton.Size = new System.Drawing.Size(56, 24);
		this.helpToolStripButton.Text = "He&lp";
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1294, 692);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.Register);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1294, 692);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.Form.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingRecord);
		this.Form.Controls.Add(this.panel15);
		this.Form.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.Form.Controls.Add(this.panel21);
		this.Form.Controls.Add(this.panel5);
		this.Form.Controls.Add(this.panel4);
		this.Form.Controls.Add(this.TsForm);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Margin = new System.Windows.Forms.Padding(2);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(2);
		this.Form.Size = new System.Drawing.Size(1286, 663);
		this.Form.TabIndex = 0;
		this.Form.Text = "Form";
		this.Form.UseVisualStyleBackColor = true;
		this.grdPendingRecord.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPendingRecord).BackColor = System.Drawing.Color.White;
		grdPendingRecord_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingRecord.DesignTimeLayout = grdPendingRecord_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingRecord.FilterMode = (FilterMode)2;
		this.grdPendingRecord.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingRecord.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).Location = new System.Drawing.Point(2, 500);
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).Name = "grdPendingRecord";
		((ScrollableBase)this.grdPendingRecord).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).Size = new System.Drawing.Size(1282, 161);
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).TabStop = false;
		this.grdPendingRecord.TotalRow = (InheritableBoolean)1;
		this.grdPendingRecord.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingRecord).VisualStyle = (VisualStyle)4;
		this.grdPendingRecord.ColumnButtonClick += new ColumnActionEventHandler(grdPendingRecord_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPendingRecord).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPendingRecord_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label11);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(2, 474);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1282, 26);
		this.panel15.TabIndex = 0;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(4, 3);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(133, 18);
		this.label11.TabIndex = 9;
		this.label11.Text = "Pending Records";
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(2, 307);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1282, 167);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.label14);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(2, 285);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1282, 22);
		this.panel21.TabIndex = 1;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(2, 2);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(53, 18);
		this.label14.TabIndex = 10;
		this.label14.Text = "Detail";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.panel2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 57);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1282, 228);
		this.panel5.TabIndex = 0;
		this.panel7.Controls.Add(this.groupBox4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 164);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1282, 60);
		this.panel7.TabIndex = 3;
		this.groupBox4.Controls.Add(this.label36);
		this.groupBox4.Controls.Add(this.label50);
		this.groupBox4.Controls.Add(this.btnResetDriverInfo);
		this.groupBox4.Controls.Add(this.txtFatherCNIC);
		this.groupBox4.Controls.Add(this.label35);
		this.groupBox4.Controls.Add(this.txtWhatsAppNo);
		this.groupBox4.Controls.Add(this.label34);
		this.groupBox4.Controls.Add(this.txtAlternateCellNo);
		this.groupBox4.Controls.Add(this.label32);
		this.groupBox4.Controls.Add(this.label33);
		this.groupBox4.Controls.Add(this.txtFatherName);
		this.groupBox4.Controls.Add(this.txtDriverCellNo);
		this.groupBox4.Controls.Add(this.label31);
		this.groupBox4.Controls.Add(this.txtDriverName);
		this.groupBox4.Controls.Add(this.label30);
		this.groupBox4.Controls.Add(this.txtCNIC);
		this.groupBox4.Controls.Add(this.label29);
		this.groupBox4.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(0, 0);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1230, 60);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Driver Information";
		this.label36.AutoSize = true;
		this.label36.BackColor = System.Drawing.Color.White;
		this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(745, 13);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(13, 16);
		this.label36.TabIndex = 107;
		this.label36.Text = "*";
		this.label50.AutoSize = true;
		this.label50.BackColor = System.Drawing.Color.White;
		this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(254, 13);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(13, 16);
		this.label50.TabIndex = 106;
		this.label50.Text = "*";
		this.btnResetDriverInfo.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnResetDriverInfo.Location = new System.Drawing.Point(156, 9);
		this.btnResetDriverInfo.Name = "btnResetDriverInfo";
		this.btnResetDriverInfo.Size = new System.Drawing.Size(20, 20);
		this.btnResetDriverInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.btnResetDriverInfo.TabIndex = 85;
		this.btnResetDriverInfo.TabStop = false;
		this.btnResetDriverInfo.Click += new System.EventHandler(btnResetDriverInfo_Click);
		this.txtFatherCNIC.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFatherCNIC.Location = new System.Drawing.Point(1002, 31);
		this.txtFatherCNIC.Margin = new System.Windows.Forms.Padding(2);
		this.txtFatherCNIC.Mask = "00000-0000000-0";
		this.txtFatherCNIC.Name = "txtFatherCNIC";
		this.txtFatherCNIC.Size = new System.Drawing.Size(161, 23);
		this.txtFatherCNIC.TabIndex = 6;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(1002, 13);
		this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(70, 15);
		this.label35.TabIndex = 81;
		this.label35.Text = "Father CNIC";
		this.txtWhatsAppNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWhatsAppNo.Location = new System.Drawing.Point(345, 31);
		this.txtWhatsAppNo.Margin = new System.Windows.Forms.Padding(2);
		this.txtWhatsAppNo.Mask = "000-000-0000000";
		this.txtWhatsAppNo.Name = "txtWhatsAppNo";
		this.txtWhatsAppNo.Size = new System.Drawing.Size(149, 23);
		this.txtWhatsAppNo.TabIndex = 2;
		this.txtWhatsAppNo.Text = "0923";
		this.txtWhatsAppNo.Click += new System.EventHandler(txtWhatsAppNo_Click);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(346, 13);
		this.label34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(73, 15);
		this.label34.TabIndex = 75;
		this.label34.Text = "WhatsApp #";
		this.txtAlternateCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAlternateCellNo.Location = new System.Drawing.Point(495, 31);
		this.txtAlternateCellNo.Margin = new System.Windows.Forms.Padding(2);
		this.txtAlternateCellNo.Mask = "000-000-0000000";
		this.txtAlternateCellNo.Name = "txtAlternateCellNo";
		this.txtAlternateCellNo.Size = new System.Drawing.Size(168, 23);
		this.txtAlternateCellNo.TabIndex = 3;
		this.txtAlternateCellNo.Text = "0923";
		this.txtAlternateCellNo.Click += new System.EventHandler(txtAlternateCellNo_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(495, 13);
		this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(84, 15);
		this.label32.TabIndex = 79;
		this.label32.Text = "Alternate Cell#";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(833, 13);
		this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(75, 15);
		this.label33.TabIndex = 78;
		this.label33.Text = "Father Name";
		this.txtFatherName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFatherName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFatherName.Location = new System.Drawing.Point(833, 31);
		this.txtFatherName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtFatherName.Name = "txtFatherName";
		this.txtFatherName.Size = new System.Drawing.Size(168, 23);
		this.txtFatherName.TabIndex = 5;
		this.txtDriverCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverCellNo.Location = new System.Drawing.Point(176, 31);
		this.txtDriverCellNo.Margin = new System.Windows.Forms.Padding(2);
		this.txtDriverCellNo.Mask = "000-000-0000000";
		this.txtDriverCellNo.Name = "txtDriverCellNo";
		this.txtDriverCellNo.Size = new System.Drawing.Size(168, 23);
		this.txtDriverCellNo.TabIndex = 1;
		this.txtDriverCellNo.Text = "0923";
		this.txtDriverCellNo.Click += new System.EventHandler(txtDriverCellNo_Click);
		this.txtDriverCellNo.Leave += new System.EventHandler(txtDriverCellNo_Leave);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(176, 13);
		this.label31.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(71, 15);
		this.label31.TabIndex = 75;
		this.label31.Text = "Driver Cell #";
		this.txtDriverName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverName.Location = new System.Drawing.Point(664, 31);
		this.txtDriverName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtDriverName.Name = "txtDriverName";
		this.txtDriverName.Size = new System.Drawing.Size(168, 23);
		this.txtDriverName.TabIndex = 4;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(664, 13);
		this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(74, 15);
		this.label30.TabIndex = 73;
		this.label30.Text = "Driver Name";
		this.txtCNIC.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCNIC.Location = new System.Drawing.Point(7, 31);
		this.txtCNIC.Margin = new System.Windows.Forms.Padding(2);
		this.txtCNIC.Mask = "00000-0000000-0";
		this.txtCNIC.Name = "txtCNIC";
		this.txtCNIC.Size = new System.Drawing.Size(168, 23);
		this.txtCNIC.TabIndex = 0;
		this.txtCNIC.Click += new System.EventHandler(txtCNIC_Click);
		this.txtCNIC.Leave += new System.EventHandler(txtCNIC_Leave);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(7, 13);
		this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(41, 15);
		this.label29.TabIndex = 71;
		this.label29.Text = "CNIC#";
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1282, 164);
		this.panel2.TabIndex = 2;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdPreviousInformation);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(690, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(540, 164);
		this.panel3.TabIndex = 0;
		this.grdPreviousInformation.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPreviousInformation).BackColor = System.Drawing.Color.White;
		grdPreviousInformation_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPreviousInformation.DesignTimeLayout = grdPreviousInformation_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPreviousInformation).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPreviousInformation).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPreviousInformation.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPreviousInformation).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdPreviousInformation).Name = "grdPreviousInformation";
		((ScrollableBase)this.grdPreviousInformation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPreviousInformation).Size = new System.Drawing.Size(540, 142);
		((System.Windows.Forms.Control)(object)this.grdPreviousInformation).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdPreviousInformation).TabStop = false;
		this.grdPreviousInformation.TotalRow = (InheritableBoolean)1;
		this.grdPreviousInformation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPreviousInformation).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.label26);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(540, 22);
		this.panel6.TabIndex = 3;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Teal;
		this.label26.Location = new System.Drawing.Point(2, 2);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(170, 18);
		this.label26.TabIndex = 10;
		this.label26.Text = "Previous Data By City";
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.txtSupplierWeight);
		this.groupBox1.Controls.Add(this.txtGpId);
		this.groupBox1.Controls.Add(this.txtBiltyFreight);
		this.groupBox1.Controls.Add(this.txtFactoryWeight);
		this.groupBox1.Controls.Add(this.txtPartyName);
		this.groupBox1.Controls.Add(this.txtShortWeight);
		this.groupBox1.Controls.Add(this.txtShortageApply);
		this.groupBox1.Controls.Add(this.txtAllowShortage);
		this.groupBox1.Controls.Add(this.txtGPNo);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.txtBiltyNo);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtGPDate);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(300, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(390, 164);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "GatePass Detail";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(191, 113);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(89, 15);
		this.label16.TabIndex = 47;
		this.label16.Text = "Shortage Apply";
		this.txtSupplierWeight.BackColor = System.Drawing.Color.White;
		this.txtSupplierWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierWeight.Location = new System.Drawing.Point(287, 14);
		this.txtSupplierWeight.Name = "txtSupplierWeight";
		this.txtSupplierWeight.ReadOnly = true;
		this.txtSupplierWeight.Size = new System.Drawing.Size(100, 23);
		this.txtSupplierWeight.TabIndex = 5;
		this.txtSupplierWeight.TabStop = false;
		this.txtGpId.BackColor = System.Drawing.Color.White;
		this.txtGpId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpId.Location = new System.Drawing.Point(50, 15);
		this.txtGpId.Name = "txtGpId";
		this.txtGpId.ReadOnly = true;
		this.txtGpId.Size = new System.Drawing.Size(17, 21);
		this.txtGpId.TabIndex = 51;
		this.txtGpId.TabStop = false;
		this.txtGpId.Visible = false;
		this.txtBiltyFreight.BackColor = System.Drawing.Color.White;
		this.txtBiltyFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyFreight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyFreight.Location = new System.Drawing.Point(76, 110);
		this.txtBiltyFreight.Name = "txtBiltyFreight";
		this.txtBiltyFreight.ReadOnly = true;
		this.txtBiltyFreight.Size = new System.Drawing.Size(110, 23);
		this.txtBiltyFreight.TabIndex = 4;
		this.txtBiltyFreight.TabStop = false;
		this.txtFactoryWeight.BackColor = System.Drawing.Color.White;
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFactoryWeight.Location = new System.Drawing.Point(287, 38);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.ReadOnly = true;
		this.txtFactoryWeight.Size = new System.Drawing.Size(100, 23);
		this.txtFactoryWeight.TabIndex = 6;
		this.txtFactoryWeight.TabStop = false;
		this.txtPartyName.BackColor = System.Drawing.Color.White;
		this.txtPartyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPartyName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPartyName.Location = new System.Drawing.Point(76, 135);
		this.txtPartyName.Name = "txtPartyName";
		this.txtPartyName.ReadOnly = true;
		this.txtPartyName.Size = new System.Drawing.Size(311, 23);
		this.txtPartyName.TabIndex = 10;
		this.txtPartyName.TabStop = false;
		this.txtShortWeight.BackColor = System.Drawing.Color.White;
		this.txtShortWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtShortWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShortWeight.Location = new System.Drawing.Point(287, 62);
		this.txtShortWeight.Name = "txtShortWeight";
		this.txtShortWeight.ReadOnly = true;
		this.txtShortWeight.Size = new System.Drawing.Size(100, 23);
		this.txtShortWeight.TabIndex = 7;
		this.txtShortWeight.TabStop = false;
		this.txtShortageApply.BackColor = System.Drawing.Color.White;
		this.txtShortageApply.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtShortageApply.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShortageApply.Location = new System.Drawing.Point(287, 111);
		this.txtShortageApply.Name = "txtShortageApply";
		this.txtShortageApply.ReadOnly = true;
		this.txtShortageApply.Size = new System.Drawing.Size(100, 23);
		this.txtShortageApply.TabIndex = 9;
		this.txtShortageApply.TabStop = false;
		this.txtAllowShortage.BackColor = System.Drawing.Color.White;
		this.txtAllowShortage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAllowShortage.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAllowShortage.Location = new System.Drawing.Point(287, 86);
		this.txtAllowShortage.Name = "txtAllowShortage";
		this.txtAllowShortage.ReadOnly = true;
		this.txtAllowShortage.Size = new System.Drawing.Size(100, 23);
		this.txtAllowShortage.TabIndex = 9;
		this.txtAllowShortage.TabStop = false;
		this.txtAllowShortage.TextChanged += new System.EventHandler(txtAllowShortage_TextChanged);
		this.txtAllowShortage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAllowShortage_KeyPress);
		this.txtGPNo.BackColor = System.Drawing.Color.White;
		this.txtGPNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGPNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGPNo.Location = new System.Drawing.Point(76, 14);
		this.txtGPNo.Name = "txtGPNo";
		this.txtGPNo.ReadOnly = true;
		this.txtGPNo.Size = new System.Drawing.Size(110, 23);
		this.txtGPNo.TabIndex = 0;
		this.txtGPNo.TabStop = false;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(191, 18);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(93, 15);
		this.label12.TabIndex = 40;
		this.label12.Text = "Supplier Weight";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(191, 89);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(88, 15);
		this.label23.TabIndex = 50;
		this.label23.Text = "Allow Shortage";
		this.txtBiltyNo.BackColor = System.Drawing.Color.White;
		this.txtBiltyNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyNo.Location = new System.Drawing.Point(76, 86);
		this.txtBiltyNo.Name = "txtBiltyNo";
		this.txtBiltyNo.ReadOnly = true;
		this.txtBiltyNo.Size = new System.Drawing.Size(110, 23);
		this.txtBiltyNo.TabIndex = 3;
		this.txtBiltyNo.TabStop = false;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(6, 90);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(49, 15);
		this.label9.TabIndex = 38;
		this.label9.Text = "Bilty No";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(191, 42);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(88, 15);
		this.label13.TabIndex = 42;
		this.label13.Text = "Factory Weight";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(6, 114);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(67, 15);
		this.label21.TabIndex = 48;
		this.label21.Text = "BiltyFreight";
		this.txtVehicleNo.BackColor = System.Drawing.Color.White;
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(76, 62);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.ReadOnly = true;
		this.txtVehicleNo.Size = new System.Drawing.Size(110, 23);
		this.txtVehicleNo.TabIndex = 2;
		this.txtVehicleNo.TabStop = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(6, 66);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(64, 15);
		this.label8.TabIndex = 36;
		this.label8.Text = "Vehicle No";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(6, 139);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(69, 15);
		this.label27.TabIndex = 44;
		this.label27.Text = "Party Name";
		this.txtGPDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGPDate.CustomFormat = "dd-MMM-yy";
		this.txtGPDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtGPDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGPDate.Location = new System.Drawing.Point(76, 38);
		this.txtGPDate.Name = "txtGPDate";
		this.txtGPDate.Size = new System.Drawing.Size(110, 22);
		this.txtGPDate.TabIndex = 1;
		this.txtGPDate.TabStop = false;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(191, 66);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(78, 15);
		this.label17.TabIndex = 44;
		this.label17.Text = "Short Weight";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(6, 18);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(41, 15);
		this.label4.TabIndex = 32;
		this.label4.Text = "GP No";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(6, 42);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(50, 15);
		this.label6.TabIndex = 34;
		this.label6.Text = "Gp Date";
		this.groupBox2.BackColor = System.Drawing.Color.White;
		this.groupBox2.Controls.Add(this.txtManualNumber);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.btnPaymentBreakUp);
		this.groupBox2.Controls.Add(this.txtbalance);
		this.groupBox2.Controls.Add(this.txtvoucherno);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.voucherdatetime);
		this.groupBox2.Controls.Add(this.lbldrcr);
		this.groupBox2.Controls.Add(this.txtremarksmain);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCashAccount);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(300, 164);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.txtManualNumber.BackColor = System.Drawing.Color.White;
		this.txtManualNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManualNumber.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManualNumber.Location = new System.Drawing.Point(100, 89);
		this.txtManualNumber.Name = "txtManualNumber";
		this.txtManualNumber.Size = new System.Drawing.Size(197, 23);
		this.txtManualNumber.TabIndex = 4;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(3, 93);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(94, 15);
		this.label37.TabIndex = 53;
		this.label37.Text = "Manual Number";
		this.btnPaymentBreakUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnPaymentBreakUp.Location = new System.Drawing.Point(277, 67);
		this.btnPaymentBreakUp.Name = "btnPaymentBreakUp";
		this.btnPaymentBreakUp.Size = new System.Drawing.Size(20, 20);
		this.btnPaymentBreakUp.TabIndex = 29;
		this.btnPaymentBreakUp.TabStop = false;
		this.btnPaymentBreakUp.Text = "+";
		this.btnPaymentBreakUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.btnPaymentBreakUp.UseVisualStyleBackColor = true;
		this.btnPaymentBreakUp.Click += new System.EventHandler(btnPaymentBreakUp_Click);
		this.txtbalance.BackColor = System.Drawing.Color.White;
		this.txtbalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbalance.Location = new System.Drawing.Point(100, 65);
		this.txtbalance.Name = "txtbalance";
		this.txtbalance.ReadOnly = true;
		this.txtbalance.Size = new System.Drawing.Size(144, 23);
		this.txtbalance.TabIndex = 3;
		this.txtbalance.TabStop = false;
		this.txtbalance.Text = "0";
		this.txtbalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtvoucherno.BackColor = System.Drawing.Color.White;
		this.txtvoucherno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvoucherno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvoucherno.Location = new System.Drawing.Point(100, 12);
		this.txtvoucherno.Name = "txtvoucherno";
		this.txtvoucherno.ReadOnly = true;
		this.txtvoucherno.Size = new System.Drawing.Size(75, 23);
		this.txtvoucherno.TabIndex = 0;
		this.txtvoucherno.TabStop = false;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(3, 16);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(70, 15);
		this.label15.TabIndex = 0;
		this.label15.Text = "Voucher No";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 115);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 6;
		this.label1.Text = "Remarks";
		this.voucherdatetime.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.voucherdatetime.CustomFormat = "dd-MMM-yy";
		this.voucherdatetime.Font = new System.Drawing.Font("Verdana", 9f);
		this.voucherdatetime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.voucherdatetime.Location = new System.Drawing.Point(176, 12);
		this.voucherdatetime.Name = "voucherdatetime";
		this.voucherdatetime.Size = new System.Drawing.Size(121, 22);
		this.voucherdatetime.TabIndex = 1;
		this.lbldrcr.AutoSize = true;
		this.lbldrcr.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbldrcr.Location = new System.Drawing.Point(247, 70);
		this.lbldrcr.Margin = new System.Windows.Forms.Padding(0);
		this.lbldrcr.Name = "lbldrcr";
		this.lbldrcr.Size = new System.Drawing.Size(25, 15);
		this.lbldrcr.TabIndex = 28;
		this.lbldrcr.Text = "Nill";
		this.lbldrcr.Visible = false;
		this.txtremarksmain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksmain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksmain.Location = new System.Drawing.Point(100, 114);
		this.txtremarksmain.Multiline = true;
		this.txtremarksmain.Name = "txtremarksmain";
		this.txtremarksmain.Size = new System.Drawing.Size(197, 41);
		this.txtremarksmain.TabIndex = 5;
		this.CmbCashAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCashAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCashAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCashAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCashAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbCashAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCashAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCashAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCashAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCashAccount).Location = new System.Drawing.Point(100, 37);
		((System.Windows.Forms.Control)(object)this.CmbCashAccount).Name = "CmbCashAccount";
		((System.Windows.Forms.Control)(object)this.CmbCashAccount).Size = new System.Drawing.Size(197, 26);
		((System.Windows.Forms.Control)(object)this.CmbCashAccount).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCashAccount).Leave += new System.EventHandler(combcreditac_Leave);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(3, 69);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(96, 15);
		this.label25.TabIndex = 4;
		this.label25.Text = "Account Balance";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(3, 43);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(100, 15);
		this.label2.TabIndex = 4;
		this.label2.Text = "Account Title (Cr)";
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.rdCreditAccount);
		this.panel4.Controls.Add(this.rdCashAccount);
		this.panel4.Controls.Add(this.PaymentVoucherLable);
		this.panel4.Controls.Add(this.ChkBox);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.panel4.Controls.Add(this.txtWeightToleranceAllowPerMTon);
		this.panel4.Controls.Add(this.txtToleranceWeight);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.label28);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 29);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1282, 28);
		this.panel4.TabIndex = 0;
		this.rdCreditAccount.AutoSize = true;
		this.rdCreditAccount.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdCreditAccount.ForeColor = System.Drawing.Color.White;
		this.rdCreditAccount.Location = new System.Drawing.Point(338, 2);
		this.rdCreditAccount.Name = "rdCreditAccount";
		this.rdCreditAccount.Size = new System.Drawing.Size(69, 24);
		this.rdCreditAccount.TabIndex = 53;
		this.rdCreditAccount.Text = "Credit";
		this.rdCreditAccount.UseVisualStyleBackColor = true;
		this.rdCreditAccount.CheckedChanged += new System.EventHandler(rdCreditAccount_CheckedChanged);
		this.rdCashAccount.AutoSize = true;
		this.rdCashAccount.Checked = true;
		this.rdCashAccount.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdCashAccount.ForeColor = System.Drawing.Color.White;
		this.rdCashAccount.Location = new System.Drawing.Point(264, 2);
		this.rdCashAccount.Name = "rdCashAccount";
		this.rdCashAccount.Size = new System.Drawing.Size(60, 24);
		this.rdCashAccount.TabIndex = 53;
		this.rdCashAccount.TabStop = true;
		this.rdCashAccount.Text = "Cash";
		this.rdCashAccount.UseVisualStyleBackColor = true;
		this.rdCashAccount.CheckedChanged += new System.EventHandler(rdCashAccount_CheckedChanged);
		this.PaymentVoucherLable.AutoSize = true;
		this.PaymentVoucherLable.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.PaymentVoucherLable.ForeColor = System.Drawing.Color.White;
		this.PaymentVoucherLable.Location = new System.Drawing.Point(2, 5);
		this.PaymentVoucherLable.Name = "PaymentVoucherLable";
		this.PaymentVoucherLable.Size = new System.Drawing.Size(215, 19);
		this.PaymentVoucherLable.TabIndex = 0;
		this.PaymentVoucherLable.Text = "Freight Payment Voucher";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Checked = true;
		this.ChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkBox.Location = new System.Drawing.Point(1157, 4);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(122, 20);
		this.ChkBox.TabIndex = 8;
		this.ChkBox.TabStop = false;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(993, 1);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		this.CmbCity.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(155, 26);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.CmbCity).TabStop = false;
		this.txtWeightToleranceAllowPerMTon.BackColor = System.Drawing.Color.White;
		this.txtWeightToleranceAllowPerMTon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightToleranceAllowPerMTon.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightToleranceAllowPerMTon.Location = new System.Drawing.Point(659, 3);
		this.txtWeightToleranceAllowPerMTon.Name = "txtWeightToleranceAllowPerMTon";
		this.txtWeightToleranceAllowPerMTon.ReadOnly = true;
		this.txtWeightToleranceAllowPerMTon.Size = new System.Drawing.Size(65, 23);
		this.txtWeightToleranceAllowPerMTon.TabIndex = 8;
		this.txtWeightToleranceAllowPerMTon.TabStop = false;
		this.txtWeightToleranceAllowPerMTon.Visible = false;
		this.txtToleranceWeight.BackColor = System.Drawing.Color.White;
		this.txtToleranceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToleranceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToleranceWeight.Location = new System.Drawing.Point(838, 3);
		this.txtToleranceWeight.Name = "txtToleranceWeight";
		this.txtToleranceWeight.ReadOnly = true;
		this.txtToleranceWeight.Size = new System.Drawing.Size(65, 23);
		this.txtToleranceWeight.TabIndex = 8;
		this.txtToleranceWeight.TabStop = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(909, 7);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(75, 14);
		this.label7.TabIndex = 52;
		this.label7.Text = "City Name";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(522, 0);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(127, 28);
		this.label28.TabIndex = 46;
		this.label28.Text = "Weight Tolerance \r\nAllow Per MTon";
		this.label28.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(729, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(100, 28);
		this.label18.TabIndex = 46;
		this.label18.Text = "Weigh Bridge \r\nTolerance";
		this.TsForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TsForm.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.TsForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.New, this.BtnRefresh, this.Update, this.Save, this.btnSlip241, this.Print, this.BtnAttachtment, this.btnShortCutKey });
		this.TsForm.Location = new System.Drawing.Point(2, 2);
		this.TsForm.Name = "TsForm";
		this.TsForm.Size = new System.Drawing.Size(1282, 27);
		this.TsForm.TabIndex = 0;
		this.TsForm.Text = "toolStrip2";
		this.New.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.New.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.New.Name = "New";
		this.New.Size = new System.Drawing.Size(59, 24);
		this.New.Text = "&New";
		this.New.Click += new System.EventHandler(btnNew_Click);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "&Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.Update.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.Update.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Update.Name = "Update";
		this.Update.Size = new System.Drawing.Size(78, 24);
		this.Update.Text = "&Update";
		this.Update.Visible = false;
		this.Update.Click += new System.EventHandler(Update_Click);
		this.Save.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Save.Name = "Save";
		this.Save.Size = new System.Drawing.Size(64, 24);
		this.Save.Text = "&Save";
		this.Save.Click += new System.EventHandler(Save_Click);
		this.btnSlip241.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip241.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip241.Name = "btnSlip241";
		this.btnSlip241.Size = new System.Drawing.Size(85, 24);
		this.btnSlip241.Text = "241-Slip";
		this.btnSlip241.Click += new System.EventHandler(btnSlip241_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(115, 24);
		this.Print.Text = "102-Voucher";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.BtnAttachtment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.BtnAttachtment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnAttachtment.Name = "BtnAttachtment";
		this.BtnAttachtment.Size = new System.Drawing.Size(109, 24);
		this.BtnAttachtment.Text = "&Attachment";
		this.BtnAttachtment.Click += new System.EventHandler(toolStripButton10_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.Register.Controls.Add((System.Windows.Forms.Control)(object)this.grdRegister);
		this.Register.Controls.Add(this.groupBox3);
		this.Register.Controls.Add(this.panel23);
		this.Register.Controls.Add(this.toolStrip3);
		this.Register.Location = new System.Drawing.Point(4, 4);
		this.Register.Name = "Register";
		this.Register.Size = new System.Drawing.Size(1286, 663);
		this.Register.TabIndex = 2;
		this.Register.Text = "Register";
		this.Register.UseVisualStyleBackColor = true;
		this.grdRegister.AllowEdit = (InheritableBoolean)2;
		this.grdRegister.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdRegister_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdRegister.DesignTimeLayout = grdRegister_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdRegister).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdRegister.DynamicFiltering = true;
		this.grdRegister.FilterMode = (FilterMode)2;
		this.grdRegister.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdRegister).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdRegister.GroupTotals = (GroupTotals)2;
		this.grdRegister.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdRegister).Location = new System.Drawing.Point(0, 129);
		((System.Windows.Forms.Control)(object)this.grdRegister).Name = "grdRegister";
		((ScrollableBase)this.grdRegister).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdRegister).Size = new System.Drawing.Size(1286, 534);
		((System.Windows.Forms.Control)(object)this.grdRegister).TabIndex = 2;
		this.grdRegister.TotalRow = (InheritableBoolean)1;
		this.grdRegister.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdRegister).VisualStyle = (VisualStyle)4;
		this.grdRegister.ColumnButtonClick += new ColumnActionEventHandler(grdRegister_ColumnButtonClick);
		this.grdRegister.LinkClicked += new ColumnActionEventHandler(grdRegister_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdRegister).DoubleClick += new System.EventHandler(grdRegister_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdRegister).KeyDown += new System.Windows.Forms.KeyEventHandler(grdRegister_KeyDown);
		this.groupBox3.Controls.Add(this.chkFreightAuditbyCity);
		this.groupBox3.Controls.Add(this.chkOnlyDiscountedRows);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAcRegister);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAcRegister);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add(this.datFromDate);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierName);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.label5);
		this.groupBox3.Controls.Add(this.txtVehicleRegister);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.txtGpNoFrom);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.txtGpNoTo);
		this.groupBox3.Controls.Add(this.datToDate);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(0, 54);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1286, 75);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.chkFreightAuditbyCity.AutoSize = true;
		this.chkFreightAuditbyCity.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkFreightAuditbyCity.Location = new System.Drawing.Point(1019, 43);
		this.chkFreightAuditbyCity.Name = "chkFreightAuditbyCity";
		this.chkFreightAuditbyCity.Size = new System.Drawing.Size(139, 19);
		this.chkFreightAuditbyCity.TabIndex = 9;
		this.chkFreightAuditbyCity.Text = "Freight Audit by City";
		this.chkFreightAuditbyCity.UseVisualStyleBackColor = true;
		this.chkOnlyDiscountedRows.AutoSize = true;
		this.chkOnlyDiscountedRows.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkOnlyDiscountedRows.Location = new System.Drawing.Point(1019, 15);
		this.chkOnlyDiscountedRows.Name = "chkOnlyDiscountedRows";
		this.chkOnlyDiscountedRows.Size = new System.Drawing.Size(228, 19);
		this.chkOnlyDiscountedRows.TabIndex = 8;
		this.chkOnlyDiscountedRows.Text = "Show Only Freight-Discounted Rows";
		this.chkOnlyDiscountedRows.UseVisualStyleBackColor = true;
		this.CmbDebitAcRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAcRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAcRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAcRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAcRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAcRegister).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDebitAcRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAcRegister).Location = new System.Drawing.Point(763, 41);
		((System.Windows.Forms.Control)(object)this.CmbDebitAcRegister).Name = "CmbDebitAcRegister";
		((System.Windows.Forms.Control)(object)this.CmbDebitAcRegister).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAcRegister).TabIndex = 7;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(671, 47);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(84, 15);
		this.label22.TabIndex = 167;
		this.label22.Text = "Debit Account";
		this.CmbCreditAcRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAcRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAcRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAcRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAcRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAcRegister).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAcRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAcRegister).Location = new System.Drawing.Point(763, 13);
		((System.Windows.Forms.Control)(object)this.CmbCreditAcRegister).Name = "CmbCreditAcRegister";
		((System.Windows.Forms.Control)(object)this.CmbCreditAcRegister).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAcRegister).TabIndex = 6;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(671, 19);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(86, 15);
		this.label19.TabIndex = 165;
		this.label19.Text = "Credit Account";
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(109, 15);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(118, 23);
		this.datFromDate.TabIndex = 0;
		this.CmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Location = new System.Drawing.Point(419, 41);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Name = "CmbSupplierName";
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).TabIndex = 5;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(5, 19);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(91, 15);
		this.label20.TabIndex = 153;
		this.label20.Text = "Date From && To";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(349, 47);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(69, 15);
		this.label5.TabIndex = 163;
		this.label5.Text = "Party Name";
		this.txtVehicleRegister.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleRegister.Location = new System.Drawing.Point(419, 15);
		this.txtVehicleRegister.Name = "txtVehicleRegister";
		this.txtVehicleRegister.Size = new System.Drawing.Size(250, 23);
		this.txtVehicleRegister.TabIndex = 4;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 47);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(100, 15);
		this.label10.TabIndex = 159;
		this.label10.Text = "GP No From && To";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(349, 19);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 161;
		this.label3.Text = "Vehicle No";
		this.txtGpNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNoFrom.Location = new System.Drawing.Point(109, 43);
		this.txtGpNoFrom.Name = "txtGpNoFrom";
		this.txtGpNoFrom.Size = new System.Drawing.Size(118, 23);
		this.txtGpNoFrom.TabIndex = 2;
		this.txtGpNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGpNoFrom_KeyPress);
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(1165, 43);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 24);
		this.btnshow.TabIndex = 10;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtGpNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNoTo.Location = new System.Drawing.Point(229, 43);
		this.txtGpNoTo.Name = "txtGpNoTo";
		this.txtGpNoTo.Size = new System.Drawing.Size(112, 23);
		this.txtGpNoTo.TabIndex = 3;
		this.txtGpNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGpNoTo_KeyPress);
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(229, 15);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(112, 23);
		this.datToDate.TabIndex = 1;
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.label24);
		this.panel23.Controls.Add(this.ctrlGrdBar4);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 27);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1286, 27);
		this.panel23.TabIndex = 164;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(6, 4);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(195, 18);
		this.label24.TabIndex = 24;
		this.label24.Text = "Freight Voucher Register";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnResetRegister, this.btnRefreshRegister, this.btnRegister });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1286, 27);
		this.toolStrip3.TabIndex = 1;
		this.toolStrip3.Text = "toolStrip3";
		this.btnResetRegister.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetRegister.Name = "btnResetRegister";
		this.btnResetRegister.Size = new System.Drawing.Size(68, 24);
		this.btnResetRegister.Text = "Reset";
		this.btnResetRegister.Click += new System.EventHandler(btnResetRegister_Click);
		this.btnRefreshRegister.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshRegister.Name = "btnRefreshRegister";
		this.btnRefreshRegister.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshRegister.Text = "Refresh";
		this.btnRefreshRegister.Click += new System.EventHandler(btnRefreshRegister_Click);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(115, 24);
		this.btnRegister.Text = "240-Register";
		this.btnRegister.Click += new System.EventHandler(btnRegister_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1242, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdPendingRecord;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 19;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1245, 3);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdRegister;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 23;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1294, 692);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "FreightVoucher";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FreightVoucher_FormClosing);
		base.Load += new System.EventHandler(FreightVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PaymentVoucherNew_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		this.Form.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingRecord).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnResetDriverInfo).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPreviousInformation).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCashAccount).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		this.TsForm.ResumeLayout(false);
		this.TsForm.PerformLayout();
		this.Register.ResumeLayout(false);
		this.Register.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdRegister).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAcRegister).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAcRegister).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
