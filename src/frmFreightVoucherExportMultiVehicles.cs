using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.lgstcm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Main;
using Architecture.Model.lgstcm;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.LogisticManagement.FromWise_Helper_Methods;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Service.lgstcm
{

public class frmFreightVoucherExportMultiVehicles : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtVoucherDetail = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtBillWeightBase = new DataTable();

	private DataTable dtRateUom = new DataTable();

	private DataTable dtCity = new DataTable();

	private string ScreenName = "frmFreightVoucherExportMultiVehicles";

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

	private List<FreightVoucherOutward> lstRemoveRecordcustomerDetail = new List<FreightVoucherOutward>();

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

	internal GridEX grdVoucher;

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

	private ToolStripButton btnPrintVoucher;

	private CheckBox chkVoucher;

	private Label label2;

	internal UltraCombo CmbTransporter;

	private TextBox txtQtyForRate;

	private TextBox txtFreightRate;

	private Label label30;

	private Label label7;

	private Label label27;

	private Label label24;

	internal UltraCombo CmbRateUom;

	internal UltraCombo CmbLogisticPoNo;

	internal UltraCombo CmbChargesToDrAccount;

	private Label label38;

	private Label label53;

	private Label label47;

	private Label label46;

	private Label label3;

	private Label label6;

	private GroupBox groupBox3;

	private Label label8;

	internal UltraCombo CmbCustomerLoader;

	private TextBox txtGpNoToLoader;

	private TextBox txtGpNoFromLoader;

	private Label label10;

	private DateTimePicker txtGpToDateLoader;

	private DateTimePicker txtGpFromDateLoader;

	private Label label12;

	private Button BtnShowLoader;

	private Button BtnApplyRate;

	internal UltraCombo CmbCityName;

	private Label label49;

	private Button BtnUpdateCityInGrid;

	private CheckBox chkUnLoadingCity;

	private CheckBox chkLoadingCity;

	public frmFreightVoucherExportMultiVehicles(UserAccount user)
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
			FreightVoucherOutward_Helper.InitializeMasterDataDetailtable(dtVoucherDetail);
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
			DataTable filterCombo = CustomerComboDbCall();
			return new
			{
				DocumentNoData = documentNoData,
				LookUpData = lookUpData,
				FilterCombo = filterCombo
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
				SupplierDtFillFromGlobal();
				SupplierBind();
				dtCityFillFromGlobal();
				AllLookUpComboBind(result.LookUpData);
				CustomerCombosFill(result.FilterCombo);
				DrAccountBindFromGlobal();
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
			return FreightVoucherOutward.GenerateMasterDocNo(new FreightVoucherOutward
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
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
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
				HeaderId = 1,
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
			DataTable dataTable = dtRateUom;
			if (dataTable != null && dataTable.Columns.Count == 0)
			{
				dtRateUom.Columns.Add("Id", typeof(int));
				dtRateUom.Columns.Add("Name");
				dtRateUom.Columns.Add("Equivalent", typeof(double));
			}
			DataTable dataTable2 = dtBillWeightBase;
			if (dataTable2 != null && dataTable2.Columns.Count == 0)
			{
				dtBillWeightBase.Columns.Add("Id", typeof(int));
				dtBillWeightBase.Columns.Add("Name");
			}
			dtRateUom.Clear();
			dtBillWeightBase.Clear();
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
						dtRateUom.Rows.Add(id, name, equivalent);
					}
				}
				else
				{
					dtBillWeightBase.Rows.Add(id, name);
				}
			}
			RateUomBind(dtRateUom);
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

	private void dtCityFillFromGlobal()
	{
		try
		{
			dtCity.Clear();
			List<GlobalAllCity> lst = clsGlobalVariables.globalAllCities;
			dtCity = DatatableHelper.PopulateDataTableAndReturn(lst, (GlobalAllCity x) => x.Id, (GlobalAllCity x) => x.CityName);
			InfragisticsHelper.BindAndRetainSelection(CmbCityName, dtCity, "Id", "Description", "City Name", AllColumns: true, Conversion.ToInt(CmbCityName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			UltraGridRow row = ((UltraGridBase)CmbLogisticPoNo).ActiveRow;
			if (CmbLogisticPoNo.Value != null && row != null)
			{
				int RateBaseUomId = Conversion.ToInt(row.Cells["RateBaseUomId"].Value);
				double Rate = Conversion.ToDouble(row.Cells["Rate"].Value);
				txtFreightRate.Text = Rate.ToString("#,##.##");
				CmbRateUom.Value = RateBaseUomId;
				((Control)(object)CmbRateUom).Enabled = false;
				txtFreightRate.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable CustomerComboDbCall()
	{
		try
		{
			return GatePassOutward.GetDataForDropDownFromGPO(UserAccount.OrganizationId, UserAccount.CompanyId, "Customer", (string)null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CustomerCombosFill(DataTable dtcombo)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerLoader, dtcombo, "Id", "ReferenceName", "Customer", AllColumns: false, Conversion.ToInt(CmbCustomerLoader.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowLoader_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		try
		{
			DataTable dt = FreightVoucherOutward.GatePassOutward_PendingForFreightVoucherOutward(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1303,
				FromDate = Conversion.ToDateTime((object)txtGpFromDateLoader.Value),
				ToDate = Conversion.ToDateTime((object)txtGpToDateLoader.Value),
				FromDocNo = Conversion.ToInt((object)txtGpNoFromLoader.Text),
				ToDocNo = Conversion.ToInt((object)txtGpNoToLoader.Text),
				SupplierCustomerId = Conversion.ToInt(CmbCustomerLoader.Value),
				ActionId = 1,
				RecId = RecId,
				HeaderId = 1
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> existingDetailIds = new HashSet<int>(from r in grdVoucher.GetRows()
				select Conversion.ToInt(r.Cells["GPId"].Value));
			foreach (DataRow dr in dt.Rows)
			{
				int gpId = Conversion.ToInt(dr["GpId"]);
				if (existingDetailIds.Add(gpId))
				{
					DataRow newRow = dtVoucherDetail.NewRow();
					DateTime docDate = ((!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dr["OutDateTime"]))) ? Conversion.ToDateTime(dr["OutDateTime"]) : DateTime.Now);
					DateTime BiltyDate = ((!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dr["BiltyDate"]))) ? Conversion.ToDateTime(dr["BiltyDate"]) : DateTime.Now);
					int BillBaseId = 24;
					newRow["Id"] = 0;
					newRow["DocDate"] = docDate;
					newRow["GPId"] = dr["GpId"];
					newRow["GpNo"] = dr["GpSrNo"];
					newRow["GpDate"] = dr["GpDate"];
					newRow["DoId"] = dr["DeliveryOrderId"];
					newRow["DoNo"] = dr["DeliveryOrderNo"];
					newRow["DoDate"] = dr["DeliveryOrderDate"];
					newRow["VehicleNo"] = dr["VehicleNo"];
					newRow["BiltyNo"] = dr["BiltyNo"];
					newRow["BiltyDate"] = BiltyDate;
					newRow["NoOfBages"] = dr["DoQty"];
					newRow["NetWeightDo"] = dr["DoWeight"];
					newRow["NetWeightWB"] = dr["WbNetWeight"];
					newRow["BillWeightBaseId"] = BillBaseId;
					newRow["BillWeight"] = dr["WbNetWeight"];
					newRow["BiltyFreight"] = dr["GpBiltyFreight"];
					newRow["TotalBiltyFreight"] = dr["GpBiltyFreight"];
					newRow["NetCreditToTransporter"] = dr["GpNetFreight"];
					dtVoucherDetail.Rows.Add(newRow);
				}
			}
			BindGrids();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrids()
	{
		try
		{
			DataTable dataTable = dtVoucherDetail;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grdVoucher.DataSource = dtVoucherDetail;
				grdVoucher.RetrieveStructure();
				grdVoucherSetting(grdVoucher);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdVoucherComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdVoucher.RootTable.Columns["LoadingCityId"], dtCity, "Id", "Description", "Loading City", 100);
			GridEX_Helper.ComboBind(grdVoucher.RootTable.Columns["UnLoadingCityId"], dtCity, "Id", "Description", "Un-Loading City", 100);
			GridEX_Helper.ComboBind(grdVoucher.RootTable.Columns["BillWeightBaseId"], dtBillWeightBase, "Id", "Name", "Bill Weight Base", 70);
			GridEX_Helper.ComboBind(grdVoucher.RootTable.Columns["RateUomId"], dtRateUom, "Id", "Name", "Rate Uom", 60);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdVoucherSetting(GridEX gridEX)
	{
		try
		{
			if (gridEX != null)
			{
				FreightVoucherOutward_Helper.VoucherDetailGridCommonSetting(gridEX, EditColumnsList());
				if (((Control)(object)gridEX).Name == "grdVoucher")
				{
					grdVoucherComboBind();
					GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
					gridEX.FrozenColumns = 3;
					ctrlGrdBar3_Load(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private Dictionary<string, EditType> EditColumnsList()
	{
		Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
		{
			{
				"DocDate",
				(EditType)2
			},
			{
				"BillWeightBaseId",
				(EditType)4
			},
			{
				"RateUomId",
				(EditType)4
			},
			{
				"LoadingCityId",
				(EditType)4
			},
			{
				"UnLoadingCityId",
				(EditType)4
			},
			{
				"BiltyDate",
				(EditType)2
			},
			{
				"NoOfBages",
				(EditType)1
			},
			{
				"OtherCharges",
				(EditType)1
			},
			{
				"Remarks",
				(EditType)1
			}
		};
		if (Conversion.ToInt(CmbLogisticPoNo.Value) == 0)
		{
			editableColumns["NetWeightDo"] = (EditType)1;
			editableColumns["FreightRate"] = (EditType)1;
			editableColumns["QtyForRate"] = (EditType)1;
			editableColumns["BiltyFreight"] = (EditType)1;
		}
		return editableColumns;
	}

	private void GridEditing()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Dictionary<string, EditType> editableColumns = EditColumnsList();
			foreach (GridEXColumn item in (JanusCollectionBase)grdVoucher.RootTable.Columns)
			{
				GridEXColumn col = item;
				if (editableColumns != null && editableColumns.Count > 0)
				{
					col.EditType = (EditType)((editableColumns != null && editableColumns.ContainsKey(col.Key)) ? ((int)editableColumns[col.Key]) : 0);
				}
				else
				{
					col.EditType = (EditType)0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdVoucher_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow row = grdVoucher.CurrentRow;
			if (row == null)
			{
				return;
			}
			string columnKey = e.Column.Key;
			switch (columnKey)
			{
			case "NoOfBages":
			case "NetWeightDo":
			case "NetWeightWB":
			case "BillWeightBaseId":
				switch (Conversion.ToInt(row.Cells["BillWeightBaseId"].Value))
				{
				case 23:
					row.Cells["BillWeight"].Value = Conversion.ToDouble(row.Cells["NetWeightDo"].Value);
					break;
				case 24:
					row.Cells["BillWeight"].Value = Conversion.ToDouble(row.Cells["NetWeightWB"].Value);
					break;
				}
				RecalculateFreight(row);
				break;
			}
			if (columnKey == "RateUomId" || columnKey == "BillWeight")
			{
				double billWeight = Conversion.ToDouble(row.Cells["BillWeight"].Value);
				double qtyForRate = Conversion.ToInt(row.Cells["RateUomId"].Value) switch
				{
					3 => billWeight / 1000.0, 
					4 => billWeight, 
					_ => 1.0, 
				};
				row.Cells["QtyForRate"].Value = qtyForRate;
				RecalculateFreight(row);
			}
			switch (columnKey)
			{
			case "FreightRate":
			case "QtyForRate":
			case "OtherCharges":
			case "AdvanceOrCashFreight":
				if (Conversion.ToDouble(row.Cells["OtherCharges"].Value) < 0.0)
				{
					row.Cells["OtherCharges"].Value = 0;
					MessageBox.Show("Invalid Other Charges Amount");
				}
				RecalculateFreight(row);
				break;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RecalculateFreight(GridEXRow row)
	{
		double num = Conversion.ToDouble(row.Cells["FreightRate"].Value);
		double qty = Conversion.ToDouble(row.Cells["QtyForRate"].Value);
		double otherCharges = Conversion.ToDouble(row.Cells["OtherCharges"].Value);
		double biltyFreight = num * qty;
		row.Cells["BiltyFreight"].Value = biltyFreight;
		double TotalFreight = biltyFreight + otherCharges;
		row.Cells["TotalBiltyFreight"].Value = TotalFreight;
		double advance = Conversion.ToDouble(row.Cells["AdvanceOrCashFreight"].Value);
		row.Cells["NetCreditToTransporter"].Value = TotalFreight - advance;
	}

	private void grdVoucher_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdVoucher.CurrentRow;
			if (r != null)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteCustomerDetailRow(r);
				}
				grdVoucher.UpdateData();
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
						FreightVoucherOutward d = new FreightVoucherOutward();
						d.FreightVoucherOutwardId = Conversion.ToInt(r.Cells["Id"].Value);
						FillDetailListCommonForInsertAndDelete(d, r);
						d.ActionId = 3;
						lstRemoveRecordcustomerDetail.Add(d);
						dtVoucherDetail.Rows[r.RowIndex].Delete();
						r.Delete();
					}
				}
				else
				{
					dtVoucherDetail.Rows[r.RowIndex].Delete();
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

	private void BtnApplyRate_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdVoucher.RowCount == 0)
			{
				return;
			}
			int poId = Conversion.ToInt(CmbLogisticPoNo.Value);
			int PoNo = Conversion.ToInt((object)((Control)(object)CmbLogisticPoNo).Text);
			double freightRate = Conversion.ToDouble((object)txtFreightRate.Text);
			int rateUomId = Conversion.ToInt(CmbRateUom.Value);
			GridEXRow[] dataRows = grdVoucher.GetDataRows();
			foreach (GridEXRow row in dataRows)
			{
				row.BeginEdit();
				row.Cells["PoId"].Value = poId;
				row.Cells["PoNo"].Value = PoNo;
				double billWeight = Conversion.ToDouble(row.Cells["BillWeight"].Value);
				double qtyForRate = rateUomId switch
				{
					3 => billWeight / 1000.0, 
					4 => billWeight, 
					_ => 1.0, 
				};
				if (poId > 0)
				{
					row.Cells["FreightRate"].Value = freightRate;
					row.Cells["RateUomId"].Value = rateUomId;
					row.Cells["QtyForRate"].Value = qtyForRate;
				}
				else
				{
					if (Conversion.ToDouble(row.Cells["FreightRate"].Value) == 0.0)
					{
						row.Cells["FreightRate"].Value = freightRate;
					}
					if (Conversion.ToDouble(row.Cells["RateUomId"].Value) == 0.0)
					{
						row.Cells["RateUomId"].Value = rateUomId;
					}
					if (Conversion.ToDouble(row.Cells["QtyForRate"].Value) == 0.0)
					{
						row.Cells["QtyForRate"].Value = qtyForRate;
					}
				}
				RecalculateFreight(row);
				row.EndEdit();
			}
			GridEditing();
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
			((Control)(object)CmbTransporter).Text = string.Empty;
			txtRemarksHeader.Text = string.Empty;
			((Control)(object)CmbLogisticPoNo).Text = string.Empty;
			dtVoucherDetail.Rows.Clear();
			BindGrids();
			txtFreightRate.Text = string.Empty;
			txtQtyForRate.Text = string.Empty;
			txtdocno.Text = DocumentNoDbCall();
			CmbTransporter_Leave(null, null);
			((Control)(object)CmbRateUom).Enabled = true;
			txtFreightRate.Enabled = true;
			txtdocdate.Focus();
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
			SupplierDtFillFromGlobal();
			SupplierBind();
			dtCityFillFromGlobal();
			AllLookUpComboBind(LookUps.LookUpAllServices(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null));
			DrAccountBindFromGlobal();
			CustomerCombosFill(CustomerComboDbCall());
			GetConfigurationsFromGlobalandBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmFreightVoucherExportMultiVehicles_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		try
		{
			if (grdVoucher.RowCount == 0)
			{
				MessageBox.Show("Please add at least one record in the grid.");
			}
			else
			{
				if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
				{
					(txtdocno, "Doc No", FormHelper.ValidType.INT),
					(txtdocdate, "Doc Date", FormHelper.ValidType.DateTime),
					((Control)(object)CmbTransporter, "Transporter", FormHelper.ValidType.INT)
				}))
				{
					return;
				}
				if (CmbChargesToDrAccount.Value == null || Conversion.ToInt(CmbChargesToDrAccount.Value) == 0)
				{
					MessageBox.Show("Please select a Charges To Account (Dr).\n\nIf the list is empty, go to Configuration, set the default Charges Account, and then click Refresh and then Select an Account.", "Missing Required Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					CmbChargesToDrAccount.Focus();
				}
				else
				{
					if (!FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
					{
						return;
					}
					FreightVoucherOutward Or = new FreightVoucherOutward();
					Or.MasterDocId = RecId;
					Or.CompanyId = UserAccount.CompanyId;
					Or.BranchesId = UserAccount.BranchesId;
					Or.ProjectsId = UserAccount.BranchesId;
					Or.OrganizationId = UserAccount.OrganizationId;
					Or.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					Or.documentTypeId = DocumentTypeId;
					Or.MasterDocNo = Conversion.ToInt((object)txtdocno.Text);
					Or.MasterDocDate = Conversion.ToDateTime((object)txtdocdate.Value);
					Or.MasterDocRemarks = Conversion.ToString((object)txtRemarksHeader.Text);
					Or.EntryUserId = UserAccount.ID;
					Or.EntryDate = DateTime.Now;
					Or.ModifyDate = DateTime.Now;
					Or.ModifyUserId = UserAccount.ID;
					Or.ApprovedUserId = UserAccount.ID;
					Or.ApprovedDate = DateTime.Now;
					Or.IsApproved = false;
					Or.FreightVoucherOutwardList = new List<FreightVoucherOutward>();
					Or.FreightVoucherOutwardPaymentDetailList = new List<FreightVoucherOutwardPaymentDetail>();
					Or.FreightVoucherOutwardExpenseDetailList = new List<FreightVoucherOutwardExpenseDetail>();
					if (lstRemoveRecordcustomerDetail.Count > 0 && RecId > 0)
					{
						for (int i = 0; i < lstRemoveRecordcustomerDetail.Count; i++)
						{
							Or.FreightVoucherOutwardList.Add(lstRemoveRecordcustomerDetail[i]);
						}
					}
					GridEXRow[] rows = grdVoucher.GetRows();
					foreach (GridEXRow r in rows)
					{
						FreightVoucherOutward vd = new FreightVoucherOutward();
						vd.FreightVoucherOutwardId = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
						vd.ActionId = ((vd.FreightVoucherOutwardId <= 0) ? 1 : 2);
						FillDetailListCommonForInsertAndDelete(vd, r);
						if (vd.PurchaseOrderHeaderId > 0)
						{
							if (vd.rateUomId != Conversion.ToInt(CmbRateUom.Value))
							{
								MessageBox.Show($"Order RateUom and RateUom in Detail Grid at row No: {r.RowIndex + 1} are not same. " + "Please press Apply button if you have changed the Purchase Order.");
								return;
							}
							if (vd.freightRate != Conversion.ToDecimal((object)txtFreightRate.Text))
							{
								MessageBox.Show($"Order Rate and Rate in Detail Grid at row No: {r.RowIndex + 1} are not same. " + "Please press Apply button if you have changed the Purchase Order.");
								return;
							}
						}
						FormHelper.ValidateField(vd.GatePassOutwardId, "GP No", r.RowIndex);
						FormHelper.ValidateField(vd.InvDeliveryOrderId, "Do No", r.RowIndex);
						FormHelper.ValidateField(vd.InvDeliveryOrderId, "Do No", r.RowIndex);
						FormHelper.ValidateField(vd.loadingFromCityId, "Loading City", r.RowIndex);
						FormHelper.ValidateField(vd.unloadingToCityId, "Un-Loading City", r.RowIndex);
						if (vd.loadingFromCityId == vd.unloadingToCityId)
						{
							MessageBox.Show($"Loading City and Unloading City cannot be the same in Detail Grid at row No: {r.RowIndex + 1}");
							return;
						}
						FormHelper.ValidateField(vd.vehicleNo, "Vehicle No", r.RowIndex);
						FormHelper.ValidateField(vd.biltyNo, "Bilty No", r.RowIndex);
						FormHelper.ValidateField(vd.NoOfBags, "No Of Bags", r.RowIndex);
						FormHelper.ValidateField(vd.BillWeightBaseId, "BillWeightBaseId", r.RowIndex);
						FormHelper.ValidateField(vd.netWeight, "BillWeight", r.RowIndex);
						FormHelper.ValidateField(vd.freightRate, "FreightRate", r.RowIndex);
						FormHelper.ValidateField(vd.rateUomId, "Rate Uom", r.RowIndex);
						FormHelper.ValidateField(vd.QtyforRate, "QtyForRate", r.RowIndex);
						FormHelper.ValidateField(vd.biltyFreight, "BiltyFreight", r.RowIndex);
						FormHelper.ValidateField(vd.totalbiltyfreight, "TotalBiltyFreight", r.RowIndex);
						if (string.IsNullOrWhiteSpace(Or.MasterDocRemarks))
						{
							Or.MasterDocRemarks = vd.RemarksHeader;
						}
						Or.FreightVoucherOutwardList.Add(vd);
						vd.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
						vd.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
						FormHelper.UpdateAttachmentsForObject<FreightVoucherOutward>(vd, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
						vd.AttachmentsValues = AttachmentsValues;
						vd.CustomAttachmentsValues = CustomAttachmentsValues;
					}
					int success = FreightVoucherOutward.SaveMulti(Or);
					Or.FreightVoucherOutwardId = success;
					if (RecId > 0)
					{
						MessageBox.Show("Data Update Successfully....  " + Or.MasterDocNo);
						foreach (FreightVoucherOutward item in Or.FreightVoucherOutwardList)
						{
							if (item.FreightVoucherOutwardId > 0)
							{
								FormHelper.DeleteAllAttachmentsFromDb(AT, item.FreightVoucherOutwardId, DocumentTypeId, base.Name);
							}
						}
					}
					else
					{
						MessageBox.Show("Data Save Successfully....  " + Or.MasterDocNo);
					}
					FromReset();
					if (ChkPreview.Checked && formright.DoHavePrintRights)
					{
						FreightVoucherOutward_Slip(Or.FreightVoucherOutwardId);
					}
					if (chkVoucher.Checked && formright.DoHavePrintRights)
					{
						PrintVoucher(Or.MasterDocId);
					}
				}
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
			List<FreightVoucherOutward> Or = FreightVoucherOutward.ReadByMasterId(RecId);
			if (Or == null || Or.Count == 0)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			tabControl1.SelectedIndex = 0;
			UpdateMode = true;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtdocdate.Value = Conversion.ToDateTime((object)Or[0].MasterDocDate);
			txtdocno.Text = Conversion.ToString((object)Or[0].MasterDocNo);
			CmbTransporter.Value = Conversion.ToInt((object)Or[0].transporterId);
			CmbTransporter_Leave(null, null);
			CmbLogisticPoNo.Value = Conversion.ToInt((object)Or[0].PurchaseOrderHeaderId);
			CmbLogisticPoNo_Leave(null, null);
			txtRemarksHeader.Text = Conversion.ToString((object)Or[0].MasterDocRemarks);
			CmbChargesToDrAccount.Value = Conversion.ToInt((object)Or[0].chargeToDrAccountId);
			AttachmentsValues = Or[0].AttachmentsValues;
			CustomAttachmentsValues = Or[0].CustomAttachmentsValues;
			dtVoucherDetail.Rows.Clear();
			FreightVoucherOutward_Helper.FillMasterVoucherDetailFromList(Or, dtVoucherDetail);
			BindGrids();
			FormHelper.LoadAttachmentsForObject(AT, Or[0].FreightVoucherOutwardId, base.Name);
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
					FreightVoucherOutward.DeleteByMasterDocID(UserAccount.ID, RecId);
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

	private void FillDetailListCommonForInsertAndDelete(FreightVoucherOutward vd, GridEXRow r)
	{
		vd.MasterDocId = RecId;
		vd.CompanyId = UserAccount.CompanyId;
		vd.BranchesId = UserAccount.BranchesId;
		vd.ProjectsId = UserAccount.BranchesId;
		vd.OrganizationId = UserAccount.OrganizationId;
		vd.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
		vd.documentTypeId = DocumentTypeId;
		vd.EntryUserId = UserAccount.ID;
		vd.EntryDate = DateTime.Now;
		vd.ModifyDate = DateTime.Now;
		vd.ModifyUserId = UserAccount.ID;
		vd.ApprovedUserId = UserAccount.ID;
		vd.ApprovedDate = DateTime.Now;
		vd.IsApproved = false;
		vd.MasterDocDate = Conversion.ToDateTime((object)txtdocdate.Value);
		vd.MasterDocNo = Conversion.ToInt((object)txtdocno.Text);
		vd.MasterDocRemarks = Conversion.ToString((object)txtRemarksHeader.Text);
		vd.transporterId = Conversion.ToInt(CmbTransporter.Value);
		vd.TransporterSupCustId = Conversion.ToInt(((UltraDropDownBase)CmbTransporter).SelectedRow.Cells[3].Value);
		vd.DocNo = Conversion.ToInt(r.Cells["DocNo"].Value);
		vd.DocDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
		vd.GatePassOutwardId = Conversion.ToInt(r.Cells["GPId"].Value);
		vd.GpSrNo = Conversion.ToInt(r.Cells["GpNo"].Value);
		vd.GpDate = Conversion.ToDateTime(r.Cells["GpDate"].Value);
		vd.InvDeliveryOrderId = Conversion.ToInt(r.Cells["DoId"].Value);
		vd.DeliveryOrderNo = Conversion.ToInt(r.Cells["DoNo"].Value);
		vd.DeliveryOrderDate = Conversion.ToDateTime(r.Cells["DoDate"].Value);
		vd.loadingFromCityId = Conversion.ToInt(r.Cells["LoadingCityId"].Value);
		vd.LoadingFromCityName = Conversion.ToString((object)r.Cells["LoadingCityId"].Text);
		vd.unloadingToCityId = Conversion.ToInt(r.Cells["UnLoadingCityId"].Value);
		vd.UnloadingToCityName = Conversion.ToString((object)r.Cells["UnLoadingCityId"].Text);
		vd.vehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
		vd.biltyNo = Conversion.ToString(r.Cells["BiltyNo"].Value);
		vd.biltyDate = Conversion.ToDateTime(r.Cells["BiltyDate"].Value);
		vd.PurchaseOrderHeaderId = Conversion.ToInt(r.Cells["PoId"].Value);
		vd.PurchaseOrderNo = Conversion.ToInt(r.Cells["PoNo"].Value);
		vd.NoOfBags = Conversion.ToDecimal(r.Cells["NoOfBages"].Value);
		vd.NetWeightDo = Conversion.ToDecimal(r.Cells["NetWeightDo"].Value);
		vd.NetWeightWB = Conversion.ToDecimal(r.Cells["NetWeightWB"].Value);
		vd.BillWeightBaseId = Conversion.ToInt(r.Cells["BillWeightBaseId"].Value);
		vd.BillWeightBase = Conversion.ToString((object)r.Cells["BillWeightBaseId"].Text);
		vd.netWeight = Conversion.ToDecimal(r.Cells["BillWeight"].Value);
		vd.freightRate = Conversion.ToDecimal(r.Cells["FreightRate"].Value);
		vd.rateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
		vd.RateUomCode = Conversion.ToString((object)r.Cells["RateUomId"].Text);
		vd.QtyforRate = Conversion.ToDecimal(r.Cells["QtyForRate"].Value);
		vd.biltyFreight = Conversion.ToDecimal(r.Cells["BiltyFreight"].Value);
		vd.otherCharges = Conversion.ToDecimal(r.Cells["OtherCharges"].Value);
		vd.totalbiltyfreight = Conversion.ToDecimal(r.Cells["TotalBiltyFreight"].Value);
		vd.AdvanceOrCashFreight = Conversion.ToDecimal(r.Cells["AdvanceOrCashFreight"].Value);
		vd.totalFreight = Conversion.ToDecimal(r.Cells["NetCreditToTransporter"].Value);
		vd.RemarksHeader = Conversion.ToString(r.Cells["Remarks"].Value);
		vd.chargeToDrAccountId = Conversion.ToInt(CmbChargesToDrAccount.Value);
		vd.ChargedToDrAccountTitle = Conversion.ToString((object)((Control)(object)CmbChargesToDrAccount).Text);
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
			CrystalReportPrint_Helper.FreightVoucherOutward_Slip_1305_01(PrintId);
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
			PrintVoucher(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PrintVoucher(int PrintId)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		try
		{
			if (PrintId > 0)
			{
				string HeaderIds = "";
				DataTable dt = FreightVoucherOutward.FreightVoucherOutward_GetVoucherHeadIds(new FreightVoucherOutward
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BranchesId = UserAccount.BranchesId,
					documentTypeId = DocumentTypeId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					MasterDocId = RecId
				});
				if (dt.Rows.Count > 0)
				{
					foreach (DataRow dr in dt.Rows)
					{
						HeaderIds = HeaderIds + dr["VoucherHeadId"].ToString() + ",";
					}
					HeaderIds = HeaderIds.TrimEnd(',');
				}
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(0, HeaderIds);
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
					dtHistoryGrid.Columns.Add("TransporterId", typeof(int));
					dtHistoryGrid.Columns.Add("TransporterAccountTitle", typeof(string));
					dtHistoryGrid.Columns.Add("PurchaseOrderHeaderId", typeof(int));
					dtHistoryGrid.Columns.Add("PurchaseOrderNo", typeof(string));
					dtHistoryGrid.Columns.Add("ChargeToDrAccountId", typeof(int));
					dtHistoryGrid.Columns.Add("ChargedToDrAccountTitle", typeof(string));
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("EntryUserName", typeof(string));
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ModifyUserName", typeof(string));
					dtHistoryGrid.Columns.Add("IsApproved", typeof(bool));
					dtHistoryGrid.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ApprovalUserName", typeof(string));
					dtHistoryGrid.Columns.Add("RemarksHeader", typeof(string));
					dtHistoryGrid.Columns.Add("NoOfAttachments", typeof(int));
					dtHistoryGrid.Columns.Add("FreightVoucherOutwardId", typeof(int));
				}
				dtHistoryGrid.Rows.Clear();
				HashSet<int> Ids = new HashSet<int>();
				foreach (DataRow row in dtHistory.Rows)
				{
					int Id = Conversion.ToInt(row["MasterDocId"]);
					if (Ids.Add(Id))
					{
						dtHistoryGrid.Rows.Add(Id, row["MasterDocNo"], row["MasterDocDate"], row["DocumentTypeId"], row["TransporterId"], row["TransporterAccountTitle"], row["PurchaseOrderHeaderId"], row["PurchaseOrderNo"], row["ChargeToDrAccountId"], row["ChargedToDrAccountTitle"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["IsApproved"], row["ApprovedDate"], row["ApprovalUserName"], row["RemarksHeader"], row["NoOfAttachments"], row["FreightVoucherOutwardId"]);
					}
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
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "TransporterId", "ChargeToDrAccountId", "PurchaseOrderHeaderId", "FreightVoucherOutwardId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 3, 3, hiddenColumns);
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["TransporterAccountTitle"].Width = 150;
			grdHistory.RootTable.Columns["ChargedToDrAccountTitle"].Width = 150;
			grdHistory.RootTable.Columns["PurchaseOrderNo"].Width = 70;
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
				PrintVoucher(Conversion.ToInt(r.Cells["Id"].Value));
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
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["FreightVoucherOutwardId"].Value), DocumentTypeId);
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
			List<FreightVoucherOutward> Obj = FreightVoucherOutward.ReadByMasterId(Id);
			if (Obj != null && Obj.Count == 0)
			{
				grdHistoryDetail.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtVoucherDetail.Clone();
			dtDetailHistory.Columns.Add("LoadingCity", typeof(string));
			dtDetailHistory.Columns.Add("UnLoadingCity", typeof(string));
			dtDetailHistory.Columns.Add("BillWeightBase", typeof(string));
			dtDetailHistory.Columns.Add("RateUom", typeof(string));
			FreightVoucherOutward_Helper.FillMasterVoucherDetailFromList(Obj, dtDetailHistory, ExtraColumns: true);
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
			grdVoucherSetting(grdHistoryDetail);
			grdHistoryDetail.RootTable.Columns["LoadingCityId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["UnLoadingCityId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["BillWeightBaseId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["RateUomId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["LoadingCity"].Position = grdHistoryDetail.RootTable.Columns["LoadingCityId"].Position + 1;
			grdHistoryDetail.RootTable.Columns["UnLoadingCity"].Position = grdHistoryDetail.RootTable.Columns["UnLoadingCityId"].Position + 1;
			grdHistoryDetail.RootTable.Columns["BillWeightBase"].Position = grdHistoryDetail.RootTable.Columns["BillWeightBaseId"].Position + 1;
			grdHistoryDetail.RootTable.Columns["RateUom"].Position = grdHistoryDetail.RootTable.Columns["RateUomId"].Position + 1;
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
			GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdVoucher, dtVoucherDetail);
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
					((Control)(object)grdVoucher).Focus();
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

	private void grdVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdVoucher.CurrentRow;
			GridEXColumn column = grdVoucher.CurrentColumn;
			if (row == null || (int)row.RowType != 0 || row.Cells.Count == 0 || column == null)
			{
				return;
			}
			string columnKey = column.Key;
			if (e.Control && e.KeyCode == Keys.Space && columnKey == "Delete")
			{
				DeleteCustomerDetailRow(row);
				grdVoucher.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteCustomerDetailRow(row);
				grdVoucher.UpdateData();
			}
			else
			{
				if (e.KeyCode != Keys.F1)
				{
					return;
				}
				switch (columnKey)
				{
				case "LoadingCityId":
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtCity, "Id", "Description", row, "LoadingCityId");
					break;
				case "UnLoadingCityId":
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtCity, "Id", "Description", row, "UnLoadingCityId");
					break;
				case "BillWeightBaseId":
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtBillWeightBase, "Id", "Name", row, "BillWeightBaseId");
					switch (Conversion.ToInt(row.Cells["BillWeightBaseId"].Value))
					{
					case 23:
						row.Cells["BillWeight"].Value = Conversion.ToDouble(row.Cells["NetWeightDo"].Value);
						break;
					case 24:
						row.Cells["BillWeight"].Value = Conversion.ToDouble(row.Cells["NetWeightWB"].Value);
						break;
					}
					RecalculateFreight(row);
					break;
				case "RateUomId":
				{
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtRateUom, "Id", "Name", row, "RateUomId");
					int rateUomId = Conversion.ToInt(row.Cells["RateUomId"].Value);
					double billWeight = Conversion.ToDouble(row.Cells["BillWeight"].Value);
					double qtyForRate = rateUomId switch
					{
						3 => billWeight / 1000.0, 
						4 => billWeight, 
						_ => 1.0, 
					};
					row.Cells["QtyForRate"].Value = qtyForRate;
					RecalculateFreight(row);
					break;
				}
				}
				grdVoucher.UpdateData();
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
					PrintVoucher(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["FreightVoucherOutwardId"].Value), DocumentTypeId);
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

	private void BtnUpdateCityInGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (!chkLoadingCity.Checked && !chkUnLoadingCity.Checked)
			{
				throw new Exception("Please select Loading or UnLoading city option.");
			}
			GridEXRow[] rows = grdVoucher.GetDataRows();
			if (rows == null || rows.Count() == 0)
			{
				throw new Exception("Detail record not found.");
			}
			int selectedCityId = Conversion.ToInt(CmbCityName.Value);
			if (selectedCityId == 0)
			{
				GridEXRow obj = rows.First();
				int firstLoadingCityId = Conversion.ToInt(obj.Cells["LoadingCityId"].Value);
				int firstUnLoadingCityId = Conversion.ToInt(obj.Cells["UnLoadingCityId"].Value);
				if (chkLoadingCity.Checked && firstLoadingCityId == 0)
				{
					throw new Exception("Loading City is empty in detail row no: 01");
				}
				if (chkUnLoadingCity.Checked && firstUnLoadingCityId == 0)
				{
					throw new Exception("UnLoading City is empty in detail row no: 01");
				}
				GridEXRow[] array = rows;
				foreach (GridEXRow r in array)
				{
					r.BeginEdit();
					if (chkLoadingCity.Checked)
					{
						r.Cells["LoadingCityId"].Value = firstLoadingCityId;
					}
					if (chkUnLoadingCity.Checked)
					{
						r.Cells["UnLoadingCityId"].Value = firstUnLoadingCityId;
					}
					r.EndEdit();
				}
			}
			else
			{
				GridEXRow[] array = rows;
				foreach (GridEXRow r2 in array)
				{
					r2.BeginEdit();
					if (chkLoadingCity.Checked)
					{
						r2.Cells["LoadingCityId"].Value = selectedCityId;
					}
					if (chkUnLoadingCity.Checked)
					{
						r2.Cells["UnLoadingCityId"].Value = selectedCityId;
					}
					r2.EndEdit();
				}
			}
			grdVoucher.UpdateData();
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
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Expected O, but got Unknown
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Expected O, but got Unknown
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Expected O, but got Unknown
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_0a6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a76: Expected O, but got Unknown
		//IL_0a83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8d: Expected O, but got Unknown
		//IL_43bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_43c5: Expected O, but got Unknown
		//IL_43e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_43f3: Expected O, but got Unknown
		GridEXLayout grdVoucher_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance29 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Service.lgstcm.frmFreightVoucherExportMultiVehicles));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdVoucher = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.chkUnLoadingCity = new System.Windows.Forms.CheckBox();
		this.chkLoadingCity = new System.Windows.Forms.CheckBox();
		this.label49 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.CmbCityName = new UltraCombo();
		this.BtnUpdateCityInGrid = new System.Windows.Forms.Button();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbCustomerLoader = new UltraCombo();
		this.txtGpNoToLoader = new System.Windows.Forms.TextBox();
		this.txtGpNoFromLoader = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtGpToDateLoader = new System.Windows.Forms.DateTimePicker();
		this.txtGpFromDateLoader = new System.Windows.Forms.DateTimePicker();
		this.label12 = new System.Windows.Forms.Label();
		this.BtnShowLoader = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbLogisticPoNo = new UltraCombo();
		this.txtQtyForRate = new System.Windows.Forms.TextBox();
		this.txtFreightRate = new System.Windows.Forms.TextBox();
		this.CmbRateUom = new UltraCombo();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.CmbTransporter = new UltraCombo();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.BtnApplyRate = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
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
		((System.ComponentModel.ISupportInitialize)this.grdVoucher).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerLoader).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLogisticPoNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(1084, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1084, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdVoucher);
		this.tabPage1.Controls.Add(this.panel14);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1076, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdVoucher.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdVoucher).BackColor = System.Drawing.Color.White;
		grdVoucher_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdVoucher.DesignTimeLayout = grdVoucher_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdVoucher).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdVoucher).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdVoucher.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdVoucher).Location = new System.Drawing.Point(3, 192);
		((System.Windows.Forms.Control)(object)this.grdVoucher).Name = "grdVoucher";
		((ScrollableBase)this.grdVoucher).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdVoucher).Size = new System.Drawing.Size(1070, 437);
		((System.Windows.Forms.Control)(object)this.grdVoucher).TabIndex = 1;
		this.grdVoucher.TotalRow = (InheritableBoolean)1;
		this.grdVoucher.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdVoucher).VisualStyle = (VisualStyle)4;
		this.grdVoucher.CellUpdated += new ColumnActionEventHandler(grdVoucher_CellUpdated);
		this.grdVoucher.ColumnButtonClick += new ColumnActionEventHandler(grdVoucher_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdVoucher).KeyDown += new System.Windows.Forms.KeyEventHandler(grdVoucher_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.chkUnLoadingCity);
		this.panel14.Controls.Add(this.chkLoadingCity);
		this.panel14.Controls.Add(this.label49);
		this.panel14.Controls.Add(this.ctrlGrdBar3);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityName);
		this.panel14.Controls.Add(this.BtnUpdateCityInGrid);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 160);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1070, 32);
		this.panel14.TabIndex = 131;
		this.chkUnLoadingCity.AutoSize = true;
		this.chkUnLoadingCity.Checked = true;
		this.chkUnLoadingCity.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkUnLoadingCity.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkUnLoadingCity.ForeColor = System.Drawing.Color.White;
		this.chkUnLoadingCity.Location = new System.Drawing.Point(604, 15);
		this.chkUnLoadingCity.Name = "chkUnLoadingCity";
		this.chkUnLoadingCity.Size = new System.Drawing.Size(129, 17);
		this.chkUnLoadingCity.TabIndex = 201;
		this.chkUnLoadingCity.Text = "Un-Loading City";
		this.chkUnLoadingCity.UseVisualStyleBackColor = true;
		this.chkLoadingCity.AutoSize = true;
		this.chkLoadingCity.Checked = true;
		this.chkLoadingCity.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkLoadingCity.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkLoadingCity.ForeColor = System.Drawing.Color.White;
		this.chkLoadingCity.Location = new System.Drawing.Point(604, 0);
		this.chkLoadingCity.Name = "chkLoadingCity";
		this.chkLoadingCity.Size = new System.Drawing.Size(106, 17);
		this.chkLoadingCity.TabIndex = 201;
		this.chkLoadingCity.Text = "Loading City";
		this.chkLoadingCity.UseVisualStyleBackColor = true;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label49.ForeColor = System.Drawing.Color.White;
		this.label49.Location = new System.Drawing.Point(5, 3);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(338, 26);
		this.label49.TabIndex = 199;
		this.label49.Text = "Click to update the selected City or the same City Selected\r\n in the 1st row when this combo is empty, in all rows of grid";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1024, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdVoucher;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 25);
		this.ctrlGrdBar3.TabIndex = 127;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.CmbCityName.AllowNull = (DefaultableBoolean)1;
		this.CmbCityName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCityName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCityName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Location = new System.Drawing.Point(347, 3);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Name = "CmbCityName";
		((System.Windows.Forms.Control)(object)this.CmbCityName).Size = new System.Drawing.Size(251, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityName).TabIndex = 4;
		this.BtnUpdateCityInGrid.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateCityInGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateCityInGrid.Font = new System.Drawing.Font("Tahoma", 8.5f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateCityInGrid.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateCityInGrid.Location = new System.Drawing.Point(739, 3);
		this.BtnUpdateCityInGrid.Name = "BtnUpdateCityInGrid";
		this.BtnUpdateCityInGrid.Size = new System.Drawing.Size(131, 26);
		this.BtnUpdateCityInGrid.TabIndex = 5;
		this.BtnUpdateCityInGrid.Text = "Update In Detail";
		this.BtnUpdateCityInGrid.UseVisualStyleBackColor = false;
		this.BtnUpdateCityInGrid.Click += new System.EventHandler(BtnUpdateCityInGrid_Click);
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 59);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1070, 101);
		this.panel5.TabIndex = 130;
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerLoader);
		this.groupBox3.Controls.Add(this.txtGpNoToLoader);
		this.groupBox3.Controls.Add(this.txtGpNoFromLoader);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.txtGpToDateLoader);
		this.groupBox3.Controls.Add(this.txtGpFromDateLoader);
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Controls.Add(this.BtnShowLoader);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(662, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(407, 96);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters To Load Data";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(4, 69);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(69, 19);
		this.label8.TabIndex = 198;
		this.label8.Text = "Customer";
		this.CmbCustomerLoader.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerLoader.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerLoader.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerLoader.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerLoader).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerLoader).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerLoader.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerLoader).Location = new System.Drawing.Point(98, 65);
		((System.Windows.Forms.Control)(object)this.CmbCustomerLoader).Name = "CmbCustomerLoader";
		((System.Windows.Forms.Control)(object)this.CmbCustomerLoader).Size = new System.Drawing.Size(251, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerLoader).TabIndex = 4;
		this.txtGpNoToLoader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNoToLoader.Location = new System.Drawing.Point(224, 40);
		this.txtGpNoToLoader.Name = "txtGpNoToLoader";
		this.txtGpNoToLoader.Size = new System.Drawing.Size(125, 23);
		this.txtGpNoToLoader.TabIndex = 3;
		this.txtGpNoFromLoader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNoFromLoader.Location = new System.Drawing.Point(98, 40);
		this.txtGpNoFromLoader.Name = "txtGpNoFromLoader";
		this.txtGpNoFromLoader.Size = new System.Drawing.Size(125, 23);
		this.txtGpNoFromLoader.TabIndex = 2;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(4, 44);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(88, 15);
		this.label10.TabIndex = 196;
		this.label10.Text = "Gp No From,To";
		this.txtGpToDateLoader.CustomFormat = "dd-MMM-yy";
		this.txtGpToDateLoader.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtGpToDateLoader.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGpToDateLoader.Location = new System.Drawing.Point(224, 15);
		this.txtGpToDateLoader.Name = "txtGpToDateLoader";
		this.txtGpToDateLoader.Size = new System.Drawing.Size(125, 22);
		this.txtGpToDateLoader.TabIndex = 1;
		this.txtGpFromDateLoader.CustomFormat = "dd-MMM-yy";
		this.txtGpFromDateLoader.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtGpFromDateLoader.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGpFromDateLoader.Location = new System.Drawing.Point(98, 15);
		this.txtGpFromDateLoader.Name = "txtGpFromDateLoader";
		this.txtGpFromDateLoader.Size = new System.Drawing.Size(125, 22);
		this.txtGpFromDateLoader.TabIndex = 0;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(4, 19);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(97, 15);
		this.label12.TabIndex = 195;
		this.label12.Text = "Gp Date From,To";
		this.BtnShowLoader.BackColor = System.Drawing.Color.Teal;
		this.BtnShowLoader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowLoader.Font = new System.Drawing.Font("Tahoma", 8.5f, System.Drawing.FontStyle.Bold);
		this.BtnShowLoader.ForeColor = System.Drawing.Color.White;
		this.BtnShowLoader.Location = new System.Drawing.Point(350, 65);
		this.BtnShowLoader.Name = "BtnShowLoader";
		this.BtnShowLoader.Size = new System.Drawing.Size(54, 26);
		this.BtnShowLoader.TabIndex = 5;
		this.BtnShowLoader.Text = "Show";
		this.BtnShowLoader.UseVisualStyleBackColor = false;
		this.BtnShowLoader.Click += new System.EventHandler(BtnShowLoader_Click);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo);
		this.groupBox2.Controls.Add(this.txtQtyForRate);
		this.groupBox2.Controls.Add(this.txtFreightRate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransporter);
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.BtnApplyRate);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label47);
		this.groupBox2.Controls.Add(this.label46);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(3, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(659, 96);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Voucher Main Information";
		this.CmbLogisticPoNo.AllowNull = (DefaultableBoolean)1;
		this.CmbLogisticPoNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLogisticPoNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLogisticPoNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLogisticPoNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLogisticPoNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo).Location = new System.Drawing.Point(368, 15);
		((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo).Name = "CmbLogisticPoNo";
		((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbLogisticPoNo).Leave += new System.EventHandler(CmbLogisticPoNo_Leave);
		this.txtQtyForRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyForRate.Enabled = false;
		this.txtQtyForRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyForRate.Location = new System.Drawing.Point(555, 44);
		this.txtQtyForRate.Name = "txtQtyForRate";
		this.txtQtyForRate.Size = new System.Drawing.Size(100, 23);
		this.txtQtyForRate.TabIndex = 7;
		this.txtQtyForRate.Tag = "double";
		this.txtFreightRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightRate.Location = new System.Drawing.Point(368, 44);
		this.txtFreightRate.Name = "txtFreightRate";
		this.txtFreightRate.Size = new System.Drawing.Size(100, 23);
		this.txtFreightRate.TabIndex = 5;
		this.txtFreightRate.Tag = "double";
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(555, 15);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 6;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(81, 17);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(100, 23);
		this.txtdocno.TabIndex = 0;
		this.CmbTransporter.AllowNull = (DefaultableBoolean)1;
		this.CmbTransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Location = new System.Drawing.Point(81, 42);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Name = "CmbTransporter";
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).TabIndex = 2;
		this.CmbTransporter.ValueChanged += new System.EventHandler(CmbTransporter_Leave);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Leave += new System.EventHandler(CmbTransporter_Leave);
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(81, 70);
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(472, 23);
		this.txtRemarksHeader.TabIndex = 3;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(67, 24);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 15);
		this.label53.TabIndex = 509;
		this.label53.Text = "*";
		this.BtnApplyRate.BackColor = System.Drawing.Color.FromArgb(0, 192, 0);
		this.BtnApplyRate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnApplyRate.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.BtnApplyRate.ForeColor = System.Drawing.Color.White;
		this.BtnApplyRate.Location = new System.Drawing.Point(555, 68);
		this.BtnApplyRate.Name = "BtnApplyRate";
		this.BtnApplyRate.Size = new System.Drawing.Size(100, 26);
		this.BtnApplyRate.TabIndex = 5;
		this.BtnApplyRate.Text = "Apply Rate";
		this.BtnApplyRate.UseVisualStyleBackColor = false;
		this.BtnApplyRate.Click += new System.EventHandler(BtnApplyRate_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(67, 48);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(14, 15);
		this.label6.TabIndex = 509;
		this.label6.Text = "*";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(542, 21);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(14, 15);
		this.label47.TabIndex = 509;
		this.label47.Text = "*";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Candara", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.Red;
		this.label46.Location = new System.Drawing.Point(355, 48);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(14, 15);
		this.label46.TabIndex = 509;
		this.label46.Text = "*";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(472, 21);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(59, 15);
		this.label30.TabIndex = 164;
		this.label30.Text = "Rate Uom";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(315, 21);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(43, 15);
		this.label7.TabIndex = 164;
		this.label7.Text = "P.O No";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(2, 48);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 164;
		this.label2.Text = "Transporter";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(182, 17);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(130, 22);
		this.txtdocdate.TabIndex = 1;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(472, 48);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(72, 15);
		this.label27.TabIndex = 131;
		this.label27.Text = "Qty For Rate";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(315, 40);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(44, 30);
		this.label24.TabIndex = 131;
		this.label24.Text = "Freight\r\nRate";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(2, 74);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 131;
		this.label1.Text = "Remarks";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(2, 13);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(73, 30);
		this.label37.TabIndex = 0;
		this.label37.Text = "Voucher No \r\n&& Date";
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
		this.panel3.Size = new System.Drawing.Size(1070, 56);
		this.panel3.TabIndex = 0;
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(838, 32);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(138, 20);
		this.ChkPreview.TabIndex = 12;
		this.ChkPreview.Text = "Preview 1305_01";
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
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnAttachment, this.btnPrint, this.btnPrintVoucher, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1070, 27);
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
		this.btnPrint.Size = new System.Drawing.Size(124, 24);
		this.btnPrint.Text = "1305_01-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnPrintVoucher.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintVoucher.Name = "btnPrintVoucher";
		this.btnPrintVoucher.Size = new System.Drawing.Size(84, 24);
		this.btnPrintVoucher.Text = "Voucher";
		this.btnPrintVoucher.Click += new System.EventHandler(btnPrintVoucher_Click);
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
		this.chkVoucher.Location = new System.Drawing.Point(979, 32);
		this.chkVoucher.Name = "chkVoucher";
		this.chkVoucher.Size = new System.Drawing.Size(80, 20);
		this.chkVoucher.TabIndex = 12;
		this.chkVoucher.Text = "Voucher";
		this.chkVoucher.UseVisualStyleBackColor = true;
		this.CmbChargesToDrAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbChargesToDrAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChargesToDrAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChargesToDrAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChargesToDrAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChargesToDrAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChargesToDrAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChargesToDrAccount).Location = new System.Drawing.Point(407, 29);
		((System.Windows.Forms.Control)(object)this.CmbChargesToDrAccount).Name = "CmbChargesToDrAccount";
		((System.Windows.Forms.Control)(object)this.CmbChargesToDrAccount).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbChargesToDrAccount).TabIndex = 0;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.White;
		this.label38.Location = new System.Drawing.Point(239, 34);
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
		this.tabPage3.Size = new System.Drawing.Size(1076, 632);
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
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1076, 274);
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
		this.panel9.Size = new System.Drawing.Size(1076, 24);
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
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1076, 193);
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
		this.panel15.Size = new System.Drawing.Size(1076, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1029, 0);
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
		this.panel13.Size = new System.Drawing.Size(1076, 66);
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
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
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
		this.panel38.Size = new System.Drawing.Size(1076, 52);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1076, 27);
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
		base.ClientSize = new System.Drawing.Size(1084, 661);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmFreightVoucherExportMultiVehicles";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmFreightVoucherExportMultiVehicles_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdVoucher).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).EndInit();
		this.panel5.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerLoader).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLogisticPoNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).EndInit();
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

}
