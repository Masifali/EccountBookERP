using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
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
namespace Architecture.WinApp.Sale
{

public class DeliveryOrder : Form
{
	private Attachment AT = new Attachment();

	public UserAccount UserAccount = new UserAccount();

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private List<InvDeliveryOrderdetail> lstRemoveRecord = new List<InvDeliveryOrderdetail>();

	private frmLoadSaleOrder LoadSaleOrder;

	private frmDeliveryScheduleCustomer LoadCustomerSchedule;

	private DataTable table = new DataTable();

	private DataTable dtExpense = new DataTable();

	private DataTable dtInfoGrid = new DataTable();

	private DataTable dthistoryfill = new DataTable();

	private DataTable BranchList = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtRefParty = new DataTable();

	private DataTable dtWh = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtJobLoat = new DataTable();

	private DataTable dtPackingType = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private int updateDetailIndex = -1;

	public int RecId;

	private bool IsApproved;

	public string dtSaleOrderIds = "";

	public string dtSaleOrderMainIds = "";

	public string DeliveryScheduleCustomerDetailIds = "";

	private bool FormInitialized;

	private int DocumentTypeId = 84;

	private string ScreenName = "DeliveryOrder";

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool WarningMessageOnDOForPackingWeight;

	private bool ShowInfoGrid;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool IsCustomerPortal;

	private bool IsStockReservedPerParty;

	private bool ScheduleExist;

	private bool ItemSearchWithNameOrCode;

	private bool WarningMsgForPaymentTermOtherThanCreditOnDO;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

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

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private Label label5;

	private GroupBox groupBox3;

	private Button btnplus;

	private TextBox txtremarksdetail;

	private Label label47;

	private TextBox txtweight;

	private TextBox txtqty;

	private Label label35;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	private Panel panel7;

	private Panel panel8;

	internal UltraCombo CmbItemName;

	internal UltraCombo CmbPackUom;

	internal UltraCombo CmbPackingType;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	private Panel panel12;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	internal UltraCombo cmbBranchTo;

	internal UltraCombo cmbBranchFrom;

	internal UltraCombo CmbSupplierCustomer;

	private Label label2;

	private TextBox txtremarks;

	internal UltraCombo CmbWareHouse;

	private Label label3;

	private Label label4;

	internal UltraCombo CmbDeliveryOrderType;

	internal UltraCombo CmbJobLot;

	private Label label8;

	private ToolStripButton btnLoadSaleOrder;

	private CheckBox ChkPrint;

	private TextBox txtGrossWeight;

	private Label label6;

	private TextBox txtPackWeight;

	private Label label1;

	private ToolStripButton btnDefineReferenceParties;

	internal UltraCombo CmbCropYear;

	private Label label7;

	internal UltraCombo CmbOrderNo;

	private Label label9;

	internal GridEX grdLoader;

	private TextBox txtBalWeight;

	private Label label11;

	private TextBox txtBalQty;

	private Label label10;

	private TextBox txtSubNoId;

	private TextBox txtDocNoId;

	private TextBox txtRefDocTypeId;

	private Label lblBalance;

	private TextBox txtVehicleNo;

	private Label label12;

	private ToolStripButton btnSaveAs;

	internal UltraCombo CmbVehicleType;

	private Label label14;

	private ToolStripButton btnDelete;

	private GroupBox groupBox1;

	private ToolStripButton btnLoaderView2;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	internal GridEX grd;

	private Panel panel5;

	private Label label15;

	private Label label16;

	internal GridEX GrdHistoryDetail;

	private TabControl DetailGridPanel;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private Panel panel10;

	private Label label17;

	internal GridEX GridExpense;

	private Panel LoaderInformationPanel;

	private Panel panel9;

	private Panel panel6;

	private Label label27;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label19;

	private ToolStripButton btnRefreshHistory;

	private Panel panel13;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton BtnShortCutkeys;

	internal UltraCombo CmbSaleType;

	private TextBox txtPackUnit;

	private Label label20;

	private ToolStripButton BtnLoadCustomerSchedule;

	private RadioButton RadNoneFormToOpenOnInsert;

	private LinkLabel labelGpOutwardForm;

	private RadioButton RadGpOutwardToOpenOnInsert;

	private CheckBox ChkIsStockReservedPerParty;

