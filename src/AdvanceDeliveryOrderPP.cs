using System;
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
using Architecture.WinApp.Sale;
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
namespace Architecture.WinApp.PartyProcessing
{

public class AdvanceDeliveryOrderPP : Form
{
	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Attachment AT = new Attachment();

	public UserAccount UserAccount = new UserAccount();

	private Rightsobjects formright = new Rightsobjects();

	private List<InvDeliveryOrderdetail> lstRemoveRecord = new List<InvDeliveryOrderdetail>();

	private DataTable table = new DataTable();

	private DataTable dthistoryfill = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtRefParty = new DataTable();

	private DataTable dtWh = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtJobLoat = new DataTable();

	private DataTable dtPackingType = new DataTable();

	private int updateDetailIndex = -1;

	public int RecId;

	private bool IsApproved;

	private bool FormInitialized;

	private int DocumentTypeId = 910;

	private string ScreenName = "AdvanceDeliveryOrderPP";

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool WarningMessageOnDOForPackingWeight;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool ItemSearchWithNameOrCode;

	private bool AllowMultipleOrPartialInvoicingForADO;

	private double ToleranceForStockWeightAndNetWeightOnADO;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private LoadSaleInvoiceForADO LoadSaleInvoice;

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

	internal UltraCombo CmbSupplierCustomer;

	private Label label2;

	private TextBox txtremarks;

	internal UltraCombo CmbWareHouse;

	private Label label3;

	internal UltraCombo CmbJobLot;

	private Label label8;

	private CheckBox ChkPrint;

	private TextBox txtGrossWeight;

	private Label label6;

	private TextBox txtPackWeight;

	private Label label1;

	private ToolStripButton btnDefineReferenceParties;

	internal UltraCombo CmbCropYear;

	private Label label7;

	private Label lblBalance;

	private TextBox txtVehicleNo;

	private ToolStripButton btnSaveAs;

	internal UltraCombo CmbVehicleType;

	private Label label14;

	private ToolStripButton btnDelete;

	private GroupBox groupBox1;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	internal GridEX grd;

	private Panel panel5;

	private Label label15;

	private Label label16;

	internal GridEX GrdHistoryDetail;

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

	private TextBox txtPackUnit;

	private Label label20;

	private RadioButton RadNoneFormToOpenOnInsert;

	private LinkLabel labelGpOutwardForm;

	private RadioButton RadGpOutwardToOpenOnInsert;

	internal UltraCombo ultraCombo1;

	private Label label4;

	private Label label9;

	private Label label10;

	private CheckBox chkPrintII;

	private ToolStripButton btnPrintII;

	private ToolStripButton BtnLoadSaleInvoice;

	private DateTimePicker txtExpiryDate;

	private ToolStripButton BtnPrintIII;

	private CheckBox chkPrintIII;

