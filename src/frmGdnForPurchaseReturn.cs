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
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Inventory_Stocks_Report;
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
namespace Architecture.WinApp.Purchase.Return
{

public class frmGdnForPurchaseReturn : Form
{
	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Attachment AT = new Attachment();

	private WaitForm waitForm = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private Rightsobjects formright = new Rightsobjects();

	private DataTable dtdetail = new DataTable();

	private DataTable dtExpense = new DataTable();

	private DataTable dtGpGrid = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtTransporter = new DataTable();

	private DataTable dtWarehouse = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtjob = new DataTable();

	private DataTable dtPack = new DataTable();

	private DataTable dtcity = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private int Id;

	private bool FormInitialized;

	private int updateDetailIndex = -1;

	private string ScreenName = "frmGdnForPurchaseReturn";

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string GrnDetailIds = "";

	private LoadavailableTransactionsForGDN LoadTransactions;

	private bool WagesActiveOrInActive;

	private bool StockLoaderVisibleInGDNDetail;

	private bool WagesStatus;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool ItemSearchWithNameOrCode;

	private int DefaultDaysToLessFromHistoryFromDate;

	private double GdnAndFactoryWeightCompareToleranceForStop;

	private frmLoadGrnForGdnPurchaseReturn LoadGrn;

	private List<InvGdnDetail> lstRemoveDetailRecord = new List<InvGdnDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private ToolStrip ToolStripForm;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton BtnPrint;

	private ToolStripButton btnAttachment;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Panel panel5;

	private Label label24;

	private Label label25;

	private Label label19;

	private TextBox txtBiltyNo;

	private Label label7;

	private TextBox txtVehicleNo;

	internal UltraCombo CmbTransport;

	private Label label48;

	private Label label26;

	private TextBox txtFreight;

	private Label label20;

	private TextBox txtFactoryWeight;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private Label label5;

	internal UltraCombo CmbUom;

	internal UltraCombo CmbCity;

	internal UltraCombo CmbWareHouseName;

	internal UltraCombo CmbPackingType;

	internal UltraCombo CmbJobLot;

	private Button Add;

	internal UltraCombo CmbItemName;

	private Label label30;

	private Label label27;

	private Label label22;

	private TextBox txtebt;

	private TextBox txtebu;

	private TextBox txtGrossWeight;

	private TextBox txtQty;

	private TextBox txtadlswt;

	private TextBox txtStockWeight;

	private Label label29;

	private TextBox txtNetWeight;

	private Label label46;

	private Label label28;

	private Label label35;

	private Label label45;

	private Label label44;

	private Label label43;

	private Label label42;

	private Label label41;

	private Label label40;

	private Label label36;

	private Panel panel7;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	internal UltraCombo CmbCropYear;

	private Label label9;

	private Label label3;

	private TextBox txtwtcuttotal;

	private TextBox txtwtcut;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton BtnRefresh;

	private CheckBox ChkBox;

	internal GridEX grdGp;

	internal GridEX DatagridHistoryDetail;

	internal UltraCombo CmbSupplierName;

	private Label label2;

	private ToolStrip ToolStripHistory;

	private ToolStripButton btnDelete;

	internal GridEX grd;

	private ToolStripButton btnPrintA;

	private ToolStripButton btnGdnFormHistory;

	private Label lblStock;

	private Panel panel9;

	private Panel panel13;

	private Label label39;

	private CtrlGrdBar ctrlGrdBar3;

	private Panel panel15;

	private Label label38;

	private GroupBox groupBox1;

	private Label label50;

	private TextBox txtTicketNos;

	private ToolStripButton BtnShortCutkeys;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private Panel panel19;

	private Panel panel10;

	private Panel panel6;

	private Panel panel12;

	private Label label52;

	internal GridEX GridExpense;

	private ToolStripButton btnStockReport;

	internal GridEX DataGridHistory;

	private Panel panel17;

	private Label label51;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private Label label53;

	private Label label54;

	private Label label55;

	private Label label56;

	private Label label57;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label58;

	private TextBox txtremarks;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	private Button btnRecordsUpdate;

	private ToolStripButton btnGenerateAvailableStock;

	private DateTimePicker txtGpDate;

	internal UltraCombo CmbGpNo;

	private TextBox txtCustomerCellNo;

	private TextBox txtCustomerCity;

	private Label label21;

	private Label label62;

	private TextBox txtAddLessHeader;

	private Label label49;

	private Label label17;

	private TextBox txtBalGrossWeightGrid;

	private Panel PanelMessage;

	private ToolStrip ToolStripMessage;

	private ToolStripButton BtnMessage;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnLoadGrn;