	public DeliveryOrder(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		List<UserAccountAllocation> userAccountAllocationsList = user.UserAccountAllocationsList;
		if (userAccountAllocationsList != null && userAccountAllocationsList.Count > 0)
		{
			ScreenName = user.UserAccountAllocationsList[0].ScreenName;
		}
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 800);
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtItem.Columns.Add("SaleOrderDetailId", typeof(int));
			dtItem.Columns.Add("Rate");
			dtItem.Columns.Add("RateUom");
			dtItem.Columns.Add("DocumentTypeId", typeof(int));
			dtCrop.Columns.Add("Id", typeof(int));
			dtCrop.Columns.Add("CropYear");
			if (dtWh.Columns.Count == 0)
			{
				dtWh.Columns.Add("Id");
				dtWh.Columns.Add("WareHouseName");
			}
			if (dtPackingType.Columns.Count == 0)
			{
				dtPackingType.Columns.Add("Id");
				dtPackingType.Columns.Add("PackTypeDesc");
			}
			if (dtJobLoat.Columns.Count == 0)
			{
				dtJobLoat.Columns.Add("Id");
				dtJobLoat.Columns.Add("JobLotDescription");
			}
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("RefDocumentTypeId", typeof(int));
			table.Columns.Add("RefDocIdNo", typeof(int));
			table.Columns.Add("RefDocSubIdNo", typeof(int));
			table.Columns.Add("SupplierCustomerId", typeof(int));
			table.Columns.Add("SupplierCustomer");
			table.Columns.Add("RefPartyId", typeof(int));
			table.Columns.Add("DeliveryScheduleCustomerId", typeof(int));
			table.Columns.Add("DeliveryScheduleCustomerNo", typeof(int));
			table.Columns.Add("DeliveryScheduleCustomerDetailId", typeof(int));
			table.Columns.Add("OrderId", typeof(int));
			table.Columns.Add("OrderNo", typeof(int));
			table.Columns.Add("SaleOrderDetailId", typeof(int));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemCode");
			table.Columns.Add("Item");
			table.Columns.Add("ItemUOM");
			table.Columns.Add("ItemUOMId", typeof(int));
			table.Columns.Add("PUomEquivalent", typeof(double));
			table.Columns.Add("CropYearId");
			table.Columns.Add("CropYear");
			table.Columns.Add("PackingTypeId", typeof(int));
			table.Columns.Add("QTY", typeof(double));
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("LoadQty", typeof(double));
			table.Columns.Add("LoadWeight", typeof(double));
			table.Columns.Add("PackingUnit", typeof(double));
			table.Columns.Add("PackingWeight", typeof(double));
			table.Columns.Add("GrossWeight", typeof(double));
			table.Columns.Add("WareHouseId", typeof(int));
			table.Columns.Add("JobLotId", typeof(int));
			table.Columns.Add("Remarks");
			table.Columns.Add("Rate", typeof(double));
			table.Columns.Add("RateUom");
			table.Columns.Add("DocumentTypeId", typeof(int));
			table.Columns.Add("AvailableStock", typeof(double));
			table.Columns.Add("OrderPaymentTermId", typeof(int));
			table.Columns.Add("OrderPaymentTerm");
			dtExpense.Columns.Add("Id", typeof(int));
			dtExpense.Columns.Add("SaleOrderId", typeof(int));
			dtExpense.Columns.Add("SaleOrder", typeof(int));
			dtExpense.Columns.Add("SaleOrderCustomerExpId");
			dtExpense.Columns.Add("ItemId");
			dtExpense.Columns.Add("Qty", typeof(double));
			dtExpense.Columns.Add("Remarks");
			dtInfoGrid.Columns.Add("DocumentType");
			dtInfoGrid.Columns.Add("OrderNo");
			dtInfoGrid.Columns.Add("TotalQty", typeof(double));
			dtInfoGrid.Columns.Add("TotalWeight", typeof(double));
			dtInfoGrid.Columns.Add("Amount", typeof(double));
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
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			string documentNoData = Conversion.ToString((object)CommonServices.DeliveryOrderGenerateCode(DocumentTypeId));
			DataTable all = VehicleType.GetAll();
			DataTable historyComboData = HistoryComboDBCall();
			CommonServices.ReferencePartyServiceBind();
			BranchList = CommonServices.BrancheServiceBind();
			ReferencePartiesDBCall();
			OtherItemdtDbCall();
			return new
			{
				HistoryComboData = historyComboData,
				documentNoData = documentNoData,
				VehicleTypeData = all
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
				btnupdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				ChkPrint.Checked = formright.DoHavePrintRights;
				btnprint.Checked = formright.DoHavePrintRights;
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
				GetConfigurationsFromGlobal();
				if (ItemSearchWithNameOrCode)
				{
					rdSearchByCode.Checked = true;
				}
				else
				{
					rdSearchByName.Checked = true;
				}
				if (RecId == 0)
				{
					UpdateDocumentNoUI(result.documentNoData);
				}
				DeliveryOrderTypeBind();
				BranchNameBind();
				VehicleTypesBind(result.VehicleTypeData);
				SaleTypeBind();
				SupplierDtFillFromGlobal();
				BindSupplierName();
				WarehousesDtFillFromGlobalAndBind();
				CropDtFillFromGlobalAndBind();
				JobLotDtFillFromGlobalAndBind();
				PackingTypeDtFillFromGlobalAndBind();
				GetConfigurationsFromGlobalAndBindValuesInColumns();
				if (table != null && table.Columns.Count > 0)
				{
					grd.DataSource = table;
					grd.RetrieveStructure();
					grdSettings();
				}
				if (dtExpense != null && dtExpense.Columns.Count > 0)
				{
					AddRowInvExpGrid();
					GridExpense.RetrieveStructure();
					grdInvExpSettings();
				}
				if (dtInfoGrid != null && dtInfoGrid.Columns.Count > 0)
				{
					grdLoader.DataSource = dtInfoGrid;
					grdLoader.RetrieveStructure();
				}
				if (BranchList.Rows.Count > 1 && Conversion.ToInt(CmbDeliveryOrderType.Value) == 2)
				{
					label13.Text = "Branch From";
					label18.Visible = true;
					((Control)(object)cmbBranchTo).Visible = true;
				}
				else
				{
					label13.Text = "Branch Name";
					label18.Visible = false;
					((Control)(object)cmbBranchTo).Visible = false;
				}
				if (ScreenName == "DeliveryOrder")
				{
					CmbDeliveryOrderType.Value = 1;
					((Control)(object)CmbDeliveryOrderType).Enabled = false;
					btnLoadSaleOrder.Visible = true;
					btnLoadSaleOrder.Enabled = true;
					btnLoaderView2.Visible = true;
					btnLoaderView2.Enabled = true;
					CmbDeliveryOrderType_Leave(null, null);
				}
				else if (ScreenName == "DeliveryOrderTransfer")
				{
					CmbDeliveryOrderType.Value = 2;
					((Control)(object)CmbDeliveryOrderType).Enabled = false;
					CmbDeliveryOrderType_Leave(null, null);
					btnLoadSaleOrder.Visible = false;
					btnLoadSaleOrder.Enabled = false;
					btnLoaderView2.Visible = false;
					btnLoaderView2.Enabled = false;
				}
				btnupdate.Visible = false;
				btnDelete.Visible = false;
				btnSaveAs.Visible = false;
				btnsave.Visible = true;
				btnplus.Visible = true;
				if (BranchList.Rows.Count > 1 && Conversion.ToInt(CmbDeliveryOrderType.Value) == 1)
				{
					((Control)(object)cmbBranchFrom).Enabled = true;
				}
				else
				{
					((Control)(object)cmbBranchFrom).Enabled = false;
				}
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				cmbBranchFrom.Value = UserAccount.BranchesId;
				((Control)(object)CmbDeliveryOrderType).Enabled = false;
				((UltraGridBase)CmbSaleType).Rows[0].Activate();
				DocDate.Focus();
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable HistoryComboDBCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (Conversion.ToString(base.Tag) == "DeliveryOrder")
			{
				obj.ReqType = "Local";
			}
			else if (Conversion.ToString(base.Tag) == "DeliveryOrderTransfer")
			{
				obj.ReqType = "StockTransfer";
			}
			obj.BranchesIds = Conversion.ToString((object)UserAccount.BranchesId);
			obj.Activity = "Customer";
			return InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder(obj);
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
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dt, "Id", "ReferenceName", "Customer Name", AllColumns: true, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			WarningMsgForPaymentTermOtherThanCreditOnDO = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WarningMsgForPaymentTermOtherThanCreditOnDO"));
			IsStockReservedPerParty = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IsStockReservedPerParty"));
			ChkIsStockReservedPerParty.Visible = IsStockReservedPerParty;
			ApplicationsAllocateToCompany obj = clsGlobalVariables.GetApplicationsAllocateToCompanyList.Find((ApplicationsAllocateToCompany row) => row.AppId == 4 && row.CompanyId == UserAccount.CompanyId);
			IsCustomerPortal = obj != null && obj.IsActive;
			BtnLoadCustomerSchedule.Visible = IsCustomerPortal;
			ShowInfoGrid = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Order Rate Show On Delivery Order"));
			if (ShowInfoGrid)
			{
				DetailGridPanel.Dock = DockStyle.Fill;
				LoaderInformationPanel.Show();
				((Control)(object)grdLoader).Show();
			}
			else
			{
				DetailGridPanel.Dock = DockStyle.Fill;
				LoaderInformationPanel.Hide();
				((Control)(object)grdLoader).Hide();
			}
			ConfigrationsAllocation obj2 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "ItemSearchByCode");
			ItemSearchWithNameOrCode = Conversion.ToBool((object)((obj2 != null) ? obj2.ConfigKey : null));
			ConfigrationsAllocation obj3 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate");
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)((obj3 != null) ? obj3.ConfigKey : null));
			WarningMessageOnDOForPackingWeight = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("WarningMessageOnDOForPackingWeight"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobalAndBindValuesInColumns()
	{
		try
		{
			int jobId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Job/Lot"));
			if (jobId != 0)
			{
				CmbJobLot.Value = jobId;
			}
			int cropYearId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Default Crop Year"));
			if (cropYearId != 0)
			{
				CmbCropYear.Value = cropYearId;
			}
			int packingTypeId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Paking Type"));
			if (packingTypeId != 0)
			{
				CmbPackingType.Value = packingTypeId;
			}
			int wareHouseId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Warehouse"));
			if (wareHouseId != 0)
			{
				CmbWareHouse.Value = wareHouseId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string documentData)
	{
		txtdocno.Text = documentData;
	}

	private void DeliveryOrderTypeBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("OrderType");
			dt.Rows.Add(1, "Local");
			dt.Rows.Add(2, "StockTransfer");
			DDL.BindDDLNew(dt, CmbDeliveryOrderType, "Id", "OrderType", "OrderType", false);
			((UltraGridBase)CmbDeliveryOrderType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchNameBind()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbBranchFrom, BranchList, "Id", "BranchName", "Branch Name", AllColumns: false, Conversion.ToInt(cmbBranchFrom.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbBranchTo, BranchList, "Id", "BranchName", "Branch Name", AllColumns: false, Conversion.ToInt(cmbBranchTo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (BranchList.Rows.Count > 1 && Conversion.ToInt(CmbDeliveryOrderType.Value) == 2)
			{
				label13.Text = "Branch From";
				label18.Visible = true;
				((Control)(object)cmbBranchTo).Visible = true;
			}
			else
			{
				label13.Text = "Branch Name";
				label18.Visible = false;
				((Control)(object)cmbBranchTo).Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VehicleTypesBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbVehicleType, dt, "Id", "VehicleDescription", "Vehicle Type", AllColumns: true, Conversion.ToInt(CmbVehicleType.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaleTypeBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			dt.Rows.Add(1, "Weigh Bridge Based Billing");
			dt.Rows.Add(2, "Pack Size Based Billing");
			DDL.BindDDL(dt, CmbSaleType, "Id", "Name", "Sale Type", ZeroIndex: false);
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
			List<getGlobalSupplierCustomer> allSuppliers = clsGlobalVariables.globalSupplierCustomer;
			dtSupplier.Clear();
			IEnumerable<getGlobalSupplierCustomer> enumerable2;
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				IEnumerable<getGlobalSupplierCustomer> enumerable = allSuppliers;
				enumerable2 = enumerable;
			}
			else
			{
				enumerable2 = allSuppliers.Where((getGlobalSupplierCustomer r) => r.PartyTypeId == 2);
			}
			IEnumerable<getGlobalSupplierCustomer> filteredSuppliers = enumerable2;
			if (filteredSuppliers == null || !filteredSuppliers.Any())
			{
				return;
			}
			foreach (getGlobalSupplierCustomer supplier in filteredSuppliers)
			{
				dtSupplier.Rows.Add(Conversion.ToInt((object)supplier.Id), Conversion.ToString((object)supplier.CompanyName), Conversion.ToString((object)supplier.PartyCode), Conversion.ToInt((object)supplier.GlAccountId), Conversion.ToInt((object)supplier.CityId), Conversion.ToString((object)supplier.CityName), Conversion.ToString((object)supplier.MobilePersonal));
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
			InfragisticsHelper.BindAndRetainSelection(CmbSupplierCustomer, dtSupplier, "Id", "CompanyName", "Customer Name", AllColumns: true, Conversion.ToInt(CmbSupplierCustomer.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			((UltraGridBase)CmbSupplierCustomer).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			((UltraGridBase)CmbSupplierCustomer).DisplayLayout.Bands[0].Columns[4].Hidden = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ReferencePartiesDBCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReferenceParties obj = new ReferenceParties();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtRefParty = ReferenceParties.GetAll(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WarehousesDtFillFromGlobalAndBind()
	{
		try
		{
			List<GlobalWarehousesWithBranch> lst = clsGlobalVariables.globalWarehousesWithBranches;
			DatatableHelper.PopulateDataTable(dtWh, lst, (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName);
			int Id = Conversion.ToInt(CmbWareHouse.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbWareHouse, dtWh, "Id", "WareHouseName", "WareHouse Name", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemDtFillFromGlobal()
	{
		try
		{
			dtItem.Clear();
			List<getGlobalAllItems> dtItemAll = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems row) => row.ItemTypeOfTypeId != 14 && row.ItemTypeOfTypeId != 17).ToList();
			if (dtItemAll == null || dtItemAll.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllItems row2 in dtItemAll)
			{
				dtItem.Rows.Add(row2.Id, row2.ItemName, row2.ItemCode);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = 0;
			if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 2)
			{
				if (Conversion.ToInt(CmbItemName.Value) > 0)
				{
					Id = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["Id"].Value);
				}
			}
			else if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1 && Conversion.ToInt(CmbItemName.Value) > 0)
			{
				Id = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["SaleOrderDetailId"].Value);
			}
			string DisplayMember = (rdSearchByName.Checked ? "ItemName" : "ItemCode");
			string Caption = (rdSearchByName.Checked ? "Item Name" : "Item Code");
			if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 2)
			{
				InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "Id", DisplayMember, Caption, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: true, 1);
			}
			else if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1)
			{
				InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "SaleOrderDetailId", DisplayMember, Caption, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: true, 1);
			}
			PackUomFromGlobalBind(Id);
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (!ShowInfoGrid)
				{
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				}
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[6].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CropDtFillFromGlobalAndBind()
	{
		try
		{
			List<GlobalCropYear> lst = clsGlobalVariables.globalCropYear;
			DatatableHelper.PopulateDataTable(dtCrop, lst, (GlobalCropYear x) => x.Id, (GlobalCropYear x) => x.CropYear);
			int Id = Conversion.ToInt(CmbCropYear.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbCropYear, dtCrop, "Id", "CropYear", "Crop Year", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void JobLotDtFillFromGlobalAndBind()
	{
		try
		{
			dtJobLoat.Clear();
			List<GetJobLotGlIdsandName> lst = clsGlobalVariables.globalJobLot;
			DatatableHelper.PopulateDataTable(dtJobLoat, lst, (GetJobLotGlIdsandName x) => x.Id, (GetJobLotGlIdsandName x) => x.JobLotDescription);
			int Id = Conversion.ToInt(CmbJobLot.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbJobLot, dtJobLoat, "Id", "JobLotDescription", "JobLot", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PackingTypeDtFillFromGlobalAndBind()
	{
		try
		{
			List<InvPackingType> Finallist = clsGlobalVariables.globalInvPackingType;
			DatatableHelper.PopulateDataTable(dtPackingType, Finallist, (InvPackingType x) => x.Id, (InvPackingType x) => x.PackTypeDesc);
			int Id = Conversion.ToInt(CmbPackingType.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbPackingType, dtPackingType, "Id", "PackTypeDesc", "Packing Type", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PackUomFromGlobalBind(int ItemId)
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUom).Text;
			DataTable dtUOM = CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId);
			InfragisticsHelper.BindAndRetainSelection(CmbPackUom, dtUOM, "Id", "UOMCode", "Uom", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OtherItemdtDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InventoryItemsOther obj = new InventoryItemsOther();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtOtherItem = InventoryItemsOther.GetAll(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 2)
			{
				PackUomFromGlobalBind(Conversion.ToInt(CmbItemName.Value));
			}
			else if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1 && Conversion.ToInt(CmbItemName.Value) > 0)
			{
				PackUomFromGlobalBind(Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["Id"].Value));
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
			}
			GetDataFromLoader();
			AvailableStockGetByItem();
			CalculateOrderBalanceQtyandWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbBranchFrom).ActiveRow == null || Conversion.ToInt(cmbBranchFrom.Value) == 0)
		{
			MessageBox.Show("branch field is required");
			cmbBranchFrom.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDeliveryOrderType).ActiveRow == null || Conversion.ToInt(CmbDeliveryOrderType.Value) == 0)
		{
			MessageBox.Show("DeliveryOrderType Field is Required");
			CmbDeliveryOrderType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSaleType).ActiveRow == null || Conversion.ToInt(CmbSaleType.Value) == 0)
		{
			MessageBox.Show("Sale Type Field is Required");
			CmbSaleType.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbSupplierCustomer).ActiveRow == null || Conversion.ToInt(CmbSupplierCustomer.Value) == 0)
		{
			MessageBox.Show("Customer field is required");
			CmbSupplierCustomer.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemName).ActiveRow == null || Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("ItemName field is required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("PackUOM field is required");
			CmbPackUom.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingType).ActiveRow == null || Conversion.ToInt(CmbPackingType.Value) == 0)
		{
			MessageBox.Show("Packingtype field is required");
			CmbPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null || Conversion.ToInt(CmbCropYear.Value) == 0)
		{
			MessageBox.Show("CropYear field is required");
			CmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("JobLot field is required");
			CmbJobLot.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtqty.Text) == 0.0)
		{
			MessageBox.Show("ItemQty field is required");
			txtqty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtweight.Text) == 0.0)
		{
			MessageBox.Show("Weight field is required");
			txtweight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtGrossWeight.Text) == 0.0)
		{
			MessageBox.Show("GrossWeight field is required");
			txtGrossWeight.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1 && (((UltraGridBase)CmbOrderNo).ActiveRow == null || Conversion.ToInt(CmbOrderNo.Value) == 0))
		{
			MessageBox.Show("OrderNo field is required");
			CmbOrderNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWareHouse).ActiveRow == null || Conversion.ToInt(CmbWareHouse.Value) == 0)
		{
			MessageBox.Show("WareHouseName field is required");
			CmbWareHouse.Focus();
			return false;
		}
		return true;
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount > 0 && ScheduleExist)
			{
				throw new Exception("You can not add manual Record because record against Customer Schedule exist in Grid");
			}
			if (FormValidationDetail())
			{
				int OrderId = Conversion.ToInt(CmbOrderNo.Value);
				int OrderNo = Conversion.ToInt((object)((Control)(object)CmbOrderNo).Text.Trim());
				int PaymentTermId = 0;
				string PaymentTerm = string.Empty;
				if (((UltraGridBase)CmbOrderNo).ActiveRow != null && OrderId > 0)
				{
					PaymentTermId = Conversion.ToInt(((UltraDropDownBase)CmbOrderNo).SelectedRow.Cells["PaymentTermsId"].Value);
					PaymentTerm = Conversion.ToString(((UltraDropDownBase)CmbOrderNo).SelectedRow.Cells["PaymentTerm"].Value);
				}
				if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1)
				{
					table.Rows.Add(0, 0, 0, 0, Conversion.ToInt(CmbSupplierCustomer.Value), ((Control)(object)CmbSupplierCustomer).Text.Trim(), 0, 0, 0, 0, OrderId, OrderNo, Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["SaleOrderDetailId"].Value), Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["Id"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value), ((Control)(object)CmbPackUom).Text.Trim(), Conversion.ToInt(CmbPackUom.Value), Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value), Conversion.ToString(CmbCropYear.Value), ((Control)(object)CmbCropYear).Text.Trim(), Conversion.ToInt(CmbPackingType.Value), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtweight.Text.Trim()), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtweight.Text.Trim()), Conversion.ToDouble((object)txtPackUnit.Text.Trim()), Conversion.ToDouble((object)txtPackWeight.Text.Trim()), Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), Conversion.ToInt(CmbWareHouse.Value), Conversion.ToInt(CmbJobLot.Value), txtremarksdetail.Text.Trim(), ShowInfoGrid ? ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["Rate"].Value : ((object)0), ShowInfoGrid ? ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["RateUom"].Value : ((object)0), ShowInfoGrid ? ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["DocumentTypeId"].Value : ((object)0), 0, PaymentTermId, PaymentTerm);
				}
				if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 2)
				{
					table.Rows.Add(0, 0, 0, 0, Conversion.ToInt(CmbSupplierCustomer.Value), ((Control)(object)CmbSupplierCustomer).Text.Trim(), 0, 0, 0, 0, Conversion.ToInt(CmbOrderNo.Value), Conversion.ToInt((object)((Control)(object)CmbOrderNo).Text.Trim()), 0, Conversion.ToInt(CmbItemName.Value), ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value, ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value, ((Control)(object)CmbPackUom).Text.Trim(), Conversion.ToInt(CmbPackUom.Value), Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value), CmbCropYear.Value, ((Control)(object)CmbCropYear).Text.Trim(), Conversion.ToInt(CmbPackingType.Value), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtweight.Text.Trim()), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtweight.Text.Trim()), Conversion.ToDouble((object)txtPackUnit.Text.Trim()), Conversion.ToDouble((object)txtPackWeight.Text.Trim()), Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), Conversion.ToInt(CmbWareHouse.Value), Conversion.ToInt(CmbJobLot.Value), txtremarksdetail.Text.Trim(), 0, 0, 0, 0);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1 && ((UltraGridBase)CmbItemName).ActiveRow != null && Conversion.ToInt(CmbItemName.Value) > 0)
				{
					LoadInGridExpense(Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["SaleOrderDetailId"].Value));
				}
				ResetDetail();
				((Control)(object)CmbDeliveryOrderType).Enabled = false;
				((Control)(object)CmbSaleType).Enabled = true;
				FillInformationGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCommonSetting(GridEX grd, bool Editing = false)
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "SupplierCustomerId", "DeliveryScheduleCustomerId", "DeliveryScheduleCustomerDetailId", "OrderId", "SaleOrderDetailId", "Weight",
				"ItemId", "ItemUOMId", "CropYearId", "QTY", "RateUom", "DocumentTypeId", "PUomEquivalent", "OrderPaymentTermId"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"LoadQty",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns, Editing ? editableColumns : null);
			grd.RootTable.Columns["DeliveryScheduleCustomerNo"].Visible = ScheduleExist;
			grd.RootTable.Columns["Rate"].Visible = ShowInfoGrid;
			grd.RootTable.Columns["RefPartyId"].Caption = "Reference Party";
			grd.RootTable.Columns["WareHouseId"].Caption = "WareHouse Name";
			grd.RootTable.Columns["PackingTypeId"].Caption = "Packing Type";
			grd.RootTable.Columns["JobLotId"].Caption = "JobLotDescription";
			grd.RootTable.Columns["DeliveryScheduleCustomerNo"].Caption = "Delivery Schedule No";
			grd.RootTable.Columns["SupplierCustomer"].Caption = "CustomerName";
			grd.RootTable.Columns["Item"].Width = 250;
			grd.RootTable.Columns["SupplierCustomer"].Width = 200;
			grd.RootTable.Columns["DeliveryScheduleCustomerNo"].Width = 100;
			grd.RootTable.Columns["RefPartyId"].Width = 100;
			grd.RootTable.Columns["WareHouseId"].Width = 150;
			grd.RootTable.Columns["PackingTypeId"].Width = 150;
			grd.RootTable.Columns["JobLotId"].Width = 150;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grd.RootTable.Columns["RefPartyId"], dtRefParty, "Id", "ReferencePartyName", "Reference Party");
			GridEX_Helper.ComboBind(grd.RootTable.Columns["WareHouseId"], dtWh, "Id", "WareHouseName", "WareHouse Name");
			GridEX_Helper.ComboBind(grd.RootTable.Columns["PackingTypeId"], dtPackingType, "Id", "PackTypeDesc", "Packing Type");
			GridEX_Helper.ComboBind(grd.RootTable.Columns["JobLotId"], dtJobLoat, "Id", "JobLotDescription", "Job Lot");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grdCommonSetting(grd, Editing: true);
			grdComboBind();
			GridEX_Helper.AddButton(grd, "Add", "+", 20, 0);
			GridEX_Helper.AddButton(grd, "Delete", "X", 20, 1);
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Add")
				{
					AddDetailRow(r);
				}
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			string colKey = e.Column.Key;
			if (colKey == "LoadQty" || colKey == "PackingWeight")
			{
				if (item.Cells["PUomEquivalent"].Value.ToString() != string.Empty && item.Cells["LoadQty"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble((object)item.Cells["LoadQty"].Value.ToString());
					double UOM = Conversion.ToDouble((object)item.Cells["PUomEquivalent"].Value.ToString());
					double Weight2 = num * UOM;
					item.Cells["LoadWeight"].Value = Weight2;
					item.Cells["GrossWeight"].Value = Weight2;
				}
				if (item.Cells["PUomEquivalent"].Value.ToString() != string.Empty && item.Cells["LoadQty"].Value.ToString() != string.Empty && item.Cells["PackingWeight"].Value.ToString() != string.Empty)
				{
					double Weight = Conversion.ToDouble((object)item.Cells["LoadWeight"].Value.ToString());
					double GrossWeight = Conversion.ToDouble((object)item.Cells["PackingWeight"].Value.ToString()) + Weight;
					item.Cells["GrossWeight"].Value = GrossWeight;
				}
				FillInformationGrid();
			}
			switch (colKey)
			{
			case "WareHouseId":
			case "CropYearId":
			case "JobLotId":
			case "PackingTypeId":
				UpdateAvailableStockInGridDetailRow(item);
				break;
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		txtRefDocTypeId.Text = string.Empty;
		txtDocNoId.Text = string.Empty;
		txtSubNoId.Text = string.Empty;
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		updateDetailIndex = -1;
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount > 0 && ScheduleExist)
			{
				throw new Exception("You can not update manual Record because record against Customer Schedule exist in Grid");
			}
			if (FormValidationDetail())
			{
				int OrderId = Conversion.ToInt(CmbOrderNo.Value);
				int OrderNo = Conversion.ToInt((object)((Control)(object)CmbOrderNo).Text.Trim());
				int PaymentTermId = 0;
				string PaymentTerm = string.Empty;
				if (((UltraGridBase)CmbOrderNo).ActiveRow != null && OrderId > 0)
				{
					PaymentTermId = Conversion.ToInt(((UltraDropDownBase)CmbOrderNo).SelectedRow.Cells["PaymentTermsId"].Value);
					PaymentTerm = Conversion.ToString(((UltraDropDownBase)CmbOrderNo).SelectedRow.Cells["PaymentTerm"].Value);
				}
				table.Rows[updateDetailIndex]["QTY"] = Conversion.ToDouble((object)txtqty.Text);
				table.Rows[updateDetailIndex]["Weight"] = Conversion.ToDouble((object)txtweight.Text);
				if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1)
				{
					table.Rows[updateDetailIndex]["OrderId"] = OrderId;
					table.Rows[updateDetailIndex]["OrderNo"] = OrderNo;
					table.Rows[updateDetailIndex]["OrderPaymentTermId"] = PaymentTermId;
					table.Rows[updateDetailIndex]["OrderPaymentTerm"] = PaymentTerm;
					table.Rows[updateDetailIndex]["SaleOrderDetailId"] = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["SaleOrderDetailId"].Value);
				}
				table.Rows[updateDetailIndex]["RefDocumentTypeId"] = Conversion.ToInt((object)txtRefDocTypeId.Text);
				table.Rows[updateDetailIndex]["RefDocIdNo"] = Conversion.ToInt((object)txtDocNoId.Text);
				table.Rows[updateDetailIndex]["RefDocSubIdNo"] = Conversion.ToInt((object)txtSubNoId.Text);
				table.Rows[updateDetailIndex]["SupplierCustomerId"] = Conversion.ToInt(CmbSupplierCustomer.Value);
				table.Rows[updateDetailIndex]["SupplierCustomer"] = Conversion.ToString((object)((Control)(object)CmbSupplierCustomer).Text);
				table.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["Id"].Value);
				table.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value);
				table.Rows[updateDetailIndex]["Item"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value);
				table.Rows[updateDetailIndex]["ItemUOMId"] = Conversion.ToInt(CmbPackUom.Value);
				table.Rows[updateDetailIndex]["ItemUOM"] = Conversion.ToString((object)((Control)(object)CmbPackUom).Text);
				table.Rows[updateDetailIndex]["PUomEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value);
				table.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(CmbPackingType.Value);
				table.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToString(CmbCropYear.Value);
				table.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
				table.Rows[updateDetailIndex]["LoadQty"] = Conversion.ToDouble((object)txtqty.Text);
				table.Rows[updateDetailIndex]["PackingUnit"] = Conversion.ToDouble((object)txtPackUnit.Text);
				table.Rows[updateDetailIndex]["PackingWeight"] = Conversion.ToDouble((object)txtPackWeight.Text);
				table.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				table.Rows[updateDetailIndex]["LoadWeight"] = Conversion.ToDouble((object)txtweight.Text);
				table.Rows[updateDetailIndex]["WareHouseId"] = Conversion.ToInt(CmbWareHouse.Value);
				table.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(CmbJobLot.Value);
				table.Rows[updateDetailIndex]["Rate"] = Conversion.ToDouble(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["Rate"].Value);
				table.Rows[updateDetailIndex]["RateUom"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["RateUom"].Value);
				table.Rows[updateDetailIndex]["DocumentTypeId"] = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["DocumentTypeId"].Value);
				table.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtremarksdetail.Text);
				grd.DataSource = table;
				if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1 && ((UltraGridBase)CmbItemName).ActiveRow != null && Conversion.ToInt(CmbItemName.Value) > 0)
				{
					LoadInGridExpense(Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["SaleOrderDetailId"].Value));
				}
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
				BindSupplierName();
				FillInformationGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0 || ScheduleExist)
			{
				return;
			}
			if (((Control)(object)CmbDeliveryOrderType).Text != "StockTransfer")
			{
				updateDetailIndex = item.RowIndex;
				CmbSupplierCustomer.Value = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
				CmbSupplierCustomer_Leave(null, null);
				CmbOrderNo.Value = Conversion.ToInt(item.Cells["OrderId"].Value);
				CmbOrderNo_Leave(null, null);
				if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 2)
				{
					CmbItemName.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				}
				else if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1)
				{
					CmbItemName.Value = Conversion.ToInt(item.Cells["SaleOrderDetailId"].Value);
				}
				PackUomFromGlobalBind(Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["Id"].Value));
				CmbPackUom.Value = item.Cells["ItemUOMId"].Value.ToString();
				((Control)(object)CmbPackUom).Text = item.Cells["ItemUOM"].Text.ToString();
				txtRefDocTypeId.Text = item.Cells["RefDocumentTypeId"].Value.ToString();
				txtDocNoId.Text = item.Cells["RefDocIdNo"].Value.ToString();
				txtSubNoId.Text = item.Cells["RefDocSubIdNo"].Value.ToString();
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				CmbCropYear.Value = item.Cells["CropYearId"].Value.ToString();
				txtBalQty.Text = item.Cells["QTY"].Value.ToString();
				txtBalWeight.Text = item.Cells["Weight"].Value.ToString();
				txtqty.Text = item.Cells["LoadQty"].Value.ToString();
				txtweight.Text = item.Cells["LoadWeight"].Value.ToString();
				txtPackUnit.Text = item.Cells["PackingUnit"].Value.ToString();
				txtPackWeight.Text = item.Cells["PackingWeight"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				CmbWareHouse.Value = item.Cells["WareHouseId"].Value.ToString();
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				txtremarksdetail.Text = item.Cells["Remarks"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbSupplierCustomer.Focus();
			}
			else
			{
				updateDetailIndex = item.RowIndex;
				txtRefDocTypeId.Text = item.Cells["RefDocumentTypeId"].Value.ToString();
				txtDocNoId.Text = item.Cells["RefDocIdNo"].Value.ToString();
				txtSubNoId.Text = item.Cells["RefDocSubIdNo"].Value.ToString();
				CmbSupplierCustomer.Value = item.Cells["SupplierCustomerId"].Value.ToString();
				((Control)(object)CmbSupplierCustomer).Text = item.Cells["SupplierCustomer"].Text.ToString();
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				((Control)(object)CmbItemName).Text = item.Cells["Item"].Value.ToString();
				PackUomFromGlobalBind(Conversion.ToInt(CmbItemName.Value));
				CmbPackUom.Value = item.Cells["ItemUOMId"].Value.ToString();
				((Control)(object)CmbPackUom).Text = item.Cells["ItemUOM"].Text.ToString();
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				CmbCropYear.Value = item.Cells["CropYearId"].Value.ToString();
				txtBalQty.Text = item.Cells["QTY"].Value.ToString();
				txtBalWeight.Text = item.Cells["Weight"].Value.ToString();
				txtqty.Text = item.Cells["LoadQty"].Value.ToString();
				txtweight.Text = item.Cells["LoadWeight"].Value.ToString();
				txtPackUnit.Text = item.Cells["PackingUnit"].Value.ToString();
				txtPackWeight.Text = item.Cells["PackingWeight"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				CmbWareHouse.Value = item.Cells["WareHouseId"].Value.ToString();
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				txtremarksdetail.Text = item.Cells["Remarks"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbSupplierCustomer.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					FillDetailListCommonForInsertAndDelete(vd, r);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					table.Rows[r.RowIndex].Delete();
					r.Delete();
					grd.UpdateData();
				}
				else
				{
					r.Delete();
					grd.UpdateData();
				}
				FillInformationGrid();
				GetSaleOrderIds();
				GetDeliveryScheduleDetailIds();
				if (grd.GetRows().Count() == 0)
				{
					((Control)(object)CmbSaleType).Enabled = true;
				}
				return;
			}
			throw new Exception("Reset Detail First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				DataRow dr = table.Rows[r.RowIndex];
				table.Rows.Add(dr.ItemArray);
				if (btnupdate.Visible && btnupdate.Enabled)
				{
					table.Rows[table.Rows.Count - 1]["Id"] = 0;
				}
				grd.DataSource = table;
				FillInformationGrid();
				GetSaleOrderIds();
				GetDeliveryScheduleDetailIds();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvExpGrid()
	{
		try
		{
			dtExpense.Rows.Add(0, 0, 0, 0, 0, 0, "");
			GridExpense.DataSource = dtExpense;
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
			GridExpense.ColumnAutoResize = false;
			GridExpense.RootTable.Columns["Id"].Visible = false;
			GridExpense.RootTable.Columns["SaleOrderId"].Visible = false;
			GridExpense.RootTable.Columns["SaleOrderCustomerExpId"].Visible = false;
			GridExpense.RootTable.Columns["Id"].EditType = (EditType)0;
			GridExpense.RootTable.Columns["SaleOrderId"].EditType = (EditType)0;
			GridExpense.RootTable.Columns["SaleOrderCustomerExpId"].EditType = (EditType)0;
			GridExpense.RootTable.Columns["SaleOrder"].EditType = (EditType)0;
			GridExpense.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			GridExpense.RootTable.Columns["ItemId"].HasValueList = true;
			GridExpense.RootTable.Columns["ItemId"].LimitToList = true;
			GridExpense.RootTable.Columns["ItemId"].EditType = (EditType)4;
			GridExpense.RootTable.Columns["ItemId"].Caption = "Item";
			GridExpense.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			GridExpense.RootTable.Columns["ItemId"].Width = 250;
			GridExpense.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GridExpense.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GridExpense.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			GridExpense.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			GridExpense.RootTable.Columns.Add("Delete");
			GridExpense.RootTable.Columns["Delete"].Key = "Delete";
			GridExpense.RootTable.Columns["Delete"].Caption = "X";
			GridExpense.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GridExpense.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			GridExpense.RootTable.Columns["Delete"].ButtonText = "X";
			GridExpense.RootTable.Columns["Delete"].Width = 20;
			GridExpense.RootTable.Columns.Add("Add");
			GridExpense.RootTable.Columns["Add"].Key = "Add";
			GridExpense.RootTable.Columns["Add"].Caption = "+";
			GridExpense.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GridExpense.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			GridExpense.RootTable.Columns["Add"].ButtonText = "+";
			GridExpense.RootTable.Columns["Add"].Width = 20;
			GridExpense.RootTable.Columns["Add"].Position = 0;
			GridExpense.RootTable.Columns["Delete"].Position = 1;
			GridExpense.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridExpense_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GridExpense.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteExpenseDetailRow(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInvExpGrid();
				}
				GridExpense.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteExpenseDetailRow(GridEXRow r)
	{
		try
		{
			dtExpense.Rows[r.RowIndex].Delete();
			r.Delete();
			if (dtExpense.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
			GridExpense.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool ValidatePaymentTypes()
	{
		List<string> validationMessages = new List<string>();
		GridEXRow[] dataRows = grd.GetDataRows();
		foreach (GridEXRow obj in dataRows)
		{
			int paymentTermId = Conversion.ToInt(obj.Cells["OrderPaymentTermId"].Value);
			string paymentTerm = Conversion.ToString(obj.Cells["OrderPaymentTerm"].Value);
			int orderNo = Conversion.ToInt(obj.Cells["OrderNo"].Value);
			if (paymentTermId == 1 || paymentTermId == 3)
			{
				validationMessages.Add($"Order No {orderNo}: Payment Term is '{paymentTerm}'.");
			}
		}
		List<string> distinctMessages = validationMessages.Distinct().ToList();
		if (distinctMessages.Count > 0)
		{
			return MessageBox.Show(string.Join(Environment.NewLine, distinctMessages) + "\n\nAre you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
		}
		return true;
	}

	private void Insert()
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Expected O, but got Unknown
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c9: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 2)
			{
				if (((UltraGridBase)cmbBranchTo).ActiveRow == null || Conversion.ToInt(cmbBranchTo.Value) == 0)
				{
					MessageBox.Show("'Branch To' Field is Required...");
					cmbBranchTo.Focus();
					return;
				}
				if (Conversion.ToInt(cmbBranchFrom.Value) == Conversion.ToInt(cmbBranchTo.Value))
				{
					MessageBox.Show(" 'Branch From' Cannot Be Equal To 'Branch To' ...");
					return;
				}
			}
			InvDeliveryOrder po = new InvDeliveryOrder();
			if (RecId > 0)
			{
				if (!FormHelper.ConfirmAction("Are you sure to Update"))
				{
					return;
				}
				po.Id = RecId;
				po.ModifyUser = UserAccount.ID;
				po.IsApproved = IsApproved;
			}
			else if (!FormHelper.ConfirmAction("Are you sure to Save"))
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			for (int j = 0; j < rows.Length; j++)
			{
				int SaleorderId = Conversion.ToInt(rows[j].Cells["OrderId"].Value);
				if (((Control)(object)CmbDeliveryOrderType).Text != "StockTransfer" && SaleorderId == 0)
				{
					throw new Exception("SaleOrder No Not Found");
				}
			}
			double TotalPackingWeight = GridEX_Helper.GetColumnSum(grd, "PackingWeight");
			if ((WarningMessageOnDOForPackingWeight && TotalPackingWeight == 0.0 && MessageBox.Show("There is no Packing Weight added on any row. Are you Sure To Proceed?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No) || (WarningMsgForPaymentTermOtherThanCreditOnDO && !ValidatePaymentTypes()))
			{
				return;
			}
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.BranchesId = Conversion.ToInt(cmbBranchFrom.Value);
			po.ToBranchId = Conversion.ToInt(cmbBranchTo.Value);
			po.DocumentTypeId = DocumentTypeId;
			po.ScreenName = ScreenName;
			po.EntryUser = UserAccount.ID;
			po.ModifyUser = UserAccount.ID;
			po.EntryDate = DateTime.Now;
			po.ModifyDate = DateTime.Now;
			po.DocNo = Conversion.ToInt((object)txtdocno.Text);
			po.DocDate = DocDate.Value;
			po.DeliveryOrderType = Conversion.ToString((object)((Control)(object)CmbDeliveryOrderType).Text);
			po.SaleTypeId = Conversion.ToInt(CmbSaleType.Value);
			po.VehicleType = Conversion.ToString((object)((Control)(object)CmbVehicleType).Text);
			po.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text.Trim());
			po.LoadingInstructions = Conversion.ToString((object)txtremarks.Text);
			if (ChkIsStockReservedPerParty.Visible)
			{
				po.IsStockReserved = Conversion.ToBool((object)ChkIsStockReservedPerParty.Checked);
			}
			po.InvDeliveryOrderDetailList = new List<InvDeliveryOrderdetail>();
			po.InvDeliveryOrderExpenseslist = new List<InvDeliveryOrderExpense>();
			po.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			po.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			if (lstRemoveRecord.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					po.InvDeliveryOrderDetailList.Add(lstRemoveRecord[i]);
				}
			}
			rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
				vd.Id = ((RecId != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd, r2);
				if (((Control)(object)CmbDeliveryOrderType).Text != "StockTransfer")
				{
					if (ScheduleExist)
					{
						FormHelper.ValidateField(vd.DeliveryScheduleId, "DeliveryScheduleCustomerId", r2.RowIndex);
						FormHelper.ValidateField(vd.DeliveryScheduleDetailId, "DeliveryScheduleCustomerDetailId", r2.RowIndex);
					}
					FormHelper.ValidateField(vd.SaleOrderId, "OrderNo", r2.RowIndex);
					FormHelper.ValidateField(vd.SaleOrderDetailId, "OrderDetailId", r2.RowIndex);
				}
				FormHelper.ValidateField(vd.SupplierCustomerId, "Customer", r2.RowIndex);
				FormHelper.ValidateField(vd.WarehouseId, "Warehouse", r2.RowIndex);
				FormHelper.ValidateField(vd.ItemId, "Item", r2.RowIndex);
				FormHelper.ValidateField(vd.CropYearId, "CropYear", r2.RowIndex);
				FormHelper.ValidateField(vd.JobLotId, "JobLot", r2.RowIndex);
				FormHelper.ValidateField(vd.InvPackingTypeId, "PackingType", r2.RowIndex);
				FormHelper.ValidateField(vd.PackUomId, "PackUom", r2.RowIndex);
				FormHelper.ValidateField(vd.DoQty, "Qty", r2.RowIndex);
				FormHelper.ValidateField(vd.LoadingWeight, "DoWeight", r2.RowIndex);
				FormHelper.ValidateField(vd.GrossWeight, "GrossWeight", r2.RowIndex);
				po.DoTotalQty += Conversion.ToDecimal((object)vd.DoQty);
				po.NetWeight += vd.LoadingWeight;
				po.PackingWeight += vd.OuterEbTotal;
				vd.TotalPackingWeight = vd.OuterEbTotal;
				po.GrossWeight += vd.GrossWeight;
				po.InvDeliveryOrderDetailList.Add(vd);
			}
			rows = GridExpense.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvDeliveryOrderExpense obj = new InvDeliveryOrderExpense();
				obj.Id = Conversion.ToInt(r.Cells["Id"].Value);
				obj.SaleOrderId = Conversion.ToInt(r.Cells["SaleOrderId"].Value);
				if (obj.SaleOrderId > 0 && !table.AsEnumerable().Any((DataRow row) => row.Field<int>("OrderId") == obj.SaleOrderId))
				{
					throw new Exception($"Order Id in 'row No {r.RowIndex + 1} in Expense Grid' Does not Exists in 'Detail Grid'\nPlease Remove that row or And Revelent SaleOrder row In Detail");
				}
				obj.SaleOrderCustomerExpId = Conversion.ToInt(r.Cells["SaleOrderCustomerExpId"].Value);
				obj.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				obj.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
				obj.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
				po.InvDeliveryOrderExpenseslist.Add(obj);
			}
			string WarningMessage = "";
			WarningMessage = InvDeliveryOrder.DeliveryOrderStackWarningMessage(po);
			if (WarningMessage != "" && MessageBox.Show(WarningMessage, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
			{
				return;
			}
			FormHelper.UpdateAttachmentsForObject<InvDeliveryOrder>(po, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
			po.AttachmentsValues = AttachmentsValues;
			po.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = InvDeliveryOrder.Save(po);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + po.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, ScreenName, DocumentTypeId, 0);
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + po.DocNo);
			}
			Reset();
			if (btnsave.Visible && btnsave.Enabled)
			{
				OpenLinkedFormOnInsert(po.DocNo);
			}
			if (ChkPrint.Checked)
			{
				CommonServices.InvDeliveryOrderSlip(success, DocumentTypeId);
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("Record Id Not Found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				if (IsApproved)
				{
					MessageBox.Show("Record has been approved", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else if (btnDelete.Visible && btnDelete.Enabled)
				{
					CommonServices.RemoveByID(DocumentTypeId, RecId);
					MessageBox.Show("Record Deleted Successfully");
					Reset();
				}
			}
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
			Reset();
			RecId = ID;
			InvDeliveryOrder po = InvDeliveryOrder.GetByID(ID);
			if (po == null || po.InvDeliveryOrderDetailList.Count <= 0)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			btnsave.Visible = false;
			btnSaveAs.Visible = false;
			btnupdate.Visible = true;
			btnDelete.Visible = true;
			txtdocno.Text = Conversion.ToString((object)po.DocNo);
			DocDate.Value = po.DocDate;
			cmbBranchFrom.Value = Conversion.ToInt((object)po.BranchesId);
			cmbBranchTo.Value = Conversion.ToInt((object)po.ToBranchId);
			((Control)(object)CmbDeliveryOrderType).Text = Conversion.ToString((object)po.DeliveryOrderType);
			CmbDeliveryOrderType_Leave(null, null);
			((Control)(object)CmbDeliveryOrderType).Enabled = false;
			if (po.SaleTypeId > 0)
			{
				CmbSaleType.Value = po.SaleTypeId;
				((Control)(object)CmbSaleType).Enabled = true;
			}
			((Control)(object)CmbVehicleType).Text = Conversion.ToString((object)po.VehicleType);
			txtVehicleNo.Text = po.VehicleNo;
			txtremarks.Text = Conversion.ToString((object)po.LoadingInstructions);
			IsApproved = po.IsApproved;
			ChkIsStockReservedPerParty.Checked = Conversion.ToBool((object)po.IsStockReserved);
			AttachmentsValues = po.AttachmentsValues;
			CustomAttachmentsValues = po.CustomAttachmentsValues;
			table.Clear();
			FilldtDetailFromListCommonForReadById(po, table);
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			dtExpense.Rows.Clear();
			if (po.InvDeliveryOrderExpenseslist != null && po.InvDeliveryOrderExpenseslist.Count > 0)
			{
				for (int i = 0; i < po.InvDeliveryOrderExpenseslist.Count; i++)
				{
					dtExpense.Rows.Add(po.InvDeliveryOrderExpenseslist[i].Id, po.InvDeliveryOrderExpenseslist[i].SaleOrderId, po.InvDeliveryOrderExpenseslist[i].SaleOrder, po.InvDeliveryOrderExpenseslist[i].SaleOrderCustomerExpId, po.InvDeliveryOrderExpenseslist[i].ItemId, po.InvDeliveryOrderExpenseslist[i].Qty, po.InvDeliveryOrderExpenseslist[i].Remarks);
				}
			}
			if (dtExpense.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
			GridExpense.DataSource = dtExpense;
			GridExpense.RetrieveStructure();
			grdInvExpSettings();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			FillInformationGrid();
			CmbDeliveryOrderType_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(InvDeliveryOrderdetail vd, GridEXRow r)
	{
		vd.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
		vd.RefDocIdNo = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
		vd.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
		vd.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
		vd.SupplierCustomer = Conversion.ToString(r.Cells["SupplierCustomer"].Value);
		vd.RefPartyId = Conversion.ToInt(r.Cells["RefPartyId"].Value);
		if (((Control)(object)CmbDeliveryOrderType).Text != "StockTransfer")
		{
			vd.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
			vd.SaleOrderDetailId = Conversion.ToInt(r.Cells["SaleOrderDetailId"].Value);
			vd.OrderNo = Conversion.ToString(r.Cells["OrderNo"].Value);
			if (ScheduleExist)
			{
				vd.DeliveryScheduleId = Conversion.ToInt(r.Cells["DeliveryScheduleCustomerId"].Value);
				vd.DeliveryScheduleDetailId = Conversion.ToInt(r.Cells["DeliveryScheduleCustomerDetailId"].Value);
				vd.DeliveryScheduleCustomerNo = Conversion.ToInt(r.Cells["DeliveryScheduleCustomerNo"].Value);
			}
		}
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["Item"].Value);
		vd.PackUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
		vd.PackUOM = Conversion.ToString(r.Cells["ItemUOM"].Value);
		vd.PUomEquivalent = Conversion.ToDouble(r.Cells["PUomEquivalent"].Value);
		vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
		vd.CropYear = Conversion.ToString(r.Cells["CropYear"].Value);
		vd.InvPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
		vd.DoQty = Conversion.ToDouble(r.Cells["LoadQty"].Value);
		vd.DoWeight = Conversion.ToDouble(r.Cells["LoadWeight"].Value);
		vd.LoadingQty = Conversion.ToDouble(r.Cells["LoadQty"].Value);
		vd.LoadingWeight = Conversion.ToDouble(r.Cells["LoadWeight"].Value);
		vd.PackingWeight = Conversion.ToDouble(r.Cells["PackingUnit"].Value);
		vd.OuterEbTotal = Conversion.ToDouble(r.Cells["PackingWeight"].Value);
		vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
		vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
		vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
		vd.LoadingRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
		vd.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
		vd.RateUom = Conversion.ToDouble(r.Cells["RateUom"].Value);
		vd.DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
	}

	private void FilldtDetailFromListCommonForReadById(InvDeliveryOrder invGrn, DataTable dt, bool AddExtraColumns = false)
	{
		if (invGrn == null || invGrn.InvDeliveryOrderDetailList == null || invGrn.InvDeliveryOrderDetailList.Count == 0)
		{
			return;
		}
		foreach (InvDeliveryOrderdetail detail in invGrn.InvDeliveryOrderDetailList)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.RefDocumentTypeId, detail.RefDocIdNo, detail.RefDocSubIdNo, detail.SupplierCustomerId, detail.SupplierCustomer, detail.RefPartyId, detail.DeliveryScheduleId, detail.DeliveryScheduleCustomerNo, detail.DeliveryScheduleDetailId,
				detail.SaleOrderId, detail.OrderNo, detail.SaleOrderDetailId, detail.ItemId, detail.ItemCode, detail.ItemName, detail.PackUOM, detail.PackUomId, detail.PUomEquivalent, detail.CropYearId,
				detail.CropYear, detail.InvPackingTypeId, detail.DoQty, detail.DoWeight, detail.LoadingQty, detail.LoadingWeight, detail.PackingWeight, detail.OuterEbTotal, detail.GrossWeight, detail.WarehouseId,
				detail.JobLotId, detail.LoadingRemarks, detail.Rate, detail.RateUom, detail.DocumentTypeId, 0, detail.OrderPaymentTermId, detail.OrderPaymentTerm
			};
			if (AddExtraColumns)
			{
				rowValues.Add(detail.WareHouseName);
				rowValues.Add(detail.JobLotDescription);
				rowValues.Add(detail.PackTypeDesc);
			}
			dt.Rows.Add(rowValues.ToArray());
			if (!ScheduleExist && detail.DeliveryScheduleDetailId > 0)
			{
				ScheduleExist = true;
			}
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string>
			{
				"UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems", "MultiCurrency", "AllCities", "CropYear", "JobLot", "InvPackingType",
				"AllBrands"
			};
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			BranchList = CommonServices.BrancheServiceBind();
			BranchNameBind();
			VehicleTypesBind(VehicleType.GetAll());
			SupplierDtFillFromGlobal();
			BindSupplierName();
			WarehousesDtFillFromGlobalAndBind();
			ItemDtFillFromGlobal();
			ItemNameBind();
			PackUomFromGlobalBind(Conversion.ToInt(CmbItemName.Value));
			CropDtFillFromGlobalAndBind();
			JobLotDtFillFromGlobalAndBind();
			PackingTypeDtFillFromGlobalAndBind();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			OtherItemdtDbCall();
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
			lstRemoveRecord.Clear();
			((Control)(object)CmbSaleType).Enabled = true;
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			ChkIsStockReservedPerParty.Checked = false;
			ScheduleExist = false;
			dtSaleOrderIds = "";
			dtSaleOrderMainIds = "";
			RecId = 0;
			((Control)(object)CmbDeliveryOrderType).Enabled = false;
			IsApproved = false;
			if (((UltraGridBase)cmbBranchTo).DataSource != null)
			{
				((UltraGridBase)cmbBranchTo).Rows[0].Activate();
			}
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtRefDocTypeId.Text = string.Empty;
			txtDocNoId.Text = string.Empty;
			txtSubNoId.Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			((Control)(object)CmbVehicleType).Text = string.Empty;
			txtPackUnit.Text = "0";
			txtPackWeight.Text = "0";
			txtGrossWeight.Text = "0";
			txtBalQty.Text = "0";
			txtBalWeight.Text = "0";
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			btnSaveAs.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			table.Rows.Clear();
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			dtExpense.Rows.Clear();
			AddRowInvExpGrid();
			GridExpense.DataSource = dtExpense;
			GridExpense.RetrieveStructure();
			grdInvExpSettings();
			dtInfoGrid.Rows.Clear();
			grdLoader.DataSource = dtInfoGrid;
			UpdateDocumentNoUI(Conversion.ToString((object)CommonServices.DeliveryOrderGenerateCode(DocumentTypeId)));
			if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 2)
			{
				if (BranchList.Rows.Count > 1)
				{
					label13.Text = "Branch From";
					label18.Visible = true;
					((Control)(object)cmbBranchTo).Visible = true;
				}
				else
				{
					label13.Text = "Branch Name";
					label18.Visible = false;
					((Control)(object)cmbBranchTo).Visible = false;
				}
			}
			cmbBranchFrom.Value = UserAccount.BranchesId;
			DeliveryScheduleCustomerDetailIds = "";
			DocDate.Focus();
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
			txtRefDocTypeId.Text = string.Empty;
			txtDocNoId.Text = string.Empty;
			txtSubNoId.Text = string.Empty;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			txtPackUnit.Text = "0";
			txtPackWeight.Text = "0";
			txtGrossWeight.Text = "0";
			txtBalQty.Text = "0";
			txtBalWeight.Text = "0";
			((Control)(object)CmbPackingType).Text = string.Empty;
			txtremarksdetail.Text = string.Empty;
			((Control)(object)CmbWareHouse).Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1)
			{
				((UltraGridBase)CmbOrderNo).DataSource = null;
				((UltraGridBase)CmbItemName).DataSource = null;
			}
			updateDetailIndex = -1;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			CmbSupplierCustomer.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryOrder_FormClosing(object sender, FormClosingEventArgs e)
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
		else
		{
			DocDate.Focus();
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
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			grdhistory.DataSource = null;
			GrdHistoryDetail.DataSource = null;
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind(HistoryComboDBCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void gridhistoryfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.DocumentTypeId = DocumentTypeId;
			reports.BranchesId = UserAccount.BranchesId;
			if (Conversion.ToString(base.Tag) == "DeliveryOrder")
			{
				reports.Activity = "Local";
			}
			else if (Conversion.ToString(base.Tag) == "DeliveryOrderTransfer")
			{
				reports.Activity = "StockTransfer";
			}
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
			reports.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dtHistoryFromDB = InvDeliveryOrder.FormHistoryNew(reports);
			if (dtHistoryFromDB.Rows.Count > 0)
			{
				if (dthistoryfill.Columns.Count == 0)
				{
					dthistoryfill.Columns.Add("Id", typeof(int));
					dthistoryfill.Columns.Add("DoType");
					dthistoryfill.Columns.Add("DocDate", typeof(DateTime));
					dthistoryfill.Columns.Add("DocNo", typeof(int));
					dthistoryfill.Columns.Add("VehicleType");
					dthistoryfill.Columns.Add("VehicleNo");
					dthistoryfill.Columns.Add("ApprovalStatus");
					dthistoryfill.Columns.Add("GpDate", typeof(DateTime));
					dthistoryfill.Columns.Add("GpNo", typeof(int));
					dthistoryfill.Columns.Add("GpStatus", typeof(string));
					dthistoryfill.Columns.Add("GdnNo", typeof(int));
					dthistoryfill.Columns.Add("FactoryWeight", typeof(double));
					dthistoryfill.Columns.Add("EntryUser");
					dthistoryfill.Columns.Add("EntryDate", typeof(DateTime));
					dthistoryfill.Columns.Add("ModifyUser");
					dthistoryfill.Columns.Add("ModifyDate", typeof(DateTime));
					dthistoryfill.Columns.Add("ApprovedUser");
					dthistoryfill.Columns.Add("ApprovedDate", typeof(DateTime));
					dthistoryfill.Columns.Add("OrderStatus");
					dthistoryfill.Columns.Add("Remarks");
					dthistoryfill.Columns.Add("NoOfAttachments");
				}
				dthistoryfill.Rows.Clear();
				HashSet<int> ids = new HashSet<int>();
				foreach (DataRow row in dtHistoryFromDB.Rows)
				{
					int id = Conversion.ToInt(row["Id"]);
					if (ids.Add(id))
					{
						dthistoryfill.Rows.Add(id, row["DeliveryOrderType"], Conversion.ToDateTime(row["DocDate"]), Conversion.ToInt(row["DocNo"]), row["VehicleType"], row["VehicleNo"], row["ApprovalStatus"], Conversion.ToDateTime(row["GpDate"]), Conversion.ToInt(row["GpNo"]), row["GpStatus"], Conversion.ToInt(row["GdnNo"]), Conversion.ToDouble(row["FactoryWeight"]), row["EntryUser"], Conversion.ToDateTime(row["EntryDate"]), row["ModifyUser"], Conversion.ToDateTime(row["ModifyDate"]), row["ApprovedUser"], Conversion.ToDateTime(row["ApprovedDate"]), row["OrderStatus"], row["HeaderRemarks"], row["NoOfAttachments"]);
					}
				}
				grdhistory.DataSource = dthistoryfill;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdhistory.ClearStructure();
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
			grdhistory.RootTable.Columns["Id"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 3, 2);
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["GpDate"].FormatString = "dd-MM-yyyy";
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GridEX_Helper.AddButton(grdhistory, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(grdhistory, "Print", "Print", 40, 1);
			GridEX_Helper.AddButton(grdhistory, "Print264", "Print 264", 70, 2);
			GridEX_Helper.AddButton(grdhistory, "SaveAs", "SaveAs", 65, 3);
			grdhistory.FrozenColumns = 4;
			GridEX_Helper.AddButton(grdhistory, "AddAttachment", "Add Attachment", 110);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
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
			if (r != null && (int)r.RowType == 0)
			{
				string status = Conversion.ToString(r.Cells["OrderStatus"].Value);
				if (status != "Open")
				{
					MessageBox.Show("Sorry You can't Update this record because Order_Status is:" + status, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Column.Key == "SaveAs")
			{
				grdhistory_Saveas(CurrId);
			}
			else if (e.Column.Key == "Edit")
			{
				string status = Conversion.ToString(grdhistory.CurrentRow.Cells["OrderStatus"].Value);
				if (status != "Open")
				{
					MessageBox.Show("Sorry You can't Update this record because Order_Status is:" + status, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				Reset();
				RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
			else if (e.Column.Key == "Print")
			{
				CommonServices.InvDeliveryOrderSlip(CurrId, DocumentTypeId);
			}
			else if (e.Column.Key == "Print264")
			{
				CommonServices.DeliveryOrderSlip264(CurrId, DocumentTypeId);
			}
			else if (e.Column.Key == "AddAttachment")
			{
				AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, CurrId, DocumentTypeId, base.Name);
				attachmentAddingFromHistory.BringToFront();
				attachmentAddingFromHistory.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_Saveas(int CurrId)
	{
		try
		{
			ReadById(CurrId);
			btnsave.Visible = false;
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			btnSaveAs.Visible = true;
			if (grd.RowCount > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["Id"].Value = 0;
					obj.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				DetailGridBind(CurrId);
			}
			else
			{
				GrdHistoryDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridBind(int CurrId)
	{
		try
		{
			InvDeliveryOrder invGrn = InvDeliveryOrder.GetByID(CurrId);
			if (invGrn == null)
			{
				GrdHistoryDetail.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = table.Clone();
			dtDetailHistory.Columns.Add("WareHouseName");
			dtDetailHistory.Columns.Add("JobLot");
			dtDetailHistory.Columns.Add("PackingType");
			FilldtDetailFromListCommonForReadById(invGrn, dtDetailHistory, AddExtraColumns: true);
			GrdHistoryDetail.DataSource = dtDetailHistory;
			GrdHistoryDetail.RetrieveStructure();
			grdHistoryDetailSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryDetailSettings()
	{
		try
		{
			grdCommonSetting(GrdHistoryDetail);
			GridEX_Helper.ComboBind(GrdHistoryDetail.RootTable.Columns["RefPartyId"], dtRefParty, "Id", "ReferencePartyName", "Reference Party");
			GrdHistoryDetail.RootTable.Columns["WareHouseId"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["CropYearId"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["JobLotId"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["OrderNo"].Visible = GridEX_Helper.IsAnyCellValueGreaterThanZero(GrdHistoryDetail, "OrderId");
			GrdHistoryDetail.RootTable.Columns["WareHouseName"].Position = GrdHistoryDetail.RootTable.Columns["WareHouseId"].Position + 1;
			GrdHistoryDetail.RootTable.Columns["CropYear"].Position = GrdHistoryDetail.RootTable.Columns["CropYearId"].Position + 1;
			GrdHistoryDetail.RootTable.Columns["JobLot"].Position = GrdHistoryDetail.RootTable.Columns["JobLotId"].Position + 1;
			GrdHistoryDetail.RootTable.Columns["PackingType"].Position = GrdHistoryDetail.RootTable.Columns["PackingTypeId"].Position + 1;
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdhistory, dthistoryfill);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void CalculateOrderBalanceQtyandWeight()
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1 && Conversion.ToInt(CmbOrderNo.Value) > 0 && ((UltraGridBase)CmbItemName).ActiveRow != null && ((UltraGridBase)CmbPackUom).ActiveRow != null && Conversion.ToInt(CmbItemName.Value) != 0 && Conversion.ToInt(CmbPackUom.Value) != 0)
			{
				double BalanceWeight = InvDeliveryOrder.OrderBalanceWeight(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(CmbOrderNo.Value),
					OrderId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["SaleOrderDetailId"].Value),
					ItemId = Conversion.ToInt(CmbItemName.Value)
				});
				if (BalanceWeight > 0.0)
				{
					txtBalWeight.Text = BalanceWeight.ToString();
					txtBalQty.Text = (BalanceWeight / Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value)).ToString();
				}
				else
				{
					txtBalWeight.Text = "";
					txtBalQty.Text = "";
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtqty.Text);
			double itemPack = ((((UltraGridBase)CmbPackUom).ActiveRow != null && Conversion.ToInt(CmbPackUom.Value) != 0) ? Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value) : 0.0);
			double weight = num * itemPack;
			txtweight.Text = weight.ToString("#,##0.###");
			txtGrossWeight.Text = weight.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateGrossWeight()
	{
		try
		{
			double weight = Conversion.ToDouble((object)txtweight.Text);
			if (weight <= 0.0)
			{
				txtGrossWeight.Text = "0";
				return;
			}
			double Qty = Conversion.ToDouble((object)txtqty.Text);
			double packingUnit = Conversion.ToDouble((object)txtPackUnit.Text);
			double PackingWeight = Conversion.ToDouble((object)txtPackWeight.Text);
			if (base.ActiveControl?.Tag?.ToString() == "PackWeight")
			{
				packingUnit = ((Qty > 0.0) ? (PackingWeight / Qty) : 0.0);
				txtPackUnit.Text = packingUnit.ToString("#,##0.#####");
			}
			else
			{
				PackingWeight = packingUnit * Qty;
				txtPackWeight.Text = PackingWeight.ToString("#,##0.###");
			}
			double GrossWeight = weight + PackingWeight;
			txtGrossWeight.Text = GrossWeight.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitempck_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			GetDataFromLoader();
			CalculateOrderBalanceQtyandWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackSize_TextChanged(object sender, EventArgs e)
	{
		CalculateGrossWeight();
	}

	private void txtweight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackUnit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.InvDeliveryOrderSlip(RecId, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			GetDataFromLoader();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		try
		{
			string OrderDetailIds = "";
			if (Conversion.ToInt((object)LoadSaleOrder.dtGDNS.Rows[0][0].ToString()) == 0)
			{
				return;
			}
			for (int i = 0; i < LoadSaleOrder.dtGDNS.Rows.Count; i++)
			{
				OrderDetailIds = OrderDetailIds + "," + Conversion.ToInt((object)LoadSaleOrder.dtGDNS.Rows[i][0].ToString());
			}
			DataTable dtSaleOrder = SaleOrder.LoadSaleOrderForDeliveryOrderInDetail(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				GdnIds = OrderDetailIds
			});
			if (dtSaleOrder.Rows.Count > 0)
			{
				HashSet<int> existingDetailIds = new HashSet<int>(from r in grd.GetRows()
					select Conversion.ToInt(r.Cells["SaleOrderDetailId"].Value));
				foreach (DataRow dr in dtSaleOrder.Rows)
				{
					int detailId = Conversion.ToInt(dr["SaleOrderDetailId"]);
					if (!existingDetailIds.Contains(detailId))
					{
						DataRow newRow = table.NewRow();
						newRow.ItemArray = new object[38]
						{
							0,
							0,
							0,
							0,
							dr["SupplierCustomerId"],
							dr["SupplierCustomer"],
							dr["RefPartyId"],
							0,
							0,
							0,
							dr["OrderId"],
							dr["OrderNo"],
							detailId,
							dr["ItemId"],
							dr["ItemCode"],
							dr["Item"],
							dr["ItemUOM"],
							dr["ItemUOMId"],
							dr["PUomEquivalent"],
							dr["CropYearId"],
							dr["CropYear"],
							dr["PackingTypeId"],
							dr["QTY"],
							dr["Weight"],
							dr["LoadQty"],
							dr["LoadWeight"],
							dr["PackingUnit"],
							dr["PackingWeight"],
							dr["GrossWeight"],
							dr["WareHouseId"],
							dr["JobLotId"],
							dr["Remarks"],
							dr["OrderItemRate"],
							dr["RateUom"],
							dr["DocumentTypeId"],
							0,
							dr["PaymentTermsId"],
							dr["PaymentTerm"]
						};
						newRow["AvailableStock"] = GetStockBytableRow(newRow);
						if (Conversion.ToInt(CmbSaleType.Value) == 2)
						{
							double qty = Conversion.ToDouble(newRow["QTY"]);
							double packEquivalent = Conversion.ToDouble(newRow["PUomEquivalent"]);
							double packWeight = Conversion.ToDouble(newRow["PackingWeight"]);
							newRow["Weight"] = qty * packEquivalent;
							newRow["LoadWeight"] = qty * packEquivalent;
							newRow["GrossWeight"] = qty * packEquivalent + packWeight;
						}
						table.Rows.Add(newRow);
					}
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				FillInformationGrid();
				GetSaleOrderIds();
				((Control)(object)CmbSaleType).Enabled = true;
			}
			else
			{
				Reset();
			}
			LoadInGridExpense(OrderDetailIds);
			OrderDetailIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridExpense(string OrderIds)
	{
		try
		{
			if (string.IsNullOrEmpty(OrderIds))
			{
				return;
			}
			DataTable dtExp = SaleOrder.SaleOrderCustomerExpensesByOrderIds(OrderIds);
			if (dtExp.Rows.Count <= 0)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < dtExp.Rows.Count; i++)
			{
				if (GridExpense.GetRows().Length != 0)
				{
					GridEXRow[] rows = GridExpense.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["SaleOrderCustomerExpId"].Value) == Conversion.ToInt(dtExp.Rows[i]["Id"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtExpense.Rows.Add(0, dtExp.Rows[i]["SaleOrderId"], dtExp.Rows[i]["Id"], dtExp.Rows[i]["SaleOrder"], dtExp.Rows[i]["InvRevExpItemId"], Conversion.ToDouble(dtExp.Rows[i]["Qty"]), Conversion.ToString(dtExp.Rows[i]["Remarks"]));
				}
				flag = false;
			}
			GridExpense.DataSource = dtExpense;
			GridExpense.RetrieveStructure();
			grdInvExpSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetSaleOrderIds()
	{
		try
		{
			dtSaleOrderIds = "";
			dtSaleOrderMainIds = "";
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				dtSaleOrderIds = dtSaleOrderIds + "," + Conversion.ToInt(item.Cells["SaleOrderDetailId"].Value);
				dtSaleOrderMainIds = dtSaleOrderMainIds + "," + Conversion.ToInt(item.Cells["OrderId"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillInformationGrid()
	{
		try
		{
			dtInfoGrid.Rows.Clear();
			if (grd.GetRows().Count() <= 0)
			{
				return;
			}
			List<int> distinctIdsForSaleOrder = (from row in table.AsEnumerable()
				where row.Field<int>("DocumentTypeId") == 81
				select row.Field<int>("OrderId")).Distinct().ToList();
			List<int> distinctIdsForBookingOrder = (from row in table.AsEnumerable()
				where row.Field<int>("DocumentTypeId") == 127
				select row.Field<int>("OrderId")).Distinct().ToList();
			if (distinctIdsForSaleOrder.Count > 0)
			{
				int j;
				for (j = 0; j < distinctIdsForSaleOrder.Count; j++)
				{
					double QtyForSaleOrder = 0.0;
					double WeightForSaleOrder = 0.0;
					double AmountForSaleOrder = 0.0;
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow item2 in rows)
					{
						if (Conversion.ToInt(item2.Cells["OrderId"].Value) == Conversion.ToInt((object)distinctIdsForSaleOrder[j]))
						{
							QtyForSaleOrder += Conversion.ToDouble(item2.Cells["LoadQty"].Value);
							WeightForSaleOrder += Conversion.ToDouble(item2.Cells["LoadWeight"].Value);
							double Amount2 = Conversion.ToDouble(item2.Cells["LoadWeight"].Value) / Conversion.ToDouble(item2.Cells["RateUom"].Value) * Conversion.ToDouble(item2.Cells["Rate"].Value);
							AmountForSaleOrder += Amount2;
						}
					}
					List<int> OrderNo2 = (from row in table.AsEnumerable()
						where row.Field<int>("OrderId") == Conversion.ToInt((object)distinctIdsForSaleOrder[j])
						select row.Field<int>("OrderNo")).ToList();
					dtInfoGrid.Rows.Add("SaleOrder", OrderNo2[0], QtyForSaleOrder, WeightForSaleOrder, AmountForSaleOrder);
				}
			}
			if (distinctIdsForBookingOrder.Count > 0)
			{
				int i;
				for (i = 0; i < distinctIdsForBookingOrder.Count; i++)
				{
					double Qty = 0.0;
					double Weight = 0.0;
					double Amount = 0.0;
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (Conversion.ToInt(item.Cells["OrderId"].Value) == Conversion.ToInt((object)distinctIdsForBookingOrder[i]))
						{
							Qty += Conversion.ToDouble(item.Cells["LoadQty"].Value);
							Weight += Conversion.ToDouble(item.Cells["LoadWeight"].Value);
							double Amt = Conversion.ToDouble(item.Cells["LoadWeight"].Value) / Conversion.ToDouble(item.Cells["RateUom"].Value) * Conversion.ToDouble(item.Cells["Rate"].Value);
							Amount += Amt;
						}
					}
					List<int> OrderNo = (from row in table.AsEnumerable()
						where row.Field<int>("OrderId") == Conversion.ToInt((object)distinctIdsForBookingOrder[i])
						select row.Field<int>("OrderNo")).ToList();
					dtInfoGrid.Rows.Add("BookingOrder", OrderNo[0], Qty, Weight, Amount);
				}
			}
			if (dtInfoGrid != null && dtInfoGrid.Rows.Count > 0)
			{
				grdLoader.DataSource = dtInfoGrid;
				grdLoader.RetrieveStructure();
				if (!ShowInfoGrid)
				{
					grdLoader.RootTable.Columns["Amount"].Visible = false;
				}
				grdLoader.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
				grdLoader.RootTable.Columns["TotalWeight"].AggregateFunction = (AggregateFunction)2;
				grdLoader.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grdLoader.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
				grdLoader.RootTable.Columns["TotalWeight"].TextAlignment = (TextAlignment)3;
				grdLoader.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grdLoader.RootTable.Columns["TotalQty"].FormatString = "#,#";
				grdLoader.RootTable.Columns["TotalWeight"].FormatString = "#,#";
				grdLoader.RootTable.Columns["Amount"].FormatString = "#,#";
				grdLoader.RootTable.Columns["TotalQty"].TotalFormatString = "#,#";
				grdLoader.RootTable.Columns["TotalWeight"].TotalFormatString = "#,#";
				grdLoader.RootTable.Columns["Amount"].TotalFormatString = "#,#";
				grdLoader.RootTable.Columns["DocumentType"].Width = 300;
				grdLoader.ColumnAutoResize = false;
			}
			else
			{
				grdLoader.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadSaleOrder_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbDeliveryOrderType).Text != "Local")
			{
				MessageBox.Show("Please Select Other DeliveryOrderType");
				return;
			}
			if (((UltraGridBase)CmbSaleType).ActiveRow == null)
			{
				CmbSaleType.Focus();
				MessageBox.Show("Please Select Sale Type First");
				return;
			}
			LoadSaleOrder = new frmLoadSaleOrder(UserAccount);
			LoadSaleOrder.documentTypeId = DocumentTypeId;
			LoadSaleOrder.MainDetail = true;
			if (dtSaleOrderIds != string.Empty)
			{
				LoadSaleOrder.orderIds = dtSaleOrderIds;
				LoadSaleOrder.orderMainIds = dtSaleOrderMainIds;
			}
			LoadSaleOrder.ShowDialog();
			LoadSaleOrder.BringToFront();
			LoadInGridDetail();
			((Control)(object)CmbDeliveryOrderType).Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDataFromLoader()
	{
	}

	private void btnLoaderView2_Click(object sender, EventArgs e)
	{
		try
		{
			LoadSaleOrder = new frmLoadSaleOrder(UserAccount);
			if (((Control)(object)CmbDeliveryOrderType).Text != "Local")
			{
				MessageBox.Show("Please Select Other DeliveryOrderType");
				return;
			}
			if (((UltraGridBase)CmbSaleType).ActiveRow == null)
			{
				CmbSaleType.Focus();
				MessageBox.Show("Please Select Sale Type First");
				return;
			}
			LoadSaleOrder.documentTypeId = DocumentTypeId;
			LoadSaleOrder.MainDetail = false;
			if (dtSaleOrderIds != string.Empty)
			{
				LoadSaleOrder.orderIds = dtSaleOrderIds;
				LoadSaleOrder.orderMainIds = dtSaleOrderMainIds;
			}
			LoadSaleOrder.ShowDialog();
			LoadSaleOrder.BringToFront();
			LoadInGridDetail();
			((Control)(object)CmbDeliveryOrderType).Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineReferenceParties_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineReferenceParties(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenLinkedFormOnInsert(int DoNo)
	{
		try
		{
			if (RadGpOutwardToOpenOnInsert.Checked)
			{
				GpOutwardForm(DoNo);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GpOutwardForm(int DoNo)
	{
		try
		{
			FormHelper.CanOpenForm("OutwardGatePass", delegate
			{
				OutwardGatePass outwardGatePass = new OutwardGatePass(UserAccount);
				outwardGatePass.Show();
				if (DoNo > 0)
				{
					outwardGatePass.CmbGPBasedOn.Value = DocumentTypeId;
					outwardGatePass.CmbOrderno.Text = DoNo.ToString();
					outwardGatePass.CmbOrderno_Leave(null, null);
				}
				outwardGatePass.BringToFront();
				bool formClosed = false;
				outwardGatePass.FormClosed += delegate
				{
					formClosed = true;
				};
				while (!formClosed)
				{
					Application.DoEvents();
				}
			}, UserAccount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void labelGpOutwardForm_Click(object sender, EventArgs e)
	{
		FormHelper.CanOpenForm("OutwardGatePass", delegate
		{
			OutwardGatePass outwardGatePass = new OutwardGatePass(UserAccount);
			outwardGatePass.Show();
			outwardGatePass.BringToFront();
		}, UserAccount);
	}

	private void SaleOrderBind()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1)
			{
				DataTable dt = SaleOrder.SaleOrderIdandNoGetForSupplierCustomerId(new SaleOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					OrderSupCustId = Conversion.ToInt(CmbSupplierCustomer.Value),
					Id = RecId
				});
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbOrderNo, "SaleOrderId", "SaleOrderNo", "SaleOrderNo", ZeroIndex: false);
					return;
				}
				((Control)(object)CmbOrderNo).Text = string.Empty;
				((UltraGridBase)CmbOrderNo).DataSource = null;
			}
			else
			{
				((Control)(object)CmbOrderNo).Text = string.Empty;
				((UltraGridBase)CmbOrderNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplierCustomer_TextChanged(object sender, EventArgs e)
	{
		SaleOrderBind();
	}

	private void ItemBindbyOrderIdDbCall()
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			dtItem.Clear();
			if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 2)
			{
				((Control)(object)CmbItemName).Text = string.Empty;
				((UltraGridBase)CmbItemName).DataSource = null;
				ItemDtFillFromGlobal();
			}
			else if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1)
			{
				DataTable dt = SaleOrder.ReadByPurchaseOrderIDNOrderItemId(new SaleOrder
				{
					Id = Conversion.ToInt(CmbOrderNo.Value)
				});
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtItem.Rows.Add(dt.Rows[i]["OrderItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"], dt.Rows[i]["Id"], dt.Rows[i]["OrderItemRate"], dt.Rows[i]["RateUom"], dt.Rows[i]["DocumentTypeId"]);
					}
				}
			}
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderNo_TextChanged(object sender, EventArgs e)
	{
		ItemBindbyOrderIdDbCall();
		CalculateOrderBalanceQtyandWeight();
	}

	private void grdLoader_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			_ = e.Column.Key == "Load";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryOrderType_Leave(object sender, EventArgs e)
	{
		if (Conversion.ToInt(CmbDeliveryOrderType.Value) == 1)
		{
			if (ShowInfoGrid)
			{
				DetailGridPanel.Dock = DockStyle.Fill;
				LoaderInformationPanel.Show();
				((Control)(object)grdLoader).Show();
			}
			else
			{
				DetailGridPanel.Dock = DockStyle.Fill;
				LoaderInformationPanel.Hide();
				((Control)(object)grdLoader).Hide();
			}
			label13.Text = "Branch Name";
			label18.Visible = false;
			((Control)(object)cmbBranchTo).Visible = false;
		}
		else
		{
			ItemDtFillFromGlobal();
			CmbWareHouse.ReadOnly = false;
			txtweight.ReadOnly = true;
			label13.Text = "Branch From";
			label18.Visible = true;
			((Control)(object)cmbBranchTo).Visible = true;
			DetailGridPanel.Dock = DockStyle.Fill;
			LoaderInformationPanel.Hide();
			((Control)(object)grdLoader).Hide();
		}
		ItemNameBind();
	}

	private void CmbSupplierCustomer_Leave(object sender, EventArgs e)
	{
		SaleOrderBind();
	}

	private void CmbOrderNo_Leave(object sender, EventArgs e)
	{
		ItemBindbyOrderIdDbCall();
		CalculateOrderBalanceQtyandWeight();
	}

	private void UpdateAvailableStockInGridDetailRow(GridEXRow r)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		try
		{
			r.BeginEdit();
			double AvailableStock = InventoryStockEvalautionDetail.GetCurrentStockByItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocDate = DocDate.Value,
				WarehouseId = Conversion.ToInt((object)r.Cells["WarehouseId"].Value.ToString()),
				ItemId = Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()),
				CropYearId = Conversion.ToInt((object)r.Cells["CropYearId"].Value.ToString()),
				CropYear = Conversion.ToString((object)r.Cells["CropYearId"].Text.ToString()),
				JobLotId = Conversion.ToInt((object)r.Cells["JobLotId"].Value.ToString()),
				PackingTypeId = Conversion.ToInt((object)r.Cells["PackingTypeId"].Value.ToString()),
				stockUOM = Conversion.ToInt((object)r.Cells["ItemUOMId"].Value.ToString())
			});
			r.Cells["AvailableStock"].Value = AvailableStock;
			r.BeginEdit();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				UpdateAvailableStockInGridDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWareHouse_TextChanged(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void CmbCropYear_Leave(object sender, EventArgs e)
	{
		try
		{
			GetDataFromLoader();
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCropYear_TextChanged(object sender, EventArgs e)
	{
		GetDataFromLoader();
		AvailableStockGetByItem();
	}

	private void CmbJobLot_Leave(object sender, EventArgs e)
	{
		try
		{
			GetDataFromLoader();
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbJobLot_TextChanged(object sender, EventArgs e)
	{
		GetDataFromLoader();
		AvailableStockGetByItem();
	}

	private void CmbPackingType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetDataFromLoader();
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackingType_Leave(object sender, EventArgs e)
	{
		try
		{
			GetDataFromLoader();
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackUom_TextChanged(object sender, EventArgs e)
	{
		GetDataFromLoader();
		AvailableStockGetByItem();
		CalculateOrderBalanceQtyandWeight();
	}

	private void AvailableStockGetByItem()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		try
		{
			double CurrStock = InvDeliveryOrder.GetAvailableStockForDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = Conversion.ToInt(CmbWareHouse.Value),
				ItemId = Conversion.ToInt(CmbItemName.Value),
				JobLotId = Conversion.ToInt(CmbJobLot.Value),
				CropYear = Conversion.ToString((object)((Control)(object)CmbCropYear).Text.Trim()),
				DocDate = Conversion.ToDateTime((object)DocDate.Value),
				PackingTypeId = Conversion.ToInt(CmbPackingType.Value),
				stockUOM = Conversion.ToInt(CmbPackUom.Value)
			});
			if (CurrStock > 0.0)
			{
				lblBalance.Text = CurrStock.ToString();
			}
			else
			{
				lblBalance.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtVehicleNo_TextChanged(object sender, EventArgs e)
	{
		txtVehicleNo.SelectionStart = txtVehicleNo.Text.Length;
		txtVehicleNo.Text = txtVehicleNo.Text.ToUpper();
	}

	private void CmbSaleType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbSaleType.Value) == 2)
			{
				txtweight.Enabled = false;
			}
			else
			{
				txtweight.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
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
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void txtweight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackUnit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void PurchsaeOrder_KeyDown(object sender, KeyEventArgs e)
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
					DocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
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
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && btnDelete.Visible && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F1)
				{
					btnLoadSaleOrder_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F2)
				{
					btnLoaderView2_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F3)
				{
					btnDefineReferenceParties_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
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
					CmbSupplierCustomer.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
				{
					btnprint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formright.DoHavePrintRights)
				{
					btnprint_Click(null, null);
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnResetHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdhistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDateHistory.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save in Form Tab and For Show History in History Tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print 262");
			dt.Rows.Add("Alt+1", "For Print 262");
			dt.Rows.Add("Ctrl+F1", "For Loader SO Main Detail");
			dt.Rows.Add("Ctrl+F2", "For Loader SO Main");
			dt.Rows.Add("Ctrl+F5", "For Reference Parties Look up");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+F12", "For SaveAs");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Item in Detail Grid when in Form tab and For focus on FromDate in history tab");
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

	private void GridExpense_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GridExpense.CurrentRow;
			if (r == null || (int)r.RowType != 0 || GridExpense.CurrentColumn == null)
			{
				return;
			}
			if (GridExpense.CurrentColumn.Key == "ItemId" && e.KeyCode == Keys.F1)
			{
				GrdPopUp obj = new GrdPopUp(dtOtherItem, "Id", "OtherItemName");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				r.Cells["ItemId"].Value = obj.ReturnId;
				GridExpense.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (GridExpense.CurrentColumn.Key == "Delete")
				{
					DeleteExpenseDetailRow(r);
				}
				else if (GridExpense.CurrentColumn.Key == "Add")
				{
					AddRowInvExpGrid();
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvExpGrid();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteExpenseDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "Add")
				{
					AddDetailRow(r);
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddDetailRow(r);
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
				string status = Conversion.ToString(item.Cells["OrderStatus"].Value);
				if (status != "Open")
				{
					MessageBox.Show("Sorry You can't Update this record because Order_Status is:" + status, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					ReadById(CuurID);
				}
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (grdhistory.CurrentColumn.Key == "Edit")
				{
					string status2 = Conversion.ToString(item.Cells["OrderStatus"].Value);
					if (status2 != "Open")
					{
						MessageBox.Show("Sorry You can't Update this record because Order_Status is:" + status2, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return;
					}
					ReadById(CuurID);
				}
				if (grdhistory.CurrentColumn.Key == "SaveAs")
				{
					grdhistory_Saveas(CuurID);
				}
				if (grdhistory.CurrentColumn.Key == "Print")
				{
					CommonServices.InvDeliveryOrderSlip(CuurID, DocumentTypeId);
				}
				if (grdhistory.CurrentColumn.Key == "Print264")
				{
					CommonServices.DeliveryOrderSlip264(CuurID, DocumentTypeId);
				}
				if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CuurID, DocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadCustomerSchedule_Click(object sender, EventArgs e)
	{
		try
		{
			if (!string.Equals(((Control)(object)CmbDeliveryOrderType).Text?.Trim(), "Local", StringComparison.OrdinalIgnoreCase))
			{
				MessageBox.Show("Please select 'Local' as Delivery Order Type.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (((UltraGridBase)CmbSaleType).ActiveRow == null || Conversion.ToInt(CmbSaleType.Value) == 0)
			{
				MessageBox.Show("Please select a Sale Type first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				CmbSaleType.Focus();
				return;
			}
			LoadCustomerSchedule = new frmDeliveryScheduleCustomer(UserAccount);
			if (!string.IsNullOrEmpty(DeliveryScheduleCustomerDetailIds))
			{
				LoadCustomerSchedule.DeliveryScheduleCustomerDetailIds = DeliveryScheduleCustomerDetailIds;
			}
			LoadCustomerSchedule.ShowDialog();
			LoadCustomerSchedule.BringToFront();
			LoadInGridDetailFromDeliveryScheduleCustomer(LoadCustomerSchedule.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void LoadInGridDetailFromDeliveryScheduleCustomer(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			HashSet<int> existingDetailIds = new HashSet<int>(from r in grd.GetRows()
				select Conversion.ToInt(r.Cells["DeliveryScheduleCustomerDetailId"].Value));
			foreach (DataRow dr in dt.Rows)
			{
				int detailId = Conversion.ToInt(dr["deliveryScheduleDetailId"]);
				if (!existingDetailIds.Contains(detailId))
				{
					DataRow newRow = table.NewRow();
					newRow.ItemArray = new object[36]
					{
						0,
						0,
						0,
						0,
						dr["SupplierCustomerId"],
						dr["CustomerName"],
						0,
						dr["Id"],
						dr["DocNo"],
						detailId,
						dr["SaleOrderId"],
						dr["SaleOrderNo"],
						dr["SaleOrderDetailId"],
						dr["ItemId"],
						dr.Table.Columns.Contains("ItemCode") ? dr["ItemCode"] : "",
						dr["ItemName"],
						dr["PackUom"],
						dr["ItemUOMId"],
						dr["PackEquivalent"],
						dr["CropYearId"],
						dr["CropYear"],
						dr["InvPackingTypeId"],
						dr["ItemQty"],
						dr["NetWeight"],
						dr["ItemQty"],
						dr["NetWeight"],
						0,
						0,
						dr["NetWeight"],
						0,
						dr["JobLotId"],
						dr["RemarksHeader"],
						dr["ItemRate"],
						dr["RateUom"],
						dr["OrderDocumentTypeId"],
						0
					};
					newRow["AvailableStock"] = GetStockBytableRow(newRow);
					if (Conversion.ToInt(CmbSaleType.Value) == 2)
					{
						double qty = Conversion.ToDouble(newRow["QTY"]);
						double packEquivalent = Conversion.ToDouble(newRow["PUomEquivalent"]);
						double packWeight = Conversion.ToDouble(newRow["PackingWeight"]);
						newRow["Weight"] = qty * packEquivalent;
						newRow["LoadWeight"] = qty * packEquivalent;
						newRow["GrossWeight"] = qty * packEquivalent + packWeight;
					}
					table.Rows.Add(newRow);
				}
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			((Control)(object)CmbDeliveryOrderType).Enabled = false;
			ScheduleExist = true;
			FillInformationGrid();
			GetDeliveryScheduleDetailIds();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDeliveryScheduleDetailIds()
	{
		try
		{
			DeliveryScheduleCustomerDetailIds = "";
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				DeliveryScheduleCustomerDetailIds = DeliveryScheduleCustomerDetailIds + "," + Conversion.ToInt(item.Cells["DeliveryScheduleCustomerDetailId"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private double GetStockBytableRow(DataRow dr)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		try
		{
			return InvDeliveryOrder.GetAvailableStockForDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = Conversion.ToInt(dr["WareHouseId"]),
				ItemId = Conversion.ToInt(dr["ItemId"]),
				JobLotId = Conversion.ToInt(dr["JobLotId"]),
				CropYear = Conversion.ToString(dr["CropYear"]),
				DocDate = Conversion.ToDateTime((object)DocDate.Value),
				PackingTypeId = Conversion.ToInt(dr["PackingTypeId"]),
				stockUOM = Conversion.ToInt(dr["ItemUOMId"])
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			return 0.0;
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
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Expected O, but got Unknown
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_0608: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Expected O, but got Unknown
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Expected O, but got Unknown
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Expected O, but got Unknown
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Expected O, but got Unknown
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Expected O, but got Unknown
		//IL_0752: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Expected O, but got Unknown
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Expected O, but got Unknown
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Expected O, but got Unknown
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07eb: Expected O, but got Unknown
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_0961: Expected O, but got Unknown
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Expected O, but got Unknown
		//IL_0a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a32: Expected O, but got Unknown
		//IL_1170: Unknown result type (might be due to invalid IL or missing references)
		//IL_117a: Expected O, but got Unknown
		//IL_1187: Unknown result type (might be due to invalid IL or missing references)
		//IL_1191: Expected O, but got Unknown
		//IL_151e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1528: Expected O, but got Unknown
		//IL_17c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cb: Expected O, but got Unknown
		//IL_7e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e9a: Expected O, but got Unknown
		//IL_7ebe: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ec8: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridExpense_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdLoader_DesignTimeLayout = new GridEXLayout();
		Appearance appearance35 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
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
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
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
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
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
		Appearance appearance123 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Sale.DeliveryOrder));
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.DetailGridPanel = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.GridExpense = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.LoaderInformationPanel = new System.Windows.Forms.Panel();
		this.grdLoader = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtPackUnit = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.lblBalance = new System.Windows.Forms.Label();
		this.txtBalWeight = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtBalQty = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbOrderNo = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbCropYear = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtPackWeight = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbSupplierCustomer = new UltraCombo();
		this.txtweight = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label38 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.CmbPackingType = new UltraCombo();
		this.CmbPackUom = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbSaleType = new UltraCombo();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.CmbVehicleType = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.cmbBranchFrom = new UltraCombo();
		this.cmbBranchTo = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbDeliveryOrderType = new UltraCombo();
		this.txtSubNoId = new System.Windows.Forms.TextBox();
		this.txtDocNoId = new System.Windows.Forms.TextBox();
		this.txtRefDocTypeId = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ChkIsStockReservedPerParty = new System.Windows.Forms.CheckBox();
		this.labelGpOutwardForm = new System.Windows.Forms.LinkLabel();
		this.RadGpOutwardToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.RadNoneFormToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkPrint = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoadSaleOrder = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoaderView2 = new System.Windows.Forms.ToolStripButton();
		this.btnDefineReferenceParties = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLoadCustomerSchedule = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.GrdHistoryDetail = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel13.SuspendLayout();
		this.DetailGridPanel.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridExpense).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdLoader).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSaleType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryOrderType).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel12.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1411, 661);
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
		this.tabControl1.Size = new System.Drawing.Size(1411, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel13);
		this.tabPage1.Controls.Add(this.LoaderInformationPanel);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdLoader);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1403, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add(this.DetailGridPanel);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(2, 276);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1399, 199);
		this.panel13.TabIndex = 9;
		this.DetailGridPanel.Controls.Add(this.tabPage3);
		this.DetailGridPanel.Controls.Add(this.tabPage4);
		this.DetailGridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.DetailGridPanel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DetailGridPanel.Location = new System.Drawing.Point(0, 0);
		this.DetailGridPanel.Name = "DetailGridPanel";
		this.DetailGridPanel.SelectedIndex = 0;
		this.DetailGridPanel.Size = new System.Drawing.Size(1399, 199);
		this.DetailGridPanel.TabIndex = 6;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1391, 170);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Do Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grd.AutomaticSort = false;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 26);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1385, 141);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 3;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label15);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1385, 23);
		this.panel5.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1345, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(6, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(53, 18);
		this.label15.TabIndex = 0;
		this.label15.Text = "Detail";
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.GridExpense);
		this.tabPage4.Controls.Add(this.panel10);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1391, 170);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Expense Grid";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.GridExpense.AutomaticSort = false;
		((ScrollableBase)this.GridExpense).BackColor = System.Drawing.Color.White;
		this.GridExpense.ColumnAutoResize = true;
		GridExpense_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridExpense.DesignTimeLayout = GridExpense_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridExpense).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GridExpense).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridExpense.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridExpense).Location = new System.Drawing.Point(3, 26);
		((System.Windows.Forms.Control)(object)this.GridExpense).Name = "GridExpense";
		((ScrollableBase)this.GridExpense).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridExpense).Size = new System.Drawing.Size(1385, 141);
		((System.Windows.Forms.Control)(object)this.GridExpense).TabIndex = 4;
		this.GridExpense.TotalRow = (InheritableBoolean)1;
		this.GridExpense.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridExpense).VisualStyle = (VisualStyle)4;
		this.GridExpense.ColumnButtonClick += new ColumnActionEventHandler(GridExpense_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.GridExpense).KeyDown += new System.Windows.Forms.KeyEventHandler(GridExpense_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel10.Controls.Add(this.label17);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1385, 23);
		this.panel10.TabIndex = 3;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(5, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(70, 18);
		this.label17.TabIndex = 0;
		this.label17.Text = "Expense";
		this.LoaderInformationPanel.BackColor = System.Drawing.Color.Teal;
		this.LoaderInformationPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.LoaderInformationPanel.Location = new System.Drawing.Point(2, 475);
		this.LoaderInformationPanel.Name = "LoaderInformationPanel";
		this.LoaderInformationPanel.Size = new System.Drawing.Size(1399, 23);
		this.LoaderInformationPanel.TabIndex = 8;
		this.grdLoader.AllowEdit = (InheritableBoolean)2;
		this.grdLoader.AutomaticSort = false;
		((ScrollableBase)this.grdLoader).BackColor = System.Drawing.Color.White;
		this.grdLoader.ColumnAutoResize = true;
		grdLoader_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdLoader.DesignTimeLayout = grdLoader_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdLoader).Dock = System.Windows.Forms.DockStyle.Bottom;
		((System.Windows.Forms.Control)(object)this.grdLoader).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdLoader.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdLoader).Location = new System.Drawing.Point(2, 498);
		((System.Windows.Forms.Control)(object)this.grdLoader).Name = "grdLoader";
		((ScrollableBase)this.grdLoader).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdLoader).Size = new System.Drawing.Size(1399, 132);
		((System.Windows.Forms.Control)(object)this.grdLoader).TabIndex = 0;
		this.grdLoader.TotalRow = (InheritableBoolean)1;
		this.grdLoader.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdLoader).VisualStyle = (VisualStyle)4;
		this.grdLoader.ColumnButtonClick += new ColumnActionEventHandler(grdLoader_ColumnButtonClick);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtSubNoId);
		this.panel4.Controls.Add(this.txtDocNoId);
		this.panel4.Controls.Add(this.txtRefDocTypeId);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 49);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1399, 227);
		this.panel4.TabIndex = 2;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.txtPackUnit);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.rdSearchByCode);
		this.groupBox3.Controls.Add(this.rdSearchByName);
		this.groupBox3.Controls.Add(this.lblBalance);
		this.groupBox3.Controls.Add(this.txtBalWeight);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add(this.txtBalQty);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNo);
		this.groupBox3.Controls.Add(this.label9);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.txtGrossWeight);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.txtPackWeight);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer);
		this.groupBox3.Controls.Add(this.txtweight);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.txtqty);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.txtremarksdetail);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(4, 109);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1146, 118);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.txtPackUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackUnit.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackUnit.Location = new System.Drawing.Point(322, 88);
		this.txtPackUnit.Name = "txtPackUnit";
		this.txtPackUnit.Size = new System.Drawing.Size(85, 25);
		this.txtPackUnit.TabIndex = 12;
		this.txtPackUnit.Tag = "PackUnit";
		this.txtPackUnit.TextChanged += new System.EventHandler(txtPackUnit_TextChanged);
		this.txtPackUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackUnit_KeyPress);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(323, 68);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(74, 15);
		this.label20.TabIndex = 56;
		this.label20.Text = "Packing Unit";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(597, 14);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 54;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(538, 14);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 53;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalance.Location = new System.Drawing.Point(679, 16);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(0, 15);
		this.lblBalance.TabIndex = 46;
		this.txtBalWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalWeight.Location = new System.Drawing.Point(232, 88);
		this.txtBalWeight.Name = "txtBalWeight";
		this.txtBalWeight.ReadOnly = true;
		this.txtBalWeight.Size = new System.Drawing.Size(89, 25);
		this.txtBalWeight.TabIndex = 11;
		this.txtBalWeight.TabStop = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(233, 69);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(62, 15);
		this.label11.TabIndex = 45;
		this.label11.Text = "BalWeight";
		this.txtBalQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalQty.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalQty.Location = new System.Drawing.Point(156, 88);
		this.txtBalQty.Name = "txtBalQty";
		this.txtBalQty.ReadOnly = true;
		this.txtBalQty.Size = new System.Drawing.Size(75, 25);
		this.txtBalQty.TabIndex = 10;
		this.txtBalQty.TabStop = false;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(156, 67);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(42, 15);
		this.label10.TabIndex = 45;
		this.label10.Text = "BalQty";
		this.CmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance79;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance85).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance153).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Location = new System.Drawing.Point(232, 36);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Name = "CmbOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Size = new System.Drawing.Size(89, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TextChanged += new System.EventHandler(CmbOrderNo_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Leave += new System.EventHandler(CmbOrderNo_Leave);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(234, 16);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(53, 15);
		this.label9.TabIndex = 43;
		this.label9.Text = "OrderNo";
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance152;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(705, 36);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TextChanged += new System.EventHandler(CmbCropYear_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Leave += new System.EventHandler(CmbCropYear_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(706, 16);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(54, 15);
		this.label7.TabIndex = 41;
		this.label7.Text = "CropYear";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(491, 88);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.ReadOnly = true;
		this.txtGrossWeight.Size = new System.Drawing.Size(122, 25);
		this.txtGrossWeight.TabIndex = 14;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(493, 67);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(75, 15);
		this.label6.TabIndex = 39;
		this.label6.Text = "GrossWeight";
		this.txtPackWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackWeight.Location = new System.Drawing.Point(408, 88);
		this.txtPackWeight.Name = "txtPackWeight";
		this.txtPackWeight.Size = new System.Drawing.Size(82, 25);
		this.txtPackWeight.TabIndex = 13;
		this.txtPackWeight.Tag = "PackWeight";
		this.txtPackWeight.TextChanged += new System.EventHandler(txtPackSize_TextChanged);
		this.txtPackWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackWeight_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(403, 68);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(91, 15);
		this.label1.TabIndex = 37;
		this.label1.Text = "Packing Weight";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(800, 36);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TextChanged += new System.EventHandler(CmbJobLot_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Leave += new System.EventHandler(CmbJobLot_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(800, 16);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(43, 15);
		this.label8.TabIndex = 35;
		this.label8.Text = "JobLot";
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance91).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(322, 36);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TextChanged += new System.EventHandler(CmbWareHouse_TextChanged);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(324, 16);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(69, 15);
		this.label3.TabIndex = 33;
		this.label3.Text = "WareHouse";
		this.CmbSupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Location = new System.Drawing.Point(10, 36);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Name = "CmbSupplierCustomer";
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).TextChanged += new System.EventHandler(CmbSupplierCustomer_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Leave += new System.EventHandler(CmbSupplierCustomer_Leave);
		this.txtweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtweight.Location = new System.Drawing.Point(75, 88);
		this.txtweight.Name = "txtweight";
		this.txtweight.Size = new System.Drawing.Size(79, 25);
		this.txtweight.TabIndex = 9;
		this.txtweight.TextChanged += new System.EventHandler(txtweight_TextChanged);
		this.txtweight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtweight_KeyPress);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(75, 66);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(46, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Weight";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(9, 16);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 31;
		this.label2.Text = "Customer Name";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(10, 88);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(64, 25);
		this.txtqty.TabIndex = 8;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1073, 88);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 25);
		this.btnCancelUpdateDetial.TabIndex = 18;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(10, 66);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1003, 88);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(68, 25);
		this.btnUpdateDetail.TabIndex = 17;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(1027, 36);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(103, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TextChanged += new System.EventHandler(CmbPackingType_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Leave += new System.EventHandler(CmbPackingType_Leave);
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance139;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance144).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(951, 36);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(75, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(CmbPackUom_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(combitempck_Leave);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance99;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance118).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(504, 36);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 3;
		this.CmbItemName.ValueChanged += new System.EventHandler(combitem_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(962, 88);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 25);
		this.btnplus.TabIndex = 16;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksdetail.Location = new System.Drawing.Point(615, 88);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(340, 25);
		this.txtremarksdetail.TabIndex = 15;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(615, 68);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(65, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Comments";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(1027, 16);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(74, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "PackingType";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(951, 16);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(63, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack UOM";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(505, 16);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(32, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSaleType);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleType);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchFrom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchTo);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(4, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1146, 107);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.CmbSaleType.AllowNull = (DefaultableBoolean)1;
		this.CmbSaleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSaleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSaleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSaleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.CmbSaleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSaleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSaleType).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSaleType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSaleType).Location = new System.Drawing.Point(359, 45);
		((System.Windows.Forms.Control)(object)this.CmbSaleType).Name = "CmbSaleType";
		((System.Windows.Forms.Control)(object)this.CmbSaleType).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbSaleType).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbSaleType).Leave += new System.EventHandler(CmbSaleType_Leave);
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(95, 19);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(59, 23);
		this.txtdocno.TabIndex = 0;
		this.CmbVehicleType.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVehicleType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Location = new System.Drawing.Point(359, 17);
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Name = "CmbVehicleType";
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Size = new System.Drawing.Size(131, 26);
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).TabIndex = 4;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(4, 23);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(290, 15);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(64, 32);
		this.label14.TabIndex = 50;
		this.label14.Text = "Vehicle Type && No";
		this.label33.BackColor = System.Drawing.Color.LightGray;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(595, 17);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(535, 26);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(290, 51);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(57, 15);
		this.label12.TabIndex = 50;
		this.label12.Text = "Sale Type";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(157, 19);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(130, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.txtVehicleNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(492, 19);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(95, 22);
		this.txtVehicleNo.TabIndex = 5;
		this.txtVehicleNo.TextChanged += new System.EventHandler(txtVehicleNo_TextChanged);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(290, 79);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(60, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Branch To";
		this.label18.Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(4, 79);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(44, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch";
		this.cmbBranchFrom.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance17;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBranchFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Location = new System.Drawing.Point(95, 73);
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Name = "cmbBranchFrom";
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).TabStop = false;
		this.cmbBranchTo.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance102;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBranchTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Location = new System.Drawing.Point(359, 73);
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Name = "cmbBranchTo";
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Visible = false;
		this.txtremarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(595, 48);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(535, 51);
		this.txtremarks.TabIndex = 8;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(4, 51);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(53, 15);
		this.label4.TabIndex = 27;
		this.label4.Text = "DO Type";
		this.CmbDeliveryOrderType.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryOrderType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryOrderType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryOrderType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryOrderType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Location = new System.Drawing.Point(95, 45);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Name = "CmbDeliveryOrderType";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Leave += new System.EventHandler(CmbDeliveryOrderType_Leave);
		this.txtSubNoId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSubNoId.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSubNoId.Location = new System.Drawing.Point(1160, 10);
		this.txtSubNoId.Name = "txtSubNoId";
		this.txtSubNoId.ReadOnly = true;
		this.txtSubNoId.Size = new System.Drawing.Size(39, 25);
		this.txtSubNoId.TabIndex = 48;
		this.txtSubNoId.TabStop = false;
		this.txtSubNoId.Visible = false;
		this.txtDocNoId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoId.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoId.Location = new System.Drawing.Point(1160, 41);
		this.txtDocNoId.Name = "txtDocNoId";
		this.txtDocNoId.ReadOnly = true;
		this.txtDocNoId.Size = new System.Drawing.Size(39, 25);
		this.txtDocNoId.TabIndex = 47;
		this.txtDocNoId.TabStop = false;
		this.txtDocNoId.Visible = false;
		this.txtRefDocTypeId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefDocTypeId.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRefDocTypeId.Location = new System.Drawing.Point(1160, 72);
		this.txtRefDocTypeId.Name = "txtRefDocTypeId";
		this.txtRefDocTypeId.ReadOnly = true;
		this.txtRefDocTypeId.Size = new System.Drawing.Size(39, 25);
		this.txtRefDocTypeId.TabIndex = 46;
		this.txtRefDocTypeId.TabStop = false;
		this.txtRefDocTypeId.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.ChkIsStockReservedPerParty);
		this.panel3.Controls.Add(this.labelGpOutwardForm);
		this.panel3.Controls.Add(this.RadGpOutwardToOpenOnInsert);
		this.panel3.Controls.Add(this.RadNoneFormToOpenOnInsert);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkPrint);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1399, 22);
		this.panel3.TabIndex = 1;
		this.ChkIsStockReservedPerParty.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkIsStockReservedPerParty.AutoSize = true;
		this.ChkIsStockReservedPerParty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.ChkIsStockReservedPerParty.ForeColor = System.Drawing.Color.White;
		this.ChkIsStockReservedPerParty.Location = new System.Drawing.Point(1148, 1);
		this.ChkIsStockReservedPerParty.Name = "ChkIsStockReservedPerParty";
		this.ChkIsStockReservedPerParty.Size = new System.Drawing.Size(138, 20);
		this.ChkIsStockReservedPerParty.TabIndex = 38;
		this.ChkIsStockReservedPerParty.Text = "Stock Reserved";
		this.ChkIsStockReservedPerParty.UseVisualStyleBackColor = true;
		this.labelGpOutwardForm.AutoSize = true;
		this.labelGpOutwardForm.BackColor = System.Drawing.Color.Teal;
		this.labelGpOutwardForm.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.labelGpOutwardForm.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.labelGpOutwardForm.Location = new System.Drawing.Point(407, 4);
		this.labelGpOutwardForm.Name = "labelGpOutwardForm";
		this.labelGpOutwardForm.Size = new System.Drawing.Size(135, 14);
		this.labelGpOutwardForm.TabIndex = 34;
		this.labelGpOutwardForm.TabStop = true;
		this.labelGpOutwardForm.Text = "Outward Gate Pass";
		this.labelGpOutwardForm.Click += new System.EventHandler(labelGpOutwardForm_Click);
		this.RadGpOutwardToOpenOnInsert.AutoSize = true;
		this.RadGpOutwardToOpenOnInsert.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadGpOutwardToOpenOnInsert.ForeColor = System.Drawing.Color.White;
		this.RadGpOutwardToOpenOnInsert.Location = new System.Drawing.Point(392, 5);
		this.RadGpOutwardToOpenOnInsert.Name = "RadGpOutwardToOpenOnInsert";
		this.RadGpOutwardToOpenOnInsert.Size = new System.Drawing.Size(14, 13);
		this.RadGpOutwardToOpenOnInsert.TabIndex = 35;
		this.RadGpOutwardToOpenOnInsert.UseVisualStyleBackColor = true;
		this.RadNoneFormToOpenOnInsert.AutoSize = true;
		this.RadNoneFormToOpenOnInsert.Checked = true;
		this.RadNoneFormToOpenOnInsert.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNoneFormToOpenOnInsert.ForeColor = System.Drawing.Color.White;
		this.RadNoneFormToOpenOnInsert.Location = new System.Drawing.Point(324, 2);
		this.RadNoneFormToOpenOnInsert.Name = "RadNoneFormToOpenOnInsert";
		this.RadNoneFormToOpenOnInsert.Size = new System.Drawing.Size(59, 18);
		this.RadNoneFormToOpenOnInsert.TabIndex = 33;
		this.RadNoneFormToOpenOnInsert.TabStop = true;
		this.RadNoneFormToOpenOnInsert.Text = "None";
		this.RadNoneFormToOpenOnInsert.UseVisualStyleBackColor = true;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(2, 1);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(127, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Delivery Order";
		this.ChkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.ForeColor = System.Drawing.Color.White;
		this.ChkPrint.Location = new System.Drawing.Point(1295, 1);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(102, 20);
		this.ChkPrint.TabIndex = 8;
		this.ChkPrint.Text = "Is Preview";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1399, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[19]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator6, this.btnupdate, this.btnattachment, this.btnSaveAs, this.btnsave, this.btnDelete, this.toolStripSeparator5, this.toolStripSeparator4,
			this.btnprint, this.toolStripSeparator2, this.btnLoadSaleOrder, this.toolStripSeparator3, this.btnLoaderView2, this.btnDefineReferenceParties, this.toolStripSeparator1, this.BtnLoadCustomerSchedule, this.BtnShortCutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1399, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnprint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "262-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnLoadSaleOrder.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadSaleOrder.Image = (System.Drawing.Image)resources.GetObject("btnLoadSaleOrder.Image");
		this.btnLoadSaleOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadSaleOrder.Name = "btnLoadSaleOrder";
		this.btnLoadSaleOrder.Size = new System.Drawing.Size(164, 24);
		this.btnLoadSaleOrder.Text = "Load_SO_MainDetail";
		this.btnLoadSaleOrder.Click += new System.EventHandler(btnLoadSaleOrder_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnLoaderView2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoaderView2.Image = (System.Drawing.Image)resources.GetObject("btnLoaderView2.Image");
		this.btnLoaderView2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoaderView2.Name = "btnLoaderView2";
		this.btnLoaderView2.Size = new System.Drawing.Size(127, 24);
		this.btnLoaderView2.Text = "Load_SO_Main";
		this.btnLoaderView2.Click += new System.EventHandler(btnLoaderView2_Click);
		this.btnDefineReferenceParties.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDefineReferenceParties.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.btnDefineReferenceParties.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineReferenceParties.Name = "btnDefineReferenceParties";
		this.btnDefineReferenceParties.Size = new System.Drawing.Size(147, 24);
		this.btnDefineReferenceParties.Text = "Reference Parties";
		this.btnDefineReferenceParties.Click += new System.EventHandler(btnDefineReferenceParties_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnLoadCustomerSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadCustomerSchedule.Image = (System.Drawing.Image)resources.GetObject("BtnLoadCustomerSchedule.Image");
		this.BtnLoadCustomerSchedule.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadCustomerSchedule.Name = "BtnLoadCustomerSchedule";
		this.BtnLoadCustomerSchedule.Size = new System.Drawing.Size(200, 24);
		this.BtnLoadCustomerSchedule.Text = "Load_Customer_Schedule";
		this.BtnLoadCustomerSchedule.Click += new System.EventHandler(BtnLoadCustomerSchedule_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1403, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(2, 149);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1399, 251);
		this.panel9.TabIndex = 51;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1399, 251);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label19);
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(2, 127);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1399, 22);
		this.panel12.TabIndex = 46;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(3, 1);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(40, 17);
		this.label19.TabIndex = 2;
		this.label19.Text = "Main";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1353, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label27);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel6.Location = new System.Drawing.Point(2, 400);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1399, 22);
		this.panel6.TabIndex = 49;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(2, 3);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(211, 17);
		this.label27.TabIndex = 2;
		this.label27.Text = "Detail Of Above Selected Row";
		this.GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GrdHistoryDetail).BackColor = System.Drawing.Color.White;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(2, 422);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(1399, 208);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 1;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 53);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1399, 74);
		this.panel7.TabIndex = 0;
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1057, 70);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(946, 42);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(946, 19);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(865, 19);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(865, 42);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(800, 36);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(499, 19);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(58, 15);
		this.label50.TabIndex = 87;
		this.label50.Text = "Customer";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(397, 19);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(64, 15);
		this.label51.TabIndex = 86;
		this.label51.Text = "To Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(296, 19);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 85;
		this.label52.Text = "From Doc No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(151, 19);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(3, 19);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance122;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance131).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance132).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(502, 36);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(296, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(400, 38);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(99, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(299, 38);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(99, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(154, 38);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(6, 38);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label16);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1399, 51);
		this.panel8.TabIndex = 0;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(6, 28);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(190, 19);
		this.label16.TabIndex = 22;
		this.label16.Text = "Delivery Order History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1399, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 10f);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(61, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1411, 661);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "DeliveryOrder";
		this.Text = "Delivery Order";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DeliveryOrder_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.DetailGridPanel.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridExpense).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdLoader).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSaleType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryOrderType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
