using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.lgstcm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Main;
using Architecture.Model.lgstcm;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.LogisticManagement.FromWise_Helper_Methods;
using Architecture.WinApp.Properties;
using Architecture.WinApp.lgstcm;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Service.lgstcm;

public class frmFreightVoucherExport : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtPaymentDetail = new DataTable();

	private DataTable dtExpGrid = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtLogiticOtherChargesItems = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private string ScreenName = "frmFreightVoucherExport";

	private bool UpdateMode;

	private int RecId;

	private int ForeignBaseCurrency;

	private double ForeignBaseCurrencyRate;

	private int DocumentTypeId = 1305;

	private int updateDetailIndex = -1;

	private bool FormInitialized;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool ChequeBookEnableStatus;

	private int DefaultDaysToLessFromHistoryFromDate;

	private int ChargesToAccountForFreightVoucherOutward;

	private List<FreightVoucherOutwardPaymentDetail> lstRemoveRecordcustomerDetail = new List<FreightVoucherOutwardPaymentDetail>();

	private DataTable dtPaymentDetailDrRecord = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnPrint;

	private TabPage tabPage3;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private GroupBox groupBox2;

	private Label label37;

	private DateTimePicker txtdocdate;

	private TextBox txtdocno;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	private Panel panel38;

	private Label label32;

	internal GridEX grdHistoryDetail;

	private Panel panel9;

	internal GridEX grdHistory;

	private Panel panel15;

	private Label label17;

	private Panel panel13;

	private GroupBox groupBox1;

	private Label label19;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker ToDateHistory;

	private Label label28;

	private TextBox ToDocNoHistory;

	private DateTimePicker FromDateHistory;

	private Label label35;

	private TextBox FromDocNoHistory;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	private CheckBox ChkPreview;

	private TextBox txtRemarksHeader;

	private Label label15;

	internal GridEX grdPaymentDetail;

	private ToolStripButton btnAttachment;

	private Label label1;

	private Label label5;

	private ToolStripButton BtnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Panel panel5;

	private Panel panel14;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label21;

	private Label label14;

	private ToolStripButton btnPrintVoucher;

	private CheckBox chkVoucher;

	private Button BtnAddCustomer;

	private Button BtnCancelCustomer;

	private Button BtnUpdateCustmer;

	private GroupBox groupBox3;

	private Label BalanceValue;

	private Label label16;

	private TextBox txtRemarks;

	private Label label121;

	private TextBox txtPayeeTitle;

	private Label label25;

	internal UltraCombo CmbAccountTitle;

	private DateTimePicker datChequeDate;

	private Label label105;

	internal UltraCombo CmbTransactionType;

	private TextBox txtAmount;

	private Label label118;

	private Label label26;

	internal UltraCombo CmbInstrumentType;

	private Label label29;

	internal UltraCombo CmbChequeNo;

	private TextBox txtChequeNo;

	private DateTimePicker txtDeliveryOrderDate;

	private DateTimePicker txtGpDate;

	internal UltraCombo CmbDeliveryOrderNo;

	private Label label8;

	internal UltraCombo CmbGpNo;

	private Label label4;

	private TextBox txtBiltyNo;

	private TextBox txtVehicleNo;

	private Label label2;

	private DateTimePicker txtBiltyDate;

	private Label label11;

	private Label label10;

	internal UltraCombo CmbTransporter;

	private Label label9;

	private TextBox txtNetFreightPayable;

	private TextBox txtAdvanceCashPaid;

	private TextBox txtOtherCharges;

	private TextBox txtBiltyFreight;

	private TextBox txtQtyForRate;

	private TextBox txtFreightRate;

	private TextBox txtNoOfBags;

	private Label label20;

	private Label label13;

	private Label label18;

	private Label label12;

	private Label label30;

	private Label label7;

	private Label label36;

	private Label label34;

	private Label label33;

	private Label label31;

	private Label label27;

	internal UltraCombo CmbUnLoadingCity;

	private Label label24;

	internal UltraCombo CmbLoadingCity;

	private Label label23;

	internal UltraCombo CmbRateUom;

	internal UltraCombo CmbLogisticPoNo;

	private Label label22;

	internal UltraCombo CmbChargesToDrAccount;

	private Label label38;

	private Label label41;

	private Label label40;

	private Label label39;

	private Label label54;

	private Label label53;

	private Label label52;

	private Label label49;

	private Label label48;

	private Label label47;

	private Label label46;

	private Label label55;

	private Label label45;

	private Label label44;

	private Label label43;

	private Label label42;

	private Label label56;

	private Label label3;

	private Label label6;

	private TextBox txtTotalBiltyFreight;

	private Label label50;

	private ToolStripButton BtnLoadGp;

	private Panel panel16;

	internal GridEX grdInvExp;

	private Panel panel11;

	private Label label81;

	private Button BtnOtherChargesItem;

	private TextBox txtNetWeightWb;

	private Label label57;

	private Label label51;

	private TextBox txtNetBillWeight;

	internal UltraCombo CmbBillWeightBase;

	private Label label60;

	private Label label58;

	private Label label59;

	private Label label61;

	private TextBox txtNetWeightDo;

	public frmFreightVoucherExport(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void frmBillOfMaterial_Manufacturing_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1120, 650);
			BtnOtherChargesItem.Visible = true;
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
			ScreenName = Conversion.ToString((object)base.Name);
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtSupplier.Columns.Add("CurrencyId", typeof(int));
			dtSupplier.Columns.Add("CurrencyCode", typeof(string));
			FreightVoucherOutward_Helper.InitializePaymentDetailtable(dtPaymentDetail);
			FreightVoucherOutward_Helper.InitializePaymentDetailtable(dtPaymentDetailDrRecord);
			FreightVoucherOutward_Helper.InitializeExpenseDetailtable(dtExpGrid);
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
			string documentNoData = DocumentNoDbCall();
			DataTable lookUpData = LookUps.LookUpAllServices(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null);
			DataTable instrumentTypes = VoucherHead.GetInstrumentTypes();
			DataTable gpDateData = PendingGpAndDoDbCall();
			dtLogiticOtherChargesItems = LogiticOtherChargesItems.FormHistory(UserAccount.OrganizationId, UserAccount.CompanyId);
			return new
			{
				DocumentNoData = documentNoData,
				LookUpData = lookUpData,
				InstrumentTypesData = instrumentTypes,
				GpDateData = gpDateData
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
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				btnPrint.Enabled = formright.DoHavePrintRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
				ctrlGrdBar3.mGridSaveLayouts.Visible = false;
				ctrlGrdBar3.removeLayoutToolStripMenuItem.Visible = false;
				GetConfigurationsFromGlobal();
				if (RecId == 0)
				{
					txtdocno.Text = result.DocumentNoData;
				}
				GpAndDoBind(result.GpDateData);
				SupplierDtFillFromGlobal();
				SupplierBind();
				CityBindFromGlobal();
				AllLookUpComboBind(result.LookUpData);
				DrAccountBindFromGlobal();
				TransactionTypeBind();
				InstrumentTypesBind(result.InstrumentTypesData);
				BillToCrAccountBindFromGlobal();
				GetConfigurationsFromGlobalandBind();
				BindGrids();
				Datetypefill();
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				txtdocdate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			ChargesToAccountForFreightVoucherOutward = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ChargesToAccountForFreightVoucherOutward"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			ForeignBaseCurrency = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ForeignBaseCurrency"));
			ForeignBaseCurrencyRate = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("FcyBaseCurrencyRate"));
			ChequeBookEnableStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CheqBook Enabled"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobalandBind()
	{
		try
		{
			if (ChargesToAccountForFreightVoucherOutward > 0)
			{
				CmbChargesToDrAccount.Value = ChargesToAccountForFreightVoucherOutward;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string DocumentNoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			return FreightVoucherOutward.GenerateCode(new FreightVoucherOutward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				documentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}).ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable PendingGpAndDoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			return FreightVoucherOutward.GatePassOutward_PendingForFreightVoucherOutward(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 91,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				RecId = RecId,
				ActionId = 1
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GpAndDoBind(DataTable dt)
	{
		try
		{
			DataTable dtGp = new DataTable();
			dtGp.Columns.Add("Id", typeof(int));
			dtGp.Columns.Add("GpNo", typeof(int));
			dtGp.Columns.Add("GpDate", typeof(DateTime));
			dtGp.Columns.Add("VehicleType");
			dtGp.Columns.Add("VehicleNo");
			dtGp.Columns.Add("BiltyNo");
			dtGp.Columns.Add("BiltyDate", typeof(DateTime));
			dtGp.Columns.Add("WbNetWeight", typeof(double));
			dtGp.Columns.Add("InDateTime", typeof(DateTime));
			dtGp.Columns.Add("OutDateTime", typeof(DateTime));
			dtGp.Columns.Add("DoId", typeof(int));
			DataTable dtDo = new DataTable();
			dtDo.Columns.Add("Id", typeof(int));
			dtDo.Columns.Add("DoNo", typeof(int));
			dtDo.Columns.Add("DoDate", typeof(DateTime));
			dtDo.Columns.Add("GpId", typeof(int));
			dtDo.Columns.Add("DoQty", typeof(double));
			dtDo.Columns.Add("DoWeight", typeof(double));
			foreach (DataRow row in dt.Rows)
			{
				DataRow gpRow = dtGp.NewRow();
				gpRow["Id"] = row["GpId"];
				gpRow["GpNo"] = row["GpSrNo"];
				gpRow["GpDate"] = row["GpDate"];
				gpRow["VehicleType"] = row["VehicleType"];
				gpRow["VehicleNo"] = row["VehicleNo"];
				gpRow["BiltyNo"] = row["BiltyNo"];
				gpRow["BiltyDate"] = row["BiltyDate"];
				gpRow["WbNetWeight"] = row["WbNetWeight"];
				gpRow["OutDateTime"] = row["OutDateTime"];
				gpRow["InDateTime"] = row["InDateTime"];
				gpRow["DoId"] = row["DeliveryOrderId"];
				dtGp.Rows.Add(gpRow);
				DataRow doRow = dtDo.NewRow();
				doRow["Id"] = row["DeliveryOrderId"];
				doRow["DoNo"] = row["DeliveryOrderNo"];
				doRow["DoDate"] = row["DeliveryOrderDate"];
				doRow["GpId"] = row["GpId"];
				doRow["DoQty"] = row["DoQty"];
				doRow["DoWeight"] = row["DoWeight"];
				dtDo.Rows.Add(doRow);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbGpNo, dtGp, "Id", "GpNo", "Gp No", AllColumns: true, Conversion.ToInt(CmbGpNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryOrderNo, dtDo, "Id", "DoNo", "Delivery Order No", AllColumns: true, Conversion.ToInt(CmbDeliveryOrderNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtGp != null && dtGp.Rows.Count > 0)
			{
				((UltraGridBase)CmbGpNo).DisplayLayout.Bands[0].Columns["DoId"].Hidden = true;
			}
			if (dtDo != null && dtDo.Rows.Count > 0)
			{
				((UltraGridBase)CmbDeliveryOrderNo).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			List<getGlobalAllSupplierCustomer> customers = clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => r.CustomerGroupId == 10).ToList();
			if (customers == null || customers.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllSupplierCustomer item in customers)
			{
				object[] rowData = new object[8] { item.Id, item.CompanyName, item.PartyCode, item.GlAccountId, item.CityName, item.MobilePersonal, item.GlAccountCurrencyId, item.GlAccountCurrencyCode };
				dtSupplier.Rows.Add(rowData);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierBind()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbTransporter, dtSupplier, "Id", "CompanyName", "Bill To Party", AllColumns: true, Conversion.ToInt(CmbTransporter.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtSupplier.Rows.Count > 0)
			{
				((UltraGridBase)CmbTransporter).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbTransporter).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				CmbTransporter_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable PendingLogisticPurchaseOrderDbCall(int PartyId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		try
		{
			return FreightVoucherOutward.PurchaseOrderHeader_PendingForFreightVoucherOutward(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				RecId = RecId,
				BillToPartyId = PartyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PurchaseOrderNoBind(DataTable dt)
	{
		try
		{
			DataTable dtDo = new DataTable();
			dtDo.Columns.Add("Id", typeof(int));
			dtDo.Columns.Add("DocNo", typeof(int));
			dtDo.Columns.Add("DocDate", typeof(DateTime));
			dtDo.Columns.Add("RateBaseUomId", typeof(int));
			dtDo.Columns.Add("Rate", typeof(double));
			dtDo.Columns.Add("Qty", typeof(double));
			foreach (DataRow row in dt.Rows)
			{
				DataRow doRow = dtDo.NewRow();
				doRow["Id"] = row["Id"];
				doRow["DocNo"] = row["DocNo"];
				doRow["DocDate"] = row["DocDate"];
				doRow["RateBaseUomId"] = row["RateBaseUomId"];
				doRow["Rate"] = row["Rate"];
				doRow["Qty"] = row["Qty"];
				dtDo.Rows.Add(doRow);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbLogisticPoNo, dtDo, "Id", "DocNo", "Order No", AllColumns: true, Conversion.ToInt(CmbLogisticPoNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtDo != null && dtDo.Rows.Count > 0)
			{
				((UltraGridBase)CmbLogisticPoNo).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				CmbLogisticPoNo_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllLookUpComboBind(DataTable dtcombo)
	{
		try
		{
			DataTable dtSP = new DataTable();
			dtSP.Columns.Add("Id", typeof(int));
			dtSP.Columns.Add("Name");
			DataTable DtUom = dtSP.Clone();
			DtUom.Columns.Add("Equivalent", typeof(double));
			foreach (DataRow row in dtcombo.Rows)
			{
				string activity = row["Activity"]?.ToString();
				int id = Conversion.ToInt(row["Id"]);
				string name = row["ReferenceName"]?.ToString();
				if (!(activity == "BillWeightBase"))
				{
					if (activity == "RateBaseUom")
					{
						double equivalent = Conversion.ToDouble(row["OtherReference"]);
						DtUom.Rows.Add(id, name, equivalent);
					}
				}
				else
				{
					dtSP.Rows.Add(id, name);
				}
			}
			RateUomBind(DtUom);
			BillWeightBaseBind(dtSP);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RateUomBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbRateUom, dt, "Id", "Name", "Rate Base", AllColumns: true, Conversion.ToInt(CmbRateUom.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillWeightBaseBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbBillWeightBase, dt, "Id", "Name", "BillWeight Base", AllColumns: true, Conversion.ToInt(CmbBillWeightBase.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalAllCities, (GlobalAllCity x) => x.Id, (GlobalAllCity x) => x.CityName);
			InfragisticsHelper.BindAndRetainSelection(CmbLoadingCity, dt, "Id", "Description", "Loading City", AllColumns: true, Conversion.ToInt(CmbLoadingCity.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbUnLoadingCity, dt, "Id", "Description", "Un-Loading City", AllColumns: true, Conversion.ToInt(CmbUnLoadingCity.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DrAccountBindFromGlobal()
	{
		try
		{
			DataTable dtAccounts = new DataTable();
			dtAccounts.Columns.Add("Id", typeof(int));
			dtAccounts.Columns.Add("AccountTitle", typeof(string));
			dtAccounts.Columns.Add("AccountCode", typeof(string));
			List<getGlobalAllAccountsWithCustomGroup> dt = clsGlobalVariables.AllAccountsWithCustomGroupId.Where((getGlobalAllAccountsWithCustomGroup r) => r.ChartOfAccountId == ChargesToAccountForFreightVoucherOutward).ToList();
			if (dt != null && dt.Count > 0)
			{
				for (int i = 0; i < dt.Count; i++)
				{
					dtAccounts.Rows.Add(dt[i].ChartOfAccountId, dt[i].AccountTitle, dt[i].AccountCode);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbChargesToDrAccount, dtAccounts, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(CmbChargesToDrAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TransactionTypeBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			dt.Rows.Add(1, "Cash");
			dt.Rows.Add(2, "Bank");
			dt.Rows.Add(3, "Other");
			InfragisticsHelper.BindAndRetainSelection(CmbTransactionType, dt, "Id", "Name", "Transaction Type", AllColumns: true, Conversion.ToInt(CmbTransactionType.Value));
			((UltraGridBase)CmbTransactionType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InstrumentTypesBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbInstrumentType, dt, "Id", "InstrumentType", "Instrument Type", AllColumns: true, Conversion.ToInt(CmbInstrumentType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbInstrumentType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillToCrAccountBindFromGlobal()
	{
		try
		{
			int Id = Conversion.ToInt(CmbTransactionType.Value);
			if (Id == 0)
			{
				((Control)(object)CmbAccountTitle).Text = string.Empty;
				((UltraGridBase)CmbAccountTitle).DataSource = null;
				((Control)(object)CmbChequeNo).Text = string.Empty;
				((UltraGridBase)CmbChequeNo).DataSource = null;
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AccountTitle", typeof(string));
			dt.Columns.Add("AccountCode", typeof(string));
			dt.Columns.Add("AccountClass", typeof(string));
			dt.Columns.Add("AccountType", typeof(string));
			dt.Columns.Add("ParentAccountTitle", typeof(string));
			List<getGlobalAllAccountsWithCustomGroup> allAccounts = clsGlobalVariables.AllAccountsWithCustomGroupId;
			switch (Id)
			{
			case 1:
				allAccounts = allAccounts.Where((getGlobalAllAccountsWithCustomGroup r) => r.AccountTypeId == 2).ToList();
				break;
			case 2:
				allAccounts = allAccounts.Where((getGlobalAllAccountsWithCustomGroup r) => r.AccountTypeId == 15).ToList();
				break;
			case 3:
				allAccounts = allAccounts.Where((getGlobalAllAccountsWithCustomGroup r) => r.AccountTypeId == 3 || r.AccountTypeId == 6 || r.AccountTypeId == 8).ToList();
				break;
			}
			HashSet<int> seenIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup acc in allAccounts)
			{
				int id = Conversion.ToInt((object)acc.ChartOfAccountId);
				if (seenIds.Add(id))
				{
					dt.Rows.Add(id, acc.AccountTitle, acc.AccountCode, acc.AccountClassName, acc.AccountType, acc.ParentAccountTitle);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbAccountTitle, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(CmbAccountTitle.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbTransactionType_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbAccountTitle).Enabled = true;
			if (((DataTable)((UltraGridBase)CmbInstrumentType).DataSource).Rows.Count > 0)
			{
				int Value = ((Conversion.ToInt(CmbTransactionType.Value) == 2) ? 1 : 3);
				CmbInstrumentType.Value = Value;
			}
			BillToCrAccountBindFromGlobal();
			CalculateNetPaid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbAccountTitle_Leave(object sender, EventArgs e)
	{
		try
		{
			AccountCurrentBalance();
			CheqNoFill();
			CalculateNetPaid();
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
				RefAccountId = Conversion.ToInt(CmbAccountTitle.Value)
			});
			if (detail.Rows.Count > 0)
			{
				double ClosingBalance = Conversion.ToDouble(detail.Rows[0]["Balance"]);
				BalanceValue.Text = ClosingBalance.ToString("#,#;(#,#);0") + ((ClosingBalance > 0.0) ? " Dr" : ((ClosingBalance < 0.0) ? " Cr" : ((ClosingBalance == 0.0) ? " Nill" : "")));
			}
			else
			{
				BalanceValue.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheqNoFill()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbTransactionType.Value) == 2 && ChequeBookEnableStatus)
			{
				((Control)(object)CmbChequeNo).Text = string.Empty;
				DataTable dtCheq = CheqBookHeader.OutstandingCheqNo(new CheqBookHeader
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BankId = Conversion.ToInt(CmbAccountTitle.Value),
					Id = CommonServices.VoucherHeadIdGet(RecId, DocumentTypeId)
				});
				if (dtCheq.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("CheqNo");
					dt.Clear();
					for (int i = 0; i < dtCheq.Rows.Count; i++)
					{
						dt.Rows.Add(dtCheq.Rows[i]["Id"], dtCheq.Rows[i]["CheqNo"]);
					}
					if (dt.Rows.Count > 0)
					{
						InfragisticsHelper.BindAndRetainSelection(CmbChequeNo, dtCheq, "Id", "CheqNo", "Cheque No", AllColumns: true, Conversion.ToInt(CmbChequeNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
					}
				}
				else
				{
					((Control)(object)CmbChequeNo).Text = string.Empty;
					((UltraGridBase)CmbChequeNo).DataSource = null;
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

	private void CmbGpNo_Leave(object sender, EventArgs e)
	{
		try
		{
			UltraGridRow row = ((UltraGridBase)CmbGpNo).ActiveRow;
			if (CmbGpNo.Value == null || row == null)
			{
				return;
			}
			int doId = Conversion.ToInt(row.Cells["DoId"].Value);
			if (doId > 0)
			{
				if (!object.Equals(CmbDeliveryOrderNo.Value, doId))
				{
					CmbDeliveryOrderNo.Value = doId;
				}
				DateTime GpDate = Conversion.ToDateTime(row.Cells["GpDate"].Value);
				txtGpDate.Value = GpDate;
				txtVehicleNo.Text = Conversion.ToString(row.Cells["VehicleNo"].Value);
				txtBiltyNo.Text = Conversion.ToString(row.Cells["BiltyNo"].Value);
				DateTime biltyDate = Conversion.ToDateTime(row.Cells["BiltyDate"].Value);
				txtBiltyDate.Value = (Conversion.CheckDateTimeNull(biltyDate) ? GpDate : biltyDate);
				txtNetWeightWb.Text = Conversion.ToString(row.Cells["WbNetWeight"].Value);
				DateTime OutDate = Conversion.ToDateTime(row.Cells["OutDateTime"].Value);
				if (!Conversion.CheckDateTimeNull(OutDate) && btnsave.Visible && btnsave.Enabled)
				{
					txtdocdate.Value = OutDate;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryOrderNo_Leave(object sender, EventArgs e)
	{
		try
		{
			UltraGridRow row = ((UltraGridBase)CmbDeliveryOrderNo).ActiveRow;
			if (CmbDeliveryOrderNo.Value == null || row == null)
			{
				return;
			}
			int gpId = Conversion.ToInt(row.Cells["GpId"].Value);
			if (gpId > 0)
			{
				if (!object.Equals(CmbGpNo.Value, gpId))
				{
					CmbGpNo.Value = gpId;
				}
				DateTime DoDate = Conversion.ToDateTime(row.Cells["DoDate"].Value);
				txtDeliveryOrderDate.Value = DoDate;
				txtNoOfBags.Text = Conversion.ToString(row.Cells["DoQty"].Value);
				txtNetWeightDo.Text = Conversion.ToString(row.Cells["DoWeight"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTransporter_Leave(object sender, EventArgs e)
	{
		try
		{
			int PartyId = Conversion.ToInt(CmbTransporter.Value);
			PurchaseOrderNoBind(PendingLogisticPurchaseOrderDbCall(PartyId));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbLogisticPoNo_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbRateUom).Enabled = true;
			txtFreightRate.Enabled = true;
			txtQtyForRate.Enabled = true;
			txtBiltyFreight.Enabled = true;
			txtNetWeightDo.Enabled = true;
			UltraGridRow row = ((UltraGridBase)CmbLogisticPoNo).ActiveRow;
			if (CmbLogisticPoNo.Value != null && row != null)
			{
				int RateBaseUomId = Conversion.ToInt(row.Cells["RateBaseUomId"].Value);
				double Rate = Conversion.ToDouble(row.Cells["Rate"].Value);
				txtFreightRate.Text = Rate.ToString("#,##.##");
				CmbRateUom.Value = RateBaseUomId;
				((Control)(object)CmbRateUom).Enabled = false;
				txtFreightRate.Enabled = false;
				txtQtyForRate.Enabled = false;
				txtBiltyFreight.Enabled = false;
				txtNetWeightDo.Enabled = false;
				CalculateBiltyFright();
				CalculateNetPaid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((Control)(object)CmbTransporter).Text == null || ((Control)(object)CmbTransporter).Text == string.Empty)
		{
			MessageBox.Show("Transporter Account field is required");
			CmbTransporter.Focus();
			return false;
		}
		if (((Control)(object)CmbTransactionType).Text == "Bank" && Conversion.ToInt(CmbInstrumentType.Value) == 0)
		{
			MessageBox.Show("Instrument Type field is required");
			CmbInstrumentType.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbTransactionType.Value) == 2 && Conversion.ToInt(CmbInstrumentType.Value) == 1)
		{
			if (ChequeBookEnableStatus)
			{
				if (((UltraGridBase)CmbChequeNo).ActiveRow == null)
				{
					MessageBox.Show("Cheque No Field is Required");
					CmbChequeNo.Focus();
					return false;
				}
			}
			else if (txtChequeNo.Text == null || txtChequeNo.Text == string.Empty)
			{
				MessageBox.Show("Cheque No field is required");
				txtChequeNo.Focus();
				return false;
			}
		}
		if (((UltraGridBase)CmbAccountTitle).ActiveRow == null || Conversion.ToInt(CmbAccountTitle.Value) == 0)
		{
			MessageBox.Show("Bill To Account(Cr) field is required");
			CmbAccountTitle.Focus();
			return false;
		}
		return true;
	}

	private void BindGrids()
	{
		try
		{
			DataTable dataTable = dtPaymentDetail;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grdPaymentDetail.DataSource = dtPaymentDetail;
				grdPaymentDetail.RetrieveStructure();
				grdCustomerDetailSetting(grdPaymentDetail);
			}
			DataTable dataTable2 = dtExpGrid;
			if (dataTable2 != null && dataTable2.Columns.Count > 0)
			{
				GenerateRowsInExpenseGrid();
				grdInvExp.DataSource = dtExpGrid;
				grdInvExp.RetrieveStructure();
				grdInvExpSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddCustomer_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (((Control)(object)CmbTransactionType).Text == null || ((Control)(object)CmbTransactionType).Text == string.Empty)
			{
				MessageBox.Show("Transaction Type field is required");
				CmbTransactionType.Focus();
				return;
			}
			if (Conversion.ToDouble((object)txtAmount.Text.Trim()) <= 0.0)
			{
				MessageBox.Show("Net Amount field is Required");
				txtAmount.Focus();
				return;
			}
			double columnSum = GridEX_Helper.GetColumnSum(grdPaymentDetail, "Amount");
			double TotalPaidAmount = Conversion.ToDouble((object)txtTotalBiltyFreight.Text);
			if (columnSum + Conversion.ToDouble((object)txtAmount.Text) > TotalPaidAmount)
			{
				throw new Exception("Amount is greater than total paid amount");
			}
			if (Conversion.ToInt(((UltraDropDownBase)CmbTransporter).SelectedRow.Cells[3].Value) == Conversion.ToInt(CmbAccountTitle.Value))
			{
				throw new Exception("Transporter account cannot be add in grid please check....");
			}
			DataRow dr = null;
			if (grdPaymentDetail.RowCount == 0 && dtPaymentDetail.Rows.Count > 0)
			{
				dr = dtPaymentDetail.Rows[0];
				FillCustomerDetailRow(dr);
			}
			else
			{
				dr = dtPaymentDetail.NewRow();
				dr["Id"] = 0;
				FillCustomerDetailRow(dr);
				dtPaymentDetail.Rows.Add(dr);
			}
			grdPaymentDetail.DataSource = dtPaymentDetail;
			grdPaymentDetail.RetrieveStructure();
			grdCustomerDetailSetting(grdPaymentDetail);
			ResetCustomerDetail();
			CalculateNetPaid();
			((Control)(object)CmbTransporter).Enabled = grdPaymentDetail.RowCount == 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateCustmer_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (((Control)(object)CmbTransactionType).Text == null || ((Control)(object)CmbTransactionType).Text == string.Empty)
			{
				MessageBox.Show("Transaction Type field is required");
				CmbTransactionType.Focus();
				return;
			}
			if (Conversion.ToDouble((object)txtAmount.Text.Trim()) <= 0.0)
			{
				MessageBox.Show("Net Amount field is Required");
				txtAmount.Focus();
				return;
			}
			if (Conversion.ToInt(((UltraDropDownBase)CmbTransporter).SelectedRow.Cells[3].Value) == Conversion.ToInt(CmbAccountTitle.Value))
			{
				throw new Exception("Transporter account cannot be add in grid please check....");
			}
			double Amount = 0.0;
			if (dtPaymentDetail.Rows.Count > 0)
			{
				double rowAmount = Conversion.ToDouble(dtPaymentDetail.Rows[updateDetailIndex]["Amount"]);
				Amount = GridEX_Helper.GetColumnSum(grdPaymentDetail, "Amount");
				Amount -= rowAmount;
			}
			double TotalPaidAmount = Conversion.ToDouble((object)txtTotalBiltyFreight.Text);
			if (Amount + Conversion.ToDouble((object)txtAmount.Text) > TotalPaidAmount)
			{
				throw new Exception("Amount is greater than total paid amount");
			}
			if (updateDetailIndex >= 0 && updateDetailIndex < dtPaymentDetail.Rows.Count)
			{
				DataRow dr = dtPaymentDetail.Rows[updateDetailIndex];
				FillCustomerDetailRow(dr);
				grdPaymentDetail.DataSource = dtPaymentDetail;
				((Control)(object)grdPaymentDetail).Refresh();
				ResetCustomerDetail();
				CalculateNetPaid();
				((Control)(object)CmbTransporter).Enabled = grdPaymentDetail.RowCount == 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillCustomerDetailRow(DataRow dr)
	{
		dr["TransactionTypeId"] = Conversion.ToInt(CmbTransactionType.Value);
		dr["TransactionType"] = Conversion.ToString((object)((Control)(object)CmbTransactionType).Text);
		dr["InstrumentTypeId"] = Conversion.ToInt(CmbInstrumentType.Value);
		dr["InstrumentType"] = Conversion.ToString((object)((Control)(object)CmbInstrumentType).Text);
		dr["AccountTitleId"] = Conversion.ToInt(CmbAccountTitle.Value);
		dr["AccountTitle"] = Conversion.ToString((object)((Control)(object)CmbAccountTitle).Text);
		dr["ChequeId"] = (ChequeBookEnableStatus ? Conversion.ToInt(CmbChequeNo.Value) : 0);
		dr["ChequeNo"] = (ChequeBookEnableStatus ? Conversion.ToString((object)((Control)(object)CmbChequeNo).Text) : txtChequeNo.Text);
		dr["ChequeDate"] = Conversion.ToDateTime((object)datChequeDate.Value);
		dr["Amount"] = Conversion.ToDecimal((object)txtAmount.Text);
		dr["PayeeTitle"] = Conversion.ToString((object)txtPayeeTitle.Text);
		dr["Remarks"] = Conversion.ToString((object)txtRemarks.Text);
	}

	private void BtnCancelCustomer_Click(object sender, EventArgs e)
	{
		try
		{
			ResetCustomerDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdPaymentDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				CmbTransactionType.Value = Conversion.ToInt(item.Cells["TransactionTypeId"].Value);
				CmbTransactionType_Leave(null, null);
				if (Conversion.ToInt(item.Cells["InstrumentTypeId"].Value) > 0)
				{
					CmbInstrumentType.Value = Conversion.ToInt(item.Cells["InstrumentTypeId"].Value);
				}
				CmbAccountTitle.Value = Conversion.ToInt(item.Cells["AccountTitleId"].Value);
				CmbAccountTitle_Leave(null, null);
				if (ChequeBookEnableStatus)
				{
					CmbChequeNo.Value = Conversion.ToInt(item.Cells["ChequeId"].Value);
				}
				else
				{
					txtChequeNo.Text = Conversion.ToString((object)item.Cells["ChequeNo"].Text);
				}
				datChequeDate.Value = Conversion.ToDateTime(item.Cells["ChequeDate"].Value);
				txtAmount.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString("#,##0.###");
				txtPayeeTitle.Text = Conversion.ToString((object)item.Cells["PayeeTitle"].Text);
				txtRemarks.Text = Conversion.ToString((object)item.Cells["Remarks"].Text);
				BtnAddCustomer.Visible = false;
				BtnUpdateCustmer.Visible = true;
				BtnCancelCustomer.Visible = true;
				CmbTransactionType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetailSetting(GridEX gridEX)
	{
		try
		{
			Dictionary<string, EditType> editableColumns = null;
			if (((Control)(object)gridEX).Name == "grdPaymentDetail")
			{
				editableColumns = new Dictionary<string, EditType>
				{
					{
						"Amount",
						(EditType)1
					},
					{
						"PayeeTitle",
						(EditType)1
					},
					{
						"Remarks",
						(EditType)1
					}
				};
			}
			FreightVoucherOutward_Helper.PaymentDetailGridCommonSetting(gridEX, editableColumns);
			if (((Control)(object)gridEX).Name == "grdPaymentDetail")
			{
				GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
				GridEX_Helper.AddButton(gridEX, "Edit", "Edit", 40, 1);
				gridEX.FrozenColumns = 3;
				ctrlGrdBar3_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdPaymentDetail.UpdateData();
			if (grdPaymentDetail.CurrentRow != null)
			{
				_ = e.Column.Key == "";
				CalculateNetPaid();
				grdPaymentDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdPaymentDetail.CurrentRow;
			if (r != null)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteCustomerDetailRow(r);
				}
				if (e.Column.Key == "Edit")
				{
					grdCustomerDetail_DoubleClick(null, null);
				}
				grdPaymentDetail.UpdateData();
				CalculateNetPaid();
				((Control)(object)CmbTransporter).Enabled = grdPaymentDetail.RowCount == 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteCustomerDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						FreightVoucherOutwardPaymentDetail d = new FreightVoucherOutwardPaymentDetail();
						d.FreightVoucherOutwardPaymentDetailId = Conversion.ToInt(r.Cells["Id"].Value);
						FillDetailListCommonForInsertAndDelete(d, r);
						d.ActionTypeId = 3;
						lstRemoveRecordcustomerDetail.Add(d);
						dtPaymentDetail.Rows[r.RowIndex].Delete();
						r.Delete();
					}
				}
				else
				{
					dtPaymentDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				return;
			}
			throw new Exception("Please Reset the Detail first..");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateRowsInExpenseGrid()
	{
		try
		{
			if (dtLogiticOtherChargesItems == null || dtLogiticOtherChargesItems.Rows.Count == 0)
			{
				return;
			}
			dtExpGrid.BeginLoadData();
			bool gridHasRows = grdInvExp.RowCount > 0;
			if (!gridHasRows)
			{
				dtExpGrid.Clear();
			}
			HashSet<int> existingItemIds = (gridHasRows ? new HashSet<int>(from r in dtExpGrid.AsEnumerable()
				select r.Field<int>("ItemId")) : new HashSet<int>());
			foreach (DataRow row in dtLogiticOtherChargesItems.Rows)
			{
				int itemId = Conversion.ToInt(row["LogiticOtherChargesItemsId"]);
				if (!gridHasRows || !existingItemIds.Contains(itemId))
				{
					DataRow newRow = dtExpGrid.NewRow();
					newRow["Id"] = 0;
					newRow["ItemId"] = itemId;
					newRow["Qty"] = 0.0;
					newRow["Rate"] = 0.0;
					newRow["AddAmount"] = 0.0;
					newRow["LessAmount"] = 0.0;
					newRow["Remarks"] = string.Empty;
					dtExpGrid.Rows.Add(newRow);
					existingItemIds.Add(itemId);
				}
			}
			dtExpGrid.EndLoadData();
			grdInvExp.DataSource = dtExpGrid;
			if (!gridHasRows)
			{
				grdInvExp.RetrieveStructure();
				grdInvExpSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteRowInExpenseGrid(GridEXRow item)
	{
		try
		{
			if (Conversion.ToInt(item.Cells["Id"].Value) <= 0 || MessageBox.Show("Are you sure to Clear this Row?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				item.Cells["Id"].Value = 0;
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
				item.Cells["AddAmount"].Value = 0;
				item.Cells["LessAmount"].Value = 0;
				GenerateRowsInExpenseGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseGridCombBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdInvExp.RootTable.Columns["ItemId"], dtLogiticOtherChargesItems, "LogiticOtherChargesItemsId", "Description", "Add/Less Account");
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExpSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Qty",
					(EditType)1
				},
				{
					"Rate",
					(EditType)1
				},
				{
					"AddAmount",
					(EditType)1
				},
				{
					"LessAmount",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdInvExp, 2, 3, hiddenColumns, editableColumns);
			ExpenseGridCombBind();
			grdInvExp.RootTable.Columns["ItemId"].Width = 90;
			grdInvExp.RootTable.Columns["Qty"].Width = 55;
			grdInvExp.RootTable.Columns["Rate"].Width = 55;
			grdInvExp.RootTable.Columns["AddAmount"].Width = 70;
			grdInvExp.RootTable.Columns["LessAmount"].Width = 70;
			grdInvExp.RootTable.Columns["Remarks"].Width = 70;
			GridEX_Helper.AddButton(grdInvExp, "Delete", "Clear", 50, 0);
			grdInvExp.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInvExp.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "Delete")
			{
				DeleteRowInExpenseGrid(item);
			}
			grdInvExp.UpdateData();
			CalculateNetPaid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdInvExp.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				switch (e.Column.Key)
				{
				default:
					return;
				case "Qty":
				case "Rate":
					UpdateAmount(row);
					break;
				case "AddAmount":
				{
					double AddAmount = Conversion.ToDouble(row.Cells["AddAmount"].Value);
					UpdateRate(row, AddAmount);
					row.Cells["LessAmount"].Value = 0;
					break;
				}
				case "LessAmount":
				{
					double amount = Conversion.ToDouble(row.Cells["LessAmount"].Value);
					UpdateRate(row, amount);
					row.Cells["AddAmount"].Value = 0;
					break;
				}
				}
				grdInvExp.UpdateData();
				CalculateNetPaid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateAmount(GridEXRow row)
	{
		double qty = Conversion.ToDouble(row.Cells["Qty"].Value);
		double rate = Conversion.ToDouble(row.Cells["Rate"].Value);
		row.Cells["LessAmount"].Value = 0;
		row.Cells["AddAmount"].Value = qty * rate;
	}

	private void UpdateRate(GridEXRow row, double amount)
	{
		double qty = Conversion.ToDouble(row.Cells["Qty"].Value);
		row.Cells["Rate"].Value = ((qty == 0.0) ? 0.0 : (amount / qty));
	}

	private void grdInvExp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grdInvExp.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdInvExp.CurrentColumn != null && grdInvExp.CurrentColumn.Key == "Delete")
				{
					DeleteRowInExpenseGrid(r);
					grdInvExp.UpdateData();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteRowInExpenseGrid(r);
				grdInvExp.UpdateData();
			}
			CalculateNetPaid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FromReset();
	}

	private void FromReset()
	{
		try
		{
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			lstRemoveRecordcustomerDetail.Clear();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			txtdocno.Text = string.Empty;
			((Control)(object)CmbGpNo).Text = string.Empty;
			((Control)(object)CmbDeliveryOrderNo).Text = string.Empty;
			((Control)(object)CmbTransporter).Text = string.Empty;
			((Control)(object)CmbLoadingCity).Text = string.Empty;
			((Control)(object)CmbUnLoadingCity).Text = string.Empty;
			txtRemarksHeader.Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			txtBiltyNo.Text = string.Empty;
			((Control)(object)CmbLogisticPoNo).Text = string.Empty;
			txtNoOfBags.Text = string.Empty;
			txtNetWeightDo.Text = string.Empty;
			txtNetWeightWb.Text = string.Empty;
			txtNetBillWeight.Text = string.Empty;
			ResetCustomerDetail();
			dtPaymentDetailDrRecord.Rows.Clear();
			dtPaymentDetail.Rows.Clear();
			dtExpGrid.Rows.Clear();
			BindGrids();
			txtFreightRate.Text = string.Empty;
			txtQtyForRate.Text = string.Empty;
			txtBiltyFreight.Text = string.Empty;
			txtOtherCharges.Text = string.Empty;
			txtAdvanceCashPaid.Text = string.Empty;
			txtNetFreightPayable.Text = string.Empty;
			txtdocno.Text = DocumentNoDbCall();
			GpAndDoBind(PendingGpAndDoDbCall());
			CmbTransporter_Leave(null, null);
			((Control)(object)CmbRateUom).Enabled = true;
			txtFreightRate.Enabled = true;
			txtQtyForRate.Enabled = true;
			txtBiltyFreight.Enabled = true;
			txtNetWeightDo.Enabled = true;
			((Control)(object)CmbTransporter).Enabled = grdPaymentDetail.RowCount == 0;
			txtdocdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetCustomerDetail()
	{
		try
		{
			((Control)(object)CmbInstrumentType).Text = string.Empty;
			((Control)(object)CmbAccountTitle).Text = string.Empty;
			((UltraGridBase)CmbAccountTitle).DataSource = null;
			BalanceValue.Text = "0";
			((Control)(object)CmbChequeNo).Text = string.Empty;
			txtChequeNo.Text = string.Empty;
			datChequeDate.Value = DateTime.Now;
			txtAmount.Clear();
			txtPayeeTitle.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			updateDetailIndex = -1;
			BtnAddCustomer.Visible = true;
			BtnCancelCustomer.Visible = false;
			BtnUpdateCustmer.Visible = false;
			CmbTransactionType.Focus();
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
			List<string> allServices = new List<string> { "SupplierCustomerLists", "AccountsWithCustomGroupId", "AllCities" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			GpAndDoBind(PendingGpAndDoDbCall());
			SupplierDtFillFromGlobal();
			SupplierBind();
			CityBindFromGlobal();
			AllLookUpComboBind(LookUps.LookUpAllServices(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null));
			DrAccountBindFromGlobal();
			TransactionTypeBind();
			InstrumentTypesBind(VoucherHead.GetInstrumentTypes());
			BillToCrAccountBindFromGlobal();
			GetConfigurationsFromGlobalandBind();
			((Control)(object)CmbTransporter).Enabled = grdPaymentDetail.RowCount == 0;
			dtLogiticOtherChargesItems = LogiticOtherChargesItems.FormHistory(UserAccount.OrganizationId, UserAccount.CompanyId);
			ExpenseGridCombBind();
			GenerateRowsInExpenseGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmFreightVoucherExport_FormClosed(object sender, FormClosedEventArgs e)
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

	private void Insert()
	{
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Expected O, but got Unknown
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Expected O, but got Unknown
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Expected O, but got Unknown
		//IL_0c1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c21: Expected O, but got Unknown
		try
		{
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				(txtdocno, "Doc No", FormHelper.ValidType.INT),
				((Control)(object)CmbGpNo, "Gate Pass No", FormHelper.ValidType.INT),
				((Control)(object)CmbDeliveryOrderNo, "Delivery Order No", FormHelper.ValidType.INT),
				((Control)(object)CmbTransporter, "Transporter", FormHelper.ValidType.INT),
				((Control)(object)CmbLoadingCity, "Loading City", FormHelper.ValidType.INT),
				((Control)(object)CmbUnLoadingCity, "Un-Loading City", FormHelper.ValidType.INT),
				(txtVehicleNo, "Vehicle No", FormHelper.ValidType.String),
				(txtBiltyNo, "Bilty No", FormHelper.ValidType.String),
				(txtNoOfBags, "No Of Bags", FormHelper.ValidType.Double),
				((Control)(object)CmbBillWeightBase, "Bill Weight Base", FormHelper.ValidType.INT),
				(txtNetBillWeight, "Bill Weight", FormHelper.ValidType.Double),
				(txtFreightRate, "Freight Rate", FormHelper.ValidType.Double),
				((Control)(object)CmbRateUom, "Rate Uom", FormHelper.ValidType.INT),
				(txtQtyForRate, "Qty For Rate", FormHelper.ValidType.Double),
				(txtBiltyFreight, "Bilty Freight", FormHelper.ValidType.Double)
			}))
			{
				return;
			}
			if (Conversion.ToDouble((object)txtNetFreightPayable.Text.Trim()) < 0.0)
			{
				MessageBox.Show("Net Freight Payable cannot be lesser than zero.");
				CmbTransactionType.Focus();
				return;
			}
			if (CmbLoadingCity.Value != null && CmbUnLoadingCity.Value != null && Conversion.ToInt(CmbLoadingCity.Value) == Conversion.ToInt(CmbUnLoadingCity.Value))
			{
				MessageBox.Show("Loading City and Unloading City cannot be the same.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				CmbUnLoadingCity.Focus();
				return;
			}
			if (CmbChargesToDrAccount.Value == null || Conversion.ToInt(CmbChargesToDrAccount.Value) == 0)
			{
				MessageBox.Show("Please select a Charges To Account (Dr).\n\nIf the list is empty, go to Configuration, set the default Charges Account's Custom Group, and then click Refresh and then Select an Account.", "Missing Required Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				CmbChargesToDrAccount.Focus();
				return;
			}
			CalculateNetPaid();
			if (!FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			FreightVoucherOutward Or = new FreightVoucherOutward();
			Or.FreightVoucherOutwardId = RecId;
			Or.CompanyId = UserAccount.CompanyId;
			Or.BranchesId = UserAccount.BranchesId;
			Or.ProjectsId = UserAccount.BranchesId;
			Or.OrganizationId = UserAccount.OrganizationId;
			Or.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Or.documentTypeId = DocumentTypeId;
			Or.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			Or.DocNo = Conversion.ToInt((object)txtdocno.Text);
			Or.GatePassOutwardId = Conversion.ToInt(CmbGpNo.Value);
			Or.GpSrNo = Conversion.ToInt(CmbGpNo.Value);
			Or.GpDate = Conversion.ToDateTime((object)txtGpDate.Value);
			Or.InvDeliveryOrderId = Conversion.ToInt(CmbDeliveryOrderNo.Value);
			Or.DeliveryOrderNo = Conversion.ToInt((object)((Control)(object)CmbDeliveryOrderNo).Text);
			Or.DeliveryOrderDate = Conversion.ToDateTime((object)txtDeliveryOrderDate.Value);
			Or.transporterId = Conversion.ToInt(CmbTransporter.Value);
			Or.TransporterSupCustId = Conversion.ToInt(((UltraDropDownBase)CmbTransporter).SelectedRow.Cells[3].Value);
			Or.loadingFromCityId = Conversion.ToInt(CmbLoadingCity.Value);
			Or.LoadingFromCityName = Conversion.ToString((object)((Control)(object)CmbLoadingCity).Text);
			Or.unloadingToCityId = Conversion.ToInt(CmbUnLoadingCity.Value);
			Or.UnloadingToCityName = Conversion.ToString((object)((Control)(object)CmbUnLoadingCity).Text);
			Or.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			Or.vehicleNo = Conversion.ToString((object)txtVehicleNo.Text);
			Or.biltyNo = Conversion.ToString((object)txtBiltyNo.Text);
			Or.biltyDate = Conversion.ToDateTime((object)txtBiltyDate.Value);
			Or.PurchaseOrderHeaderId = Conversion.ToInt(CmbLogisticPoNo.Value);
			Or.PurchaseOrderNo = Conversion.ToInt((object)((Control)(object)CmbLogisticPoNo).Text);
			Or.NoOfBags = Conversion.ToDecimal((object)txtNoOfBags.Text);
			Or.NetWeightDo = Conversion.ToDecimal((object)txtNetWeightDo.Text);
			Or.NetWeightWB = Conversion.ToDecimal((object)txtNetWeightWb.Text);
			Or.BillWeightBaseId = Conversion.ToInt(CmbBillWeightBase.Value);
			Or.netWeight = Conversion.ToDecimal((object)txtNetBillWeight.Text);
			Or.freightRate = Conversion.ToDecimal((object)txtFreightRate.Text);
			Or.rateUomId = Conversion.ToInt(CmbRateUom.Value);
			Or.RateUomCode = Conversion.ToString((object)((Control)(object)CmbRateUom).Text);
			Or.QtyforRate = Conversion.ToDecimal((object)txtQtyForRate.Text);
			Or.biltyFreight = Conversion.ToDecimal((object)txtBiltyFreight.Text);
			Or.otherCharges = Conversion.ToDecimal((object)txtOtherCharges.Text);
			Or.totalbiltyfreight = Conversion.ToDecimal((object)txtTotalBiltyFreight.Text);
			Or.AdvanceOrCashFreight = Conversion.ToDecimal((object)txtAdvanceCashPaid.Text);
			Or.totalFreight = Conversion.ToDecimal((object)txtNetFreightPayable.Text);
			Or.chargeToDrAccountId = Conversion.ToInt(CmbChargesToDrAccount.Value);
			Or.ChargedToDrAccountTitle = Conversion.ToString((object)((Control)(object)CmbChargesToDrAccount).Text);
			Or.EntryUserId = UserAccount.ID;
			Or.EntryDate = DateTime.Now;
			Or.ModifyDate = DateTime.Now;
			Or.ModifyUserId = UserAccount.ID;
			Or.ApprovedUserId = UserAccount.ID;
			Or.ApprovedDate = DateTime.Now;
			Or.IsApproved = false;
			Or.FreightVoucherOutwardPaymentDetailList = new List<FreightVoucherOutwardPaymentDetail>();
			Or.FreightVoucherOutwardExpenseDetailList = new List<FreightVoucherOutwardExpenseDetail>();
			if (lstRemoveRecordcustomerDetail.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveRecordcustomerDetail.Count; i++)
				{
					Or.FreightVoucherOutwardPaymentDetailList.Add(lstRemoveRecordcustomerDetail[i]);
				}
			}
			GridEXRow[] rows;
			if (grdPaymentDetail.RowCount > 0)
			{
				rows = grdPaymentDetail.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					FreightVoucherOutwardPaymentDetail vd2 = new FreightVoucherOutwardPaymentDetail();
					vd2.FreightVoucherOutwardPaymentDetailId = ((RecId != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
					vd2.ActionTypeId = ((vd2.FreightVoucherOutwardPaymentDetailId <= 0) ? 1 : 2);
					FillDetailListCommonForInsertAndDelete(vd2, r2);
					if (Or.TransporterSupCustId == vd2.AccountId)
					{
						throw new Exception("Transporter account cannot be add in grid please check....");
					}
					FormHelper.ValidateField(vd2.TransTypeId, "Transaction Type", r2.RowIndex);
					if (vd2.TransTypeId == 2)
					{
						FormHelper.ValidateField(vd2.InstrumentTypeId, "Instrument Type", r2.RowIndex);
					}
					FormHelper.ValidateField(vd2.AccountId, "Account", r2.RowIndex);
					if (vd2.TransTypeId == 2 && vd2.TransTypeId == 1)
					{
						if (ChequeBookEnableStatus)
						{
							FormHelper.ValidateField(vd2.CheqId, "Cheque No", r2.RowIndex);
						}
						else
						{
							FormHelper.ValidateField(vd2.CheqNo, "Cheque No", r2.RowIndex);
						}
					}
					FormHelper.ValidateField(vd2.CreditAmount, "Amount", r2.RowIndex);
					if (string.IsNullOrWhiteSpace(Or.RemarksHeader))
					{
						Or.RemarksHeader = vd2.Remarks;
					}
					Or.FreightVoucherOutwardPaymentDetailList.Add(vd2);
				}
			}
			decimal debitamount = Conversion.ToDecimal((object)txtNetFreightPayable.Text);
			if (dtPaymentDetailDrRecord.Rows.Count == 0 && debitamount > 0m)
			{
				FreightVoucherOutwardPaymentDetail vd = new FreightVoucherOutwardPaymentDetail();
				vd.FreightVoucherOutwardPaymentDetailId = 0;
				vd.ActionTypeId = ((vd.FreightVoucherOutwardPaymentDetailId <= 0) ? 1 : 2);
				vd.TransTypeId = 3;
				vd.TransactionType = "Other";
				vd.InstrumentTypeId = 3;
				vd.InstrumentType = "Other";
				vd.AccountId = Conversion.ToInt(CmbChargesToDrAccount.Value);
				vd.AccountTitle = Conversion.ToString(CmbAccountTitle.Value);
				vd.CheqId = 0;
				vd.CheqNo = string.Empty;
				vd.CheqDate = DateTime.Now;
				vd.DebitAmount = debitamount;
				vd.PayeeTitle = string.Empty;
				vd.Remarks = string.Empty;
				Or.FreightVoucherOutwardPaymentDetailList.Add(vd);
			}
			else if (dtPaymentDetailDrRecord.Rows.Count > 0)
			{
				FreightVoucherOutwardPaymentDetail vd3 = new FreightVoucherOutwardPaymentDetail();
				DataRow dr = dtPaymentDetailDrRecord.Rows[0];
				vd3.FreightVoucherOutwardPaymentDetailId = ((RecId != 0) ? Conversion.ToInt(dr["Id"]) : 0);
				vd3.ActionTypeId = ((vd3.FreightVoucherOutwardPaymentDetailId == 0) ? 1 : ((debitamount > 0m) ? 2 : 3));
				vd3.TransTypeId = Conversion.ToInt(dr["TransactionTypeId"]);
				vd3.TransactionType = Conversion.ToString(dr["TransactionType"]);
				vd3.InstrumentTypeId = Conversion.ToInt(dr["InstrumentTypeId"]);
				vd3.InstrumentType = Conversion.ToString(dr["InstrumentType"]);
				vd3.AccountId = Conversion.ToInt(dr["AccountTitleId"]);
				vd3.AccountTitle = Conversion.ToString(dr["AccountTitle"]);
				vd3.CheqId = Conversion.ToInt(dr["ChequeId"]);
				vd3.CheqNo = Conversion.ToString(dr["ChequeNo"]);
				vd3.CheqDate = Conversion.ToDateTime(dr["ChequeDate"]);
				vd3.DebitAmount = Conversion.ToDecimal(dr["Amount"]);
				vd3.PayeeTitle = Conversion.ToString(dr["PayeeTitle"]);
				vd3.Remarks = Conversion.ToString(dr["Remarks"]);
				Or.FreightVoucherOutwardPaymentDetailList.Add(vd3);
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["ItemId"].Value) != 0 && (Conversion.ToDouble(r.Cells["Qty"].Value) > 0.0 || Conversion.ToDouble(r.Cells["Rate"].Value) > 0.0 || Conversion.ToDouble(r.Cells["AddAmount"].Value) > 0.0 || Conversion.ToDouble(r.Cells["LessAmount"].Value) > 0.0))
				{
					FreightVoucherOutwardExpenseDetail PE = new FreightVoucherOutwardExpenseDetail();
					if (Conversion.ToDouble(r.Cells["AddAmount"].Value) == 0.0 && Conversion.ToDouble(r.Cells["LessAmount"].Value) == 0.0)
					{
						throw new Exception($"AddAmount or LessAmount Field Required in Expense Grid Row no : {r.RowIndex + 1}");
					}
					PE.FreightVoucherOutwardExpenseDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					PE.LogiticOtherChargesItemsId = Conversion.ToInt(r.Cells["ItemId"].Value);
					PE.OtherChargesItemDescription = Conversion.ToString((object)r.Cells["ItemId"].Text);
					PE.Qty = Conversion.ToInt(r.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					PE.AddAmount = Conversion.ToDouble(r.Cells["AddAmount"].Value);
					PE.LessAmount = Conversion.ToDouble(r.Cells["LessAmount"].Value);
					PE.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					Or.FreightVoucherOutwardExpenseDetailList.Add(PE);
				}
			}
			Or.AttachmentsList = new List<DMSAttachments>();
			Or.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<FreightVoucherOutward>(Or, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			Or.AttachmentsValues = AttachmentsValues;
			Or.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = FreightVoucherOutward.Save(Or);
			Or.FreightVoucherOutwardId = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + Or.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + Or.DocNo);
			}
			FromReset();
			if (ChkPreview.Checked && formright.DoHavePrintRights)
			{
				FreightVoucherOutward_Slip(Or.FreightVoucherOutwardId);
			}
			if (chkVoucher.Checked && formright.DoHavePrintRights)
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(CommonServices.VoucherHeadIdGet(Or.FreightVoucherOutwardId, DocumentTypeId));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		try
		{
			FromReset();
			RecId = Id;
			FreightVoucherOutward Or = FreightVoucherOutward.ReadById(RecId);
			if (Or == null || Or.FreightVoucherOutwardPaymentDetailList == null)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			tabControl1.SelectedIndex = 0;
			UpdateMode = true;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtdocdate.Value = Or.DocDate;
			txtdocno.Text = Conversion.ToString((object)Or.DocNo);
			GpAndDoBind(PendingGpAndDoDbCall());
			CmbDeliveryOrderNo.Value = Conversion.ToInt((object)Or.InvDeliveryOrderId);
			CmbGpNo.Value = Conversion.ToInt((object)Or.GatePassOutwardId);
			CmbTransporter.Value = Conversion.ToInt((object)Or.transporterId);
			CmbTransporter_Leave(null, null);
			CmbLoadingCity.Value = Conversion.ToInt((object)Or.loadingFromCityId);
			CmbUnLoadingCity.Value = Conversion.ToInt((object)Or.unloadingToCityId);
			txtRemarksHeader.Text = Conversion.ToString((object)Or.RemarksHeader);
			txtVehicleNo.Text = Conversion.ToString((object)Or.vehicleNo);
			txtBiltyNo.Text = Conversion.ToString((object)Or.biltyNo);
			txtBiltyDate.Value = Conversion.ToDateTime((object)Or.biltyDate);
			CmbLogisticPoNo.Value = Conversion.ToInt((object)Or.PurchaseOrderHeaderId);
			CmbLogisticPoNo_Leave(null, null);
			txtNoOfBags.Text = Or.NoOfBags.ToString("#,##.##");
			txtNetWeightDo.Text = Or.NetWeightDo.ToString("#,##.##");
			txtNetWeightWb.Text = Or.NetWeightWB.ToString("#,##.##");
			CmbBillWeightBase.Value = Conversion.ToInt((object)Or.BillWeightBaseId);
			txtNetBillWeight.Text = Or.netWeight.ToString("#,##.##");
			txtFreightRate.Text = Or.freightRate.ToString(clsGlobalVariables.DecimalRateFormate);
			CmbRateUom.Value = Conversion.ToInt((object)Or.rateUomId);
			txtQtyForRate.Text = Or.QtyforRate.ToString("#,##.##");
			txtBiltyFreight.Text = Or.biltyFreight.ToString(clsGlobalVariables.stringFormatsingle);
			txtOtherCharges.Text = Or.otherCharges.ToString(clsGlobalVariables.stringFormatsingle);
			txtTotalBiltyFreight.Text = Or.totalbiltyfreight.ToString(clsGlobalVariables.stringFormatsingle);
			txtAdvanceCashPaid.Text = Or.AdvanceOrCashFreight.ToString(clsGlobalVariables.stringFormatsingle);
			txtNetFreightPayable.Text = Or.totalFreight.ToString(clsGlobalVariables.stringFormatsingle);
			CmbChargesToDrAccount.Value = Conversion.ToInt((object)Or.chargeToDrAccountId);
			AttachmentsValues = Or.AttachmentsValues;
			CustomAttachmentsValues = Or.CustomAttachmentsValues;
			dtPaymentDetail.Rows.Clear();
			dtPaymentDetailDrRecord.Rows.Clear();
			FreightVoucherOutward_Helper.FillPaymentDetailFromList(Or, dtPaymentDetail, dtPaymentDetailDrRecord);
			dtExpGrid.Rows.Clear();
			foreach (FreightVoucherOutwardExpenseDetail d in Or.FreightVoucherOutwardExpenseDetailList)
			{
				DataRow row = dtExpGrid.NewRow();
				row["Id"] = d.FreightVoucherOutwardExpenseDetailId;
				row["ItemId"] = d.LogiticOtherChargesItemsId;
				row["Qty"] = d.Qty;
				row["Rate"] = d.Rate;
				row["AddAmount"] = d.AddAmount;
				row["LessAmount"] = d.LessAmount;
				row["Remarks"] = d.Remarks;
				dtExpGrid.Rows.Add(row);
			}
			BindGrids();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			CalculateNetPaid();
			txtdocdate.Focus();
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
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					FreightVoucherOutward.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					FromReset();
				}
			}
			else
			{
				MessageBox.Show("No record found to Delete");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(FreightVoucherOutwardPaymentDetail vd, GridEXRow r)
	{
		vd.TransTypeId = Conversion.ToInt(r.Cells["TransactionTypeId"].Value);
		vd.TransactionType = Conversion.ToString(r.Cells["TransactionType"].Value);
		vd.InstrumentTypeId = Conversion.ToInt(r.Cells["InstrumentTypeId"].Value);
		vd.InstrumentType = Conversion.ToString(r.Cells["InstrumentType"].Value);
		vd.AccountId = Conversion.ToInt(r.Cells["AccountTitleId"].Value);
		vd.AccountTitle = Conversion.ToString(r.Cells["AccountTitle"].Value);
		vd.CheqId = Conversion.ToInt(r.Cells["ChequeId"].Value);
		vd.CheqNo = Conversion.ToString(r.Cells["ChequeNo"].Value);
		vd.CheqDate = Conversion.ToDateTime(r.Cells["ChequeDate"].Value);
		vd.CreditAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
		vd.PayeeTitle = Conversion.ToString(r.Cells["PayeeTitle"].Value);
		vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	private void CalculateQtyForRate()
	{
		try
		{
			double NetWeight = Conversion.ToDouble((object)txtNetBillWeight.Text);
			int RateUomId = Conversion.ToInt(CmbRateUom.Value);
			double QtyForRate = 0.0;
			QtyForRate = RateUomId switch
			{
				3 => NetWeight / 1000.0, 
				4 => NetWeight, 
				_ => 1.0, 
			};
			txtQtyForRate.Text = QtyForRate.ToString("#,##.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNoOfBags_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateQtyForRate();
			CalculateBiltyFright();
			CalculateNetPaid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNetBillWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			switch (Conversion.ToInt(CmbBillWeightBase.Value))
			{
			case 23:
				txtNetBillWeight.Text = txtNetWeightDo.Text;
				break;
			case 24:
				txtNetBillWeight.Text = txtNetWeightWb.Text;
				break;
			}
			CalculateQtyForRate();
			CalculateBiltyFright();
			CalculateNetPaid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateBiltyFright()
	{
		try
		{
			double BiltyFreight = 0.0;
			double FreightRate = Conversion.ToDouble((object)txtFreightRate.Text);
			if (FreightRate > 0.0)
			{
				double QtyForRate = Conversion.ToDouble((object)txtQtyForRate.Text);
				BiltyFreight = FreightRate * QtyForRate;
			}
			txtBiltyFreight.Text = BiltyFreight.ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateNetPaid()
	{
		try
		{
			double BiltyFreight = Conversion.ToDouble((object)txtBiltyFreight.Text);
			double OtherCharges = GridEX_Helper.GetColumnSum(grdInvExp, "AddAmount") - GridEX_Helper.GetColumnSum(grdInvExp, "LessAmount");
			txtOtherCharges.Text = OtherCharges.ToString(clsGlobalVariables.stringFormatsingle);
			double totalbiltyfreight = BiltyFreight + OtherCharges;
			txtTotalBiltyFreight.Text = totalbiltyfreight.ToString(clsGlobalVariables.stringFormatsingle);
			double AdvanceCashPaid = 0.0;
			GridEXRow[] dataRows = grdPaymentDetail.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				AdvanceCashPaid += Conversion.ToDouble(r.Cells["Amount"].Value);
			}
			txtAdvanceCashPaid.Text = AdvanceCashPaid.ToString(clsGlobalVariables.stringFormatsingle);
			double NetFreightPayable = BiltyFreight + OtherCharges - AdvanceCashPaid;
			txtNetFreightPayable.Text = NetFreightPayable.ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRateUom_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateQtyForRate();
			CalculateBiltyFright();
			CalculateNetPaid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBiltyFright();
			CalculateNetPaid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyForRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBiltyFright();
			CalculateNetPaid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBiltyFreight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateNetPaid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOtherCharges_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateNetPaid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAdvanceCashPaid_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateNetPaid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAmout_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateNetPaid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBillWeightBase_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			switch (Conversion.ToInt(CmbBillWeightBase.Value))
			{
			case 23:
				txtNetBillWeight.Text = txtNetWeightDo.Text;
				break;
			case 24:
				txtNetBillWeight.Text = txtNetWeightWb.Text;
				break;
			}
			CalculateQtyForRate();
			CalculateBiltyFright();
			CalculateNetPaid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			FreightVoucherOutward_Slip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightVoucherOutward_Slip(int PrintId)
	{
		try
		{
			CrystalReportPrint_Helper.FreightVoucherOutward_Slip_1305(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintVoucher_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(CommonServices.VoucherHeadIdGet(RecId, DocumentTypeId));
				return;
			}
			throw new Exception("Record Not Found For Display");
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
			cmbDateTypeHistory.Focus();
		}
		else
		{
			txtdocdate.Focus();
		}
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			return FreightVoucherOutward.GetDataForDropDownFromFreightVoucherOutward(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = DocumentTypeId.ToString()
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dtcombo)
	{
		try
		{
			DataTable dtST = new DataTable();
			dtST.Columns.Add("Id", typeof(int));
			dtST.Columns.Add("Name");
			DataTable dtStatus = dtST.Clone();
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ServiceType")
				{
					dtST.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "BrokerOrServiceProvider")
				{
					dtStatus.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			InfragisticsHelper.BindComboDateType(cmbDateTypeHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbDateTypeHistory.Value) == 1)
			{
				FromDateHistory.Value = DateTime.Today;
			}
			else if (Conversion.ToInt(cmbDateTypeHistory.Value) == 2)
			{
				FromDateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Conversion.ToInt(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.Now;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				FromDateHistory.Value = first;
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Conversion.ToInt(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				FromDateHistory.Value = first2;
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Conversion.ToInt(cmbDateTypeHistory.Value) == 5)
			{
				FromDateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbDateTypeHistory).Text = string.Empty;
			grdHistory.DataSource = null;
			grdHistoryDetail.DataSource = null;
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		Datetypefill();
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

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = DocumentTypeId;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			obj.EntryUser = UserAccount.ID;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			DataTable dtHistory = FreightVoucherOutward.FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dataTable = dtHistoryGrid;
				if (dataTable != null && dataTable.Columns.Count == 0)
				{
					dtHistoryGrid.Columns.Add("Id", typeof(int));
					dtHistoryGrid.Columns.Add("DocNo", typeof(int));
					dtHistoryGrid.Columns.Add("DocDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("DocumentTypeId", typeof(int));
					dtHistoryGrid.Columns.Add("GatePassOutwardId", typeof(int));
					dtHistoryGrid.Columns.Add("GpSrNo", typeof(int));
					dtHistoryGrid.Columns.Add("GpDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("InvDeliveryOrderId", typeof(int));
					dtHistoryGrid.Columns.Add("DeliveryOrderNo", typeof(string));
					dtHistoryGrid.Columns.Add("DeliveryOrderDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("TransporterId", typeof(int));
					dtHistoryGrid.Columns.Add("TransporterAccountTitle", typeof(string));
					dtHistoryGrid.Columns.Add("VehicleNo", typeof(string));
					dtHistoryGrid.Columns.Add("BiltyNo", typeof(string));
					dtHistoryGrid.Columns.Add("BiltyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ChargeToDrAccountId", typeof(int));
					dtHistoryGrid.Columns.Add("ChargedToDrAccountTitle", typeof(string));
					dtHistoryGrid.Columns.Add("LoadingFromCityName", typeof(string));
					dtHistoryGrid.Columns.Add("UnloadingToCityName", typeof(string));
					dtHistoryGrid.Columns.Add("PurchaseOrderHeaderId", typeof(int));
					dtHistoryGrid.Columns.Add("PurchaseOrderNo", typeof(string));
					dtHistoryGrid.Columns.Add("NoOfBags", typeof(int));
					dtHistoryGrid.Columns.Add("NetWeightDo", typeof(decimal));
					dtHistoryGrid.Columns.Add("NetWeightWb", typeof(decimal));
					dtHistoryGrid.Columns.Add("BillWeight", typeof(decimal));
					dtHistoryGrid.Columns.Add("FreightRate", typeof(decimal));
					dtHistoryGrid.Columns.Add("RateUomCode", typeof(string));
					dtHistoryGrid.Columns.Add("QtyforRate", typeof(decimal));
					dtHistoryGrid.Columns.Add("BiltyFreight", typeof(decimal));
					dtHistoryGrid.Columns.Add("OtherCharges", typeof(decimal));
					dtHistoryGrid.Columns.Add("TotalFreight", typeof(decimal));
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("EntryUserName", typeof(string));
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ModifyUserName", typeof(string));
					dtHistoryGrid.Columns.Add("IsApproved", typeof(bool));
					dtHistoryGrid.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ApprovalUserName", typeof(string));
					dtHistoryGrid.Columns.Add("RemarksHeader", typeof(string));
					dtHistoryGrid.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtHistoryGrid.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					dtHistoryGrid.Rows.Add(row["FreightVoucherOutwardId"], row["DocNo"], row["DocDate"], row["DocumentTypeId"], row["GatePassOutwardId"], row["GpSrNo"], row["GpDate"], row["InvDeliveryOrderId"], row["DeliveryOrderNo"], row["DeliveryOrderDate"], row["TransporterId"], row["TransporterAccountTitle"], row["VehicleNo"], row["BiltyNo"], row["BiltyDate"], row["ChargeToDrAccountId"], row["ChargedToDrAccountTitle"], row["LoadingFromCityName"], row["UnloadingToCityName"], row["PurchaseOrderHeaderId"], row["PurchaseOrderNo"], row["NoOfBags"], row["NetWeightDo"], row["NetWeightWB"], row["NetWeight"], row["FreightRate"], row["RateUomCode"], row["QtyforRate"], row["BiltyFreight"], row["OtherCharges"], row["TotalFreight"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["IsApproved"], row["ApprovedDate"], row["ApprovalUserName"], row["RemarksHeader"], row["NoOfAttachments"]);
				}
				grdHistory.DataSource = dtHistoryGrid;
				grdHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				grdHistory.DataSource = null;
				grdHistoryDetail.DataSource = null;
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
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "GatePassOutwardId", "InvDeliveryOrderId", "TransporterId", "ChargeToDrAccountId", "PurchaseOrderHeaderId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 3, 3, hiddenColumns);
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["GpDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["DeliveryOrderDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["BiltyDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["GpSrNo"].Width = 60;
			grdHistory.RootTable.Columns["DeliveryOrderNo"].Width = 70;
			grdHistory.RootTable.Columns["DeliveryOrderDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["TransporterAccountTitle"].Width = 150;
			grdHistory.RootTable.Columns["VehicleNo"].Width = 90;
			grdHistory.RootTable.Columns["BiltyNo"].Width = 90;
			grdHistory.RootTable.Columns["BiltyDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ChargedToDrAccountTitle"].Width = 150;
			grdHistory.RootTable.Columns["LoadingFromCityName"].Width = 100;
			grdHistory.RootTable.Columns["UnloadingToCityName"].Width = 100;
			grdHistory.RootTable.Columns["PurchaseOrderNo"].Width = 70;
			grdHistory.RootTable.Columns["NoOfBags"].Width = 80;
			grdHistory.RootTable.Columns["NetWeightDo"].Width = 80;
			grdHistory.RootTable.Columns["NetWeightWB"].Width = 80;
			grdHistory.RootTable.Columns["BillWeight"].Width = 80;
			grdHistory.RootTable.Columns["FreightRate"].Width = 80;
			grdHistory.RootTable.Columns["RateUomCode"].Width = 60;
			grdHistory.RootTable.Columns["QtyforRate"].Width = 80;
			grdHistory.RootTable.Columns["BiltyFreight"].Width = 100;
			grdHistory.RootTable.Columns["OtherCharges"].Width = 100;
			grdHistory.RootTable.Columns["TotalFreight"].Width = 100;
			grdHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ApprovalUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 50, grdHistory.RootTable.Columns["DocNo"].Position - 1);
			GridEX_Helper.AddButton(grdHistory, "Print", "Print", 50, grdHistory.RootTable.Columns["DocNo"].Position - 1);
			GridEX_Helper.AddButton(grdHistory, "Voucher", "Voucher", 70, grdHistory.RootTable.Columns["DocNo"].Position - 1);
			grdHistory.FrozenColumns = 4;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("ypu don't have updae rights...");
				}
				ReadById(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			if (e.Column.Key == "Print")
			{
				if (!formright.DoHavePrintRights)
				{
					throw new Exception("you don't have print rights...");
				}
				FreightVoucherOutward_Slip(Conversion.ToInt(r.Cells["Id"].Value));
			}
			if (e.Column.Key == "Voucher")
			{
				if (!formright.DoHavePrintRights)
				{
					throw new Exception("you don't have print rights...");
				}
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(CommonServices.VoucherHeadIdGet(Conversion.ToInt(r.Cells["Id"].Value), Conversion.ToInt(r.Cells["DocumentTypeId"].Value)));
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			FreightVoucherOutward Obj = FreightVoucherOutward.ReadById(Id);
			if (Obj != null && Obj.FreightVoucherOutwardPaymentDetailList.Count == 0)
			{
				grdHistoryDetail.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtPaymentDetail.Clone();
			FreightVoucherOutward_Helper.FillPaymentDetailFromList(Obj, dtDetailHistory);
			grdHistoryDetail.DataSource = dtDetailHistory;
			grdHistoryDetail.RetrieveStructure();
			grddetailhistorySettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailhistorySettings()
	{
		try
		{
			grdCustomerDetailSetting(grdHistoryDetail);
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistory, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		if (FormInitialized)
		{
			GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdPaymentDetail, dtPaymentDetail);
		}
	}

	private void frmBillOfMaterial_Manufacturing_KeyDown(object sender, KeyEventArgs e)
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
					txtdocdate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)grdHistory).Focus();
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
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete)
				{
					if (formright.DoHaveCanDelete && btnDelete.Enabled)
					{
						btnDelete_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					if (btnPrint.Visible && btnPrint.Enabled)
					{
						btnPrint_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdocdate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdPaymentDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					txtdocdate.Focus();
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistoryDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdHistory).Focus();
				}
			}
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
			dt.Rows.Add("Ctrl+S", "For Save When on Entry form and For Show Data when on History Form");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("F1", "For Combo Lookup on Current Column of any Focused Grid");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Doc Date");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
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

	private void grdCustomerDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPaymentDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdPaymentDetail.CurrentColumn.Key == "Delete")
				{
					DeleteCustomerDetailRow(r);
				}
				grdPaymentDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteCustomerDetailRow(r);
				grdPaymentDetail.UpdateData();
			}
			if (e.KeyCode == Keys.F1)
			{
				grdPaymentDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					if (!formright.DoHaveUpdateRights)
					{
						throw new Exception("you don't have update rights...");
					}
					ReadById(Conversion.ToInt((object)CurrId));
				}
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					if (!formright.DoHavePrintRights)
					{
						throw new Exception("you don't have print rights...");
					}
					FreightVoucherOutward_Slip(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					if (!formright.DoHavePrintRights)
					{
						throw new Exception("you don't have print rights...");
					}
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(CommonServices.VoucherHeadIdGet(CurrId, Conversion.ToInt(r.Cells["DocumentTypeId"].Value)));
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, DocumentTypeId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)CurrId));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadGp_Click(object sender, EventArgs e)
	{
		try
		{
			int alreadySelectedGpId = Conversion.ToInt(CmbGpNo.Value);
			using frmLoadPendingGpForFreightVoucher loadGp = new frmLoadPendingGpForFreightVoucher(UserAccount);
			loadGp.AlreadySelectedGpId = alreadySelectedGpId;
			loadGp.FreightVoucherRecId = RecId;
			loadGp.ShowDialog();
			if (loadGp.dtLoader != null && loadGp.dtLoader.Rows.Count > 0)
			{
				int newSelectedGpId = Conversion.ToInt(loadGp.dtLoader.Rows[0]["GpId"]);
				if (alreadySelectedGpId != newSelectedGpId)
				{
					CmbGpNo.Value = newSelectedGpId;
					CmbGpNo_Leave(null, null);
				}
				CmbTransporter.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnOtherChargesItem_Click(object sender, EventArgs e)
	{
		try
		{
			new frmLogiticOtherChargesItems(UserAccount).Show();
			dtLogiticOtherChargesItems = LogiticOtherChargesItems.FormHistory(UserAccount.OrganizationId, UserAccount.CompanyId);
			ExpenseGridCombBind();
			GenerateRowsInExpenseGrid();
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
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Expected O, but got Unknown
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Expected O, but got Unknown
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Expected O, but got Unknown
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Expected O, but got Unknown
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Expected O, but got Unknown
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Expected O, but got Unknown
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Expected O, but got Unknown
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Expected O, but got Unknown
		//IL_0a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Expected O, but got Unknown
		//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a41: Expected O, but got Unknown
		//IL_0a58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Expected O, but got Unknown
		//IL_0a9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa4: Expected O, but got Unknown
		//IL_1003: Unknown result type (might be due to invalid IL or missing references)
		//IL_100d: Expected O, but got Unknown
		//IL_101a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1024: Expected O, but got Unknown
		//IL_1360: Unknown result type (might be due to invalid IL or missing references)
		//IL_136a: Expected O, but got Unknown
		//IL_1377: Unknown result type (might be due to invalid IL or missing references)
		//IL_1381: Expected O, but got Unknown
		//IL_8e5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_8e67: Expected O, but got Unknown
		//IL_8e8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_8e95: Expected O, but got Unknown
		GridEXLayout grdPaymentDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
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
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
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
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Service.lgstcm.frmFreightVoucherExport));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdPaymentDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdInvExp = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label81 = new System.Windows.Forms.Label();
		this.BtnOtherChargesItem = new System.Windows.Forms.Button();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.BalanceValue = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.BtnAddCustomer = new System.Windows.Forms.Button();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.BtnUpdateCustmer = new System.Windows.Forms.Button();
		this.label121 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.BtnCancelCustomer = new System.Windows.Forms.Button();
		this.txtPayeeTitle = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.CmbAccountTitle = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.datChequeDate = new System.Windows.Forms.DateTimePicker();
		this.label105 = new System.Windows.Forms.Label();
		this.CmbTransactionType = new UltraCombo();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label118 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.CmbInstrumentType = new UltraCombo();
		this.label56 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbChequeNo = new UltraCombo();
		this.txtChequeNo = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtNetWeightDo = new System.Windows.Forms.TextBox();
		this.txtBiltyDate = new System.Windows.Forms.DateTimePicker();
		this.txtBiltyNo = new System.Windows.Forms.TextBox();
		this.txtNetBillWeight = new System.Windows.Forms.TextBox();
		this.txtNetWeightWb = new System.Windows.Forms.TextBox();
		this.txtNoOfBags = new System.Windows.Forms.TextBox();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.CmbBillWeightBase = new UltraCombo();
		this.CmbLogisticPoNo = new UltraCombo();
		this.txtNetFreightPayable = new System.Windows.Forms.TextBox();
		this.txtAdvanceCashPaid = new System.Windows.Forms.TextBox();
		this.txtTotalBiltyFreight = new System.Windows.Forms.TextBox();
		this.txtOtherCharges = new System.Windows.Forms.TextBox();
		this.txtBiltyFreight = new System.Windows.Forms.TextBox();
		this.txtQtyForRate = new System.Windows.Forms.TextBox();
		this.txtFreightRate = new System.Windows.Forms.TextBox();
		this.CmbRateUom = new UltraCombo();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.CmbUnLoadingCity = new UltraCombo();
		this.CmbLoadingCity = new UltraCombo();
		this.CmbTransporter = new UltraCombo();
		this.CmbDeliveryOrderNo = new UltraCombo();
		this.CmbGpNo = new UltraCombo();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.label54 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtDeliveryOrderDate = new System.Windows.Forms.DateTimePicker();
		this.txtGpDate = new System.Windows.Forms.DateTimePicker();
		this.label36 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.label50 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.label21 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrintVoucher = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadGp = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.chkVoucher = new System.Windows.Forms.CheckBox();
		this.CmbChargesToDrAccount = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.grdHistoryDetail = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbDateTypeHistory = new UltraCombo();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label19 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.panel38 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label32 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel11.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInstrumentType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeNo).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBillWeightBase).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLogisticPoNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUnLoadingCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGpNo).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesToDrAccount).BeginInit();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1104, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1104, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentDetail);
		this.tabPage1.Controls.Add(this.panel14);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1096, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdPaymentDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPaymentDetail).BackColor = System.Drawing.Color.White;
		grdPaymentDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentDetail.DesignTimeLayout = grdPaymentDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Location = new System.Drawing.Point(3, 481);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Name = "grdPaymentDetail";
		((ScrollableBase)this.grdPaymentDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Size = new System.Drawing.Size(1090, 148);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).TabIndex = 0;
		this.grdPaymentDetail.TotalRow = (InheritableBoolean)1;
		this.grdPaymentDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentDetail).VisualStyle = (VisualStyle)4;
		this.grdPaymentDetail.CellUpdated += new ColumnActionEventHandler(grdCustomerDetail_CellUpdated);
		this.grdPaymentDetail.ColumnButtonClick += new ColumnActionEventHandler(grdCustomerDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).DoubleClick += new System.EventHandler(grdCustomerDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdCustomerDetail_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar3);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 460);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1090, 21);
		this.panel14.TabIndex = 131;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1044, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdPaymentDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar3.TabIndex = 127;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel5.Controls.Add(this.panel16);
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 59);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1090, 401);
		this.panel5.TabIndex = 130;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel16.Controls.Add(this.panel11);
		this.panel16.Location = new System.Drawing.Point(620, 128);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(452, 270);
		this.panel16.TabIndex = 2;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(452, 244);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInvExp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInvExp_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label81);
		this.panel11.Controls.Add(this.BtnOtherChargesItem);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.ForeColor = System.Drawing.Color.Teal;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(452, 26);
		this.panel11.TabIndex = 36;
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label81.ForeColor = System.Drawing.Color.White;
		this.label81.Location = new System.Drawing.Point(3, 7);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(185, 14);
		this.label81.TabIndex = 25;
		this.label81.Text = "Add / Less From Bilty Freight";
		this.BtnOtherChargesItem.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnOtherChargesItem.FlatAppearance.BorderSize = 0;
		this.BtnOtherChargesItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnOtherChargesItem.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.BtnOtherChargesItem.ForeColor = System.Drawing.Color.White;
		this.BtnOtherChargesItem.Location = new System.Drawing.Point(288, 2);
		this.BtnOtherChargesItem.Name = "BtnOtherChargesItem";
		this.BtnOtherChargesItem.Size = new System.Drawing.Size(159, 22);
		this.BtnOtherChargesItem.TabIndex = 10;
		this.BtnOtherChargesItem.Text = "Define Charges Item";
		this.BtnOtherChargesItem.UseVisualStyleBackColor = false;
		this.BtnOtherChargesItem.Visible = false;
		this.BtnOtherChargesItem.Click += new System.EventHandler(BtnOtherChargesItem_Click);
		this.groupBox3.Controls.Add(this.BalanceValue);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.BtnAddCustomer);
		this.groupBox3.Controls.Add(this.txtRemarks);
		this.groupBox3.Controls.Add(this.BtnUpdateCustmer);
		this.groupBox3.Controls.Add(this.label121);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.BtnCancelCustomer);
		this.groupBox3.Controls.Add(this.txtPayeeTitle);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitle);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.datChequeDate);
		this.groupBox3.Controls.Add(this.label105);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransactionType);
		this.groupBox3.Controls.Add(this.txtAmount);
		this.groupBox3.Controls.Add(this.label118);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInstrumentType);
		this.groupBox3.Controls.Add(this.label56);
		this.groupBox3.Controls.Add(this.label29);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChequeNo);
		this.groupBox3.Controls.Add(this.txtChequeNo);
		this.groupBox3.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.ForeColor = System.Drawing.Color.Blue;
		this.groupBox3.Location = new System.Drawing.Point(5, 247);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(609, 153);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Add multiple credit entries if required, or fill the fields below for a single entry";
		this.BalanceValue.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BalanceValue.ForeColor = System.Drawing.Color.Black;
		this.BalanceValue.Location = new System.Drawing.Point(369, 15);
		this.BalanceValue.Name = "BalanceValue";
		this.BalanceValue.Size = new System.Drawing.Size(101, 18);
		this.BalanceValue.TabIndex = 630;
		this.BalanceValue.Text = "0";
		this.BalanceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.Black;
		this.label16.Location = new System.Drawing.Point(114, 65);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(75, 15);
		this.label16.TabIndex = 626;
		this.label16.Text = "Cheque Date";
		this.BtnAddCustomer.BackColor = System.Drawing.Color.Teal;
		this.BtnAddCustomer.FlatAppearance.BorderSize = 0;
		this.BtnAddCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddCustomer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddCustomer.ForeColor = System.Drawing.Color.White;
		this.BtnAddCustomer.Location = new System.Drawing.Point(510, 129);
		this.BtnAddCustomer.Name = "BtnAddCustomer";
		this.BtnAddCustomer.Size = new System.Drawing.Size(39, 21);
		this.BtnAddCustomer.TabIndex = 8;
		this.BtnAddCustomer.Text = "+";
		this.BtnAddCustomer.UseVisualStyleBackColor = false;
		this.BtnAddCustomer.Click += new System.EventHandler(BtnAddCustomer_Click);
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(3, 127);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(505, 23);
		this.txtRemarks.TabIndex = 7;
		this.BtnUpdateCustmer.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateCustmer.FlatAppearance.BorderSize = 0;
		this.BtnUpdateCustmer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateCustmer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateCustmer.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateCustmer.Location = new System.Drawing.Point(510, 129);
		this.BtnUpdateCustmer.Name = "BtnUpdateCustmer";
		this.BtnUpdateCustmer.Size = new System.Drawing.Size(64, 21);
		this.BtnUpdateCustmer.TabIndex = 9;
		this.BtnUpdateCustmer.Text = "Update";
		this.BtnUpdateCustmer.UseVisualStyleBackColor = false;
		this.BtnUpdateCustmer.Visible = false;
		this.BtnUpdateCustmer.Click += new System.EventHandler(BtnUpdateCustmer_Click);
		this.label121.AutoSize = true;
		this.label121.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label121.ForeColor = System.Drawing.Color.Black;
		this.label121.Location = new System.Drawing.Point(3, 110);
		this.label121.Name = "label121";
		this.label121.Size = new System.Drawing.Size(52, 15);
		this.label121.TabIndex = 614;
		this.label121.Text = "Remarks";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.Black;
		this.label14.Location = new System.Drawing.Point(247, 17);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(109, 15);
		this.label14.TabIndex = 164;
		this.label14.Text = "Bill To Account (Cr)";
		this.BtnCancelCustomer.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelCustomer.FlatAppearance.BorderSize = 0;
		this.BtnCancelCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelCustomer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelCustomer.ForeColor = System.Drawing.Color.White;
		this.BtnCancelCustomer.Location = new System.Drawing.Point(510, 108);
		this.BtnCancelCustomer.Name = "BtnCancelCustomer";
		this.BtnCancelCustomer.Size = new System.Drawing.Size(64, 20);
		this.BtnCancelCustomer.TabIndex = 10;
		this.BtnCancelCustomer.Text = "Cancel";
		this.BtnCancelCustomer.UseVisualStyleBackColor = false;
		this.BtnCancelCustomer.Visible = false;
		this.BtnCancelCustomer.Click += new System.EventHandler(BtnCancelCustomer_Click);
		this.txtPayeeTitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPayeeTitle.Location = new System.Drawing.Point(245, 83);
		this.txtPayeeTitle.Name = "txtPayeeTitle";
		this.txtPayeeTitle.Size = new System.Drawing.Size(329, 23);
		this.txtPayeeTitle.TabIndex = 6;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Black;
		this.label25.Location = new System.Drawing.Point(245, 65);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(61, 15);
		this.label25.TabIndex = 623;
		this.label25.Text = "PayeeTitle";
		this.CmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Location = new System.Drawing.Point(245, 35);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Name = "CmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Leave += new System.EventHandler(CmbAccountTitle_Leave);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(355, 17);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(14, 15);
		this.label41.TabIndex = 509;
		this.label41.Text = "*";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.Red;
		this.label40.Location = new System.Drawing.Point(208, 17);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(14, 15);
		this.label40.TabIndex = 509;
		this.label40.Text = "*";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.Red;
		this.label39.Location = new System.Drawing.Point(99, 17);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(14, 15);
		this.label39.TabIndex = 509;
		this.label39.Text = "*";
		this.datChequeDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datChequeDate.CustomFormat = "dd-MMM-yyyy";
		this.datChequeDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.datChequeDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datChequeDate.Location = new System.Drawing.Point(114, 83);
		this.datChequeDate.Name = "datChequeDate";
		this.datChequeDate.Size = new System.Drawing.Size(130, 22);
		this.datChequeDate.TabIndex = 5;
		this.label105.AutoSize = true;
		this.label105.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label105.ForeColor = System.Drawing.Color.Black;
		this.label105.Location = new System.Drawing.Point(2, 17);
		this.label105.Name = "label105";
		this.label105.Size = new System.Drawing.Size(96, 15);
		this.label105.TabIndex = 598;
		this.label105.Text = "Transaction Type";
		this.CmbTransactionType.AllowNull = (DefaultableBoolean)1;
		this.CmbTransactionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransactionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransactionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransactionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransactionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransactionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransactionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransactionType).Location = new System.Drawing.Point(2, 35);
		((System.Windows.Forms.Control)(object)this.CmbTransactionType).Name = "CmbTransactionType";
		((System.Windows.Forms.Control)(object)this.CmbTransactionType).Size = new System.Drawing.Size(111, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransactionType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbTransactionType).Leave += new System.EventHandler(CmbTransactionType_Leave);
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(474, 37);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(100, 23);
		this.txtAmount.TabIndex = 3;
		this.txtAmount.Tag = "double";
		this.txtAmount.Text = "0";
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmount.TextChanged += new System.EventHandler(txtAmout_TextChanged);
		this.label118.AutoSize = true;
		this.label118.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label118.ForeColor = System.Drawing.Color.Black;
		this.label118.Location = new System.Drawing.Point(474, 17);
		this.label118.Name = "label118";
		this.label118.Size = new System.Drawing.Size(51, 15);
		this.label118.TabIndex = 608;
		this.label118.Text = "Amount";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.ForeColor = System.Drawing.Color.Black;
		this.label26.Location = new System.Drawing.Point(114, 17);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(94, 15);
		this.label26.TabIndex = 616;
		this.label26.Text = "Instrument Type";
		this.CmbInstrumentType.AllowNull = (DefaultableBoolean)1;
		this.CmbInstrumentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInstrumentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInstrumentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInstrumentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.CmbInstrumentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInstrumentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInstrumentType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInstrumentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInstrumentType).Location = new System.Drawing.Point(114, 35);
		((System.Windows.Forms.Control)(object)this.CmbInstrumentType).Name = "CmbInstrumentType";
		((System.Windows.Forms.Control)(object)this.CmbInstrumentType).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.CmbInstrumentType).TabIndex = 1;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.Color.Red;
		this.label56.Location = new System.Drawing.Point(537, 17);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(14, 15);
		this.label56.TabIndex = 509;
		this.label56.Text = "*";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.ForeColor = System.Drawing.Color.Black;
		this.label29.Location = new System.Drawing.Point(4, 65);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(66, 15);
		this.label29.TabIndex = 620;
		this.label29.Text = "Cheque No";
		this.CmbChequeNo.AllowNull = (DefaultableBoolean)1;
		this.CmbChequeNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChequeNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChequeNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance102;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChequeNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChequeNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Location = new System.Drawing.Point(3, 81);
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Name = "CmbChequeNo";
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.CmbChequeNo).TabIndex = 4;
		this.txtChequeNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtChequeNo.Location = new System.Drawing.Point(3, 83);
		this.txtChequeNo.Name = "txtChequeNo";
		this.txtChequeNo.Size = new System.Drawing.Size(110, 23);
		this.txtChequeNo.TabIndex = 3;
		this.groupBox2.Controls.Add(this.txtNetWeightDo);
		this.groupBox2.Controls.Add(this.txtBiltyDate);
		this.groupBox2.Controls.Add(this.txtBiltyNo);
		this.groupBox2.Controls.Add(this.txtNetBillWeight);
		this.groupBox2.Controls.Add(this.txtNetWeightWb);
		this.groupBox2.Controls.Add(this.txtNoOfBags);
		this.groupBox2.Controls.Add(this.txtVehicleNo);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBillWeightBase);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo);
		this.groupBox2.Controls.Add(this.txtNetFreightPayable);
		this.groupBox2.Controls.Add(this.txtAdvanceCashPaid);
		this.groupBox2.Controls.Add(this.txtTotalBiltyFreight);
		this.groupBox2.Controls.Add(this.txtOtherCharges);
		this.groupBox2.Controls.Add(this.txtBiltyFreight);
		this.groupBox2.Controls.Add(this.txtQtyForRate);
		this.groupBox2.Controls.Add(this.txtFreightRate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUnLoadingCity);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoadingCity);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransporter);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderNo);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGpNo);
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label49);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add(this.label47);
		this.groupBox2.Controls.Add(this.label46);
		this.groupBox2.Controls.Add(this.label55);
		this.groupBox2.Controls.Add(this.label45);
		this.groupBox2.Controls.Add(this.label60);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Controls.Add(this.label61);
		this.groupBox2.Controls.Add(this.label57);
		this.groupBox2.Controls.Add(this.label43);
		this.groupBox2.Controls.Add(this.label42);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label58);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.txtDeliveryOrderDate);
		this.groupBox2.Controls.Add(this.txtGpDate);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label59);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(4, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1068, 244);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.txtNetWeightDo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightDo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightDo.Location = new System.Drawing.Point(496, 138);
		this.txtNetWeightDo.Name = "txtNetWeightDo";
		this.txtNetWeightDo.Size = new System.Drawing.Size(116, 23);
		this.txtNetWeightDo.TabIndex = 15;
		this.txtNetWeightDo.Tag = "double";
		this.txtNetWeightDo.TextChanged += new System.EventHandler(txtNetBillWeight_TextChanged);
		this.txtBiltyDate.CustomFormat = "dd-MMM-yy";
		this.txtBiltyDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtBiltyDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtBiltyDate.Location = new System.Drawing.Point(496, 61);
		this.txtBiltyDate.Name = "txtBiltyDate";
		this.txtBiltyDate.Size = new System.Drawing.Size(116, 22);
		this.txtBiltyDate.TabIndex = 12;
		this.txtBiltyNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNo.Enabled = false;
		this.txtBiltyNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyNo.Location = new System.Drawing.Point(496, 37);
		this.txtBiltyNo.Name = "txtBiltyNo";
		this.txtBiltyNo.Size = new System.Drawing.Size(116, 23);
		this.txtBiltyNo.TabIndex = 11;
		this.txtNetBillWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetBillWeight.Enabled = false;
		this.txtNetBillWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetBillWeight.Location = new System.Drawing.Point(496, 216);
		this.txtNetBillWeight.Name = "txtNetBillWeight";
		this.txtNetBillWeight.Size = new System.Drawing.Size(116, 23);
		this.txtNetBillWeight.TabIndex = 18;
		this.txtNetBillWeight.Tag = "double";
		this.txtNetBillWeight.TextChanged += new System.EventHandler(txtNetBillWeight_TextChanged);
		this.txtNetWeightWb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightWb.Enabled = false;
		this.txtNetWeightWb.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightWb.Location = new System.Drawing.Point(496, 163);
		this.txtNetWeightWb.Name = "txtNetWeightWb";
		this.txtNetWeightWb.Size = new System.Drawing.Size(116, 23);
		this.txtNetWeightWb.TabIndex = 16;
		this.txtNetWeightWb.Tag = "double";
		this.txtNetWeightWb.TextChanged += new System.EventHandler(txtNetBillWeight_TextChanged);
		this.txtNoOfBags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfBags.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfBags.Location = new System.Drawing.Point(496, 113);
		this.txtNoOfBags.Name = "txtNoOfBags";
		this.txtNoOfBags.Size = new System.Drawing.Size(116, 23);
		this.txtNoOfBags.TabIndex = 14;
		this.txtNoOfBags.Tag = "double";
		this.txtNoOfBags.TextChanged += new System.EventHandler(txtNoOfBags_TextChanged);
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Enabled = false;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(496, 13);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(116, 23);
		this.txtVehicleNo.TabIndex = 10;
		this.CmbBillWeightBase.AllowNull = (DefaultableBoolean)1;
		this.CmbBillWeightBase.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBillWeightBase.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBillWeightBase.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBillWeightBase).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBillWeightBase).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBillWeightBase.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBillWeightBase).Location = new System.Drawing.Point(496, 188);
		((System.Windows.Forms.Control)(object)this.CmbBillWeightBase).Name = "CmbBillWeightBase";
		((System.Windows.Forms.Control)(object)this.CmbBillWeightBase).Size = new System.Drawing.Size(116, 26);
		((System.Windows.Forms.Control)(object)this.CmbBillWeightBase).TabIndex = 17;
		this.CmbBillWeightBase.ValueChanged += new System.EventHandler(CmbBillWeightBase_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbBillWeightBase).Leave += new System.EventHandler(CmbBillWeightBase_ValueChanged);
		this.CmbLogisticPoNo.AllowNull = (DefaultableBoolean)1;
		this.CmbLogisticPoNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLogisticPoNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLogisticPoNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLogisticPoNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo).Location = new System.Drawing.Point(496, 85);
		((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo).Name = "CmbLogisticPoNo";
		((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo).Size = new System.Drawing.Size(116, 26);
		((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo).Leave += new System.EventHandler(CmbLogisticPoNo_Leave);
		this.txtNetFreightPayable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetFreightPayable.Enabled = false;
		this.txtNetFreightPayable.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetFreightPayable.Location = new System.Drawing.Point(957, 98);
		this.txtNetFreightPayable.Name = "txtNetFreightPayable";
		this.txtNetFreightPayable.Size = new System.Drawing.Size(110, 23);
		this.txtNetFreightPayable.TabIndex = 26;
		this.txtAdvanceCashPaid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAdvanceCashPaid.Enabled = false;
		this.txtAdvanceCashPaid.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAdvanceCashPaid.Location = new System.Drawing.Point(957, 70);
		this.txtAdvanceCashPaid.Name = "txtAdvanceCashPaid";
		this.txtAdvanceCashPaid.Size = new System.Drawing.Size(110, 23);
		this.txtAdvanceCashPaid.TabIndex = 25;
		this.txtAdvanceCashPaid.TextChanged += new System.EventHandler(txtAdvanceCashPaid_TextChanged);
		this.txtTotalBiltyFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalBiltyFreight.Enabled = false;
		this.txtTotalBiltyFreight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalBiltyFreight.Location = new System.Drawing.Point(957, 42);
		this.txtTotalBiltyFreight.Name = "txtTotalBiltyFreight";
		this.txtTotalBiltyFreight.Size = new System.Drawing.Size(110, 23);
		this.txtTotalBiltyFreight.TabIndex = 24;
		this.txtTotalBiltyFreight.Tag = "double";
		this.txtTotalBiltyFreight.TextChanged += new System.EventHandler(txtOtherCharges_TextChanged);
		this.txtOtherCharges.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherCharges.Enabled = false;
		this.txtOtherCharges.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherCharges.Location = new System.Drawing.Point(957, 15);
		this.txtOtherCharges.Name = "txtOtherCharges";
		this.txtOtherCharges.Size = new System.Drawing.Size(110, 23);
		this.txtOtherCharges.TabIndex = 23;
		this.txtOtherCharges.Tag = "double";
		this.txtOtherCharges.TextChanged += new System.EventHandler(txtOtherCharges_TextChanged);
		this.txtBiltyFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyFreight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyFreight.Location = new System.Drawing.Point(701, 98);
		this.txtBiltyFreight.Name = "txtBiltyFreight";
		this.txtBiltyFreight.Size = new System.Drawing.Size(110, 23);
		this.txtBiltyFreight.TabIndex = 22;
		this.txtBiltyFreight.Tag = "double";
		this.txtBiltyFreight.TextChanged += new System.EventHandler(txtBiltyFreight_TextChanged);
		this.txtQtyForRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyForRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyForRate.Location = new System.Drawing.Point(701, 70);
		this.txtQtyForRate.Name = "txtQtyForRate";
		this.txtQtyForRate.Size = new System.Drawing.Size(110, 23);
		this.txtQtyForRate.TabIndex = 21;
		this.txtQtyForRate.Tag = "double";
		this.txtQtyForRate.TextChanged += new System.EventHandler(txtQtyForRate_TextChanged);
		this.txtFreightRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightRate.Location = new System.Drawing.Point(701, 15);
		this.txtFreightRate.Name = "txtFreightRate";
		this.txtFreightRate.Size = new System.Drawing.Size(110, 23);
		this.txtFreightRate.TabIndex = 19;
		this.txtFreightRate.Tag = "double";
		this.txtFreightRate.TextChanged += new System.EventHandler(txtFreightRate_TextChanged);
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance121).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(701, 40);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 20;
		this.CmbRateUom.ValueChanged += new System.EventHandler(CmbRateUom_Leave);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Leave += new System.EventHandler(CmbRateUom_Leave);
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(129, 15);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(120, 23);
		this.txtdocno.TabIndex = 0;
		this.CmbUnLoadingCity.AllowNull = (DefaultableBoolean)1;
		this.CmbUnLoadingCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUnLoadingCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUnLoadingCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance137).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUnLoadingCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUnLoadingCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUnLoadingCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUnLoadingCity).Location = new System.Drawing.Point(129, 152);
		((System.Windows.Forms.Control)(object)this.CmbUnLoadingCity).Name = "CmbUnLoadingCity";
		((System.Windows.Forms.Control)(object)this.CmbUnLoadingCity).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.CmbUnLoadingCity).TabIndex = 8;
		this.CmbLoadingCity.AllowNull = (DefaultableBoolean)1;
		this.CmbLoadingCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoadingCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoadingCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance142;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance148).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoadingCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLoadingCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).Location = new System.Drawing.Point(129, 124);
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).Name = "CmbLoadingCity";
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoadingCity).TabIndex = 7;
		this.CmbTransporter.AllowNull = (DefaultableBoolean)1;
		this.CmbTransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance155;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Location = new System.Drawing.Point(129, 96);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Name = "CmbTransporter";
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).TabIndex = 6;
		this.CmbTransporter.ValueChanged += new System.EventHandler(CmbTransporter_Leave);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Leave += new System.EventHandler(CmbTransporter_Leave);
		this.CmbDeliveryOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderNo).Location = new System.Drawing.Point(129, 68);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderNo).Name = "CmbDeliveryOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderNo).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderNo).TabIndex = 4;
		this.CmbDeliveryOrderNo.ValueChanged += new System.EventHandler(CmbDeliveryOrderNo_Leave);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderNo).Leave += new System.EventHandler(CmbDeliveryOrderNo_Leave);
		this.CmbGpNo.AllowNull = (DefaultableBoolean)1;
		this.CmbGpNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGpNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGpNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGpNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbGpNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGpNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Location = new System.Drawing.Point(129, 40);
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Name = "CmbGpNo";
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbGpNo).TabIndex = 2;
		this.CmbGpNo.ValueChanged += new System.EventHandler(CmbGpNo_Leave);
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Leave += new System.EventHandler(CmbGpNo_Leave);
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(129, 179);
		this.txtRemarksHeader.Multiline = true;
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(253, 60);
		this.txtRemarksHeader.TabIndex = 9;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.Red;
		this.label54.Location = new System.Drawing.Point(115, 46);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(14, 15);
		this.label54.TabIndex = 509;
		this.label54.Text = "*";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(115, 21);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 15);
		this.label53.TabIndex = 509;
		this.label53.Text = "*";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(115, 102);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(14, 15);
		this.label6.TabIndex = 509;
		this.label6.Text = "*";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.Red;
		this.label52.Location = new System.Drawing.Point(115, 73);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(14, 15);
		this.label52.TabIndex = 509;
		this.label52.Text = "*";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(688, 102);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(14, 15);
		this.label49.TabIndex = 509;
		this.label49.Text = "*";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.ForeColor = System.Drawing.Color.Red;
		this.label48.Location = new System.Drawing.Point(688, 74);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(14, 15);
		this.label48.TabIndex = 509;
		this.label48.Text = "*";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(688, 46);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(14, 15);
		this.label47.TabIndex = 509;
		this.label47.Text = "*";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.Red;
		this.label46.Location = new System.Drawing.Point(688, 19);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(14, 15);
		this.label46.TabIndex = 509;
		this.label46.Text = "*";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.ForeColor = System.Drawing.Color.Red;
		this.label55.Location = new System.Drawing.Point(481, 17);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(14, 15);
		this.label55.TabIndex = 509;
		this.label55.Text = "*";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(481, 41);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(14, 15);
		this.label45.TabIndex = 509;
		this.label45.Text = "*";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(481, 220);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(14, 15);
		this.label60.TabIndex = 509;
		this.label60.Text = "*";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.ForeColor = System.Drawing.Color.Red;
		this.label44.Location = new System.Drawing.Point(481, 65);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(14, 15);
		this.label44.TabIndex = 509;
		this.label44.Text = "*";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.ForeColor = System.Drawing.Color.Red;
		this.label61.Location = new System.Drawing.Point(481, 194);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(14, 15);
		this.label61.TabIndex = 509;
		this.label61.Text = "*";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.Red;
		this.label57.Location = new System.Drawing.Point(481, 170);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(14, 15);
		this.label57.TabIndex = 509;
		this.label57.Text = "*";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(481, 122);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(14, 15);
		this.label43.TabIndex = 509;
		this.label43.Text = "*";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(481, 145);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(14, 15);
		this.label42.TabIndex = 509;
		this.label42.Text = "*";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(115, 158);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(14, 15);
		this.label20.TabIndex = 509;
		this.label20.Text = "*";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Red;
		this.label13.Location = new System.Drawing.Point(115, 130);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(14, 15);
		this.label13.TabIndex = 509;
		this.label13.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(2, 158);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(88, 15);
		this.label18.TabIndex = 164;
		this.label18.Text = "UnLoading City";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(2, 130);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(73, 15);
		this.label12.TabIndex = 164;
		this.label12.Text = "Loading City";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(385, 194);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(92, 15);
		this.label58.TabIndex = 164;
		this.label58.Text = "Bill Weight Base";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(618, 46);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(59, 15);
		this.label30.TabIndex = 164;
		this.label30.Text = "Rate Uom";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(385, 91);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(43, 15);
		this.label7.TabIndex = 164;
		this.label7.Text = "P.O No";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(2, 102);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 164;
		this.label2.Text = "Transporter";
		this.txtDeliveryOrderDate.CustomFormat = "dd-MMM-yy";
		this.txtDeliveryOrderDate.Enabled = false;
		this.txtDeliveryOrderDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDeliveryOrderDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDeliveryOrderDate.Location = new System.Drawing.Point(251, 70);
		this.txtDeliveryOrderDate.Name = "txtDeliveryOrderDate";
		this.txtDeliveryOrderDate.Size = new System.Drawing.Size(130, 22);
		this.txtDeliveryOrderDate.TabIndex = 5;
		this.txtGpDate.CustomFormat = "dd-MMM-yy";
		this.txtGpDate.Enabled = false;
		this.txtGpDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtGpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGpDate.Location = new System.Drawing.Point(251, 42);
		this.txtGpDate.Name = "txtGpDate";
		this.txtGpDate.Size = new System.Drawing.Size(130, 22);
		this.txtGpDate.TabIndex = 3;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(816, 102);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(139, 15);
		this.label36.TabIndex = 131;
		this.label36.Text = "Net Credit To Transporter";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(252, 15);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(130, 22);
		this.txtdocdate.TabIndex = 1;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(816, 46);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(99, 15);
		this.label50.TabIndex = 131;
		this.label50.Text = "Total Bilty Freight";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(816, 74);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(114, 15);
		this.label34.TabIndex = 131;
		this.label34.Text = "Less Advance / Cash";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(816, 19);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(140, 15);
		this.label33.TabIndex = 131;
		this.label33.Text = "Add / Less Other Charges";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(385, 65);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(58, 15);
		this.label11.TabIndex = 131;
		this.label11.Text = "Bilty Date";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(618, 102);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(70, 15);
		this.label31.TabIndex = 131;
		this.label31.Text = "Bilty Freight";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(385, 41);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(49, 15);
		this.label10.TabIndex = 131;
		this.label10.Text = "Bilty No";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(385, 220);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(65, 15);
		this.label59.TabIndex = 131;
		this.label59.Text = "Bill Weight";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(618, 74);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(72, 15);
		this.label27.TabIndex = 131;
		this.label27.Text = "Qty For Rate";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(385, 167);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(98, 15);
		this.label51.TabIndex = 131;
		this.label51.Text = "Net Weight (Wb)";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(618, 19);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(70, 15);
		this.label24.TabIndex = 131;
		this.label24.Text = "Freight Rate";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(385, 142);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(95, 15);
		this.label23.TabIndex = 131;
		this.label23.Text = "Net Weight (Do)";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(385, 117);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(67, 15);
		this.label22.TabIndex = 131;
		this.label22.Text = "No Of Bags";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(385, 17);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(64, 15);
		this.label9.TabIndex = 131;
		this.label9.Text = "Vehicle No";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(2, 183);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 131;
		this.label1.Text = "Remarks";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(2, 19);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(110, 15);
		this.label37.TabIndex = 138;
		this.label37.Text = "Voucher No && Date";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(2, 74);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(102, 15);
		this.label8.TabIndex = 616;
		this.label8.Text = "Delivery Order No";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(2, 46);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(116, 15);
		this.label4.TabIndex = 616;
		this.label4.Text = "Gate Pass No && Date";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ChkPreview);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.chkVoucher);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChargesToDrAccount);
		this.panel3.Controls.Add(this.label38);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1090, 56);
		this.panel3.TabIndex = 0;
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(875, 32);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(114, 20);
		this.ChkPreview.TabIndex = 12;
		this.ChkPreview.Text = "Preview 1305";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(4, 32);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(197, 19);
		this.label21.TabIndex = 128;
		this.label21.Text = "Freight Voucher Export";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnAttachment, this.btnPrint, this.btnPrintVoucher, this.BtnLoadGp,
			this.btnshortcutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1090, 27);
		this.toolStrip1.TabIndex = 2;
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
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
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
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1305-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnPrintVoucher.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintVoucher.Name = "btnPrintVoucher";
		this.btnPrintVoucher.Size = new System.Drawing.Size(84, 24);
		this.btnPrintVoucher.Text = "Voucher";
		this.btnPrintVoucher.Click += new System.EventHandler(btnPrintVoucher_Click);
		this.BtnLoadGp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadGp.Image = Architecture.WinApp.Properties.Resources.download;
		this.BtnLoadGp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadGp.Name = "BtnLoadGp";
		this.BtnLoadGp.Size = new System.Drawing.Size(146, 24);
		this.BtnLoadGp.Text = "Gate Pass Loader";
		this.BtnLoadGp.Click += new System.EventHandler(BtnLoadGp_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.chkVoucher.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkVoucher.AutoSize = true;
		this.chkVoucher.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkVoucher.ForeColor = System.Drawing.Color.White;
		this.chkVoucher.Location = new System.Drawing.Point(997, 32);
		this.chkVoucher.Name = "chkVoucher";
		this.chkVoucher.Size = new System.Drawing.Size(80, 20);
		this.chkVoucher.TabIndex = 12;
		this.chkVoucher.Text = "Voucher";
		this.chkVoucher.UseVisualStyleBackColor = true;
		this.CmbChargesToDrAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbChargesToDrAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChargesToDrAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChargesToDrAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChargesToDrAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChargesToDrAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChargesToDrAccount).Location = new System.Drawing.Point(434, 29);
		((System.Windows.Forms.Control)(object)this.CmbChargesToDrAccount).Name = "CmbChargesToDrAccount";
		((System.Windows.Forms.Control)(object)this.CmbChargesToDrAccount).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbChargesToDrAccount).TabIndex = 0;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.White;
		this.label38.Location = new System.Drawing.Point(259, 34);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(171, 14);
		this.label38.TabIndex = 164;
		this.label38.Text = "Charges To Account (Dr)";
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage3.Controls.Add(this.panel9);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel38);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1096, 632);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 141);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1096, 274);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label15);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel9.Location = new System.Drawing.Point(0, 415);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1096, 24);
		this.panel9.TabIndex = 51;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(4, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(225, 16);
		this.label15.TabIndex = 129;
		this.label15.Text = "Detail of the Above Selected Row";
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(0, 439);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1096, 193);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 1;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 118);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1096, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1049, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(8, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(112, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "Filtered Records";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 52);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1096, 66);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox1.Controls.Add(this.rdapproveddate);
		this.groupBox1.Controls.Add(this.rdmodifydate);
		this.groupBox1.Controls.Add(this.drdocdate);
		this.groupBox1.Controls.Add(this.rdentrydate);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.ToDateHistory);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.ToDocNoHistory);
		this.groupBox1.Controls.Add(this.FromDateHistory);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.FromDocNoHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(823, 65);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(3, 35);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(156, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(713, 41);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 11;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(713, 22);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 10;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(633, 22);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 8;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(633, 41);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 9;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(573, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 25);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(3, 16);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 174;
		this.label19.Text = "Date Type";
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(294, 37);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(130, 22);
		this.ToDateHistory.TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(294, 16);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 15);
		this.label3.TabIndex = 171;
		this.label3.Text = "Date To";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(162, 16);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(63, 15);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(500, 37);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.Size = new System.Drawing.Size(70, 23);
		this.ToDocNoHistory.TabIndex = 4;
		this.ToDocNoHistory.Tag = "int";
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(162, 37);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(130, 22);
		this.FromDateHistory.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(500, 16);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(45, 15);
		this.label5.TabIndex = 173;
		this.label5.Text = "Doc To";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(426, 16);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(60, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From";
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(426, 37);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.Size = new System.Drawing.Size(70, 23);
		this.FromDocNoHistory.TabIndex = 3;
		this.FromDocNoHistory.Tag = "int";
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1096, 52);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1096, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnHistoryRefresh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "&Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(5, 31);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(197, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Freight Voucher Export";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1104, 661);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmFreightVoucherExport";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmFreightVoucherExport_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentDetail).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransactionType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInstrumentType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeNo).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBillWeightBase).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLogisticPoNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUnLoadingCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGpNo).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbChargesToDrAccount).EndInit();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
