using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Main;
using Architecture.Model.pcc;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.pcc;

public class frmGdnWages : Form
{
	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private Attachment AT = new Attachment();

	private UserAccount UserAccount = new UserAccount();

	private Dictionary<(int itemId, int contractorId), DataTable> WagesRateCatchDuringLoop = new Dictionary<(int, int), DataTable>();

	private DataTable dtGdnDetail = new DataTable();

	private DataTable dtWagesDetail = new DataTable();

	private DataTable dtExtraWagesDetail = new DataTable();

	private DataTable dthistoryGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtExtraWagesCombo = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtItemForWages = new DataTable();

	private DataTable dtVarientForWages = new DataTable();

	private DataTable dtContr = new DataTable();

	private int DocumentTypeId = 1869;

	private int RecId;

	private int RefDocumentTypeId;

	private bool Approved;

	private bool FormInitialized;

	private string ScreenName = "frmGdnWages";

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool HasMultiCurrencyFeature;

	private bool SubsidiaryAccountAllownOnVouchers;

	private int DefaultDaysToLessFromHistoryFromDate;

	private int BaseCurrencyId;

	private double BaseCurrencyRate;

	private List<ContractorWagesBillDetail> lstRemoveDetailRecord = new List<ContractorWagesBillDetail>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnAttachment;

	private TabPage tabPage2;

	internal GridEX grdGdn;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private ToolStripButton btnSlip;

	internal UltraCombo CmbCustomerName;

	internal UltraCombo comproject;

	internal UltraCombo combranches;

	private Label label13;

	private Label label18;

	private Panel panel9;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel10;

	internal GridEX grdGdnDetailHistory;

	private CheckBox ChkPrintslip;

	private Panel panel11;

	private Label label17;

	private Panel panel16;

	private Label label23;

	private Label label54;

	private Panel panel18;

	private Label label55;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton btnshortcutkeys;

	private ToolStripSeparator toolStripSeparator1;

	private TabControl tabControl2;

	private TabPage tabGdnDetail;

	internal GridEX grdContractorWages;

	private Panel panel6;

	private Label label58;

	private Panel panel7;

	private Label label78;

	private GroupBox groupBox4;

	private Button btnshow;

	private Label label73;

	internal UltraCombo CmbCustomerHistory;

	private Label label75;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private Label label76;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private TextBox txtFromNoHistory;

	private Label label3;

	private ToolStripButton BtnRefreshHistory;

	internal GridEX grdPendingGdns;

	private TabPage tabWages;

	private Panel panel4;

	private Label label16;

	private Button BtnGenerateWagesRows;

	private Button BtnUpdateConctratorInWages;

	internal UltraCombo CmbActivityforContractorUpdateInWagesGrid;

	internal UltraCombo CmbContractorForUpdateInWagesGrid;

	private Label label66;

	private Label label65;

	private Panel PanelMessage;

	private ToolStrip ToolStripMessage;

	private ToolStripButton BtnMessage;

	private TabControl tabControl3;

	private TabPage tabWagesDetailH;

	internal GridEX grdWagesDetailHistory;

	private Panel panel12;

	private Label label1;

	private TabPage tabGdnDetailH;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private TabPage tabExtraWages;

	internal GridEX grdExtraWages;

	private Panel panel14;

	internal UltraCombo CmbContractorExtraWages;

	private Label label2;

	private Label label15;

	private Label label31;

	internal UltraCombo CmbActivtyExtraWages;

	private Button BtnUpdateExtraWages;

	private Label label32;

	private GroupBox groupBox1;

	private TextBox txtTotalAmount;

	private RadioButton RdPartyByCode;

	private RadioButton RdPartyByName;

	private Label label8;

	private Label labelFcyAmt;

	private TextBox txtTotalQty;

	private TextBox txtExchangeRate;

	private Label labelRemarksH;

	private TextBox txtFcyAmount;

	private Label labelExRate;

	private DateTimePicker DocDate;

	internal UltraCombo cmbCurrency;

	private TextBox txtremarks;

	private Label label43;

	private Label labelFcyCode;

	private TextBox txtdocno;

	private Label label33;

	private Label label35;

	private TextBox txtCustomerCellNo;

	private TextBox txtCustomerCity;

	private Label label4;

	private Label label62;

	private ToolStripButton btnsave;

	private ToolStripButton btnDelete;

	private TabPage tabExtraWagesH;

	internal GridEX grdExtraWagesDetailHistory;

	private Panel panel5;

	private Label label5;

	private CtrlGrdBar ctrlGrdBar4;

	private Panel panel8;

	private GroupBox groupBox2;

	internal UltraCombo CmbDateTypePending;

	private Button BtnShowPending;

	private Label label6;

	private Label label7;

	private DateTimePicker txtToDatePending;

	private Label label9;

	private TextBox txtToDocPending;

	private DateTimePicker txtFromDatePending;

	private Label label11;

	private TextBox txtFromDocPending;

	private ToolStrip toolStrip3;

	private ToolStripButton BtnNewPending;

	internal UltraCombo CmbCustomerPending;

	private ToolStripButton BtnRefreshPending;

	private Button btnRecordsUpdate;