	public frmGdnForPurchaseReturn(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
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
			base.AutoScrollMinSize = new Size(1120, 750);
			ScreenName = Conversion.ToString((object)base.Name);
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtTransporter.Columns.Add("Id", typeof(int));
			dtTransporter.Columns.Add("AccountTitle", typeof(string));
			dtTransporter.Columns.Add("AccountCode", typeof(string));
			dtTransporter.Columns.Add("SupplierCustomerId", typeof(int));
			dtWarehouse.Columns.Add("Id", typeof(int));
			dtWarehouse.Columns.Add("WareHouseName", typeof(string));
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtjob.Columns.Add("Id");
			dtjob.Columns.Add("JobLotDescription");
			dtPack.Columns.Add("Id");
			dtPack.Columns.Add("PackTypeDesc");
			dtcity.Columns.Add("Id");
			dtcity.Columns.Add("CityName");
			dtCrop.Columns.Add("Id");
			dtCrop.Columns.Add("CropYear");
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("GrnId", typeof(int));
			dtdetail.Columns.Add("GrnDetailId", typeof(int));
			dtdetail.Columns.Add("GrnDocumentTypeId", typeof(int));
			dtdetail.Columns.Add("GrnNo", typeof(int));
			dtdetail.Columns.Add("WareHouseId", typeof(int));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemCode");
			dtdetail.Columns.Add("Item");
			dtdetail.Columns.Add("CropYearId", typeof(int));
			dtdetail.Columns.Add("JobId", typeof(int));
			dtdetail.Columns.Add("PackingTypeId", typeof(int));
			dtdetail.Columns.Add("UOMId", typeof(int));
			dtdetail.Columns.Add("UOM");
			dtdetail.Columns.Add("UomEquivalent");
			dtdetail.Columns.Add("Qty", typeof(double));
			dtdetail.Columns.Add("GrossWight", typeof(double));
			dtdetail.Columns.Add("EbUnit", typeof(double));
			dtdetail.Columns.Add("EbTotal", typeof(double));
			dtdetail.Columns.Add("WtCut", typeof(double));
			dtdetail.Columns.Add("WtCutTotal", typeof(double));
			dtdetail.Columns.Add("AddLesswt", typeof(double));
			dtdetail.Columns.Add("NetWeight", typeof(double));
			dtdetail.Columns.Add("StockWeight", typeof(double));
			dtdetail.Columns.Add("CityId", typeof(int));
			dtdetail.Columns.Add("AvailableStock", typeof(double));
			dtdetail.Columns.Add("BalQty", typeof(double));
			dtdetail.Columns.Add("RefDocumentTypeId", typeof(int));
			dtdetail.Columns.Add("RefDocType");
			dtdetail.Columns.Add("RefDocId", typeof(int));
			dtdetail.Columns.Add("RefDocNo", typeof(int));
			dtdetail.Columns.Add("RefDocDetailId", typeof(int));
			dtExpense.Columns.Add("ItemId");
			dtExpense.Columns.Add("Qty", typeof(double));
			dtExpense.Columns.Add("Remarks");
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)CmbGpNo).Enabled = false;
			DocDate.Focus();
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
			string documentNoData = Conversion.ToString((object)CommonServices.InvGdnGenerateCode(703));
			SupplierDtFillDbCall();
			BindTransporterDbCall();
			ItemDtFillFromPurchaseDbCall(Conversion.ToInt(CmbSupplierName.Value));
			OtherItemdtFillDbCall();
			DataTable gpData = GatepassDataDbCall();
			DataTable historyComboData = HistoryCombosdtDbCall();
			return new
			{
				documentNoData = documentNoData,
				GpData = gpData,
				HistoryComboData = historyComboData
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
				btnSave.Enabled = formright.DoHaveSaveRight;
				BtnPrint.Enabled = formright.DoHavePrintRights;
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
				if (ItemSearchWithNameOrCode)
				{
					rdSearchByCode.Checked = true;
				}
				else
				{
					rdSearchByName.Checked = true;
				}
				if (Id == 0)
				{
					UpdateDocumentNoUI(result.documentNoData);
				}
				BindSupplierName();
				TransporterAcFill();
				WarehousesDtFillFromGlobalAndBind();
				ItemDetailBind();
				CropDtFillFromGlobalAndBind();
				JobLotDtFillFromGlobalAndBind();
				PackingTypeDtFillFromGlobalAndBind();
				CityDtFillFromGlobalAndBind();
				if (dtdetail != null && dtdetail.Columns.Count > 0)
				{
					grd.DataSource = dtdetail;
					grd.RetrieveStructure();
					grdSettings();
				}
				if (dtExpense != null && dtExpense.Columns.Count > 0)
				{
					AddRowInExpenseGrid();
					GridExpense.RetrieveStructure();
					grdInvExpSettings();
				}
				GatepassGridFill(result.GpData);
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				defaultConfiquration();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void defaultConfiquration()
	{
		try
		{
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate");
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)((obj != null) ? obj.ConfigKey : null));
			ConfigrationsAllocation obj2 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "WagesCompulsoryOnGdnForPurchaseReturn");
			WagesStatus = Conversion.ToBool((object)((obj2 != null) ? obj2.ConfigKey : null));
			WagesRefDocumentsStatus obj3 = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 703);
			WagesActiveOrInActive = obj3 != null && obj3.IsActive;
			ConfigrationsAllocation obj4 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "StockLoaderVisibleInGDNDetail");
			StockLoaderVisibleInGDNDetail = Conversion.ToBool((object)((obj4 != null) ? obj4.ConfigKey : null));
			ConfigrationsAllocation obj5 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "ItemSearchByCode");
			ItemSearchWithNameOrCode = Conversion.ToBool((object)((obj5 != null) ? obj5.ConfigKey : null));
			ConfigrationsAllocation obj6 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "GdnAndFactoryWeightCompareToleranceForStop");
			GdnAndFactoryWeightCompareToleranceForStop = Conversion.ToDouble((object)((obj6 != null) ? obj6.ConfigKey : null));
			if (Conversion.ToDouble((object)GdnAndFactoryWeightCompareToleranceForStop) == 0.0)
			{
				GdnAndFactoryWeightCompareToleranceForStop = 60.0;
			}
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (list.Count > 0)
			{
				int CityId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				CmbCity.Value = CityId;
			}
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				CmbJobLot.Value = JobId;
			}
			List<ConfigrationsAllocation> lstCrop = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Default Crop Year").ToList();
			if (lstCrop.Count > 0)
			{
				int CropYearId = Conversion.ToInt((object)lstCrop[0].ConfigKey.ToString());
				CmbCropYear.Value = CropYearId;
			}
			List<ConfigrationsAllocation> lstPackingType = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Paking Type").ToList();
			if (lstPackingType.Count > 0)
			{
				int PackingTypeId = Conversion.ToInt((object)lstPackingType[0].ConfigKey.ToString());
				CmbPackingType.Value = PackingTypeId;
			}
			List<ConfigrationsAllocation> lstWareHouse = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Warehouse").ToList();
			if (lstWareHouse.Count > 0)
			{
				int WareHouseId = Conversion.ToInt((object)lstWareHouse[0].ConfigKey.ToString());
				CmbWareHouseName.Value = WareHouseId;
			}
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				List<ConfigrationsAllocation> listfr = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "FreightOutwardAc").ToList();
				if (listfr.Count > 0)
				{
					int TransporterId = Conversion.ToInt((object)listfr[0].ConfigKey.ToString());
					CmbTransport.Value = TransporterId;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string documentData)
	{
		txtdocno.Text = documentData;
	}

	private void SupplierDtFillDbCall()
	{
		try
		{
			dtSupplier.Clear();
			int partyTypeId = 0;
			DataTable data = InvPurchaseInvoice.PurchaseInvoice_GetVendorsAndCustomers(UserAccount.OrganizationId, UserAccount.CompanyId, partyTypeId, "56,57,702,64");
			if (data == null || data.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow row in data.Rows)
			{
				dtSupplier.Rows.Add(Convert.ToInt32(row["Id"]), Convert.ToString(row["CompanyName"]), Convert.ToString(row["PartyCode"]), Convert.ToInt32(row["GlAccountId"]), Convert.ToInt32(row["CityId"]), Convert.ToString(row["CityName"]), Convert.ToString(row["MobilePersonal"]));
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
			int Id = Conversion.ToInt(CmbSupplierName.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbSupplierName, dtSupplier, "Id", "CompanyName", "Supplier Name", AllColumns: true, Id, insertDefaultRow: true);
			if (dtSupplier.Rows.Count > 0 && ((UltraGridBase)CmbSupplierName).DataSource != null)
			{
				((UltraGridBase)CmbSupplierName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
			((UltraGridBase)CmbSupplierName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindTransporterDbCall()
	{
		try
		{
			dtTransporter.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				DataTable dt = CommonServices.GetVendorsAndCustomersForTransporter();
				if (dt.Rows.Count <= 0)
				{
					return;
				}
				{
					foreach (DataRow dr in dt.Rows)
					{
						dtTransporter.Rows.Add(dr["GlAccountId"], dr["CompanyName"], dr["PartyCode"], dr["Id"]);
					}
					return;
				}
			}
			DataTable dt2 = CommonServices.CoaAllocationGetAllServiceBind();
			if (dt2.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow dr2 in dt2.Rows)
			{
				int AccountTypeId = Conversion.ToInt(dr2["AccountTypeId"]);
				if (AccountTypeId == 6 || AccountTypeId == 8)
				{
					dtTransporter.Rows.Add(dr2["Id"], dr2["AccountTitle"], dr2["AccountCode"], 0);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TransporterAcFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbTransport.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbTransport, dtTransporter, "Id", "AccountTitle", "Transporter Name", AllColumns: true, Id, insertDefaultRow: true);
			if (dtTransporter.Rows.Count > 0 && ((UltraGridBase)CmbTransport).DataSource != null)
			{
				((UltraGridBase)CmbTransport).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
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
			DatatableHelper.PopulateDataTable(dtWarehouse, lst, (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName);
			int Id = Conversion.ToInt(CmbWareHouseName.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbWareHouseName, dtWarehouse, "Id", "WareHouseName", "WareHouse Name", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemDtFillFromPurchaseDbCall(int PartyId)
	{
		try
		{
			dtitem.Clear();
			DataTable dt = InvPurchaseInvoice.GetItemsFromPurchaseInvoiceStoreAgainstPartyId(UserAccount.OrganizationId, UserAccount.CompanyId, PartyId, "56,57,64,702");
			if (dt == null || dt.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow dr in dt.Rows)
			{
				dtitem.Rows.Add(dr["Id"], dr["ItemName"], dr["ItemCodeNew"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemName.Value);
			string ValueMember = (rdSearchByName.Checked ? "ItemName" : "ItemCode");
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtitem, "Id", ValueMember, "Item Name", AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: true, 1);
			CmbItemName_Leave(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ItemDetailBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			List<GetJobLotGlIdsandName> lst = clsGlobalVariables.globalJobLot;
			DatatableHelper.PopulateDataTable(dtjob, lst, (GetJobLotGlIdsandName x) => x.Id, (GetJobLotGlIdsandName x) => x.JobLotDescription);
			int Id = Conversion.ToInt(CmbJobLot.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbJobLot, dtjob, "Id", "JobLotDescription", "JobLot", AllColumns: true, Id, insertDefaultRow: true);
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
			List<InvPackingType> lst = clsGlobalVariables.globalInvPackingType;
			DatatableHelper.PopulateDataTable(dtPack, lst, (InvPackingType x) => x.Id, (InvPackingType x) => x.PackTypeDesc);
			int Id = Conversion.ToInt(CmbPackingType.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbPackingType, dtPack, "Id", "PackTypeDesc", "Packing Type", AllColumns: true, Id, insertDefaultRow: true);
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
			string Packuom = ((Control)(object)CmbUom).Text;
			DataTable dt = CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId);
			if (dt != null)
			{
				InfragisticsHelper.BindAndRetainSelection(CmbUom, dt, "Id", "UOMCode", "Uom", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CityDtFillFromGlobalAndBind()
	{
		try
		{
			List<GlobalAllCity> lst = clsGlobalVariables.globalAllCities;
			DatatableHelper.PopulateDataTable(dtcity, lst, (GlobalAllCity x) => x.Id, (GlobalAllCity x) => x.CityName);
			int Id = Conversion.ToInt(CmbCity.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbCity, dtcity, "Id", "CityName", "City Name", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemdtFillDbCall()
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

	private void combitem_Leave(object sender, EventArgs e)
	{
		try
		{
			UpdateAvailableStockLabelValue();
			txtGrossWeight.Enabled = true;
			double FactoryWeight = 0.0;
			if (txtFactoryWeight.Text.Trim() != string.Empty)
			{
				FactoryWeight = Math.Round(Conversion.ToDouble((object)txtFactoryWeight.Text.Trim()), 2);
			}
			double GridGrossWeight = GridEX_Helper.GetColumnSum(grd, "GrossWight");
			txtGrossWeight.Text = ((GridGrossWeight < FactoryWeight) ? (FactoryWeight - GridGrossWeight).ToString() : "0");
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
			int Id = Conversion.ToInt(CmbSupplierName.Value);
			if (((UltraGridBase)CmbSupplierName).ActiveRow == null || Id <= 0)
			{
				return;
			}
			string CustomerCellNo = Conversion.ToString(((UltraDropDownBase)CmbSupplierName).SelectedRow.Cells["MobileNo"].Value);
			txtCustomerCellNo.Text = CustomerCellNo;
			string CityName = Conversion.ToString(((UltraDropDownBase)CmbSupplierName).SelectedRow.Cells["CityName"].Value);
			txtCustomerCity.Text = CityName;
			if (!string.IsNullOrEmpty(CityName) && Conversion.ToInt(CmbCity.Value) == 0 && ((UltraGridBase)CmbCity).DataSource is DataTable dt && dt.Rows.Count > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("CityName") != null && row.Field<string>("CityName") == CityName))
				{
					((Control)(object)CmbCity).Text = CityName;
				}
				else
				{
					((Control)(object)CmbCity).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplierName_Leave(object sender, EventArgs e)
	{
		try
		{
			int PartyId = Conversion.ToInt(CmbSupplierName.Value);
			if (((UltraGridBase)CmbSupplierName).ActiveRow != null && PartyId > 0)
			{
				ItemDtFillFromPurchaseDbCall(PartyId);
				ItemDetailBind();
			}
			BindCustomerCity();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemName.Value);
			PackUomFromGlobalBind(Id);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddRowInExpenseGrid()
	{
		try
		{
			dtExpense.Rows.Add(0, 0, "");
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
			GridEX_Helper.GridWrappingAndColumnSettings(GridExpense, 3, 3);
			GridEX_Helper.ComboBind(GridExpense.RootTable.Columns["ItemId"], dtOtherItem, "Id", "OtherItemName", "Item Name", 230);
			GridEX_Helper.AddButton(GridExpense, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(GridExpense, "Add", "+", 20, 1);
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
					dtExpense.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				if (e.Column.Key == "Add")
				{
					AddRowInExpenseGrid();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbGpNo.Value) == 0)
			{
				throw new Exception("Please Load Any GatePass First...");
			}
			if (grd.RowCount > 0 && GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "GrnId"))
			{
				throw new Exception("You can not add manual Record because record against Grn exist in Grid");
			}
			if (FormDetailValidation())
			{
				dtdetail.Rows.Add(0, 0, 0, 0, 0, CmbWareHouseName.Value, CmbItemName.Value, Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value), CmbCropYear.Value, CmbJobLot.Value, CmbPackingType.Value, Conversion.ToInt(CmbUom.Value), Conversion.ToString((object)((Control)(object)CmbUom).Text.Trim()), Conversion.ToDouble(((UltraDropDownBase)CmbUom).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtQty.Text.Trim()), Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), Conversion.ToDouble((object)txtebu.Text.Trim()), Conversion.ToDouble((object)txtebt.Text.Trim()), Conversion.ToDouble((object)txtwtcut.Text.Trim()), Conversion.ToDouble((object)txtwtcuttotal.Text.Trim()), Conversion.ToDouble((object)txtadlswt.Text.Trim()), Conversion.ToDouble((object)txtNetWeight.Text.Trim()), Conversion.ToDouble((object)txtStockWeight.Text.Trim()), CmbCity.Value, 0, 0, 0, "", 0, 0, 0);
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
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
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && !GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "GrnId"))
			{
				updateDetailIndex = item.RowIndex;
				CmbWareHouseName.Value = item.Cells["WareHouseId"].Value.ToString();
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				CmbCropYear.Value = item.Cells["CropYearId"].Value.ToString();
				CmbJobLot.Value = item.Cells["JobId"].Value.ToString();
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				CmbItemName_Leave(null, null);
				CmbUom.Value = item.Cells["UOMId"].Value.ToString();
				txtQty.Text = item.Cells["Qty"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWight"].Value.ToString();
				txtebu.Text = item.Cells["EbUnit"].Value.ToString();
				txtebt.Text = item.Cells["EbTotal"].Value.ToString();
				txtwtcut.Text = item.Cells["WtCut"].Value.ToString();
				txtwtcuttotal.Text = item.Cells["WtCutTotal"].Value.ToString();
				txtadlswt.Text = item.Cells["AddLesswt"].Value.ToString();
				txtNetWeight.Text = item.Cells["NetWeight"].Value.ToString();
				txtStockWeight.Text = item.Cells["StockWeight"].Value.ToString();
				CmbCity.Value = item.Cells["CityId"].Value.ToString();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
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
			if (grd.RowCount > 0 && GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "GrnId"))
			{
				throw new Exception("You can not update manual Record because record against Grn exist in Grid");
			}
			if (FormDetailValidation())
			{
				dtdetail.Rows[updateDetailIndex]["WareHouseId"] = Conversion.ToInt(CmbWareHouseName.Value);
				dtdetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItemName.Value);
				dtdetail.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value);
				dtdetail.Rows[updateDetailIndex]["Item"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value);
				dtdetail.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(CmbCropYear.Value);
				dtdetail.Rows[updateDetailIndex]["JobId"] = Conversion.ToInt(CmbJobLot.Value);
				dtdetail.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(CmbPackingType.Value);
				dtdetail.Rows[updateDetailIndex]["UOMId"] = Conversion.ToInt(CmbUom.Value);
				dtdetail.Rows[updateDetailIndex]["UOM"] = Conversion.ToString((object)((Control)(object)CmbUom).Text);
				dtdetail.Rows[updateDetailIndex]["UomEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbUom).SelectedRow.Cells[2].Value);
				dtdetail.Rows[updateDetailIndex]["Qty"] = Conversion.ToDouble((object)txtQty.Text);
				dtdetail.Rows[updateDetailIndex]["GrossWight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				dtdetail.Rows[updateDetailIndex]["EbUnit"] = Conversion.ToDouble((object)txtebu.Text);
				dtdetail.Rows[updateDetailIndex]["EbTotal"] = Conversion.ToDouble((object)txtebt.Text);
				dtdetail.Rows[updateDetailIndex]["WtCut"] = Conversion.ToDouble((object)txtwtcut.Text);
				dtdetail.Rows[updateDetailIndex]["WtCutTotal"] = Conversion.ToDouble((object)txtwtcuttotal.Text);
				dtdetail.Rows[updateDetailIndex]["AddLesswt"] = Conversion.ToDouble((object)txtadlswt.Text);
				dtdetail.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDouble((object)txtNetWeight.Text);
				dtdetail.Rows[updateDetailIndex]["StockWeight"] = Conversion.ToDouble((object)txtStockWeight.Text);
				dtdetail.Rows[updateDetailIndex]["CityId"] = Conversion.ToInt(CmbCity.Value);
				Add.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
				((Control)(object)CmbSupplierName).Enabled = grd.RowCount == 0;
			}
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
			ResetDetail();
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
			List<string> hiddenColumns = new List<string>
			{
				"Id", "GrnId", "GrnDetailId", "GrnDocumentTypeId", "GrnNo", "ItemId", "UOMId", "UomEquivalent", "RefDocumentTypeId", "RefDocId",
				"RefDocDetailId", "RefDocType", "RefDocNo", "BalQty", "AvailableStock"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Qty",
					(EditType)1
				},
				{
					"GrossWight",
					(EditType)1
				},
				{
					"EbUnit",
					(EditType)1
				},
				{
					"WtCut",
					(EditType)1
				},
				{
					"AddLesswt",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 2, hiddenColumns, editableColumns);
			CommonSettingInGrid(grd);
			bool GrnExists = GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "GrnId");
			grd.RootTable.Columns["GrnNo"].Visible = GrnExists;
			panel5.Visible = !GrnExists;
			if (GrnExists)
			{
				grd.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
				grd.RootTable.Columns["CropYearId"].EditType = (EditType)0;
				grd.RootTable.Columns["JobId"].EditType = (EditType)0;
				grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			}
			GridEX_Helper.AddButton(grd, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grd, "Add", "+", 20, 1);
			if (StockLoaderVisibleInGDNDetail)
			{
				GridEX_Helper.AddButton(grd, "Loader", "Loader", 65, 2);
				grd.RootTable.Columns["BalQty"].Visible = true;
				grd.RootTable.Columns["AvailableStock"].Visible = true;
				grd.RootTable.Columns["AvailableStock"].ColumnType = (ColumnType)5;
				grd.FrozenColumns = 3;
			}
			else
			{
				grd.FrozenColumns = 2;
			}
			ctrlGrdBar1_Load(null, null);
			((Control)(object)CmbSupplierName).Enabled = grd.RowCount == 0;
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
			if (e.Column.Key == "Qty")
			{
				double Qty2 = Conversion.ToDouble(item.Cells["Qty"].Value);
				double PackUom = Conversion.ToDouble(item.Cells["UomEquivalent"].Value);
				double GrossWight3 = Qty2 * PackUom;
				item.Cells["GrossWight"].Value = GrossWight3;
				double EmptybagsTotal6 = Conversion.ToDouble(item.Cells["EbTotal"].Value);
				double Emptybags2 = Conversion.ToDouble((object)item.Cells["EbUnit"].Value.ToString());
				if (EmptybagsTotal6 == 0.0 && Emptybags2 > 0.0)
				{
					EmptybagsTotal6 = Qty2 * Emptybags2;
					item.Cells["EbTotal"].Value = EmptybagsTotal6;
				}
				else if (Emptybags2 == 0.0 && EmptybagsTotal6 > 0.0)
				{
					Emptybags2 = EmptybagsTotal6 / Qty2;
					item.Cells["EbUnit"].Value = Emptybags2;
				}
				double WtCut2 = Conversion.ToDouble((object)item.Cells["WtCut"].Value.ToString());
				double WtCutTotal6 = Conversion.ToDouble((object)item.Cells["WtCutTotal"].Value.ToString());
				if (WtCutTotal6 == 0.0 && WtCut2 > 0.0)
				{
					WtCutTotal6 = Qty2 * WtCut2;
					item.Cells["WtCutTotal"].Value = WtCutTotal6;
				}
				else if (WtCut2 == 0.0 && WtCutTotal6 > 0.0)
				{
					WtCut2 = WtCutTotal6 / Qty2;
					item.Cells["WtCut"].Value = WtCut2;
				}
				double AddLesswt6 = Conversion.ToDouble((object)item.Cells["AddLesswt"].Value.ToString());
				double NetWeight6 = GrossWight3 - EmptybagsTotal6 - WtCutTotal6 + AddLesswt6;
				item.Cells["NetWeight"].Value = NetWeight6;
				double StockWeight6 = GrossWight3 - EmptybagsTotal6 + AddLesswt6;
				item.Cells["StockWeight"].Value = StockWeight6;
			}
			if (e.Column.Key == "GrossWight")
			{
				double num = Conversion.ToDouble(item.Cells["GrossWight"].Value);
				double EmptybagsTotal5 = Conversion.ToDouble(item.Cells["EbTotal"].Value);
				double AddLesswt5 = Conversion.ToDouble((object)item.Cells["AddLesswt"].Value.ToString());
				double WtCutTotal5 = Conversion.ToDouble((object)item.Cells["WtCutTotal"].Value.ToString());
				double NetWeight5 = num - EmptybagsTotal5 - WtCutTotal5 + AddLesswt5;
				item.Cells["NetWeight"].Value = NetWeight5;
				double StockWeight5 = num - EmptybagsTotal5 + AddLesswt5;
				item.Cells["StockWeight"].Value = StockWeight5;
			}
			if (e.Column.Key == "EbUnit")
			{
				double num2 = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double GrossWight2 = Conversion.ToDouble((object)item.Cells["GrossWight"].Value.ToString());
				double Emptybags = Conversion.ToDouble((object)item.Cells["EbUnit"].Value.ToString());
				double EmptybagsTotal4 = num2 * Emptybags;
				item.Cells["EbTotal"].Value = EmptybagsTotal4;
				double WtCutTotal4 = Conversion.ToDouble((object)item.Cells["WtCutTotal"].Value.ToString());
				double AddLesswt4 = Conversion.ToDouble((object)item.Cells["AddLesswt"].Value.ToString());
				double NetWeight4 = GrossWight2 - EmptybagsTotal4 - WtCutTotal4 + AddLesswt4;
				item.Cells["NetWeight"].Value = NetWeight4;
				double StockWeight4 = GrossWight2 - EmptybagsTotal4 + AddLesswt4;
				item.Cells["StockWeight"].Value = StockWeight4;
			}
			if (e.Column.Key == "EbTotal")
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double num3 = Conversion.ToDouble((object)item.Cells["GrossWight"].Value.ToString());
				double EmptybagsTotal3 = Conversion.ToDouble((object)item.Cells["EbTotal"].Value.ToString());
				_ = EmptybagsTotal3 / Qty;
				double WtCutTotal3 = Conversion.ToDouble((object)item.Cells["WtCutTotal"].Value.ToString());
				double AddLesswt3 = Conversion.ToDouble((object)item.Cells["AddLesswt"].Value.ToString());
				double NetWeight3 = num3 - EmptybagsTotal3 - WtCutTotal3 + AddLesswt3;
				item.Cells["NetWeight"].Value = NetWeight3;
				double StockWeight3 = num3 - EmptybagsTotal3 + AddLesswt3;
				item.Cells["StockWeight"].Value = StockWeight3;
			}
			if (e.Column.Key == "WtCut")
			{
				double num4 = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double GrossWight = Conversion.ToDouble((object)item.Cells["GrossWight"].Value.ToString());
				Conversion.ToDouble((object)item.Cells["EbUnit"].Value.ToString());
				double EmptybagsTotal2 = Conversion.ToDouble((object)item.Cells["EbTotal"].Value.ToString());
				double WtCut = Conversion.ToDouble((object)item.Cells["WtCut"].Value.ToString());
				double WtCutTotal2 = num4 * WtCut;
				item.Cells["WtCutTotal"].Value = WtCutTotal2;
				double AddLesswt2 = Conversion.ToDouble((object)item.Cells["AddLesswt"].Value.ToString());
				double NetWeight2 = GrossWight - EmptybagsTotal2 - WtCutTotal2 + AddLesswt2;
				item.Cells["NetWeight"].Value = NetWeight2;
				double StockWeight2 = GrossWight - EmptybagsTotal2 + AddLesswt2;
				item.Cells["StockWeight"].Value = StockWeight2;
			}
			if (e.Column.Key == "AddLesswt")
			{
				double num5 = Conversion.ToDouble(item.Cells["GrossWight"].Value);
				double EmptybagsTotal = Conversion.ToDouble(item.Cells["EbTotal"].Value);
				double AddLesswt = Conversion.ToDouble((object)item.Cells["AddLesswt"].Value.ToString());
				double WtCutTotal = Conversion.ToDouble((object)item.Cells["WtCutTotal"].Value.ToString());
				double NetWeight = num5 - EmptybagsTotal - WtCutTotal + AddLesswt;
				item.Cells["NetWeight"].Value = NetWeight;
				double StockWeight = num5 - EmptybagsTotal + AddLesswt;
				item.Cells["StockWeight"].Value = StockWeight;
			}
			grd.UpdateData();
			BalGrossWeightGrd();
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
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0 && e.Column.Key == "AvailableStock")
			{
				AvailableStockClickInDetailGrid(row);
			}
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
				if (e.Column.Key == "Delete")
				{
					DeleteRowInDetailGrid(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInDetailGrid(r);
				}
				if (e.Column.Key == "Loader")
				{
					LoaderClickInDetailGrid(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommonSettingInGrid(GridEX grd)
	{
		try
		{
			GridEX_Helper.ComboBind(grd.RootTable.Columns["WareHouseId"], dtWarehouse, "Id", "WareHouseName", "WareHouse Name", 130);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["CropYearId"], dtCrop, "Id", "CropYear", "Crop Year", 100);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["JobId"], dtjob, "Id", "JobLotDescription", "Job Lot", 100);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["PackingTypeId"], dtPack, "Id", "PackTypeDesc", "Packing Type", 100);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["CityId"], dtcity, "Id", "CityName", "City Name", 100);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AvailableStockClickInDetailGrid(GridEXRow r)
	{
		try
		{
			if (!(Conversion.ToDouble(r.Cells["AvailableStock"].Value) > 0.0))
			{
				return;
			}
			List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmStockReport").ToList();
			if (newList.Count > 0)
			{
				if (Conversion.ToBool((object)newList[0].Value.ToString()))
				{
					frmStockReport frmStockReport = new frmStockReport(UserAccount);
					frmStockReport.RequestedbyOtherForm = true;
					frmStockReport.DateTo = DocDate.Value;
					frmStockReport.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					frmStockReport.WarehouseId = Conversion.ToInt(r.Cells["WareHouse"].Value);
					frmStockReport.CropYear = Conversion.ToString((object)r.Cells["CropYear"].Text);
					frmStockReport.JobLotId = Conversion.ToInt(r.Cells["JobId"].Value);
					frmStockReport.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					frmStockReport.UomId = Conversion.ToInt(r.Cells["UOMId"].Value);
					frmStockReport.ShowDialog();
				}
				else
				{
					MessageBox.Show("You don't have right");
				}
			}
			else
			{
				MessageBox.Show("You don't have right");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInDetailGrid(GridEXRow r)
	{
		try
		{
			DataRow dr = dtdetail.Rows[r.RowIndex];
			dtdetail.Rows.Add(dr.ItemArray);
			if (btnUpdate.Visible && btnUpdate.Enabled)
			{
				dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoaderClickInDetailGrid(GridEXRow r)
	{
		try
		{
			int Index = Conversion.ToInt((object)r.RowIndex);
			int BeforeLoadGridRowCount = grd.GetRows().Count();
			if (updateDetailIndex > -1)
			{
				throw new Exception("Reset Detail firts...");
			}
			if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
			{
				throw new Exception("ItemId not found");
			}
			if (Conversion.ToInt(r.Cells["WareHouse"].Value) == 0)
			{
				throw new Exception("WareHouse not found");
			}
			if (string.IsNullOrEmpty(Conversion.ToString(r.Cells["CropYear"].Value)))
			{
				throw new Exception("CropYear not found");
			}
			if (Conversion.ToInt(r.Cells["JobId"].Value) == 0)
			{
				throw new Exception("JobId not found");
			}
			if (Conversion.ToInt(r.Cells["PackingTypeId"].Value) == 0)
			{
				throw new Exception("PackingTypeId not found");
			}
			if (Conversion.ToInt(r.Cells["UOMId"].Value) == 0)
			{
				throw new Exception("UOMId not found");
			}
			LoadTransactions = new LoadavailableTransactionsForGDN(UserAccount);
			LoadTransactions.FromDate = clsGlobalVariables.ActiveYr.Start_Period;
			LoadTransactions.Todate = DocDate.Value;
			LoadTransactions.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
			LoadTransactions.WarehouseId = Conversion.ToInt(r.Cells["WareHouse"].Value);
			LoadTransactions.CropYear = Conversion.ToString((object)r.Cells["CropYear"].Text);
			LoadTransactions.JobLotId = Conversion.ToInt(r.Cells["JobId"].Value);
			LoadTransactions.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
			LoadTransactions.ItemUomId = Conversion.ToInt(r.Cells["UOMId"].Value);
			LoadTransactions.ShowDialog();
			LoadTransactions.BringToFront();
			LoadInGridDetail(r);
			if (grd.GetRows().Count() > BeforeLoadGridRowCount)
			{
				dtdetail.Rows[Index].Delete();
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail(GridEXRow r)
	{
		try
		{
			DataTable dtpurchaseinvoice = LoadTransactions.dtIssuance;
			if (dtpurchaseinvoice.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtpurchaseinvoice.Rows.Count; i++)
			{
				bool flag = false;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]) == Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]) == Conversion.ToInt(item.Cells["RefDocId"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]) == Conversion.ToInt(item.Cells["RefDocDetailId"].Value))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					dtdetail.Rows.Add(0, 0, 0, 0, 0, Conversion.ToInt(r.Cells["WareHouseId"].Value), Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["ItemCode"].Value), Conversion.ToString(r.Cells["Item"].Value), Conversion.ToString(r.Cells["CropYearId"].Value), Conversion.ToInt(r.Cells["JobId"].Value), Conversion.ToInt(r.Cells["PackingTypeId"].Value), Conversion.ToInt(r.Cells["UOMId"].Value), Conversion.ToString(r.Cells["UOM"].Value), Conversion.ToDouble(r.Cells["UomEquivalent"].Value), Conversion.ToDouble(r.Cells["Qty"].Value), Conversion.ToDouble(r.Cells["GrossWight"].Value), Conversion.ToDouble(r.Cells["EbUnit"].Value), Conversion.ToDouble(r.Cells["EbTotal"].Value), Conversion.ToDouble(r.Cells["WtCut"].Value), Conversion.ToDouble(r.Cells["WtCutTotal"].Value), Conversion.ToDouble(r.Cells["AddLesswt"].Value), Conversion.ToDouble(r.Cells["GrossWight"].Value) - Conversion.ToDouble(r.Cells["EbTotal"].Value) - Conversion.ToDouble(r.Cells["WtCutTotal"].Value) + Conversion.ToDouble(r.Cells["AddLesswt"].Value), Conversion.ToDouble(r.Cells["GrossWight"].Value) - Conversion.ToDouble(r.Cells["EbTotal"].Value) - Conversion.ToDouble(r.Cells["WtCutTotal"].Value) + Conversion.ToDouble(r.Cells["AddLesswt"].Value), Conversion.ToInt(r.Cells["CityId"].Value), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["WeightBalance"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["QtyBalance"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]), Conversion.ToString(dtpurchaseinvoice.Rows[i]["RefDocumentType"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["DocCodeNo"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]));
				}
			}
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteRowInDetailGrid(GridEXRow r)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvGdnDetail detail = new InvGdnDetail();
					detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					FillGdnDetailListCommonForInsertAndDelete(detail, r);
					detail.ActionTypeId = 3;
					lstRemoveDetailRecord.Add(detail);
					dtdetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
				grd.UpdateData();
				GetDeliveryScheduleDetailIds();
				((Control)(object)CmbSupplierName).Enabled = grd.RowCount == 0;
				panel5.Visible = grd.RowCount == 0;
				return;
			}
			throw new Exception("Reset Detail first...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable GatepassDataDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			return GatePassOutward.GatePassOutward_PendingForGdnPurchaseReturn(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GatepassGridFill(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count == 0)
			{
				grdGp.DataSource = null;
				return;
			}
			if (dtGpGrid.Columns.Count == 0)
			{
				dtGpGrid.Columns.Add("OutwardGatePassId", typeof(int));
				dtGpGrid.Columns.Add("DocumentTypeId", typeof(int));
				dtGpGrid.Columns.Add("OrderType", typeof(string));
				dtGpGrid.Columns.Add("GatepassType", typeof(string));
				dtGpGrid.Columns.Add("GpDate", typeof(DateTime));
				dtGpGrid.Columns.Add("GpSrNo", typeof(int));
				dtGpGrid.Columns.Add("Status", typeof(string));
				dtGpGrid.Columns.Add("SupplierCustomerId", typeof(int));
				dtGpGrid.Columns.Add("SupplierName", typeof(string));
				dtGpGrid.Columns.Add("ItemName", typeof(string));
				dtGpGrid.Columns.Add("Qty", typeof(double));
				dtGpGrid.Columns.Add("VehicleNo", typeof(string));
				dtGpGrid.Columns.Add("BiltyNo", typeof(string));
				dtGpGrid.Columns.Add("FactoryWeight", typeof(double));
				dtGpGrid.Columns.Add("InTime", typeof(DateTime));
				dtGpGrid.Columns.Add("OutTime", typeof(DateTime));
				dtGpGrid.Columns.Add("WbSlipNos", typeof(string));
				dtGpGrid.Columns.Add("Freight", typeof(double));
				dtGpGrid.Columns.Add("NetPaid", typeof(double));
				dtGpGrid.Columns.Add("FlowValue", typeof(int));
				dtGpGrid.Columns.Add("FlowDescription", typeof(string));
			}
			dtGpGrid.Rows.Clear();
			foreach (DataRow row in dt.Rows)
			{
				dtGpGrid.Rows.Add(row["OutwardGatePassId"], row["DocumentTypeId"], row["OrderType"], row["GatepassType"], row["GpDate"], row["GpSrNo"], row["Status"], row["SupplierCustomerId"], row["SupplierName"], row["ItemName"], row["Qty"], row["VehicleNo"], row["BiltyNo"], row["FactoryWeight"], row["InTime"], row["OutTime"], row["WbSlipNos"], row["Freight"], row["NetPaid"], row["FlowValue"], row["FlowDescription"]);
			}
			grdGp.DataSource = dtGpGrid;
			grdGp.RetrieveStructure();
			GpGridSetting();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GpGridSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "OutwardGatePassId", "DocumentTypeId", "SupplierCustomerId", "WarehouseId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdGp, 3, 2, hiddenColumns);
			GridEX_Helper.AddButton(grdGp, "Load", "Load", 50, 0);
			grdGp.FrozenColumns = 2;
			grdGp.RootTable.Columns["InTime"].FormatString = "hh:mm tt";
			grdGp.RootTable.Columns["OutTime"].FormatString = "hh:mm tt";
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGp.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Load")
			{
				LoadClickInGpGrid(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadClickInGpGrid(GridEXRow row)
	{
		try
		{
			if (!btnSave.Visible || !btnSave.Enabled)
			{
				throw new Exception("Can't Load Gp in Update Mode...Please Reset The Form First..");
			}
			if (Conversion.ToInt(CmbGpNo.Value) != 0)
			{
				throw new Exception("Another Gp Is Already Loaded...Please Reset The Form First..");
			}
			string status = Conversion.ToString(row.Cells["Status"].Value);
			int num = Conversion.ToInt(row.Cells["FlowValue"].Value);
			double factoryWeight = Conversion.ToDouble(row.Cells["FactoryWeight"].Value);
			int supplierId = Conversion.ToInt(row.Cells["SupplierCustomerId"].Value);
			if (num == 1)
			{
				if (factoryWeight == 0.0)
				{
					throw new Exception("Weigh Bridge against this Gp is Not done Yet. Please Make Sure Weigh Bridge entry First");
				}
			}
			else if (!status.Equals("Accepted", StringComparison.OrdinalIgnoreCase))
			{
				throw new Exception("Status Not Accepted. Please check status");
			}
			int gpId = Conversion.ToInt(row.Cells["OutwardGatePassId"].Value);
			string gpSrNo = Conversion.ToString(row.Cells["GpSrNo"].Value);
			GpNoBind(gpId, gpSrNo);
			txtGpDate.Value = Conversion.ToDateTime(row.Cells["GpDate"].Value);
			txtVehicleNo.Text = Conversion.ToString(row.Cells["VehicleNo"].Value);
			txtBiltyNo.Text = Conversion.ToString(row.Cells["BiltyNo"].Value);
			txtFreight.Text = ((Conversion.ToDouble(row.Cells["NetPaid"].Value) > 0.0) ? Conversion.ToString(row.Cells["NetPaid"].Value) : Conversion.ToString(row.Cells["Freight"].Value));
			txtTicketNos.Text = Conversion.ToString(row.Cells["WbSlipNos"].Value);
			txtFactoryWeight.Text = factoryWeight.ToString();
			CmbSupplierName.Value = supplierId;
			CmbSupplierName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GpNoBind(int GpId, string GpNo)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("GpNo", typeof(string));
			dt.Rows.Add(GpId, GpNo);
			InfragisticsHelper.BindAndRetainSelection(CmbGpNo, dt, "Id", "GpNo", "Gp No");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ValidateDetailRecordsWithorWithoutLoader()
	{
		try
		{
			bool AnyRowFromLoader = false;
			int indexOfRowFromLoader = -1;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				int refDocumentTypeId = Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value);
				int refDocIdNo2 = Conversion.ToInt(r2.Cells["RefDocId"].Value);
				int refDocSubIdNo2 = Conversion.ToInt(r2.Cells["RefDocDetailId"].Value);
				if (refDocumentTypeId > 0 || refDocIdNo2 > 0 || refDocSubIdNo2 > 0)
				{
					indexOfRowFromLoader = r2.RowIndex;
					AnyRowFromLoader = true;
					break;
				}
			}
			if (!AnyRowFromLoader)
			{
				return;
			}
			rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				int num = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
				int refDocIdNo = Conversion.ToInt(r.Cells["RefDocId"].Value);
				int refDocSubIdNo = Conversion.ToInt(r.Cells["RefDocDetailId"].Value);
				if (num == 0 || refDocIdNo == 0 || refDocSubIdNo == 0)
				{
					throw new Exception("All rows in the detail grid must either be from the loader or none should be from the loader.\n" + $"Row No: {r.RowIndex + 1} does not have loader values, while Row No: {indexOfRowFromLoader + 1} has loader values.");
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnRecordsUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dtIds = InvGdn.GetGdnIdsForAutoUpdation(UserAccount.OrganizationId, UserAccount.CompanyId, 703);
			if (dtIds != null && dtIds.Rows.Count > 0)
			{
				bool ShouldStop = false;
				for (int i = 0; i < dtIds.Rows.Count; i++)
				{
					int RecId = Conversion.ToInt(dtIds.Rows[i]["Id"]);
					ReadById(RecId);
					ShouldStop = InsertOrAutoUpdate(RecId, isAutoUpdate: true);
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

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
			InsertOrAutoUpdate();
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
			if (Id == 0)
			{
				throw new Exception("Record not update because Id not found");
			}
			InsertOrAutoUpdate(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool InsertOrAutoUpdate(int? id = null, bool isAutoUpdate = false)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return false;
			}
			InvGdn GRN = new InvGdn();
			if (id.HasValue)
			{
				if (!isAutoUpdate && MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return false;
				}
				GRN.Id = id.Value;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return false;
			}
			if (Conversion.ToDouble((object)txtFreight.Text) > 0.0)
			{
				if (((UltraGridBase)CmbTransport).ActiveRow == null || Conversion.ToInt(CmbTransport.Value) == 0)
				{
					MessageBox.Show("Transporter Account field required");
					CmbTransport.Focus();
					return false;
				}
				GRN.TransporterId = Conversion.ToInt(CmbTransport.Value);
				GRN.CarriageAmount = Conversion.ToDouble((object)txtFreight.Text);
				GRN.TransporterSupCustId = (SubsidiaryAccountAllownOnVouchers ? Conversion.ToInt(((UltraDropDownBase)CmbTransport).SelectedRow.Cells["SupplierCustomerId"].Value) : 0);
			}
			ValidateDetailRecordsWithorWithoutLoader();
			GRN.OrganizationId = UserAccount.OrganizationId;
			GRN.CompanyId = UserAccount.CompanyId;
			GRN.BranchesId = UserAccount.BranchesId;
			GRN.ProjectsId = UserAccount.BranchesId;
			GRN.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			GRN.DocumentTypeId = 703;
			GRN.EntryUser = UserAccount.ID;
			GRN.ModifyUser = UserAccount.ID;
			GRN.EntryDate = DateTime.Now;
			GRN.ModifyDate = DateTime.Now;
			GRN.IsApproved = false;
			GRN.ScreenName = ScreenName;
			GRN.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			GRN.DocDate = DocDate.Value;
			GRN.SupplierCustomerId = Conversion.ToInt(CmbSupplierName.Value);
			GRN.OutwardGatePassId = Conversion.ToInt(CmbGpNo.Value);
			GRN.GpNo = Conversion.ToInt((object)((Control)(object)CmbGpNo).Text);
			GRN.CarriageAmount = Conversion.ToDouble((object)txtFreight.Text.Trim());
			GRN.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text.Trim());
			GRN.BiltyNo = txtBiltyNo.Text.Trim();
			GRN.FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
			GRN.RemarksHeader = txtremarks.Text.Trim();
			GRN.AutoUpdateId = (isAutoUpdate ? 1 : 0);
			GRN.invGdnDetail = new List<InvGdnDetail>();
			GRN.InvGdnExpenseslist = new List<InvGdnExpense>();
			double GrossWt = 0.0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvGdnDetail detail = new InvGdnDetail();
				detail.Id = ((Id != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				detail.ActionTypeId = ((detail.Id <= 0) ? 1 : 2);
				FillGdnDetailListCommonForInsertAndDelete(detail, r);
				if (detail.WarehouseId == 0)
				{
					throw new Exception("Warehouse Required in Detail Grid And row No: " + (r.RowIndex + 1));
				}
				if (detail.ItemId == 0)
				{
					throw new Exception("Item Required in Detail Grid And row No: " + (r.RowIndex + 1));
				}
				if (detail.CropYearId == 0)
				{
					throw new Exception("CropYear Required in Detail Grid And row No: " + (r.RowIndex + 1));
				}
				if (detail.JobLotId == 0)
				{
					throw new Exception("JobLot Required in Detail Grid And row No: " + (r.RowIndex + 1));
				}
				if (detail.PackingTypeId == 0)
				{
					throw new Exception("PackingType Required in Detail Grid And row No: " + (r.RowIndex + 1));
				}
				if (detail.ItemUomId == 0)
				{
					throw new Exception("Uom Required in Detail Grid And row No: " + (r.RowIndex + 1));
				}
				if (detail.ItemQty == 0.0)
				{
					throw new Exception("Qty Required in Detail Grid And row No: " + (r.RowIndex + 1));
				}
				if (detail.GrossWeight == 0.0)
				{
					throw new Exception("GrossWeight Required in Detail Grid And row No: " + (r.RowIndex + 1));
				}
				if (detail.NetBillWeight == 0.0)
				{
					throw new Exception("NetBillWeight Required in Detail Grid And row No: " + (r.RowIndex + 1));
				}
				if (detail.StockWeight == 0.0)
				{
					throw new Exception("StockWeight Required in Detail Grid And row No: " + (r.RowIndex + 1));
				}
				GrossWt += detail.GrossWeight;
				GRN.invGdnDetail.Add(detail);
			}
			if (GRN.invGdnDetail.Count == 0)
			{
				throw new Exception("At least enter value/quantity in one of the rows of detail ");
			}
			if (lstRemoveDetailRecord.Count > 0 && Id > 0)
			{
				for (int i = 0; i < lstRemoveDetailRecord.Count; i++)
				{
					GRN.invGdnDetail.Add(lstRemoveDetailRecord[i]);
				}
			}
			double FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
			if (Math.Round(GrossWt, 2) != Math.Round(FactoryWeight, 2))
			{
				throw new Exception($"Mismatch in weights: Gross weight entered is {Math.Round(GrossWt, 2)}, but it should be equal to the factory weight {Math.Round(FactoryWeight, 2)} '.");
			}
			rows = GridExpense.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["ItemId"].Value) > 0 || Conversion.ToDouble(item.Cells["Qty"].Value) > 0.0)
				{
					InvGdnExpense obj = new InvGdnExpense();
					obj.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
					obj.Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
					obj.Remarks = Conversion.ToString(item.Cells["Remarks"].Value);
					GRN.InvGdnExpenseslist.Add(obj);
				}
			}
			GRN.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			GRN.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvGdn>(GRN, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			GRN.AttachmentsValues = AttachmentsValues;
			GRN.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = InvGdn.Save(GRN);
			string action = (id.HasValue ? "Update" : "Save");
			MessageBox.Show($"Record {action} Successfully: {GRN.DocNo}");
			if (id.HasValue && AT.lst.Count == 0)
			{
				global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(GRN.Id, ScreenName, 703, 0);
			}
			if (!isAutoUpdate)
			{
				if (WagesStatus && WagesActiveOrInActive)
				{
					frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
					frmwagesBillHeader.RefDocTypeId = 703;
					frmwagesBillHeader.RefDocId = success;
					frmwagesBillHeader.GrossWeightTotal = GrossWt;
					frmwagesBillHeader.ShowDialog();
				}
				reset();
				if (ChkBox.Checked)
				{
					CommonServices.Gdn_PurchaseReturnSlip703(success);
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			reset();
			Id = Conversion.ToInt((object)ID);
			InvGdn invGrn = InvGdn.GetByID(Id);
			if (invGrn != null)
			{
				txtdocno.Text = invGrn.DocNo.ToString();
				DocDate.Value = invGrn.DocDate;
				tabControl1.SelectedIndex = 0;
				BindSupplierName();
				CmbSupplierName.Value = invGrn.SupplierCustomerId;
				txtTicketNos.Text = invGrn.TicketNos;
				CmbTransport.Value = invGrn.TransporterId;
				txtFreight.Text = invGrn.CarriageAmount.ToString();
				txtFactoryWeight.Text = Conversion.ToString((object)invGrn.FactoryWeight);
				((Control)(object)CmbGpNo).Enabled = false;
				txtVehicleNo.Text = Conversion.ToString((object)invGrn.VehicleNo);
				txtBiltyNo.Text = Conversion.ToString((object)invGrn.BiltyNo);
				txtremarks.Text = Conversion.ToString((object)invGrn.RemarksHeader);
				GpNoBind(invGrn.OutwardGatePassId, Conversion.ToString((object)invGrn.GpNo));
				((Control)(object)CmbGpNo).Text = Conversion.ToString((object)invGrn.GpNo);
				AttachmentsValues = invGrn.AttachmentsValues;
				CustomAttachmentsValues = invGrn.CustomAttachmentsValues;
				dtdetail.Rows.Clear();
				FilldtDetailFromListCommonForReadById(invGrn, dtdetail);
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings();
				dtExpense.Rows.Clear();
				if (invGrn.InvGdnExpenseslist != null && invGrn.InvGdnExpenseslist.Count > 0)
				{
					for (int i = 0; i < invGrn.InvGdnExpenseslist.Count; i++)
					{
						dtExpense.Rows.Add(invGrn.InvGdnExpenseslist[i].ItemId, invGrn.InvGdnExpenseslist[i].Qty, invGrn.InvGdnExpenseslist[i].Remarks);
					}
				}
				if (dtExpense.Rows.Count == 0)
				{
					AddRowInExpenseGrid();
				}
				GridExpense.DataSource = dtExpense;
				GridExpense.RetrieveStructure();
				grdInvExpSettings();
				FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				btnDelete.Visible = true;
			}
			else
			{
				reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 703,
						Id = Id,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Successfully");
					reset();
				}
				return;
			}
			throw new Exception("Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FilldtDetailFromListCommonForReadById(InvGdn invGrn, DataTable dt, bool AddExtraColumns = false)
	{
		if (invGrn == null || invGrn.invGdnDetail == null || invGrn.invGdnDetail.Count <= 0)
		{
			return;
		}
		foreach (InvGdnDetail detail in invGrn.invGdnDetail)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.GrnId, detail.GrnDetailId, detail.GrnDocumentTypeId, detail.GrnNo, detail.WarehouseId, detail.ItemId, detail.ItemCode, detail.Item, detail.CropYearId,
				detail.JobLotId, detail.PackingTypeId, detail.ItemUomId, detail.UOMCode, detail.UOMEquivalent, detail.ItemQty, detail.GrossWeight, detail.EBWPerUnit, detail.EBWTotal, detail.WtCut,
				detail.WtCutTotal, detail.AdLsWeight, detail.NetBillWeight, detail.StockWeight, detail.CityId, 0, detail.ItemQty, detail.RefDocumentTypeId, detail.RefDocType, detail.RefDocIdNo,
				detail.RefDocNo, detail.RefDocSubIdNo
			};
			if (AddExtraColumns)
			{
				rowValues.Add(detail.WareHouseCode);
				rowValues.Add(detail.CropYear);
				rowValues.Add(detail.JobLot);
				rowValues.Add(detail.PackingType);
				rowValues.Add(detail.AreaCity);
			}
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FillGdnDetailListCommonForInsertAndDelete(InvGdnDetail detail, GridEXRow r)
	{
		detail.GrnId = Conversion.ToInt(r.Cells["GrnId"].Value);
		detail.GrnDetailId = Conversion.ToInt(r.Cells["GrnDetailId"].Value);
		detail.GrnDocumentTypeId = Conversion.ToInt(r.Cells["GrnDocumentTypeId"].Value);
		detail.GrnNo = Conversion.ToInt(r.Cells["GrnNo"].Value);
		detail.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
		detail.WareHouseCode = Conversion.ToString((object)r.Cells["WarehouseId"].Text);
		detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		detail.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		detail.Item = Conversion.ToString(r.Cells["Item"].Value);
		detail.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
		detail.CropYear = Conversion.ToString((object)r.Cells["CropYearId"].Text);
		detail.JobLotId = Conversion.ToInt(r.Cells["JobId"].Value);
		detail.JobLot = Conversion.ToString((object)r.Cells["JobId"].Text);
		detail.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
		detail.PackingType = Conversion.ToString((object)r.Cells["PackingTypeId"].Text);
		detail.ItemUomId = Conversion.ToInt(r.Cells["UOMId"].Value);
		detail.UOMCode = Conversion.ToString(r.Cells["UOM"].Value);
		detail.UOMEquivalent = Conversion.ToDouble(r.Cells["UomEquivalent"].Value);
		detail.ItemQty = Conversion.ToDouble(r.Cells["Qty"].Value);
		detail.GrossWeight = Conversion.ToDouble(r.Cells["GrossWight"].Value);
		detail.EBWPerUnit = Conversion.ToDouble(r.Cells["EbUnit"].Value);
		detail.EBWTotal = Conversion.ToDouble(r.Cells["EbTotal"].Value);
		detail.WtCut = Conversion.ToDouble(r.Cells["WtCut"].Value);
		detail.WtCutTotal = Conversion.ToDouble(r.Cells["WtCutTotal"].Value);
		detail.AdLsWeight = Conversion.ToDouble(r.Cells["AddLesswt"].Value);
		detail.NetBillWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
		detail.StockWeight = Conversion.ToDouble(r.Cells["StockWeight"].Value);
		detail.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
		detail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
		detail.RefDocType = Conversion.ToString(r.Cells["RefDocType"].Value);
		detail.RefDocIdNo = Conversion.ToInt(r.Cells["RefDocId"].Value);
		detail.RefDocNo = Conversion.ToInt(r.Cells["RefDocNo"].Value);
		detail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocDetailId"].Value);
		detail.GpNo = Conversion.ToInt((object)((Control)(object)CmbGpNo).Text);
		detail.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text);
		detail.GpDate = Conversion.ToDateTime((object)txtGpDate.Value);
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSupplierName).ActiveRow == null || Conversion.ToInt(CmbSupplierName.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbSupplierName.Focus();
			return false;
		}
		if (((Control)(object)CmbGpNo).Text.Trim() == "" || ((Control)(object)CmbGpNo).Text.Trim() == "0" || ((Control)(object)CmbGpNo).Text.Trim() == null)
		{
			MessageBox.Show("Gate pass Field is Required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbGpNo.Focus();
			return false;
		}
		if (txtFactoryWeight.Text.Trim() == "" || txtFactoryWeight.Text.Trim() == "0" || txtFactoryWeight.Text.Trim() == null)
		{
			MessageBox.Show("FactoryWeight Field is Required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtFactoryWeight.Focus();
			return false;
		}
		return true;
	}

	private bool FormDetailValidation()
	{
		if (((UltraGridBase)CmbWareHouseName).ActiveRow == null || Conversion.ToInt(CmbWareHouseName.Value) == 0)
		{
			MessageBox.Show("Warehouse Field is Required");
			CmbWareHouseName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemName).ActiveRow == null || Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("Item Field is Required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null || Conversion.ToInt(CmbCropYear.Value) == 0)
		{
			MessageBox.Show("Crop Year Field is Required");
			CmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("Job/Lot Field is Required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingType).ActiveRow == null || Conversion.ToInt(CmbPackingType.Value) == 0)
		{
			MessageBox.Show("Packing Type Field is Required");
			CmbPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbUom).ActiveRow == null || Conversion.ToInt(CmbUom.Value) == 0)
		{
			MessageBox.Show("Pack Unit Field is Required");
			CmbUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtQty.Focus();
			return false;
		}
		if (txtGrossWeight.Text.Trim() == "" || Conversion.ToDouble((object)txtGrossWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Gross Weight Field is Required");
			txtGrossWeight.Focus();
			return false;
		}
		if (txtNetWeight.Text.Trim() == "" || Conversion.ToDouble((object)txtNetWeight.Text.Trim()) < 0.0)
		{
			MessageBox.Show("NetBillWeight Field is Empty or Less than zero Please Check");
			txtNetWeight.Focus();
			return false;
		}
		if (txtStockWeight.Text.Trim() == "" || Conversion.ToDouble((object)txtStockWeight.Text.Trim()) < 0.0)
		{
			MessageBox.Show("StockWeight Field is Empty or Less than zero Please Check");
			txtStockWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtGrossWeight.Text.Trim()) < Conversion.ToDouble((object)txtNetWeight.Text.Trim()))
		{
			MessageBox.Show("Gross Weight cannot be less than Net Weight");
			txtGrossWeight.Focus();
			return false;
		}
		return true;
	}

	private void reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			lstRemoveDetailRecord.Clear();
			GrnDetailIds = "";
			Id = 0;
			txtdocno.Text = string.Empty;
			((Control)(object)CmbSupplierName).Text = string.Empty;
			((UltraGridBase)CmbGpNo).DataSource = null;
			((Control)(object)CmbGpNo).Text = string.Empty;
			((Control)(object)CmbTransport).Text = string.Empty;
			txtFreight.Text = string.Empty;
			txtFactoryWeight.Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			txtBiltyNo.Text = string.Empty;
			txtTicketNos.Text = string.Empty;
			txtremarks.Text = string.Empty;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			DocDate.Focus();
			((Control)(object)CmbItemName).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtebu.Text = string.Empty;
			txtebt.Text = string.Empty;
			txtadlswt.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtStockWeight.Text = string.Empty;
			((Control)(object)CmbUom).Text = string.Empty;
			updateDetailIndex = -1;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			Add.Visible = true;
			dtdetail.Rows.Clear();
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			dtExpense.Rows.Clear();
			AddRowInExpenseGrid();
			GridExpense.DataSource = dtExpense;
			GridExpense.RetrieveStructure();
			grdInvExpSettings();
			UpdateDocumentNoUI(Conversion.ToString((object)CommonServices.InvGdnGenerateCode(703)));
			GatepassGridFill(GatepassDataDbCall());
			defaultConfiquration();
			panel5.Visible = true;
			((Control)(object)CmbSupplierName).Enabled = grd.RowCount == 0;
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
			txtQty.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtebu.Text = string.Empty;
			txtebt.Text = string.Empty;
			txtadlswt.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtStockWeight.Text = string.Empty;
			((Control)(object)CmbUom).Text = string.Empty;
			updateDetailIndex = -1;
			CmbItemName.Focus();
			Add.Visible = true;
			btnCancelUpdateDetial.Visible = false;
			btnUpdateDetail.Visible = false;
			((Control)(object)CmbSupplierName).Enabled = grd.RowCount == 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmGdnForPurchaseReturn_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			waitForm.Close();
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "MultiCurrency", "AllCities", "CropYear", "JobLot", "InvPackingType" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 703);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			SupplierDtFillDbCall();
			BindTransporterDbCall();
			ItemDtFillFromPurchaseDbCall(Conversion.ToInt(CmbSupplierName.Value));
			OtherItemdtFillDbCall();
			BindSupplierName();
			TransporterAcFill();
			WarehousesDtFillFromGlobalAndBind();
			ItemDetailBind();
			CropDtFillFromGlobalAndBind();
			JobLotDtFillFromGlobalAndBind();
			PackingTypeDtFillFromGlobalAndBind();
			CityDtFillFromGlobalAndBind();
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, DataGridHistory, dtHistory);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grd, dtdetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdGp, dtGpGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void UpdateAvailableStockLabelValue()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		try
		{
			double AvailableStock = GetAvgRatesAndStockInHand.GetStockInHandFromInventoryTrasactions(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocDate = DocDate.Value,
				WarehouseId = Conversion.ToInt(CmbWareHouseName.Value),
				ItemId = Conversion.ToInt(CmbItemName.Value),
				JobLotId = Conversion.ToInt(CmbJobLot.Value),
				CropYear = Conversion.ToString((object)((Control)(object)CmbCropYear).Text),
				ItemUomId = Conversion.ToInt(CmbUom.Value)
			});
			if (AvailableStock > 0.0)
			{
				lblStock.Text = AvailableStock.ToString();
			}
			else
			{
				lblStock.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combwr_Leave(object sender, EventArgs e)
	{
		UpdateAvailableStockLabelValue();
	}

	private void combjoblot_Leave(object sender, EventArgs e)
	{
		UpdateAvailableStockLabelValue();
	}

	private void CmbCropYear_Leave(object sender, EventArgs e)
	{
		UpdateAvailableStockLabelValue();
	}

	private void CmbPackingType_Leave(object sender, EventArgs e)
	{
		UpdateAvailableStockLabelValue();
	}

	private void Total()
	{
		try
		{
			double qty = 0.0;
			double ebUnit = 0.0;
			double ebTotal = 0.0;
			double weightCut = 0.0;
			double weightCutTotal = 0.0;
			double addLess = 0.0;
			if (((UltraGridBase)CmbUom).ActiveRow != null)
			{
				Conversion.ToDouble(((UltraDropDownBase)CmbUom).SelectedRow.Cells["Equivalent"].Value);
			}
			qty = Conversion.ToDouble((object)txtQty.Text);
			double num = Conversion.ToDouble((object)txtGrossWeight.Text);
			ebUnit = Conversion.ToDouble((object)txtebu.Text);
			ebTotal = Conversion.ToDouble((object)txtebt.Text);
			Conversion.ToDouble((object)txtNetWeight.Text);
			Conversion.ToDouble((object)txtStockWeight.Text);
			weightCut = Conversion.ToDouble((object)txtwtcut.Text);
			addLess = Conversion.ToDouble((object)txtadlswt.Text);
			string tag = Conversion.ToString(base.ActiveControl?.Tag);
			if (tag == "PmUnit")
			{
				ebTotal = ebUnit * qty;
				txtebt.Text = ebTotal.ToString("#,##0.###");
			}
			else if (tag == "PmTotal")
			{
				ebUnit = ((qty != 0.0) ? (ebTotal / qty) : 0.0);
				txtebu.Text = ebUnit.ToString("#,##0.####");
			}
			else
			{
				ebUnit = ((qty != 0.0) ? (ebTotal / qty) : 0.0);
				txtebu.Text = ebUnit.ToString("#,##0.####");
			}
			if (qty > 0.0 && weightCut > 0.0)
			{
				weightCutTotal = qty * weightCut;
				txtwtcuttotal.Text = weightCutTotal.ToString("#,##0.###");
			}
			else
			{
				txtwtcuttotal.Text = "0";
			}
			ebTotal = ebUnit * qty;
			txtebt.Text = ebTotal.ToString("#,##0.###");
			double NetWeight = num - ebTotal - weightCutTotal + addLess;
			txtNetWeight.Text = NetWeight.ToString("#,##0.###");
			double StockWeight = num - ebTotal + addLess;
			txtStockWeight.Text = StockWeight.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalWeight()
	{
		try
		{
			double PackUOM = 0.0;
			double Qty = 0.0;
			double GrossWeight = 0.0;
			if (((UltraGridBase)CmbUom).ActiveRow != null)
			{
				PackUOM = Conversion.ToDouble((object)((UltraDropDownBase)CmbUom).SelectedRow.Cells["Equivalent"].Value.ToString());
			}
			if (txtQty.Text != string.Empty)
			{
				Qty = Conversion.ToDouble((object)txtQty.Text.ToString());
			}
			if (txtGrossWeight.Text != string.Empty)
			{
				GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text.ToString());
			}
			txtGrossWeight.Text = (PackUOM * Qty).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combpckuom_Leave(object sender, EventArgs e)
	{
		TotalWeight();
		Total();
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		TotalWeight();
		Total();
	}

	private void txtgwt_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtebu_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtebt_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtadlswt_TextChanged_1(object sender, EventArgs e)
	{
		Total();
	}

	private void txtadlswt_Leave(object sender, EventArgs e)
	{
		Total();
	}

	private void txtfctwt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			BalGrossWeightGrd();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BalGrossWeightGrd()
	{
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				txtBalGrossWeightGrid.Text = "0";
				return;
			}
			double totalGrossWeight = GridEX_Helper.GetColumnSum(grd, "GrossWight");
			double balGrossWeight = Conversion.ToDouble((object)txtFactoryWeight.Text) - totalGrossWeight;
			txtBalGrossWeightGrid.Text = balGrossWeight.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddLessHeader_TextChanged(object sender, EventArgs e)
	{
		GridAddless();
	}

	private void GridAddless()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grd.CurrentRow;
			if (row == null || (int)row.RowType != 0 || row.Cells.Count <= 0)
			{
				return;
			}
			grd.UpdateData();
			if (grd.CurrentRow.Cells.Count > 0)
			{
				double Addless = Conversion.ToDouble((object)txtAddLessHeader.Text);
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					item.BeginEdit();
					if (Conversion.ToDouble(item.Cells["AddLesswt"].Value) == 0.0)
					{
						item.Cells["AddLesswt"].Value = Addless.ToString();
					}
					item.EndEdit();
				}
				rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (r != null && (int)r.RowType == 0 && r.Cells["GrossWight"].Value.ToString() != string.Empty && r.Cells["NetWeight"].Value.ToString() != string.Empty)
					{
						double num = Conversion.ToDouble((object)r.Cells["GrossWight"].Value.ToString());
						double EbTotal = Conversion.ToDouble((object)r.Cells["EbTotal"].Value.ToString());
						double AddLesswt = Conversion.ToDouble((object)r.Cells["AddLesswt"].Value.ToString());
						double diff = num - EbTotal + AddLesswt;
						r.Cells["NetWeight"].Value = diff.ToString();
						r.Cells["StockWeight"].Value = diff.ToString();
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

	private void chkPropBalGrossInGrid_Click(object sender, EventArgs e)
	{
		try
		{
			PropotionateBalGrossWeightInGrd();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PropotionateBalGrossWeightInGrd()
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow[] rows = grd.GetRows();
			if (rows.Count() == 0)
			{
				MessageBox.Show("Grid record not found...");
				return;
			}
			double FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text);
			double totalGrossWeight = GridEX_Helper.GetColumnSum(grd, "GrossWight");
			if (Math.Round(FactoryWeight, 2) == Math.Round(totalGrossWeight, 2))
			{
				MessageBox.Show($"Already FactoryWeight {Math.Round(FactoryWeight, 2)} is Equal To Total GrossWeight in Grid {Math.Round(totalGrossWeight, 2)}...");
				return;
			}
			double balGrossWeight = Conversion.ToDouble((object)txtBalGrossWeightGrid.Text);
			if (FactoryWeight > totalGrossWeight)
			{
				double NowGrossWeight = totalGrossWeight;
				GridEXRow[] array = rows;
				foreach (GridEXRow row in array)
				{
					if (row != null && (int)row.RowType == 0)
					{
						row.BeginEdit();
						double currentGross = Conversion.ToDouble(row.Cells["GrossWight"].Value);
						double proportionalIncrease = ((totalGrossWeight > 0.0) ? (balGrossWeight / totalGrossWeight) : 0.0) * currentGross;
						double updatedGross = currentGross + proportionalIncrease;
						row.Cells["GrossWight"].Value = updatedGross;
						double ebTotal = Conversion.ToDouble(row.Cells["EbTotal"].Value);
						double addLess = Conversion.ToDouble(row.Cells["AddLesswt"].Value);
						double netWeight = updatedGross - ebTotal + addLess;
						row.Cells["NetWeight"].Value = netWeight;
						row.Cells["StockWeight"].Value = netWeight;
						NowGrossWeight += proportionalIncrease;
						if (FactoryWeight == NowGrossWeight)
						{
							break;
						}
						row.EndEdit();
					}
				}
			}
			else if (FactoryWeight < totalGrossWeight)
			{
				double NowGrossWeight2 = totalGrossWeight;
				GridEXRow[] array = rows;
				foreach (GridEXRow row2 in array)
				{
					if (row2 != null && (int)row2.RowType == 0)
					{
						row2.BeginEdit();
						double currentGross2 = Conversion.ToDouble(row2.Cells["GrossWight"].Value);
						double proportionalDecrease = ((totalGrossWeight > 0.0) ? (balGrossWeight / totalGrossWeight) : 0.0) * currentGross2;
						double updatedGross2 = currentGross2 + proportionalDecrease;
						row2.Cells["GrossWight"].Value = updatedGross2;
						double ebTotal2 = Conversion.ToDouble(row2.Cells["EbTotal"].Value);
						double addLess2 = Conversion.ToDouble(row2.Cells["AddLesswt"].Value);
						double netWeight2 = updatedGross2 - ebTotal2 + addLess2;
						row2.Cells["NetWeight"].Value = netWeight2;
						row2.Cells["StockWeight"].Value = netWeight2;
						NowGrossWeight2 += proportionalDecrease;
						if (FactoryWeight == NowGrossWeight2)
						{
							break;
						}
						row2.EndEdit();
					}
				}
			}
			grd.UpdateData();
			BalGrossWeightGrd();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerateAvailableStock_Click(object sender, EventArgs e)
	{
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		try
		{
			GridEXRow[] rows = grd.GetRows();
			if (rows.Count() == 0)
			{
				return;
			}
			bool fifoFeature = CommonServices.GetERPFeatureById(5);
			GridEXRow[] array = rows;
			foreach (GridEXRow obj in array)
			{
				int itemId = Conversion.ToInt(obj.Cells["ItemId"].Value);
				int warehouseId = Conversion.ToInt(obj.Cells["WareHouseId"].Value);
				string cropYear = Conversion.ToString((object)obj.Cells["CropYearId"].Text);
				int jobLotId = Conversion.ToInt(obj.Cells["JobId"].Value);
				int packingTypeId = Conversion.ToInt(obj.Cells["PackingTypeId"].Value);
				int uomId = Conversion.ToInt(obj.Cells["UOMId"].Value);
				ReportsParameters parameters = new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ItemId = itemId,
					DocDate = DocDate.Value,
					WarehouseId = warehouseId,
					CropYear = cropYear,
					JobLotId = jobLotId,
					PackingTypeId = packingTypeId,
					stockUOM = uomId
				};
				double availableWeight = 0.0;
				if (fifoFeature)
				{
					DataTable dt = GetAvgRatesAndStockInHand.GetStockByFifoMethod(parameters);
					if (dt.Rows.Count > 0)
					{
						availableWeight = dt.AsEnumerable().Sum((DataRow r) => r.Field<double>("BalWeight"));
					}
				}
				else
				{
					parameters.ActionId = 1;
					DataTable dt2 = GetAvgRatesAndStockInHand.GetAvailableStock(parameters);
					if (dt2.Rows.Count > 0)
					{
						availableWeight = Conversion.ToDouble(dt2.Rows[0]["BalWeight"]);
					}
				}
				obj.BeginEdit();
				obj.Cells["AvailableStock"].Value = availableWeight;
				obj.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnPrint260A_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.Gdn_PurchaseReturnDeliveryChallanSlip703A(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void printToolStripButton_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.Gdn_PurchaseReturnSlip703(Id);
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
			else
			{
				DocDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGdnFormHistory_Click(object sender, EventArgs e)
	{
		try
		{
			frmGDNHistory obj = new frmGDNHistory(UserAccount);
			if (clsGlobalVariables.ScreenViewReights.Count > 0)
			{
				if (Conversion.ToBool((object)clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmGDNHistory").ToList()[0].Value.ToString()))
				{
					obj.CmbSupplier.Value = Conversion.ToInt(CmbCustomerHistory.Value);
					obj.txtGdnNoFrom.Text = Conversion.ToString((object)FromDocNoHistory.Text);
					obj.txtGdnNoTo.Text = Conversion.ToString((object)ToDocNoHistory.Text);
					obj.datGdnFromDate.Value = Conversion.ToDateTime((object)FromDateHistory.Value);
					obj.dateGdnToDate.Value = Conversion.ToDateTime((object)ToDateHistory.Value);
					obj.ShowDialog();
				}
				else
				{
					MessageBox.Show("Please Check Screen Rights");
				}
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
			ResetHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetHistory()
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
			DatagridHistoryDetail.DataSource = null;
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private DataTable HistoryCombosdtDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			return InvGdn.GetDataForDropDownFromGdn(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesIds = Conversion.ToString((object)UserAccount.BranchesId),
				Activity = "Supplier",
				DocumentTypeIds = "703"
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
			int Id = Conversion.ToInt(CmbCustomerHistory.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dt, "Id", "ReferenceName", "Supplier Name", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind(HistoryCombosdtDbCall());
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
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = 703;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
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
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dtHistry = InvGdn.GetHisoty(obj);
			if (dtHistry.Rows.Count > 0)
			{
				if (dtHistory.Columns.Count == 0)
				{
					dtHistory.Columns.Add("Id", typeof(int));
					dtHistory.Columns.Add("DocDate", typeof(DateTime));
					dtHistory.Columns.Add("DocNo", typeof(int));
					dtHistory.Columns.Add("DocumentTypeId", typeof(int));
					dtHistory.Columns.Add("SupplierCustomerId", typeof(int));
					dtHistory.Columns.Add("SupplierName", typeof(string));
					dtHistory.Columns.Add("GpId", typeof(int));
					dtHistory.Columns.Add("GpNo", typeof(int));
					dtHistory.Columns.Add("VehicleNo", typeof(string));
					dtHistory.Columns.Add("BiltyNo", typeof(string));
					dtHistory.Columns.Add("FactoryWeight", typeof(double));
					dtHistory.Columns.Add("AccountTitle", typeof(string));
					dtHistory.Columns.Add("Freight", typeof(double));
					dtHistory.Columns.Add("NetPaid", typeof(double));
					dtHistory.Columns.Add("RemarksHeader", typeof(string));
					dtHistory.Columns.Add("EntryUser", typeof(string));
					dtHistory.Columns.Add("EntryDate", typeof(DateTime));
					dtHistory.Columns.Add("ModifyUser", typeof(string));
					dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistory.Columns.Add("ApprovedUser", typeof(string));
					dtHistory.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistory.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtHistory.Rows.Clear();
				foreach (DataRow dr in dtHistry.Rows)
				{
					dtHistory.Rows.Add(dr["Id"], dr["DocDate"], dr["DocNo"], dr["DocumentTypeId"], dr["SupplierCustomerId"], dr["CustomerName"], dr["GpId"], dr["GpNo"], dr["VehicleNo"], dr["BiltyNo"], dr["FactoryWeight"], dr["AccountTitle"], dr["Freight"], dr["NetPaid"], dr["RemarksHeader"], dr["UserName"], dr["EntryDate"], dr["ModifyUserName"], dr["ModifyDate"], dr["ApprovedUserName"], dr["ApprovedDate"], dr["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtHistory;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
				DatagridHistoryDetail.ClearStructure();
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
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "SupplierCustomerId", "GpId" };
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 3, 2, hiddenColumns);
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedUser"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
			GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 50, 0);
			GridEX_Helper.AddButton(DataGridHistory, "Print", "703-Print", 80, 1);
			GridEX_Helper.AddButton(DataGridHistory, "Print1", "703A-Print", 80, 2);
			DataGridHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
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
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
			{
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(r.Cells["Id"].Value), ScreenName);
				}
				if (e.Column.Key == "GpNo")
				{
					int GpId = Conversion.ToInt(r.Cells["GpId"].Value);
					EditOutwardGpRecord(GpId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EditOutwardGpRecord(int GPId)
	{
		try
		{
			OutwardGatePass obj = new OutwardGatePass(UserAccount);
			if (clsGlobalVariables.ScreenViewReights.Count > 0)
			{
				if (Conversion.ToBool((object)clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "OutwardGatePass").ToList()[0].Value.ToString()))
				{
					obj.Show();
					obj.ReadById(GPId);
				}
				else
				{
					MessageBox.Show("Please Check Screen Rights");
				}
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
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.Gdn_PurchaseReturnSlip703(CurrId);
				}
				if (e.Column.Key == "Print1")
				{
					CommonServices.Gdn_PurchaseReturnDeliveryChallanSlip703A(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (e.Column.Key == "View")
				{
					CommonServices.Gdn_PurchaseReturnSlip703(CurrId);
				}
				if (e.Column.Key == "Print1")
				{
					CommonServices.Gdn_PurchaseReturnDeliveryChallanSlip703A(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				DetailGridBind(CurrId);
			}
			else
			{
				DatagridHistoryDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridBind(int CurrId)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (DataGridHistory.CurrentRow != null && (int)DataGridHistory.CurrentRow.RowType == 0 && DataGridHistory.CurrentRow.Cells.Count > 0)
			{
				InvGdn invGrn = InvGdn.GetByID(CurrId);
				if (invGrn == null)
				{
					DatagridHistoryDetail.ClearStructure();
					return;
				}
				DataTable dtDetailHistory = dtdetail.Clone();
				dtDetailHistory.Columns.Add("WareHouseName");
				dtDetailHistory.Columns.Add("CropYear");
				dtDetailHistory.Columns.Add("JobLot");
				dtDetailHistory.Columns.Add("PackingType");
				dtDetailHistory.Columns.Add("CityName");
				FilldtDetailFromListCommonForReadById(invGrn, dtDetailHistory, AddExtraColumns: true);
				DatagridHistoryDetail.DataSource = dtDetailHistory;
				DatagridHistoryDetail.RetrieveStructure();
				grdHistoryDetailSettings();
			}
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
				"Id", "GrnId", "GrnDetailId", "GrnDocumentTypeId", "GrnNo", "ItemId", "UOMId", "UomEquivalent", "RefDocumentTypeId", "RefDocId",
				"RefDocDetailId", "RefDocType", "RefDocNo", "BalQty", "AvailableStock", "WareHouseId", "CropYearId", "JobId", "PackingTypeId", "CityId"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(DatagridHistoryDetail, 3, 2, hiddenColumns);
			DatagridHistoryDetail.RootTable.Columns["GrnNo"].Visible = GridEX_Helper.IsAnyCellValueGreaterThanZero(DatagridHistoryDetail, "GrnId");
			DatagridHistoryDetail.RootTable.Columns["WareHouseName"].Position = DatagridHistoryDetail.RootTable.Columns["WareHouseId"].Position + 1;
			DatagridHistoryDetail.RootTable.Columns["CropYear"].Position = DatagridHistoryDetail.RootTable.Columns["CropYearId"].Position + 1;
			DatagridHistoryDetail.RootTable.Columns["JobLot"].Position = DatagridHistoryDetail.RootTable.Columns["JobId"].Position + 1;
			DatagridHistoryDetail.RootTable.Columns["PackingType"].Position = DatagridHistoryDetail.RootTable.Columns["PackingTypeId"].Position + 1;
			DatagridHistoryDetail.RootTable.Columns["CityName"].Position = DatagridHistoryDetail.RootTable.Columns["CityId"].Position + 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtgwt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtebu_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtwtcut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void combgatepass_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtcarramount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtebt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAddLessHeader_KeyPress(object sender, KeyPressEventArgs e)
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

	private void InvFrmGRN_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			btnRecordsUpdate.Visible = false;
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.A)
			{
				btnRecordsUpdate.Visible = true;
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.F1)
			{
				btnStockReport_Click(null, null);
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DatagridHistoryDetail).Focus();
				}
				if (e.Control && e.KeyCode == Keys.H)
				{
					btnGdnFormHistory_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P && BtnPrint.Visible && BtnPrint.Enabled)
			{
				printToolStripButton_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
			{
				btnSave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbWareHouseName.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grd).Focused)
				{
					((Control)(object)grdGp).Focus();
				}
				else
				{
					((Control)(object)grdGp).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbItemName.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				AT.Show();
			}
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
				if (grd.CurrentColumn.Key == "Delete")
				{
					DeleteRowInDetailGrid(r);
				}
				if (grd.CurrentColumn.Key == "Add")
				{
					AddRowInDetailGrid(r);
				}
				if (grd.CurrentColumn.Key == "AvailableStock")
				{
					AvailableStockClickInDetailGrid(r);
				}
				if (grd.CurrentColumn.Key == "Loader")
				{
					LoaderClickInDetailGrid(r);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteRowInDetailGrid(r);
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInDetailGrid(r);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			if (e.KeyCode == Keys.F1)
			{
				if (Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value) != 0 && Conversion.ToInt(r.Cells["RefDocIdNo"].Value) != 0 && Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value) != 0)
				{
					throw new Exception("You can't change parameters when RefDocument is Present");
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "WareHouseId")
				{
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtWarehouse, "Id", "WareHouseName", r, "WareHouseId");
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "CropYearId")
				{
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtCrop, "Id", "CropYear", r, "CropYearId");
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "JobId")
				{
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtjob, "Id", "JobLotDescription", r, "JobId");
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "PackingTypeId")
				{
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtPack, "Id", "PackTypeDesc", r, "PackingTypeId");
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "CityId")
				{
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtcity, "Id", "CityName", r, "CityId");
				}
				grd.UpdateData();
			}
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
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (GridExpense.CurrentColumn.Key == "Delete")
				{
					dtExpense.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				if (GridExpense.CurrentColumn.Key == "Add")
				{
					AddRowInExpenseGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtExpense.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInExpenseGrid();
			}
			if (e.KeyCode == Keys.F1)
			{
				if (GridExpense.CurrentColumn != null && GridExpense.CurrentColumn.Key == "ItemId")
				{
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtOtherItem, "Id", "OtherItemName", r, "ItemId");
				}
				GridExpense.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGp.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdGp.CurrentColumn != null)
			{
				if (e.Control && e.KeyCode == Keys.Space && grdGp.CurrentColumn.Key == "Load")
				{
					LoadClickInGpGrid(r);
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					LoadClickInGpGrid(r);
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
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(CurrId);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.Gdn_PurchaseReturnSlip703(CurrId);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Print1")
				{
					CommonServices.Gdn_PurchaseReturnDeliveryChallanSlip703A(CurrId);
				}
				else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CurrId, ScreenName);
				}
				else if (DataGridHistory.CurrentColumn.Key == "GpNo")
				{
					int GpId = Conversion.ToInt(r.Cells["GpId"].Value);
					EditOutwardGpRecord(GpId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(CurrId);
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+H", "For History Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Combo in Detail Box");
			dt.Rows.Add("Ctrl+ArrowRight", "For Focus From One Grid To Another");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnStockReport_Click(object sender, EventArgs e)
	{
		try
		{
			List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmStockReport").ToList();
			if (newList.Count > 0)
			{
				if (Conversion.ToBool((object)newList[0].Value.ToString()))
				{
					new frmStockReport(UserAccount).ShowDialog();
				}
				else
				{
					MessageBox.Show("You don't have right");
				}
			}
			else
			{
				MessageBox.Show("You don't have right");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnLoadGrn_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbGpNo.Value) == 0)
			{
				throw new Exception("Please Load Any GatePass First...");
			}
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow currentRow = grd.CurrentRow;
				object obj;
				if (currentRow == null)
				{
					obj = null;
				}
				else
				{
					GridEXCell obj2 = currentRow.Cells["GrnId"];
					obj = ((obj2 != null) ? obj2.Value : null);
				}
				if (Conversion.ToInt(obj) == 0)
				{
					MessageBox.Show("You cannot load 'GRN' because record without 'GRN' exists in the grid.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			LoadGrn = new frmLoadGrnForGdnPurchaseReturn(UserAccount);
			if (!string.IsNullOrEmpty(GrnDetailIds))
			{
				LoadGrn.GrnDetailIds = GrnDetailIds;
			}
			LoadGrn.ShowDialog();
			LoadGrn.BringToFront();
			LoadInGridDetailFromGrn(LoadGrn.dtLoader);
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
			if (grd.GetRows().Count() > 0 && Conversion.ToInt(CmbSupplierName.Value) != Conversion.ToInt(dt.Rows[0]["SupplierCustomerId"]))
			{
				throw new Exception("Already Loaded Row's Have Different Supplier. So you Can't Load Rows Of Different Supplier!");
			}
			CmbSupplierName.Value = Conversion.ToInt(dt.Rows[0]["SupplierCustomerId"]);
			HashSet<int> existingDetailIds = new HashSet<int>(from r in grd.GetRows()
				select Conversion.ToInt(r.Cells["GrnDetailId"].Value));
			foreach (DataRow dr in dt.Rows)
			{
				int detailId = Conversion.ToInt(dr["DetailId"]);
				if (!existingDetailIds.Contains(detailId))
				{
					DataRow newRow = dtdetail.NewRow();
					newRow.ItemArray = new object[32]
					{
						0,
						dr["Id"],
						dr["DetailId"],
						dr["DocumentTypeId"],
						dr["GrnNo"],
						dr["WarehouseId"],
						dr["ItemId"],
						dr["ItemCode"],
						dr["ItemName"],
						dr["CropYearId"],
						dr["JobLotId"],
						dr["PackingTypeId"],
						dr["ItemUomId"],
						dr["ItemUom"],
						dr["ItemEquivalent"],
						dr["BalQty"],
						dr["BalWeight"],
						dr["EBWPerUnit"],
						dr["EBWTotal"],
						dr["WtCut"],
						dr["WtCutTotal"],
						dr["AdLsWeight"],
						dr["BalWeight"],
						dr["StockWeight"],
						dr["CityId"],
						0,
						dr["BalWeight"],
						0,
						"",
						0,
						0,
						0
					};
					dtdetail.Rows.Add(newRow);
				}
			}
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			((Control)(object)CmbSupplierName).Enabled = false;
			panel5.Visible = false;
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
			GrnDetailIds = "";
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				GrnDetailIds = GrnDetailIds + "," + Conversion.ToInt(item.Cells["GrnDetailId"].Value);
			}
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
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Expected O, but got Unknown
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Expected O, but got Unknown
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Expected O, but got Unknown
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Expected O, but got Unknown
		//IL_0795: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Expected O, but got Unknown
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07aa: Expected O, but got Unknown
		//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Expected O, but got Unknown
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Expected O, but got Unknown
		//IL_090b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Expected O, but got Unknown
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a4: Expected O, but got Unknown
		//IL_0ff9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1003: Expected O, but got Unknown
		//IL_1488: Unknown result type (might be due to invalid IL or missing references)
		//IL_1492: Expected O, but got Unknown
		//IL_149f: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a9: Expected O, but got Unknown
		//IL_14b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c0: Expected O, but got Unknown
		//IL_4a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a8f: Expected O, but got Unknown
		//IL_7837: Unknown result type (might be due to invalid IL or missing references)
		//IL_7841: Expected O, but got Unknown
		//IL_7865: Unknown result type (might be due to invalid IL or missing references)
		//IL_786f: Expected O, but got Unknown
		GridEXLayout grdGp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance114 = new Appearance();
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
		Appearance appearance125 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		GridEXLayout GridExpense_DesignTimeLayout = new GridEXLayout();
		Appearance appearance158 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Purchase.Return.frmGdnForPurchaseReturn));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdGp = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label39 = new System.Windows.Forms.Label();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.label9 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.CmbCity = new UltraCombo();
		this.Add = new System.Windows.Forms.Button();
		this.CmbJobLot = new UltraCombo();
		this.txtwtcuttotal = new System.Windows.Forms.TextBox();
		this.lblStock = new System.Windows.Forms.Label();
		this.txtebt = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.txtwtcut = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.txtebu = new System.Windows.Forms.TextBox();
		this.label41 = new System.Windows.Forms.Label();
		this.txtadlswt = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.txtStockWeight = new System.Windows.Forms.TextBox();
		this.CmbCropYear = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.CmbUom = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.CmbWareHouseName = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.CmbPackingType = new UltraCombo();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label27 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.GridExpense = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label52 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.PanelMessage = new System.Windows.Forms.Panel();
		this.ToolStripMessage = new System.Windows.Forms.ToolStrip();
		this.BtnMessage = new System.Windows.Forms.ToolStripButton();
		this.txtAddLessHeader = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.txtTicketNos = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.txtBalGrossWeightGrid = new System.Windows.Forms.TextBox();
		this.txtBiltyNo = new System.Windows.Forms.TextBox();
		this.txtCustomerCellNo = new System.Windows.Forms.TextBox();
		this.txtCustomerCity = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtFreight = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.txtGpDate = new System.Windows.Forms.DateTimePicker();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label26 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbGpNo = new UltraCombo();
		this.CmbSupplierName = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbTransport = new UltraCombo();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnRecordsUpdate = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ToolStripForm = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrintA = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnStockReport = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnGenerateAvailableStock = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label51 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label58 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.DatagridHistoryDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label38 = new System.Windows.Forms.Label();
		this.ToolStripHistory = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnGdnFormHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoadGrn = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGp).BeginInit();
		this.panel9.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel13.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.tabPage4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridExpense).BeginInit();
		this.panel12.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.PanelMessage.SuspendLayout();
		this.ToolStripMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGpNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.ToolStripForm.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel17.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistoryDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.ToolStripHistory.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1034, 661);
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
		this.tabControl1.Size = new System.Drawing.Size(1034, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1026, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdGp);
		this.panel10.Controls.Add(this.panel9);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(2, 534);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1022, 96);
		this.panel10.TabIndex = 8;
		this.grdGp.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGp).BackColor = System.Drawing.Color.White;
		grdGp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGp.DesignTimeLayout = grdGp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGp).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGp.FilterMode = (FilterMode)2;
		this.grdGp.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGp.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grdGp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGp).Location = new System.Drawing.Point(0, 20);
		((System.Windows.Forms.Control)(object)this.grdGp).Name = "grdGp";
		((ScrollableBase)this.grdGp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGp).Size = new System.Drawing.Size(1022, 76);
		((System.Windows.Forms.Control)(object)this.grdGp).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdGp).TabStop = false;
		this.grdGp.TotalRow = (InheritableBoolean)1;
		this.grdGp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGp).VisualStyle = (VisualStyle)4;
		this.grdGp.ColumnButtonClick += new ColumnActionEventHandler(grdGp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGp_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar3);
		this.panel9.Controls.Add(this.label39);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1022, 20);
		this.panel9.TabIndex = 1;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(982, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdGp;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 18);
		this.ctrlGrdBar3.TabIndex = 26;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label39.ForeColor = System.Drawing.Color.White;
		this.label39.Location = new System.Drawing.Point(3, 3);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(933, 14);
		this.label39.TabIndex = 25;
		this.label39.Text = "This Grid show Pending GPs For GDN Entry,User can Load GatePasses With Accepted Status for Entry From This grid,GP With Open Status Will Not Load";
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(2, 186);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1022, 348);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add(this.panel19);
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1014, 319);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel19.Controls.Add(this.panel13);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(3, 104);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1008, 212);
		this.panel19.TabIndex = 6;
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 20);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1008, 192);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.ctrlGrdBar1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1008, 20);
		this.panel13.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(968, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 20);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel5.Controls.Add(this.btnCancelUpdateDetial);
		this.panel5.Controls.Add(this.rdSearchByCode);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.rdSearchByName);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.panel5.Controls.Add(this.Add);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel5.Controls.Add(this.txtwtcuttotal);
		this.panel5.Controls.Add(this.lblStock);
		this.panel5.Controls.Add(this.txtebt);
		this.panel5.Controls.Add(this.label36);
		this.panel5.Controls.Add(this.txtwtcut);
		this.panel5.Controls.Add(this.label40);
		this.panel5.Controls.Add(this.txtebu);
		this.panel5.Controls.Add(this.label41);
		this.panel5.Controls.Add(this.txtadlswt);
		this.panel5.Controls.Add(this.label42);
		this.panel5.Controls.Add(this.txtStockWeight);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.panel5.Controls.Add(this.label29);
		this.panel5.Controls.Add(this.label43);
		this.panel5.Controls.Add(this.txtNetWeight);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUom);
		this.panel5.Controls.Add(this.label46);
		this.panel5.Controls.Add(this.txtQty);
		this.panel5.Controls.Add(this.label28);
		this.panel5.Controls.Add(this.txtGrossWeight);
		this.panel5.Controls.Add(this.label35);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseName);
		this.panel5.Controls.Add(this.label45);
		this.panel5.Controls.Add(this.label22);
		this.panel5.Controls.Add(this.label44);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.panel5.Controls.Add(this.btnUpdateDetail);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Controls.Add(this.label30);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1008, 101);
		this.panel5.TabIndex = 0;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(869, 73);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(68, 23);
		this.btnCancelUpdateDetial.TabIndex = 18;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(254, 3);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 56;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(238, 52);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(54, 15);
		this.label9.TabIndex = 44;
		this.label9.Text = "Cut Total";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(167, 52);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 15);
		this.label3.TabIndex = 43;
		this.label3.Text = "Wt/Cut";
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(201, 3);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 55;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance183;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance190).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(585, 71);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 15;
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(800, 72);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(39, 25);
		this.Add.TabIndex = 16;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance207;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(481, 23);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Leave += new System.EventHandler(combjoblot_Leave);
		this.txtwtcuttotal.BackColor = System.Drawing.Color.White;
		this.txtwtcuttotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtcuttotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtwtcuttotal.Location = new System.Drawing.Point(233, 73);
		this.txtwtcuttotal.Name = "txtwtcuttotal";
		this.txtwtcuttotal.ReadOnly = true;
		this.txtwtcuttotal.Size = new System.Drawing.Size(82, 23);
		this.txtwtcuttotal.TabIndex = 11;
		this.txtwtcuttotal.TabStop = false;
		this.lblStock.AutoSize = true;
		this.lblStock.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblStock.Location = new System.Drawing.Point(330, 5);
		this.lblStock.Name = "lblStock";
		this.lblStock.Size = new System.Drawing.Size(37, 15);
		this.lblStock.TabIndex = 45;
		this.lblStock.Text = "Stock";
		this.txtebt.BackColor = System.Drawing.Color.White;
		this.txtebt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtebt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtebt.Location = new System.Drawing.Point(67, 73);
		this.txtebt.Name = "txtebt";
		this.txtebt.Size = new System.Drawing.Size(98, 23);
		this.txtebt.TabIndex = 9;
		this.txtebt.Tag = "PmTotal";
		this.txtebt.TextChanged += new System.EventHandler(txtebt_TextChanged);
		this.txtebt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtebt_KeyPress);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(169, 5);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(67, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Item Name";
		this.txtwtcut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtcut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtwtcut.Location = new System.Drawing.Point(167, 73);
		this.txtwtcut.Name = "txtwtcut";
		this.txtwtcut.Size = new System.Drawing.Size(65, 23);
		this.txtwtcut.TabIndex = 10;
		this.txtwtcut.TextChanged += new System.EventHandler(txtebu_TextChanged);
		this.txtwtcut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtwtcut_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(625, 5);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(77, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Packing Type";
		this.txtebu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtebu.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtebu.Location = new System.Drawing.Point(5, 73);
		this.txtebu.Name = "txtebu";
		this.txtebu.Size = new System.Drawing.Size(61, 23);
		this.txtebu.TabIndex = 8;
		this.txtebu.Tag = "PmUnit";
		this.txtebu.TextChanged += new System.EventHandler(txtebu_TextChanged);
		this.txtebu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtebu_KeyPress);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(800, 5);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(26, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Qty";
		this.txtadlswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtadlswt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtadlswt.Location = new System.Drawing.Point(316, 73);
		this.txtadlswt.Name = "txtadlswt";
		this.txtadlswt.Size = new System.Drawing.Size(62, 23);
		this.txtadlswt.TabIndex = 12;
		this.txtadlswt.TextChanged += new System.EventHandler(txtadlswt_TextChanged_1);
		this.txtadlswt.Leave += new System.EventHandler(txtadlswt_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(725, 5);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(35, 15);
		this.label42.TabIndex = 21;
		this.label42.Text = "UOM";
		this.txtStockWeight.BackColor = System.Drawing.Color.White;
		this.txtStockWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStockWeight.Enabled = false;
		this.txtStockWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtStockWeight.Location = new System.Drawing.Point(481, 73);
		this.txtStockWeight.Name = "txtStockWeight";
		this.txtStockWeight.Size = new System.Drawing.Size(103, 23);
		this.txtStockWeight.TabIndex = 14;
		this.txtStockWeight.TabStop = false;
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance137).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(379, 23);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Leave += new System.EventHandler(CmbCropYear_Leave);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(481, 52);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(79, 15);
		this.label29.TabIndex = 25;
		this.label29.Text = "Stock Weight";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(892, 5);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(78, 15);
		this.label43.TabIndex = 23;
		this.label43.Text = "Gross Weight";
		this.txtNetWeight.BackColor = System.Drawing.Color.White;
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetWeight.Location = new System.Drawing.Point(379, 73);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(101, 23);
		this.txtNetWeight.TabIndex = 13;
		this.txtNetWeight.TabStop = false;
		this.CmbUom.AllowNull = (DefaultableBoolean)1;
		this.CmbUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance142;
		((SpecialBoxBase)((UltraGridBase)this.CmbUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance148).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUom).Location = new System.Drawing.Point(725, 23);
		((System.Windows.Forms.Control)(object)this.CmbUom).Name = "CmbUom";
		((System.Windows.Forms.Control)(object)this.CmbUom).Size = new System.Drawing.Size(74, 26);
		((System.Windows.Forms.Control)(object)this.CmbUom).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbUom).Leave += new System.EventHandler(combpckuom_Leave);
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(379, 52);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(68, 15);
		this.label46.TabIndex = 25;
		this.label46.Text = "Net Weight";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(800, 25);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(91, 23);
		this.txtQty.TabIndex = 6;
		this.txtQty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(585, 52);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(56, 15);
		this.label28.TabIndex = 25;
		this.label28.Text = "City/Area";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(892, 25);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(111, 23);
		this.txtGrossWeight.TabIndex = 7;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtgwt_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgwt_KeyPress);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(319, 52);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(38, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "Ad/Ls";
		this.CmbWareHouseName.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance121).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouseName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Location = new System.Drawing.Point(5, 23);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Name = "CmbWareHouseName";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Leave += new System.EventHandler(combwr_Leave);
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(67, 52);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(52, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "E.b Total";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(379, 5);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(57, 15);
		this.label22.TabIndex = 41;
		this.label22.Text = "Crop Year";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(5, 52);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(56, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "E.b / Unit";
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance195;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance202).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(625, 23);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(98, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Leave += new System.EventHandler(CmbPackingType_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(800, 73);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(68, 23);
		this.btnUpdateDetail.TabIndex = 17;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(481, 5);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(48, 15);
		this.label27.TabIndex = 41;
		this.label27.Text = "Job/Lot";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(5, 5);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(67, 15);
		this.label30.TabIndex = 41;
		this.label30.Text = "Warehouse";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance155;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance224).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(167, 23);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 1;
		this.CmbItemName.ValueChanged += new System.EventHandler(combitem_Leave);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.tabPage4.Controls.Add(this.panel6);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1014, 319);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Expenses";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.GridExpense);
		this.panel6.Controls.Add(this.panel12);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1008, 313);
		this.panel6.TabIndex = 0;
		this.GridExpense.AutomaticSort = false;
		((ScrollableBase)this.GridExpense).BackColor = System.Drawing.Color.White;
		GridExpense_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridExpense.DesignTimeLayout = GridExpense_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridExpense).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GridExpense).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridExpense.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridExpense).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.GridExpense).Name = "GridExpense";
		((ScrollableBase)this.GridExpense).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridExpense).Size = new System.Drawing.Size(1008, 291);
		((System.Windows.Forms.Control)(object)this.GridExpense).TabIndex = 5;
		this.GridExpense.TotalRow = (InheritableBoolean)1;
		this.GridExpense.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridExpense).VisualStyle = (VisualStyle)4;
		this.GridExpense.ColumnButtonClick += new ColumnActionEventHandler(GridExpense_ColumnButtonClick);
		this.panel12.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel12.Controls.Add(this.label52);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Margin = new System.Windows.Forms.Padding(2);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1008, 22);
		this.panel12.TabIndex = 4;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label52.ForeColor = System.Drawing.Color.White;
		this.label52.Location = new System.Drawing.Point(6, 1);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(70, 18);
		this.label52.TabIndex = 0;
		this.label52.Text = "Expense";
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1022, 129);
		this.panel4.TabIndex = 1;
		this.groupBox1.Controls.Add(this.PanelMessage);
		this.groupBox1.Controls.Add(this.txtAddLessHeader);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.txtTicketNos);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtFactoryWeight);
		this.groupBox1.Controls.Add(this.txtBalGrossWeightGrid);
		this.groupBox1.Controls.Add(this.txtBiltyNo);
		this.groupBox1.Controls.Add(this.txtCustomerCellNo);
		this.groupBox1.Controls.Add(this.txtCustomerCity);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label62);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.txtFreight);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.txtGpDate);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGpNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierName);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransport);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1014, 124);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.PanelMessage.BackColor = System.Drawing.Color.White;
		this.PanelMessage.Controls.Add(this.ToolStripMessage);
		this.PanelMessage.Location = new System.Drawing.Point(844, 43);
		this.PanelMessage.Name = "PanelMessage";
		this.PanelMessage.Size = new System.Drawing.Size(28, 24);
		this.PanelMessage.TabIndex = 162;
		this.PanelMessage.Click += new System.EventHandler(chkPropBalGrossInGrid_Click);
		this.ToolStripMessage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.ToolStripMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnMessage });
		this.ToolStripMessage.Location = new System.Drawing.Point(0, 0);
		this.ToolStripMessage.Name = "ToolStripMessage";
		this.ToolStripMessage.Size = new System.Drawing.Size(28, 25);
		this.ToolStripMessage.TabIndex = 0;
		this.ToolStripMessage.Text = "toolStrip3";
		this.BtnMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.BtnMessage.Image = Architecture.WinApp.Properties.Resources.UpdateIcon;
		this.BtnMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnMessage.Name = "BtnMessage";
		this.BtnMessage.Size = new System.Drawing.Size(23, 22);
		this.BtnMessage.ToolTipText = "Click To Prpotionate Bal Gross Wt In Grid";
		this.BtnMessage.Click += new System.EventHandler(chkPropBalGrossInGrid_Click);
		this.txtAddLessHeader.BackColor = System.Drawing.Color.White;
		this.txtAddLessHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLessHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAddLessHeader.Location = new System.Drawing.Point(875, 71);
		this.txtAddLessHeader.Name = "txtAddLessHeader";
		this.txtAddLessHeader.Size = new System.Drawing.Size(136, 23);
		this.txtAddLessHeader.TabIndex = 15;
		this.txtAddLessHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLessHeader_KeyPress);
		this.txtAddLessHeader.Leave += new System.EventHandler(txtAddLessHeader_TextChanged);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(769, 75);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(54, 15);
		this.label49.TabIndex = 35;
		this.label49.Text = "Add Less";
		this.txtTicketNos.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTicketNos.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTicketNos.Location = new System.Drawing.Point(647, 97);
		this.txtTicketNos.Name = "txtTicketNos";
		this.txtTicketNos.ReadOnly = true;
		this.txtTicketNos.Size = new System.Drawing.Size(364, 23);
		this.txtTicketNos.TabIndex = 12;
		this.txtTicketNos.TabStop = false;
		this.txtremarks.BackColor = System.Drawing.Color.White;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(92, 97);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(484, 23);
		this.txtremarks.TabIndex = 4;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(769, 48);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(77, 15);
		this.label17.TabIndex = 34;
		this.label17.Text = "Bal Gross Wt ";
		this.txtFactoryWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFactoryWeight.Location = new System.Drawing.Point(875, 17);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.ReadOnly = true;
		this.txtFactoryWeight.Size = new System.Drawing.Size(136, 23);
		this.txtFactoryWeight.TabIndex = 13;
		this.txtFactoryWeight.TabStop = false;
		this.txtFactoryWeight.TextChanged += new System.EventHandler(txtfctwt_TextChanged);
		this.txtBalGrossWeightGrid.BackColor = System.Drawing.SystemColors.Menu;
		this.txtBalGrossWeightGrid.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBalGrossWeightGrid.Location = new System.Drawing.Point(875, 44);
		this.txtBalGrossWeightGrid.Name = "txtBalGrossWeightGrid";
		this.txtBalGrossWeightGrid.ReadOnly = true;
		this.txtBalGrossWeightGrid.Size = new System.Drawing.Size(136, 23);
		this.txtBalGrossWeightGrid.TabIndex = 14;
		this.txtBalGrossWeightGrid.TabStop = false;
		this.txtBiltyNo.BackColor = System.Drawing.Color.White;
		this.txtBiltyNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBiltyNo.Location = new System.Drawing.Point(647, 71);
		this.txtBiltyNo.Name = "txtBiltyNo";
		this.txtBiltyNo.ReadOnly = true;
		this.txtBiltyNo.Size = new System.Drawing.Size(118, 23);
		this.txtBiltyNo.TabIndex = 11;
		this.txtCustomerCellNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCustomerCellNo.Enabled = false;
		this.txtCustomerCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerCellNo.Location = new System.Drawing.Point(381, 17);
		this.txtCustomerCellNo.Name = "txtCustomerCellNo";
		this.txtCustomerCellNo.Size = new System.Drawing.Size(195, 23);
		this.txtCustomerCellNo.TabIndex = 5;
		this.txtCustomerCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCustomerCity.Enabled = false;
		this.txtCustomerCity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerCity.Location = new System.Drawing.Point(92, 71);
		this.txtCustomerCity.Name = "txtCustomerCity";
		this.txtCustomerCity.Size = new System.Drawing.Size(194, 23);
		this.txtCustomerCity.TabIndex = 3;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(290, 21);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(83, 15);
		this.label21.TabIndex = 148;
		this.label21.Text = "Supplier Cell #";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(4, 75);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(74, 15);
		this.label62.TabIndex = 149;
		this.label62.Text = "Supplier City";
		this.txtdocno.BackColor = System.Drawing.SystemColors.Menu;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(92, 17);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(84, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(4, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No && Date";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(582, 75);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(49, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Bilty No";
		this.txtFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFreight.Location = new System.Drawing.Point(647, 17);
		this.txtFreight.Name = "txtFreight";
		this.txtFreight.Size = new System.Drawing.Size(118, 23);
		this.txtFreight.TabIndex = 9;
		this.txtFreight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcarramount_KeyPress);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(582, 101);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(66, 15);
		this.label50.TabIndex = 28;
		this.label50.Text = "Ticket Nos.";
		this.txtGpDate.CustomFormat = "dd-MMM-yy";
		this.txtGpDate.Enabled = false;
		this.txtGpDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtGpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGpDate.Location = new System.Drawing.Point(467, 44);
		this.txtGpDate.Name = "txtGpDate";
		this.txtGpDate.Size = new System.Drawing.Size(109, 22);
		this.txtGpDate.TabIndex = 7;
		this.txtGpDate.TabStop = false;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(177, 17);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(109, 22);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(582, 21);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(44, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Freight";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(582, 48);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(64, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Vehicle No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(290, 48);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(81, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "GP No && Date";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(290, 75);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(67, 15);
		this.label48.TabIndex = 8;
		this.label48.Text = "Transporter";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(4, 48);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Supplier Name";
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(4, 101);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Remarks";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbGpNo.AllowNull = (DefaultableBoolean)1;
		this.CmbGpNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGpNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGpNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGpNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.CmbGpNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGpNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Location = new System.Drawing.Point(381, 42);
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Name = "CmbGpNo";
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.CmbGpNo).TabIndex = 6;
		this.CmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance179).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance217).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Location = new System.Drawing.Point(92, 42);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Name = "CmbSupplierName";
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Size = new System.Drawing.Size(194, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Leave += new System.EventHandler(CmbSupplierName_Leave);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(769, 21);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(85, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "FactoryWeight";
		this.CmbTransport.AllowNull = (DefaultableBoolean)1;
		this.CmbTransport.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbTransport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance231;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransport.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Location = new System.Drawing.Point(381, 69);
		((System.Windows.Forms.Control)(object)this.CmbTransport).Name = "CmbTransport";
		((System.Windows.Forms.Control)(object)this.CmbTransport).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransport).TabIndex = 8;
		this.txtVehicleNo.BackColor = System.Drawing.Color.White;
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVehicleNo.Location = new System.Drawing.Point(647, 44);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.ReadOnly = true;
		this.txtVehicleNo.Size = new System.Drawing.Size(118, 23);
		this.txtVehicleNo.TabIndex = 10;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.btnRecordsUpdate);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 33);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1022, 24);
		this.panel3.TabIndex = 0;
		this.btnRecordsUpdate.BackColor = System.Drawing.Color.FromArgb(192, 0, 0);
		this.btnRecordsUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Black;
		this.btnRecordsUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRecordsUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRecordsUpdate.ForeColor = System.Drawing.Color.White;
		this.btnRecordsUpdate.Location = new System.Drawing.Point(377, 1);
		this.btnRecordsUpdate.Name = "btnRecordsUpdate";
		this.btnRecordsUpdate.Size = new System.Drawing.Size(159, 23);
		this.btnRecordsUpdate.TabIndex = 114;
		this.btnRecordsUpdate.Text = "Auto Update Records";
		this.btnRecordsUpdate.UseVisualStyleBackColor = false;
		this.btnRecordsUpdate.Visible = false;
		this.btnRecordsUpdate.Click += new System.EventHandler(btnRecordsUpdate_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(357, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Goods Dispatch Notes For Purchase Return";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Checked = true;
		this.ChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBox.Cursor = System.Windows.Forms.Cursors.Arrow;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(927, 3);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(84, 20);
		this.ChkBox.TabIndex = 20;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.ToolStripForm);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1022, 31);
		this.panel2.TabIndex = 0;
		this.ToolStripForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToolStripForm.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.ToolStripForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[18]
		{
			this.btnNew, this.BtnRefresh, this.toolStripSeparator5, this.btnUpdate, this.btnSave, this.btnDelete, this.toolStripSeparator4, this.btnAttachment, this.toolStripSeparator3, this.BtnPrint,
			this.btnPrintA, this.toolStripSeparator6, this.btnLoadGrn, this.toolStripSeparator2, this.btnStockReport, this.BtnShortCutkeys, this.toolStripSeparator1, this.btnGenerateAvailableStock
		});
		this.ToolStripForm.Location = new System.Drawing.Point(0, 0);
		this.ToolStripForm.Name = "ToolStripForm";
		this.ToolStripForm.Size = new System.Drawing.Size(1022, 27);
		this.ToolStripForm.TabIndex = 0;
		this.ToolStripForm.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.ToolTipText = "Ctrl_N";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "&Refresh";
		this.BtnRefresh.ToolTipText = "Alt_R";
		this.BtnRefresh.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.ToolTipText = "Ctrl_U";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.ToolTipText = "Ctrl_S";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnPrint.Text = "703-Print";
		this.BtnPrint.Click += new System.EventHandler(printToolStripButton_Click);
		this.btnPrintA.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintA.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintA.Name = "btnPrintA";
		this.btnPrintA.Size = new System.Drawing.Size(101, 24);
		this.btnPrintA.Text = "703A-Print";
		this.btnPrintA.Click += new System.EventHandler(btnPrint260A_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnStockReport.Image = Architecture.WinApp.Properties.Resources.Fixed_Assets;
		this.btnStockReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnStockReport.Name = "btnStockReport";
		this.btnStockReport.Size = new System.Drawing.Size(117, 24);
		this.btnStockReport.Text = "Stock Report";
		this.btnStockReport.Click += new System.EventHandler(btnStockReport_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnGenerateAvailableStock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnGenerateAvailableStock.Image = (System.Drawing.Image)resources.GetObject("btnGenerateAvailableStock.Image");
		this.btnGenerateAvailableStock.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateAvailableStock.Name = "btnGenerateAvailableStock";
		this.btnGenerateAvailableStock.Size = new System.Drawing.Size(114, 20);
		this.btnGenerateAvailableStock.Text = "Generate Stock";
		this.btnGenerateAvailableStock.Visible = false;
		this.btnGenerateAvailableStock.Click += new System.EventHandler(btnGenerateAvailableStock_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistoryDetail);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1026, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(2, 154);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1022, 270);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label51);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel17.Location = new System.Drawing.Point(2, 424);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1022, 25);
		this.panel17.TabIndex = 48;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(4, 5);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(209, 14);
		this.label51.TabIndex = 25;
		this.label51.Text = "Detail of the Selected above row";
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label58);
		this.panel15.Controls.Add(this.ctrlGrdBar2);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(2, 129);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1022, 25);
		this.panel15.TabIndex = 45;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(3, 1);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(132, 18);
		this.label58.TabIndex = 27;
		this.label58.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(977, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(42, 24);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.DatagridHistoryDetail.AllowEdit = (InheritableBoolean)2;
		DatagridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistoryDetail.DesignTimeLayout = DatagridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.DatagridHistoryDetail.DynamicFiltering = true;
		this.DatagridHistoryDetail.FilterMode = (FilterMode)2;
		this.DatagridHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryDetail).Location = new System.Drawing.Point(2, 449);
		((System.Windows.Forms.Control)(object)this.DatagridHistoryDetail).Name = "DatagridHistoryDetail";
		((ScrollableBase)this.DatagridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryDetail).Size = new System.Drawing.Size(1022, 181);
		((System.Windows.Forms.Control)(object)this.DatagridHistoryDetail).TabIndex = 1;
		this.DatagridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DatagridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel8.Controls.Add(this.groupBox2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 56);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1022, 73);
		this.panel8.TabIndex = 1;
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.label55);
		this.groupBox2.Controls.Add(this.label56);
		this.groupBox2.Controls.Add(this.label57);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(844, 66);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(736, 40);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(736, 16);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(655, 16);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(655, 40);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(588, 37);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(354, 17);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(51, 15);
		this.label53.TabIndex = 87;
		this.label53.Text = "Supplier";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(201, 43);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(45, 15);
		this.label54.TabIndex = 86;
		this.label54.Text = "To Doc";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(198, 17);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(60, 15);
		this.label55.TabIndex = 85;
		this.label55.Text = "From Doc";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(6, 43);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(48, 15);
		this.label56.TabIndex = 84;
		this.label56.Text = "To Date";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(3, 17);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(63, 15);
		this.label57.TabIndex = 83;
		this.label57.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance189;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance197).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(418, 11);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(232, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(264, 39);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(84, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(264, 13);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(84, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(68, 39);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(127, 22);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(68, 13);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(127, 22);
		this.FromDateHistory.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label38);
		this.panel7.Controls.Add(this.ToolStripHistory);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1022, 54);
		this.panel7.TabIndex = 0;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label38.ForeColor = System.Drawing.Color.White;
		this.label38.Location = new System.Drawing.Point(5, 30);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(254, 19);
		this.label38.TabIndex = 27;
		this.label38.Text = "Goods Dispatch Notes History ";
		this.ToolStripHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToolStripHistory.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.ToolStripHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnGdnFormHistory });
		this.ToolStripHistory.Location = new System.Drawing.Point(0, 0);
		this.ToolStripHistory.Name = "ToolStripHistory";
		this.ToolStripHistory.Size = new System.Drawing.Size(1022, 27);
		this.ToolStripHistory.TabIndex = 46;
		this.ToolStripHistory.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnGdnFormHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGdnFormHistory.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnGdnFormHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGdnFormHistory.Name = "btnGdnFormHistory";
		this.btnGdnFormHistory.Size = new System.Drawing.Size(107, 24);
		this.btnGdnFormHistory.Text = "Gdn History";
		this.btnGdnFormHistory.ToolTipText = "Crtl_H";
		this.btnGdnFormHistory.Visible = false;
		this.btnGdnFormHistory.Click += new System.EventHandler(btnGdnFormHistory_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.btnLoadGrn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadGrn.Name = "btnLoadGrn";
		this.btnLoadGrn.Size = new System.Drawing.Size(73, 24);
		this.btnLoadGrn.Text = "Load &GRN";
		this.btnLoadGrn.Click += new System.EventHandler(btnLoadGrn_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1034, 661);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmGdnForPurchaseReturn";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmGdnForPurchaseReturn_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvFrmGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGp).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.tabPage4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridExpense).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.PanelMessage.ResumeLayout(false);
		this.PanelMessage.PerformLayout();
		this.ToolStripMessage.ResumeLayout(false);
		this.ToolStripMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGpNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.ToolStripForm.ResumeLayout(false);
		this.ToolStripForm.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistoryDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.ToolStripHistory.ResumeLayout(false);
		this.ToolStripHistory.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