	public AdvanceDeliveryOrderPP(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
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
			dtViewAttachments.Columns.Add("AttchmentPath");
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
			table.Columns.Add("RefDocType", typeof(string));
			table.Columns.Add("InvoiceNo", typeof(int));
			table.Columns.Add("InvoiceManualBillNo", typeof(string));
			table.Columns.Add("WareHouseId", typeof(int));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemCode");
			table.Columns.Add("Item");
			table.Columns.Add("CropYearId", typeof(int));
			table.Columns.Add("JobLotId", typeof(int));
			table.Columns.Add("PackingTypeId", typeof(int));
			table.Columns.Add("ItemUOMId", typeof(int));
			table.Columns.Add("ItemUOM");
			table.Columns.Add("PUomEquivalent", typeof(double));
			table.Columns.Add("QTY", typeof(double));
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("PackingUnit", typeof(double));
			table.Columns.Add("PackingWeight", typeof(double));
			table.Columns.Add("GrossWeight", typeof(double));
			table.Columns.Add("Remarks");
			table.Columns.Add("AvailableStock", typeof(double));
			table.Columns.Add("OtherRemarks");
			table.Columns.Add("StockWeight", typeof(double));
			table.Columns.Add("InvoiceQty", typeof(double));
			table.Columns.Add("InvoiceWeight", typeof(double));
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
			ScreenName = Conversion.ToString(base.Tag);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			string documentNoData = Conversion.ToString((object)CommonServices.DeliveryOrderGenerateCode(DocumentTypeId));
			DataTable all = VehicleType.GetAll();
			DataTable historyComboData = HistoryComboDBCall();
			CommonServices.ReferencePartyServiceBind();
			ReferencePartiesDBCall();
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
				btnprint.Checked = formright.DoHavePrintRights;
				ChkPrint.Checked = formright.DoHavePrintRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formright);
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
				VehicleTypesBind(result.VehicleTypeData);
				SupplierDtFillFromGlobal();
				BindSupplierName();
				ReferencePartiesBind();
				WarehousesDtFillFromGlobalAndBind();
				ItemDtFillFromGlobal();
				ItemNameBind();
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
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		try
		{
			return InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				ReqType = "AdvanceDO",
				BranchesIds = Conversion.ToString((object)UserAccount.BranchesId),
				Activity = "Customer"
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
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "ItemSearchByCode");
			ItemSearchWithNameOrCode = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			ConfigrationsAllocation obj2 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate");
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)((obj2 != null) ? obj2.ConfigKey : null));
			WarningMessageOnDOForPackingWeight = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("WarningMessageOnDOForPackingWeight"));
			ToleranceForStockWeightAndNetWeightOnADO = Conversion.ToDouble((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ToleranceForStockWeightAndNetWeightOnADO"));
			ConfigrationsAllocation obj3 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "AllowMultipleOrPartialInvoicingForADO");
			AllowMultipleOrPartialInvoicingForADO = Conversion.ToBool((object)((obj3 != null) ? obj3.ConfigKey : null));
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

	private void ReferencePartiesBind()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(ultraCombo1, dtRefParty, "Id", "ReferencePartyName", "Reference Party Name", AllColumns: false, Conversion.ToInt(ultraCombo1.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			int Id = Conversion.ToInt(CmbItemName.Value);
			string ValueMember = (rdSearchByName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdSearchByName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: true, 1);
			PackUomFromGlobalBind(Conversion.ToInt(CmbItemName.Value));
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

	private void CmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			PackUomFromGlobalBind(Conversion.ToInt(CmbItemName.Value));
			UpdateAvailableStockLabel();
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

	private bool FormValidationDetail()
	{
		return FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbSupplierCustomer, "Customer", FormHelper.ValidType.INT),
			((Control)(object)CmbWareHouse, "Warehouse", FormHelper.ValidType.INT),
			((Control)(object)CmbItemName, "Item Name", FormHelper.ValidType.INT),
			((Control)(object)CmbCropYear, "Crop Year", FormHelper.ValidType.INT),
			((Control)(object)CmbPackingType, "Packing Type", FormHelper.ValidType.INT),
			((Control)(object)CmbPackUom, "Pack UOM", FormHelper.ValidType.INT),
			(txtqty, "Item Qty", FormHelper.ValidType.Double),
			(txtweight, "Weight", FormHelper.ValidType.Double),
			(txtGrossWeight, "Gross Weight", FormHelper.ValidType.Double)
		});
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
	}

	private void grdComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grd.RootTable.Columns["CropYearId"], dtCrop, "Id", "CropYear", "Crop Year");
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
			List<string> hiddenColumns = new List<string> { "Id", "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "ItemId", "ItemUOMId", "PUomEquivalent", "PackingUnit", "PackingWeight", "GrossWeight" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Remarks",
					(EditType)1
				},
				{
					"OtherRemarks",
					(EditType)1
				}
			};
			if (AllowMultipleOrPartialInvoicingForADO)
			{
				editableColumns.Add("QTY", (EditType)1);
				editableColumns.Add("Weight", (EditType)1);
				editableColumns.Add("PackingUnit", (EditType)1);
			}
			else
			{
				editableColumns.Add("StockWeight", (EditType)1);
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns, editableColumns);
			grd.RootTable.Columns["Item"].Width = 250;
			grd.RootTable.Columns["WareHouseId"].Width = 150;
			grd.RootTable.Columns["PackingTypeId"].Width = 150;
			grd.RootTable.Columns["JobLotId"].Width = 150;
			grd.RootTable.Columns["RefDocType"].Width = 70;
			grdComboBind();
			grd.RootTable.Columns["CropYearId"].EditType = (EditType)0;
			grd.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			if (AllowMultipleOrPartialInvoicingForADO)
			{
				GridEX_Helper.AddButton(grd, "Delete", "X", 20, 1);
				grd.FrozenColumns = 2;
			}
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow row = grd.CurrentRow;
			if (row == null || (int)row.RowType != 0)
			{
				return;
			}
			string colKey = e.Column.Key;
			double qty = Conversion.ToDouble(row.Cells["QTY"].Value);
			double weight = Conversion.ToDouble(row.Cells["Weight"].Value);
			double puom = Conversion.ToDouble(row.Cells["PUomEquivalent"].Value);
			double packingUnit = Conversion.ToDouble(row.Cells["PackingUnit"].Value);
			double packingWeight = Conversion.ToDouble(row.Cells["PackingWeight"].Value);
			double InvoiceQty = Conversion.ToDouble(row.Cells["InvoiceQty"].Value);
			double InvoiceWeight = Conversion.ToDouble(row.Cells["InvoiceWeight"].Value);
			switch (colKey)
			{
			case "QTY":
			case "PackingUnit":
			{
				double newWeight = qty * puom;
				row.Cells["Weight"].Value = newWeight;
				double newPackingWeight2 = packingUnit * qty;
				row.Cells["PackingWeight"].Value = newPackingWeight2;
				double grossWeight3 = newWeight + newPackingWeight2;
				row.Cells["GrossWeight"].Value = grossWeight3;
				if (AllowMultipleOrPartialInvoicingForADO)
				{
					double StockWeight3 = ((InvoiceWeight > 0.0 && InvoiceQty > 0.0) ? (InvoiceWeight / InvoiceQty * qty) : 0.0);
					row.Cells["StockWeight"].Value = StockWeight3;
				}
				break;
			}
			case "Weight":
			{
				double num = Conversion.ToDouble(row.Cells["Weight"].Value);
				double newPackingWeight = packingUnit * qty;
				row.Cells["PackingWeight"].Value = newPackingWeight;
				double grossWeight2 = num + newPackingWeight;
				row.Cells["GrossWeight"].Value = grossWeight2;
				if (AllowMultipleOrPartialInvoicingForADO)
				{
					double StockWeight2 = ((InvoiceWeight > 0.0 && InvoiceQty > 0.0) ? (InvoiceWeight / InvoiceQty * qty) : 0.0);
					row.Cells["StockWeight"].Value = StockWeight2;
				}
				break;
			}
			case "PackingWeight":
			{
				double newPackingUnit = ((qty > 0.0) ? (packingWeight / qty) : 0.0);
				row.Cells["PackingUnit"].Value = newPackingUnit;
				double grossWeight = packingWeight + weight;
				row.Cells["GrossWeight"].Value = grossWeight;
				if (AllowMultipleOrPartialInvoicingForADO)
				{
					double StockWeight = ((InvoiceWeight > 0.0 && InvoiceQty > 0.0) ? (InvoiceWeight / InvoiceQty * qty) : 0.0);
					row.Cells["StockWeight"].Value = StockWeight;
				}
				break;
			}
			}
			switch (colKey)
			{
			case "WareHouseId":
			case "CropYearId":
			case "JobLotId":
			case "PackingTypeId":
				UpdateAvailableStockInGridDetailRow(row);
				break;
			}
			if (colKey == "StockWeight")
			{
				double stockWeight = Conversion.ToDouble(row.Cells["StockWeight"].Value);
				double num2 = Conversion.ToDouble(row.Cells["GrossWeight"].Value);
				double upperLimit = num2 + ToleranceForStockWeightAndNetWeightOnADO;
				double lowerLimit = num2 - ToleranceForStockWeightAndNetWeightOnADO;
				if (stockWeight > upperLimit)
				{
					MessageBox.Show($"Stock Weight should be less than or equal to Gross Weight + {ToleranceForStockWeightAndNetWeightOnADO}");
					row.Cells["StockWeight"].Value = upperLimit;
				}
				else if (stockWeight < lowerLimit)
				{
					MessageBox.Show($"Stock Weight should be greater than or equal to Gross Weight - {ToleranceForStockWeightAndNetWeightOnADO}");
					row.Cells["StockWeight"].Value = lowerLimit;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		ResetDetail();
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
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
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						FillDetailListCommonForInsertAndDelete(vd, r);
						vd.ActionTypeId = 3;
						lstRemoveRecord.Add(vd);
						table.Rows[r.RowIndex].Delete();
						r.Delete();
						grd.UpdateData();
					}
				}
				else
				{
					r.Delete();
					grd.UpdateData();
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
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				(txtdocno, "Doc No", FormHelper.ValidType.INT),
				((Control)(object)CmbSupplierCustomer, "Customer", FormHelper.ValidType.INT)
			}))
			{
				return;
			}
			if (Conversion.ToDateTime((object)DocDate.Value).Date > Conversion.ToDateTime((object)txtExpiryDate.Value).Date)
			{
				throw new Exception("Expiry Date Should be Greater than or Equal to Doc Date");
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
			double TotalPackingWeight = GridEX_Helper.GetColumnSum(grd, "PackingWeight");
			if (WarningMessageOnDOForPackingWeight && TotalPackingWeight == 0.0 && !FormHelper.ConfirmAction("There is no Packing Weight added in any row. Are you Sure To Proceed?"))
			{
				return;
			}
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.BranchesId = UserAccount.BranchesId;
			po.DocumentTypeId = DocumentTypeId;
			po.ScreenName = ScreenName;
			po.EntryUser = UserAccount.ID;
			po.ModifyUser = UserAccount.ID;
			po.EntryDate = DateTime.Now;
			po.ModifyDate = DateTime.Now;
			po.DocNo = Conversion.ToInt((object)txtdocno.Text);
			po.DocDate = DocDate.Value;
			po.ExpiryDate = Conversion.ToDateTime((object)txtExpiryDate.Value);
			po.DeliveryOrderType = "AdvanceDO";
			po.VehicleType = Conversion.ToString((object)((Control)(object)CmbVehicleType).Text);
			po.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text.Trim());
			po.LoadingInstructions = Conversion.ToString((object)txtremarks.Text);
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
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
				vd.Id = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd, r);
				FormHelper.ValidateField(vd.WarehouseId, "Warehouse", r.RowIndex);
				FormHelper.ValidateField(vd.ItemId, "Item", r.RowIndex);
				FormHelper.ValidateField(vd.CropYearId, "CropYear", r.RowIndex);
				FormHelper.ValidateField(vd.JobLotId, "JobLot", r.RowIndex);
				FormHelper.ValidateField(vd.InvPackingTypeId, "PackingType", r.RowIndex);
				FormHelper.ValidateField(vd.PackUomId, "PackUom", r.RowIndex);
				FormHelper.ValidateField(vd.DoQty, "Qty", r.RowIndex);
				FormHelper.ValidateField(vd.LoadingWeight, "DoWeight", r.RowIndex);
				FormHelper.ValidateField(vd.GrossWeight, "GrossWeight", r.RowIndex);
				double upperLimit = vd.GrossWeight + ToleranceForStockWeightAndNetWeightOnADO;
				double lowerLimit = vd.GrossWeight - ToleranceForStockWeightAndNetWeightOnADO;
				if (!AllowMultipleOrPartialInvoicingForADO)
				{
					if (vd.StockWeight > upperLimit)
					{
						MessageBox.Show($"Stock Weight should be less than or equal to Gross Weight + Tolerance : {ToleranceForStockWeightAndNetWeightOnADO} in Detail Grid Row No {r.RowIndex + 1}");
						return;
					}
					if (vd.StockWeight < lowerLimit)
					{
						MessageBox.Show($"Stock Weight should be greater than or equal to Gross Weight - Tolerance : {ToleranceForStockWeightAndNetWeightOnADO} in Detail Grid Row No {r.RowIndex + 1}");
						return;
					}
				}
				po.DoTotalQty += Conversion.ToDecimal((object)vd.DoQty);
				po.NetWeight += vd.LoadingWeight;
				po.PackingWeight += vd.OuterEbTotal;
				vd.TotalPackingWeight = vd.OuterEbTotal;
				po.GrossWeight += vd.GrossWeight;
				po.InvDeliveryOrderDetailList.Add(vd);
			}
			FormHelper.UpdateAttachmentsForObject<InvDeliveryOrder>(po, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
			po.AttachmentsValues = AttachmentsValues;
			po.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = InvDeliveryOrder.Save(po);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + po.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, po.DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + po.DocNo);
			}
			Reset();
			if (btnsave.Visible && btnsave.Enabled)
			{
				_ = RadGpOutwardToOpenOnInsert.Checked;
			}
			if (ChkPrint.Checked)
			{
				CommonServices.AdvanceDeliveryOrderSlip910(success, DocumentTypeId);
			}
			if (chkPrintIII.Checked)
			{
				CommonServices.AdvanceDeliveryOrderSlip910_01(success, DocumentTypeId);
			}
			if (chkPrintII.Checked)
			{
				CommonServices.AdvanceDeliveryOrderChallanSlip910A(success, DocumentTypeId);
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
			if (po != null && po.InvDeliveryOrderDetailList != null && po.InvDeliveryOrderDetailList.Count > 0)
			{
				tabControl1.SelectedIndex = 0;
				btnsave.Visible = false;
				btnSaveAs.Visible = false;
				btnupdate.Visible = true;
				btnDelete.Visible = true;
				txtdocno.Text = Conversion.ToString((object)po.DocNo);
				DocDate.Value = po.DocDate;
				txtExpiryDate.Value = Conversion.ToDateTime((object)po.ExpiryDate);
				((Control)(object)CmbVehicleType).Text = Conversion.ToString((object)po.VehicleType);
				txtVehicleNo.Text = po.VehicleNo;
				CmbSupplierCustomer.Value = po.InvDeliveryOrderDetailList[0].SupplierCustomerId;
				ultraCombo1.Value = po.InvDeliveryOrderDetailList[0].RefPartyId;
				txtremarks.Text = Conversion.ToString((object)po.LoadingInstructions);
				IsApproved = po.IsApproved;
				AttachmentsValues = po.AttachmentsValues;
				CustomAttachmentsValues = po.CustomAttachmentsValues;
				table.Clear();
				FilldtDetailFromListCommonForReadById(po, table);
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(InvDeliveryOrderdetail vd, GridEXRow r)
	{
		vd.SupplierCustomerId = Conversion.ToInt(CmbSupplierCustomer.Value);
		vd.SupplierCustomer = Conversion.ToString((object)((Control)(object)CmbSupplierCustomer).Text);
		vd.RefPartyId = Conversion.ToInt(ultraCombo1.Value);
		vd.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
		vd.RefDocIdNo = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
		vd.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
		vd.RefDocumentType = Conversion.ToString(r.Cells["RefDocType"].Value);
		vd.RefDocNo = Conversion.ToInt(r.Cells["InvoiceNo"].Value);
		vd.RefManualBillNo = Conversion.ToString(r.Cells["InvoiceManualBillNo"].Value);
		vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
		vd.WareHouseName = Conversion.ToString((object)r.Cells["WareHouseId"].Text);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["Item"].Value);
		vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
		vd.CropYear = Conversion.ToString((object)r.Cells["CropYearId"].Text);
		vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
		vd.JobLotDescription = Conversion.ToString((object)r.Cells["JobLotId"].Text);
		vd.InvPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
		vd.PackTypeDesc = Conversion.ToString((object)r.Cells["PackingTypeId"].Text);
		vd.PackUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
		vd.PackUOM = Conversion.ToString(r.Cells["ItemUOM"].Value);
		vd.PUomEquivalent = Conversion.ToDouble(r.Cells["PUomEquivalent"].Value);
		vd.DoQty = Conversion.ToDouble(r.Cells["QTY"].Value);
		vd.DoWeight = Conversion.ToDouble(r.Cells["Weight"].Value);
		vd.LoadingQty = Conversion.ToDouble(r.Cells["QTY"].Value);
		vd.LoadingWeight = Conversion.ToDouble(r.Cells["Weight"].Value);
		vd.PackingWeight = Conversion.ToDouble(r.Cells["PackingUnit"].Value);
		vd.OuterEbTotal = Conversion.ToDouble(r.Cells["PackingWeight"].Value);
		vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
		vd.StockWeight = Conversion.ToDouble(r.Cells["StockWeight"].Value);
		vd.LoadingRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
		vd.InspectionRemarks = Conversion.ToString(r.Cells["OtherRemarks"].Value);
		vd.RefDocQty = Conversion.ToDouble(r.Cells["InvoiceQty"].Value);
		vd.RefDocWeight = Conversion.ToDouble(r.Cells["InvoiceWeight"].Value);
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
				detail.Id, detail.RefDocumentTypeId, detail.RefDocIdNo, detail.RefDocSubIdNo, detail.RefDocumentType, detail.RefDocNo, detail.RefManualBillNo, detail.WarehouseId, detail.ItemId, detail.ItemCode,
				detail.ItemName, detail.CropYearId, detail.JobLotId, detail.InvPackingTypeId, detail.PackUomId, detail.PackUOM, detail.PUomEquivalent, detail.DoQty, detail.DoWeight, detail.PackingWeight,
				detail.OuterEbTotal, detail.GrossWeight, detail.LoadingRemarks, 0, detail.InspectionRemarks, detail.StockWeight, detail.RefDocQty, detail.RefDocWeight
			};
			if (AddExtraColumns)
			{
				rowValues.Add(detail.WareHouseName);
				rowValues.Add(detail.CropYear);
				rowValues.Add(detail.JobLotDescription);
				rowValues.Add(detail.PackTypeDesc);
			}
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems", "MultiCurrency", "AllCities", "CropYear", "JobLot", "InvPackingType" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			ReferencePartiesDBCall();
			VehicleTypesBind(VehicleType.GetAll());
			SupplierDtFillFromGlobal();
			BindSupplierName();
			ReferencePartiesBind();
			WarehousesDtFillFromGlobalAndBind();
			ItemDtFillFromGlobal();
			ItemNameBind();
			PackUomFromGlobalBind(Conversion.ToInt(CmbItemName.Value));
			CropDtFillFromGlobalAndBind();
			JobLotDtFillFromGlobalAndBind();
			PackingTypeDtFillFromGlobalAndBind();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
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
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			RecId = 0;
			IsApproved = false;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			btnSaveAs.Visible = false;
			txtdocno.Text = string.Empty;
			((Control)(object)CmbSupplierCustomer).Text = string.Empty;
			((Control)(object)ultraCombo1).Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			((Control)(object)CmbVehicleType).Text = string.Empty;
			txtremarks.Text = string.Empty;
			((Control)(object)CmbWareHouse).Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbCropYear).Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
			((UltraGridBase)CmbPackUom).DataSource = null;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			txtPackUnit.Text = "0";
			txtPackWeight.Text = "0";
			txtGrossWeight.Text = "0";
			((Control)(object)CmbPackingType).Text = string.Empty;
			txtremarksdetail.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			table.Rows.Clear();
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			UpdateDocumentNoUI(Conversion.ToString((object)CommonServices.DeliveryOrderGenerateCode(DocumentTypeId)));
			GetConfigurationsFromGlobalAndBindValuesInColumns();
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
			((Control)(object)CmbItemName).Text = string.Empty;
			((UltraGridBase)CmbPackUom).DataSource = null;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			txtPackUnit.Text = "0";
			txtPackWeight.Text = "0";
			txtGrossWeight.Text = "0";
			((Control)(object)CmbPackingType).Text = string.Empty;
			updateDetailIndex = -1;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			CmbWareHouse.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AdvanceDeliveryOrderPP_FormClosing(object sender, FormClosingEventArgs e)
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

	private void txtremarks_Leave(object sender, EventArgs e)
	{
		txtdocno.Focus();
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
			reports.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			reports.Activity = "AdvanceDO";
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
					dthistoryfill.Columns.Add("ExpiryDate", typeof(DateTime));
					dthistoryfill.Columns.Add("VehicleType");
					dthistoryfill.Columns.Add("VehicleNo");
					dthistoryfill.Columns.Add("SupplierCustomerId", typeof(int));
					dthistoryfill.Columns.Add("CustomerName");
					dthistoryfill.Columns.Add("RefPartyId", typeof(int));
					dthistoryfill.Columns.Add("ReferencePartyName");
					dthistoryfill.Columns.Add("ApprovalStatus");
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
						dthistoryfill.Rows.Add(id, row["DeliveryOrderType"], Conversion.ToDateTime(row["DocDate"]), Conversion.ToInt(row["DocNo"]), Conversion.ToDateTime(row["ExpiryDate"]), row["VehicleType"], row["VehicleNo"], row["SupplierCustomerId"], row["CustomerName"], row["RefPartyId"], row["ReferencePartyName"], row["ApprovalStatus"], row["EntryUser"], row["EntryDate"], row["ModifyUser"], row["ModifyDate"], row["ApprovedUser"], row["ApprovedDate"], row["OrderStatus"], row["HeaderRemarks"], row["NoOfAttachments"]);
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
			List<string> hiddenCols = new List<string> { "Id", "SupplierCustomerId", "RefPartyId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 3, 2, hiddenCols);
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["ExpiryDate"].FormatString = "dd-MM-yyyy";
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GridEX_Helper.AddButton(grdhistory, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(grdhistory, "Print", "Print", 40, 1);
			GridEX_Helper.AddButton(grdhistory, "PrintII", "PrintII", 55, 2);
			GridEX_Helper.AddButton(grdhistory, "Print910A", "Print 910A", 70, 3);
			GridEX_Helper.AddButton(grdhistory, "SaveAs", "SaveAs", 65, 4);
			grdhistory.FrozenColumns = 5;
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
			if (e.Column.Key == "Edit")
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
			if (e.Column.Key == "Print")
			{
				CommonServices.AdvanceDeliveryOrderSlip910(CurrId, DocumentTypeId);
			}
			if (e.Column.Key == "PrintII")
			{
				CommonServices.AdvanceDeliveryOrderSlip910_01(CurrId, DocumentTypeId);
			}
			if (e.Column.Key == "Print910A")
			{
				CommonServices.AdvanceDeliveryOrderChallanSlip910A(CurrId, DocumentTypeId);
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
			dtDetailHistory.Columns.Add("CropYear");
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
			List<string> hiddenColumns = new List<string>
			{
				"Id", "WareHouseId", "ItemId", "ItemUOMId", "PUomEquivalent", "CropYearId", "JobLotId", "PackingTypeId", "RefDocumentTypeId", "RefDocIdNo",
				"RefDocSubIdNo", "PackingUnit", "PackingWeight", "GrossWeight", "InvoiceQty", "InvoiceWeight"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(GrdHistoryDetail, 2, 3, hiddenColumns);
			GrdHistoryDetail.RootTable.Columns["RefDocType"].Width = 70;
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
			CommonServices.AdvanceDeliveryOrderSlip910(RecId, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintII_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.AdvanceDeliveryOrderChallanSlip910A(RecId, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintIII_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.AdvanceDeliveryOrderSlip910_01(RecId, DocumentTypeId);
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
		FormHelper.CanOpenForm("GatePassInwardPartyProcessing", delegate
		{
			GatePassInwardPartyProcessing gatePassInwardPartyProcessing = new GatePassInwardPartyProcessing(UserAccount);
			gatePassInwardPartyProcessing.Tag = "GatePassOutwardParyProcessing";
			gatePassInwardPartyProcessing.Show();
			gatePassInwardPartyProcessing.BringToFront();
		}, UserAccount);
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

	private void CmbWareHouse_TextChanged(object sender, EventArgs e)
	{
		UpdateAvailableStockLabel();
	}

	private void CmbCropYear_Leave(object sender, EventArgs e)
	{
		UpdateAvailableStockLabel();
	}

	private void CmbCropYear_TextChanged(object sender, EventArgs e)
	{
		UpdateAvailableStockLabel();
	}

	private void CmbJobLot_Leave(object sender, EventArgs e)
	{
		UpdateAvailableStockLabel();
	}

	private void CmbJobLot_TextChanged(object sender, EventArgs e)
	{
		UpdateAvailableStockLabel();
	}

	private void CmbPackingType_TextChanged(object sender, EventArgs e)
	{
		UpdateAvailableStockLabel();
	}

	private void CmbPackingType_Leave(object sender, EventArgs e)
	{
		UpdateAvailableStockLabel();
	}

	private void CmbPackUom_TextChanged(object sender, EventArgs e)
	{
		UpdateAvailableStockLabel();
	}

	private void UpdateAvailableStockLabel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		try
		{
			double CurrStock = InvDeliveryOrder.GetAvailableStockForDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocDate = Conversion.ToDateTime((object)DocDate.Value),
				WarehouseId = Conversion.ToInt(CmbWareHouse.Value),
				ItemId = Conversion.ToInt(CmbItemName.Value),
				CropYear = Conversion.ToString((object)((Control)(object)CmbCropYear).Text.Trim()),
				JobLotId = Conversion.ToInt(CmbJobLot.Value),
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
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
				{
					btnprint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formright.DoHavePrintRights)
				{
					btnprint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2) && formright.DoHavePrintRights)
				{
					btnPrintII_Click(null, null);
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
			dt.Rows.Add("Ctrl+P", "For Print 910");
			dt.Rows.Add("Alt+1", "For Print 910");
			dt.Rows.Add("Alt+2", "For Print 910A");
			dt.Rows.Add("Ctrl+F1", "For Reference Parties Look up");
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
				if (AllowMultipleOrPartialInvoicingForADO)
				{
					if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "Add")
					{
						AddDetailRow(r);
					}
					if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "Delete")
					{
						DeleteDetailRow(r);
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.D && AllowMultipleOrPartialInvoicingForADO)
			{
				AddDetailRow(r);
			}
			if (e.Control && e.KeyCode == Keys.Delete && AllowMultipleOrPartialInvoicingForADO)
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
					CommonServices.AdvanceDeliveryOrderSlip910(CuurID, DocumentTypeId);
				}
				if (grdhistory.CurrentColumn.Key == "PrintII")
				{
					CommonServices.AdvanceDeliveryOrderSlip910_01(CuurID, DocumentTypeId);
				}
				if (grdhistory.CurrentColumn.Key == "Print910A")
				{
					CommonServices.AdvanceDeliveryOrderChallanSlip910A(CuurID, DocumentTypeId);
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

	private void BtnLoadSaleInvoice_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount > 0 && !GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "RefDocIdNo"))
			{
				MessageBox.Show("You cannot load 'SaleInvoice' because record without 'SaleInvoice' exists in the grid.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			LoadSaleInvoice = new LoadSaleInvoiceForADO(UserAccount);
			LoadSaleInvoice.ShowDialog();
			LoadSaleInvoice.BringToFront();
			LoadInGridDetailFromGrn(LoadSaleInvoice.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void LoadInGridDetailFromGrn(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			if (grd.GetRows().Count() > 0)
			{
				if (!GridEX_Helper.IsAnyCellValueHaveSameValue(grd, "RefDocIdNo", Conversion.ToInt(dt.Rows[0]["Id"])) && !AllowMultipleOrPartialInvoicingForADO)
				{
					if (!FormHelper.ConfirmAction("Already Loaded Row's Have Different Invoice. So you Can't Load Rows Of Different Invoice!.If you proceed, the existing data will be reset.Are you sure you want to continue?"))
					{
						return;
					}
					table.Clear();
					grd.ClearStructure();
				}
				else if (Conversion.ToInt(CmbSupplierCustomer.Value) != Conversion.ToInt(dt.Rows[0]["SupplierCustomerId"]))
				{
					if (!FormHelper.ConfirmAction("Already Loaded Row's Have Different Customer. So you Can't Load Rows Of Different Customer!. If you proceed, the existing data will be reset. Are you sure you want to continue?"))
					{
						return;
					}
					table.Clear();
					grd.ClearStructure();
				}
			}
			CmbSupplierCustomer.Value = Conversion.ToInt(dt.Rows[0]["SupplierCustomerId"]);
			HashSet<int> existingDetailIds = new HashSet<int>(from r in grd.GetRows()
				select Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value));
			foreach (DataRow dr in dt.Rows)
			{
				int detailId = Conversion.ToInt(dr["DetailId"]);
				if (!existingDetailIds.Contains(detailId))
				{
					DataRow newRow = table.NewRow();
					newRow.ItemArray = new object[28]
					{
						0,
						dr["DocumentTypeId"],
						dr["Id"],
						dr["DetailId"],
						dr["DocumentType"],
						dr["DocNo"],
						dr["ManualBillNo"],
						dr["WarehouseId"],
						dr["ItemId"],
						dr["ItemCode"],
						dr["ItemName"],
						dr["CropYearId"],
						dr["JobLotId"],
						dr["PackingTypeId"],
						dr["ItemUOMId"],
						dr["PackUom"],
						dr["PackEquivalent"],
						dr["AvailableForDoDoQty"],
						dr["AvailableForDoDoWeight"],
						0,
						0,
						dr["AvailableForDoDoWeight"],
						"",
						0,
						"",
						dr["AvailableForDoDoWeight"],
						dr["AvailableForDoDoQty"],
						dr["AvailableForDoDoWeight"]
					};
					table.Rows.Add(newRow);
				}
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			((Control)(object)CmbSupplierCustomer).Enabled = grd.RowCount == 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GetSaleInvoiceDataAgainstId(int SaleId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		try
		{
			DataTable dt = InvSaleInvoice.PendingReservedSaleInvoiceForAdvanceDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				InvoiceId = SaleId
			});
			LoadInGridDetailFromGrn(dt);
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
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Expected O, but got Unknown
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Expected O, but got Unknown
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Expected O, but got Unknown
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Expected O, but got Unknown
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Expected O, but got Unknown
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Expected O, but got Unknown
		//IL_071d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Expected O, but got Unknown
		//IL_075f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Expected O, but got Unknown
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Expected O, but got Unknown
		//IL_0cf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfa: Expected O, but got Unknown
		//IL_0d07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d11: Expected O, but got Unknown
		//IL_6036: Unknown result type (might be due to invalid IL or missing references)
		//IL_6040: Expected O, but got Unknown
		//IL_6064: Unknown result type (might be due to invalid IL or missing references)
		//IL_606e: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PartyProcessing.AdvanceDeliveryOrderPP));
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtPackUnit = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.lblBalance = new System.Windows.Forms.Label();
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
		this.txtweight = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label38 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.CmbPackingType = new UltraCombo();
		this.CmbPackUom = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.CmbVehicleType = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtExpiryDate = new System.Windows.Forms.DateTimePicker();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.ultraCombo1 = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbSupplierCustomer = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.labelGpOutwardForm = new System.Windows.Forms.LinkLabel();
		this.RadGpOutwardToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.RadNoneFormToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.label23 = new System.Windows.Forms.Label();
		this.chkPrintII = new System.Windows.Forms.CheckBox();
		this.chkPrintIII = new System.Windows.Forms.CheckBox();
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
		this.BtnPrintIII = new System.Windows.Forms.ToolStripButton();
		this.btnPrintII = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnDefineReferenceParties = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLoadSaleInvoice = new System.Windows.Forms.ToolStripButton();
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
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(986, 661);
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
		this.tabControl1.Size = new System.Drawing.Size(986, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(978, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grd.AutomaticSort = false;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(2, 169);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(974, 461);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
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
		this.panel5.Location = new System.Drawing.Point(2, 146);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(974, 23);
		this.panel5.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(934, 1);
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
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 49);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(974, 97);
		this.panel4.TabIndex = 2;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.txtPackUnit);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.rdSearchByCode);
		this.groupBox3.Controls.Add(this.rdSearchByName);
		this.groupBox3.Controls.Add(this.lblBalance);
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
		this.groupBox3.Controls.Add(this.txtweight);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.txtqty);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox3.Controls.Add(this.txtremarksdetail);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(4, 99);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(965, 118);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(709, 88);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 25);
		this.btnplus.TabIndex = 12;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtPackUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackUnit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackUnit.Location = new System.Drawing.Point(186, 88);
		this.txtPackUnit.Name = "txtPackUnit";
		this.txtPackUnit.Size = new System.Drawing.Size(79, 23);
		this.txtPackUnit.TabIndex = 8;
		this.txtPackUnit.Tag = "PackUnit";
		this.txtPackUnit.TextChanged += new System.EventHandler(txtPackUnit_TextChanged);
		this.txtPackUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackUnit_KeyPress);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(186, 68);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(74, 15);
		this.label20.TabIndex = 56;
		this.label20.Text = "Packing Unit";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(279, 14);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 54;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(220, 14);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 53;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalance.Location = new System.Drawing.Point(361, 16);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(0, 15);
		this.lblBalance.TabIndex = 46;
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance219;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance224).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(387, 36);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TextChanged += new System.EventHandler(CmbCropYear_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Leave += new System.EventHandler(CmbCropYear_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(388, 16);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(54, 15);
		this.label7.TabIndex = 41;
		this.label7.Text = "CropYear";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(367, 88);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.ReadOnly = true;
		this.txtGrossWeight.Size = new System.Drawing.Size(114, 23);
		this.txtGrossWeight.TabIndex = 10;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(369, 67);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(75, 15);
		this.label6.TabIndex = 39;
		this.label6.Text = "GrossWeight";
		this.txtPackWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackWeight.Location = new System.Drawing.Point(266, 88);
		this.txtPackWeight.Name = "txtPackWeight";
		this.txtPackWeight.Size = new System.Drawing.Size(100, 23);
		this.txtPackWeight.TabIndex = 9;
		this.txtPackWeight.Tag = "PackWeight";
		this.txtPackWeight.TextChanged += new System.EventHandler(txtPackSize_TextChanged);
		this.txtPackWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackWeight_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(266, 68);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(91, 15);
		this.label1.TabIndex = 37;
		this.label1.Text = "Packing Weight";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance139;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance145).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(482, 36);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TextChanged += new System.EventHandler(CmbJobLot_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Leave += new System.EventHandler(CmbJobLot_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(482, 16);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(43, 15);
		this.label8.TabIndex = 35;
		this.label8.Text = "JobLot";
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance167).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance168).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(5, 36);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TextChanged += new System.EventHandler(CmbWareHouse_TextChanged);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 16);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(69, 15);
		this.label3.TabIndex = 33;
		this.label3.Text = "WareHouse";
		this.txtweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtweight.Location = new System.Drawing.Point(85, 88);
		this.txtweight.Name = "txtweight";
		this.txtweight.Size = new System.Drawing.Size(100, 23);
		this.txtweight.TabIndex = 7;
		this.txtweight.TextChanged += new System.EventHandler(txtweight_TextChanged);
		this.txtweight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtweight_KeyPress);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(85, 66);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(46, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Weight";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(5, 88);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(79, 23);
		this.txtqty.TabIndex = 6;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(774, 88);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(61, 25);
		this.btnCancelUpdateDetial.TabIndex = 14;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(5, 66);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(709, 88);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(64, 25);
		this.btnUpdateDetail.TabIndex = 13;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance114;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(709, 36);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(126, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TextChanged += new System.EventHandler(CmbPackingType_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Leave += new System.EventHandler(CmbPackingType_Leave);
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance207;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(633, 36);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(75, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(CmbPackUom_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(combitempck_Leave);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance188).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(186, 36);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarksdetail.Location = new System.Drawing.Point(482, 88);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(226, 23);
		this.txtremarksdetail.TabIndex = 11;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(482, 68);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(65, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Comments";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(709, 16);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(74, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "PackingType";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(633, 16);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(63, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack UOM";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(187, 16);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(32, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item";
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleType);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.txtExpiryDate);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.ultraCombo1);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(4, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(965, 92);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(78, 13);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(80, 24);
		this.txtdocno.TabIndex = 0;
		this.CmbVehicleType.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance132).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVehicleType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Location = new System.Drawing.Point(346, 12);
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Name = "CmbVehicleType";
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).TabIndex = 4;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(4, 45);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 3;
		this.label9.Text = "Expiry Date";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(4, 18);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(76, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No,Date";
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(274, 45);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(64, 15);
		this.label10.TabIndex = 50;
		this.label10.Text = "Vehicle No";
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(274, 18);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(73, 15);
		this.label14.TabIndex = 50;
		this.label14.Text = "Vehicle Type";
		this.label33.AutoSize = true;
		this.label33.BackColor = System.Drawing.Color.Transparent;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(4, 70);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(52, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtExpiryDate.CustomFormat = "dd-MMM-yy";
		this.txtExpiryDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtExpiryDate.Location = new System.Drawing.Point(78, 41);
		this.txtExpiryDate.Name = "txtExpiryDate";
		this.txtExpiryDate.Size = new System.Drawing.Size(191, 22);
		this.txtExpiryDate.TabIndex = 2;
		this.txtExpiryDate.TabStop = false;
		this.txtExpiryDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(159, 14);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(110, 22);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(346, 41);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(200, 23);
		this.txtVehicleNo.TabIndex = 5;
		this.txtVehicleNo.TextChanged += new System.EventHandler(txtVehicleNo_TextChanged);
		this.txtremarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(78, 67);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(856, 21);
		this.txtremarks.TabIndex = 3;
		this.ultraCombo1.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo1.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance152;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance158).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance159).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ultraCombo1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Location = new System.Drawing.Point(647, 40);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Name = "ultraCombo1";
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Size = new System.Drawing.Size(287, 25);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Leave += new System.EventHandler(txtremarks_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(551, 45);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(89, 15);
		this.label4.TabIndex = 31;
		this.label4.Text = "Ref Party Name";
		this.CmbSupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance177;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance182).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance183).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance193).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Location = new System.Drawing.Point(647, 13);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Name = "CmbSupplierCustomer";
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Size = new System.Drawing.Size(287, 25);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).TabIndex = 6;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(551, 18);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 31;
		this.label2.Text = "Customer Name";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.labelGpOutwardForm);
		this.panel3.Controls.Add(this.RadGpOutwardToOpenOnInsert);
		this.panel3.Controls.Add(this.RadNoneFormToOpenOnInsert);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.chkPrintII);
		this.panel3.Controls.Add(this.chkPrintIII);
		this.panel3.Controls.Add(this.ChkPrint);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(974, 22);
		this.panel3.TabIndex = 1;
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
		this.labelGpOutwardForm.Visible = false;
		this.labelGpOutwardForm.Click += new System.EventHandler(labelGpOutwardForm_Click);
		this.RadGpOutwardToOpenOnInsert.AutoSize = true;
		this.RadGpOutwardToOpenOnInsert.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadGpOutwardToOpenOnInsert.ForeColor = System.Drawing.Color.White;
		this.RadGpOutwardToOpenOnInsert.Location = new System.Drawing.Point(392, 5);
		this.RadGpOutwardToOpenOnInsert.Name = "RadGpOutwardToOpenOnInsert";
		this.RadGpOutwardToOpenOnInsert.Size = new System.Drawing.Size(14, 13);
		this.RadGpOutwardToOpenOnInsert.TabIndex = 35;
		this.RadGpOutwardToOpenOnInsert.UseVisualStyleBackColor = true;
		this.RadGpOutwardToOpenOnInsert.Visible = false;
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
		this.RadNoneFormToOpenOnInsert.Visible = false;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(2, 1);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(127, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Delivery Order";
		this.chkPrintII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintII.AutoSize = true;
		this.chkPrintII.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintII.ForeColor = System.Drawing.Color.White;
		this.chkPrintII.Location = new System.Drawing.Point(846, 1);
		this.chkPrintII.Name = "chkPrintII";
		this.chkPrintII.Size = new System.Drawing.Size(125, 20);
		this.chkPrintII.TabIndex = 8;
		this.chkPrintII.Text = "Preview 910A";
		this.chkPrintII.UseVisualStyleBackColor = true;
		this.chkPrintIII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintIII.AutoSize = true;
		this.chkPrintIII.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintIII.ForeColor = System.Drawing.Color.White;
		this.chkPrintIII.Location = new System.Drawing.Point(700, 1);
		this.chkPrintIII.Name = "chkPrintIII";
		this.chkPrintIII.Size = new System.Drawing.Size(142, 20);
		this.chkPrintIII.TabIndex = 8;
		this.chkPrintIII.Text = "Preview 910_01";
		this.chkPrintIII.UseVisualStyleBackColor = true;
		this.ChkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.ForeColor = System.Drawing.Color.White;
		this.ChkPrint.Location = new System.Drawing.Point(581, 1);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(115, 20);
		this.ChkPrint.TabIndex = 8;
		this.ChkPrint.Text = "Preview 910";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(974, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[19]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator6, this.btnupdate, this.btnSaveAs, this.btnsave, this.btnDelete, this.toolStripSeparator5, this.btnattachment, this.toolStripSeparator4,
			this.btnprint, this.BtnPrintIII, this.btnPrintII, this.toolStripSeparator2, this.toolStripSeparator3, this.btnDefineReferenceParties, this.toolStripSeparator1, this.BtnLoadSaleInvoice, this.BtnShortCutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(974, 27);
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
		this.btnprint.Text = "910-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.BtnPrintIII.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintIII.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintIII.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintIII.Name = "BtnPrintIII";
		this.BtnPrintIII.Size = new System.Drawing.Size(116, 24);
		this.BtnPrintIII.Text = "910_01-Print";
		this.BtnPrintIII.Click += new System.EventHandler(BtnPrintIII_Click);
		this.btnPrintII.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintII.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintII.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintII.Name = "btnPrintII";
		this.btnPrintII.Size = new System.Drawing.Size(101, 24);
		this.btnPrintII.Text = "910A-Print";
		this.btnPrintII.Click += new System.EventHandler(btnPrintII_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnDefineReferenceParties.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDefineReferenceParties.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.btnDefineReferenceParties.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineReferenceParties.Name = "btnDefineReferenceParties";
		this.btnDefineReferenceParties.Size = new System.Drawing.Size(147, 24);
		this.btnDefineReferenceParties.Text = "Reference Parties";
		this.btnDefineReferenceParties.Click += new System.EventHandler(btnDefineReferenceParties_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnLoadSaleInvoice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadSaleInvoice.Image = Architecture.WinApp.Properties.Resources.download;
		this.BtnLoadSaleInvoice.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadSaleInvoice.Name = "BtnLoadSaleInvoice";
		this.BtnLoadSaleInvoice.Size = new System.Drawing.Size(148, 24);
		this.BtnLoadSaleInvoice.Text = "Load &Sale Invoice";
		this.BtnLoadSaleInvoice.Click += new System.EventHandler(BtnLoadSaleInvoice_Click);
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
		this.tabPage2.Size = new System.Drawing.Size(978, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(2, 145);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(974, 255);
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(974, 255);
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
		this.panel12.Location = new System.Drawing.Point(2, 123);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(974, 22);
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
		this.ctrlGrdBar2.Location = new System.Drawing.Point(928, -1);
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
		this.panel6.Size = new System.Drawing.Size(974, 22);
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
		this.GrdHistoryDetail.GroupByBoxVisible = false;
		this.GrdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(2, 422);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(974, 208);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 1;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 53);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(974, 70);
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
		this.groupBox2.Size = new System.Drawing.Size(965, 67);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(860, 42);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(860, 19);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(779, 19);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(779, 42);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(714, 36);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(419, 18);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(58, 15);
		this.label50.TabIndex = 87;
		this.label50.Text = "Customer";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(343, 18);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(64, 15);
		this.label51.TabIndex = 86;
		this.label51.Text = "To Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(266, 18);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 85;
		this.label52.Text = "From Doc No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(135, 18);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(3, 18);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance192;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance200).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(422, 36);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(346, 38);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(75, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(269, 38);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(75, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(138, 38);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(130, 22);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(6, 38);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(130, 22);
		this.FromDateHistory.TabIndex = 0;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label16);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(974, 51);
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
		this.toolStrip2.Size = new System.Drawing.Size(974, 27);
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
		base.ClientSize = new System.Drawing.Size(986, 661);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "AdvanceDeliveryOrderPP";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AdvanceDeliveryOrderPP_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).EndInit();
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