	public frmGdnWages(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(1200, 800);
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtContr = dtSupplier.Clone();
			dtItemForWages.Columns.Add("Id", typeof(int));
			dtItemForWages.Columns.Add("ItemName", typeof(string));
			dtItemForWages.Columns.Add("ItemCode", typeof(string));
			dtItemForWages.Columns.Add("InvGdnId", typeof(int));
			dtItemForWages.Columns.Add("GdnNo", typeof(int));
			dtVarientForWages.Columns.Add("ItemAttributeId", typeof(int));
			dtVarientForWages.Columns.Add("ItemAttribute", typeof(string));
			dtVarientForWages.Columns.Add("ItemId", typeof(int));
			dtVarientForWages.Columns.Add("ItemName", typeof(string));
			dtVarientForWages.Columns.Add("VarientEquivalent", typeof(decimal));
			dtVarientForWages.Columns.Add("Qty", typeof(decimal));
			dtVarientForWages.Columns.Add("NetWeight", typeof(decimal));
			dtVarientForWages.Columns.Add("InvGdnId", typeof(int));
			dtVarientForWages.Columns.Add("GdnNo", typeof(int));
			dtGdnDetail.Columns.Add("Id", typeof(int));
			dtGdnDetail.Columns.Add("InvGdnId", typeof(int));
			dtGdnDetail.Columns.Add("GdnNo", typeof(int));
			dtGdnDetail.Columns.Add("OrderId", typeof(int));
			dtGdnDetail.Columns.Add("OrderDetailId", typeof(int));
			dtGdnDetail.Columns.Add("OrderNo", typeof(int));
			dtGdnDetail.Columns.Add("DoOrderDetailId", typeof(int));
			dtGdnDetail.Columns.Add("WarehouseId", typeof(int));
			dtGdnDetail.Columns.Add("Warehouse", typeof(string));
			dtGdnDetail.Columns.Add("ItemId", typeof(int));
			dtGdnDetail.Columns.Add("ItemName", typeof(string));
			dtGdnDetail.Columns.Add("ItemCode", typeof(string));
			dtGdnDetail.Columns.Add("AttributeVarientId", typeof(int));
			dtGdnDetail.Columns.Add("AttributeVarient", typeof(string));
			dtGdnDetail.Columns.Add("VarientUnit", typeof(decimal));
			dtGdnDetail.Columns.Add("JobLotId", typeof(int));
			dtGdnDetail.Columns.Add("JobLot", typeof(string));
			dtGdnDetail.Columns.Add("ItemQty", typeof(decimal));
			dtGdnDetail.Columns.Add("ItemWeight", typeof(decimal));
			dtGdnDetail.Columns.Add("NetWeight", typeof(decimal));
			dtGdnDetail.Columns.Add("CityId", typeof(int));
			dtGdnDetail.Columns.Add("CityName", typeof(string));
			dtGdnDetail.Columns.Add("IsFOC", typeof(bool));
			dtGdnDetail.Columns.Add("RemarksDetail", typeof(string));
			dtWagesDetail.Columns.Add("Id", typeof(int));
			dtWagesDetail.Columns.Add("InvGdnId", typeof(int));
			dtWagesDetail.Columns.Add("GdnNo", typeof(int));
			dtWagesDetail.Columns.Add("ItemId", typeof(int));
			dtWagesDetail.Columns.Add("ItemName");
			dtWagesDetail.Columns.Add("AttributeVarientId", typeof(int));
			dtWagesDetail.Columns.Add("AttributeVarient");
			dtWagesDetail.Columns.Add("VarientEquivalent", typeof(decimal));
			dtWagesDetail.Columns.Add("ContractorId", typeof(int));
			dtWagesDetail.Columns.Add("ContractorName");
			dtWagesDetail.Columns.Add("ContractorWagesRateScheduleId", typeof(int));
			dtWagesDetail.Columns.Add("ServiceActivity");
			dtWagesDetail.Columns.Add("ParentUomId", typeof(int));
			dtWagesDetail.Columns.Add("Qty", typeof(decimal));
			dtWagesDetail.Columns.Add("ItemNetWeight", typeof(decimal));
			dtWagesDetail.Columns.Add("Rate", typeof(decimal));
			dtWagesDetail.Columns.Add("Amount", typeof(decimal));
			dtWagesDetail.Columns.Add("AddLess", typeof(decimal));
			dtWagesDetail.Columns.Add("NetAmount", typeof(decimal));
			dtWagesDetail.Columns.Add("FcyAmount", typeof(decimal));
			dtWagesDetail.Columns.Add("Remarks");
			dtWagesDetail.Columns.Add("ContractorWagesActivityId", typeof(int));
			dtExtraWagesDetail.Columns.Add("Id", typeof(int));
			dtExtraWagesDetail.Columns.Add("InvGdnId", typeof(int));
			dtExtraWagesDetail.Columns.Add("GdnNo", typeof(int));
			dtExtraWagesDetail.Columns.Add("ContractorId", typeof(int));
			dtExtraWagesDetail.Columns.Add("ContractorName");
			dtExtraWagesDetail.Columns.Add("ServiceActivityId", typeof(int));
			dtExtraWagesDetail.Columns.Add("ServiceActivity");
			dtExtraWagesDetail.Columns.Add("Qty", typeof(decimal));
			dtExtraWagesDetail.Columns.Add("Rate", typeof(decimal));
			dtExtraWagesDetail.Columns.Add("Amount", typeof(decimal));
			dtExtraWagesDetail.Columns.Add("FcyAmount", typeof(decimal));
			dtExtraWagesDetail.Columns.Add("Remarks");
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
			ScreenName = base.Name;
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			DataTable historyComboData = HistoryComboDbCAll();
			DataTable pendingComboData = PendingComboDbCall();
			string documentNoData = DocumentNoDbCall();
			DataTable refDocumentTypeData = CommonServices.StaticColumnsService("RefDocumentTypeForPackingMaterial");
			DataTable pendingGdnData = PendingGdnDBCall(AllFilters: false);
			dtForGridComboFill();
			return new
			{
				HistoryComboData = historyComboData,
				documentNoData = documentNoData,
				RefDocumentTypeData = refDocumentTypeData,
				PendingGdnData = pendingGdnData,
				PendingComboData = pendingComboData
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
				btnSlip.Enabled = formright.DoHavePrintRights;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				ChkPrintslip.Checked = formright.DoHavePrintRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar4, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formright);
				MultiCurrencyFeature();
				GetConfigurationsFromGlobal();
				if (RecId == 0)
				{
					UpdateDocNoUI(result.documentNoData);
				}
				SupplierDtFillFromGlobal();
				BindSupplierName();
				CurrencyBindFromGlobal();
				ImplementConfiguration();
				BindGrids();
				InfragisticsHelper.BindComboDateType(CmbDateTypePending);
				PendingComboBind(result.PendingComboData);
				PendingGdnGridFill(result.PendingGdnData);
				txtFromDatePending.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-7.0));
				txtToDatePending.Value = DateTime.Now;
				InfragisticsHelper.BindComboDateType(cmbDateTypeHistory);
				HistoryComboBind(result.HistoryComboData);
				txtFromdateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-7.0));
				txtToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				DocDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			BaseCurrencyId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Base Currency"));
			BaseCurrencyRate = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BaseCurrencyRate"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImplementConfiguration()
	{
		try
		{
			cmbCurrency.Value = BaseCurrencyId;
			txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private string DocumentNoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			return Conversion.ToString((object)ContractorWagesBill.GenerateCode(new ContractorWagesBill
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

	private void UpdateDocNoUI(string documentData)
	{
		txtdocno.Text = documentData;
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			IEnumerable<getGlobalSupplierCustomer> enumerable;
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				IEnumerable<getGlobalSupplierCustomer> globalSupplierCustomer = clsGlobalVariables.globalSupplierCustomer;
				enumerable = globalSupplierCustomer;
			}
			else
			{
				enumerable = clsGlobalVariables.globalSupplierCustomer.Where((getGlobalSupplierCustomer r) => r.PartyTypeId == 2);
			}
			foreach (getGlobalSupplierCustomer supplier2 in enumerable)
			{
				dtSupplier.Rows.Add(Conversion.ToInt((object)supplier2.Id), Conversion.ToString((object)supplier2.CompanyName), Conversion.ToString((object)supplier2.PartyCode), Conversion.ToInt((object)supplier2.GlAccountId), Conversion.ToInt((object)supplier2.CityId), Conversion.ToString((object)supplier2.CityName), Conversion.ToString((object)supplier2.MobilePersonal));
			}
			dtContr.Clear();
			foreach (getGlobalSupplierCustomer supplier in clsGlobalVariables.globalSupplierCustomer.Where((getGlobalSupplierCustomer r) => r.CustomerGroupId == 8))
			{
				dtContr.Rows.Add(Conversion.ToInt((object)supplier.Id), Conversion.ToString((object)supplier.CompanyName), Conversion.ToString((object)supplier.PartyCode), Conversion.ToInt((object)supplier.GlAccountId), Conversion.ToInt((object)supplier.CityId), Conversion.ToString((object)supplier.CityName), Conversion.ToString((object)supplier.MobilePersonal));
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindSupplierName()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerName, dtSupplier, "Id", "CompanyName", "Customer Name", AllColumns: true, Conversion.ToInt(CmbCustomerName.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			DataTable dataTable = dtSupplier;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				CmbCustomerName_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WagesContractorFilterBind()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbContractorForUpdateInWagesGrid, dtContr, "Id", "CompanyName", "Contractor Name", AllColumns: true, Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			DataTable dataTable = dtContr;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				((UltraGridBase)CmbContractorForUpdateInWagesGrid).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbContractorForUpdateInWagesGrid).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ExtraWagesContractorFilterBind()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbContractorExtraWages, dtContr, "Id", "CompanyName", "Contractor Name", AllColumns: true, Conversion.ToInt(CmbContractorExtraWages.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			DataTable dataTable = dtContr;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				((UltraGridBase)CmbContractorExtraWages).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbContractorExtraWages).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CurrencyBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalMultiCurrency, (MultiCurrency x) => x.Id, (MultiCurrency x) => x.CurrencyName);
			InfragisticsHelper.BindAndRetainSelection(cmbCurrency, dt, "Id", "Description", "Currency", AllColumns: true, Conversion.ToInt(cmbCurrency.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbCustomerName_Leave(object sender, EventArgs e)
	{
		try
		{
			BindCustomerCity();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCustomerCity()
	{
		try
		{
			txtCustomerCity.Text = string.Empty;
			txtCustomerCellNo.Text = string.Empty;
			int Id = Conversion.ToInt(CmbCustomerName.Value);
			if (((UltraGridBase)CmbCustomerName).ActiveRow != null && Id > 0)
			{
				string CustomerCellNo = Conversion.ToString(((UltraDropDownBase)CmbCustomerName).SelectedRow.Cells["MobileNo"].Value);
				txtCustomerCellNo.Text = CustomerCellNo;
				string CityName = Conversion.ToString(((UltraDropDownBase)CmbCustomerName).SelectedRow.Cells["CityName"].Value);
				txtCustomerCity.Text = CityName;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0 || Conversion.ToDouble((object)txtExchangeRate.Text) != 0.0)
			{
				return;
			}
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrencyId)
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
				txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dtForGridComboFill()
	{
		try
		{
			dtExtraWagesCombo.Clear();
			dtExtraWagesCombo = ContractorWagesActivity.ExtraWagesActivitiesCombo(UserAccount.OrganizationId, UserAccount.CompanyId);
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
			if (dtGdnDetail != null && dtGdnDetail.Columns.Count > 0)
			{
				grdGdn.DataSource = dtGdnDetail;
				grdGdn.RetrieveStructure();
				grdGdnSettings(grdGdn);
			}
			if (dtWagesDetail != null && dtWagesDetail.Columns.Count > 0)
			{
				if (dtWagesDetail.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
				grdContractorWages.DataSource = dtWagesDetail;
				grdContractorWages.RetrieveStructure();
				grdContractorWagesGridSetting(grdContractorWages);
			}
			if (dtExtraWagesDetail != null && dtExtraWagesDetail.Columns.Count > 0)
			{
				if (dtExtraWagesDetail.Rows.Count == 0)
				{
					AddRowInExtraWagesGrid();
				}
				grdExtraWages.DataSource = dtExtraWagesDetail;
				grdExtraWages.RetrieveStructure();
				grdExtraWagesGridSetting(grdExtraWages);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindComboGrids()
	{
		try
		{
			FillWagesGridFilterCombo();
			FillExtraWagesGridFilterCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGdnSettings(GridEX gridEX)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		try
		{
			dtItemAndVariantForWagesItemFillFromDetail();
			List<string> hiddenColumns = new List<string> { "Id", "InvGdnId", "OrderId", "OrderDetailId", "DoOrderDetailId", "WarehouseId", "ItemId", "AttributeVarientId", "JobLotId", "CityId" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 2, hiddenColumns);
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(gridEX.RootTable.Columns["IsFOC"], (ConditionOperator)0, (object)true);
			fc.FormatStyle.ForeColor = Color.Red;
			gridEX.RootTable.FormatConditions.Add(fc);
			if (((Control)(object)gridEX).Name == "grdGdn")
			{
				ctrlGrdBar1_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdGdn.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && grdGdn.CurrentColumn != null && grdGdn.CurrentColumn.Key == "IsFOC")
			{
				grdGdn.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dtItemAndVariantForWagesItemFillFromDetail()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow currentRow = grdGdn.CurrentRow;
			if (currentRow == null || (int)currentRow.RowType != 0 || currentRow.Cells.Count == 0)
			{
				return;
			}
			dtItemForWages.Clear();
			dtVarientForWages.Clear();
			var rows = (from r in grdGdn.GetDataRows()
				where (int)r.RowType == 0
				select new
				{
					GdnId = Conversion.ToInt(r.Cells["InvGdnId"].Value),
					GdnNo = Conversion.ToString(r.Cells["GdnNo"].Value),
					ItemId = Conversion.ToInt(r.Cells["ItemId"].Value),
					ItemName = Conversion.ToString(r.Cells["ItemName"].Value),
					ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value),
					AttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value),
					AttributeVarient = Conversion.ToString(r.Cells["AttributeVarient"].Value),
					VarientUnit = Conversion.ToString(r.Cells["VarientUnit"].Value),
					ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value),
					NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value)
				}).ToList();
			foreach (var item in from x in rows
				group x by new { x.ItemId, x.GdnId } into g
				select g.First())
			{
				dtItemForWages.Rows.Add(item.ItemId, item.ItemName, item.ItemCode, item.GdnId, item.GdnNo);
			}
			foreach (var v in from x in rows
				group x by new { x.ItemId, x.AttributeVarientId, x.GdnId } into g
				select new
				{
					ItemId = g.Key.ItemId,
					AttributeVarientId = g.Key.AttributeVarientId,
					GdnId = g.Key.GdnId,
					GdnNo = g.First().GdnNo,
					ItemName = g.First().ItemName,
					AttributeVarient = g.First().AttributeVarient,
					VarientUnit = g.First().VarientUnit,
					ItemQty = g.Sum(x => x.ItemQty),
					NetWeight = g.Sum(x => x.NetWeight)
				})
			{
				dtVarientForWages.Rows.Add(v.AttributeVarientId, v.AttributeVarient, v.ItemId, v.ItemName, v.VarientUnit, v.ItemQty, v.NetWeight, v.GdnId, v.GdnNo);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillWagesGridFilterCombo()
	{
		try
		{
			WagesContractorFilterBind();
			DataTable dtFiltered = new DataTable();
			if (grdContractorWages.GetDataRows().Length != 0 && dtWagesDetail.Rows.Count > 0)
			{
				dtFiltered = (from row in dtWagesDetail.AsEnumerable()
					group row by row.Field<int>("ContractorWagesRateScheduleId") into g
					select g.First()).CopyToDataTable();
			}
			InfragisticsHelper.BindAndRetainSelection(CmbActivityforContractorUpdateInWagesGrid, dtFiltered, "ContractorWagesRateScheduleId", "ServiceActivity", "Service Activity", AllColumns: false, Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInContractorWagesGrid()
	{
		dtWagesDetail.Rows.Add(0, 0, 0, 0, "", 0, "", 0, 0, "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, "", 0);
		grdContractorWages.DataSource = dtWagesDetail;
	}

	private void grdContractorWagesGridSetting(GridEX gridEX)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "InvGdnWagesId", "InvGdnId", "ItemId", "AttributeVarientId", "ContractorId", "ContractorWagesRateScheduleId", "ParentUomId", "ContractorWagesActivityId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Qty",
					(EditType)1
				},
				{
					"AddLess",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 2, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["AttributeVarient"].Caption = "Variant";
			gridEX.RootTable.Columns["VarientEquivalent"].Caption = "Unit";
			gridEX.RootTable.Columns["ItemNetWeight"].Caption = "Weight";
			gridEX.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			gridEX.RootTable.Columns["ItemName"].Width = 140;
			gridEX.RootTable.Columns["AttributeVarient"].Width = 70;
			gridEX.RootTable.Columns["VarientEquivalent"].Width = 70;
			gridEX.RootTable.Columns["ContractorName"].Width = 140;
			gridEX.RootTable.Columns["ServiceActivity"].Width = 130;
			gridEX.RootTable.Columns["Qty"].Width = 70;
			gridEX.RootTable.Columns["ItemNetWeight"].Width = 80;
			gridEX.RootTable.Columns["Rate"].Width = 75;
			gridEX.RootTable.Columns["Amount"].Width = 100;
			gridEX.RootTable.Columns["AddLess"].Width = 80;
			gridEX.RootTable.Columns["NetAmount"].Width = 105;
			gridEX.RootTable.Columns["Remarks"].Width = 140;
			gridEX.RootTable.Groups.Add("ServiceActivity");
			gridEX.RootTable.GroupTotals = (GroupTotals)2;
			if (((Control)(object)gridEX).Name == "grdContractorWages")
			{
				GridEX_Helper.AddButton(gridEX, "DeleteAll", "DeleteAll", 80, 0);
				GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 1);
				GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 2);
				gridEX.FrozenColumns = 3;
				ctrlGrdBar4_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdContractorWages.UpdateData();
			GridEXRow item = grdContractorWages.CurrentRow;
			if (e.Column.Key == "Qty" && item != null && (int)item.RowType == 0)
			{
				decimal Amount = default(decimal);
				decimal NetAmount2 = default(decimal);
				decimal Qty = default(decimal);
				decimal Weight = default(decimal);
				decimal Rate = default(decimal);
				decimal AddLess2 = default(decimal);
				int ParentUomId = 0;
				if (!(item.Cells["Qty"].Value.ToString() != string.Empty) || !(item.Cells["Rate"].Value.ToString() != string.Empty))
				{
					throw new Exception("Qty And Rate Should be greater than zero...");
				}
				Qty = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
				DataRow foundRow = dtGdnDetail.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ItemId") == Conversion.ToInt(item.Cells["ItemId"].Value));
				decimal obj = ((foundRow != null) ? Conversion.ToDecimal(foundRow["ItemWeight"]) : 0m);
				decimal VarientEquivalent = Conversion.ToDecimal((object)item.Cells["VarientEquivalent"].Text.ToString());
				Weight = obj * Qty * VarientEquivalent;
				item.Cells["ItemNetWeight"].Value = Weight;
				Rate = Conversion.ToDecimal((object)item.Cells["Rate"].Text.ToString());
				AddLess2 = Conversion.ToDecimal((object)item.Cells["AddLess"].Text.ToString());
				switch (Conversion.ToInt((object)item.Cells["ParentUomId"].Text.ToString()))
				{
				case 2:
					Amount = Rate * Qty;
					item.Cells["Amount"].Value = Amount;
					NetAmount2 = Amount + AddLess2;
					item.Cells["NetAmount"].Value = NetAmount2;
					break;
				case 3:
					Amount = Weight / 1000m * Rate;
					item.Cells["Amount"].Value = Amount;
					NetAmount2 = Amount + AddLess2;
					item.Cells["NetAmount"].Value = NetAmount2;
					break;
				default:
					item.Cells["Amount"].Value = 0;
					NetAmount2 = Amount + AddLess2;
					item.Cells["NetAmount"].Value = NetAmount2;
					MessageBox.Show("ParentUom of RangeUom of Selected Wages Activity is not Defined...Please Check");
					return;
				}
			}
			if (e.Column.Key == "AddLess" && item != null && (int)item.RowType == 0)
			{
				decimal AddLess = default(decimal);
				decimal NetAmount = default(decimal);
				decimal num = Conversion.ToDecimal((object)item.Cells["Amount"].Text.ToString());
				AddLess = Conversion.ToDecimal((object)item.Cells["AddLess"].Text.ToString());
				NetAmount = num + AddLess;
				item.Cells["NetAmount"].Value = NetAmount;
			}
			grdContractorWages.UpdateData();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractorWages.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					WagesDeleteRow(r);
				}
				if (e.Column.Key == "DeleteAll")
				{
					WagesDeleteAllButtonClick(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInContractorWagesGrid();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesDeleteRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ContractorWagesBillDetail vd = new ContractorWagesBillDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				FillWagesDetailListCommonForInsertAndDelete(vd, r);
				vd.ActionTypeId = 3;
				lstRemoveDetailRecord.Add(vd);
				dtWagesDetail.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			else
			{
				r.Delete();
			}
			if (dtWagesDetail.Rows.Count == 0)
			{
				AddRowInContractorWagesGrid();
			}
			grdContractorWages.UpdateData();
			txtExchangeRate_TextChanged(null, null);
			FillWagesGridFilterCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesDeleteAllButtonClick(GridEXRow clickedRow)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		try
		{
			GridEXRow[] allRows = grdContractorWages.GetDataRows();
			if (allRows.Length == 0)
			{
				return;
			}
			string targetActivity = Conversion.ToString(clickedRow.Cells["ServiceActivity"].Value);
			if (MessageBox.Show("Are you sure to Delete All Rows in Wages Grid Of Activity " + targetActivity + "?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
			{
				return;
			}
			GridEXRow[] array = allRows;
			foreach (GridEXRow row in array)
			{
				if (!(Conversion.ToString(row.Cells["ServiceActivity"].Value) != targetActivity))
				{
					int rowId = Conversion.ToInt(row.Cells["Id"].Value);
					if (rowId > 0)
					{
						ContractorWagesBillDetail wageDetail = new ContractorWagesBillDetail
						{
							Id = rowId,
							ActionTypeId = 3
						};
						FillWagesDetailListCommonForInsertAndDelete(wageDetail, row);
						lstRemoveDetailRecord.Add(wageDetail);
						dtWagesDetail.Rows[row.RowIndex].Delete();
					}
					row.Delete();
				}
			}
			if (dtWagesDetail.Rows.Count == 0)
			{
				AddRowInContractorWagesGrid();
			}
			grdContractorWages.UpdateData();
			txtExchangeRate_TextChanged(null, null);
			FillWagesGridFilterCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnGenerateWagesRows_Click(object sender, EventArgs e)
	{
		try
		{
			dtItemAndVariantForWagesItemFillFromDetail();
			GenerateRowsInWagesGrid();
			FillWagesGridFilterCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateRowsInWagesGrid()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGdnDetail.Rows.Count == 0 || dtVarientForWages.Rows.Count == 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdContractorWages.GetDataRows();
			if (dataRows.Length == 1)
			{
				GridEXRow firstRow = dataRows[0];
				if (firstRow != null && (int)firstRow.RowType == 0)
				{
					GridEXCell obj = firstRow.Cells["ItemId"];
					if (Conversion.ToInt((obj != null) ? obj.Value : null) == 0)
					{
						GridEXCell obj2 = firstRow.Cells["Amount"];
						if (Conversion.ToDecimal((obj2 != null) ? obj2.Value : null) == 0m)
						{
							dtWagesDetail.Clear();
						}
					}
				}
			}
			foreach (DataRow variantRow in dtVarientForWages.Rows)
			{
				int itemId = Conversion.ToInt(variantRow["ItemId"]);
				string itemName = Conversion.ToString(variantRow["ItemName"]);
				int invGdnId = Conversion.ToInt(variantRow["InvGdnId"]);
				string gdnNo = Conversion.ToString(variantRow["GdnNo"]);
				int attrId = Conversion.ToInt(variantRow["ItemAttributeId"]);
				string attrName = Conversion.ToString(variantRow["ItemAttribute"]);
				decimal varientEquivalent = Conversion.ToDecimal(variantRow["VarientEquivalent"]);
				decimal qty = Conversion.ToDecimal(variantRow["Qty"]);
				decimal netWeight = Conversion.ToDecimal(variantRow["NetWeight"]);
				foreach (DataRow activityRow in ContractorWagesRateSchedule.GetServiceActivityByItemId(2, itemId, Conversion.ToDateTime((object)DocDate.Value), 0, 0).Rows)
				{
					int scheduleId = Conversion.ToInt(activityRow["ContractorWagesRateScheduleId"]);
					int ContractorWagesActivityId = Conversion.ToInt(activityRow["ContractorWagesActivityId"]);
					int parentUomId = Conversion.ToInt(activityRow["ParentUomId"]);
					decimal rate = Conversion.ToDecimal(activityRow["Rate"]);
					string activity = Conversion.ToString(activityRow["WagesActivity"]);
					decimal amount = default(decimal);
					amount = parentUomId switch
					{
						2 => rate * qty, 
						3 => netWeight / 1000m * rate, 
						_ => default(decimal), 
					};
					GridEXRow existingRow = grdContractorWages.GetDataRows().FirstOrDefault((GridEXRow r) => Conversion.ToInt(r.Cells["InvGdnId"].Value) == invGdnId && Conversion.ToInt(r.Cells["ItemId"].Value) == itemId && Conversion.ToInt(r.Cells["AttributeVarientId"].Value) == attrId && Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) == scheduleId);
					if (existingRow != null)
					{
						existingRow.BeginEdit();
						existingRow.Cells["ContractorWagesActivityId"].Value = ContractorWagesActivityId;
						existingRow.Cells["Qty"].Value = qty;
						existingRow.Cells["ItemNetWeight"].Value = netWeight;
						existingRow.Cells["Amount"].Value = amount;
						decimal addLess = Conversion.ToDecimal(existingRow.Cells["AddLess"].Value);
						existingRow.Cells["NetAmount"].Value = amount + addLess;
						existingRow.EndEdit();
						continue;
					}
					dtWagesDetail.Rows.Add(0, invGdnId, gdnNo, itemId, itemName, attrId, attrName, varientEquivalent, 0, "", scheduleId, activity, parentUomId, qty, netWeight, rate, amount, 0, amount, 0, "", ContractorWagesActivityId);
				}
			}
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateConctratorInWages_Click(object sender, EventArgs e)
	{
		try
		{
			GridEXRow[] rows = grdContractorWages.GetDataRows();
			if (rows.Length == 0)
			{
				throw new Exception("Wages Grid Record not Found");
			}
			bool hasContractor = ((UltraGridBase)CmbContractorForUpdateInWagesGrid).ActiveRow != null && Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value) != 0;
			bool hasActivity = ((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).ActiveRow != null && Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value) != 0;
			if (!hasContractor && !hasActivity)
			{
				GridEXRow obj = rows[0];
				int contrId = Conversion.ToInt(obj.Cells["ContractorId"].Value);
				string contrName = Conversion.ToString(obj.Cells["ContractorName"].Value);
				if (contrId == 0)
				{
					throw new Exception("ContractorName Field is Empty in Wages Grid Row No: 1");
				}
				WagesUpdateContractorRows(rows, contrId, contrName);
				return;
			}
			if (hasContractor && !hasActivity)
			{
				int contrId2 = Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value);
				string contrName2 = Conversion.ToString((object)((Control)(object)CmbContractorForUpdateInWagesGrid).Text);
				WagesUpdateContractorRows(rows, contrId2, contrName2);
				return;
			}
			if (!hasContractor && hasActivity)
			{
				CmbContractorForUpdateInWagesGrid.Focus();
				throw new Exception("ContractorName Field is Empty");
			}
			int targetContrId = Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value);
			string targetContrName = Conversion.ToString((object)((Control)(object)CmbContractorForUpdateInWagesGrid).Text);
			string targetActivity = Conversion.ToString((object)((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text);
			IEnumerable<GridEXRow> filteredRows = rows.Where((GridEXRow r) => Conversion.ToString(r.Cells["ServiceActivity"].Value) == targetActivity);
			WagesUpdateContractorRows(filteredRows, targetContrId, targetContrName);
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesUpdateContractorRows(IEnumerable<GridEXRow> rows, int contractorId, string contractorName)
	{
		foreach (GridEXRow row in rows)
		{
			row.BeginEdit();
			row.Cells["ContractorId"].Value = contractorId;
			row.Cells["ContractorName"].Value = contractorName;
			WagesApplyRateAndAmounts(row, contractorId);
			row.EndEdit();
		}
	}

	private void WagesApplyRateAndAmounts(GridEXRow row, int contractorId)
	{
		if (contractorId == 0)
		{
			contractorId = Conversion.ToInt(row.Cells["ContractorId"].Value);
		}
		decimal addLess = Conversion.ToDecimal(row.Cells["AddLess"].Value);
		string serviceActivity = Conversion.ToString(row.Cells["ServiceActivity"].Value);
		int itemId = Conversion.ToInt(row.Cells["ItemId"].Value);
		DataRow foundRow = GetCachedServiceActivity(itemId, contractorId)?.AsEnumerable().FirstOrDefault((DataRow r) => r.Field<string>("WagesActivity") == serviceActivity);
		decimal rate = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m);
		int parentUomId = ((foundRow != null) ? Conversion.ToInt(foundRow["ParentUOMId"]) : 0);
		int scheduleId = ((foundRow != null) ? Conversion.ToInt(foundRow["ContractorWagesRateScheduleId"]) : 0);
		row.Cells["ContractorWagesRateScheduleId"].Value = scheduleId;
		row.Cells["Rate"].Value = rate;
		row.Cells["ParentUomId"].Value = parentUomId;
		decimal qty = Conversion.ToDecimal(row.Cells["Qty"].Value);
		decimal netWeight = Conversion.ToDecimal(row.Cells["ItemNetWeight"].Value);
		decimal amount = default(decimal);
		switch (parentUomId)
		{
		case 2:
			amount = rate * qty;
			break;
		case 3:
			amount = netWeight / 1000m * rate;
			break;
		}
		row.Cells["Amount"].Value = amount;
		row.Cells["NetAmount"].Value = amount + addLess;
	}

	private void FillExtraWagesGridFilterCombo()
	{
		try
		{
			ExtraWagesContractorFilterBind();
			DataTable dtFiltered = new DataTable();
			if (grdExtraWages.GetDataRows().Length != 0 && dtExtraWagesDetail.Rows.Count > 0)
			{
				dtFiltered = (from row in dtExtraWagesDetail.AsEnumerable()
					group row by row.Field<int>("ServiceActivityId") into g
					select g.First()).CopyToDataTable();
			}
			InfragisticsHelper.BindAndRetainSelection(CmbActivtyExtraWages, dtFiltered, "ServiceActivityId", "ServiceActivity", "Service Activity", AllColumns: false, Conversion.ToInt(CmbActivtyExtraWages.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInExtraWagesGrid()
	{
		if (dtExtraWagesCombo.Rows.Count > 0)
		{
			dtExtraWagesDetail.Rows.Add(0, 0, 0, 0, "", Conversion.ToInt(dtExtraWagesCombo.Rows[0][0]), Conversion.ToString(dtExtraWagesCombo.Rows[0][1]), 0, 0, 0, 0, "");
			grdExtraWages.DataSource = dtExtraWagesDetail;
		}
		else
		{
			dtExtraWagesDetail.Rows.Add(0, 0, 0, 0, "", 0, "", 0, 0, 0, 0, "");
			grdExtraWages.DataSource = dtExtraWagesDetail;
		}
	}

	private void grdExtraWagesGridSetting(GridEX gridEX)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "InvGdnId", "ContractorId", "ServiceActivityId" };
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
					"Amount",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 2, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			gridEX.RootTable.Columns["ContractorName"].Width = 150;
			gridEX.RootTable.Columns["ServiceActivity"].Width = 150;
			gridEX.RootTable.Columns["Qty"].Width = 70;
			gridEX.RootTable.Columns["Rate"].Width = 80;
			gridEX.RootTable.Columns["Amount"].Width = 100;
			gridEX.RootTable.Groups.Add("ServiceActivity");
			gridEX.RootTable.GroupTotals = (GroupTotals)2;
			if (((Control)(object)gridEX).Name == "grdExtraWages")
			{
				GridEX_Helper.AddButton(gridEX, "DeleteAll", "DeleteAll", 80, 0);
				GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 1);
				GridEX_Helper.AddButton(gridEX, "Add", "+", 20, 2);
				gridEX.FrozenColumns = 3;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdExtraWages_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdExtraWages.UpdateData();
			GridEXRow item = grdExtraWages.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
				{
					if (item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
					{
						decimal num = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
						decimal Rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
						decimal Amount = num * Rate;
						item.Cells["Amount"].Value = Math.Round(Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
				}
				else if (e.Column.Key == "Amount")
				{
					item.Cells["Amount"].Value = Math.Round(Conversion.ToDecimal(item.Cells["Amount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
			}
			grdExtraWages.UpdateData();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdExtraWages_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdExtraWages.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					ExtraWagesDeleteRow(r);
				}
				if (e.Column.Key == "DeleteAll")
				{
					ExtraWagesDeleteAllButtonClick(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInExtraWagesGrid();
				}
				grdExtraWages.UpdateData();
				txtExchangeRate_TextChanged(null, null);
				FillExtraWagesGridFilterCombo();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExtraWagesDeleteRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ContractorWagesBillDetail vd = new ContractorWagesBillDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				FillExtraWagesDetailListCommonForInsertAndDelete(vd, r);
				vd.ActionTypeId = 3;
				lstRemoveDetailRecord.Add(vd);
				dtExtraWagesDetail.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			else
			{
				r.Delete();
			}
			if (dtExtraWagesDetail.Rows.Count == 0)
			{
				AddRowInExtraWagesGrid();
			}
			grdExtraWages.UpdateData();
			txtExchangeRate_TextChanged(null, null);
			FillExtraWagesGridFilterCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExtraWagesDeleteAllButtonClick(GridEXRow clickedRow)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		try
		{
			GridEXRow[] allRows = grdExtraWages.GetDataRows();
			if (allRows.Length == 0)
			{
				return;
			}
			string targetActivity = Conversion.ToString(clickedRow.Cells["ServiceActivity"].Value);
			if (MessageBox.Show("Are you sure to Delete All Rows in Extra Wages Grid?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
			{
				return;
			}
			GridEXRow[] array = allRows;
			foreach (GridEXRow row in array)
			{
				if (!(Conversion.ToString(row.Cells["ServiceActivity"].Value) != targetActivity))
				{
					int rowId = Conversion.ToInt(row.Cells["Id"].Value);
					if (rowId > 0)
					{
						ContractorWagesBillDetail wageDetail = new ContractorWagesBillDetail
						{
							Id = rowId,
							ActionTypeId = 3
						};
						FillExtraWagesDetailListCommonForInsertAndDelete(wageDetail, row);
						lstRemoveDetailRecord.Add(wageDetail);
						dtExtraWagesDetail.Rows[row.RowIndex].Delete();
					}
					row.Delete();
				}
			}
			if (dtExtraWagesDetail.Rows.Count == 0)
			{
				AddRowInExtraWagesGrid();
			}
			grdExtraWages.UpdateData();
			txtExchangeRate_TextChanged(null, null);
			FillExtraWagesGridFilterCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateExtraWages_Click(object sender, EventArgs e)
	{
		try
		{
			GridEXRow[] rows = grdExtraWages.GetDataRows();
			if (rows.Length == 0)
			{
				throw new Exception("Wages Grid Record not Found");
			}
			bool hasContractor = ((UltraGridBase)CmbContractorExtraWages).ActiveRow != null && Conversion.ToInt(CmbContractorExtraWages.Value) != 0;
			bool hasActivity = ((UltraGridBase)CmbActivtyExtraWages).ActiveRow != null && Conversion.ToInt(CmbActivtyExtraWages.Value) != 0;
			if (!hasContractor && !hasActivity)
			{
				int firstRowContrId = Conversion.ToInt(rows[0].Cells["ContractorId"].Value);
				string firstRowContrName = Conversion.ToString(rows[0].Cells["ContractorName"].Value);
				if (firstRowContrId == 0)
				{
					throw new Exception("ContractorName field is empty in Wages Grid Row No: 1");
				}
				ExtraWagesUpdateContractorForRows(rows, firstRowContrId, firstRowContrName);
			}
			else if (hasContractor && !hasActivity)
			{
				int contrId = Conversion.ToInt(CmbContractorExtraWages.Value);
				string contrName = Conversion.ToString((object)((Control)(object)CmbContractorExtraWages).Text);
				ExtraWagesUpdateContractorForRows(rows, contrId, contrName);
			}
			else
			{
				if (!hasContractor && hasActivity)
				{
					CmbContractorExtraWages.Focus();
					throw new Exception("ContractorName field is empty");
				}
				int contrId2 = Conversion.ToInt(CmbContractorExtraWages.Value);
				string contrName2 = Conversion.ToString((object)((Control)(object)CmbContractorExtraWages).Text);
				string selectedActivity = Conversion.ToString((object)((Control)(object)CmbActivtyExtraWages).Text);
				IEnumerable<GridEXRow> matchingRows = rows.Where((GridEXRow r) => Conversion.ToString(r.Cells["ServiceActivity"].Value) == selectedActivity);
				ExtraWagesUpdateContractorForRows(matchingRows, contrId2, contrName2);
			}
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExtraWagesUpdateContractorForRows(IEnumerable<GridEXRow> rows, int contractorId, string contractorName)
	{
		foreach (GridEXRow row in rows)
		{
			row.BeginEdit();
			row.Cells["ContractorId"].Value = contractorId;
			row.Cells["ContractorName"].Value = contractorName;
			row.EndEdit();
		}
	}

	private void BtnNewPending_Click(object sender, EventArgs e)
	{
		try
		{
			CmbDateTypePending.Value = 3;
			txtFromDocPending.Text = string.Empty;
			txtToDocPending.Text = string.Empty;
			((Control)(object)CmbCustomerPending).Text = string.Empty;
			CmbDateTypePending.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshPending_Click(object sender, EventArgs e)
	{
		try
		{
			InfragisticsHelper.BindComboDateType(CmbDateTypePending);
			PendingComboBind(PendingComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable PendingComboDbCall()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			Conversion.ToInt(CmbCustomerHistory.Value);
			return InvGdn.GetDataForDropDownFromGdn(new ReportsParameters
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

	private void PendingComboBind(DataTable dt)
	{
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Name");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier_Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerPending, dtCustomer, "Id", "Name", "Customer Name", AllColumns: true, Conversion.ToInt(CmbCustomerPending.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDateTypePending_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(CmbDateTypePending.Value) == 1)
			{
				txtFromDatePending.Value = DateTime.Today;
				txtToDatePending.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypePending.Value) == 2)
			{
				txtFromDatePending.Value = DateTime.Today.AddDays(-7.0);
				txtToDatePending.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypePending.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromDatePending.Value = first;
				txtToDatePending.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypePending.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromDatePending.Value = first2;
				txtToDatePending.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypePending.Value) == 5)
			{
				txtFromDatePending.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				txtToDatePending.Value = DateTime.Today;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowPending_Click(object sender, EventArgs e)
	{
		try
		{
			PendingGdnGridFill(PendingGdnDBCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable PendingGdnDBCall(bool AllFilters = true)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.BranchesId = UserAccount.BranchesId;
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PI.DocumentTypeId = 1855;
			if (AllFilters)
			{
				if (txtFromDatePending.Checked)
				{
					PI.FromDate = txtFromDatePending.Value;
				}
				if (txtToDatePending.Checked)
				{
					PI.ToDate = txtToDatePending.Value;
				}
				PI.FromDocNo = Conversion.ToInt((object)txtFromDocPending.Text);
				PI.ToDocNo = Conversion.ToInt((object)txtToDocPending.Text);
				PI.SupplierCustomerId = Conversion.ToInt(CmbCustomerPending.Value);
			}
			else
			{
				int Days = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DefaultDaysToLessFromHistoryFromDate : 7);
				PI.FromDate = DateTime.Now.AddDays(-Days);
				PI.ToDate = DateTime.Now;
			}
			return InvGdn.InvGdn_PendingForWages(PI);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PendingGdnGridFill(DataTable dthistory)
	{
		try
		{
			if (dthistory.Rows.Count > 0)
			{
				DataTable dtResult = new DataTable();
				dtResult.Columns.Add("Id", typeof(int));
				dtResult.Columns.Add("DocumentTypeId", typeof(int));
				dtResult.Columns.Add("DocumentType", typeof(string));
				dtResult.Columns.Add("DocDate", typeof(DateTime));
				dtResult.Columns.Add("DocNo", typeof(int));
				dtResult.Columns.Add("InvoiceNo", typeof(int));
				dtResult.Columns.Add("SupplierCustomerId", typeof(int));
				dtResult.Columns.Add("CustomerName", typeof(string));
				dtResult.Columns.Add("OrderType", typeof(string));
				dtResult.Columns.Add("DoId", typeof(int));
				dtResult.Columns.Add("DoNo", typeof(int));
				dtResult.Columns.Add("DoDate", typeof(DateTime));
				dtResult.Columns.Add("SaleOrderNo", typeof(int));
				dtResult.Columns.Add("ReferenceNo", typeof(string));
				dtResult.Columns.Add("Distance", typeof(string));
				dtResult.Columns.Add("ReferencePartyName", typeof(string));
				dtResult.Columns.Add("ReferencePartyAddress", typeof(string));
				dtResult.Columns.Add("ReferencePartyCellNo", typeof(string));
				dtResult.Columns.Add("DeliveryTerm", typeof(string));
				dtResult.Columns.Add("TransporterName", typeof(string));
				dtResult.Columns.Add("CarriageAmount", typeof(decimal));
				dtResult.Columns.Add("GpNo", typeof(string));
				dtResult.Columns.Add("GpDate", typeof(DateTime));
				dtResult.Columns.Add("VehicleType", typeof(string));
				dtResult.Columns.Add("VehicleNo", typeof(string));
				dtResult.Columns.Add("DriverName", typeof(string));
				dtResult.Columns.Add("DriverCNICNo", typeof(string));
				dtResult.Columns.Add("DriverCellNo", typeof(string));
				dtResult.Columns.Add("IsApproved", typeof(bool));
				dtResult.Columns.Add("EntryDate", typeof(DateTime));
				dtResult.Columns.Add("EntryUserName", typeof(string));
				dtResult.Columns.Add("ModifyDate", typeof(DateTime));
				dtResult.Columns.Add("ModifyUserName", typeof(string));
				dtResult.Columns.Add("RemarksHeader", typeof(string));
				dtResult.Columns.Add("NoOfAttachments", typeof(int));
				foreach (DataRow row in dthistory.Rows)
				{
					dtResult.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocumentType"], row["DocDate"], row["DocNo"], row["InvoiceNo"], row["SupplierCustomerId"], row["CustomerName"], row["OrderType"], row["OrderId"], row["DoNo"], row["DoDate"], row["SaleOrderNo"], row["ReferenceNo"], row["Distance"], row["ReferencPartyName"], row["ReferencPartyAddress"], row["ReferencPartyCellNo"], row["DeliveryTerm"], row["TransporterName"], row["CarriageAmount"], row["GpNo"], row["GpDate"], row["VehicleType"], row["VehicleNo"], row["DriverName"], row["DriverCNICNo"], row["DriverCellNo"], row["IsApproved"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["RemarksHeader"], row["NoOfAttachments"]);
				}
				grdPendingGdns.DataSource = dtResult;
				grdPendingGdns.RetrieveStructure();
				grdPendingGdnsSetting();
			}
			else
			{
				grdPendingGdns.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingGdnsSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DoId", "OrderType", "DocumentTypeId", "SupplierCustomerId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdPendingGdns, 3, 2, hiddenColumns);
			grdPendingGdns.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdPendingGdns.RootTable.Columns["NoOfAttachments"].Caption = "Attached";
			grdPendingGdns.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdPendingGdns.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdPendingGdns.RootTable.Columns["DocDate"].Width = 80;
			grdPendingGdns.RootTable.Columns["DocNo"].Width = 65;
			grdPendingGdns.RootTable.Columns["InvoiceNo"].Width = 70;
			grdPendingGdns.RootTable.Columns["CustomerName"].Width = 140;
			grdPendingGdns.RootTable.Columns["OrderType"].Width = 100;
			grdPendingGdns.RootTable.Columns["DoNo"].Width = 65;
			grdPendingGdns.RootTable.Columns["DoDate"].Width = 80;
			grdPendingGdns.RootTable.Columns["SaleOrderNo"].Width = 70;
			grdPendingGdns.RootTable.Columns["ReferenceNo"].Width = 100;
			grdPendingGdns.RootTable.Columns["Distance"].Width = 90;
			grdPendingGdns.RootTable.Columns["ReferencePartyName"].Width = 120;
			grdPendingGdns.RootTable.Columns["ReferencePartyAddress"].Width = 120;
			grdPendingGdns.RootTable.Columns["ReferencePartyCellNo"].Width = 100;
			grdPendingGdns.RootTable.Columns["DeliveryTerm"].Width = 100;
			grdPendingGdns.RootTable.Columns["TransporterName"].Width = 120;
			grdPendingGdns.RootTable.Columns["CarriageAmount"].Width = 80;
			grdPendingGdns.RootTable.Columns["GpNo"].Width = 65;
			grdPendingGdns.RootTable.Columns["GpDate"].Width = 80;
			grdPendingGdns.RootTable.Columns["VehicleType"].Width = 100;
			grdPendingGdns.RootTable.Columns["VehicleNo"].Width = 100;
			grdPendingGdns.RootTable.Columns["DriverName"].Width = 120;
			grdPendingGdns.RootTable.Columns["DriverCNICNo"].Width = 100;
			grdPendingGdns.RootTable.Columns["DriverCellNo"].Width = 100;
			grdPendingGdns.RootTable.Columns["IsApproved"].Width = 80;
			grdPendingGdns.RootTable.Columns["EntryDate"].Width = 80;
			grdPendingGdns.RootTable.Columns["EntryUserName"].Width = 100;
			grdPendingGdns.RootTable.Columns["ModifyDate"].Width = 80;
			grdPendingGdns.RootTable.Columns["ModifyUserName"].Width = 100;
			grdPendingGdns.RootTable.Columns["RemarksHeader"].Width = 130;
			grdPendingGdns.RootTable.Columns["NoOfAttachments"].Width = 90;
			GridEX_Helper.AddButton(grdPendingGdns, "Load", "Load", 50, 0);
			GridEX_Helper.AddSelectButton(grdPendingGdns, "Select", 30, 1);
			grdPendingGdns.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdPendingGdns_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPendingGdns.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0 && e.Column.Key == "Load")
			{
				PendingGdnLoad(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingGdnLoad(GridEXRow r)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				throw new Exception("Can not load in Update Mode. Please save or clear the form before proceeding.");
			}
			if (grdGdn.GetDataRows().Count() > 0)
			{
				throw new Exception("There is already data in Gdn Detail. Please save or clear the form before proceeding.");
			}
			if (grdContractorWages.GetDataRows().Length != 0)
			{
				GridEXRow firstRow2 = grdContractorWages.GetRow(0);
				if (firstRow2 != null && (int)firstRow2.RowType == 0 && (Conversion.ToInt(firstRow2.Cells["ItemId"].Value) > 0 || Conversion.ToDecimal(firstRow2.Cells["Amount"].Value) > 0m))
				{
					throw new Exception("There is already data Wages Detail. Please save or clear the form before proceeding.");
				}
			}
			GridEXRow[] checkedRows = grdPendingGdns.GetCheckedRows();
			string grnIds;
			if (checkedRows.Length == 0)
			{
				grnIds = Conversion.ToString(r.Cells["Id"].Value);
				txtremarks.Text = Conversion.ToString(r.Cells["RemarksHeader"].Value);
				CmbCustomerName.Value = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				((Control)(object)CmbCustomerName).Enabled = false;
				cmbCurrency_Leave(null, null);
			}
			else
			{
				grnIds = string.Join(",", checkedRows.Select((GridEXRow rr) => Conversion.ToInt(rr.Cells["Id"].Value)).Distinct());
				List<int> customerIds = checkedRows.Select((GridEXRow rr) => Conversion.ToInt(rr.Cells["SupplierCustomerId"].Value)).Distinct().ToList();
				List<int> DocumentTypeIds = checkedRows.Select((GridEXRow rr) => Conversion.ToInt(rr.Cells["DocumentTypeId"].Value)).Distinct().ToList();
				if (customerIds.Count > 1)
				{
					MessageBox.Show("You have selected multiple customers's Rows. Please select a single customer's Rows.");
					return;
				}
				if (DocumentTypeIds.Count > 1)
				{
					MessageBox.Show("You have selected multiple DocumentType's Rows. Please select a single DocumentType's Rows.");
					return;
				}
				GridEXRow firstRow = checkedRows[0];
				txtremarks.Text = Conversion.ToString(firstRow.Cells["RemarksHeader"].Value);
				CmbCustomerName.Value = customerIds.First();
				RefDocumentTypeId = DocumentTypeIds.First();
				((Control)(object)CmbCustomerName).Enabled = false;
				cmbCurrency_Leave(null, null);
			}
			if (!string.IsNullOrEmpty(grnIds))
			{
				LoadGdnData(grnIds, grdGdn, dtGdnDetail);
				LoadwagesData(grnIds);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadGdnData(string invGdnIds, GridEX gridEX, DataTable dtGdn)
	{
		try
		{
			DataTable item = InvGdn.GdnDetailByGdnIds(invGdnIds);
			if (item.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> existingIds = new HashSet<int>(from r in gridEX.GetDataRows()
				select Conversion.ToInt(r.Cells["Id"].Value));
			bool rowsAdded = false;
			foreach (DataRow row in item.Rows)
			{
				int rowId = Conversion.ToInt(row["Id"]);
				if (existingIds.Add(rowId))
				{
					dtGdn.Rows.Add(rowId, row["InvGdnId"], row["GdnNo"], row["OrderId"], row["OrderDetailId"], row["OrderNo"], row["DoOrderDetailId"], row["WarehouseId"], row["WareHouseName"], row["ItemId"], row["ItemName"], row["ItemCode"], row["ItemAttributeVarientId"], row["ItemAttributeVarient"], row["VarientEquivalent"], row["JobLotId"], row["JobLotDescription"], row["ItemQty"], row["ItemWeight"], row["ItemNetWeight"], row["CityId"], row["CityName"], row["IsFOC"], row["RemarksDetail"]);
					rowsAdded = true;
				}
			}
			if (rowsAdded)
			{
				gridEX.DataSource = dtGdn;
				gridEX.RetrieveStructure();
				grdGdnSettings(gridEX);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadwagesData(string invGdnIds)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable item = InvGdn.GdnWagesDetailByGdnIds(invGdnIds);
			if (item.Rows.Count == 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdContractorWages.GetDataRows();
			if (dataRows.Length == 1)
			{
				GridEXRow firstRow = dataRows[0];
				if (firstRow != null && (int)firstRow.RowType == 0)
				{
					GridEXCell obj = firstRow.Cells["ItemId"];
					if (Conversion.ToInt((obj != null) ? obj.Value : null) == 0)
					{
						GridEXCell obj2 = firstRow.Cells["Amount"];
						if (Conversion.ToDecimal((obj2 != null) ? obj2.Value : null) == 0m)
						{
							dtWagesDetail.Clear();
						}
					}
				}
			}
			foreach (DataRow row in item.Rows)
			{
				dtWagesDetail.Rows.Add(0, row["InvGdnId"], row["GdnNo"], row["ItemId"], row["ItemName"], row["ItemAttributeVarientId"], row["ItemAttributeVarient"], row["VarientEquivalent"], row["ContractorId"], row["ContractorName"], row["ContractorWagesRateScheduleId"], row["ServiceActivity"], row["ParentUomId"], row["Qty"], row["ItemNetWeight"], row["Rate"], row["Amount"], row["AddLess"], row["NetAmount"], 0, row["Remarks"], row["ContractorWagesActivityId"]);
			}
			if (dtWagesDetail.Rows.Count == 0)
			{
				AddRowInContractorWagesGrid();
			}
			else
			{
				grdContractorWages.DataSource = dtWagesDetail;
			}
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting(grdContractorWages);
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
			RefDocumentTypeId = 0;
			WagesRateCatchDuringLoop.Clear();
			Approved = false;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			btnsave.Visible = true;
			lstRemoveDetailRecord.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			((Control)(object)CmbCustomerName).Text = string.Empty;
			((Control)(object)CmbCustomerName).Enabled = true;
			txtCustomerCellNo.Text = string.Empty;
			txtCustomerCity.Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtTotalAmount.Text = "0.00";
			dtGdnDetail.Rows.Clear();
			dtWagesDetail.Rows.Clear();
			dtExtraWagesDetail.Rows.Clear();
			BindGrids();
			UpdateDocNoUI(DocumentNoDbCall());
			GetConfigurationsFromGlobal();
			ImplementConfiguration();
			PendingGdnGridFill(PendingGdnDBCall());
			DocDate.Focus();
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
			List<string> allServices = new List<string> { "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "MultiCurrency" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			CurrencyBindFromGlobal();
			dtForGridComboFill();
			BindComboGrids();
			ImplementConfiguration();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void frmGdnWages_FormClosing(object sender, FormClosingEventArgs e)
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

	private void WagesValidationWithDetail()
	{
		try
		{
			if (grdGdn.RowCount <= 0 || grdContractorWages.RowCount <= 0 || dtVarientForWages.Rows.Count <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdContractorWages.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				int itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				int invGdnId = Conversion.ToInt(r.Cells["InvGdnId"].Value);
				int gdnNo = Conversion.ToInt(r.Cells["GdnNo"].Value);
				string item = Conversion.ToString(r.Cells["ItemName"].Value);
				string attributeVarient = Conversion.ToString(r.Cells["AttributeVarient"].Value);
				int attributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
				int contractorWagesRateScheduleId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
				if (itemId != 0)
				{
					if (!dtGdnDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemId") == itemId && row.Field<int>("InvGdnId") == invGdnId))
					{
						throw new Exception($"Item '{item}' for GdnNo '{gdnNo}' in Contractor Wages Grid does not exist in Detail Grid.");
					}
					decimal totalWagesGridQty = (from row in dtGdnDetail.AsEnumerable()
						where row.Field<int>("ItemId") == itemId && row.Field<int>("InvGdnId") == invGdnId && row.Field<int>("AttributeVarientId") == attributeVarientId
						select row).Sum((DataRow row) => row.Field<decimal>("ItemQty"));
					decimal totalDetailGridQty = (from row in dtWagesDetail.AsEnumerable()
						where row.Field<int>("ItemId") == itemId && row.Field<int>("InvGdnId") == invGdnId && row.Field<int>("AttributeVarientId") == attributeVarientId && row.Field<int>("ContractorWagesRateScheduleId") == contractorWagesRateScheduleId
						select row).Sum((DataRow row) => row.Field<decimal>("Qty"));
					if (totalWagesGridQty != totalDetailGridQty)
					{
						throw new Exception($"Qty mismatch for Item '{item}' and Variant '{attributeVarient}' for GdnNo '{gdnNo}'. Wages Grid Qty = {totalWagesGridQty}, Detail Grid Qty = {totalDetailGridQty}.");
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Wages Validation Failed: " + ex.Message, ex);
		}
	}

	private void FillWagesDetailListCommonForInsertAndDelete(ContractorWagesBillDetail vd, GridEXRow r)
	{
		vd.WagesTypeId = 1;
		vd.RefDocumentTypeId = 1855;
		vd.RefDocNoId = Conversion.ToInt(r.Cells["InvGdnId"].Value);
		vd.RefDocNo = Conversion.ToInt(r.Cells["GdnNo"].Value);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
		vd.ItemAttributeVarient = Conversion.ToString(r.Cells["AttributeVarient"].Value);
		vd.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientEquivalent"].Value);
		vd.ContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
		vd.ContractorName = Conversion.ToString(r.Cells["ContractorName"].Value);
		vd.ContractorWagesRateScheduleId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
		vd.ContractorWagesActivityId = Conversion.ToInt(r.Cells["ContractorWagesActivityId"].Value);
		vd.ContractorWagesActivity = Conversion.ToString(r.Cells["ServiceActivity"].Value);
		vd.ParentUomId = Conversion.ToInt(r.Cells["ParentUomId"].Value);
		vd.Qty = Conversion.ToDecimal(r.Cells["QTY"].Value);
		vd.ItemNetWeight = Conversion.ToDecimal(r.Cells["ItemNetWeight"].Value);
		vd.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
		vd.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
		vd.AddLess = Conversion.ToDecimal(r.Cells["AddLess"].Value);
		vd.NetAmount = Conversion.ToDecimal(r.Cells["NetAmount"].Value);
		vd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
		vd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
		vd.FcyAmount = vd.NetAmount * vd.ExchangeRate;
		vd.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	private void FillExtraWagesDetailListCommonForInsertAndDelete(ContractorWagesBillDetail vd, GridEXRow r)
	{
		vd.WagesTypeId = 2;
		vd.RefDocumentTypeId = 1855;
		vd.RefDocNoId = Conversion.ToInt(r.Cells["InvGdnId"].Value);
		vd.RefDocNo = Conversion.ToInt(r.Cells["GdnNo"].Value);
		vd.ItemId = 0;
		vd.ItemCode = "";
		vd.ItemName = "";
		vd.ItemAttributeVarientId = 0;
		vd.ItemAttributeVarient = "";
		vd.VarientEquivalent = 0m;
		vd.ContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
		vd.ContractorName = Conversion.ToString(r.Cells["ContractorName"].Value);
		vd.ContractorWagesActivityId = Conversion.ToInt(r.Cells["ServiceActivityId"].Value);
		vd.ContractorWagesRateScheduleId = 0;
		vd.ContractorWagesActivity = Conversion.ToString(r.Cells["ServiceActivity"].Value);
		vd.ParentUomId = 0;
		vd.Qty = Conversion.ToDecimal(r.Cells["QTY"].Value);
		vd.ItemNetWeight = 0m;
		vd.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
		vd.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
		vd.AddLess = 0m;
		vd.NetAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
		vd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
		vd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
		vd.FcyAmount = vd.NetAmount * vd.ExchangeRate;
		vd.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	private bool Insert(bool IsAutoUpdate = false, int RecUpdateId = 0)
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		try
		{
			if (grdContractorWages.GetDataRows().Count() == 0 || grdGdn.GetDataRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				(txtdocno, "Doc No", FormHelper.ValidType.INT),
				((Control)(object)CmbCustomerName, "Customer", FormHelper.ValidType.INT)
			}))
			{
				return false;
			}
			if (!FormHelper.ConfirmAction((IsAutoUpdate || RecId != 0) ? "Are you sure to Update" : "Are you sure to Save"))
			{
				return false;
			}
			WagesValidationWithDetail();
			txtExchangeRate_TextChanged(null, null);
			ContractorWagesBill po = new ContractorWagesBill();
			if (IsAutoUpdate)
			{
				po.Id = RecUpdateId;
				po.AutoUpdate = 1;
			}
			else
			{
				po.Id = RecId;
			}
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.BranchId = UserAccount.BranchesId;
			po.ProjectId = UserAccount.BranchesId;
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.DocumentTypeId = DocumentTypeId;
			po.EntryUserId = UserAccount.ID;
			po.EntryDate = DateTime.Now;
			po.ModifyUserId = UserAccount.ID;
			po.ModifyDate = DateTime.Now;
			po.ApprovedUserId = UserAccount.ID;
			po.ApprovedDate = DateTime.Now;
			po.IsApproved = false;
			po.RefDocDate = DateTime.Now;
			po.DocNo = Conversion.ToInt((object)txtdocno.Text);
			po.DocDate = Conversion.ToDateTime((object)DocDate.Text);
			po.SupplierCustomerId = Conversion.ToInt(CmbCustomerName.Value);
			po.RefDocumentTypeId = RefDocumentTypeId;
			po.RefDocNoId = 0;
			po.RefDocNo = 0;
			po.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
			po.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			po.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			po.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			po.ContractorWagesBillDetailList = new List<ContractorWagesBillDetail>();
			po.AttachmentsList = new List<DMSAttachments>();
			po.DeleteAttachmentsList = new List<DMSAttachments>();
			int rowIndex = 0;
			GridEXRow[] dataRows = grdContractorWages.GetDataRows();
			foreach (GridEXRow r2 in dataRows)
			{
				ContractorWagesBillDetail vd2 = new ContractorWagesBillDetail();
				vd2.Id = ((RecId != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
				vd2.ActionTypeId = ((vd2.Id <= 0) ? 1 : 2);
				FillWagesDetailListCommonForInsertAndDelete(vd2, r2);
				FormHelper.ValidateField(vd2.RefDocNoId, "Gdn No", rowIndex, "Wages Detail");
				FormHelper.ValidateField(vd2.ItemId, "Item", rowIndex, "Wages Detail");
				FormHelper.ValidateField(vd2.ItemAttributeVarientId, "Variant", rowIndex, "Wages Detail");
				FormHelper.ValidateField(vd2.ContractorId, "Contractor", rowIndex, "Wages Detail");
				FormHelper.ValidateField(vd2.ContractorWagesRateScheduleId, "Activity", rowIndex, "Wages Detail");
				FormHelper.ValidateField(vd2.Qty, "Qty", rowIndex, "Wages Detail");
				FormHelper.ValidateField(vd2.ItemNetWeight, "M.Ton", rowIndex, "Wages Detail");
				FormHelper.ValidateField(vd2.Rate, "Rate", rowIndex, "Wages Detail");
				FormHelper.ValidateField(vd2.Amount, "Amount", rowIndex, "Wages Detail");
				po.ContractorWagesBillDetailList.Add(vd2);
				rowIndex++;
			}
			if (po.ContractorWagesBillDetailList.Count == 0)
			{
				throw new Exception("At least enter value/quantity in one of the rows of detail ");
			}
			rowIndex = 0;
			dataRows = grdExtraWages.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				if (Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0)
				{
					ContractorWagesBillDetail vd = new ContractorWagesBillDetail();
					vd.Id = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
					FillExtraWagesDetailListCommonForInsertAndDelete(vd, r);
					FormHelper.ValidateField(vd.RefDocNoId, "Gdn No", rowIndex, "Extra Wages Detail");
					FormHelper.ValidateField(vd.ContractorId, "Contractor", rowIndex, "Extra Wages Detail");
					FormHelper.ValidateField(vd.ContractorWagesActivityId, "Activity", rowIndex, "Extra Wages Detail");
					FormHelper.ValidateField(vd.Qty, "Qty", rowIndex, "Extra Wages Detail");
					FormHelper.ValidateField(vd.Rate, "Rate", rowIndex, "Extra Wages Detail");
					FormHelper.ValidateField(vd.Amount, "Amount", rowIndex, "Extra Wages Detail");
					po.ContractorWagesBillDetailList.Add(vd);
				}
				rowIndex++;
			}
			po.TotalQty = Conversion.ToDouble((object)po.ContractorWagesBillDetailList.Sum((ContractorWagesBillDetail x) => x.Qty));
			if (lstRemoveDetailRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveDetailRecord.Count; i++)
				{
					po.ContractorWagesBillDetailList.Add(lstRemoveDetailRecord[i]);
				}
			}
			FormHelper.UpdateAttachmentsForObject<ContractorWagesBill>(po, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			po.AttachmentsValues = AttachmentsValues;
			po.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = (po.Id = ContractorWagesBill.Save(po));
			if (IsAutoUpdate)
			{
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && Attachment.dtAttachments.Rows.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecId, base.Name, DocumentTypeId, 0);
				}
				Reset();
			}
			else
			{
				if (RecId > 0)
				{
					MessageBox.Show("Data Update Successfully....  " + po.DocNo);
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && Attachment.dtAttachments.Rows.Count == 0)
					{
						DMSAttachments.RemoveByIdAndNames(RecId, base.Name, DocumentTypeId, 0);
					}
				}
				else
				{
					MessageBox.Show("Data Save Successfully....  " + po.DocNo);
				}
				Reset();
				if (ChkPrintslip.Checked)
				{
					CommonServices.ContractorWagesBillSlipConcrete_1869(success);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
		return true;
	}

	private void BtnSave_Click(object sender, EventArgs e)
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
				throw new Exception("Recid Not foundd");
			}
			if (Approved)
			{
				throw new Exception("Record Not Update beacause Record has approved");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			Reset();
			RecId = ID;
			ContractorWagesBill invPI = ContractorWagesBill.ReadById(RecId);
			if (invPI != null)
			{
				tabControl1.SelectedIndex = 0;
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				btnDelete.Visible = true;
				DocDate.Value = invPI.DocDate;
				txtdocno.Text = invPI.DocNo.ToString();
				CmbCustomerName.Value = invPI.SupplierCustomerId;
				((Control)(object)CmbCustomerName).Enabled = false;
				CmbCustomerName_Leave(null, null);
				txtTotalQty.Text = invPI.TotalQty.ToString("#,##0");
				cmbCurrency.Value = invPI.CurrencyId;
				txtExchangeRate.Text = invPI.ExchangeRate.ToString(clsGlobalVariables.DecimalRateFormate);
				txtFcyAmount.Text = invPI.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				RefDocumentTypeId = invPI.RefDocumentTypeId;
				Approved = invPI.IsApproved;
				AttachmentsValues = invPI.AttachmentsValues;
				CustomAttachmentsValues = invPI.CustomAttachmentsValues;
				dtWagesDetail.Rows.Clear();
				dtExtraWagesDetail.Rows.Clear();
				FillDetailFromListCommonForReadById(invPI, dtWagesDetail, dtExtraWagesDetail);
				dtGdnDetail.Rows.Clear();
				string grnIds = string.Join(",", (from row in dtWagesDetail.AsEnumerable()
					select Conversion.ToInt(row["InvGdnId"])).Distinct());
				LoadGdnData(grnIds, grdGdn, dtGdnDetail);
				BindGrids();
				FormHelper.LoadAttachmentsForObject(AT, RecId, ScreenName);
				txtExchangeRate_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromListCommonForReadById(ContractorWagesBill invGrn, DataTable dtWages, DataTable dtExtraWages)
	{
		if (((invGrn != null) ? invGrn.ContractorWagesBillDetailList : null) == null || invGrn.ContractorWagesBillDetailList.Count == 0)
		{
			return;
		}
		foreach (ContractorWagesBillDetail detail in invGrn.ContractorWagesBillDetailList)
		{
			if (detail.WagesTypeId == 2)
			{
				dtExtraWages.Rows.Add(detail.Id, detail.RefDocNoId, detail.RefDocNo, detail.ContractorId, detail.ContractorName, detail.ContractorWagesActivityId, detail.ContractorWagesActivity, detail.Qty, detail.Rate, detail.Amount, detail.FcyAmount, detail.RemarksDetail);
			}
			else
			{
				dtWages.Rows.Add(detail.Id, detail.RefDocNoId, detail.RefDocNo, detail.ItemId, detail.ItemName, detail.ItemAttributeVarientId, detail.ItemAttributeVarient, detail.VarientEquivalent, detail.ContractorId, detail.ContractorName, detail.ContractorWagesRateScheduleId, detail.ContractorWagesActivity, detail.ParentUomId, detail.Qty, detail.ItemNetWeight, detail.Rate, detail.Amount, detail.AddLess, detail.NetAmount, detail.FcyAmount, detail.RemarksDetail, detail.ContractorWagesActivityId);
			}
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Not Delete because RecId Not Found");
			}
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ContractorWagesBill.DeleteByID(UserAccount.ID, RecId);
				MessageBox.Show("Delete Record Seccessfully");
				Reset();
			}
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
			if (Conversion.ToInt(cmbDateTypeHistory.Value) == 0 && ((UltraGridBase)cmbDateTypeHistory).DataSource != null)
			{
				((UltraGridBase)cmbDateTypeHistory).Rows[0].Activate();
			}
			cmbDateTypeHistory.Focus();
		}
	}

	private void Resethistory()
	{
		try
		{
			cmbDateTypeHistory.Value = 3;
			txtFromNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			Resethistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			InfragisticsHelper.BindComboDateType(cmbDateTypeHistory);
			HistoryComboBind(HistoryComboDbCAll());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable HistoryComboDbCAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return InvGdn.GetDataForDropDownFromGdn(new ReportsParameters
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

	private void HistoryComboBind(DataTable dt)
	{
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Name");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier_Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dtCustomer, "Id", "Name", "Customer Name", AllColumns: true, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				txtFromdateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				txtFromdateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromdateHistory.Value = first;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromdateHistory.Value = first2;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				txtFromdateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
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
			FillHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.BranchesId = UserAccount.BranchesId;
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PI.DocumentTypeId = DocumentTypeId;
			PI.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!PI.CanViewAllRecord)
			{
				PI.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.FromDate = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ToDate = txtToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.EntryDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.EntryDateTo = txtToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.ModifyDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ModifyDateTo = txtToDateHistory.Value;
				}
			}
			PI.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text);
			PI.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			PI.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dthistory = ContractorWagesBill.FormHistory(PI);
			if (dthistory.Rows.Count > 0)
			{
				if (dthistoryGrid.Columns.Count == 0)
				{
					dthistoryGrid.Columns.Add("Id", typeof(int));
					dthistoryGrid.Columns.Add("DocumentTypeId", typeof(int));
					dthistoryGrid.Columns.Add("DocDate", typeof(DateTime));
					dthistoryGrid.Columns.Add("DocNo", typeof(int));
					dthistoryGrid.Columns.Add("RefDocumentType", typeof(string));
					dthistoryGrid.Columns.Add("SupplierCustomerId", typeof(int));
					dthistoryGrid.Columns.Add("CustomerName", typeof(string));
					dthistoryGrid.Columns.Add("TotalQty", typeof(decimal));
					dthistoryGrid.Columns.Add("TotalAmount", typeof(decimal));
					dthistoryGrid.Columns.Add("FcyCode", typeof(string));
					dthistoryGrid.Columns.Add("ExchangeRate", typeof(decimal));
					dthistoryGrid.Columns.Add("FcyAmount", typeof(decimal));
					dthistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dthistoryGrid.Columns.Add("EntryUser", typeof(string));
					dthistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dthistoryGrid.Columns.Add("ModifyUser", typeof(string));
					dthistoryGrid.Columns.Add("IsApproved", typeof(bool));
					dthistoryGrid.Columns.Add("ApprovedDate", typeof(DateTime));
					dthistoryGrid.Columns.Add("ApprovedUser", typeof(string));
					dthistoryGrid.Columns.Add("RemarksHeader", typeof(string));
					dthistoryGrid.Columns.Add("NoOfAttachments", typeof(int));
				}
				dthistoryGrid.Clear();
				foreach (DataRow row in dthistory.Rows)
				{
					dthistoryGrid.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocDate"], row["DocNo"], row["RefDocumentType"], row["SupplierCustomerId"], row["SupplierCustomerName"], row["TotalQty"], Conversion.ToDecimal(row["ExchangeRate"]) * Conversion.ToDecimal(row["FcyAmount"]), row["FcyCode"], row["ExchangeRate"], row["FcyAmount"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["IsApproved"], row["ApprovedDate"], row["ApprovedUserName"], row["RemarksHeader"], row["NoOfAttachments"]);
				}
				grdHistory.DataSource = dthistoryGrid;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdHistory.DataSource = null;
				grdWagesDetailHistory.DataSource = null;
				grdExtraWagesDetailHistory.DataSource = null;
				grdGdnDetailHistory.DataSource = null;
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
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "SupplierCustomerId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 3, 2, hiddenColumns);
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["NoOfAttachments"].Caption = "Attached";
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["DocDate"].Width = 80;
			grdHistory.RootTable.Columns["DocNo"].Width = 65;
			grdHistory.RootTable.Columns["CustomerName"].Width = 140;
			grdHistory.RootTable.Columns["EntryDate"].Width = 80;
			grdHistory.RootTable.Columns["EntryUser"].Width = 100;
			grdHistory.RootTable.Columns["ModifyDate"].Width = 80;
			grdHistory.RootTable.Columns["ModifyUser"].Width = 100;
			grdHistory.RootTable.Columns["IsApproved"].Width = 80;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = 80;
			grdHistory.RootTable.Columns["ApprovedUser"].Width = 100;
			grdHistory.RootTable.Columns["RemarksHeader"].Width = 130;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = 90;
			GridEX_Helper.AddButton(grdHistory, "Print", "Print", 50, 0);
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 50, 1);
			grdHistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdHistory.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				ReadById(Conversion.ToInt(row.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdHistory.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				int RId = Conversion.ToInt(row.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(RId);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.ContractorWagesBillSlipConcrete_1869(RId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdHistory.CurrentRow;
			if (row != null && (int)row.RowType == 0 && row.Cells.Count > 0)
			{
				int RId = Conversion.ToInt(row.Cells["Id"].Value);
				GetDetailGrdByHeadId(RId);
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
			ContractorWagesBill Obj = ContractorWagesBill.ReadById(Id);
			if (Obj == null || Obj.ContractorWagesBillDetailList.Count == 0)
			{
				grdWagesDetailHistory.ClearStructure();
				grdExtraWagesDetailHistory.ClearStructure();
				grdGdnDetailHistory.ClearStructure();
				return;
			}
			DataTable dtWagesDetailHistory = dtWagesDetail.Clone();
			DataTable dtExtraWagesDetailHistory = dtExtraWagesDetail.Clone();
			FillDetailFromListCommonForReadById(Obj, dtWagesDetailHistory, dtExtraWagesDetailHistory);
			grdWagesDetailHistory.DataSource = dtWagesDetailHistory;
			grdWagesDetailHistory.RetrieveStructure();
			grdContractorWagesGridSetting(grdWagesDetailHistory);
			grdExtraWagesDetailHistory.DataSource = dtExtraWagesDetailHistory;
			grdExtraWagesDetailHistory.RetrieveStructure();
			grdExtraWagesGridSetting(grdExtraWagesDetailHistory);
			string grnIds = string.Join(",", (from row in dtWagesDetailHistory.AsEnumerable()
				select Conversion.ToInt(row["InvGdnId"])).Distinct());
			DataTable dtGdnDetailH = dtGdnDetail.Clone();
			LoadGdnData(grnIds, grdGdnDetailHistory, dtGdnDetailH);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ContractorWagesBillSlipConcrete_1869(RecId);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdGdn, dtGdnDetail);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdContractorWages, dtWagesDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistory, dthistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void MultiCurrencyFeature()
	{
		try
		{
			((Control)(object)cmbCurrency).Visible = HasMultiCurrencyFeature;
			txtExchangeRate.Visible = HasMultiCurrencyFeature;
			txtFcyAmount.Visible = HasMultiCurrencyFeature;
			labelFcyCode.Visible = HasMultiCurrencyFeature;
			labelExRate.Visible = HasMultiCurrencyFeature;
			labelFcyAmt.Visible = HasMultiCurrencyFeature;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotalInformation()
	{
		try
		{
			double TotalQty = GridEX_Helper.GetColumnSum(grdContractorWages, "Qty") + GridEX_Helper.GetColumnSum(grdExtraWages, "Qty");
			double ItemAmount = GridEX_Helper.GetColumnSum(grdContractorWages, "NetAmount") + GridEX_Helper.GetColumnSum(grdExtraWages, "Amount");
			double ExchangeRate = Conversion.ToDouble((object)txtExchangeRate.Text);
			double TotalFcyAmount = ((ExchangeRate > 0.0) ? (ItemAmount / ExchangeRate) : 0.0);
			txtTotalQty.Text = Math.Round(TotalQty, 2).ToString("#,##0.##");
			txtTotalAmount.Text = ItemAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtFcyAmount.Text = TotalFcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdContractorWages.GetDataRows().Count() > 0 && Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m)
			{
				GridEXRow[] dataRows = grdContractorWages.GetDataRows();
				foreach (GridEXRow item3 in dataRows)
				{
					if (item3 != null && (int)item3.RowType == 0)
					{
						item3.BeginEdit();
						decimal amount2 = Conversion.ToDecimal(item3.Cells["NetAmount"].Value) / Conversion.ToDecimal((object)txtExchangeRate.Text);
						item3.Cells["FcyAmount"].Value = Math.Round(amount2, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
						item3.EndEdit();
					}
				}
			}
			else
			{
				GridEXRow[] dataRows = grdContractorWages.GetDataRows();
				foreach (GridEXRow item4 in dataRows)
				{
					if (item4 != null && (int)item4.RowType == 0)
					{
						item4.BeginEdit();
						item4.Cells["FcyAmount"].Value = 0;
						item4.EndEdit();
					}
				}
			}
			if (grdExtraWages.GetDataRows().Count() > 0 && Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m)
			{
				GridEXRow[] dataRows = grdExtraWages.GetDataRows();
				foreach (GridEXRow item in dataRows)
				{
					if (item != null && (int)item.RowType == 0)
					{
						item.BeginEdit();
						decimal amount = Conversion.ToDecimal(item.Cells["Amount"].Value) / Conversion.ToDecimal((object)txtExchangeRate.Text);
						item.Cells["FcyAmount"].Value = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
						item.EndEdit();
					}
				}
			}
			else
			{
				GridEXRow[] dataRows = grdExtraWages.GetDataRows();
				foreach (GridEXRow item2 in dataRows)
				{
					if (item2 != null && (int)item2.RowType == 0)
					{
						item2.BeginEdit();
						item2.Cells["FcyAmount"].Value = 0;
						item2.EndEdit();
					}
				}
			}
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private DataTable GetCachedServiceActivity(int itemId, int contractorId)
	{
		(int, int) key = (itemId, contractorId);
		if (!WagesRateCatchDuringLoop.ContainsKey(key))
		{
			WagesRateCatchDuringLoop[key] = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, itemId, Conversion.ToDateTime((object)DocDate.Value), 0, contractorId);
		}
		return WagesRateCatchDuringLoop[key];
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			WagesRateCatchDuringLoop.Clear();
			UpdateWagesRateForAllRows();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateWagesRateForAllRows()
	{
		try
		{
			GridEXRow[] rows = grdContractorWages.GetDataRows();
			if (rows.Length != 0)
			{
				GridEXRow[] array = rows;
				foreach (GridEXRow row in array)
				{
					row.BeginEdit();
					WagesApplyRateAndAmounts(row, 0);
					row.EndEdit();
				}
				txtExchangeRate_TextChanged(null, null);
			}
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
			MessageBox.Show(ex.Message);
		}
	}

	private void frmGdnWages_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			btnRecordsUpdate.Visible = false;
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
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
					cmbDateTypeHistory.Focus();
				}
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.F)
			{
				btnRecordsUpdate.Visible = true;
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
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 83;
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					CmbCustomerName.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdGdn).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdGdn).Focused)
					{
						((Control)(object)grdPendingGdns).Focus();
					}
					if (((Control)(object)grdPendingGdns).Focused)
					{
						((Control)(object)grdContractorWages).Focus();
					}
					if (((Control)(object)grdContractorWages).Focused)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdGdn).Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdGdn).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					if (((Control)(object)grdPendingGdns).Focused)
					{
						txtFromDatePending.Focus();
					}
					else
					{
						DocDate.Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnAttachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnSlip_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnSlip_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdGdnDetailHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdGdnDetailHistory).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdGdnDetailHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)grdHistory).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print Slip");
			dt.Rows.Add("Ctrl+F5", "For Focus on DocDate");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On FromDate in Pending Records's Filters when Pending Grid is Selected OtherWise on DocDate");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					ReadById(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.ContractorWagesBillSlipConcrete_1869(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, DocumentTypeId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				ReadById(CurrId);
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				CommonServices.ContractorWagesBillSlipConcrete_1869(CurrId);
			}
			((Control)(object)grdHistory).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractorWages.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdContractorWages.CurrentColumn == null)
			{
				return;
			}
			string colKey = grdContractorWages.CurrentColumn.Key;
			if (e.Control && e.KeyCode == Keys.Space)
			{
				switch (colKey)
				{
				case "Add":
					AddRowInContractorWagesGrid();
					return;
				case "DeleteAll":
					WagesDeleteAllButtonClick(r);
					return;
				case "Delete":
					WagesDeleteRow(r);
					return;
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInContractorWagesGrid();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				WagesDeleteRow(r);
			}
			else
			{
				if (e.KeyCode != Keys.F1)
				{
					return;
				}
				switch (colKey)
				{
				case "InvGdnId":
				case "GdnNo":
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow((from row in dtVarientForWages.AsEnumerable()
						group row by row.Field<int>("InvGdnId") into g
						select g.FirstOrDefault()).CopyToDataTable(), "InvGdnId", "GdnNo", r, "InvGdnId", "GdnNo", bothNameAndId: true);
					Conversion.ToInt(r.Cells["InvGdnId"].Value);
					break;
				case "ContractorId":
				case "ContractorName":
				{
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtContr, "Id", "CompanyName", r, "ContractorId", "ContractorName", bothNameAndId: true);
					int contractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
					WagesApplyRateAndAmounts(r, contractorId);
					break;
				}
				case "ItemId":
				case "ItemName":
				{
					int invGdnId = Conversion.ToInt(r.Cells["InvGdnId"].Value);
					if (invGdnId == 0)
					{
						throw new Exception("Please Select any Gdn First in This Row");
					}
					DataTable dt = (from rr in dtItemForWages.AsEnumerable()
						where rr.Field<int>("InvGdnId") == invGdnId
						select rr).CopyToDataTable();
					int previousItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dt, "Id", "ItemName", r, "ItemId", "ItemName", bothNameAndId: true);
					if (Conversion.ToInt(r.Cells["ItemId"].Value) != previousItemId)
					{
						r.Cells["AttributeVarientId"].Value = 0;
						r.Cells["AttributeVarient"].Value = "";
						r.Cells["VarientEquivalent"].Value = 0;
						r.Cells["Qty"].Value = 0;
						r.Cells["ItemNetWeight"].Value = 0;
						r.Cells["ContractorWagesRateScheduleId"].Value = 0;
						r.Cells["ServiceActivity"].Value = "";
						r.Cells["ParentUomId"].Value = 0;
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
						r.Cells["NetAmount"].Value = 0;
					}
					break;
				}
				case "AttributeVarientId":
				case "AttributeVarient":
				{
					int itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (Conversion.ToInt(r.Cells["InvGdnId"].Value) == 0)
					{
						throw new Exception("Please Select any Gdn First in This Row");
					}
					if (itemId == 0)
					{
						MessageBox.Show("Please Select an Item First");
						return;
					}
					DataRow[] matchingRows = dtVarientForWages.Select("ItemId=" + itemId);
					if (matchingRows.Length == 0)
					{
						MessageBox.Show("No Variants found for selected Item");
						return;
					}
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(matchingRows.CopyToDataTable(), "ItemAttributeId", "ItemAttribute", r, "AttributeVarientId", "AttributeVarient", bothNameAndId: true);
					int variantId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
					if (variantId > 0)
					{
						DataRow foundRow = dtVarientForWages.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ItemAttributeId") == variantId && row.Field<int>("ItemId") == itemId);
						decimal varientEquivalent = ((foundRow != null) ? Conversion.ToDecimal(foundRow["VarientEquivalent"]) : 0m);
						decimal currRowQty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Qty"]) : 0m);
						decimal itemNetWeight = ((foundRow != null) ? Conversion.ToDecimal(foundRow["NetWeight"]) : 0m);
						r.Cells["Qty"].Value = currRowQty;
						r.Cells["ItemNetWeight"].Value = itemNetWeight;
						r.Cells["VarientEquivalent"].Value = varientEquivalent;
						decimal rate2 = Conversion.ToDecimal(r.Cells["Rate"].Value);
						decimal addLess2 = Conversion.ToDecimal(r.Cells["AddLess"].Value);
						int parentUomId2 = Conversion.ToInt(r.Cells["ParentUomId"].Value);
						decimal amount2 = default(decimal);
						switch (parentUomId2)
						{
						case 2:
							amount2 = rate2 * currRowQty;
							break;
						case 3:
							amount2 = itemNetWeight / 1000m * rate2;
							break;
						}
						r.Cells["Amount"].Value = amount2;
						r.Cells["NetAmount"].Value = amount2 + addLess2;
					}
					else
					{
						r.Cells["Qty"].Value = 0;
						r.Cells["ItemNetWeight"].Value = 0;
						r.Cells["Amount"].Value = 0;
						r.Cells["NetAmount"].Value = 0;
					}
					break;
				}
				case "ServiceActivity":
				case "ContractorWagesRateScheduleId":
				{
					int selectedItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (selectedItemId == 0)
					{
						MessageBox.Show("Please Select an Item First");
						return;
					}
					int selectedContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
					if (selectedContractorId == 0)
					{
						MessageBox.Show("Please Select a Contractor First");
						return;
					}
					DataTable dtAct = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, selectedItemId, Conversion.ToDateTime((object)DocDate.Value), 0, selectedContractorId);
					GrdPopUp obj = new GrdPopUp(dtAct, "ContractorWagesRateScheduleId", "WagesActivity", "ContractorWagesActivityId");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					int scheduleId = Conversion.ToInt((object)obj.ReturnId);
					string ServiceActivity = Conversion.ToString((object)obj.ReturnName);
					int ContractorWagesActivityId = Conversion.ToInt((object)obj.ReturnCode);
					r.Cells["ContractorWagesRateScheduleId"].Value = scheduleId;
					r.Cells["ServiceActivity"].Value = ServiceActivity;
					r.Cells["ContractorWagesActivityId"].Value = ContractorWagesActivityId;
					if (scheduleId > 0)
					{
						DataRow activityRow = dtAct.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ContractorWagesRateScheduleId") == scheduleId);
						decimal rate = ((activityRow != null) ? Conversion.ToDecimal(activityRow["Rate"]) : 0m);
						int parentUomId = ((activityRow != null) ? Conversion.ToInt(activityRow["ParentUOMId"]) : 0);
						r.Cells["Rate"].Value = rate;
						r.Cells["ParentUomId"].Value = parentUomId;
						decimal rowQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
						decimal netWeight = Conversion.ToDecimal(r.Cells["ItemNetWeight"].Value);
						decimal addLess = Conversion.ToDecimal(r.Cells["AddLess"].Value);
						decimal amount = default(decimal);
						switch (parentUomId)
						{
						case 2:
							amount = rate * rowQty;
							break;
						case 3:
							amount = netWeight / 1000m * rate;
							break;
						default:
							r.Cells["ContractorWagesRateScheduleId"].Value = 0;
							r.Cells["ServiceActivity"].Value = "";
							r.Cells["ParentUomId"].Value = 0;
							r.Cells["Rate"].Value = 0;
							r.Cells["Amount"].Value = 0;
							r.Cells["NetAmount"].Value = 0;
							MessageBox.Show("Parent UOM of selected Wages Activity is not defined. Please check.");
							return;
						}
						r.Cells["Amount"].Value = amount;
						r.Cells["NetAmount"].Value = amount + addLess;
					}
					else
					{
						r.Cells["ContractorWagesRateScheduleId"].Value = 0;
						r.Cells["ServiceActivity"].Value = "";
						r.Cells["ParentUomId"].Value = 0;
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
						r.Cells["NetAmount"].Value = 0;
					}
					break;
				}
				}
				grdContractorWages.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdExtraWages_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow currentRow = grdExtraWages.CurrentRow;
			GridEXColumn currentColumn = grdExtraWages.CurrentColumn;
			if (currentRow == null || (int)currentRow.RowType != 0 || currentColumn == null)
			{
				return;
			}
			string columnKey = currentColumn.Key;
			if (e.Control && e.KeyCode == Keys.Space)
			{
				switch (columnKey)
				{
				case "Add":
					AddRowInExtraWagesGrid();
					break;
				case "Delete":
					ExtraWagesDeleteRow(currentRow);
					break;
				case "DeleteAll":
					ExtraWagesDeleteAllButtonClick(currentRow);
					break;
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInExtraWagesGrid();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				ExtraWagesDeleteRow(currentRow);
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			if (columnKey == "InvGdnId" || columnKey == "GdnNo")
			{
				GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow((from row in dtVarientForWages.AsEnumerable()
					group row by row.Field<int>("InvGdnId") into g
					select g.FirstOrDefault()).CopyToDataTable(), "InvGdnId", "GdnNo", currentRow, "InvGdnId", "GdnNo", bothNameAndId: true);
				Conversion.ToInt(currentRow.Cells["InvGdnId"].Value);
			}
			switch (columnKey)
			{
			case "ContractorId":
			case "ContractorName":
				GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtContr, "Id", "CompanyName", currentRow, "ContractorId", "ContractorName", bothNameAndId: true);
				break;
			case "ServiceActivity":
			case "ServiceActivityId":
				GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtExtraWagesCombo, "Id", "ServiceActivity", currentRow, "ServiceActivityId", "ServiceActivity", bothNameAndId: true);
				break;
			}
			grdExtraWages.UpdateData();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingGdns_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow currentRow = grdPendingGdns.CurrentRow;
			GridEXColumn currentColumn = grdPendingGdns.CurrentColumn;
			if (currentRow != null && (int)currentRow.RowType == 0 && currentColumn != null)
			{
				string columnKey = currentColumn.Key;
				if (e.Control && e.KeyCode == Keys.Space && columnKey == "Load")
				{
					PendingGdnLoad(currentRow);
				}
				if (e.Control && e.KeyCode == Keys.L)
				{
					PendingGdnLoad(currentRow);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRecordsUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dtIds = ContractorWagesBill.GetContractorWagesBillIdsForAutoUpdation(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId);
			if (dtIds != null && dtIds.Rows.Count > 0)
			{
				bool ShouldStop = false;
				for (int i = 0; i < dtIds.Rows.Count; i++)
				{
					int RecId = Conversion.ToInt(dtIds.Rows[i]["Id"]);
					ReadById(RecId);
					ShouldStop = Insert(IsAutoUpdate: true, RecId);
					if (!ShouldStop)
					{
						break;
					}
				}
				if (ShouldStop)
				{
					MessageBox.Show("Records Update Successfully");
				}
				btnRecordsUpdate.Visible = false;
				return;
			}
			btnRecordsUpdate.Visible = false;
			throw new Exception("Ids Not Found For Update");
		}
		catch (Exception ex)
		{
			btnRecordsUpdate.Visible = false;
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
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Expected O, but got Unknown
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Expected O, but got Unknown
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Expected O, but got Unknown
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Expected O, but got Unknown
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Expected O, but got Unknown
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Expected O, but got Unknown
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Expected O, but got Unknown
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bb: Expected O, but got Unknown
		//IL_0877: Unknown result type (might be due to invalid IL or missing references)
		//IL_0881: Expected O, but got Unknown
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c3: Expected O, but got Unknown
		//IL_08e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ef: Expected O, but got Unknown
		//IL_0911: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Expected O, but got Unknown
		//IL_0969: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Expected O, but got Unknown
		//IL_0974: Unknown result type (might be due to invalid IL or missing references)
		//IL_097e: Expected O, but got Unknown
		//IL_0fd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe1: Expected O, but got Unknown
		//IL_23f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2403: Expected O, but got Unknown
		//IL_2410: Unknown result type (might be due to invalid IL or missing references)
		//IL_241a: Expected O, but got Unknown
		//IL_3426: Unknown result type (might be due to invalid IL or missing references)
		//IL_3430: Expected O, but got Unknown
		//IL_343d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3447: Expected O, but got Unknown
		//IL_6e38: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e42: Expected O, but got Unknown
		//IL_6e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e70: Expected O, but got Unknown
		GridEXLayout grdPendingGdns_DesignTimeLayout = new GridEXLayout();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		GridEXLayout grdContractorWages_DesignTimeLayout = new GridEXLayout();
		Appearance appearance15 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		GridEXLayout grdExtraWages_DesignTimeLayout = new GridEXLayout();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		GridEXLayout grdGdn_DesignTimeLayout = new GridEXLayout();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdWagesDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdExtraWagesDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGdnDetailHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.pcc.frmGdnWages));
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdPendingGdns = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbDateTypePending = new UltraCombo();
		this.CmbCustomerPending = new UltraCombo();
		this.BtnShowPending = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtToDatePending = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.txtToDocPending = new System.Windows.Forms.TextBox();
		this.txtFromDatePending = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.txtFromDocPending = new System.Windows.Forms.TextBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.BtnNewPending = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshPending = new System.Windows.Forms.ToolStripButton();
		this.label58 = new System.Windows.Forms.Label();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabWages = new System.Windows.Forms.TabPage();
		this.grdContractorWages = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.PanelMessage = new System.Windows.Forms.Panel();
		this.ToolStripMessage = new System.Windows.Forms.ToolStrip();
		this.BtnMessage = new System.Windows.Forms.ToolStripButton();
		this.CmbActivityforContractorUpdateInWagesGrid = new UltraCombo();
		this.CmbContractorForUpdateInWagesGrid = new UltraCombo();
		this.label66 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.BtnUpdateConctratorInWages = new System.Windows.Forms.Button();
		this.BtnGenerateWagesRows = new System.Windows.Forms.Button();
		this.label16 = new System.Windows.Forms.Label();
		this.tabExtraWages = new System.Windows.Forms.TabPage();
		this.grdExtraWages = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.CmbContractorExtraWages = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.CmbActivtyExtraWages = new UltraCombo();
		this.BtnUpdateExtraWages = new System.Windows.Forms.Button();
		this.label32 = new System.Windows.Forms.Label();
		this.tabGdnDetail = new System.Windows.Forms.TabPage();
		this.grdGdn = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtCustomerCellNo = new System.Windows.Forms.TextBox();
		this.txtCustomerCity = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.RdPartyByCode = new System.Windows.Forms.RadioButton();
		this.RdPartyByName = new System.Windows.Forms.RadioButton();
		this.label8 = new System.Windows.Forms.Label();
		this.labelFcyAmt = new System.Windows.Forms.Label();
		this.txtTotalQty = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.labelRemarksH = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.labelExRate = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.cmbCurrency = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.labelFcyCode = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.CmbCustomerName = new UltraCombo();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.ChkPrintslip = new System.Windows.Forms.CheckBox();
		this.comproject = new UltraCombo();
		this.combranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label78 = new System.Windows.Forms.Label();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabWagesDetailH = new System.Windows.Forms.TabPage();
		this.grdWagesDetailHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.tabExtraWagesH = new System.Windows.Forms.TabPage();
		this.grdExtraWagesDetailHistory = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.tabGdnDetailH = new System.Windows.Forms.TabPage();
		this.grdGdnDetailHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label55 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.CmbCustomerHistory = new UltraCombo();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.label76 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label54 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRecordsUpdate = new System.Windows.Forms.Button();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingGdns).BeginInit();
		this.panel8.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypePending).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerPending).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabWages.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).BeginInit();
		this.panel4.SuspendLayout();
		this.PanelMessage.SuspendLayout();
		this.ToolStripMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbActivityforContractorUpdateInWagesGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractorForUpdateInWagesGrid).BeginInit();
		this.tabExtraWages.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdExtraWages).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractorExtraWages).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbActivtyExtraWages).BeginInit();
		this.tabGdnDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGdn).BeginInit();
		this.panel16.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.tabControl3.SuspendLayout();
		this.tabWagesDetailH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWagesDetailHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.tabExtraWagesH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdExtraWagesDetailHistory).BeginInit();
		this.panel5.SuspendLayout();
		this.tabGdnDetailH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGdnDetailHistory).BeginInit();
		this.panel18.SuspendLayout();
		this.panel10.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1084, 801);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1076, 772);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingGdns);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1070, 766);
		this.panel1.TabIndex = 7;
		this.grdPendingGdns.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPendingGdns).BackColor = System.Drawing.Color.White;
		this.grdPendingGdns.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPendingGdns_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingGdns.DesignTimeLayout = grdPendingGdns_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingGdns).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingGdns.FilterMode = (FilterMode)2;
		this.grdPendingGdns.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingGdns).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingGdns.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingGdns).Location = new System.Drawing.Point(0, 590);
		((System.Windows.Forms.Control)(object)this.grdPendingGdns).Name = "grdPendingGdns";
		((ScrollableBase)this.grdPendingGdns).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingGdns).Size = new System.Drawing.Size(1070, 176);
		((System.Windows.Forms.Control)(object)this.grdPendingGdns).TabIndex = 0;
		this.grdPendingGdns.TotalRow = (InheritableBoolean)1;
		this.grdPendingGdns.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingGdns).VisualStyle = (VisualStyle)4;
		this.grdPendingGdns.ColumnButtonClick += new ColumnActionEventHandler(grdPendingGdns_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPendingGdns).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPendingGdns_KeyDown);
		this.panel8.Controls.Add(this.groupBox2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 541);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1070, 49);
		this.panel8.TabIndex = 27;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateTypePending);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerPending);
		this.groupBox2.Controls.Add(this.BtnShowPending);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.txtToDatePending);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.txtToDocPending);
		this.groupBox2.Controls.Add(this.txtFromDatePending);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtFromDocPending);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 1);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1032, 46);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.CmbDateTypePending.AllowNull = (DefaultableBoolean)1;
		this.CmbDateTypePending.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDateTypePending.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDateTypePending.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypePending).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypePending).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateTypePending).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateTypePending).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateTypePending.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateTypePending).Location = new System.Drawing.Point(62, 15);
		((System.Windows.Forms.Control)(object)this.CmbDateTypePending).Name = "CmbDateTypePending";
		((System.Windows.Forms.Control)(object)this.CmbDateTypePending).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateTypePending).TabIndex = 0;
		this.CmbDateTypePending.ValueChanged += new System.EventHandler(CmbDateTypePending_ValueChanged);
		this.CmbCustomerPending.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerPending.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerPending.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerPending.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Appearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerPending).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance109;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerPending).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance8).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerPending).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerPending).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerPending.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerPending).Location = new System.Drawing.Point(762, 15);
		((System.Windows.Forms.Control)(object)this.CmbCustomerPending).Name = "CmbCustomerPending";
		((System.Windows.Forms.Control)(object)this.CmbCustomerPending).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerPending).TabIndex = 5;
		this.BtnShowPending.BackColor = System.Drawing.Color.Teal;
		this.BtnShowPending.FlatAppearance.BorderSize = 0;
		this.BtnShowPending.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowPending.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowPending.ForeColor = System.Drawing.Color.White;
		this.BtnShowPending.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowPending.Location = new System.Drawing.Point(963, 16);
		this.BtnShowPending.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowPending.Name = "BtnShowPending";
		this.BtnShowPending.Size = new System.Drawing.Size(55, 25);
		this.BtnShowPending.TabIndex = 6;
		this.BtnShowPending.Text = "Show";
		this.BtnShowPending.UseVisualStyleBackColor = false;
		this.BtnShowPending.Click += new System.EventHandler(BtnShowPending_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(704, 21);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(58, 15);
		this.label6.TabIndex = 178;
		this.label6.Text = "Customer";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 21);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(60, 15);
		this.label7.TabIndex = 174;
		this.label7.Text = "Date Type";
		this.txtToDatePending.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDatePending.CustomFormat = "dd-MMM-yy";
		this.txtToDatePending.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtToDatePending.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDatePending.Location = new System.Drawing.Point(392, 17);
		this.txtToDatePending.Name = "txtToDatePending";
		this.txtToDatePending.Size = new System.Drawing.Size(125, 22);
		this.txtToDatePending.TabIndex = 2;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(185, 21);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(79, 15);
		this.label9.TabIndex = 171;
		this.label9.Text = "Date From,To";
		this.txtToDocPending.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocPending.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocPending.Location = new System.Drawing.Point(640, 17);
		this.txtToDocPending.Name = "txtToDocPending";
		this.txtToDocPending.Size = new System.Drawing.Size(60, 23);
		this.txtToDocPending.TabIndex = 4;
		this.txtToDocPending.Tag = "int";
		this.txtFromDatePending.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDatePending.CustomFormat = "dd-MMM-yy";
		this.txtFromDatePending.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtFromDatePending.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDatePending.Location = new System.Drawing.Point(265, 17);
		this.txtFromDatePending.Name = "txtFromDatePending";
		this.txtFromDatePending.Size = new System.Drawing.Size(125, 22);
		this.txtFromDatePending.TabIndex = 1;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(519, 21);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 173;
		this.label11.Text = "Doc From";
		this.txtFromDocPending.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromDocPending.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocPending.Location = new System.Drawing.Point(579, 17);
		this.txtFromDocPending.Name = "txtFromDocPending";
		this.txtFromDocPending.Size = new System.Drawing.Size(60, 23);
		this.txtFromDocPending.TabIndex = 3;
		this.txtFromDocPending.Tag = "int";
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.toolStrip3);
		this.panel6.Controls.Add(this.label58);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 492);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1070, 49);
		this.panel6.TabIndex = 16;
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewPending, this.BtnRefreshPending });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1070, 25);
		this.toolStrip3.TabIndex = 127;
		this.toolStrip3.Text = "toolStrip3";
		this.BtnNewPending.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewPending.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewPending.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewPending.Name = "BtnNewPending";
		this.BtnNewPending.Size = new System.Drawing.Size(55, 22);
		this.BtnNewPending.Text = "&New";
		this.BtnNewPending.Click += new System.EventHandler(BtnNewPending_Click);
		this.BtnRefreshPending.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshPending.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshPending.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshPending.Name = "BtnRefreshPending";
		this.BtnRefreshPending.Size = new System.Drawing.Size(76, 22);
		this.BtnRefreshPending.Text = "Refresh";
		this.BtnRefreshPending.Click += new System.EventHandler(BtnRefreshPending_Click);
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(4, 29);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(255, 16);
		this.label58.TabIndex = 126;
		this.label58.Text = "Pending GDN (Not Referred In Invoice)";
		this.tabControl2.Controls.Add(this.tabWages);
		this.tabControl2.Controls.Add(this.tabExtraWages);
		this.tabControl2.Controls.Add(this.tabGdnDetail);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Location = new System.Drawing.Point(0, 152);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1070, 340);
		this.tabControl2.TabIndex = 1;
		this.tabWages.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractorWages);
		this.tabWages.Controls.Add(this.panel4);
		this.tabWages.Location = new System.Drawing.Point(4, 25);
		this.tabWages.Name = "tabWages";
		this.tabWages.Size = new System.Drawing.Size(1062, 311);
		this.tabWages.TabIndex = 1;
		this.tabWages.Text = "Contractor Wages";
		this.tabWages.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdContractorWages).BackColor = System.Drawing.Color.White;
		grdContractorWages_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdContractorWages.DesignTimeLayout = grdContractorWages_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractorWages.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Name = "grdContractorWages";
		((ScrollableBase)this.grdContractorWages).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Size = new System.Drawing.Size(1062, 284);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).TabIndex = 0;
		this.grdContractorWages.TotalRow = (InheritableBoolean)1;
		this.grdContractorWages.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractorWages).VisualStyle = (VisualStyle)4;
		this.grdContractorWages.CellUpdated += new ColumnActionEventHandler(grdContractorWages_CellUpdated);
		this.grdContractorWages.ColumnButtonClick += new ColumnActionEventHandler(grdContractorWages_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractorWages_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar4);
		this.panel4.Controls.Add(this.PanelMessage);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid);
		this.panel4.Controls.Add(this.label66);
		this.panel4.Controls.Add(this.label65);
		this.panel4.Controls.Add(this.BtnUpdateConctratorInWages);
		this.panel4.Controls.Add(this.BtnGenerateWagesRows);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1062, 27);
		this.panel4.TabIndex = 0;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1014, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar4.MyGrid = this.grdContractorWages;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(47, 24);
		this.ctrlGrdBar4.TabIndex = 162;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.PanelMessage.BackColor = System.Drawing.Color.White;
		this.PanelMessage.Controls.Add(this.ToolStripMessage);
		this.PanelMessage.Location = new System.Drawing.Point(480, 2);
		this.PanelMessage.Name = "PanelMessage";
		this.PanelMessage.Size = new System.Drawing.Size(25, 23);
		this.PanelMessage.TabIndex = 161;
		this.ToolStripMessage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.ToolStripMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnMessage });
		this.ToolStripMessage.Location = new System.Drawing.Point(0, 0);
		this.ToolStripMessage.Name = "ToolStripMessage";
		this.ToolStripMessage.Size = new System.Drawing.Size(25, 25);
		this.ToolStripMessage.TabIndex = 0;
		this.ToolStripMessage.Text = "toolStrip3";
		this.BtnMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.BtnMessage.Image = Architecture.WinApp.Properties.Resources.info_i;
		this.BtnMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnMessage.Name = "BtnMessage";
		this.BtnMessage.Size = new System.Drawing.Size(23, 22);
		this.BtnMessage.ToolTipText = "Click to update the selected contractor for the selected activity or the same contractor (as in the 1st row) when both combos are empty, in all rows of grid";
		this.CmbActivityforContractorUpdateInWagesGrid.AllowNull = (DefaultableBoolean)1;
		this.CmbActivityforContractorUpdateInWagesGrid.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbActivityforContractorUpdateInWagesGrid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbActivityforContractorUpdateInWagesGrid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbActivityforContractorUpdateInWagesGrid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Location = new System.Drawing.Point(568, 1);
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Name = "CmbActivityforContractorUpdateInWagesGrid";
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).TabIndex = 1;
		this.CmbContractorForUpdateInWagesGrid.AllowNull = (DefaultableBoolean)1;
		this.CmbContractorForUpdateInWagesGrid.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractorForUpdateInWagesGrid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractorForUpdateInWagesGrid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance103;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractorForUpdateInWagesGrid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Location = new System.Drawing.Point(774, 1);
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Name = "CmbContractorForUpdateInWagesGrid";
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).TabIndex = 2;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label66.ForeColor = System.Drawing.Color.White;
		this.label66.Location = new System.Drawing.Point(698, 5);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(81, 19);
		this.label66.TabIndex = 159;
		this.label66.Text = "Contractor";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label65.ForeColor = System.Drawing.Color.White;
		this.label65.Location = new System.Drawing.Point(507, 5);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(60, 19);
		this.label65.TabIndex = 160;
		this.label65.Text = "Activity";
		this.BtnUpdateConctratorInWages.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateConctratorInWages.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateConctratorInWages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateConctratorInWages.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateConctratorInWages.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateConctratorInWages.Location = new System.Drawing.Point(941, 1);
		this.BtnUpdateConctratorInWages.Name = "BtnUpdateConctratorInWages";
		this.BtnUpdateConctratorInWages.Size = new System.Drawing.Size(70, 26);
		this.BtnUpdateConctratorInWages.TabIndex = 3;
		this.BtnUpdateConctratorInWages.Text = "Update";
		this.BtnUpdateConctratorInWages.UseVisualStyleBackColor = false;
		this.BtnUpdateConctratorInWages.Click += new System.EventHandler(BtnUpdateConctratorInWages_Click);
		this.BtnGenerateWagesRows.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnGenerateWagesRows.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnGenerateWagesRows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnGenerateWagesRows.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.BtnGenerateWagesRows.ForeColor = System.Drawing.Color.White;
		this.BtnGenerateWagesRows.Location = new System.Drawing.Point(100, 1);
		this.BtnGenerateWagesRows.Name = "BtnGenerateWagesRows";
		this.BtnGenerateWagesRows.Size = new System.Drawing.Size(98, 26);
		this.BtnGenerateWagesRows.TabIndex = 0;
		this.BtnGenerateWagesRows.Text = "Generate";
		this.BtnGenerateWagesRows.UseVisualStyleBackColor = false;
		this.BtnGenerateWagesRows.Click += new System.EventHandler(BtnGenerateWagesRows_Click);
		this.label16.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(4, 1);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(96, 27);
		this.label16.TabIndex = 126;
		this.label16.Text = "Click To Auto Generate Rows";
		this.tabExtraWages.Controls.Add((System.Windows.Forms.Control)(object)this.grdExtraWages);
		this.tabExtraWages.Controls.Add(this.panel14);
		this.tabExtraWages.Location = new System.Drawing.Point(4, 25);
		this.tabExtraWages.Name = "tabExtraWages";
		this.tabExtraWages.Size = new System.Drawing.Size(1062, 311);
		this.tabExtraWages.TabIndex = 2;
		this.tabExtraWages.Text = "Extra Wages";
		this.tabExtraWages.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdExtraWages).BackColor = System.Drawing.Color.White;
		grdExtraWages_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdExtraWages.DesignTimeLayout = grdExtraWages_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdExtraWages).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdExtraWages).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdExtraWages.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdExtraWages).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.grdExtraWages).Name = "grdExtraWages";
		((ScrollableBase)this.grdExtraWages).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdExtraWages).Size = new System.Drawing.Size(1062, 282);
		((System.Windows.Forms.Control)(object)this.grdExtraWages).TabIndex = 5;
		this.grdExtraWages.TotalRow = (InheritableBoolean)1;
		this.grdExtraWages.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdExtraWages).VisualStyle = (VisualStyle)4;
		this.grdExtraWages.CellUpdated += new ColumnActionEventHandler(grdExtraWages_CellUpdated);
		this.grdExtraWages.ColumnButtonClick += new ColumnActionEventHandler(grdExtraWages_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdExtraWages).KeyDown += new System.Windows.Forms.KeyEventHandler(grdExtraWages_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractorExtraWages);
		this.panel14.Controls.Add(this.label2);
		this.panel14.Controls.Add(this.label15);
		this.panel14.Controls.Add(this.label31);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbActivtyExtraWages);
		this.panel14.Controls.Add(this.BtnUpdateExtraWages);
		this.panel14.Controls.Add(this.label32);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1062, 29);
		this.panel14.TabIndex = 4;
		this.CmbContractorExtraWages.AllowNull = (DefaultableBoolean)1;
		this.CmbContractorExtraWages.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractorExtraWages.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractorExtraWages.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance77).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractorExtraWages).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractorExtraWages).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractorExtraWages.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractorExtraWages).Location = new System.Drawing.Point(836, 1);
		((System.Windows.Forms.Control)(object)this.CmbContractorExtraWages).Name = "CmbContractorExtraWages";
		((System.Windows.Forms.Control)(object)this.CmbContractorExtraWages).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractorExtraWages).TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(757, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(81, 19);
		this.label2.TabIndex = 159;
		this.label2.Text = "Contractor";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(574, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(60, 19);
		this.label15.TabIndex = 160;
		this.label15.Text = "Activity";
		this.label31.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label31.ForeColor = System.Drawing.Color.White;
		this.label31.Location = new System.Drawing.Point(129, 0);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(446, 28);
		this.label31.TabIndex = 158;
		this.label31.Text = "Click to update the selected contractor for the selected activity or the same contractor (as in the 1st row) when both combos are empty, in all rows of grid";
		this.CmbActivtyExtraWages.AllowNull = (DefaultableBoolean)1;
		this.CmbActivtyExtraWages.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbActivtyExtraWages.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbActivtyExtraWages.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance88).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbActivtyExtraWages).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbActivtyExtraWages).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbActivtyExtraWages.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbActivtyExtraWages).Location = new System.Drawing.Point(635, 1);
		((System.Windows.Forms.Control)(object)this.CmbActivtyExtraWages).Name = "CmbActivtyExtraWages";
		((System.Windows.Forms.Control)(object)this.CmbActivtyExtraWages).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbActivtyExtraWages).TabIndex = 1;
		this.BtnUpdateExtraWages.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateExtraWages.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateExtraWages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateExtraWages.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateExtraWages.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateExtraWages.Location = new System.Drawing.Point(990, 1);
		this.BtnUpdateExtraWages.Name = "BtnUpdateExtraWages";
		this.BtnUpdateExtraWages.Size = new System.Drawing.Size(65, 26);
		this.BtnUpdateExtraWages.TabIndex = 3;
		this.BtnUpdateExtraWages.Text = "Update";
		this.BtnUpdateExtraWages.UseVisualStyleBackColor = false;
		this.BtnUpdateExtraWages.Click += new System.EventHandler(BtnUpdateExtraWages_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(2, 6);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(95, 17);
		this.label32.TabIndex = 129;
		this.label32.Text = "Extra Wages";
		this.tabGdnDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grdGdn);
		this.tabGdnDetail.Controls.Add(this.panel16);
		this.tabGdnDetail.Location = new System.Drawing.Point(4, 25);
		this.tabGdnDetail.Name = "tabGdnDetail";
		this.tabGdnDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabGdnDetail.Size = new System.Drawing.Size(1062, 311);
		this.tabGdnDetail.TabIndex = 0;
		this.tabGdnDetail.Text = "Gdn Detail";
		this.tabGdnDetail.UseVisualStyleBackColor = true;
		this.grdGdn.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGdn).BackColor = System.Drawing.Color.White;
		grdGdn_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGdn.DesignTimeLayout = grdGdn_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGdn).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGdn).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGdn.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGdn).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grdGdn).Name = "grdGdn";
		((ScrollableBase)this.grdGdn).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGdn).Size = new System.Drawing.Size(1056, 284);
		((System.Windows.Forms.Control)(object)this.grdGdn).TabIndex = 0;
		this.grdGdn.TotalRow = (InheritableBoolean)1;
		this.grdGdn.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGdn).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdGdn).Click += new System.EventHandler(grd_Click);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label23);
		this.panel16.Controls.Add(this.ctrlGrdBar1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1056, 21);
		this.panel16.TabIndex = 33;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 17);
		this.label23.TabIndex = 35;
		this.label23.Text = "Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1016, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdGdn;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1070, 127);
		this.panel3.TabIndex = 0;
		this.groupBox1.Controls.Add(this.txtCustomerCellNo);
		this.groupBox1.Controls.Add(this.txtCustomerCity);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label62);
		this.groupBox1.Controls.Add(this.txtTotalAmount);
		this.groupBox1.Controls.Add(this.RdPartyByCode);
		this.groupBox1.Controls.Add(this.RdPartyByName);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.labelFcyAmt);
		this.groupBox1.Controls.Add(this.txtTotalQty);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.labelRemarksH);
		this.groupBox1.Controls.Add(this.txtFcyAmount);
		this.groupBox1.Controls.Add(this.labelExRate);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.labelFcyCode);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerName);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 29);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1034, 97);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtCustomerCellNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCustomerCellNo.Enabled = false;
		this.txtCustomerCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerCellNo.Location = new System.Drawing.Point(424, 43);
		this.txtCustomerCellNo.Name = "txtCustomerCellNo";
		this.txtCustomerCellNo.Size = new System.Drawing.Size(200, 23);
		this.txtCustomerCellNo.TabIndex = 5;
		this.txtCustomerCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCustomerCity.Enabled = false;
		this.txtCustomerCity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerCity.Location = new System.Drawing.Point(424, 15);
		this.txtCustomerCity.Name = "txtCustomerCity";
		this.txtCustomerCity.Size = new System.Drawing.Size(200, 23);
		this.txtCustomerCity.TabIndex = 4;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(345, 47);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(80, 15);
		this.label4.TabIndex = 136;
		this.label4.Text = "Customer Cell";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(345, 19);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(81, 15);
		this.label62.TabIndex = 137;
		this.label62.Text = "Customer City";
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Enabled = false;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(707, 43);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.Size = new System.Drawing.Size(122, 23);
		this.txtTotalAmount.TabIndex = 7;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.RdPartyByCode.AutoSize = true;
		this.RdPartyByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdPartyByCode.Location = new System.Drawing.Point(61, 53);
		this.RdPartyByCode.Margin = new System.Windows.Forms.Padding(2);
		this.RdPartyByCode.Name = "RdPartyByCode";
		this.RdPartyByCode.Size = new System.Drawing.Size(52, 19);
		this.RdPartyByCode.TabIndex = 133;
		this.RdPartyByCode.Text = "Code";
		this.RdPartyByCode.UseVisualStyleBackColor = true;
		this.RdPartyByName.AutoSize = true;
		this.RdPartyByName.Checked = true;
		this.RdPartyByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdPartyByName.Location = new System.Drawing.Point(61, 37);
		this.RdPartyByName.Margin = new System.Windows.Forms.Padding(2);
		this.RdPartyByName.Name = "RdPartyByName";
		this.RdPartyByName.Size = new System.Drawing.Size(57, 19);
		this.RdPartyByName.TabIndex = 132;
		this.RdPartyByName.TabStop = true;
		this.RdPartyByName.Text = "Name";
		this.RdPartyByName.UseVisualStyleBackColor = true;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(628, 47);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(80, 15);
		this.label8.TabIndex = 37;
		this.label8.Text = "Total Amount";
		this.labelFcyAmt.AutoSize = true;
		this.labelFcyAmt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.labelFcyAmt.Location = new System.Drawing.Point(833, 73);
		this.labelFcyAmt.Name = "labelFcyAmt";
		this.labelFcyAmt.Size = new System.Drawing.Size(72, 15);
		this.labelFcyAmt.TabIndex = 43;
		this.labelFcyAmt.Text = "Fcy Amount";
		this.txtTotalQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalQty.Enabled = false;
		this.txtTotalQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalQty.Location = new System.Drawing.Point(707, 15);
		this.txtTotalQty.Name = "txtTotalQty";
		this.txtTotalQty.Size = new System.Drawing.Size(122, 23);
		this.txtTotalQty.TabIndex = 6;
		this.txtTotalQty.TabStop = false;
		this.txtTotalQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(912, 43);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(116, 23);
		this.txtExchangeRate.TabIndex = 9;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.labelRemarksH.AutoSize = true;
		this.labelRemarksH.BackColor = System.Drawing.Color.White;
		this.labelRemarksH.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.labelRemarksH.Location = new System.Drawing.Point(2, 70);
		this.labelRemarksH.Name = "labelRemarksH";
		this.labelRemarksH.Size = new System.Drawing.Size(67, 20);
		this.labelRemarksH.TabIndex = 8;
		this.labelRemarksH.Text = "Remarks";
		this.labelRemarksH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmount.Location = new System.Drawing.Point(912, 69);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(116, 23);
		this.txtFcyAmount.TabIndex = 10;
		this.txtFcyAmount.TabStop = false;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.labelExRate.AutoSize = true;
		this.labelExRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.labelExRate.Location = new System.Drawing.Point(833, 47);
		this.labelExRate.Name = "labelExRate";
		this.labelExRate.Size = new System.Drawing.Size(78, 15);
		this.labelExRate.TabIndex = 42;
		this.labelExRate.Text = "Echange Rate";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(217, 15);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(125, 22);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(912, 13);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(116, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(122, 69);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(707, 23);
		this.txtremarks.TabIndex = 3;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(628, 19);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(55, 15);
		this.label43.TabIndex = 39;
		this.label43.Text = "Total Qty";
		this.labelFcyCode.AutoSize = true;
		this.labelFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelFcyCode.Location = new System.Drawing.Point(833, 19);
		this.labelFcyCode.Name = "labelFcyCode";
		this.labelFcyCode.Size = new System.Drawing.Size(55, 15);
		this.labelFcyCode.TabIndex = 36;
		this.labelFcyCode.Text = "Fcy Code";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(122, 15);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(94, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(2, 19);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(88, 15);
		this.label33.TabIndex = 3;
		this.label33.Text = "Doc No && Date";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(2, 47);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(58, 15);
		this.label35.TabIndex = 0;
		this.label35.Text = "Customer";
		this.CmbCustomerName.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Location = new System.Drawing.Point(122, 41);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Name = "CmbCustomerName";
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Leave += new System.EventHandler(CmbCustomerName_Leave);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.btnRecordsUpdate);
		this.panel11.Controls.Add(this.label17);
		this.panel11.Controls.Add(this.ChkPrintslip);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1070, 26);
		this.panel11.TabIndex = 110;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 4);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(104, 19);
		this.label17.TabIndex = 34;
		this.label17.Text = "GDN Wages";
		this.ChkPrintslip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintslip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintslip.Location = new System.Drawing.Point(980, 4);
		this.ChkPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintslip.Name = "ChkPrintslip";
		this.ChkPrintslip.Size = new System.Drawing.Size(88, 20);
		this.ChkPrintslip.TabIndex = 1;
		this.ChkPrintslip.Text = "Print Slip";
		this.ChkPrintslip.UseVisualStyleBackColor = true;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance136;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance142).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance23).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1392, 41);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance14;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1392, 73);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(1302, 79);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 86;
		this.label13.Text = "Branch Name";
		this.label13.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1302, 47);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(79, 15);
		this.label18.TabIndex = 87;
		this.label18.Text = "Project Name";
		this.label18.Visible = false;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnsave, this.btnDelete, this.btnUpdate, this.toolStripSeparator3, this.btnAttachment, this.toolStripSeparator4, this.btnSlip,
			this.toolStripSeparator5, this.btnshortcutkeys, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1070, 25);
		this.toolStrip1.TabIndex = 26;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(60, 22);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(BtnSave_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 22);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.btnSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(96, 22);
		this.btnSlip.Text = "1869-Print";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(122, 22);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.tabControl3);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1076, 772);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 139);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1070, 323);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label78);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 112);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1070, 27);
		this.panel7.TabIndex = 48;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1018, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(49, 25);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.ForeColor = System.Drawing.Color.White;
		this.label78.Location = new System.Drawing.Point(5, 3);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(142, 19);
		this.label78.TabIndex = 35;
		this.label78.Text = "Filtered Records";
		this.tabControl3.Controls.Add(this.tabWagesDetailH);
		this.tabControl3.Controls.Add(this.tabExtraWagesH);
		this.tabControl3.Controls.Add(this.tabGdnDetailH);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl3.Location = new System.Drawing.Point(3, 462);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1070, 307);
		this.tabControl3.TabIndex = 50;
		this.tabWagesDetailH.Controls.Add((System.Windows.Forms.Control)(object)this.grdWagesDetailHistory);
		this.tabWagesDetailH.Controls.Add(this.panel12);
		this.tabWagesDetailH.Location = new System.Drawing.Point(4, 25);
		this.tabWagesDetailH.Name = "tabWagesDetailH";
		this.tabWagesDetailH.Padding = new System.Windows.Forms.Padding(3);
		this.tabWagesDetailH.Size = new System.Drawing.Size(1062, 278);
		this.tabWagesDetailH.TabIndex = 0;
		this.tabWagesDetailH.Text = "Wages Detail";
		this.tabWagesDetailH.UseVisualStyleBackColor = true;
		this.grdWagesDetailHistory.AllowEdit = (InheritableBoolean)2;
		grdWagesDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdWagesDetailHistory.DesignTimeLayout = grdWagesDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdWagesDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdWagesDetailHistory.DynamicFiltering = true;
		this.grdWagesDetailHistory.FilterMode = (FilterMode)2;
		this.grdWagesDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdWagesDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWagesDetailHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdWagesDetailHistory).Location = new System.Drawing.Point(3, 30);
		((System.Windows.Forms.Control)(object)this.grdWagesDetailHistory).Name = "grdWagesDetailHistory";
		((ScrollableBase)this.grdWagesDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWagesDetailHistory).Size = new System.Drawing.Size(1056, 245);
		((System.Windows.Forms.Control)(object)this.grdWagesDetailHistory).TabIndex = 0;
		this.grdWagesDetailHistory.TotalRow = (InheritableBoolean)1;
		this.grdWagesDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWagesDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label1);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1056, 27);
		this.panel12.TabIndex = 49;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(347, 17);
		this.label1.TabIndex = 35;
		this.label1.Text = "Wages Detail Information of Above Selected Row";
		this.tabExtraWagesH.Controls.Add((System.Windows.Forms.Control)(object)this.grdExtraWagesDetailHistory);
		this.tabExtraWagesH.Controls.Add(this.panel5);
		this.tabExtraWagesH.Location = new System.Drawing.Point(4, 25);
		this.tabExtraWagesH.Name = "tabExtraWagesH";
		this.tabExtraWagesH.Size = new System.Drawing.Size(1062, 278);
		this.tabExtraWagesH.TabIndex = 2;
		this.tabExtraWagesH.Text = "Extra Wages Detail";
		this.tabExtraWagesH.UseVisualStyleBackColor = true;
		this.grdExtraWagesDetailHistory.AllowEdit = (InheritableBoolean)2;
		grdExtraWagesDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdExtraWagesDetailHistory.DesignTimeLayout = grdExtraWagesDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdExtraWagesDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdExtraWagesDetailHistory.DynamicFiltering = true;
		this.grdExtraWagesDetailHistory.FilterMode = (FilterMode)2;
		this.grdExtraWagesDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdExtraWagesDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdExtraWagesDetailHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdExtraWagesDetailHistory).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdExtraWagesDetailHistory).Name = "grdExtraWagesDetailHistory";
		((ScrollableBase)this.grdExtraWagesDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdExtraWagesDetailHistory).Size = new System.Drawing.Size(1062, 251);
		((System.Windows.Forms.Control)(object)this.grdExtraWagesDetailHistory).TabIndex = 50;
		this.grdExtraWagesDetailHistory.TotalRow = (InheritableBoolean)1;
		this.grdExtraWagesDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdExtraWagesDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label5);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1062, 27);
		this.panel5.TabIndex = 51;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(388, 17);
		this.label5.TabIndex = 35;
		this.label5.Text = "Extra Wages Detail Information of Above Selected Row";
		this.tabGdnDetailH.Controls.Add((System.Windows.Forms.Control)(object)this.grdGdnDetailHistory);
		this.tabGdnDetailH.Controls.Add(this.panel18);
		this.tabGdnDetailH.Location = new System.Drawing.Point(4, 25);
		this.tabGdnDetailH.Name = "tabGdnDetailH";
		this.tabGdnDetailH.Padding = new System.Windows.Forms.Padding(3);
		this.tabGdnDetailH.Size = new System.Drawing.Size(1062, 278);
		this.tabGdnDetailH.TabIndex = 1;
		this.tabGdnDetailH.Text = "Gdn Detail";
		this.tabGdnDetailH.UseVisualStyleBackColor = true;
		this.grdGdnDetailHistory.AllowEdit = (InheritableBoolean)2;
		grdGdnDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGdnDetailHistory.DesignTimeLayout = grdGdnDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGdnDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGdnDetailHistory.DynamicFiltering = true;
		this.grdGdnDetailHistory.FilterMode = (FilterMode)2;
		this.grdGdnDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGdnDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGdnDetailHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdGdnDetailHistory).Location = new System.Drawing.Point(3, 30);
		((System.Windows.Forms.Control)(object)this.grdGdnDetailHistory).Name = "grdGdnDetailHistory";
		((ScrollableBase)this.grdGdnDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGdnDetailHistory).Size = new System.Drawing.Size(1056, 245);
		((System.Windows.Forms.Control)(object)this.grdGdnDetailHistory).TabIndex = 0;
		this.grdGdnDetailHistory.TotalRow = (InheritableBoolean)1;
		this.grdGdnDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGdnDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label55);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 3);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1056, 27);
		this.panel18.TabIndex = 48;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(5, 3);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(347, 17);
		this.label55.TabIndex = 35;
		this.label55.Text = "Wages Detail Information of Above Selected Row";
		this.panel10.Controls.Add(this.rdmodifydate);
		this.panel10.Controls.Add(this.drdocdate);
		this.panel10.Controls.Add(this.rdentrydate);
		this.panel10.Controls.Add(this.groupBox4);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 57);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1070, 55);
		this.panel10.TabIndex = 0;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(987, 35);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 3;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(987, 3);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 1;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(987, 19);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 2;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox4.Controls.Add(this.txtFromNoHistory);
		this.groupBox4.Controls.Add(this.txtToDocNoHistory);
		this.groupBox4.Controls.Add(this.label3);
		this.groupBox4.Controls.Add(this.label75);
		this.groupBox4.Controls.Add(this.btnshow);
		this.groupBox4.Controls.Add(this.label76);
		this.groupBox4.Controls.Add(this.label73);
		this.groupBox4.Controls.Add(this.txtToDateHistory);
		this.groupBox4.Controls.Add(this.txtFromdateHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(2, 1);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(982, 50);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance64;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance141).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(745, 16);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance96;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance115).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(64, 16);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(576, 18);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(55, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.txtFromNoHistory.Tag = "int";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(632, 18);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(55, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtToDocNoHistory.Tag = "int";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(501, 22);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(76, 15);
		this.label3.TabIndex = 173;
		this.label3.Text = "Doc From,To";
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(3, 22);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(60, 15);
		this.label75.TabIndex = 174;
		this.label75.Text = "Date Type";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(926, 17);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(54, 25);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(176, 22);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(79, 15);
		this.label76.TabIndex = 171;
		this.label76.Text = "Date From,To";
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(689, 22);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(58, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Customer";
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(378, 18);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(120, 22);
		this.txtToDateHistory.TabIndex = 2;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(257, 18);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(120, 22);
		this.txtFromdateHistory.TabIndex = 1;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label54);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1070, 27);
		this.panel9.TabIndex = 47;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(5, 3);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(172, 19);
		this.label54.TabIndex = 35;
		this.label54.Text = "GDN Wages History ";
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1070, 27);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1070, 25);
		this.toolStrip2.TabIndex = 27;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(btnLoadAll_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.btnRecordsUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnRecordsUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRecordsUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnRecordsUpdate.ForeColor = System.Drawing.Color.White;
		this.btnRecordsUpdate.Location = new System.Drawing.Point(111, 3);
		this.btnRecordsUpdate.Name = "btnRecordsUpdate";
		this.btnRecordsUpdate.Size = new System.Drawing.Size(75, 22);
		this.btnRecordsUpdate.TabIndex = 45;
		this.btnRecordsUpdate.Text = "Records Update";
		this.btnRecordsUpdate.UseVisualStyleBackColor = false;
		this.btnRecordsUpdate.Visible = false;
		this.btnRecordsUpdate.Click += new System.EventHandler(btnRecordsUpdate_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1084, 801);
		base.Controls.Add(this.tabControl1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmGdnWages";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmGdnWages_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmGdnWages_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingGdns).EndInit();
		this.panel8.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypePending).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerPending).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabWages.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.PanelMessage.ResumeLayout(false);
		this.PanelMessage.PerformLayout();
		this.ToolStripMessage.ResumeLayout(false);
		this.ToolStripMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbActivityforContractorUpdateInWagesGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractorForUpdateInWagesGrid).EndInit();
		this.tabExtraWages.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdExtraWages).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractorExtraWages).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbActivtyExtraWages).EndInit();
		this.tabGdnDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGdn).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.tabControl3.ResumeLayout(false);
		this.tabWagesDetailH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdWagesDetailHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.tabExtraWagesH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdExtraWagesDetailHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.tabGdnDetailH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGdnDetailHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
