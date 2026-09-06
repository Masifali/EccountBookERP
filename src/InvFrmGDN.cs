// using Architecture.Reports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.BLL.WeightBridge;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.WeightBridge;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Inventory_Stocks_Report;
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
namespace Architecture.WinApp.Sale
{

public class InvFrmGDN : Form
{
	private DropDownBind DDL = new DropDownBind();

	private WaitForm waitForm = new WaitForm();

	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private int SoOrderId;

	private int GPID;

	private int updateDetailIndex = -1;

	private int Id;

	private string OrderStatus = "";

	private string DeliveryOrderType = "";

	private string GpType = string.Empty;

	private bool LoaderStatus;

	private bool WagesStatus;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool ItemSearchWithNameOrCode;

	private bool MultipleGdnsAgainstSingleParty;

	private bool IsStockReservedPerParty;

	private int DefaultDaysToLessFromHistoryFromDate;

	private double GdnAndFactoryWeightCompareToleranceForStop;

	private DataTable dtdetail = new DataTable();

	private DataTable dtExpense = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtTransporter = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtBrandItem = new DataTable();

	private DataTable dtjob = new DataTable();

	private DataTable dtPack = new DataTable();

	private DataTable dtUOM = new DataTable();

	private DataTable dtwh = new DataTable();

	private DataTable dtcity = new DataTable();

	private DataTable dtpo = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtJobLotWithJobOrderAndItems = new DataTable();

	private static bool bool_EbUnit = false;

	private static bool bool_EbTotal = true;

	private static bool bool_EbUnitStock = false;

	private static bool bool_EbTotalStock = true;

	private bool WagesActiveOrInActive;

	private bool StockLoaderVisibleInGDNDetail;

	private bool SaleCostingJobOrderWise;

	private bool EbWeightEditableForSaleTypePackWise;

	private bool FormInitialized;

	private LoadavailableTransactionsForGDN LoadTransactions;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "InvFrmGDN";

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton printToolStripButton;

	private ToolStripButton btnAttachment;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Panel panel5;

	private Label label24;

	private Label label25;

	private Label label19;

	private TextBox txtbltyno;

	private Label label7;

	private TextBox txtvehno;

	internal UltraCombo CmbTransport;

	private Label label48;

	private Label label26;

	private TextBox txtcarramount;

	private Label label20;

	private TextBox txtsuppwt;

	private Label label6;

	private TextBox txtfctwt;

	private Label label11;

	private TextBox txtwtdiff;

	private Label label1;

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private Label label5;

	private GroupBox groupBox3;

	internal UltraCombo CmbPackUom;

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

	private TextBox txtgwt;

	private TextBox txtqty;

	private TextBox txtadlswt;

	private TextBox txtstockwt;

	private Label label29;

	private TextBox txtnetwt;

	private Label label46;

	private Label label28;

	private Label label35;

	private Label label45;

	private Label label44;

	private Label label43;

	private Label label42;

	private ComboBox comblabno;

	private Label label31;

	private Label label41;

	private Label label40;

	private Label label36;

	private Label label37;

	private Panel panel7;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	internal UltraCombo CmbOrderNo;

	internal UltraCombo CmbCropYear;

	private Label label9;

	private Label label3;

	private TextBox txtwtcuttotal;

	private TextBox txtwtcut;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton toolStripButton1;

	private TextBox CmbGpNo;

	private Label label32;

	private Label label16;

	private TextBox txtContainerNo1;

	private TextBox txtContainerNo;

	private Label label33;

	private TextBox txtContainerNodetail;

	private CheckBox ChkBox;

	internal GridEX grdGp;

	internal UltraCombo combbrnch;

	internal UltraCombo combproject;

	internal UltraCombo combcompany;

	internal GridEX DatagridHistoryDetail;

	private Label label15;

	private TextBox txtDeliverTerm;

	internal UltraCombo combstockprty;

	internal UltraCombo CmbCustomerName;

	private Label label2;

	private Label label10;

	private Label label8;

	private TextBox txtsuppref;

	private Label label4;

	private TextBox txtrefno;

	private ToolStrip toolStrip2;

	private ToolStripButton btnDelete;

	internal GridEX grd;

	private TextBox txtPartyCount;

	private TextBox txtGdnGrossWeight;

	private TextBox txtBalanceWeight;

	private Label label14;

	private Label label21;

	private Label label34;

	private ToolStripButton btnPrint260A;

	private ToolStripButton btnGdnFormHistory;

	private Label lblStock;

	private CheckBox chkPropBalGrossInGrid;

	private Label label17;

	private TextBox txtBalGrossWeightGrid;

	private Panel panel9;

	private Panel panel13;

	private Label label39;

	private Label label47;

	private CtrlGrdBar ctrlGrdBar3;

	private TextBox txtAddLessHeader;

	private Label label49;

	private CtrlGrdBar ctrlGrdBar4;

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

	private TextBox txtShortExcessWeight;

	private Label label60;

	private TextBox txtTotalDoWeight;

	private Label label59;

	private CheckBox chkShortWeightLessFromNetWeight;

	private TextBox txtSaleType;

	private CheckBox ChkIsStockReservedPerParty;

	private Panel panel11;

	private RadioButton RadBrandName;

	private RadioButton RadBrandCode;

	internal UltraCombo CmbBrandItemName;

	private Label label61;

	private Label labelAdvDoNo;

	internal UltraCombo CmbAdvanceDo;

	internal UltraCombo CmbWareHouseTo;

	private Label labelWareTo;

	private TextBox txtEbTotalStock;

	private TextBox txtEbUnitStock;

	private Label label12;

	private Label label62;

	public InvFrmGDN(UserAccount user)
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
			base.AutoScrollMinSize = new Size(1100, 800);
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
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtItem.Columns.Add("OrderItemRate");
			dtItem.Columns.Add("RateUOM");
			dtItem.Columns.Add("SoDetailId", typeof(int));
			dtItem.Columns.Add("PackUomId", typeof(int));
			dtItem.Columns.Add("PackUom", typeof(string));
			dtBrandItem.Columns.Add("Id", typeof(int));
			dtBrandItem.Columns.Add("ItemName");
			dtBrandItem.Columns.Add("ItemCode");
			dtjob.Columns.Add("Id", typeof(int));
			dtjob.Columns.Add("JobLotDescription");
			dtPack.Columns.Add("Id", typeof(int));
			dtPack.Columns.Add("PackTypeDesc");
			dtwh.Columns.Add("Id", typeof(int));
			dtwh.Columns.Add("WareHouseName");
			dtcity.Columns.Add("Id", typeof(int));
			dtcity.Columns.Add("CityName");
			dtCrop.Columns.Add("Id", typeof(int));
			dtCrop.Columns.Add("CropYear");
			dtpo.Columns.Add("Id");
			dtpo.Columns.Add("DocNo");
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("RefDocumentTypeId", typeof(int));
			dtdetail.Columns.Add("RefDocIdNo", typeof(int));
			dtdetail.Columns.Add("RefDocSubIdNo", typeof(int));
			dtdetail.Columns.Add("SaleOrderDetailId", typeof(int));
			dtdetail.Columns.Add("OrderId", typeof(int));
			dtdetail.Columns.Add("OrderNo");
			dtdetail.Columns.Add("RefPartyId", typeof(int));
			dtdetail.Columns.Add("WareHouse");
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemCode");
			dtdetail.Columns.Add("Item");
			dtdetail.Columns.Add("BrandItemId", typeof(int));
			dtdetail.Columns.Add("BrandItemCode");
			dtdetail.Columns.Add("BrandItemName");
			dtdetail.Columns.Add("CropYear");
			dtdetail.Columns.Add("JobId", typeof(int));
			dtdetail.Columns.Add("Job");
			dtdetail.Columns.Add("PackingTypeId", typeof(int));
			dtdetail.Columns.Add("PackingType");
			dtdetail.Columns.Add("UOM");
			dtdetail.Columns.Add("PUomEquivalent");
			dtdetail.Columns.Add("UOMId", typeof(int));
			dtdetail.Columns.Add("Qty", typeof(double));
			dtdetail.Columns.Add("GrossWight", typeof(double));
			dtdetail.Columns.Add("EbUnit", typeof(double));
			dtdetail.Columns.Add("EbTotal", typeof(double));
			dtdetail.Columns.Add("WtCut", typeof(double));
			dtdetail.Columns.Add("WtCutTotal", typeof(double));
			dtdetail.Columns.Add("AddLesswt", typeof(double));
			dtdetail.Columns.Add("ShortWeight", typeof(double));
			dtdetail.Columns.Add("NetWeight", typeof(double));
			dtdetail.Columns.Add("EbUnitStock", typeof(double));
			dtdetail.Columns.Add("EbTotalStock", typeof(double));
			dtdetail.Columns.Add("StockWeight", typeof(double));
			dtdetail.Columns.Add("ItemRate", typeof(double));
			dtdetail.Columns.Add("ContainerNo", typeof(string));
			dtdetail.Columns.Add("LabNo");
			dtdetail.Columns.Add("CityId", typeof(int));
			dtdetail.Columns.Add("City");
			dtdetail.Columns.Add("AvailableStock", typeof(double));
			dtdetail.Columns.Add("BalQty", typeof(double));
			dtdetail.Columns.Add("DoId", typeof(int));
			dtdetail.Columns.Add("DoDetailId", typeof(int));
			dtdetail.Columns.Add("RefDocType");
			dtdetail.Columns.Add("RefDocNo", typeof(int));
			dtdetail.Columns.Add("RateUomId", typeof(int));
			dtdetail.Columns.Add("WareHouseToId", typeof(int));
			dtExpense.Columns.Add("ItemId");
			dtExpense.Columns.Add("Qty", typeof(double));
			dtExpense.Columns.Add("Remarks");
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			CmbGpNo.Enabled = false;
			panel5.Dock = DockStyle.Top;
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
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			DataTable historyComboData = HistoryComboDbCall("Supplier");
			string documentNoData = Conversion.ToString((object)CommonServices.InvGdnGenerateCode(86));
			DataTable referencePartyData = CommonServices.ReferencePartyServiceBind();
			OtherItemdtDbCall();
			DataTable gatepassData = GatepassDataDbCall();
			return new
			{
				HistoryComboData = historyComboData,
				documentNoData = documentNoData,
				ReferencePartyData = referencePartyData,
				GatepassData = gatepassData
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
				printToolStripButton.Enabled = formright.DoHavePrintRights;
				btnPrint260A.Enabled = formright.DoHavePrintRights;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				btnDelete.Visible = false;
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
				if (SaleCostingJobOrderWise)
				{
					dtJobLotWithJobOrderAndItems = jobLot.JobLot_GetWithJobOrderAndItem(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
				}
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
				SupplierDtFillFromGlobal();
				BindSupplierName();
				TransporterDtFillFromGlobal();
				TransporterAcFill();
				WarehousesDtFillFromGlobalAndBind();
				ItemDtFillFromGlobal();
				ItemNameBind();
				BrandDtFillFromGlobal();
				BrandItemNameBind();
				CropDtFillFromGlobalAndBind();
				JobLotDtFillFromGlobalAndBind();
				PackingTypeDtFillFromGlobalAndBind();
				CityDtFillFromGlobalAndBind();
				GetConfigurationsFromGlobalAndBindValuesInColumns();
				if (dtdetail != null && dtdetail.Columns.Count > 0)
				{
					grd.DataSource = dtdetail;
					grd.RetrieveStructure();
					grdSettings();
				}
				if (dtExpense != null && dtExpense.Columns.Count > 0)
				{
					AddRowInvExpGrid();
					GridExpense.RetrieveStructure();
					grdInvExpSettings();
				}
				GatepassGridFill(result.GatepassData);
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable HistoryComboDbCall(string Activity = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			return InvGdn.GetDataForDropDownFromGdn(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesIds = Conversion.ToString((object)UserAccount.BranchesId),
				Activity = Activity
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
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobalAndBindValuesInColumns()
	{
		try
		{
			int jobId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Job/Lot"));
			if (jobId != 0 && dtjob.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == jobId))
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
				CmbWareHouseName.Value = wareHouseId;
			}
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				int transporterId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("FreightOutwardAc"));
				if (transporterId != 0)
				{
					CmbTransport.Value = transporterId;
				}
			}
			int cityId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("City Area"));
			if (cityId > 0)
			{
				CmbCity.Value = cityId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			EbWeightEditableForSaleTypePackWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("EbWeightEditableForSaleTypePackWise"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			WagesStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WagesCompulsoryOnGdn"));
			WagesActiveOrInActive = GlobalVariables_Helper.GetWagesRefDocumentsStatusById(86);
			LoaderStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IssuanceByLoader"));
			StockLoaderVisibleInGDNDetail = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("StockLoaderVisibleInGDNDetail"));
			IsStockReservedPerParty = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IsStockReservedPerParty"));
			GdnAndFactoryWeightCompareToleranceForStop = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("GdnAndFactoryWeightCompareToleranceForStop"));
			ChkIsStockReservedPerParty.Visible = IsStockReservedPerParty;
			if (Conversion.ToDouble((object)GdnAndFactoryWeightCompareToleranceForStop) == 0.0)
			{
				GdnAndFactoryWeightCompareToleranceForStop = 61.0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string documentData)
	{
		txtdocno.Text = documentData;
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			List<getGlobalSupplierCustomer> obj = clsGlobalVariables.globalSupplierCustomer;
			dtSupplier.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				List<getGlobalSupplierCustomer> lst = (from r in obj.AsEnumerable()
					where r.PartyTypeId == 2
					select r).ToList();
				if (lst != null && lst.Count > 0)
				{
					for (int i = 0; i < lst.Count; i++)
					{
						dtSupplier.Rows.Add(Conversion.ToInt((object)lst[i].Id), Conversion.ToString((object)lst[i].CompanyName), Conversion.ToString((object)lst[i].PartyCode), Conversion.ToInt((object)lst[i].GlAccountId), Conversion.ToInt((object)lst[i].CityId), Conversion.ToString((object)lst[i].CityName), Conversion.ToString((object)lst[i].MobilePersonal));
					}
				}
			}
			else if (obj != null && obj.Count > 0)
			{
				for (int j = 0; j < obj.Count; j++)
				{
					dtSupplier.Rows.Add(Conversion.ToInt((object)obj[j].Id), Conversion.ToString((object)obj[j].CompanyName), Conversion.ToString((object)obj[j].PartyCode), Conversion.ToInt((object)obj[j].GlAccountId), Conversion.ToInt((object)obj[j].CityId), Conversion.ToString((object)obj[j].CityName), Conversion.ToString((object)obj[j].MobilePersonal));
				}
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
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerName, dtSupplier, "Id", "CompanyName", "Customer Name", AllColumns: true, Conversion.ToInt(CmbCustomerName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			DataTable dataTable = dtSupplier;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TransporterDtFillFromGlobal()
	{
		try
		{
			List<getGlobalVendorsAndCustomersForTransporter> lst = clsGlobalVariables.getGlobalVendorsAndCustomersForTransporters;
			dtTransporter.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				CommonServices.GetVendorsAndCustomersForTransporter();
				if (lst != null && lst.Count > 0)
				{
					for (int i = 0; i < lst.Count; i++)
					{
						dtTransporter.Rows.Add(lst[i].GlAccountId, lst[i].CompanyName, lst[i].PartyCode, lst[i].Id);
					}
				}
				return;
			}
			int[] excludedIds = new int[6] { 2, 4, 10, 11, 12, 15 };
			IEnumerable<getGlobalAllAccountsWithCustomGroup> enumerable = clsGlobalVariables.AllAccountsWithCustomGroupId.Where((getGlobalAllAccountsWithCustomGroup r) => !excludedIds.Contains(r.AccountTypeId));
			HashSet<int> existingIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup acc in enumerable)
			{
				int id = Conversion.ToInt((object)acc.ChartOfAccountId);
				if (!existingIds.Contains(id))
				{
					dtTransporter.Rows.Add(acc.ChartOfAccountId, acc.AccountTitle, acc.AccountCode, 0);
				}
				existingIds.Add(id);
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
			InfragisticsHelper.BindAndRetainSelection(CmbTransport, dtTransporter, "Id", "AccountTitle", "Transporter Name", AllColumns: true, Conversion.ToInt(CmbTransport.Value));
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
			DatatableHelper.PopulateDataTable(dtwh, lst, (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName);
			InfragisticsHelper.BindAndRetainSelection(CmbWareHouseName, dtwh, "Id", "WareHouseName", "WareHouse Name", AllColumns: true, Conversion.ToInt(CmbWareHouseName.Value), insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(CmbWareHouseTo, dtwh, "Id", "WareHouseName", "WareHouse To", AllColumns: true, Conversion.ToInt(CmbWareHouseTo.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemDtFillFromPoDBCall()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		try
		{
			dtItem.Clear();
			DataTable dtPoDetail = SaleOrder.ReadByPurchaseOrderIDNOrderItemId(new SaleOrder
			{
				Id = Conversion.ToInt(CmbOrderNo.Value)
			});
			if (dtPoDetail == null || dtPoDetail.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow dr in dtPoDetail.Rows)
			{
				dtItem.Rows.Add(dr["OrderItemId"], dr["ItemName"], dr["ItemCodeNew"], dr["OrderItemRate"], dr["RateUOM"], dr["Id"], dr["OrderItemUOMId"], dr["Equivalent"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				dtItem.Rows.Add(row2.Id, row2.ItemName, row2.ItemCode, 0);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BrandDtFillFromGlobal()
	{
		try
		{
			dtBrandItem.Clear();
			List<getGlobalAllBrands> dtItemAll = clsGlobalVariables.getGlobalAllBrands.ToList();
			if (dtItemAll == null || dtItemAll.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllBrands row in dtItemAll)
			{
				dtBrandItem.Rows.Add(row.Id, row.BrandName, row.BrandCode);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BrandItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbBrandItemName.Value);
			string ValueMember = (RadBrandName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (RadBrandName.Checked ? "Brand Name" : "Brand Code");
			InfragisticsHelper.BindAndRetainSelection(CmbBrandItemName, dtBrandItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			combitem_Leave(null, null);
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
			dtjob.Clear();
			if (SaleCostingJobOrderWise)
			{
				HashSet<int> Ids = new HashSet<int>();
				foreach (DataRow dr in dtJobLotWithJobOrderAndItems.Rows)
				{
					int ID = Conversion.ToInt(dr["Id"]);
					if (!Ids.Contains(ID))
					{
						dtjob.Rows.Add(ID, dr["JobLotDescription"]);
						Ids.Add(ID);
					}
				}
			}
			else
			{
				List<GetJobLotGlIdsandName> lst = clsGlobalVariables.globalJobLot;
				DatatableHelper.PopulateDataTable(dtjob, lst, (GetJobLotGlIdsandName x) => x.Id, (GetJobLotGlIdsandName x) => x.JobLotDescription);
			}
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
			List<InvPackingType> Finallist = clsGlobalVariables.globalInvPackingType;
			DatatableHelper.PopulateDataTable(dtPack, Finallist, (InvPackingType x) => x.Id, (InvPackingType x) => x.PackTypeDesc);
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
			string Packuom = ((Control)(object)CmbPackUom).Text;
			dtUOM.Clear();
			dtUOM = CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId);
			if (dtUOM != null)
			{
				InfragisticsHelper.BindAndRetainSelection(CmbPackUom, dtUOM, "Id", "UOMCode", "Uom", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
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

	private void combitem_Leave(object sender, EventArgs e)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		try
		{
			AvailableStock();
			if (CmbOrderNo.Value != null)
			{
				GatePassRecordFill();
				PackUomFromGlobalBind(Conversion.ToInt(CmbItemName.Value));
				if (txtDeliverTerm.Text != "Ponch")
				{
					txtgwt.Text = txtfctwt.Text.Trim();
				}
				if (txtDeliverTerm.Text == "Ponch")
				{
					double FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
					txtgwt.Text = FactoryWeight.ToString();
				}
				DataTable dtSODetailget = new DataTable();
				dtSODetailget = SaleOrder.ReadByPurchaseOrderIDNOrderItemId(new SaleOrder
				{
					Id = Conversion.ToInt(CmbOrderNo.Value)
				});
				if (dtSODetailget.Rows.Count > 0)
				{
					CmbPackUom.Value = dtSODetailget.Rows[0]["OrderItemUOMId"].ToString();
					((Control)(object)CmbJobLot).Text = dtSODetailget.Rows[0]["JobLotDescription"].ToString();
					txtebu.Text = dtSODetailget.Rows[0]["BagWeight"].ToString();
					((Control)(object)CmbCity).Text = dtSODetailget.Rows[0]["CityArea"].ToString();
					comblabno.Text = dtSODetailget.Rows[0]["LabSampleNo"].ToString();
				}
			}
			else
			{
				txtgwt.Enabled = true;
				double SupplierWeight = 0.0;
				double FactoryWeight2 = 0.0;
				if (txtsuppwt.Text.Trim() != string.Empty)
				{
					SupplierWeight = Math.Round(Conversion.ToDouble((object)txtsuppwt.Text.Trim()), 2);
				}
				if (txtfctwt.Text.Trim() != string.Empty)
				{
					FactoryWeight2 = Math.Round(Conversion.ToDouble((object)txtfctwt.Text.Trim()), 2);
				}
				if (SupplierWeight > FactoryWeight2)
				{
					txtgwt.Text = FactoryWeight2.ToString();
				}
				else
				{
					txtgwt.Text = FactoryWeight2.ToString();
				}
				JobLotDtFillFromGlobalAndBind();
				PackUomFromGlobalBind(Conversion.ToInt(CmbItemName.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combjoblot_Leave(object sender, EventArgs e)
	{
		AvailableStock();
	}

	private void combwr_Leave(object sender, EventArgs e)
	{
		AvailableStock();
	}

	private void comborderno_Leave(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		try
		{
			if (CmbOrderNo.Value != null && Convert.ToInt32(CmbOrderNo.Value) != 0)
			{
				DataTable dtGetDTGPType = SaleOrder.GetGpTypeandDEliveryTerm(new SaleOrder
				{
					Id = Conversion.ToInt(CmbOrderNo.Value)
				});
				if (dtGetDTGPType.Rows.Count > 0)
				{
					txtDeliverTerm.Text = dtGetDTGPType.Rows[0]["DeliveryTerm"].ToString();
					GpType = dtGetDTGPType.Rows[0]["GatepassType"].ToString();
				}
				SaleOrder SoForSupplier = new SaleOrder();
				SoForSupplier.OrganizationId = UserAccount.OrganizationId;
				SoForSupplier.CompanyId = UserAccount.CompanyId;
				if (GpType == "Export")
				{
					SoForSupplier.DocumentTypeId = 83;
				}
				else
				{
					SoForSupplier.DocumentTypeId = 81;
				}
				SoForSupplier.Id = Conversion.ToInt(CmbOrderNo.Value);
				DataTable dtsupp = SaleOrder.GetSupplierBySaleOrderNo(SoForSupplier);
				if (dtsupp.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("CompanyName");
					for (int i = 0; i < dtsupp.Rows.Count; i++)
					{
						dt.Rows.Add(dtsupp.Rows[i]["Id"], dtsupp.Rows[i]["CompanyName"]);
					}
					if (dt.Rows.Count > 0)
					{
						DDL.BindDDL(dt, CmbCustomerName, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
						((UltraGridBase)CmbCustomerName).Rows[0].Activate();
					}
					ItemDtFillFromPoDBCall();
					ItemNameBind();
				}
			}
			else
			{
				ItemDtFillFromGlobal();
				ItemNameBind();
				JobLotDtFillFromGlobalAndBind();
				PackUomFromGlobalBind(Conversion.ToInt(CmbItemName.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combsupplier_Leave(object sender, EventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		try
		{
			if (!btnSave.Visible || !btnSave.Enabled)
			{
				return;
			}
			DataTable dtSrNoGet = GatePassOutward.ReadByGpNoForGdnDeliveryOrder(new GatePassOutward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = GPID,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dtSrNoGet.Rows.Count > 0)
			{
				GPID = Conversion.ToInt(dtSrNoGet.Rows[0]["Id"]);
				SoOrderId = Conversion.ToInt(dtSrNoGet.Rows[0]["SaleOrderId"]);
				OrderStatus = Conversion.ToString(dtSrNoGet.Rows[0]["OrderStatus"]);
				txtvehno.Text = Conversion.ToString(dtSrNoGet.Rows[0]["VehicleNo"]);
				txtbltyno.Text = Conversion.ToString(dtSrNoGet.Rows[0]["BiltyNo"]);
				txtfctwt.Text = Conversion.ToString(dtSrNoGet.Rows[0]["FactoryWeight"]);
				txtcarramount.Text = Conversion.ToString(dtSrNoGet.Rows[0]["NetPaid"]);
				DeliveryOrderType = Conversion.ToString(dtSrNoGet.Rows[0]["DeliveryOrderType"]);
				if (IsStockReservedPerParty)
				{
					ChkIsStockReservedPerParty.Checked = Conversion.ToBool(dtSrNoGet.Rows[0]["IsStockReserved"]);
				}
				txtDeliverTerm.Text = OrderStatus;
				if (Conversion.ToInt(dtSrNoGet.Rows[0]["SaleTypeId"]) == 2)
				{
					txtSaleType.Text = "Pack Size Based Billing";
					txtSaleType.Tag = 2;
				}
				else
				{
					txtSaleType.Text = "Weigh Bridge Based Billing";
					txtSaleType.Tag = 1;
				}
				if (OrderStatus == "DeliverOrder")
				{
					chkShortWeightLessFromNetWeight.Visible = false;
					panel5.Dock = DockStyle.None;
					txtDeliverTerm.Size = new Size(120, 23);
					DataTable dtSupplier = InvDeliveryOrder.CustomerLoadForDeliveryOrderId(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						Id = GPID
					});
					if (dtSupplier.Rows.Count > 0)
					{
						int count = dtSupplier.Rows.Count;
						txtPartyCount.Text = count.ToString();
						DDL.BindDDL(dtSupplier, CmbCustomerName, "SupplierCustomerId", "CompanyName", "CustomerName", ZeroIndex: false);
						((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[1].Width = 300;
					}
					double GdnGrossWeight = InvGdn.GetGdnGrossWeightForValidation(new InvGdn
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 86,
						Id = Conversion.ToInt((object)GPID)
					});
					if (GdnGrossWeight > 0.0)
					{
						txtGdnGrossWeight.Text = GdnGrossWeight.ToString();
					}
					else
					{
						txtGdnGrossWeight.Text = "0";
					}
					Conversion.ToDouble((object)txtGdnGrossWeight.Text.Trim());
					double FinalWt = Conversion.ToDouble((object)txtfctwt.Text.Trim()) - GdnGrossWeight;
					txtBalanceWeight.Text = FinalWt.ToString();
					DeliverOrderRecordFill();
					if (Conversion.ToDouble((object)txtShortExcessWeight.Text) != 0.0)
					{
						if (Conversion.ToDouble((object)txtShortExcessWeight.Text) < 0.0 && Conversion.ToInt(txtSaleType.Tag) == 2)
						{
							chkShortWeightLessFromNetWeight.Visible = true;
							label60.Text = "Short Weight";
							ProportionateShortWeight();
						}
						else if (Conversion.ToDouble((object)txtShortExcessWeight.Text) > 0.0)
						{
							label60.Text = "Excess Weight";
							chkShortWeightLessFromNetWeight.Visible = false;
						}
						else
						{
							label60.Text = "Short/Excess Wt";
							chkShortWeightLessFromNetWeight.Visible = false;
						}
					}
				}
				else
				{
					txtBalanceWeight.Text = "0";
					txtPartyCount.Text = "0";
					panel5.Dock = DockStyle.Top;
					chkShortWeightLessFromNetWeight.Visible = false;
					txtDeliverTerm.Size = new Size(215, 23);
				}
			}
			BalGrossWeightGrd();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDeliverTerm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtDeliverTerm.Text == "DeliverOrder")
			{
				btnGenerateAvailableStock.Visible = true;
			}
			else
			{
				btnGenerateAvailableStock.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemIdExistsInJobLot(int itemId, string itemName, int jobLotId, string jobLotName, int rowNo = 0)
	{
		try
		{
			if (!SaleCostingJobOrderWise || dtJobLotWithJobOrderAndItems.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == jobLotId && row.Field<int>("ItemId") == itemId))
			{
				return;
			}
			throw new Exception((rowNo == 0) ? ("The selected item '" + itemName + "' does not exist against the job lot '" + jobLotName + "'.") : $"The selected item '{itemName}' does not exist against the job lot '{jobLotName}' in detail grid row no: {rowNo}.");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void RadBrandName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			BrandItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChkIsStockReservedPerParty_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbAdvanceDo).Visible = IsStockReservedPerParty && ChkIsStockReservedPerParty.Checked;
			labelAdvDoNo.Visible = IsStockReservedPerParty && ChkIsStockReservedPerParty.Checked;
			((Control)(object)CmbWareHouseTo).Visible = IsStockReservedPerParty && ChkIsStockReservedPerParty.Checked;
			labelWareTo.Visible = IsStockReservedPerParty && ChkIsStockReservedPerParty.Checked;
			((Control)(object)CmbCity).Width = ((IsStockReservedPerParty && ChkIsStockReservedPerParty.Checked) ? 130 : 260);
			if (IsStockReservedPerParty && ChkIsStockReservedPerParty.Checked)
			{
				AdvanceDoBind();
			}
			if (grd.RowCount > 0)
			{
				grd.RootTable.Columns["WareHouseToId"].Visible = IsStockReservedPerParty && ChkIsStockReservedPerParty.Checked;
				grd.RootTable.Columns["WareHouseToId"].ColumnType = (ColumnType)0;
				grd.RootTable.Columns["WareHouseToId"].HasValueList = true;
				grd.RootTable.Columns["WareHouseToId"].LimitToList = true;
				grd.RootTable.Columns["WareHouseToId"].Caption = "WareHouse To";
				grd.RootTable.Columns["WareHouseToId"].ValueList.PopulateValueList((IEnumerable)dtwh.DefaultView, "Id", "WareHouseName");
				grd.RootTable.Columns["WareHouseToId"].Width = 150;
				grd.RootTable.Columns["WareHouseToId"].EditType = (EditType)4;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AdvanceDoBind()
	{
		try
		{
			DataTable dtAdvanceDo = InvDeliveryOrder.DeliveryOrder_GetAdvanceDo(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.BranchesId, clsGlobalVariables.ActiveYr.Id, Conversion.ToInt(CmbCustomerName.Value), 0, Id);
			InfragisticsHelper.BindAndRetainSelection(CmbAdvanceDo, dtAdvanceDo, "Id", "DocNo", "Advance Do No", AllColumns: true, Conversion.ToInt(CmbAdvanceDo.Value), insertDefaultRow: true);
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

	private void DeliverOrderRecordFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		try
		{
			dtdetail.Rows.Clear();
			DataTable dt = InvDeliveryOrder.DeliveryOrderLoad(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 84,
				SupplierCustomerId = Conversion.ToInt(CmbCustomerName.Value),
				Id = SoOrderId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtdetail.Rows.Add(0, dt.Rows[i]["RefDocumentTypeId"], dt.Rows[i]["RefDocIdNo"], dt.Rows[i]["RefDocSubIdNo"], dt.Rows[i]["SaleOrderDetailId"], dt.Rows[i]["OrderId"], dt.Rows[i]["OrderNo"], dt.Rows[i]["RefPartyId"], dt.Rows[i]["WareHouse"], dt.Rows[i]["ItemId"], dt.Rows[i]["ItemCode"], dt.Rows[i]["Item"], 0, "", "", dt.Rows[i]["CropYear"], dt.Rows[i]["JobId"], dt.Rows[i]["Job"], dt.Rows[i]["PackingTypeId"], dt.Rows[i]["PackingType"], dt.Rows[i]["UOM"], dt.Rows[i]["PUomEquivalent"], dt.Rows[i]["UOMId"], dt.Rows[i]["Qty"], dt.Rows[i]["GrossWight"], (Conversion.ToInt(txtSaleType.Tag) == 1) ? dt.Rows[i]["EbUnit"] : ((object)0), (Conversion.ToInt(txtSaleType.Tag) == 1) ? dt.Rows[i]["EbTotal"] : ((object)0), dt.Rows[i]["WtCut"], dt.Rows[i]["WtCutTotal"], 0, 0, dt.Rows[i]["NetWeight"], dt.Rows[i]["EbUnit"], dt.Rows[i]["EbTotal"], Conversion.ToDouble(dt.Rows[i]["StockWeight"]), dt.Rows[i]["ItemRate"], dt.Rows[i]["ContainerNo"], dt.Rows[i]["LabNo"], dt.Rows[i]["CityId"], dt.Rows[i]["City"], 0, dt.Rows[i]["Qty"], dt.Rows[i]["InvDeliveryOrderId"], dt.Rows[i]["InvDeliveryOrderDetailId"], "", 0, 0, dt.Rows[i]["WareHouse"]);
				}
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings();
				NetandStockWeight();
			}
			else
			{
				grd.ClearStructure();
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
			ItemIdExistsInJobLot(Conversion.ToInt(CmbItemName.Value), Conversion.ToString((object)((Control)(object)CmbItemName).Text), Conversion.ToInt(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text));
			if (FormDetailValidation() && (Conversion.ToInt(CmbPackUom.Value) == Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["PackUomId"].Value) || MessageBox.Show("The Pack UOM of the selected item in the order does not match the current selected Pack UOM. Are you sure you want to add this record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.No))
			{
				int BrandId = Conversion.ToInt(CmbBrandItemName.Value);
				string BrandName = "";
				string BrandCode = "";
				if (((UltraGridBase)CmbBrandItemName).ActiveRow != null && BrandId > 0)
				{
					BrandCode = Conversion.ToString(((UltraDropDownBase)CmbBrandItemName).SelectedRow.Cells["ItemCode"].Value);
					BrandName = Conversion.ToString(((UltraDropDownBase)CmbBrandItemName).SelectedRow.Cells["ItemName"].Value);
				}
				((Control)(object)CmbOrderNo).Enabled = false;
				dtdetail.Rows.Add(0, 0, 0, 0, ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["SoDetailId"].Value, CmbOrderNo.Value, ((Control)(object)CmbOrderNo).Text, 0, CmbWareHouseName.Value, CmbItemName.Value, Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value), BrandId, BrandCode, BrandName, ((Control)(object)CmbCropYear).Text, CmbJobLot.Value, ((Control)(object)CmbJobLot).Text, CmbPackingType.Value, ((Control)(object)CmbPackingType).Text, Conversion.ToString((object)((Control)(object)CmbPackUom).Text.Trim()), Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value), Conversion.ToInt(CmbPackUom.Value), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtgwt.Text.Trim()), Conversion.ToDouble((object)txtebu.Text.Trim()), Conversion.ToDouble((object)txtebt.Text.Trim()), Conversion.ToDouble((object)txtwtcut.Text.Trim()), Conversion.ToDouble((object)txtwtcuttotal.Text.Trim()), Conversion.ToDouble((object)txtadlswt.Text.Trim()), 0, Conversion.ToDouble((object)txtnetwt.Text.Trim()), Conversion.ToDouble((object)txtEbUnitStock.Text.Trim()), Conversion.ToDouble((object)txtEbTotalStock.Text.Trim()), Conversion.ToDouble((object)txtstockwt.Text.Trim()), 0, txtContainerNodetail.Text, comblabno.Text, CmbCity.Value, ((Control)(object)CmbCity).Text, 0, 0, 0, 0, "", 0, 0, Conversion.ToInt(CmbWareHouseTo.Value));
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
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				if (item.Cells["OrderNo"].Text == string.Empty)
				{
					((Control)(object)CmbOrderNo).Enabled = false;
				}
				DataTable dt = new DataTable();
				dt.Columns.Add("OrderId");
				dt.Columns.Add("OrderNo");
				dt.Clear();
				dt.Rows.Add(item.Cells["OrderId"].Value.ToString(), item.Cells["OrderNo"].Text.ToString());
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbOrderNo, "OrderId", "OrderNo", "Order No", ZeroIndex: false);
				}
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				if (Conversion.ToInt(item.Cells["BrandItemId"].Value) > 0)
				{
					CmbBrandItemName.Value = item.Cells["BrandItemId"].Value.ToString();
				}
				((Control)(object)CmbCropYear).Text = item.Cells["CropYear"].Text.ToString();
				CmbJobLot.Value = item.Cells["JobId"].Value.ToString();
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				txtqty.Text = item.Cells["Qty"].Value.ToString();
				CmbPackUom.Value = item.Cells["UOMId"].Value.ToString();
				txtgwt.Text = item.Cells["GrossWight"].Value.ToString();
				txtebu.Text = item.Cells["EbUnit"].Value.ToString();
				txtebt.Text = item.Cells["EbTotal"].Value.ToString();
				txtwtcut.Text = item.Cells["WtCut"].Value.ToString();
				txtwtcuttotal.Text = item.Cells["WtCutTotal"].Value.ToString();
				txtadlswt.Text = item.Cells["AddLesswt"].Value.ToString();
				txtnetwt.Text = item.Cells["NetWeight"].Value.ToString();
				txtstockwt.Text = item.Cells["StockWeight"].Value.ToString();
				CmbWareHouseName.Value = item.Cells["WareHouse"].Value.ToString();
				txtContainerNodetail.Text = item.Cells["ContainerNo"].Value.ToString();
				comblabno.Text = item.Cells["LabNo"].Value.ToString();
				CmbCity.Value = item.Cells["CityId"].Value.ToString();
				((Control)(object)CmbCity).Text = item.Cells["City"].Value.ToString();
				CmbWareHouseTo.Value = item.Cells["WareHouseToId"].Value.ToString();
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

	private void GridComboBind()
	{
		try
		{
			grd.RootTable.Columns["WareHouse"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["WareHouse"].HasValueList = true;
			grd.RootTable.Columns["WareHouse"].LimitToList = true;
			grd.RootTable.Columns["WareHouse"].Caption = "WareHouseName";
			grd.RootTable.Columns["WareHouse"].ValueList.PopulateValueList((IEnumerable)dtwh.DefaultView, "Id", "WareHouseName");
			if (IsStockReservedPerParty && ChkIsStockReservedPerParty.Checked)
			{
				grd.RootTable.Columns["WareHouseToId"].ColumnType = (ColumnType)0;
				grd.RootTable.Columns["WareHouseToId"].HasValueList = true;
				grd.RootTable.Columns["WareHouseToId"].LimitToList = true;
				grd.RootTable.Columns["WareHouseToId"].Caption = "WareHouse To";
				grd.RootTable.Columns["WareHouseToId"].ValueList.PopulateValueList((IEnumerable)dtwh.DefaultView, "Id", "WareHouseName");
			}
			grd.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["PackingTypeId"].HasValueList = true;
			grd.RootTable.Columns["PackingTypeId"].LimitToList = true;
			grd.RootTable.Columns["PackingTypeId"].Caption = "Pack Type";
			grd.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtPack.DefaultView, "Id", "PackTypeDesc");
			grd.RootTable.Columns["CropYear"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["CropYear"].HasValueList = true;
			grd.RootTable.Columns["CropYear"].LimitToList = true;
			grd.RootTable.Columns["CropYear"].Caption = "Crop Year";
			grd.RootTable.Columns["CropYear"].ValueList.PopulateValueList((IEnumerable)dtCrop.DefaultView, "Id", "CropYear");
			grd.RootTable.Columns["City"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["City"].HasValueList = true;
			grd.RootTable.Columns["City"].LimitToList = true;
			grd.RootTable.Columns["City"].Caption = "City Name";
			grd.RootTable.Columns["City"].ValueList.PopulateValueList((IEnumerable)dtcity.DefaultView, "Id", "CityName");
			grd.RootTable.Columns["JobId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["JobId"].HasValueList = true;
			grd.RootTable.Columns["JobId"].LimitToList = true;
			grd.RootTable.Columns["JobId"].Caption = "Job Lot";
			grd.RootTable.Columns["JobId"].ValueList.PopulateValueList((IEnumerable)dtjob.DefaultView, "Id", "JobLotDescription");
			grd.RootTable.Columns["WareHouse"].EditType = (EditType)4;
			grd.RootTable.Columns["WareHouseToId"].EditType = (EditType)4;
			grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
			grd.RootTable.Columns["CropYear"].EditType = (EditType)4;
			grd.RootTable.Columns["City"].EditType = (EditType)4;
			grd.RootTable.Columns["JobId"].EditType = (EditType)4;
			if (grd == null || grd.CurrentRow == null)
			{
				return;
			}
			grd.RootTable.Columns["EbUnit"].EditType = (EditType)((Conversion.ToInt(txtSaleType.Tag) != 2 || EbWeightEditableForSaleTypePackWise) ? 1 : 0);
			grd.RootTable.Columns["EbTotal"].EditType = (EditType)((Conversion.ToInt(txtSaleType.Tag) != 2 || EbWeightEditableForSaleTypePackWise) ? 1 : 0);
			grd.RootTable.Columns["EbUnit"].Visible = ((Conversion.ToInt(txtSaleType.Tag) != 2 || EbWeightEditableForSaleTypePackWise) ? true : false);
			grd.RootTable.Columns["EbTotal"].Visible = ((Conversion.ToInt(txtSaleType.Tag) != 2 || EbWeightEditableForSaleTypePackWise) ? true : false);
			if (Conversion.ToInt(txtSaleType.Tag) == 2 && !EbWeightEditableForSaleTypePackWise)
			{
				GridEXRow[] dataRows = grd.GetDataRows();
				foreach (GridEXRow obj in dataRows)
				{
					obj.BeginEdit();
					obj.Cells["EbUnit"].Value = 0;
					obj.Cells["EbTotal"].Value = 0;
					obj.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["WareHouse"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grd.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["BrandItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grd.RootTable.Columns["BrandItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grd.RootTable.Columns["Job"].Width = Constants.InventoryConstants.JobLotConstant;
			grd.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			grd.RootTable.Columns["UOM"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["GrossWight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["EbUnit"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["EbTotal"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["WtCut"].Width = Constants.InventoryConstants.WtCut;
			grd.RootTable.Columns["WtCutTotal"].Width = Constants.InventoryConstants.WtCut;
			grd.RootTable.Columns["AddLesswt"].Width = Constants.InventoryConstants.AddLessConstant;
			grd.RootTable.Columns["ShortWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["EbUnitStock"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["EbTotalStock"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["ContainerNo"].Width = Constants.InventoryConstants.ContainerConstant;
			grd.RootTable.Columns["LabNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["City"].Width = Constants.InventoryConstants.CityName;
			grd.RootTable.Columns["AvailableStock"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["RefDocType"].Width = Constants.InventoryConstants.DocumentType;
			grd.RootTable.Columns["RefDocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["WareHouseToId"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["RefPartyId"].Visible = false;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["RefDocIdNo"].Visible = false;
			grd.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grd.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grd.RootTable.Columns["CityId"].Visible = false;
			grd.RootTable.Columns["PUomEquivalent"].Visible = false;
			grd.RootTable.Columns["ItemRate"].Visible = false;
			grd.RootTable.Columns["BalQty"].Visible = false;
			grd.RootTable.Columns["DoId"].Visible = false;
			grd.RootTable.Columns["DoDetailId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["BrandItemId"].Visible = false;
			grd.RootTable.Columns["WareHouseToId"].Visible = IsStockReservedPerParty && ChkIsStockReservedPerParty.Checked;
			grd.RootTable.Columns["WtCut"].Visible = false;
			grd.RootTable.Columns["WtCutTotal"].Visible = false;
			grd.RootTable.Columns["UOM"].Caption = "Pack Uom";
			grd.RootTable.Columns["AddLesswt"].Caption = "Add Weight";
			grd.RootTable.Columns["GrossWight"].Caption = "Gross Weight";
			grd.RootTable.Columns["NetWeight"].Caption = "Net Bill Weight";
			if (MultipleGdnsAgainstSingleParty)
			{
				grd.RootTable.Columns["ItemRate"].Visible = true;
			}
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocType"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocNo"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocIdNo"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocSubIdNo"].EditType = (EditType)0;
			grd.RootTable.Columns["SaleOrderDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["Qty"].EditType = (EditType)0;
			grd.RootTable.Columns["WtCut"].EditType = (EditType)0;
			grd.RootTable.Columns["WtCutTotal"].EditType = (EditType)0;
			grd.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["ContainerNo"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["Item"].EditType = (EditType)0;
			grd.RootTable.Columns["BrandItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["BrandItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["Job"].EditType = (EditType)0;
			grd.RootTable.Columns["LabNo"].EditType = (EditType)0;
			grd.RootTable.Columns["UOM"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemRate"].EditType = (EditType)0;
			grd.RootTable.Columns["PUomEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["ShortWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["StockWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["AvailableStock"].EditType = (EditType)0;
			grd.RootTable.Columns["BalQty"].EditType = (EditType)0;
			grd.RootTable.Columns["DoId"].EditType = (EditType)0;
			grd.RootTable.Columns["DoDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["AvailableStock"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["WtCut"].EditType = (EditType)0;
			grd.RootTable.Columns["WtCutTotal"].EditType = (EditType)0;
			grd.RootTable.Columns["CropYear"].EditType = (EditType)1;
			grd.RootTable.Columns["PackingType"].EditType = (EditType)1;
			grd.RootTable.Columns["WareHouse"].EditType = (EditType)1;
			grd.RootTable.Columns["City"].EditType = (EditType)1;
			grd.RootTable.Columns["JobId"].EditType = (EditType)1;
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWight"].FormatString = "#,##0.00";
			grd.RootTable.Columns["GrossWight"].TotalFormatString = "#,##0.00";
			grd.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbUnit"].FormatString = "#,##0.000";
			grd.RootTable.Columns["EbUnit"].TotalFormatString = "#,##0.000";
			grd.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbTotal"].FormatString = "#,##0.00";
			grd.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.00";
			grd.RootTable.Columns["EbUnitStock"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbUnitStock"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbUnitStock"].FormatString = "#,##0.000";
			grd.RootTable.Columns["EbUnitStock"].TotalFormatString = "#,##0.000";
			grd.RootTable.Columns["EbTotalStock"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbTotalStock"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbTotalStock"].FormatString = "#,##0.00";
			grd.RootTable.Columns["EbTotalStock"].TotalFormatString = "#,##0.00";
			grd.RootTable.Columns["WtCut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WtCut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WtCutTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WtCutTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLesswt"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AddLesswt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].FormatString = "#,##0.00";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.00";
			grd.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["StockWeight"].FormatString = "#,##0.00";
			grd.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.00";
			grd.RootTable.Columns["AvailableStock"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AvailableStock"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AvailableStock"].FormatString = "#,##0.00";
			grd.RootTable.Columns["AvailableStock"].TotalFormatString = "#,##0.00";
			grd.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalQty"].FormatString = "#,##0.00";
			grd.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.00";
			grd.RootTable.Columns["ShortWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ShortWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ShortWeight"].FormatString = "#,##0.000";
			grd.RootTable.Columns["ShortWeight"].TotalFormatString = "#,##0.000";
			GridComboBind();
			if (OrderStatus == "DeliverOrder")
			{
				grd.RootTable.Columns["SaleOrderDetailId"].Visible = false;
				grd.RootTable.Columns["OrderId"].Visible = false;
				grd.RootTable.Columns["ItemId"].Visible = false;
				grd.RootTable.Columns["UOMId"].Visible = false;
				grd.RootTable.Columns["WtCut"].Visible = false;
				grd.RootTable.Columns["WtCutTotal"].Visible = false;
				grd.RootTable.Columns["ContainerNo"].Visible = false;
				grd.RootTable.Columns["Job"].Visible = false;
				grd.RootTable.Columns["PackingType"].Visible = false;
				grd.RootTable.Columns["LabNo"].Visible = false;
				grd.RootTable.Columns["ShortWeight"].Visible = ((Conversion.ToDouble((object)txtShortExcessWeight.Text) < 0.0 && Conversion.ToInt(txtSaleType.Tag) == 2) ? true : false);
				grd.RootTable.Columns["NetWeight"].EditType = (EditType)0;
				grd.RootTable.Columns["Qty"].EditType = (EditType)1;
				grd.RootTable.Columns["GrossWight"].EditType = (EditType)(Conversion.ToInt(txtSaleType.Tag) != 2);
				grd.RootTable.Columns["AddLesswt"].EditType = (EditType)(Conversion.ToInt(txtSaleType.Tag) != 2);
				grd.RootTable.Columns["EbUnit"].EditType = (EditType)((Conversion.ToInt(txtSaleType.Tag) != 2 || EbWeightEditableForSaleTypePackWise) ? 1 : 0);
				grd.RootTable.Columns["EbTotal"].EditType = (EditType)((Conversion.ToInt(txtSaleType.Tag) != 2 || EbWeightEditableForSaleTypePackWise) ? 1 : 0);
				grd.RootTable.Columns["EbUnit"].Visible = ((Conversion.ToInt(txtSaleType.Tag) != 2 || EbWeightEditableForSaleTypePackWise) ? true : false);
				grd.RootTable.Columns["EbTotal"].Visible = ((Conversion.ToInt(txtSaleType.Tag) != 2 || EbWeightEditableForSaleTypePackWise) ? true : false);
				grd.RootTable.Columns["AddLesswt"].Visible = Conversion.ToInt(txtSaleType.Tag) != 2;
				EnableDisableGridCombo();
				grd.RootTable.Columns.Add("Delete");
				grd.RootTable.Columns["Delete"].Key = "Delete";
				grd.RootTable.Columns["Delete"].Caption = "X";
				grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Delete"].ButtonText = "X";
				grd.RootTable.Columns["Delete"].Width = 20;
				grd.RootTable.Columns["Delete"].Position = 0;
				grd.RootTable.Columns.Add("Add");
				grd.RootTable.Columns["Add"].Key = "Add";
				grd.RootTable.Columns["Add"].Caption = "+";
				grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Add"].ButtonText = "+";
				grd.RootTable.Columns["Add"].Width = 20;
				grd.RootTable.Columns["Add"].Position = 1;
				if (StockLoaderVisibleInGDNDetail)
				{
					grd.RootTable.Columns["BalQty"].Visible = true;
					grd.RootTable.Columns["AvailableStock"].Visible = true;
					grd.RootTable.Columns.Add("Loader");
					grd.RootTable.Columns["Loader"].Key = "Loader";
					grd.RootTable.Columns["Loader"].Caption = "Loader";
					grd.RootTable.Columns["Loader"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					grd.RootTable.Columns["Loader"].ButtonStyle = (ButtonStyle)5;
					grd.RootTable.Columns["Loader"].ButtonText = "Loader";
					grd.RootTable.Columns["Loader"].Width = 60;
					grd.RootTable.Columns["Loader"].Position = 2;
					grd.FrozenColumns = 3;
				}
				else
				{
					grd.FrozenColumns = 2;
				}
			}
			else
			{
				grd.RootTable.Columns["SaleOrderDetailId"].Visible = false;
				grd.RootTable.Columns["ShortWeight"].Visible = false;
				grd.RootTable.Columns["OrderId"].Visible = false;
				grd.RootTable.Columns["ItemId"].Visible = false;
				grd.RootTable.Columns["UOMId"].Visible = false;
				grd.RootTable.Columns["JobId"].Visible = false;
				grd.RootTable.Columns["PackingTypeId"].Visible = false;
				grd.RootTable.Columns["AddLesswt"].Visible = true;
				grd.RootTable.Columns["EbUnit"].Visible = true;
				grd.RootTable.Columns["EbTotal"].Visible = true;
				grd.RootTable.Columns["PackingType"].EditType = (EditType)0;
				grd.RootTable.Columns["StockWeight"].EditType = (EditType)0;
				grd.RootTable.Columns["AddLesswt"].EditType = (EditType)0;
				grd.RootTable.Columns["GrossWight"].EditType = (EditType)0;
				grd.RootTable.Columns["EbUnit"].EditType = (EditType)0;
				grd.RootTable.Columns["EbTotal"].EditType = (EditType)0;
				grd.RootTable.Columns["EbUnitStock"].EditType = (EditType)0;
				grd.RootTable.Columns["EbTotalStock"].EditType = (EditType)0;
				grd.RootTable.Columns.Add("Delete");
				grd.RootTable.Columns["Delete"].Key = "Delete";
				grd.RootTable.Columns["Delete"].Caption = "X";
				grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Delete"].ButtonText = "X";
				grd.RootTable.Columns["Delete"].Width = 20;
				grd.RootTable.Columns["Delete"].Position = 0;
				grd.RootTable.Columns.Add("Add");
				grd.RootTable.Columns["Add"].Key = "Add";
				grd.RootTable.Columns["Add"].Caption = "+";
				grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Add"].ButtonText = "+";
				grd.RootTable.Columns["Add"].Width = 20;
				grd.RootTable.Columns["Add"].Position = 1;
				grd.FrozenColumns = 2;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnableDisableGridCombo()
	{
		try
		{
			if (!(OrderStatus == "DeliverOrder") || grd.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value) != 0 || Conversion.ToInt(r.Cells["RefDocIdNo"].Value) != 0 || Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value) != 0)
				{
					grd.RootTable.Columns["WareHouse"].EditType = (EditType)0;
					grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
					grd.RootTable.Columns["CropYear"].EditType = (EditType)0;
					grd.RootTable.Columns["JobId"].EditType = (EditType)0;
					break;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			ItemIdExistsInJobLot(Conversion.ToInt(CmbItemName.Value), Conversion.ToString((object)((Control)(object)CmbItemName).Text), Conversion.ToInt(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text));
			if (FormDetailValidation() && (Conversion.ToInt(CmbPackUom.Value) == Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["PackUomId"].Value) || MessageBox.Show("The Pack UOM of the selected item in the order does not match the current selected Pack UOM. Are you sure you want to Update this record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.No))
			{
				int BrandId = Conversion.ToInt(CmbBrandItemName.Value);
				string BrandName = "";
				string BrandCode = "";
				if (((UltraGridBase)CmbBrandItemName).ActiveRow != null && BrandId > 0)
				{
					BrandCode = Conversion.ToString(((UltraDropDownBase)CmbBrandItemName).SelectedRow.Cells["ItemCode"].Value);
					BrandName = Conversion.ToString(((UltraDropDownBase)CmbBrandItemName).SelectedRow.Cells["ItemName"].Value);
				}
				dtdetail.Rows[updateDetailIndex]["SaleOrderDetailId"] = ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["SoDetailId"].Value;
				dtdetail.Rows[updateDetailIndex]["OrderId"] = CmbOrderNo.Value;
				dtdetail.Rows[updateDetailIndex]["OrderNo"] = ((Control)(object)CmbOrderNo).Text;
				dtdetail.Rows[updateDetailIndex]["WareHouse"] = CmbWareHouseName.Value;
				dtdetail.Rows[updateDetailIndex]["ItemId"] = CmbItemName.Value;
				dtdetail.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value);
				dtdetail.Rows[updateDetailIndex]["Item"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value);
				dtdetail.Rows[updateDetailIndex]["BrandItemId"] = BrandId;
				dtdetail.Rows[updateDetailIndex]["BrandItemCode"] = BrandCode;
				dtdetail.Rows[updateDetailIndex]["BrandItemName"] = BrandName;
				dtdetail.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropYear).Text;
				dtdetail.Rows[updateDetailIndex]["JobId"] = CmbJobLot.Value;
				dtdetail.Rows[updateDetailIndex]["Job"] = ((Control)(object)CmbJobLot).Text;
				dtdetail.Rows[updateDetailIndex]["PackingTypeId"] = CmbPackingType.Value;
				dtdetail.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)CmbPackingType).Text;
				dtdetail.Rows[updateDetailIndex]["Qty"] = txtqty.Text;
				dtdetail.Rows[updateDetailIndex]["UOM"] = ((Control)(object)CmbPackUom).Text;
				dtdetail.Rows[updateDetailIndex]["PUomEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				dtdetail.Rows[updateDetailIndex]["UOMId"] = CmbPackUom.Value;
				dtdetail.Rows[updateDetailIndex]["GrossWight"] = txtgwt.Text;
				dtdetail.Rows[updateDetailIndex]["EbUnit"] = txtebu.Text;
				dtdetail.Rows[updateDetailIndex]["EbTotal"] = txtebt.Text;
				dtdetail.Rows[updateDetailIndex]["WtCut"] = txtwtcut.Text;
				dtdetail.Rows[updateDetailIndex]["WtCutTotal"] = txtwtcuttotal.Text;
				dtdetail.Rows[updateDetailIndex]["AddLesswt"] = txtadlswt.Text;
				dtdetail.Rows[updateDetailIndex]["NetWeight"] = txtnetwt.Text;
				dtdetail.Rows[updateDetailIndex]["StockWeight"] = txtstockwt.Text;
				dtdetail.Rows[updateDetailIndex]["ContainerNo"] = txtContainerNodetail.Text.Trim();
				dtdetail.Rows[updateDetailIndex]["LabNo"] = comblabno.Text;
				dtdetail.Rows[updateDetailIndex]["CityId"] = CmbCity.Value;
				dtdetail.Rows[updateDetailIndex]["City"] = ((Control)(object)CmbCity).Text;
				dtdetail.Rows[updateDetailIndex]["WareHouseToId"] = Conversion.ToInt(CmbWareHouseTo.Value);
				Add.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
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
		//IL_0b24: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "GrossWight" && item.Cells["GrossWight"].Value.ToString() != string.Empty && item.Cells["NetWeight"].Value.ToString() != string.Empty)
				{
					double GrossWeight3 = Conversion.ToDouble((object)item.Cells["GrossWight"].Value.ToString());
					double EbTotal2 = Conversion.ToDouble((object)item.Cells["EbTotal"].Value.ToString());
					double EbTotalStock2 = Conversion.ToDouble((object)item.Cells["EbTotalStock"].Value.ToString());
					double AddLesswt6 = Conversion.ToDouble((object)item.Cells["AddLesswt"].Value.ToString());
					double diff = GrossWeight3 - EbTotal2 + AddLesswt6;
					item.Cells["NetWeight"].Value = diff;
					item.Cells["StockWeight"].Value = GrossWeight3 - EbTotalStock2 + AddLesswt6;
				}
				if (e.Column.Key == "EbUnit" || e.Column.Key == "EbUnitStock" || e.Column.Key == "Qty")
				{
					if (Conversion.ToInt(txtSaleType.Tag) != 2 && item.Cells["GrossWight"].Value.ToString() != string.Empty && item.Cells["NetWeight"].Value.ToString() != string.Empty)
					{
						double Qty3 = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
						double GrossWeight2 = Conversion.ToDouble((object)item.Cells["GrossWight"].Value.ToString());
						double Emptybags = Conversion.ToDouble((object)item.Cells["EbUnit"].Value.ToString());
						double EmptyBagsTotal = Conversion.ToDouble(item.Cells["EbTotal"].Value);
						double EmptybagsStock = Conversion.ToDouble(item.Cells["EbUnitStock"].Value);
						double EmptyBagsTotalStock = Conversion.ToDouble(item.Cells["EbTotalStock"].Value);
						double AddLesswt4 = Conversion.ToDouble((object)item.Cells["AddLesswt"].Value.ToString());
						if (e.Column.Key == "EbUnit")
						{
							EmptyBagsTotal = Qty3 * Emptybags;
							item.Cells["EbTotal"].Value = EmptyBagsTotal;
							bool_EbUnit = true;
							bool_EbTotal = false;
						}
						else if (bool_EbUnit)
						{
							EmptyBagsTotal = Emptybags * Qty3;
							item.Cells["EbTotal"].Value = EmptyBagsTotal;
						}
						else if (bool_EbTotal)
						{
							Emptybags = ((EmptyBagsTotal > 0.0) ? (EmptyBagsTotal / Qty3) : 0.0);
							item.Cells["EbUnit"].Value = Emptybags;
						}
						if (e.Column.Key == "EbUnitStock")
						{
							EmptyBagsTotalStock = Qty3 * EmptybagsStock;
							item.Cells["EbTotalStock"].Value = EmptyBagsTotalStock;
							bool_EbUnitStock = true;
							bool_EbTotalStock = false;
						}
						else if (bool_EbUnitStock)
						{
							EmptyBagsTotalStock = EmptybagsStock * Qty3;
							item.Cells["EbTotalStock"].Value = EmptyBagsTotalStock;
						}
						else if (bool_EbTotalStock)
						{
							EmptybagsStock = ((EmptyBagsTotalStock > 0.0) ? (EmptyBagsTotalStock / Qty3) : 0.0);
							item.Cells["EbUnitStock"].Value = EmptybagsStock;
						}
						double Diff3 = GrossWeight2 - EmptyBagsTotal + AddLesswt4;
						item.Cells["NetWeight"].Value = Diff3;
						item.Cells["StockWeight"].Value = GrossWeight2 - EmptyBagsTotalStock + AddLesswt4;
					}
					else if (Conversion.ToInt(txtSaleType.Tag) == 2 && EbWeightEditableForSaleTypePackWise)
					{
						double EmptyBagsTotal2 = Conversion.ToDouble(item.Cells["EbTotal"].Value);
						double EmptyBagsTotalStock2 = Conversion.ToDouble(item.Cells["EbTotalStock"].Value);
						double PUomEquivalent = Conversion.ToDouble(item.Cells["PUomEquivalent"].Value);
						double Qty4 = Conversion.ToDouble(item.Cells["Qty"].Value);
						double BalQty = Conversion.ToDouble(item.Cells["BalQty"].Value);
						double sum = Conversion.ToDouble(dtdetail.Compute("Sum(Qty)", "DoDetailId = '" + Conversion.ToString(item.Cells["DoDetailId"].Value) + "'"));
						if (sum > BalQty)
						{
							double Rem = BalQty - sum;
							Qty4 = ((Rem < 0.0) ? 0.0 : Rem);
							item.Cells["Qty"].Value = Qty4;
						}
						double Emptybags2 = Conversion.ToDouble(item.Cells["EbUnit"].Value);
						double EmptybagsStock2 = Conversion.ToDouble(item.Cells["EbUnitStock"].Value);
						double AddLesswt5 = Conversion.ToDouble(item.Cells["AddLesswt"].Value);
						double Gross = Qty4 * PUomEquivalent;
						if (e.Column.Key == "EbUnit")
						{
							EmptyBagsTotal2 = Qty4 * Emptybags2;
							item.Cells["EbTotal"].Value = EmptyBagsTotal2;
							bool_EbUnit = true;
							bool_EbTotal = false;
						}
						else if (bool_EbUnit)
						{
							EmptyBagsTotal2 = Emptybags2 * Qty4;
							item.Cells["EbTotal"].Value = EmptyBagsTotal2;
						}
						else if (bool_EbTotal)
						{
							Emptybags2 = ((EmptyBagsTotal2 > 0.0) ? (EmptyBagsTotal2 / Qty4) : 0.0);
							item.Cells["EbUnit"].Value = Emptybags2;
						}
						if (e.Column.Key == "EbUnitStock")
						{
							EmptyBagsTotalStock2 = Qty4 * EmptybagsStock2;
							item.Cells["EbTotalStock"].Value = EmptyBagsTotalStock2;
							bool_EbUnitStock = true;
							bool_EbTotalStock = false;
						}
						else if (bool_EbUnitStock)
						{
							EmptyBagsTotalStock2 = EmptybagsStock2 * Qty4;
							item.Cells["EbTotalStock"].Value = EmptyBagsTotalStock2;
						}
						else if (bool_EbTotalStock)
						{
							EmptybagsStock2 = ((EmptyBagsTotalStock2 > 0.0) ? (EmptyBagsTotalStock2 / Qty4) : 0.0);
							item.Cells["EbUnitStock"].Value = EmptybagsStock2;
						}
						item.Cells["GrossWight"].Value = Gross;
						ProportionateShortWeight();
						double Diff4 = Gross - EmptyBagsTotal2 + AddLesswt5;
						double ShortWeight3 = Conversion.ToDouble(item.Cells["ShortWeight"].Value);
						if (chkShortWeightLessFromNetWeight.Checked)
						{
							Diff4 -= ShortWeight3;
						}
						item.BeginEdit();
						item.Cells["NetWeight"].Value = Diff4;
						item.Cells["StockWeight"].Value = Gross - EmptyBagsTotalStock2 + AddLesswt5;
						item.EndEdit();
					}
				}
				if (e.Column.Key == "EbTotal" && item.Cells["GrossWight"].Value.ToString() != string.Empty && item.Cells["NetWeight"].Value.ToString() != string.Empty)
				{
					double Qty2 = Conversion.ToDouble(item.Cells["Qty"].Value);
					double num = Conversion.ToDouble((object)item.Cells["GrossWight"].Value.ToString());
					double EmptybagsTotal = Conversion.ToDouble((object)item.Cells["EbTotal"].Value.ToString());
					double AddLesswt3 = Conversion.ToDouble((object)item.Cells["AddLesswt"].Value.ToString());
					double Diff2 = num - EmptybagsTotal + AddLesswt3;
					double ShortWeight2 = Conversion.ToDouble(item.Cells["ShortWeight"].Value);
					if (Conversion.ToInt(txtSaleType.Tag) == 2 && chkShortWeightLessFromNetWeight.Checked)
					{
						Diff2 -= ShortWeight2;
					}
					item.Cells["NetWeight"].Value = Diff2;
					double EBUnit2 = 0.0;
					EBUnit2 = ((EmptybagsTotal > 0.0) ? (EmptybagsTotal / Qty2) : 0.0);
					item.Cells["EbUnit"].Value = EBUnit2;
					bool_EbTotal = true;
					bool_EbUnit = false;
				}
				if (e.Column.Key == "EbTotalStock" && item.Cells["GrossWight"].Value.ToString() != string.Empty)
				{
					double Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
					double num2 = Conversion.ToDouble(item.Cells["GrossWight"].Value);
					double EmptybagsTotalStock = Conversion.ToDouble(item.Cells["EbTotalStock"].Value);
					double AddLesswt2 = Conversion.ToDouble(item.Cells["AddLesswt"].Value);
					double Diff = num2 - EmptybagsTotalStock + AddLesswt2;
					item.Cells["StockWeight"].Value = Diff;
					double EBUnit = 0.0;
					EBUnit = ((EmptybagsTotalStock > 0.0) ? (EmptybagsTotalStock / Qty) : 0.0);
					item.Cells["EbUnitStock"].Value = EBUnit;
					bool_EbTotalStock = true;
					bool_EbUnitStock = false;
				}
				if (e.Column.Key == "AddLesswt" && item != null && (int)item.RowType == 0 && item.Cells["GrossWight"].Value.ToString() != string.Empty && item.Cells["NetWeight"].Value.ToString() != string.Empty)
				{
					double GrossWeight = Conversion.ToDouble(item.Cells["GrossWight"].Value);
					double EbTotal = Conversion.ToDouble(item.Cells["EbTotal"].Value);
					double EbTotalStock = Conversion.ToDouble(item.Cells["EbTotalStock"].Value);
					double AddLesswt = Conversion.ToDouble(item.Cells["AddLesswt"].Value);
					double NetWeight = GrossWeight - EbTotal + AddLesswt;
					double ShortWeight = Conversion.ToDouble(item.Cells["ShortWeight"].Value);
					if (Conversion.ToInt(txtSaleType.Tag) == 2 && chkShortWeightLessFromNetWeight.Checked)
					{
						NetWeight -= ShortWeight;
					}
					item.Cells["NetWeight"].Value = NetWeight;
					item.Cells["StockWeight"].Value = GrossWeight - EbTotalStock + AddLesswt;
				}
				if (e.Column.Key == "City")
				{
					int CityId = Conversion.ToInt(item.Cells["City"].Value);
					item.Cells["CityId"].Value = CityId;
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

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (updateDetailIndex != -1)
				{
					throw new Exception("Reset Detail firts...");
				}
				dtdetail.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			if (e.Column.Key == "Add")
			{
				if (Conversion.ToInt(txtSaleType.Tag) == 2)
				{
					double BalQty = Conversion.ToDouble(r.Cells["BalQty"].Value);
					double sum = Conversion.ToDouble(dtdetail.Compute("Sum(Qty)", "DoDetailId = '" + Conversion.ToString(r.Cells["DoDetailId"].Value) + "'"));
					double Rem = BalQty - sum;
					if (Rem > 0.0)
					{
						DataRow dr = dtdetail.Rows[r.RowIndex];
						dtdetail.Rows.Add(dr.ItemArray);
						if (btnUpdate.Visible && btnUpdate.Enabled)
						{
							dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
						}
						dtdetail.Rows[dtdetail.Rows.Count - 1]["Qty"] = Rem;
					}
					else
					{
						MessageBox.Show("Can't break further bcoz Total Qty is Equal To BalQty:" + BalQty.ToString("#,##0.###") + ".\n If you want to break further first break any row of same Do Detail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
				else
				{
					DataRow dr2 = dtdetail.Rows[r.RowIndex];
					dtdetail.Rows.Add(dr2.ItemArray);
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
					}
				}
			}
			if (e.Column.Key == "Loader")
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
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

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grd.CurrentRow;
			if (row == null || (int)row.RowType != 0 || !(e.Column.Key == "AvailableStock") || !(Conversion.ToDouble(row.Cells["AvailableStock"].Value) > 0.0))
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
					frmStockReport.ItemId = Conversion.ToInt(row.Cells["ItemId"].Value);
					frmStockReport.WarehouseId = Conversion.ToInt(row.Cells["WareHouse"].Value);
					frmStockReport.CropYear = Conversion.ToString((object)row.Cells["CropYear"].Text);
					frmStockReport.JobLotId = Conversion.ToInt(row.Cells["JobId"].Value);
					frmStockReport.PackingTypeId = Conversion.ToInt(row.Cells["PackingTypeId"].Value);
					frmStockReport.UomId = Conversion.ToInt(row.Cells["UOMId"].Value);
					frmStockReport.ShowDialog();
				}
				else
				{
					MessageBox.Show("You dont have right");
				}
			}
			else
			{
				MessageBox.Show("You dont have right");
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
					if (Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]) == Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]) == Conversion.ToInt(item.Cells["RefDocIdNo"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]) == Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					dtdetail.Rows.Add(0, Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]), Conversion.ToInt(r.Cells["SaleOrderDetailId"].Value), Conversion.ToInt(r.Cells["OrderId"].Value), Conversion.ToString(r.Cells["OrderNo"].Value), Conversion.ToInt(r.Cells["RefPartyId"].Value), Conversion.ToInt(r.Cells["WareHouse"].Value), Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["ItemCode"].Value), Conversion.ToString(r.Cells["Item"].Value), Conversion.ToInt(r.Cells["BrandItemId"].Value), Conversion.ToString(r.Cells["BrandItemCode"].Value), Conversion.ToString(r.Cells["BrandItemName"].Value), Conversion.ToString(r.Cells["CropYear"].Value), Conversion.ToInt(r.Cells["JobId"].Value), Conversion.ToString(r.Cells["Job"].Value), Conversion.ToInt(r.Cells["PackingTypeId"].Value), Conversion.ToString(r.Cells["PackingType"].Value), Conversion.ToString(r.Cells["UOM"].Value), Conversion.ToDouble(r.Cells["PUomEquivalent"].Value), Conversion.ToInt(r.Cells["UOMId"].Value), Conversion.ToDouble(r.Cells["Qty"].Value), Conversion.ToDouble(r.Cells["GrossWight"].Value), Conversion.ToDouble(r.Cells["EbUnit"].Value), Conversion.ToDouble(r.Cells["EbTotal"].Value), Conversion.ToDouble(r.Cells["WtCut"].Value), Conversion.ToDouble(r.Cells["WtCutTotal"].Value), Conversion.ToDouble(r.Cells["AddLesswt"].Value), Conversion.ToDouble(r.Cells["ShortWeight"].Value), Conversion.ToDouble(r.Cells["GrossWight"].Value) - Conversion.ToDouble(r.Cells["EbTotal"].Value) - Conversion.ToDouble(r.Cells["WtCutTotal"].Value) + Conversion.ToDouble(r.Cells["AddLesswt"].Value), Conversion.ToDouble(r.Cells["EbUnitStock"].Value), Conversion.ToDouble(r.Cells["EbTotalStock"].Value), Conversion.ToDouble(r.Cells["GrossWight"].Value) - Conversion.ToDouble(r.Cells["EbTotalStock"].Value) + Conversion.ToDouble(r.Cells["AddLesswt"].Value), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["AVgRate"]), Conversion.ToString(r.Cells["ContainerNo"].Value), Conversion.ToString(r.Cells["LabNo"].Value), Conversion.ToInt(r.Cells["CityId"].Value), Conversion.ToString(r.Cells["City"].Value), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["WeightBalance"]), Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["QtyBalance"]), Conversion.ToInt(r.Cells["DoId"].Value), Conversion.ToInt(r.Cells["DoDetailId"].Value), Conversion.ToString(dtpurchaseinvoice.Rows[i]["RefDocumentType"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["DocCodeNo"]), Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RateUomId"]), Conversion.ToInt(r.Cells["WareHouse"].Value));
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

	private void GetExpenseDataFromDo(string doIds)
	{
		try
		{
			dtExpense.Rows.Clear();
			DataTable dt = GatePassOutward.DeliveryOrderExpensesByDoIds(doIds);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtExpense.Rows.Add(dt.Rows[i]["ItemId"], dt.Rows[i]["Qty"], dt.Rows[i]["Remarks"]);
				}
				GridExpense.DataSource = dtExpense;
			}
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
			return GatePassOutward.GetByPendingGatepass(new GatePassOutward
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
			if (dt.Rows.Count > 0)
			{
				grdGp.DataSource = dt;
				grdGp.RetrieveStructure();
				GRNGridSetting();
			}
			else
			{
				grdGp.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GatePassRecordFillWinthoutSaleorder()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			DataTable dtgrpsrno = GatePassOutward.ReadByGpNoForGdn(new GatePassOutward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = GPID
			});
			if (dtgrpsrno.Rows.Count > 0)
			{
				CmbGpNo.Enabled = false;
				GPID = Conversion.ToInt(dtgrpsrno.Rows[0]["Id"]);
				txtvehno.Text = dtgrpsrno.Rows[0]["VehicleNo"].ToString();
				txtbltyno.Text = dtgrpsrno.Rows[0]["BiltyNo"].ToString();
				txtremarks.Text = dtgrpsrno.Rows[0]["OtherRemarks"].ToString();
				GpType = dtgrpsrno.Rows[0]["GatepassType"].ToString();
				txtcarramount.Text = dtgrpsrno.Rows[0]["NetPaid"].ToString();
				txtqty.Text = dtgrpsrno.Rows[0]["NoOfPackages"].ToString();
				txtsuppwt.Text = dtgrpsrno.Rows[0]["SupplierWeight"].ToString();
				txtfctwt.Text = dtgrpsrno.Rows[0]["FactoryWeight"].ToString();
				txtwtdiff.Text = dtgrpsrno.Rows[0]["DifferenceWeight"].ToString();
				((Control)(object)CmbWareHouseName).Text = dtgrpsrno.Rows[0]["WareHouseName"].ToString();
				txtContainerNo.Text = dtgrpsrno.Rows[0]["Container"].ToString();
				txtContainerNo1.Text = dtgrpsrno.Rows[0]["Container1"].ToString();
				DataTable dt = new DataTable();
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("CompanyName");
				dt.Clear();
				for (int i = 0; i < dtgrpsrno.Rows.Count; i++)
				{
					dt.Rows.Add(dtgrpsrno.Rows[i]["SupplierCustomerId"], dtgrpsrno.Rows[i]["CompanyName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbCustomerName, "SupplierCustomerId", "CompanyName", "Supplier Name", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassRecordFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		try
		{
			DataTable dtSrNoGet = GatePassOutward.ReadByGpNoForGdn(new GatePassOutward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = GPID
			});
			if (dtSrNoGet.Rows.Count <= 0)
			{
				return;
			}
			CmbGpNo.Enabled = false;
			txtSaleType.Text = "Weigh Bridge Based Billing";
			txtSaleType.Tag = 1;
			GPID = Conversion.ToInt(dtSrNoGet.Rows[0]["Id"]);
			DataTable lstWb = WbTransactions.GetNetWeightFromWbTransactions(new WbTransactions
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ReferenceDocTypeId = 91,
				ReferenceDocNoId = GPID
			});
			if (lstWb.Rows.Count <= 0)
			{
				return;
			}
			double NetWeight = 0.0;
			for (int j = 0; j < lstWb.Rows.Count; j++)
			{
				NetWeight += Convert.ToDouble(lstWb.Rows[j]["NetWbWeight"]);
			}
			txtvehno.Text = dtSrNoGet.Rows[0]["VehicleNo"].ToString();
			txtbltyno.Text = dtSrNoGet.Rows[0]["BiltyNo"].ToString();
			txtremarks.Text = dtSrNoGet.Rows[0]["OtherRemarks"].ToString();
			GpType = dtSrNoGet.Rows[0]["GatepassType"].ToString();
			txtcarramount.Text = dtSrNoGet.Rows[0]["NetPaid"].ToString();
			txtqty.Text = dtSrNoGet.Rows[0]["NoOfPackages"].ToString();
			txtfctwt.Text = NetWeight.ToString();
			txtContainerNo.Text = dtSrNoGet.Rows[0]["Container"].ToString();
			txtContainerNo1.Text = dtSrNoGet.Rows[0]["Container1"].ToString();
			if (Conversion.ToInt(dtSrNoGet.Rows[0]["SaleOrderId"]) > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("SaleOrderId");
				dt.Columns.Add("SupplierContractCode");
				dt.Clear();
				for (int i = 0; i < dtSrNoGet.Rows.Count; i++)
				{
					dt.Rows.Add(dtSrNoGet.Rows[i]["SaleOrderId"], dtSrNoGet.Rows[i]["SupplierContractCode"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbOrderNo, "SaleOrderId", "SupplierContractCode", "Order No", ZeroIndex: false);
					if (Conversion.ToInt(CmbOrderNo.Value) == 0)
					{
						((UltraGridBase)CmbOrderNo).Rows[0].Activate();
						comborderno_Leave(null, null);
					}
				}
				return;
			}
			throw new Exception("Sale Order Id not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GRNGridSetting()
	{
		try
		{
			grdGp.RootTable.Columns["OutwardGatePassId"].Visible = false;
			grdGp.RootTable.Columns["GatepassType"].Visible = false;
			grdGp.RootTable.Columns["SupplierContractCode"].Visible = false;
			grdGp.RootTable.Columns["FlowValue"].Visible = false;
			grdGp.RootTable.Columns["SupplierWeight"].Visible = false;
			grdGp.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdGp, 2, 3, null, null, null, ColumnSetting: false);
			grdGp.RootTable.Columns["GpSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdGp.RootTable.Columns["OrderType"].Width = Constants.InventoryConstants.CastingType + 20;
			grdGp.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdGp.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grdGp.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdGp.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grdGp.RootTable.Columns["VarietyName"].Width = Constants.InventoryConstants.ItemName + 20;
			grdGp.RootTable.Columns["NoOfPackages"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 25;
			grdGp.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["DifferenceWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdGp.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant;
			grdGp.RootTable.Columns["FlowDescripton"].Width = Constants.InventoryConstants.StringConstant - 10;
			grdGp.RootTable.Columns["NoOfPackages"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["NoOfPackages"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["NoOfPackages"].FormatString = "0,0";
			grdGp.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["FactoryWeight"].FormatString = "0,0";
			grdGp.RootTable.Columns["FactoryWeight"].TotalFormatString = "0,0";
			grdGp.RootTable.Columns["DifferenceWeight"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["DifferenceWeight"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["DifferenceWeight"].FormatString = "0,0";
			grdGp.RootTable.Columns["DifferenceWeight"].TotalFormatString = "0,0";
			grdGp.RootTable.Columns.Add("Load");
			grdGp.RootTable.Columns["Load"].Caption = "Load";
			grdGp.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGp.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
			grdGp.RootTable.Columns["Load"].ButtonText = "Load";
			grdGp.RootTable.Columns["Load"].Width = 40;
			grdGp.RootTable.Columns["Load"].Position = 0;
			grdGp.RootTable.Columns["GpSrNo"].Position = 1;
			grdGp.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void combgatepass_Leave(object sender, EventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Expected O, but got Unknown
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Expected O, but got Unknown
		try
		{
			if (!btnSave.Visible || !btnSave.Enabled)
			{
				return;
			}
			DataTable dtSrNoGet = GatePassOutward.ReadByGpNoForGdnDeliveryOrder(new GatePassOutward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = GPID
			});
			if (dtSrNoGet.Rows.Count > 0)
			{
				CmbGpNo.Enabled = false;
				GPID = Conversion.ToInt(dtSrNoGet.Rows[0]["Id"]);
				SoOrderId = Conversion.ToInt(dtSrNoGet.Rows[0]["SaleOrderId"]);
				OrderStatus = Conversion.ToString(dtSrNoGet.Rows[0]["OrderStatus"]);
				txtvehno.Text = Conversion.ToString(dtSrNoGet.Rows[0]["VehicleNo"]);
				txtbltyno.Text = Conversion.ToString(dtSrNoGet.Rows[0]["BiltyNo"]);
				txtfctwt.Text = Conversion.ToString(dtSrNoGet.Rows[0]["FactoryWeight"]);
				txtcarramount.Text = Conversion.ToString(dtSrNoGet.Rows[0]["NetPaid"]);
				txtDeliverTerm.Text = OrderStatus;
				DeliveryOrderType = Conversion.ToString(dtSrNoGet.Rows[0]["DeliveryOrderType"]);
				txtTicketNos.Text = Conversion.ToString(dtSrNoGet.Rows[0]["TicketNos"]);
				txtTotalDoWeight.Text = Conversion.ToDouble(dtSrNoGet.Rows[0]["TotalDoWeight"]).ToString("#,##0.####");
				txtShortExcessWeight.Text = Conversion.ToDouble(dtSrNoGet.Rows[0]["ShortExcessWeight"]).ToString("#,##0.####");
				if (Conversion.ToInt(dtSrNoGet.Rows[0]["SaleTypeId"]) == 2)
				{
					txtSaleType.Text = "Pack Size Based Billing";
					txtSaleType.Tag = 2;
				}
				else
				{
					txtSaleType.Text = "Weigh Bridge Based Billing";
					txtSaleType.Tag = 1;
				}
				if (OrderStatus == "DeliverOrder")
				{
					string DoIds = "";
					DoIds = SoOrderId.ToString();
					panel5.Dock = DockStyle.None;
					txtDeliverTerm.Size = new Size(120, 23);
					DataTable dtSupplier = InvDeliveryOrder.CustomerLoadForDeliveryOrderId(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						Id = GPID
					});
					if (dtSupplier.Rows.Count > 0)
					{
						int count = dtSupplier.Rows.Count;
						txtPartyCount.Text = count.ToString();
						DDL.BindDDL(dtSupplier, CmbCustomerName, "SupplierCustomerId", "CompanyName", "CustomerName", ZeroIndex: false);
						if (count == 0)
						{
							((UltraGridBase)CmbCustomerName).Rows[0].Activate();
						}
					}
					double GdnGrossWeight = InvGdn.GetGdnGrossWeightForValidation(new InvGdn
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 86,
						Id = Conversion.ToInt((object)GPID)
					});
					if (GdnGrossWeight > 0.0)
					{
						txtGdnGrossWeight.Text = GdnGrossWeight.ToString();
					}
					else
					{
						txtGdnGrossWeight.Text = "0";
					}
					Conversion.ToDouble((object)txtGdnGrossWeight.Text.Trim());
					double FinalWt = Conversion.ToDouble((object)txtfctwt.Text.Trim()) - GdnGrossWeight;
					txtBalanceWeight.Text = FinalWt.ToString();
					DeliverOrderRecordFill();
					if (Conversion.ToDouble((object)txtShortExcessWeight.Text) != 0.0)
					{
						if (Conversion.ToDouble((object)txtShortExcessWeight.Text) < 0.0 && Conversion.ToInt(txtSaleType.Tag) == 2)
						{
							chkShortWeightLessFromNetWeight.Visible = true;
							label60.Text = "Short Weight";
							ProportionateShortWeight();
						}
						else if (Conversion.ToDouble((object)txtShortExcessWeight.Text) > 0.0)
						{
							label60.Text = "Excess Weight";
							chkShortWeightLessFromNetWeight.Visible = false;
						}
						else
						{
							label60.Text = "Short/Excess Wt";
							chkShortWeightLessFromNetWeight.Visible = false;
						}
					}
					GetExpenseDataFromDo(DoIds);
					if (dtExpense != null && dtExpense.Rows.Count == 0)
					{
						AddRowInvExpGrid();
					}
					GridExpense.RetrieveStructure();
					grdInvExpSettings();
					if (IsStockReservedPerParty)
					{
						ChkIsStockReservedPerParty.Checked = Conversion.ToBool(dtSrNoGet.Rows[0]["IsStockReserved"]);
					}
				}
				else
				{
					txtBalanceWeight.Text = "0";
					txtPartyCount.Text = "0";
					panel5.Dock = DockStyle.Top;
					txtDeliverTerm.Size = new Size(215, 23);
				}
			}
			else
			{
				GatePassRecordFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProportionateShortWeight()
	{
		try
		{
			grd.UpdateData();
			if (grd.GetRows().Count() > 0)
			{
				double TotalDoWeiht = Conversion.ToDouble((object)txtTotalDoWeight.Text);
				double ShortWeight = Math.Abs(Conversion.ToDouble((object)txtShortExcessWeight.Text));
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					double qty = Conversion.ToDouble(obj.Cells["Qty"].Value);
					double PackEquivalent = Conversion.ToDouble(obj.Cells["PUomEquivalent"].Value);
					double value = ShortWeight / TotalDoWeiht * (qty * PackEquivalent);
					obj.BeginEdit();
					obj.Cells["ShortWeight"].Value = value;
					obj.EndEdit();
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (btnSave.Visible && btnSave.Enabled && e.Column.Key == "Load")
			{
				LoadGpRow();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadGpRow(int GpId = 0)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = ((GpId == 0) ? grdGp.CurrentRow : GridEX_Helper.GetRowByColumnNameAndValue(grdGp, "Id", GpId));
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (GPID == 0)
			{
				GPID = 0;
				string GpStatus = Conversion.ToString(r.Cells["Status"].Value);
				int num = Conversion.ToInt(r.Cells["FlowValue"].Value);
				double FactoryWeight = Conversion.ToInt(r.Cells["FactoryWeight"].Value);
				int SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				if (num == 1)
				{
					if (FactoryWeight == 0.0)
					{
						throw new Exception("Weigh Bridge against this Gp is Not done Yet.Please Make Sure Weigh Bridge entry First");
					}
					GPID = Conversion.ToInt(r.Cells["OutwardGatePassId"].Value);
					CmbGpNo.Text = Conversion.ToString(r.Cells["GpSrNo"].Value);
					combgatepass_Leave(null, null);
					CmbCustomerName.Focus();
				}
				else
				{
					if (!(GpStatus == "Accepted"))
					{
						throw new Exception("Status Not Accepted Please check status");
					}
					GPID = Conversion.ToInt(r.Cells["OutwardGatePassId"].Value);
					CmbGpNo.Text = Conversion.ToString(r.Cells["GpSrNo"].Value);
					combgatepass_Leave(null, null);
					CmbCustomerName.Focus();
				}
				CmbCustomerName.Value = SupplierCustomerId;
				if (IsStockReservedPerParty && ChkIsStockReservedPerParty.Checked)
				{
					AdvanceDoBind();
				}
				combsupplier_Leave(null, null);
				return;
			}
			throw new Exception("Another Gp Is Already Loaded...Please Reset The Form First..");
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
				int refDocIdNo2 = Conversion.ToInt(r2.Cells["RefDocIdNo"].Value);
				int refDocSubIdNo2 = Conversion.ToInt(r2.Cells["RefDocSubIdNo"].Value);
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
				int refDocIdNo = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
				int refDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
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

	private bool AutoUpdateRecord(int UpdateRecID)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Expected O, but got Unknown
		//IL_0c88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8f: Expected O, but got Unknown
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
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return false;
			}
			GRN.Id = UpdateRecID;
			GRN.AutoUpdateId = 1;
			if (Conversion.ToDouble((object)txtcarramount.Text.Trim()) > 0.0)
			{
				if (((UltraGridBase)CmbTransport).ActiveRow == null)
				{
					MessageBox.Show("Transporter Account field required");
					CmbTransport.Focus();
					return false;
				}
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					GRN.TransporterId = Conversion.ToInt(CmbTransport.Value);
					GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
					GRN.TransporterSupCustId = 0;
				}
				else
				{
					GRN.TransporterId = Conversion.ToInt(((UltraDropDownBase)CmbTransport).SelectedRow.Cells[3].Value);
					GRN.TransporterSupCustId = Conversion.ToInt(CmbTransport.Value);
					GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
				}
			}
			GRN.OrganizationId = UserAccount.OrganizationId;
			GRN.CompanyId = UserAccount.CompanyId;
			GRN.BranchesId = UserAccount.BranchesId;
			GRN.ProjectsId = UserAccount.BranchesId;
			GRN.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			GRN.DocumentTypeId = 86;
			GRN.EntryUser = UserAccount.ID;
			GRN.ModifyUser = UserAccount.ID;
			GRN.EntryDate = DateTime.Now;
			GRN.ModifyDate = DateTime.Now;
			GRN.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			GRN.DocDate = DocDate.Value;
			GRN.SupplierCustomerId = Conversion.ToInt(CmbCustomerName.Value);
			GRN.StockPartyId = Conversion.ToInt(combstockprty.Value);
			GRN.TransporterDocRef = OrderStatus;
			GRN.DeliveryTerm = Conversion.ToString((object)txtDeliverTerm.Text);
			GRN.GpNo = Conversion.ToInt((object)CmbGpNo.Text);
			GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
			GRN.VehicleNo = Conversion.ToString((object)txtvehno.Text.Trim());
			GRN.BiltyNo = txtbltyno.Text.Trim();
			GRN.ContainerNo = txtContainerNo.Text.Trim();
			GRN.ContainerNo1 = txtContainerNo1.Text.Trim();
			GRN.SupplierReference = txtsuppref.Text.Trim();
			GRN.ReferenceDocNo = txtrefno.Text.Trim();
			GRN.FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			GRN.PartyWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
			GRN.RemarksHeader = txtremarks.Text.Trim();
			GRN.OutwardGatePassId = Conversion.ToInt((object)GPID);
			GRN.IsApproved = false;
			GRN.ScreenName = Conversion.ToString((object)base.Name);
			GRN.ShortWeightDeductionApply = Conversion.ToBool((object)chkShortWeightLessFromNetWeight.Checked);
			GRN.invGdnDetail = new List<InvGdnDetail>();
			GRN.InvGdnExpenseslist = new List<InvGdnExpense>();
			GRN.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			double GrossWt = 0.0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvGdnDetail grnDetail = new InvGdnDetail();
				grnDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (Conversion.ToInt(r.Cells["OrderId"].Value) > 0)
				{
					grnDetail.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					grnDetail.SaleOrderDetailId = Conversion.ToInt(r.Cells["SaleOrderDetailId"].Value);
					if (Conversion.ToInt(r.Cells["RefPartyId"].Value) > 0)
					{
						grnDetail.RefPartyId = Conversion.ToInt(r.Cells["RefPartyId"].Value);
					}
					if (Conversion.ToInt(r.Cells["ItemId"].Value) > 0)
					{
						grnDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						if (Conversion.ToString(r.Cells["CropYear"].Value) != string.Empty)
						{
							grnDetail.CropYear = Conversion.ToString((object)r.Cells["CropYear"].Text);
							if (Conversion.ToInt(r.Cells["JobId"].Value) > 0)
							{
								grnDetail.JobLotId = Conversion.ToInt(r.Cells["JobId"].Value);
								if (Conversion.ToInt(r.Cells["PackingTypeId"].Value) > 0)
								{
									grnDetail.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
									if (Conversion.ToDouble(r.Cells["Qty"].Value) > 0.0)
									{
										grnDetail.ItemQty = Conversion.ToDouble(r.Cells["Qty"].Value);
										if (Conversion.ToDouble(r.Cells["GrossWight"].Value) > 0.0)
										{
											grnDetail.GrossWeight = Conversion.ToDouble(r.Cells["GrossWight"].Value);
											GrossWt += grnDetail.GrossWeight;
											if (Conversion.ToInt(r.Cells["UOMId"].Value) > 0)
											{
												grnDetail.ItemUomId = Conversion.ToInt(r.Cells["UOMId"].Value);
												if (Conversion.ToDouble(r.Cells["NetWeight"].Value) > 0.0)
												{
													grnDetail.NetBillWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
													if (Conversion.ToDouble(r.Cells["StockWeight"].Value) > 0.0)
													{
														grnDetail.StockWeight = Conversion.ToDouble(r.Cells["StockWeight"].Value);
														grnDetail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
														grnDetail.RefDocIdNo = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
														grnDetail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
														grnDetail.EBWPerUnit = Conversion.ToDouble(r.Cells["EbUnit"].Value);
														grnDetail.EBWTotal = Conversion.ToDouble(r.Cells["EbTotal"].Value);
														grnDetail.WtCut = Conversion.ToDouble(r.Cells["WtCut"].Value);
														grnDetail.WtCutTotal = Conversion.ToDouble(r.Cells["WtCutTotal"].Value);
														grnDetail.AdLsWeight = Conversion.ToDouble(r.Cells["AddLesswt"].Value);
														grnDetail.GpDate = DateTime.Now;
														if (Conversion.ToInt(r.Cells["WareHouse"].Value) > 0)
														{
															grnDetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouse"].Value);
															if (Conversion.ToString((object)r.Cells["City"].Text) != string.Empty && Conversion.ToString((object)r.Cells["City"].Text) != null)
															{
																grnDetail.AreaCity = Conversion.ToString((object)r.Cells["City"].Text);
																grnDetail.CityId = Conversion.ToInt((object)r.Cells["CityId"].Text);
																grnDetail.ContainerNo = Conversion.ToString((object)r.Cells["ContainerNo"].Text);
																grnDetail.LabReportRef = Conversion.ToString((object)r.Cells["LabNo"].Text);
																grnDetail.ItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
																GRN.invGdnDetail.Add(grnDetail);
																continue;
															}
															MessageBox.Show("City Field Required");
															return false;
														}
														MessageBox.Show("WareHouse Field Required");
														return false;
													}
													MessageBox.Show("StockWeight Field Required");
													return false;
												}
												MessageBox.Show("NetWeight Field Required");
												return false;
											}
											throw new Exception("PackUom Field Required");
										}
										MessageBox.Show("GrossWight Field Required");
										return false;
									}
									throw new Exception("Item Qty Field Required");
								}
								MessageBox.Show("Packing Type Field Required");
								return false;
							}
							MessageBox.Show("JobLot Field Required");
							return false;
						}
						MessageBox.Show("CropYear Field Required");
						return false;
					}
					MessageBox.Show("ItemName Field Required");
					return false;
				}
				MessageBox.Show("OrderNo Field Required");
				return false;
			}
			if (btnSave.Visible && btnSave.Enabled)
			{
				if (txtDeliverTerm.Text == "DeliverOrder")
				{
					int PartyCount = Conversion.ToInt((object)txtPartyCount.Text.Trim());
					double GdnGrossWeight = Conversion.ToDouble((object)txtGdnGrossWeight.Text.Trim());
					double FinalWt = Conversion.ToDouble((object)txtfctwt.Text.Trim()) - GdnGrossWeight;
					if (PartyCount == 1)
					{
						if (!MultipleGdnsAgainstSingleParty)
						{
							if (GrossWt != FinalWt && Conversion.ToInt(txtSaleType.Tag) != 2)
							{
								throw new Exception($"Mismatch in weights: Gross weight entered is {GrossWt}, but it should be equal to the final weight {FinalWt} for sale type '{txtSaleType.Text}'.");
							}
						}
						else if (GrossWt > FinalWt)
						{
							throw new Exception($"Invalid weight entry: Total gross weight ({GrossWt}) cannot be greater than the balance weight ({FinalWt}).");
						}
					}
					if (PartyCount > 1 && GrossWt >= FinalWt)
					{
						throw new Exception($"Invalid weight entry: Gross weight ({GrossWt}) cannot be greater than or equal to balance weight ({FinalWt}) as there are still {PartyCount} parties remaining.");
					}
				}
				else
				{
					double FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
					if (GrossWt != FactoryWeight)
					{
						throw new Exception($"Mismatch in weights: Gross weight entered is {GrossWt}, but it should be equal to the factory weight {FactoryWeight} '.");
					}
				}
			}
			else if (txtDeliverTerm.Text == "DeliverOrder")
			{
				int num = Conversion.ToInt((object)txtPartyCount.Text.Trim());
				double GdnGrossWeight2 = Conversion.ToDouble((object)txtGdnGrossWeight.Text.Trim());
				double FinalWt2 = Conversion.ToDouble((object)txtfctwt.Text.Trim()) - GdnGrossWeight2;
				if (num == 1 && GrossWt != FinalWt2 && Conversion.ToInt(txtSaleType.Tag) != 2)
				{
					throw new Exception($"Mismatch in weights: Gross weight entered is {GrossWt}, but it should be equal to the final weight {FinalWt2} for sale type '{txtSaleType.Text}'.");
				}
			}
			rows = GridExpense.GetRows();
			foreach (GridEXRow item in rows)
			{
				InvGdnExpense obj = new InvGdnExpense();
				obj.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				obj.Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				obj.Remarks = Conversion.ToString(item.Cells["Remarks"].Value);
				GRN.InvGdnExpenseslist.Add(obj);
			}
			InvGdn.Save(GRN);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_1370: Unknown result type (might be due to invalid IL or missing references)
		//IL_1377: Expected O, but got Unknown
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
			InvGdn GRN = new InvGdn();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GRN.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["ShortWeight"], (AggregateFunction)2)) > 0.0 && !chkShortWeightLessFromNetWeight.Checked && MessageBox.Show("You have not Apply ShortWeight Deduction In bill Weight...Are you sure to Continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (Conversion.ToDouble((object)txtcarramount.Text.Trim()) > 0.0)
			{
				if (((UltraGridBase)CmbTransport).ActiveRow == null)
				{
					MessageBox.Show("Transporter Account field required");
					CmbTransport.Focus();
					return;
				}
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					GRN.TransporterId = Conversion.ToInt(CmbTransport.Value);
					GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
					GRN.TransporterSupCustId = 0;
				}
				else
				{
					GRN.TransporterId = Conversion.ToInt(((UltraDropDownBase)CmbTransport).SelectedRow.Cells[3].Value);
					GRN.TransporterSupCustId = Conversion.ToInt(CmbTransport.Value);
					GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
				}
			}
			ValidateDetailRecordsWithorWithoutLoader();
			GRN.OrganizationId = UserAccount.OrganizationId;
			GRN.CompanyId = UserAccount.CompanyId;
			GRN.BranchesId = UserAccount.BranchesId;
			GRN.ProjectsId = UserAccount.BranchesId;
			GRN.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			GRN.DocumentTypeId = 86;
			GRN.EntryUser = UserAccount.ID;
			GRN.ModifyUser = UserAccount.ID;
			GRN.EntryDate = DateTime.Now;
			GRN.ModifyDate = DateTime.Now;
			GRN.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			GRN.DocDate = DocDate.Value;
			GRN.SupplierCustomerId = Conversion.ToInt(CmbCustomerName.Value);
			GRN.StockPartyId = Conversion.ToInt(combstockprty.Value);
			GRN.TransporterDocRef = OrderStatus;
			GRN.DeliveryTerm = Conversion.ToString((object)txtDeliverTerm.Text);
			GRN.GpNo = Conversion.ToInt((object)CmbGpNo.Text);
			GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
			GRN.VehicleNo = Conversion.ToString((object)txtvehno.Text.Trim());
			GRN.BiltyNo = txtbltyno.Text.Trim();
			GRN.ContainerNo = txtContainerNo.Text.Trim();
			GRN.ContainerNo1 = txtContainerNo1.Text.Trim();
			GRN.SupplierReference = txtsuppref.Text.Trim();
			GRN.ReferenceDocNo = txtrefno.Text.Trim();
			GRN.FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			GRN.PartyWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
			GRN.RemarksHeader = txtremarks.Text.Trim();
			GRN.OutwardGatePassId = Conversion.ToInt((object)GPID);
			GRN.IsApproved = false;
			if (ChkIsStockReservedPerParty.Visible)
			{
				GRN.IsStockReserved = Conversion.ToBool((object)ChkIsStockReservedPerParty.Checked);
				GRN.AdvanceDeliveryOrderId = Conversion.ToInt(CmbAdvanceDo.Value);
			}
			GRN.ShortWeightDeductionApply = Conversion.ToBool((object)chkShortWeightLessFromNetWeight.Checked);
			GRN.ScreenName = Conversion.ToString((object)base.Name);
			GRN.invGdnDetail = new List<InvGdnDetail>();
			GRN.InvGdnExpenseslist = new List<InvGdnExpense>();
			double GrossWt = 0.0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				double GrossWeight = Conversion.ToDouble(r.Cells["GrossWight"].Value);
				double PUom = Conversion.ToDouble(r.Cells["PUomEquivalent"].Value);
				double Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
				double QtyMargin = 0.0;
				if (Conversion.ToInt(txtSaleType.Tag) != 2)
				{
					double num = GrossWeight / PUom;
					QtyMargin = num * 5.0 / 100.0;
					double minAllowed = num - QtyMargin;
					double maxAllowed = num + QtyMargin;
					if ((Qty < minAllowed || Qty > maxAllowed) && MessageBox.Show("Entered Qty is outside the allowed range.\n" + $"Allowed Range: {minAllowed:F2} - {maxAllowed:F2}\n" + $"Entered Qty: {Qty:F2}\n\n" + "Do you want to continue?", "Confirm Quantity", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					{
						return;
					}
				}
				InvGdnDetail grnDetail = new InvGdnDetail();
				grnDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (Conversion.ToInt(r.Cells["OrderId"].Value) > 0)
				{
					grnDetail.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					grnDetail.SaleOrderDetailId = Conversion.ToInt(r.Cells["SaleOrderDetailId"].Value);
					if (Conversion.ToInt(r.Cells["RefPartyId"].Value) > 0)
					{
						grnDetail.RefPartyId = Conversion.ToInt(r.Cells["RefPartyId"].Value);
					}
					if (Conversion.ToInt(r.Cells["ItemId"].Value) > 0)
					{
						grnDetail.Item = Conversion.ToString(r.Cells["Item"].Value);
						grnDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						if (Conversion.ToString(r.Cells["CropYear"].Value) != string.Empty)
						{
							grnDetail.CropYearId = Conversion.ToInt(r.Cells["CropYear"].Value);
							grnDetail.CropYear = Conversion.ToString((object)r.Cells["CropYear"].Text);
							if (Conversion.ToInt(r.Cells["JobId"].Value) > 0)
							{
								grnDetail.JobLot = Conversion.ToString((object)r.Cells["JobId"].Text);
								grnDetail.JobLotId = Conversion.ToInt(r.Cells["JobId"].Value);
								ItemIdExistsInJobLot(grnDetail.ItemId, grnDetail.Item, grnDetail.JobLotId, grnDetail.JobLot, r.RowIndex + 1);
								if (Conversion.ToInt(r.Cells["PackingTypeId"].Value) > 0)
								{
									grnDetail.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
									if (Qty > 0.0)
									{
										grnDetail.ItemQty = Qty;
										if (Conversion.ToDouble(r.Cells["GrossWight"].Value) > 0.0)
										{
											grnDetail.GrossWeight = Conversion.ToDouble(r.Cells["GrossWight"].Value);
											GrossWt += grnDetail.GrossWeight;
											if (Conversion.ToInt(r.Cells["UOMId"].Value) > 0)
											{
												grnDetail.ItemUomId = Conversion.ToInt(r.Cells["UOMId"].Value);
												if (Conversion.ToDouble(r.Cells["NetWeight"].Value) > 0.0)
												{
													grnDetail.NetBillWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
													if (Conversion.ToDouble(r.Cells["StockWeight"].Value) > 0.0)
													{
														grnDetail.StockWeight = Conversion.ToDouble(r.Cells["StockWeight"].Value);
														grnDetail.BrandItemId = Conversion.ToInt(r.Cells["BrandItemId"].Value);
														grnDetail.BrandItemName = Conversion.ToString(r.Cells["BrandItemName"].Value);
														grnDetail.BrandItemCode = Conversion.ToString(r.Cells["BrandItemCode"].Value);
														grnDetail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
														grnDetail.RefDocIdNo = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
														grnDetail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
														grnDetail.EBWPerUnit = Conversion.ToDouble(r.Cells["EbUnit"].Value);
														grnDetail.EBWTotal = Conversion.ToDouble(r.Cells["EbTotal"].Value);
														grnDetail.EbUnitStock = Conversion.ToDouble(r.Cells["EbUnitStock"].Value);
														grnDetail.EbTotalStock = Conversion.ToDouble(r.Cells["EbTotalStock"].Value);
														int validationTypeId = 0;
														if (grnDetail.PackingTypeId == 1 || grnDetail.PackingTypeId == 2 || grnDetail.PackingTypeId == 5)
														{
															validationTypeId = grnDetail.PackingTypeId;
														}
														else
														{
															validationTypeId = 2;
														}
														InvPackingType packType = clsGlobalVariables.globalInvPackingType.Find((InvPackingType row) => row.Id == validationTypeId);
														if (packType != null && grnDetail.EBWPerUnit > 0.0)
														{
															if (grnDetail.EBWPerUnit < packType.MinEbWeight || grnDetail.EBWPerUnit > packType.MaxEbWeight)
															{
																throw new Exception($"Eb Per Unit Should be in Range of: {packType.MinEbWeight} to {packType.MaxEbWeight}\n" + "For Packing Type:" + packType.PackTypeDesc);
															}
														}
														else if (packType != null && grnDetail.EBWTotal > 0.0 && grnDetail.EBWPerUnit == 0.0)
														{
															double EbUnit2 = grnDetail.EBWTotal / grnDetail.ItemQty;
															if (EbUnit2 < packType.MinEbWeight || EbUnit2 > packType.MaxEbWeight)
															{
																throw new Exception($"Eb Per Unit Should be in Range of: {packType.MinEbWeight} to {packType.MaxEbWeight}\n" + "For Packing Type:" + packType.PackTypeDesc);
															}
														}
														if (packType != null && grnDetail.EbUnitStock > 0.0)
														{
															if (grnDetail.EbUnitStock < packType.MinEbStockWeight || grnDetail.EbUnitStock > packType.MaxEbStockWeight)
															{
																throw new Exception($"Eb Per Unit Stock Should be in Range of: {packType.MinEbStockWeight} to {packType.MaxEbStockWeight}\n" + "For Packing Type:" + packType.PackTypeDesc);
															}
														}
														else if (packType != null && grnDetail.EbTotalStock > 0.0 && grnDetail.EbUnitStock == 0.0)
														{
															double EbUnit = grnDetail.EbTotalStock / grnDetail.ItemQty;
															if (EbUnit < packType.MinEbStockWeight || EbUnit > packType.MaxEbStockWeight)
															{
																throw new Exception($"Eb Per Unit Stock Should be in Range of: {packType.MinEbStockWeight} to {packType.MaxEbStockWeight}\n" + "For Packing Type:" + packType.PackTypeDesc);
															}
														}
														grnDetail.WtCut = Conversion.ToDouble(r.Cells["WtCut"].Value);
														grnDetail.WtCutTotal = Conversion.ToDouble(r.Cells["WtCutTotal"].Value);
														grnDetail.AdLsWeight = Conversion.ToDouble(r.Cells["AddLesswt"].Value);
														grnDetail.ShortWeight = Conversion.ToDouble(r.Cells["ShortWeight"].Value);
														grnDetail.GpDate = DateTime.Now;
														if (Conversion.ToInt(r.Cells["WareHouse"].Value) > 0)
														{
															grnDetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouse"].Value);
															if (Conversion.ToString((object)r.Cells["City"].Text) != string.Empty && Conversion.ToString((object)r.Cells["City"].Text) != null)
															{
																grnDetail.AreaCity = Conversion.ToString((object)r.Cells["City"].Text);
																grnDetail.CityId = Conversion.ToInt((object)r.Cells["CityId"].Text);
																grnDetail.InvDeliveryOrderId = Conversion.ToInt(r.Cells["DoId"].Value);
																grnDetail.InvDeliveryOrderDetailId = Conversion.ToInt(r.Cells["DoDetailId"].Value);
																grnDetail.ContainerNo = Conversion.ToString((object)r.Cells["ContainerNo"].Text);
																grnDetail.LabReportRef = Conversion.ToString((object)r.Cells["LabNo"].Text);
																grnDetail.ItemRate = Conversion.ToDouble((object)r.Cells["ItemRate"].Text);
																double BalQty = Conversion.ToDouble(r.Cells["BalQty"].Value);
																if (Conversion.ToInt(txtSaleType.Tag) == 2)
																{
																	double sum = Conversion.ToDouble(dtdetail.Compute("Sum(Qty)", "DoDetailId = '" + Conversion.ToString((object)grnDetail.InvDeliveryOrderDetailId) + "'"));
																	if (sum > BalQty)
																	{
																		throw new Exception("Qty: " + sum + " Sum Of DoDetailId: " + grnDetail.InvDeliveryOrderDetailId + " Greater than BalQty: " + BalQty);
																	}
																}
																grnDetail.WareHouseToId = Conversion.ToInt(r.Cells["WareHouseToId"].Value);
																grnDetail.WareHouseToCode = Conversion.ToString((object)r.Cells["WareHouseToId"].Text);
																if (IsStockReservedPerParty && ChkIsStockReservedPerParty.Checked && grnDetail.WareHouseToId == 0)
																{
																	FormHelper.ValidateField(grnDetail.WareHouseToId, "WareHouse To", r.RowIndex);
																}
																GRN.invGdnDetail.Add(grnDetail);
																continue;
															}
															MessageBox.Show("City Field Required");
															return;
														}
														MessageBox.Show("WareHouse Field Required");
														return;
													}
													MessageBox.Show("StockWeight Field Required");
													return;
												}
												MessageBox.Show("NetWeight Field Required");
												return;
											}
											throw new Exception("PackUom Field Required");
										}
										MessageBox.Show("GrossWight Field Required");
										return;
									}
									throw new Exception("Item Qty Field Required");
								}
								MessageBox.Show("Packing Type Field Required");
								return;
							}
							MessageBox.Show("JobLot Field Required");
							return;
						}
						MessageBox.Show("CropYear Field Required");
						return;
					}
					MessageBox.Show("ItemName Field Required");
					return;
				}
				MessageBox.Show("OrderNo Field Required");
				return;
			}
			if (btnSave.Visible && btnSave.Enabled)
			{
				if (txtDeliverTerm.Text == "DeliverOrder")
				{
					int PartyCount = Conversion.ToInt((object)txtPartyCount.Text.Trim());
					double GdnGrossWeight = Conversion.ToDouble((object)txtGdnGrossWeight.Text.Trim());
					double FinalWt = Conversion.ToDouble((object)txtfctwt.Text.Trim()) - GdnGrossWeight;
					if (PartyCount == 1)
					{
						if (Conversion.ToInt(txtSaleType.Tag) == 2)
						{
							double Diff = Math.Abs(GrossWt - FinalWt);
							if (GrossWt != FinalWt && Diff >= GdnAndFactoryWeightCompareToleranceForStop)
							{
								throw new Exception($"Mismatch detected: Adjusted Gross Weight ({GrossWt}) does not match the Final Factory Weight ({FinalWt}). Tolerance Used: {GdnAndFactoryWeightCompareToleranceForStop}. Entry cannot proceed.");
							}
						}
						else if (GrossWt != FinalWt)
						{
							throw new Exception($"Mismatch detected: Total Gross Weight in Detail Grid ({GrossWt}) is not equal to Balance Weight ({FinalWt}).");
						}
					}
					if (PartyCount > 1 && GrossWt >= FinalWt)
					{
						throw new Exception($"Invalid weight entry: Gross weight ({GrossWt}) cannot be greater than or equal to balance weight ({FinalWt}) as there are still {PartyCount} parties remaining.");
					}
				}
				else
				{
					double FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
					if (GrossWt != FactoryWeight)
					{
						throw new Exception($"Mismatch in weights: Gross weight entered is {GrossWt}, but it should be equal to the factory weight {FactoryWeight} '.");
					}
				}
			}
			else if (txtDeliverTerm.Text == "DeliverOrder")
			{
				int num2 = Conversion.ToInt((object)txtPartyCount.Text.Trim());
				double GdnGrossWeight2 = Conversion.ToDouble((object)txtGdnGrossWeight.Text.Trim());
				double FactoryWeight2 = Conversion.ToDouble((object)txtfctwt.Text.Trim());
				double FinalWt2 = FactoryWeight2 - GdnGrossWeight2;
				if (num2 == 1)
				{
					if (Conversion.ToInt(txtSaleType.Tag) == 2)
					{
						double Diff2 = Math.Abs(GrossWt - FinalWt2);
						if (GrossWt != FinalWt2 && Diff2 >= GdnAndFactoryWeightCompareToleranceForStop)
						{
							throw new Exception($"Mismatch detected: Adjusted Gross Weight ({GrossWt}) does not match the Final Factory Weight ({FinalWt2}). Tolerance Used: {GdnAndFactoryWeightCompareToleranceForStop}. Entry cannot proceed.");
						}
					}
					else if (GrossWt != FactoryWeight2)
					{
						throw new Exception($"Mismatch in weights: Gross weight entered is {GrossWt}, but it should be equal to the factory weight {FactoryWeight2} '.");
					}
				}
			}
			rows = GridExpense.GetRows();
			foreach (GridEXRow item in rows)
			{
				InvGdnExpense obj = new InvGdnExpense();
				obj.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				obj.Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				obj.Remarks = Conversion.ToString(item.Cells["Remarks"].Value);
				GRN.InvGdnExpenseslist.Add(obj);
			}
			GRN.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			GRN.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvGdn>(GRN, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			GRN.AttachmentsValues = AttachmentsValues;
			GRN.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = InvGdn.Save(GRN);
			if (Id > 0)
			{
				MessageBox.Show("Record Update Successfully " + GRN.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(Id, base.Name, 86, 0);
				}
			}
			else
			{
				MessageBox.Show("Record Save Successfully " + GRN.DocNo);
			}
			if (WagesStatus && WagesActiveOrInActive)
			{
				frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
				frmwagesBillHeader.RefDocTypeId = 86;
				frmwagesBillHeader.RefDocId = success;
				frmwagesBillHeader.GrossWeightTotal = GrossWt;
				frmwagesBillHeader.ShowDialog();
				frmwagesBillHeader.BringToFront();
			}
			reset();
			if (ChkBox.Checked)
			{
				CommonServices.InvGdnSlip260(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
			Insert();
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Expected O, but got Unknown
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_0913: Expected O, but got Unknown
		try
		{
			Id = Conversion.ToInt((object)ID);
			InvGdn invGrn = InvGdn.GetByID(Id);
			if (invGrn != null)
			{
				combcompany.Value = invGrn.CompanyId;
				combbrnch.Value = invGrn.BranchesId;
				combproject.Value = invGrn.ProjectsId;
				txtdocno.Text = invGrn.DocNo.ToString();
				DocDate.Value = invGrn.DocDate;
				tabControl1.SelectedIndex = 0;
				BindSupplierName();
				CmbCustomerName.Value = invGrn.SupplierCustomerId;
				((Control)(object)CmbCustomerName).Enabled = false;
				combstockprty.Value = invGrn.StockPartyId;
				txtsuppref.Text = invGrn.SupplierReference;
				txtrefno.Text = invGrn.ReferenceDocNo;
				txtTicketNos.Text = invGrn.TicketNos;
				if (SubsidiaryAccountAllownOnVouchers)
				{
					CmbTransport.Value = invGrn.TransporterSupCustId;
				}
				else
				{
					CmbTransport.Value = invGrn.TransporterId;
				}
				txtcarramount.Text = invGrn.CarriageAmount.ToString();
				txtsuppwt.Text = Conversion.ToString((object)invGrn.PartyWeight);
				txtfctwt.Text = Conversion.ToString((object)invGrn.FactoryWeight);
				CmbGpNo.Text = Conversion.ToString((object)invGrn.GpNo);
				CmbGpNo.Enabled = false;
				txtvehno.Text = Conversion.ToString((object)invGrn.VehicleNo);
				txtbltyno.Text = Conversion.ToString((object)invGrn.BiltyNo);
				txtContainerNo.Text = Conversion.ToString((object)invGrn.ContainerNo);
				txtContainerNo1.Text = Conversion.ToString((object)invGrn.ContainerNo1);
				txtremarks.Text = Conversion.ToString((object)invGrn.RemarksHeader);
				OrderStatus = Conversion.ToString((object)invGrn.OtherSupCust);
				txtDeliverTerm.Text = OrderStatus;
				GPID = Conversion.ToInt((object)invGrn.OutwardGatePassId);
				AttachmentsValues = invGrn.AttachmentsValues;
				CustomAttachmentsValues = invGrn.CustomAttachmentsValues;
				chkShortWeightLessFromNetWeight.Checked = Conversion.ToBool((object)invGrn.ShortWeightDeductionApply);
				ChkIsStockReservedPerParty.Checked = Conversion.ToBool((object)invGrn.IsStockReserved);
				CmbAdvanceDo.Value = Conversion.ToInt((object)invGrn.AdvanceDeliveryOrderId);
				dtdetail.Rows.Clear();
				double GrossWeight = 0.0;
				for (int j = 0; j < invGrn.invGdnDetail.Count; j++)
				{
					dtdetail.Rows.Add(invGrn.invGdnDetail[j].Id, invGrn.invGdnDetail[j].RefDocumentTypeId, invGrn.invGdnDetail[j].RefDocIdNo, invGrn.invGdnDetail[j].RefDocSubIdNo, invGrn.invGdnDetail[j].SaleOrderDetailId, invGrn.invGdnDetail[j].SaleOrderId, invGrn.invGdnDetail[j].SaleOrderNo, invGrn.invGdnDetail[j].RefPartyId, invGrn.invGdnDetail[j].WarehouseId, invGrn.invGdnDetail[j].ItemId, invGrn.invGdnDetail[j].ItemCode, invGrn.invGdnDetail[j].Item, invGrn.invGdnDetail[j].BrandItemId, invGrn.invGdnDetail[j].BrandItemCode, invGrn.invGdnDetail[j].BrandItemName, invGrn.invGdnDetail[j].CropYear, invGrn.invGdnDetail[j].JobLotId, invGrn.invGdnDetail[j].JobLot, invGrn.invGdnDetail[j].PackingTypeId, invGrn.invGdnDetail[j].PackingType, invGrn.invGdnDetail[j].UOMCode, invGrn.invGdnDetail[j].UOMEquivalent, invGrn.invGdnDetail[j].ItemUomId, invGrn.invGdnDetail[j].ItemQty, invGrn.invGdnDetail[j].GrossWeight, invGrn.invGdnDetail[j].EBWPerUnit, invGrn.invGdnDetail[j].EBWTotal, invGrn.invGdnDetail[j].WtCut, invGrn.invGdnDetail[j].WtCutTotal, invGrn.invGdnDetail[j].AdLsWeight, invGrn.invGdnDetail[j].ShortWeight, invGrn.invGdnDetail[j].NetBillWeight, invGrn.invGdnDetail[j].EbUnitStock, invGrn.invGdnDetail[j].EbTotalStock, invGrn.invGdnDetail[j].StockWeight, invGrn.invGdnDetail[j].ItemRate, invGrn.invGdnDetail[j].ContainerNo, invGrn.invGdnDetail[j].LabReportRef, invGrn.invGdnDetail[j].CityId, invGrn.invGdnDetail[j].AreaCity, 0, invGrn.invGdnDetail[j].ItemQty, invGrn.invGdnDetail[j].InvDeliveryOrderId, invGrn.invGdnDetail[j].InvDeliveryOrderDetailId, invGrn.invGdnDetail[j].RefDocType, invGrn.invGdnDetail[j].RefDocNo, 0, invGrn.invGdnDetail[j].WareHouseToId);
					GrossWeight += invGrn.invGdnDetail[j].GrossWeight;
				}
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				if (OrderStatus == "DeliverOrder")
				{
					txtPartyCount.Text = "1";
					panel5.Dock = DockStyle.None;
					double GdnGrossWeight = InvGdn.GetGdnGrossWeightForValidation(new InvGdn
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 86,
						Id = Conversion.ToInt((object)GPID)
					});
					if (GdnGrossWeight > 0.0)
					{
						double num = Conversion.ToDouble((object)txtfctwt.Text.Trim());
						double DispatchWeight = GdnGrossWeight - GrossWeight;
						double BalanceWeight = num - DispatchWeight;
						txtGdnGrossWeight.Text = Conversion.ToString((object)DispatchWeight);
						txtBalanceWeight.Text = Conversion.ToString((object)BalanceWeight);
					}
					DataTable dtSrNoGet = GatePassOutward.ReadByGpNoForGdnDeliveryOrder(new GatePassOutward
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						FinancialYearId = clsGlobalVariables.ActiveYr.Id,
						Id = GPID
					});
					if (dtSrNoGet.Rows.Count > 0)
					{
						txtTotalDoWeight.Text = Conversion.ToDouble(dtSrNoGet.Rows[0]["TotalDoWeight"]).ToString("#,##0.####");
						txtShortExcessWeight.Text = Conversion.ToDouble(dtSrNoGet.Rows[0]["ShortExcessWeight"]).ToString("#,##0.####");
						int SaleTypeId = Conversion.ToInt(dtSrNoGet.Rows[0]["SaleTypeId"]);
						if (SaleTypeId == 2)
						{
							txtSaleType.Text = "Pack Size Based Billing";
							txtSaleType.Tag = 2;
						}
						else
						{
							txtSaleType.Text = "Weigh Bridge Based Billing";
							txtSaleType.Tag = 1;
						}
						if (Conversion.ToDouble((object)txtShortExcessWeight.Text) < 0.0 && SaleTypeId == 2)
						{
							label60.Text = "Short Weight";
							chkShortWeightLessFromNetWeight.Visible = true;
							ProportionateShortWeight();
						}
						else if (Conversion.ToDouble((object)txtShortExcessWeight.Text) > 0.0)
						{
							label60.Text = "Excess Weight";
							chkShortWeightLessFromNetWeight.Visible = false;
						}
						else
						{
							label60.Text = "Short/Excess Wt";
							chkShortWeightLessFromNetWeight.Visible = false;
						}
					}
					txtDeliverTerm.Size = new Size(120, 23);
				}
				else
				{
					panel5.Dock = DockStyle.Top;
					txtDeliverTerm.Size = new Size(215, 23);
				}
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
					AddRowInvExpGrid();
				}
				grdSettings();
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
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
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
						DocumentTypeId = 86,
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

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCustomerName).ActiveRow == null || Conversion.ToInt(CmbCustomerName.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbCustomerName.Focus();
			return false;
		}
		if (CmbGpNo.Text.Trim() == "" || CmbGpNo.Text.Trim() == "0" || CmbGpNo.Text.Trim() == null)
		{
			MessageBox.Show("Gatepass Field is Required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbGpNo.Focus();
			return false;
		}
		if (txtfctwt.Text.Trim() == "" || txtfctwt.Text.Trim() == "0" || txtfctwt.Text.Trim() == null)
		{
			MessageBox.Show("FactoryWeight Field is Required", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtfctwt.Focus();
			return false;
		}
		return true;
	}

	private bool FormDetailValidation()
	{
		if (((UltraGridBase)CmbOrderNo).ActiveRow == null || Conversion.ToInt(CmbOrderNo.Value) == 0)
		{
			MessageBox.Show("Order Field is Required");
			CmbOrderNo.Focus();
			return false;
		}
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
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("Pack Unit Field is Required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtqty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (txtgwt.Text.Trim() == "" || Conversion.ToDouble((object)txtgwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Gross Weight Field is Required");
			txtgwt.Focus();
			return false;
		}
		if (txtnetwt.Text.Trim() == "" || Conversion.ToDouble((object)txtnetwt.Text.Trim()) < 0.0)
		{
			MessageBox.Show("NetBillWeight Field is Empty or Less than zero Please Check");
			txtnetwt.Focus();
			return false;
		}
		if (txtstockwt.Text.Trim() == "" || Conversion.ToDouble((object)txtstockwt.Text.Trim()) < 0.0)
		{
			MessageBox.Show("StockWeight Field is Empty or Less than zero Please Check");
			txtstockwt.Focus();
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
			txtDeliverTerm.Size = new Size(215, 23);
			chkShortWeightLessFromNetWeight.Checked = false;
			txtSaleType.Text = "";
			txtSaleType.Tag = 0;
			txtTotalDoWeight.Text = string.Empty;
			txtShortExcessWeight.Text = string.Empty;
			((Control)(object)CmbAdvanceDo).Text = "";
			txtAddLessHeader.Text = "";
			GPID = 0;
			Id = 0;
			OrderStatus = "";
			DeliveryOrderType = "";
			txtGdnGrossWeight.Text = "0";
			txtPartyCount.Text = "0";
			txtBalanceWeight.Text = "0";
			((Control)(object)CmbCustomerName).Text = string.Empty;
			((Control)(object)combstockprty).Text = string.Empty;
			txtTicketNos.Text = string.Empty;
			txtsuppref.Text = string.Empty;
			txtrefno.Text = string.Empty;
			((Control)(object)CmbTransport).Text = string.Empty;
			txtcarramount.Text = string.Empty;
			txtsuppwt.Text = string.Empty;
			txtfctwt.Text = string.Empty;
			txtwtdiff.Text = string.Empty;
			CmbGpNo.Text = string.Empty;
			txtvehno.Text = string.Empty;
			txtbltyno.Text = string.Empty;
			txtremarks.Text = string.Empty;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtDeliverTerm.Text = string.Empty;
			txtebu.Text = string.Empty;
			txtebt.Text = string.Empty;
			txtEbUnitStock.Text = string.Empty;
			txtEbTotalStock.Text = string.Empty;
			txtadlswt.Text = string.Empty;
			txtnetwt.Text = string.Empty;
			txtstockwt.Text = string.Empty;
			comblabno.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtdocno.Text = string.Empty;
			btnSave.Visible = true;
			ChkIsStockReservedPerParty.Checked = false;
			panel5.Dock = DockStyle.Top;
			dtdetail.Rows.Clear();
			grd.ClearStructure();
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			dtExpense.Rows.Clear();
			AddRowInvExpGrid();
			GridExpense.DataSource = dtExpense;
			((Control)(object)CmbCustomerName).Enabled = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			Add.Visible = true;
			((Control)(object)CmbOrderNo).Enabled = true;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			DocDate.Focus();
			((UltraGridBase)CmbCustomerName).DataSource = null;
			updateDetailIndex = -1;
			label60.Text = "Short/Excess Wt";
			UpdateDocumentNoUI(Conversion.ToString((object)CommonServices.InvGdnGenerateCode(86)));
			GatepassGridFill(GatepassDataDbCall());
			GetConfigurationsFromGlobalAndBindValuesInColumns();
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
			txtqty.Text = string.Empty;
			txtgwt.Text = string.Empty;
			txtebu.Text = string.Empty;
			txtebt.Text = string.Empty;
			txtEbUnitStock.Text = string.Empty;
			txtEbTotalStock.Text = string.Empty;
			txtadlswt.Text = string.Empty;
			txtnetwt.Text = string.Empty;
			txtstockwt.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtContainerNodetail.Text = string.Empty;
			comblabno.SelectedIndex = -1;
			updateDetailIndex = -1;
			CmbItemName.Focus();
			Add.Visible = true;
			btnCancelUpdateDetial.Visible = false;
			btnUpdateDetail.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvFrmGDN_FormClosing(object sender, FormClosingEventArgs e)
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
			List<string> allServices = new List<string>
			{
				"BranchesAllocatedToUser", "AccountsWithCustomGroupId", "ItemsWithCustomGroupId", "LookUpsEngr", "PaymentTerm", "UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems",
				"MultiCurrency", "AllCities", "CropYear", "JobLot", "InvPackingType"
			};
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			dtJobLotWithJobOrderAndItems = jobLot.JobLot_GetWithJobOrderAndItem(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
			GetConfigurationsFromGlobal();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			TransporterDtFillFromGlobal();
			TransporterAcFill();
			WarehousesDtFillFromGlobalAndBind();
			if (Conversion.ToInt(CmbOrderNo.Value) == 0)
			{
				ItemDtFillFromGlobal();
			}
			else
			{
				ItemDtFillFromPoDBCall();
			}
			ItemNameBind();
			BrandDtFillFromGlobal();
			BrandItemNameBind();
			CropDtFillFromGlobalAndBind();
			JobLotDtFillFromGlobalAndBind();
			PackingTypeDtFillFromGlobalAndBind();
			CityDtFillFromGlobalAndBind();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			OtherItemdtDbCall();
			GridComboBind();
			if (IsStockReservedPerParty && ChkIsStockReservedPerParty.Checked)
			{
				AdvanceDoBind();
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
			MessageBox.Show(ex.ToString());
		}
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdGp).Name);
			if (fs != null)
			{
				grdGp.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DatagridHistoryDetail).Name);
			if (fs != null)
			{
				DatagridHistoryDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void AvailableStock()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		try
		{
			double AvailableStock = GetAvgRatesAndStockInHand.GetStockInHandFromInventoryTrasactions(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = Conversion.ToInt(CmbItemName.Value),
				DocDate = DocDate.Value,
				JobLotId = Conversion.ToInt(CmbJobLot.Value),
				WarehouseId = Conversion.ToInt(CmbWareHouseName.Value)
			});
			if (AvailableStock > 0.0)
			{
				lblStock.Text = AvailableStock.ToString("#,##.##");
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

	private void Total()
	{
		try
		{
			double Qty = 0.0;
			double GrossWeight = 0.0;
			double EBUnit = 0.0;
			double EBTotal = 0.0;
			double WeightCut = 0.0;
			double WeightCutTotal = 0.0;
			double AddLess = 0.0;
			double EbTotalStock = 0.0;
			double EbUnitStock = 0.0;
			if (((UltraGridBase)CmbPackUom).ActiveRow != null)
			{
				Conversion.ToDouble((object)((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value.ToString());
			}
			if (txtqty.Text != string.Empty && txtqty.Text != null)
			{
				Qty = Conversion.ToDouble((object)txtqty.Text.Trim().ToString());
			}
			if (txtgwt.Text != string.Empty && txtgwt.Text != null)
			{
				GrossWeight = Conversion.ToDouble((object)txtgwt.Text.Trim().ToString());
			}
			if (Conversion.ToDouble((object)txtebu.Text) != 0.0)
			{
				EBUnit = Conversion.ToDouble((object)txtebu.Text);
			}
			if (Conversion.ToDouble((object)txtebt.Text) != 0.0)
			{
				EBTotal = Conversion.ToDouble((object)txtebt.Text);
			}
			if (Conversion.ToDouble((object)txtEbUnitStock.Text) != 0.0)
			{
				EbUnitStock = Conversion.ToDouble((object)txtEbUnitStock.Text);
			}
			if (Conversion.ToDouble((object)txtEbTotalStock.Text) != 0.0)
			{
				EbTotalStock = Conversion.ToDouble((object)txtEbTotalStock.Text);
			}
			if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "PmUnit")
			{
				EBTotal = EBUnit * Qty;
				txtebt.Text = EBTotal.ToString("#,##0.###");
				bool_EbUnit = true;
				bool_EbTotal = false;
			}
			else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "PmTotal")
			{
				EBUnit = ((EBTotal > 0.0) ? (EBTotal / Qty) : 0.0);
				txtebu.Text = EBUnit.ToString("#,##0.####");
				bool_EbTotal = true;
				bool_EbUnit = false;
			}
			else if (bool_EbUnit)
			{
				EBTotal = EBUnit * Qty;
				txtebt.Text = EBTotal.ToString("#,##0.##");
			}
			else if (bool_EbTotal)
			{
				EBUnit = ((EBTotal > 0.0) ? (EBTotal / Qty) : 0.0);
				txtebu.Text = EBUnit.ToString("#,##0.####");
			}
			if (txtnetwt.Text != string.Empty && txtnetwt.Text != null)
			{
				Conversion.ToDouble((object)txtnetwt.Text.Trim().ToString());
			}
			if (txtstockwt.Text != string.Empty && txtstockwt.Text != null)
			{
				Conversion.ToDouble((object)txtstockwt.Text.Trim().ToString());
			}
			if (txtwtcut.Text != string.Empty && txtqty.Text != string.Empty)
			{
				WeightCut = Conversion.ToDouble((object)txtwtcut.Text.Trim().ToString());
				txtwtcuttotal.Text = (Qty * WeightCut).ToString();
			}
			else
			{
				WeightCut = 0.0;
				txtwtcuttotal.Text = "0";
			}
			if (txtwtcuttotal.Text != string.Empty && txtwtcuttotal.Text != null)
			{
				WeightCutTotal = Conversion.ToDouble((object)txtwtcuttotal.Text.Trim().ToString());
			}
			if (txtadlswt.Text != string.Empty && txtadlswt.Text != null)
			{
				AddLess = Conversion.ToDouble((object)txtadlswt.Text.Trim());
			}
			double Weight = GrossWeight - EBTotal - WeightCutTotal + AddLess;
			txtnetwt.Text = Weight.ToString();
			if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "EbUnitStock")
			{
				EbTotalStock = EbUnitStock * Qty;
				txtEbTotalStock.Text = EbTotalStock.ToString("#,##0.###");
				bool_EbUnitStock = true;
				bool_EbTotalStock = false;
			}
			else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "EbTotalStock")
			{
				EbUnitStock = ((EbTotalStock > 0.0) ? (EbTotalStock / Qty) : 0.0);
				txtEbUnitStock.Text = EbUnitStock.ToString("#,##0.####");
				bool_EbTotalStock = true;
				bool_EbUnitStock = false;
			}
			else if (bool_EbUnitStock)
			{
				EbTotalStock = EbUnitStock * Qty;
				txtEbTotalStock.Text = EbTotalStock.ToString("#,##0.##");
			}
			else if (bool_EbTotalStock)
			{
				EbUnitStock = ((EbTotalStock > 0.0) ? (EbTotalStock / Qty) : 0.0);
				txtEbUnitStock.Text = EbUnitStock.ToString("#,##0.####");
			}
			double TotalStockWeight = GrossWeight - EbTotalStock + AddLess;
			txtstockwt.Text = TotalStockWeight.ToString();
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
			if (((UltraGridBase)CmbPackUom).ActiveRow != null)
			{
				PackUOM = Conversion.ToDouble((object)((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value.ToString());
			}
			if (txtqty.Text != string.Empty)
			{
				Qty = Conversion.ToDouble((object)txtqty.Text.ToString());
			}
			if (txtgwt.Text != string.Empty)
			{
				GrossWeight = Conversion.ToDouble((object)txtgwt.Text.ToString());
			}
			txtgwt.Text = (PackUOM * Qty).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalSupplierWeight()
	{
		try
		{
			if (txtsuppwt.Text != string.Empty && txtfctwt.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
				double FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
				txtwtdiff.Text = (num - FactoryWeight).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NetandStockWeight()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double GrossWeight = Conversion.ToDouble(obj.Cells["GrossWight"].Value);
					double Emptybags = Conversion.ToDouble(obj.Cells["EbTotal"].Value);
					double EmptybagsStock = Conversion.ToDouble(obj.Cells["EbTotalStock"].Value);
					double AddLesswt = Conversion.ToDouble(obj.Cells["AddLesswt"].Value);
					obj.Cells["NetWeight"].Value = (GrossWeight - Emptybags + AddLesswt).ToString();
					obj.Cells["StockWeight"].Value = GrossWeight - EmptybagsStock + AddLesswt;
				}
			}
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
			int num = grd.GetRows().Count();
			double BalGrossWeight = 0.0;
			if (num > 0)
			{
				double TotalGrossWeight = 0.0;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					TotalGrossWeight += Conversion.ToDouble((object)item.Cells["GrossWight"].Value.ToString());
				}
				if (TotalGrossWeight > 0.0)
				{
					BalGrossWeight = Conversion.ToDouble((object)txtfctwt.Text) - TotalGrossWeight;
				}
				txtBalGrossWeightGrid.Text = BalGrossWeight.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PropotionateBalGrossWeightInGrd()
	{
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				double BalGrossWeight = Conversion.ToDouble((object)txtBalGrossWeightGrid.Text);
				Conversion.ToDouble((object)txtfctwt.Text);
				double TotalGrossWeight = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["GrossWight"], (AggregateFunction)2));
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double GrossWeight = Conversion.ToDouble(obj.Cells["GrossWight"].Value);
					double CurrentGW = Conversion.ToDouble(obj.Cells["GrossWight"].Value) + BalGrossWeight / TotalGrossWeight * GrossWeight;
					obj.Cells["GrossWight"].Value = Conversion.ToString((object)CurrentGW);
					obj.EndEdit();
				}
				grd.UpdateData();
				rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (r != null && (int)r.RowType == 0 && r.Cells["GrossWight"].Value.ToString() != string.Empty && r.Cells["NetWeight"].Value.ToString() != string.Empty)
					{
						double num = Conversion.ToDouble((object)r.Cells["GrossWight"].Value.ToString());
						double EbTotal = Conversion.ToDouble((object)r.Cells["EbTotal"].Value.ToString());
						double AddLesswt = Conversion.ToDouble((object)r.Cells["AddLesswt"].Value.ToString());
						double diff = num - EbTotal + AddLesswt;
						r.Cells["NetWeight"].Value = diff.ToString();
						r.Cells["StockWeight"].Value = diff.ToString();
					}
					r.EndEdit();
				}
				grd.UpdateData();
			}
			else
			{
				MessageBox.Show("Grid Record Not Found...");
			}
			BalGrossWeightGrd();
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

	private void GridAddless()
	{
	}

	private void txtAddLessHeader_TextChanged(object sender, EventArgs e)
	{
		GridAddless();
	}

	private void txtadlswt_TextChanged_1(object sender, EventArgs e)
	{
		Total();
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		TotalWeight();
		Total();
	}

	private void txtebu_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtsuppwt_TextChanged(object sender, EventArgs e)
	{
		TotalSupplierWeight();
	}

	private void txtfctwt_TextChanged(object sender, EventArgs e)
	{
		TotalSupplierWeight();
	}

	private void combpckuom_Leave(object sender, EventArgs e)
	{
		Total();
	}

	private void txtgwt_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtadlswt_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtadlswt_Leave(object sender, EventArgs e)
	{
		Total();
	}

	private void chkShortWeightLessFromNetWeight_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (chkShortWeightLessFromNetWeight.Checked)
			{
				ProportionateShortWeight();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					double Gross = Conversion.ToDouble(obj.Cells["GrossWight"].Value);
					double EbTotal = Conversion.ToDouble(obj.Cells["EbTotal"].Value);
					double AddLesswt = Conversion.ToDouble(obj.Cells["AddLesswt"].Value);
					double ShortWeight = Conversion.ToDouble(obj.Cells["ShortWeight"].Value);
					double NetWeight = Gross - EbTotal + AddLesswt - ShortWeight;
					obj.BeginEdit();
					obj.Cells["NetWeight"].Value = NetWeight;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					double Gross2 = Conversion.ToDouble(obj2.Cells["GrossWight"].Value);
					double EbTotal2 = Conversion.ToDouble(obj2.Cells["EbTotal"].Value);
					double AddLesswt2 = Conversion.ToDouble(obj2.Cells["AddLesswt"].Value);
					double NetWeight2 = Gross2 - EbTotal2 + AddLesswt2;
					obj2.BeginEdit();
					obj2.Cells["NetWeight"].Value = NetWeight2;
					obj2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtebt_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnPrint260A_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvGrnandGdnReports.InvGdnSlip260(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Id
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "260A-DeliveryChallansByGDN.rpt");
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
			CommonServices.InvGdnSlip260(Id);
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

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind(HistoryComboDbCall("Supplier"));
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
			obj.DocumentTypeId = 86;
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
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("GpId", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DocNo", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("CustomerName", typeof(string));
				dtcol.Columns.Add("GpNo", typeof(int));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("FactoryWeight", typeof(double));
				dtcol.Columns.Add("PartyWeight", typeof(double));
				dtcol.Columns.Add("AccountTitle", typeof(string));
				dtcol.Columns.Add("Freight", typeof(double));
				dtcol.Columns.Add("NetPaid", typeof(double));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("StockStatus", typeof(string));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ApprovedUser", typeof(string));
				dtcol.Columns.Add("ApprovedDate", typeof(DateTime));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtHistry.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtHistry.Rows[i]["RecordNo"], dtHistry.Rows[i]["Id"], dtHistry.Rows[i]["GpId"], dtHistry.Rows[i]["DocDate"], dtHistry.Rows[i]["DocNo"], dtHistry.Rows[i]["DocumentTypeId"], dtHistry.Rows[i]["CustomerName"], dtHistry.Rows[i]["GpNo"], dtHistry.Rows[i]["VehicleNo"], dtHistry.Rows[i]["BiltyNo"], dtHistry.Rows[i]["FactoryWeight"], dtHistry.Rows[i]["PartyWeight"], dtHistry.Rows[i]["AccountTitle"], dtHistry.Rows[i]["Freight"], dtHistry.Rows[i]["NetPaid"], dtHistry.Rows[i]["RemarksHeader"], dtHistry.Rows[i]["StockStatus"], dtHistry.Rows[i]["UserName"], dtHistry.Rows[i]["EntryDate"], dtHistry.Rows[i]["ModifyUserName"], dtHistry.Rows[i]["ModifyDate"], dtHistry.Rows[i]["ApprovedUserName"], dtHistry.Rows[i]["ApprovedDate"], dtHistry.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtcol;
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
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3, null, null, null, ColumnSetting: false);
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName + 30;
			DataGridHistory.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			DataGridHistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			DataGridHistory.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			DataGridHistory.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["PartyWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			DataGridHistory.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["NetPaid"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["StockStatus"].Width = Constants.InventoryConstants.StatusConstant;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			DataGridHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedUser"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FactoryWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Freight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["Freight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["NetPaid"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetPaid"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetPaid"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["NetPaid"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["RecordNo"].Visible = false;
			DataGridHistory.RootTable.Columns["GpId"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["PartyWeight"].Visible = false;
			DataGridHistory.RootTable.Columns["StockStatus"].Visible = IsStockReservedPerParty;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 35;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 45;
			DataGridHistory.RootTable.Columns.Add("Print1");
			DataGridHistory.RootTable.Columns["Print1"].Caption = "Print1";
			DataGridHistory.RootTable.Columns["Print1"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print1"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print1"].ButtonText = "260A-Print";
			DataGridHistory.RootTable.Columns["Print1"].Width = 75;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.RootTable.Columns["Print1"].Position = 2;
			DataGridHistory.FrozenColumns = 3;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				GridEXRow item = DataGridHistory.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(item.Cells["Id"].Value), base.Name);
				}
			}
			if (!(e.Column.Key == "GpNo"))
			{
				return;
			}
			OutwardGatePass obj = new OutwardGatePass(UserAccount);
			if (clsGlobalVariables.ScreenViewReights.Count > 0)
			{
				if (Conversion.ToBool((object)clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "OutwardGatePass").ToList()[0].Value.ToString()))
				{
					obj.Show();
					obj.ReadById(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["GpId"].Value));
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
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				reset();
				Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				ReadById(Id);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.InvGdnSlip260(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Print1")
			{
				DataTable dt = InvGrnandGdnReports.InvGdnSlip260(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value)
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "260A-DeliveryChallansByGDN.rpt");
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
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Column.Key == "Edit")
			{
				reset();
				Id = CurrId;
				ReadById(Id);
				((Control)(object)CmbOrderNo).Enabled = false;
			}
			if (e.Column.Key == "View")
			{
				CommonServices.InvGdnSlip260(CurrId);
			}
			if (e.Column.Key == "Print1")
			{
				DataTable dt = InvGrnandGdnReports.InvGdnSlip260(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value)
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "260A-DeliveryChallansByGDN.rpt");
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
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DetailGridBind();
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

	private void DetailGridBind()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (DataGridHistory.CurrentRow != null && (int)DataGridHistory.CurrentRow.RowType == 0 && DataGridHistory.CurrentRow.Cells.Count > 0)
			{
				Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				InvGdn invGrn = InvGdn.GetByID(Id);
				DataTable dtDetailHistory = new DataTable();
				dtDetailHistory.Columns.Add("OrderNo", typeof(int));
				dtDetailHistory.Columns.Add("WareHouse");
				dtDetailHistory.Columns.Add("ItemCode");
				dtDetailHistory.Columns.Add("ItemName");
				dtDetailHistory.Columns.Add("BrandItemCode");
				dtDetailHistory.Columns.Add("BrandItemName");
				dtDetailHistory.Columns.Add("PackType");
				dtDetailHistory.Columns.Add("CropYear");
				dtDetailHistory.Columns.Add("JobLot");
				dtDetailHistory.Columns.Add("PackUOM");
				dtDetailHistory.Columns.Add("ItemQty", typeof(double));
				dtDetailHistory.Columns.Add("GrossWeight", typeof(double));
				dtDetailHistory.Columns.Add("EBWPerUnit");
				dtDetailHistory.Columns.Add("EBWTotal", typeof(double));
				dtDetailHistory.Columns.Add("WeightCut");
				dtDetailHistory.Columns.Add("WeightCutTotal", typeof(double));
				dtDetailHistory.Columns.Add("AdLsWeight");
				dtDetailHistory.Columns.Add("NetBillWeight", typeof(double));
				dtDetailHistory.Columns.Add("EbUnitStock", typeof(double));
				dtDetailHistory.Columns.Add("EbTotalStock", typeof(double));
				dtDetailHistory.Columns.Add("StockWeight", typeof(double));
				dtDetailHistory.Columns.Add("ContainerNo");
				dtDetailHistory.Columns.Add("LabNo");
				dtDetailHistory.Columns.Add("CityName");
				dtDetailHistory.Columns.Add("RefDocType");
				dtDetailHistory.Columns.Add("RefDocNo");
				for (int i = 0; i < invGrn.invGdnDetail.Count; i++)
				{
					dtDetailHistory.Rows.Add(invGrn.invGdnDetail[i].SaleOrderNo, invGrn.invGdnDetail[i].WareHouseCode, invGrn.invGdnDetail[i].ItemCode, invGrn.invGdnDetail[i].Item, invGrn.invGdnDetail[i].BrandItemCode, invGrn.invGdnDetail[i].BrandItemName, invGrn.invGdnDetail[i].PackingType, invGrn.invGdnDetail[i].CropYear, invGrn.invGdnDetail[i].JobLot, invGrn.invGdnDetail[i].UOMCode, invGrn.invGdnDetail[i].ItemQty, invGrn.invGdnDetail[i].GrossWeight, Math.Round(invGrn.invGdnDetail[i].EBWPerUnit, 2), Math.Round(invGrn.invGdnDetail[i].EBWTotal), Math.Round(invGrn.invGdnDetail[i].WtCut), Math.Round(invGrn.invGdnDetail[i].WtCutTotal), Math.Round(invGrn.invGdnDetail[i].AdLsWeight), Math.Round(invGrn.invGdnDetail[i].NetBillWeight), invGrn.invGdnDetail[i].EbUnitStock, invGrn.invGdnDetail[i].EbTotalStock, Math.Round(invGrn.invGdnDetail[i].StockWeight), invGrn.invGdnDetail[i].ContainerNo, invGrn.invGdnDetail[i].LabReportRef, invGrn.invGdnDetail[i].AreaCity, invGrn.invGdnDetail[i].RefDocType, invGrn.invGdnDetail[i].RefDocNo);
				}
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
			DatagridHistoryDetail.AllowEdit = (InheritableBoolean)2;
			DatagridHistoryDetail.AllowDelete = (InheritableBoolean)2;
			GridEX_Helper.GridWrappingAndColumnSettings(DatagridHistoryDetail, 2, 3, null, null, null, ColumnSetting: false);
			DatagridHistoryDetail.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DatagridHistoryDetail.RootTable.Columns["WareHouse"].Width = Constants.InventoryConstants.WareHouseName;
			DatagridHistoryDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			DatagridHistoryDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			DatagridHistoryDetail.RootTable.Columns["BrandItemCode"].Width = Constants.InventoryConstants.ItemCode;
			DatagridHistoryDetail.RootTable.Columns["BrandItemName"].Width = Constants.InventoryConstants.ItemName;
			DatagridHistoryDetail.RootTable.Columns["PackType"].Width = Constants.InventoryConstants.PackingType;
			DatagridHistoryDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			DatagridHistoryDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			DatagridHistoryDetail.RootTable.Columns["PackUOM"].Width = Constants.InventoryConstants.UomConstant;
			DatagridHistoryDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			DatagridHistoryDetail.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DatagridHistoryDetail.RootTable.Columns["EBWPerUnit"].Width = Constants.InventoryConstants.EbWt;
			DatagridHistoryDetail.RootTable.Columns["EBWTotal"].Width = Constants.InventoryConstants.EbWt;
			DatagridHistoryDetail.RootTable.Columns["EbUnitStock"].Width = Constants.InventoryConstants.EbWt;
			DatagridHistoryDetail.RootTable.Columns["EbTotalStock"].Width = Constants.InventoryConstants.EbWt;
			DatagridHistoryDetail.RootTable.Columns["WeightCut"].Width = Constants.InventoryConstants.WtCut;
			DatagridHistoryDetail.RootTable.Columns["WeightCutTotal"].Width = Constants.InventoryConstants.WtCut;
			DatagridHistoryDetail.RootTable.Columns["AdLsWeight"].Width = Constants.InventoryConstants.AddLessConstant;
			DatagridHistoryDetail.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DatagridHistoryDetail.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DatagridHistoryDetail.RootTable.Columns["ContainerNo"].Width = Constants.InventoryConstants.ContainerConstant;
			DatagridHistoryDetail.RootTable.Columns["LabNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DatagridHistoryDetail.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			DatagridHistoryDetail.RootTable.Columns["RefDocType"].Width = Constants.InventoryConstants.DocumentType;
			DatagridHistoryDetail.RootTable.Columns["RefDocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DatagridHistoryDetail.RootTable.Columns["ItemQty"].Caption = "Qty";
			DatagridHistoryDetail.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["ItemName"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["PackType"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["CropYear"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["PackUOM"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["GrossWeight"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["EBWPerUnit"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["EBWTotal"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["WeightCut"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["WeightCutTotal"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["AdLsWeight"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["NetBillWeight"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["StockWeight"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["ContainerNo"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["LabNo"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["CityName"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["RefDocType"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["RefDocNo"].EditType = (EditType)0;
			DatagridHistoryDetail.RootTable.Columns["OrderNo"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["WareHouse"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["JobLot"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["PackUOM"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["ItemQty"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["GrossWeight"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["EBWPerUnit"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["EBWTotal"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["WeightCut"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["WeightCutTotal"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["AdLsWeight"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["NetBillWeight"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["StockWeight"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["ContainerNo"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["LabNo"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["CityName"].FilterEditType = (FilterEditType)1;
			DatagridHistoryDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DatagridHistoryDetail.RootTable.Columns["ItemQty"].FormatString = "0,0";
			DatagridHistoryDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			DatagridHistoryDetail.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			DatagridHistoryDetail.RootTable.Columns["EBWPerUnit"].TextAlignment = (TextAlignment)3;
			DatagridHistoryDetail.RootTable.Columns["EBWPerUnit"].FormatString = "#,##0.###";
			DatagridHistoryDetail.RootTable.Columns["EBWTotal"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryDetail.RootTable.Columns["EBWTotal"].TextAlignment = (TextAlignment)3;
			DatagridHistoryDetail.RootTable.Columns["EBWTotal"].FormatString = "#,##0.###";
			DatagridHistoryDetail.RootTable.Columns["WeightCut"].TextAlignment = (TextAlignment)3;
			DatagridHistoryDetail.RootTable.Columns["WeightCut"].FormatString = "#,##0.###";
			DatagridHistoryDetail.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryDetail.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			DatagridHistoryDetail.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.###";
			DatagridHistoryDetail.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.###";
			DatagridHistoryDetail.RootTable.Columns["AdLsWeight"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryDetail.RootTable.Columns["AdLsWeight"].TextAlignment = (TextAlignment)3;
			DatagridHistoryDetail.RootTable.Columns["AdLsWeight"].FormatString = "#,##0.###";
			DatagridHistoryDetail.RootTable.Columns["AdLsWeight"].TotalFormatString = "#,##0.###";
			DatagridHistoryDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			DatagridHistoryDetail.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.###";
			DatagridHistoryDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.###";
			DatagridHistoryDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			DatagridHistoryDetail.RootTable.Columns["StockWeight"].FormatString = "#,##0.###";
			DatagridHistoryDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.###";
			DatagridHistoryDetail.RootTable.Columns["EbUnitStock"].TextAlignment = (TextAlignment)3;
			DatagridHistoryDetail.RootTable.Columns["EbUnitStock"].FormatString = "#,##0.###";
			DatagridHistoryDetail.RootTable.Columns["EbTotalStock"].AggregateFunction = (AggregateFunction)2;
			DatagridHistoryDetail.RootTable.Columns["EbTotalStock"].TextAlignment = (TextAlignment)3;
			DatagridHistoryDetail.RootTable.Columns["EbTotalStock"].FormatString = "#,##0.###";
			ctrlGrdBar4_Load(null, null);
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

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = DataGridHistory.CurrentRow;
				if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
				{
					if (DataGridHistory.CurrentColumn.Key == "Edit")
					{
						reset();
						Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
						ReadById(Id);
					}
					else if (DataGridHistory.CurrentColumn.Key == "Print")
					{
						CommonServices.InvGdnSlip260(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
					}
					else if (DataGridHistory.CurrentColumn.Key == "Print1")
					{
						DataTable dt = InvGrnandGdnReports.InvGdnSlip260(new ReportsParameters
						{
							OrganizationId = UserAccount.OrganizationId,
							CompanyId = UserAccount.CompanyId,
							Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value)
						});
						if (dt.Rows.Count == 0 || dt == null)
						{
							throw new Exception("Record Not Found For Display");
						}
						Reporting val = new Reporting();
						val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
						val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
						val.ShowReportWithDataTable(dt, "260A-DeliveryChallansByGDN.rpt");
					}
					else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
					{
						GridEXRow item = DataGridHistory.CurrentRow;
						if (item != null && (int)item.RowType == 0)
						{
							CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(item.Cells["Id"].Value), base.Name);
						}
					}
					else if (DataGridHistory.CurrentColumn.Key == "GpNo")
					{
						OutwardGatePass obj = new OutwardGatePass(UserAccount);
						if (clsGlobalVariables.ScreenViewReights.Count > 0)
						{
							if (!Conversion.ToBool((object)clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "OutwardGatePass").ToList()[0].Value.ToString()))
							{
								MessageBox.Show("Please Check Screen Rights");
								return;
							}
							obj.Show();
							obj.ReadById(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["GpId"].Value));
						}
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				reset();
				Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				ReadById(Id);
			}
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
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 113;
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
				if (e.Control && e.KeyCode == Keys.P)
				{
					CommonServices.InvGdnSlip260(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
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
			if (printToolStripButton.Visible && printToolStripButton.Enabled && e.Control && e.KeyCode == Keys.P)
			{
				printToolStripButton_Click(null, null);
			}
			if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnSave_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
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
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
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
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtdetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				if (grd.CurrentColumn.Key == "Add")
				{
					if (Conversion.ToInt(txtSaleType.Tag) == 2)
					{
						double BalQty = Conversion.ToDouble(r.Cells["BalQty"].Value);
						double sum = Conversion.ToDouble(dtdetail.Compute("Sum(Qty)", "DoDetailId = '" + Conversion.ToString(r.Cells["DoDetailId"].Value) + "'"));
						double Rem = BalQty - sum;
						if (Rem > 0.0)
						{
							DataRow dr = dtdetail.Rows[r.RowIndex];
							dtdetail.Rows.Add(dr.ItemArray);
							if (btnUpdate.Visible && btnUpdate.Enabled)
							{
								dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
							}
							dtdetail.Rows[dtdetail.Rows.Count - 1]["Qty"] = Rem;
						}
						else
						{
							MessageBox.Show("Can't break further bcoz Total Qty is Equal To BalQty:" + BalQty.ToString("#,##0.###") + ".\n If you want to break further first break any row of same Do Detail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
					}
					else
					{
						DataRow dr2 = dtdetail.Rows[r.RowIndex];
						dtdetail.Rows.Add(dr2.ItemArray);
						if (btnUpdate.Visible && btnUpdate.Enabled)
						{
							dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
						}
					}
				}
				if (grd.CurrentColumn.Key == "AvailableStock" && Conversion.ToDouble(r.Cells["AvailableStock"].Value) > 0.0)
				{
					List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmStockReport").ToList();
					if (newList.Count <= 0)
					{
						MessageBox.Show("You dont have right");
						return;
					}
					if (!Conversion.ToBool((object)newList[0].Value.ToString()))
					{
						MessageBox.Show("You dont have right");
						return;
					}
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
				if (grd.CurrentColumn.Key == "Loader")
				{
					int Index = Conversion.ToInt((object)r.RowIndex);
					int BeforeLoadGridRowCount = grd.GetRows().Count();
					if (updateDetailIndex > -1)
					{
						throw new Exception("Reset Detail first...");
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
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string Name = "";
			if (grd.CurrentColumn.Key == "WareHouse" || grd.CurrentColumn.Key == "CropYear" || grd.CurrentColumn.Key == "JobId" || grd.CurrentColumn.Key == "PackingTypeId")
			{
				if (Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value) != 0 && Conversion.ToInt(r.Cells["RefDocIdNo"].Value) != 0 && Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value) != 0)
				{
					throw new Exception("You can't change parameters when RefDocument is Present");
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "WareHouse")
				{
					GrdPopUp obj7 = new GrdPopUp(dtwh, "Id", "WareHouseName");
					obj7.StartPosition = FormStartPosition.CenterScreen;
					obj7.ShowDialog();
					obj7.BringToFront();
					Id = Conversion.ToInt((object)obj7.ReturnId);
					Name = Conversion.ToString((object)obj7.ReturnName);
					r.Cells["WareHouse"].Value = Id;
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "CropYear")
				{
					GrdPopUp obj6 = new GrdPopUp(dtCrop, "Id", "CropYear");
					obj6.StartPosition = FormStartPosition.CenterScreen;
					obj6.ShowDialog();
					obj6.BringToFront();
					Id = Conversion.ToInt((object)obj6.ReturnId);
					Name = Conversion.ToString((object)obj6.ReturnName);
					r.Cells["CropYear"].Value = Name;
				}
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "Job" || grd.CurrentColumn.Key == "JobId"))
				{
					GrdPopUp obj5 = new GrdPopUp(dtjob, "Id", "JobLotDescription");
					obj5.StartPosition = FormStartPosition.CenterScreen;
					obj5.ShowDialog();
					obj5.BringToFront();
					Id = Conversion.ToInt((object)obj5.ReturnId);
					Name = Conversion.ToString((object)obj5.ReturnName);
					r.Cells["JobId"].Value = Id;
					r.Cells["Job"].Value = Name;
				}
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "PackingType" || grd.CurrentColumn.Key == "PackingTypeId"))
				{
					GrdPopUp obj4 = new GrdPopUp(dtPack, "Id", "PackTypeDesc");
					obj4.StartPosition = FormStartPosition.CenterScreen;
					obj4.ShowDialog();
					obj4.BringToFront();
					Id = Conversion.ToInt((object)obj4.ReturnId);
					Name = Conversion.ToString((object)obj4.ReturnName);
					r.Cells["PackingTypeId"].Value = Id;
					r.Cells["PackingType"].Value = Name;
				}
			}
			if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "City" || grd.CurrentColumn.Key == "CityId"))
			{
				GrdPopUp obj3 = new GrdPopUp(dtcity, "Id", "CityName");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				Name = Conversion.ToString((object)obj3.ReturnName);
				r.Cells["CityId"].Value = Id;
				r.Cells["City"].Value = Name;
			}
			if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "BrandItemId" || grd.CurrentColumn.Key == "BrandItemName" || grd.CurrentColumn.Key == "BrandItemCode"))
			{
				GrdPopUp obj2 = new GrdPopUp(dtBrandItem, "Id", "ItemName");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				Name = Conversion.ToString((object)obj2.ReturnName);
				DataRow foundRow = dtBrandItem.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Id);
				string Code = ((foundRow != null) ? Conversion.ToString(foundRow["ItemCode"]) : "");
				r.Cells["BrandItemId"].Value = Id;
				r.Cells["BrandItemName"].Value = Name;
				r.Cells["BrandItemCode"].Value = Code;
			}
			if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "UOMId" || grd.CurrentColumn.Key == "UOM") && Conversion.ToInt(txtSaleType.Tag) != 2)
			{
				DataTable dataTable = CommonServices.dtUomFromGloablUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
				GrdPopUp obj = new GrdPopUp(dataTable, "Id", "UOMCode");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				Name = Conversion.ToString((object)obj.ReturnName);
				decimal Uom = default(decimal);
				DataRow firstRow = (from row in dataTable.AsEnumerable()
					where row.Field<int>("Id") == Id
					select row).FirstOrDefault();
				if (firstRow != null)
				{
					Uom = Conversion.ToDecimal(firstRow["Equivalent"]);
				}
				r.Cells["UOMId"].Value = Id;
				r.Cells["UOM"].Value = Name;
				r.Cells["PUomEquivalent"].Value = Uom;
			}
			grd.UpdateData();
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

	private void grdGp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdGp.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdGp.CurrentColumn == null || !(grdGp.CurrentColumn.Key == "Load"))
			{
				return;
			}
			if (btnSave.Visible && btnSave.Enabled)
			{
				GPID = 0;
				string GpStatus = Conversion.ToString(grdGp.CurrentRow.Cells["Status"].Value);
				int num = Conversion.ToInt(grdGp.CurrentRow.Cells["FlowValue"].Value);
				double FactoryWeight = Conversion.ToInt(grdGp.CurrentRow.Cells["FactoryWeight"].Value);
				if (num == 1)
				{
					if (FactoryWeight == 0.0)
					{
						throw new Exception("Weigh Bridge against this Gp is Not done Yet.Please Make Sure Weigh Bridge entry First");
					}
					GPID = Conversion.ToInt(grdGp.CurrentRow.Cells["OutwardGatePassId"].Value);
					CmbGpNo.Text = Conversion.ToString(grdGp.CurrentRow.Cells["GpSrNo"].Value);
					combgatepass_Leave(null, null);
					CmbCustomerName.Focus();
				}
				else
				{
					if (!(GpStatus == "Accepted"))
					{
						throw new Exception("Status Not Accepted Please check status");
					}
					GPID = Conversion.ToInt(grdGp.CurrentRow.Cells["OutwardGatePassId"].Value);
					CmbGpNo.Text = Conversion.ToString(grdGp.CurrentRow.Cells["GpSrNo"].Value);
					combgatepass_Leave(null, null);
					CmbCustomerName.Focus();
				}
			}
			else
			{
				MessageBox.Show("Please Reset the form First...");
			}
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

	private void btnRecordsUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dtIds = InvGdn.GetGdnIdsForAutoUpdation(UserAccount.OrganizationId, UserAccount.CompanyId, 86);
			if (dtIds != null && dtIds.Rows.Count > 0)
			{
				bool ShouldStop = false;
				for (int i = 0; i < dtIds.Rows.Count; i++)
				{
					int RecId = Conversion.ToInt(dtIds.Rows[i]["Id"]);
					ReadById(RecId);
					ShouldStop = AutoUpdateRecord(RecId);
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

	private void btnGenerateAvailableStock_Click(object sender, EventArgs e)
	{
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() <= 0)
			{
				return;
			}
			bool FifoFeature = CommonServices.GetERPFeatureById(5);
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (FifoFeature)
				{
					double AvailableWeight = 0.0;
					DataTable dt = GetAvgRatesAndStockInHand.GetStockByFifoMethod(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						ItemId = Conversion.ToInt(item.Cells["ItemId"].Value),
						DocDate = DocDate.Value,
						WarehouseId = Conversion.ToInt(item.Cells["WareHouse"].Value),
						CropYear = Conversion.ToString((object)item.Cells["CropYear"].Text),
						JobLotId = Conversion.ToInt(item.Cells["JobId"].Value),
						PackingTypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value),
						stockUOM = Conversion.ToInt(item.Cells["UOMId"].Value)
					});
					if (dt.Rows.Count > 0)
					{
						AvailableWeight = dt.AsEnumerable().Sum((DataRow row) => row.Field<double>("BalWeight"));
					}
					item.BeginEdit();
					item.Cells["AvailableStock"].Value = AvailableWeight;
					item.EndEdit();
				}
				else
				{
					double Weight = 0.0;
					DataTable dt2 = GetAvgRatesAndStockInHand.GetAvailableStock(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						ItemId = Conversion.ToInt(item.Cells["ItemId"].Value),
						DocDate = DocDate.Value,
						WarehouseId = Conversion.ToInt(item.Cells["WareHouse"].Value),
						CropYear = Conversion.ToString((object)item.Cells["CropYear"].Text),
						JobLotId = Conversion.ToInt(item.Cells["JobId"].Value),
						PackingTypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value),
						stockUOM = Conversion.ToInt(item.Cells["UOMId"].Value),
						ActionId = 1
					});
					if (dt2.Rows.Count > 0)
					{
						Weight = Conversion.ToDouble(dt2.Rows[0]["BalWeight"]);
					}
					item.BeginEdit();
					item.Cells["AvailableStock"].Value = Weight;
					item.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "EbUnit" || e.Column.Key == "EbTotal" || e.Column.Key == "WtCut" || e.Column.Key == "WtCutTotal" || e.Column.Key == "EbUnitStock" || e.Column.Key == "EbTotalStock" || e.Column.Key == "AddLesswt") && !CommonServices.IsPositiveNumeric(e))
			{
				throw new Exception("Please Type Only Positive Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void txtadlswt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEbUnitStock_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEbUnitStock_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbTotalStock_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbTotalStock_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_0645: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Expected O, but got Unknown
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Expected O, but got Unknown
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Expected O, but got Unknown
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Expected O, but got Unknown
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0783: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Expected O, but got Unknown
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Expected O, but got Unknown
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07af: Expected O, but got Unknown
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Expected O, but got Unknown
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Expected O, but got Unknown
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Expected O, but got Unknown
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Expected O, but got Unknown
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Expected O, but got Unknown
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Expected O, but got Unknown
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Expected O, but got Unknown
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8d: Expected O, but got Unknown
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b98: Expected O, but got Unknown
		//IL_0d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3a: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d66: Expected O, but got Unknown
		//IL_0e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Expected O, but got Unknown
		//IL_14a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b2: Expected O, but got Unknown
		//IL_1939: Unknown result type (might be due to invalid IL or missing references)
		//IL_1943: Expected O, but got Unknown
		//IL_1950: Unknown result type (might be due to invalid IL or missing references)
		//IL_195a: Expected O, but got Unknown
		//IL_1967: Unknown result type (might be due to invalid IL or missing references)
		//IL_1971: Expected O, but got Unknown
		//IL_197e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1988: Expected O, but got Unknown
		//IL_1995: Unknown result type (might be due to invalid IL or missing references)
		//IL_199f: Expected O, but got Unknown
		//IL_b845: Unknown result type (might be due to invalid IL or missing references)
		//IL_b84f: Expected O, but got Unknown
		//IL_b873: Unknown result type (might be due to invalid IL or missing references)
		//IL_b87d: Expected O, but got Unknown
		GridEXLayout grdGp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance39 = new Appearance();
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
		Appearance appearance52 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		GridEXLayout GridExpense_DesignTimeLayout = new GridEXLayout();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Sale.InvFrmGDN));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
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
		this.label47 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtEbTotalStock = new System.Windows.Forms.TextBox();
		this.Add = new System.Windows.Forms.Button();
		this.txtEbUnitStock = new System.Windows.Forms.TextBox();
		this.panel11 = new System.Windows.Forms.Panel();
		this.RadBrandName = new System.Windows.Forms.RadioButton();
		this.RadBrandCode = new System.Windows.Forms.RadioButton();
		this.label12 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.label62 = new System.Windows.Forms.Label();
		this.lblStock = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.CmbCropYear = new UltraCombo();
		this.CmbOrderNo = new UltraCombo();
		this.CmbPackUom = new UltraCombo();
		this.CmbCity = new UltraCombo();
		this.CmbWareHouseTo = new UltraCombo();
		this.CmbWareHouseName = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.CmbPackingType = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.CmbBrandItemName = new UltraCombo();
		this.labelWareTo = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.txtContainerNodetail = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtebt = new System.Windows.Forms.TextBox();
		this.txtebu = new System.Windows.Forms.TextBox();
		this.txtgwt = new System.Windows.Forms.TextBox();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.txtadlswt = new System.Windows.Forms.TextBox();
		this.txtstockwt = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.txtnetwt = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.comblabno = new System.Windows.Forms.ComboBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtwtcuttotal = new System.Windows.Forms.TextBox();
		this.txtwtcut = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.GridExpense = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label52 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.ChkIsStockReservedPerParty = new System.Windows.Forms.CheckBox();
		this.txtDeliverTerm = new System.Windows.Forms.TextBox();
		this.txtShortExcessWeight = new System.Windows.Forms.TextBox();
		this.label60 = new System.Windows.Forms.Label();
		this.txtTotalDoWeight = new System.Windows.Forms.TextBox();
		this.label59 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtAddLessHeader = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtcarramount = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtTicketNos = new System.Windows.Forms.TextBox();
		this.txtBalGrossWeightGrid = new System.Windows.Forms.TextBox();
		this.txtfctwt = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.txtBalanceWeight = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.txtGdnGrossWeight = new System.Windows.Forms.TextBox();
		this.labelAdvDoNo = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbAdvanceDo = new UltraCombo();
		this.txtContainerNo1 = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtbltyno = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.txtContainerNo = new System.Windows.Forms.TextBox();
		this.CmbCustomerName = new UltraCombo();
		this.CmbGpNo = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbTransport = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtvehno = new System.Windows.Forms.TextBox();
		this.txtSaleType = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtPartyCount = new System.Windows.Forms.TextBox();
		this.combbrnch = new UltraCombo();
		this.combproject = new UltraCombo();
		this.combstockprty = new UltraCombo();
		this.combcompany = new UltraCombo();
		this.txtrefno = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtsuppwt = new System.Windows.Forms.TextBox();
		this.txtsuppref = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtwtdiff = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.chkShortWeightLessFromNetWeight = new System.Windows.Forms.CheckBox();
		this.chkPropBalGrossInGrid = new System.Windows.Forms.CheckBox();
		this.btnRecordsUpdate = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.btnPrint260A = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnStockReport = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnGenerateAvailableStock = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label51 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.DatagridHistoryDetail = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label58 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
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
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnGdnFormHistory = new System.Windows.Forms.ToolStripButton();
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
		this.groupBox3.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.tabPage4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridExpense).BeginInit();
		this.panel12.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAdvanceDo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combbrnch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combstockprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcompany).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistoryDetail).BeginInit();
		this.panel15.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1596, 768);
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
		this.tabControl1.Size = new System.Drawing.Size(1596, 768);
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
		this.tabPage1.Size = new System.Drawing.Size(1588, 739);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdGp);
		this.panel10.Controls.Add(this.panel9);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(2, 565);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1584, 172);
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
		((System.Windows.Forms.Control)(object)this.grdGp).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdGp).Name = "grdGp";
		((ScrollableBase)this.grdGp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGp).Size = new System.Drawing.Size(1584, 147);
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
		this.panel9.Size = new System.Drawing.Size(1584, 25);
		this.panel9.TabIndex = 1;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1544, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdGp;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 26;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.White;
		this.label39.Location = new System.Drawing.Point(3, 5);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(974, 16);
		this.label39.TabIndex = 25;
		this.label39.Text = "This Grid show Pending GPs For GDN Entry,User can Load GatePasses With Accepted Status for Entry From This grid,GP With Open Status Will Not Load";
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(2, 217);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1584, 348);
		this.tabControl2.TabIndex = 7;
		this.tabPage3.Controls.Add(this.panel19);
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1576, 319);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel19.Controls.Add(this.panel13);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(3, 124);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1570, 192);
		this.panel19.TabIndex = 6;
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1570, 168);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellValueChanged += new ColumnActionEventHandler(grd_CellValueChanged);
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.UpdatingCell += new UpdatingCellEventHandler(grd_UpdatingCell);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.ctrlGrdBar1);
		this.panel13.Controls.Add(this.label47);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1570, 24);
		this.panel13.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1530, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.White;
		this.label47.Location = new System.Drawing.Point(3, 4);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(135, 14);
		this.label47.TabIndex = 26;
		this.label47.Text = "This Grid show Detail";
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Controls.Add(this.txtwtcuttotal);
		this.panel5.Controls.Add(this.txtwtcut);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1570, 121);
		this.panel5.TabIndex = 0;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.txtEbTotalStock);
		this.groupBox3.Controls.Add(this.Add);
		this.groupBox3.Controls.Add(this.txtEbUnitStock);
		this.groupBox3.Controls.Add(this.panel11);
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Controls.Add(this.rdSearchByCode);
		this.groupBox3.Controls.Add(this.rdSearchByName);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.lblStock);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNo);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseTo);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseName);
		this.groupBox3.Controls.Add(this.label33);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandItemName);
		this.groupBox3.Controls.Add(this.labelWareTo);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox3.Controls.Add(this.label30);
		this.groupBox3.Controls.Add(this.txtContainerNodetail);
		this.groupBox3.Controls.Add(this.label27);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add(this.txtebt);
		this.groupBox3.Controls.Add(this.txtebu);
		this.groupBox3.Controls.Add(this.txtgwt);
		this.groupBox3.Controls.Add(this.txtqty);
		this.groupBox3.Controls.Add(this.txtadlswt);
		this.groupBox3.Controls.Add(this.txtstockwt);
		this.groupBox3.Controls.Add(this.label29);
		this.groupBox3.Controls.Add(this.txtnetwt);
		this.groupBox3.Controls.Add(this.label46);
		this.groupBox3.Controls.Add(this.label28);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.label45);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.label43);
		this.groupBox3.Controls.Add(this.label42);
		this.groupBox3.Controls.Add(this.comblabno);
		this.groupBox3.Controls.Add(this.label31);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Controls.Add(this.label61);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, 4);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1158, 113);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.txtEbTotalStock.BackColor = System.Drawing.Color.White;
		this.txtEbTotalStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotalStock.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotalStock.Location = new System.Drawing.Point(453, 86);
		this.txtEbTotalStock.Name = "txtEbTotalStock";
		this.txtEbTotalStock.Size = new System.Drawing.Size(78, 23);
		this.txtEbTotalStock.TabIndex = 17;
		this.txtEbTotalStock.Tag = "EbTotalStock";
		this.txtEbTotalStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbTotalStock.TextChanged += new System.EventHandler(txtEbTotalStock_TextChanged);
		this.txtEbTotalStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbTotalStock_KeyPress);
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(1090, 86);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(39, 23);
		this.Add.TabIndex = 23;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.txtEbUnitStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnitStock.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnitStock.Location = new System.Drawing.Point(383, 86);
		this.txtEbUnitStock.Name = "txtEbUnitStock";
		this.txtEbUnitStock.Size = new System.Drawing.Size(69, 23);
		this.txtEbUnitStock.TabIndex = 16;
		this.txtEbUnitStock.Tag = "EbUnitStock";
		this.txtEbUnitStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbUnitStock.TextChanged += new System.EventHandler(txtEbUnitStock_TextChanged);
		this.txtEbUnitStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnitStock_KeyPress);
		this.panel11.Controls.Add(this.RadBrandName);
		this.panel11.Controls.Add(this.RadBrandCode);
		this.panel11.Location = new System.Drawing.Point(481, 14);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(118, 23);
		this.panel11.TabIndex = 57;
		this.RadBrandName.AutoSize = true;
		this.RadBrandName.Checked = true;
		this.RadBrandName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadBrandName.Location = new System.Drawing.Point(3, 2);
		this.RadBrandName.Name = "RadBrandName";
		this.RadBrandName.Size = new System.Drawing.Size(57, 19);
		this.RadBrandName.TabIndex = 55;
		this.RadBrandName.TabStop = true;
		this.RadBrandName.Text = "Name";
		this.RadBrandName.UseVisualStyleBackColor = true;
		this.RadBrandName.CheckedChanged += new System.EventHandler(RadBrandName_CheckedChanged);
		this.RadBrandCode.AutoSize = true;
		this.RadBrandCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadBrandCode.Location = new System.Drawing.Point(63, 2);
		this.RadBrandCode.Name = "RadBrandCode";
		this.RadBrandCode.Size = new System.Drawing.Size(52, 19);
		this.RadBrandCode.TabIndex = 56;
		this.RadBrandCode.TabStop = true;
		this.RadBrandCode.Text = "Code";
		this.RadBrandCode.UseVisualStyleBackColor = true;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(453, 67);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(79, 15);
		this.label12.TabIndex = 60;
		this.label12.Text = "EbTotal Stock";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(323, 16);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 56;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(270, 16);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 55;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(378, 67);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(75, 15);
		this.label62.TabIndex = 61;
		this.label62.Text = "EbUnit Stock";
		this.lblStock.AutoSize = true;
		this.lblStock.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblStock.Location = new System.Drawing.Point(375, 18);
		this.lblStock.Name = "lblStock";
		this.lblStock.Size = new System.Drawing.Size(37, 15);
		this.lblStock.TabIndex = 45;
		this.lblStock.Text = "Stock";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1090, 64);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(63, 21);
		this.btnCancelUpdateDetial.TabIndex = 25;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(651, 38);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(89, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 4;
		this.CmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance137;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Location = new System.Drawing.Point(3, 38);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Name = "CmbOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Size = new System.Drawing.Size(70, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Leave += new System.EventHandler(comborderno_Leave);
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance75).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(1002, 38);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(74, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(combpckuom_Leave);
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance164;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance171).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(804, 84);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 21;
		this.CmbWareHouseTo.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouseTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseTo).Location = new System.Drawing.Point(947, 84);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseTo).Name = "CmbWareHouseTo";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseTo).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseTo).TabIndex = 22;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseTo).Leave += new System.EventHandler(combwr_Leave);
		this.CmbWareHouseName.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance25;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouseName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Location = new System.Drawing.Point(74, 38);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Name = "CmbWareHouseName";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Leave += new System.EventHandler(combwr_Leave);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(623, 67);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(77, 15);
		this.label33.TabIndex = 0;
		this.label33.Text = "Container No";
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance177;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance98).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(898, 38);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(103, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 6;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance196).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(741, 38);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(156, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Leave += new System.EventHandler(combjoblot_Leave);
		this.CmbBrandItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance209).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance211).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBrandItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).Location = new System.Drawing.Point(443, 38);
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).Name = "CmbBrandItemName";
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).TabIndex = 3;
		this.CmbBrandItemName.ValueChanged += new System.EventHandler(combitem_Leave);
		this.labelWareTo.AutoSize = true;
		this.labelWareTo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelWareTo.Location = new System.Drawing.Point(947, 67);
		this.labelWareTo.Name = "labelWareTo";
		this.labelWareTo.Size = new System.Drawing.Size(83, 15);
		this.labelWareTo.TabIndex = 41;
		this.labelWareTo.Text = "Warehouse To";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance203;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(235, 38);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 2;
		this.CmbItemName.ValueChanged += new System.EventHandler(combitem_Leave);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(74, 18);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(67, 15);
		this.label30.TabIndex = 41;
		this.label30.Text = "Warehouse";
		this.txtContainerNodetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainerNodetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtContainerNodetail.Location = new System.Drawing.Point(623, 86);
		this.txtContainerNodetail.Name = "txtContainerNodetail";
		this.txtContainerNodetail.Size = new System.Drawing.Size(105, 23);
		this.txtContainerNodetail.TabIndex = 19;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(741, 18);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(48, 15);
		this.label27.TabIndex = 41;
		this.label27.Text = "Job/Lot";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(651, 18);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(57, 15);
		this.label22.TabIndex = 41;
		this.label22.Text = "Crop Year";
		this.txtebt.BackColor = System.Drawing.Color.White;
		this.txtebt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtebt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtebt.Location = new System.Drawing.Point(143, 86);
		this.txtebt.Name = "txtebt";
		this.txtebt.Size = new System.Drawing.Size(70, 23);
		this.txtebt.TabIndex = 11;
		this.txtebt.Tag = "PmTotal";
		this.txtebt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtebt.TextChanged += new System.EventHandler(txtebt_TextChanged);
		this.txtebt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtebt_KeyPress);
		this.txtebu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtebu.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtebu.Location = new System.Drawing.Point(87, 86);
		this.txtebu.Name = "txtebu";
		this.txtebu.Size = new System.Drawing.Size(55, 23);
		this.txtebu.TabIndex = 10;
		this.txtebu.Tag = "PmUnit";
		this.txtebu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtebu.TextChanged += new System.EventHandler(txtebu_TextChanged);
		this.txtebu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtebu_KeyPress);
		this.txtgwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgwt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgwt.Location = new System.Drawing.Point(3, 86);
		this.txtgwt.Name = "txtgwt";
		this.txtgwt.Size = new System.Drawing.Size(83, 23);
		this.txtgwt.TabIndex = 9;
		this.txtgwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtgwt.TextChanged += new System.EventHandler(txtgwt_TextChanged);
		this.txtgwt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgwt_KeyPress);
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(1077, 40);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(76, 23);
		this.txtqty.TabIndex = 8;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtadlswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtadlswt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtadlswt.Location = new System.Drawing.Point(214, 86);
		this.txtadlswt.Name = "txtadlswt";
		this.txtadlswt.Size = new System.Drawing.Size(72, 23);
		this.txtadlswt.TabIndex = 14;
		this.txtadlswt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtadlswt.TextChanged += new System.EventHandler(txtadlswt_TextChanged_1);
		this.txtadlswt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtadlswt_KeyPress);
		this.txtadlswt.Leave += new System.EventHandler(txtadlswt_Leave);
		this.txtstockwt.BackColor = System.Drawing.Color.White;
		this.txtstockwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtstockwt.Enabled = false;
		this.txtstockwt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtstockwt.Location = new System.Drawing.Point(532, 86);
		this.txtstockwt.Name = "txtstockwt";
		this.txtstockwt.Size = new System.Drawing.Size(90, 23);
		this.txtstockwt.TabIndex = 18;
		this.txtstockwt.TabStop = false;
		this.txtstockwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(532, 67);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(79, 15);
		this.label29.TabIndex = 25;
		this.label29.Text = "Stock Weight";
		this.txtnetwt.BackColor = System.Drawing.Color.White;
		this.txtnetwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtnetwt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtnetwt.Location = new System.Drawing.Point(287, 86);
		this.txtnetwt.Name = "txtnetwt";
		this.txtnetwt.ReadOnly = true;
		this.txtnetwt.Size = new System.Drawing.Size(95, 23);
		this.txtnetwt.TabIndex = 15;
		this.txtnetwt.TabStop = false;
		this.txtnetwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(287, 67);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(87, 15);
		this.label46.TabIndex = 25;
		this.label46.Text = "Net Bill Weight";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(804, 67);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(56, 15);
		this.label28.TabIndex = 25;
		this.label28.Text = "City/Area";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(214, 67);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(71, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "Add Weight";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(143, 67);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(52, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "E.b Total";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(87, 67);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(56, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "E.b / Unit";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(3, 67);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(78, 15);
		this.label43.TabIndex = 23;
		this.label43.Text = "Gross Weight";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(1002, 18);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(35, 15);
		this.label42.TabIndex = 21;
		this.label42.Text = "UOM";
		this.comblabno.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.comblabno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comblabno.FormattingEnabled = true;
		this.comblabno.Location = new System.Drawing.Point(729, 85);
		this.comblabno.Name = "comblabno";
		this.comblabno.Size = new System.Drawing.Size(74, 24);
		this.comblabno.TabIndex = 20;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(729, 67);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(76, 15);
		this.label31.TabIndex = 17;
		this.label31.Text = "Lab Sample#";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(1077, 18);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(26, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Qty";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(443, 18);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(38, 15);
		this.label61.TabIndex = 8;
		this.label61.Text = "Brand";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(898, 18);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(77, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Packing Type";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(235, 18);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(32, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Item";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(3, 18);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(56, 15);
		this.label37.TabIndex = 0;
		this.label37.Text = "Order No";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1090, 87);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(63, 21);
		this.btnUpdateDetail.TabIndex = 24;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtwtcuttotal.BackColor = System.Drawing.Color.White;
		this.txtwtcuttotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtcuttotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtwtcuttotal.Location = new System.Drawing.Point(581, 154);
		this.txtwtcuttotal.Name = "txtwtcuttotal";
		this.txtwtcuttotal.ReadOnly = true;
		this.txtwtcuttotal.Size = new System.Drawing.Size(75, 23);
		this.txtwtcuttotal.TabIndex = 13;
		this.txtwtcuttotal.TabStop = false;
		this.txtwtcuttotal.Visible = false;
		this.txtwtcut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtcut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtwtcut.Location = new System.Drawing.Point(525, 154);
		this.txtwtcut.Name = "txtwtcut";
		this.txtwtcut.Size = new System.Drawing.Size(55, 23);
		this.txtwtcut.TabIndex = 12;
		this.txtwtcut.Visible = false;
		this.txtwtcut.TextChanged += new System.EventHandler(txtebu_TextChanged);
		this.txtwtcut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtwtcut_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(525, 135);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 15);
		this.label3.TabIndex = 43;
		this.label3.Text = "Wt/Cut";
		this.label3.Visible = false;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(586, 135);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(54, 15);
		this.label9.TabIndex = 44;
		this.label9.Text = "Cut Total";
		this.label9.Visible = false;
		this.tabPage4.Controls.Add(this.panel6);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1576, 319);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Expenses";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.GridExpense);
		this.panel6.Controls.Add(this.panel12);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1570, 313);
		this.panel6.TabIndex = 0;
		this.GridExpense.AutomaticSort = false;
		((ScrollableBase)this.GridExpense).BackColor = System.Drawing.Color.White;
		this.GridExpense.ColumnAutoResize = true;
		GridExpense_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridExpense.DesignTimeLayout = GridExpense_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridExpense).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GridExpense).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridExpense.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridExpense).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.GridExpense).Name = "GridExpense";
		((ScrollableBase)this.GridExpense).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridExpense).Size = new System.Drawing.Size(1570, 291);
		((System.Windows.Forms.Control)(object)this.GridExpense).TabIndex = 5;
		this.GridExpense.TotalRow = (InheritableBoolean)1;
		this.GridExpense.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridExpense).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel12.Controls.Add(this.label52);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Margin = new System.Windows.Forms.Padding(2);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1570, 22);
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
		this.panel4.Controls.Add(this.label34);
		this.panel4.Controls.Add(this.txtPartyCount);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combbrnch);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combproject);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combstockprty);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combcompany);
		this.panel4.Controls.Add(this.txtrefno);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtsuppwt);
		this.panel4.Controls.Add(this.txtsuppref);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.txtwtdiff);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1584, 160);
		this.panel4.TabIndex = 1;
		this.groupBox1.Controls.Add(this.ChkIsStockReservedPerParty);
		this.groupBox1.Controls.Add(this.txtDeliverTerm);
		this.groupBox1.Controls.Add(this.txtShortExcessWeight);
		this.groupBox1.Controls.Add(this.label60);
		this.groupBox1.Controls.Add(this.txtTotalDoWeight);
		this.groupBox1.Controls.Add(this.label59);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.txtAddLessHeader);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.txtcarramount);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtTicketNos);
		this.groupBox1.Controls.Add(this.txtBalGrossWeightGrid);
		this.groupBox1.Controls.Add(this.txtfctwt);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.txtBalanceWeight);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.txtGdnGrossWeight);
		this.groupBox1.Controls.Add(this.labelAdvDoNo);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAdvanceDo);
		this.groupBox1.Controls.Add(this.txtContainerNo1);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtbltyno);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.txtContainerNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerName);
		this.groupBox1.Controls.Add(this.CmbGpNo);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransport);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.txtvehno);
		this.groupBox1.Controls.Add(this.txtSaleType);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1164, 154);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.ChkIsStockReservedPerParty.AutoSize = true;
		this.ChkIsStockReservedPerParty.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkIsStockReservedPerParty.Location = new System.Drawing.Point(1047, 71);
		this.ChkIsStockReservedPerParty.Name = "ChkIsStockReservedPerParty";
		this.ChkIsStockReservedPerParty.Size = new System.Drawing.Size(114, 20);
		this.ChkIsStockReservedPerParty.TabIndex = 18;
		this.ChkIsStockReservedPerParty.Text = "Stock Reserved";
		this.ChkIsStockReservedPerParty.UseVisualStyleBackColor = true;
		this.ChkIsStockReservedPerParty.CheckedChanged += new System.EventHandler(ChkIsStockReservedPerParty_CheckedChanged);
		this.txtDeliverTerm.BackColor = System.Drawing.SystemColors.Menu;
		this.txtDeliverTerm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliverTerm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliverTerm.Location = new System.Drawing.Point(100, 71);
		this.txtDeliverTerm.Name = "txtDeliverTerm";
		this.txtDeliverTerm.ReadOnly = true;
		this.txtDeliverTerm.Size = new System.Drawing.Size(120, 23);
		this.txtDeliverTerm.TabIndex = 3;
		this.txtDeliverTerm.TabStop = false;
		this.txtDeliverTerm.TextChanged += new System.EventHandler(txtDeliverTerm_TextChanged);
		this.txtShortExcessWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtShortExcessWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtShortExcessWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtShortExcessWeight.Location = new System.Drawing.Point(1064, 44);
		this.txtShortExcessWeight.Name = "txtShortExcessWeight";
		this.txtShortExcessWeight.ReadOnly = true;
		this.txtShortExcessWeight.Size = new System.Drawing.Size(97, 23);
		this.txtShortExcessWeight.TabIndex = 17;
		this.txtShortExcessWeight.TabStop = false;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(972, 48);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(94, 15);
		this.label60.TabIndex = 34;
		this.label60.Text = "Short/Excess Wt";
		this.txtTotalDoWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTotalDoWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalDoWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalDoWeight.Location = new System.Drawing.Point(1064, 18);
		this.txtTotalDoWeight.Name = "txtTotalDoWeight";
		this.txtTotalDoWeight.ReadOnly = true;
		this.txtTotalDoWeight.Size = new System.Drawing.Size(97, 23);
		this.txtTotalDoWeight.TabIndex = 16;
		this.txtTotalDoWeight.TabStop = false;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(972, 22);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(92, 15);
		this.label59.TabIndex = 32;
		this.label59.Text = "Do Item Net Wt";
		this.txtdocno.BackColor = System.Drawing.SystemColors.Menu;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(100, 18);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(77, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.txtAddLessHeader.BackColor = System.Drawing.Color.White;
		this.txtAddLessHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLessHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAddLessHeader.Location = new System.Drawing.Point(839, 44);
		this.txtAddLessHeader.Name = "txtAddLessHeader";
		this.txtAddLessHeader.Size = new System.Drawing.Size(130, 23);
		this.txtAddLessHeader.TabIndex = 17;
		this.txtAddLessHeader.Visible = false;
		this.txtAddLessHeader.Leave += new System.EventHandler(txtAddLessHeader_TextChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 22);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No && Date";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(748, 48);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(54, 15);
		this.label49.TabIndex = 31;
		this.label49.Text = "Add Less";
		this.label49.Visible = false;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(321, 75);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(49, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Bilty No";
		this.txtcarramount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcarramount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcarramount.Location = new System.Drawing.Point(416, 18);
		this.txtcarramount.Name = "txtcarramount";
		this.txtcarramount.Size = new System.Drawing.Size(118, 23);
		this.txtcarramount.TabIndex = 6;
		this.txtcarramount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcarramount_KeyPress);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(748, 22);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(90, 15);
		this.label50.TabIndex = 28;
		this.label50.Text = "W.B Ticket Nos.";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(748, 75);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(77, 15);
		this.label17.TabIndex = 28;
		this.label17.Text = "Bal Gross Wt ";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(180, 18);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(135, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.txtTicketNos.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTicketNos.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTicketNos.Location = new System.Drawing.Point(839, 18);
		this.txtTicketNos.Name = "txtTicketNos";
		this.txtTicketNos.ReadOnly = true;
		this.txtTicketNos.Size = new System.Drawing.Size(130, 23);
		this.txtTicketNos.TabIndex = 16;
		this.txtTicketNos.TabStop = false;
		this.txtBalGrossWeightGrid.BackColor = System.Drawing.SystemColors.Menu;
		this.txtBalGrossWeightGrid.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBalGrossWeightGrid.Location = new System.Drawing.Point(839, 71);
		this.txtBalGrossWeightGrid.Name = "txtBalGrossWeightGrid";
		this.txtBalGrossWeightGrid.ReadOnly = true;
		this.txtBalGrossWeightGrid.Size = new System.Drawing.Size(130, 23);
		this.txtBalGrossWeightGrid.TabIndex = 18;
		this.txtBalGrossWeightGrid.TabStop = false;
		this.txtfctwt.BackColor = System.Drawing.SystemColors.Menu;
		this.txtfctwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfctwt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtfctwt.Location = new System.Drawing.Point(637, 18);
		this.txtfctwt.Name = "txtfctwt";
		this.txtfctwt.ReadOnly = true;
		this.txtfctwt.Size = new System.Drawing.Size(107, 23);
		this.txtfctwt.TabIndex = 11;
		this.txtfctwt.TabStop = false;
		this.txtfctwt.TextChanged += new System.EventHandler(txtfctwt_TextChanged);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(538, 75);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(62, 15);
		this.label21.TabIndex = 25;
		this.label21.Text = "BalWeight";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(321, 22);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(94, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Freight  Amount";
		this.txtBalanceWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtBalanceWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBalanceWeight.Location = new System.Drawing.Point(637, 71);
		this.txtBalanceWeight.Name = "txtBalanceWeight";
		this.txtBalanceWeight.ReadOnly = true;
		this.txtBalanceWeight.Size = new System.Drawing.Size(107, 23);
		this.txtBalanceWeight.TabIndex = 13;
		this.txtBalanceWeight.TabStop = false;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(321, 101);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(63, 15);
		this.label16.TabIndex = 0;
		this.label16.Text = "Container1";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(538, 48);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(96, 15);
		this.label14.TabIndex = 23;
		this.label14.Text = "Dispatch Weight";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(321, 128);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(68, 15);
		this.label32.TabIndex = 3;
		this.label32.Text = "Container 2";
		this.txtGdnGrossWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtGdnGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGdnGrossWeight.Location = new System.Drawing.Point(637, 44);
		this.txtGdnGrossWeight.Name = "txtGdnGrossWeight";
		this.txtGdnGrossWeight.ReadOnly = true;
		this.txtGdnGrossWeight.Size = new System.Drawing.Size(107, 23);
		this.txtGdnGrossWeight.TabIndex = 12;
		this.txtGdnGrossWeight.TabStop = false;
		this.labelAdvDoNo.AutoSize = true;
		this.labelAdvDoNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelAdvDoNo.Location = new System.Drawing.Point(972, 101);
		this.labelAdvDoNo.Name = "labelAdvDoNo";
		this.labelAdvDoNo.Size = new System.Drawing.Size(98, 15);
		this.labelAdvDoNo.TabIndex = 0;
		this.labelAdvDoNo.Text = "Advance D.O No.";
		this.labelAdvDoNo.Visible = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(321, 48);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(64, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Vehicle No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 101);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Sale Type";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(5, 67);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(82, 30);
		this.label15.TabIndex = 0;
		this.label15.Text = "Order Term && \r\nGate Pass";
		this.CmbAdvanceDo.AllowNull = (DefaultableBoolean)1;
		this.CmbAdvanceDo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAdvanceDo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAdvanceDo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance91).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAdvanceDo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAdvanceDo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbAdvanceDo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAdvanceDo).Location = new System.Drawing.Point(972, 122);
		((System.Windows.Forms.Control)(object)this.CmbAdvanceDo).Name = "CmbAdvanceDo";
		((System.Windows.Forms.Control)(object)this.CmbAdvanceDo).Size = new System.Drawing.Size(186, 26);
		((System.Windows.Forms.Control)(object)this.CmbAdvanceDo).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.CmbAdvanceDo).Visible = false;
		this.txtContainerNo1.BackColor = System.Drawing.Color.White;
		this.txtContainerNo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainerNo1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtContainerNo1.Location = new System.Drawing.Point(416, 124);
		this.txtContainerNo1.Name = "txtContainerNo1";
		this.txtContainerNo1.ReadOnly = true;
		this.txtContainerNo1.Size = new System.Drawing.Size(118, 23);
		this.txtContainerNo1.TabIndex = 10;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(5, 128);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(67, 15);
		this.label48.TabIndex = 8;
		this.label48.Text = "Transporter";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 48);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Customer Name";
		this.txtbltyno.BackColor = System.Drawing.Color.White;
		this.txtbltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbltyno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbltyno.Location = new System.Drawing.Point(416, 71);
		this.txtbltyno.Name = "txtbltyno";
		this.txtbltyno.ReadOnly = true;
		this.txtbltyno.Size = new System.Drawing.Size(118, 23);
		this.txtbltyno.TabIndex = 8;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(538, 101);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Remarks";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtContainerNo.BackColor = System.Drawing.Color.White;
		this.txtContainerNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainerNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtContainerNo.Location = new System.Drawing.Point(416, 97);
		this.txtContainerNo.Name = "txtContainerNo";
		this.txtContainerNo.ReadOnly = true;
		this.txtContainerNo.Size = new System.Drawing.Size(118, 23);
		this.txtContainerNo.TabIndex = 9;
		this.CmbCustomerName.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance100;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance105).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Location = new System.Drawing.Point(100, 42);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Name = "CmbCustomerName";
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Leave += new System.EventHandler(combsupplier_Leave);
		this.CmbGpNo.BackColor = System.Drawing.SystemColors.Menu;
		this.CmbGpNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.CmbGpNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGpNo.Location = new System.Drawing.Point(221, 71);
		this.CmbGpNo.Name = "CmbGpNo";
		this.CmbGpNo.Size = new System.Drawing.Size(94, 23);
		this.CmbGpNo.TabIndex = 4;
		this.CmbGpNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(combgatepass_KeyPress);
		this.CmbGpNo.Leave += new System.EventHandler(combgatepass_Leave);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(538, 22);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(85, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "FactoryWeight";
		this.CmbTransport.AllowNull = (DefaultableBoolean)1;
		this.CmbTransport.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbTransport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance216;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransport.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Location = new System.Drawing.Point(100, 122);
		((System.Windows.Forms.Control)(object)this.CmbTransport).Name = "CmbTransport";
		((System.Windows.Forms.Control)(object)this.CmbTransport).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransport).TabIndex = 5;
		this.txtremarks.BackColor = System.Drawing.Color.White;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(637, 97);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(332, 51);
		this.txtremarks.TabIndex = 15;
		this.txtvehno.BackColor = System.Drawing.Color.White;
		this.txtvehno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvehno.Location = new System.Drawing.Point(416, 44);
		this.txtvehno.Name = "txtvehno";
		this.txtvehno.ReadOnly = true;
		this.txtvehno.Size = new System.Drawing.Size(118, 23);
		this.txtvehno.TabIndex = 7;
		this.txtSaleType.BackColor = System.Drawing.SystemColors.Menu;
		this.txtSaleType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSaleType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleType.Location = new System.Drawing.Point(100, 96);
		this.txtSaleType.Name = "txtSaleType";
		this.txtSaleType.ReadOnly = true;
		this.txtSaleType.Size = new System.Drawing.Size(215, 23);
		this.txtSaleType.TabIndex = 36;
		this.txtSaleType.TabStop = false;
		this.label34.AutoSize = true;
		this.label34.Enabled = false;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(1088, 11);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(81, 15);
		this.label34.TabIndex = 26;
		this.label34.Text = "Pending Party";
		this.label34.Visible = false;
		this.txtPartyCount.BackColor = System.Drawing.Color.White;
		this.txtPartyCount.Enabled = false;
		this.txtPartyCount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPartyCount.Location = new System.Drawing.Point(1172, 9);
		this.txtPartyCount.Name = "txtPartyCount";
		this.txtPartyCount.ReadOnly = true;
		this.txtPartyCount.Size = new System.Drawing.Size(54, 23);
		this.txtPartyCount.TabIndex = 21;
		this.txtPartyCount.TabStop = false;
		this.txtPartyCount.Visible = false;
		this.combbrnch.AllowNull = (DefaultableBoolean)1;
		this.combbrnch.AutoCompleteMode = (AutoCompleteMode)4;
		this.combbrnch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combbrnch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combbrnch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.combbrnch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combbrnch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.combbrnch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combbrnch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).TextHAlignAsString = "Left";
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.combbrnch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combbrnch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combbrnch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combbrnch).Enabled = false;
		((System.Windows.Forms.Control)(object)this.combbrnch).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combbrnch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combbrnch).Location = new System.Drawing.Point(1299, 73);
		((System.Windows.Forms.Control)(object)this.combbrnch).Name = "combbrnch";
		((System.Windows.Forms.Control)(object)this.combbrnch).Size = new System.Drawing.Size(51, 26);
		((System.Windows.Forms.Control)(object)this.combbrnch).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combbrnch).Visible = false;
		this.combproject.AllowNull = (DefaultableBoolean)1;
		this.combproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.combproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.combproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.combproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.combproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.combproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance195).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.combproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.combproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combproject).Enabled = false;
		((System.Windows.Forms.Control)(object)this.combproject).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combproject).Location = new System.Drawing.Point(1299, 99);
		((System.Windows.Forms.Control)(object)this.combproject).Name = "combproject";
		((System.Windows.Forms.Control)(object)this.combproject).Size = new System.Drawing.Size(51, 26);
		((System.Windows.Forms.Control)(object)this.combproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combproject).Visible = false;
		this.combstockprty.AllowNull = (DefaultableBoolean)1;
		this.combstockprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.combstockprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combstockprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combstockprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.combstockprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combstockprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.combstockprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combstockprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance118).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.combstockprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combstockprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combstockprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combstockprty).Enabled = false;
		((System.Windows.Forms.Control)(object)this.combstockprty).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combstockprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combstockprty).Location = new System.Drawing.Point(1172, 83);
		((System.Windows.Forms.Control)(object)this.combstockprty).Name = "combstockprty";
		((System.Windows.Forms.Control)(object)this.combstockprty).Size = new System.Drawing.Size(54, 26);
		((System.Windows.Forms.Control)(object)this.combstockprty).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.combstockprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combstockprty).Visible = false;
		this.combcompany.AllowNull = (DefaultableBoolean)1;
		this.combcompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.combcompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.combcompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.combcompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance132).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.combcompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcompany).Enabled = false;
		((System.Windows.Forms.Control)(object)this.combcompany).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combcompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcompany).Location = new System.Drawing.Point(1172, 116);
		((System.Windows.Forms.Control)(object)this.combcompany).Name = "combcompany";
		((System.Windows.Forms.Control)(object)this.combcompany).Size = new System.Drawing.Size(54, 26);
		((System.Windows.Forms.Control)(object)this.combcompany).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combcompany).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combcompany).Visible = false;
		this.txtrefno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtrefno.Enabled = false;
		this.txtrefno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtrefno.Location = new System.Drawing.Point(1172, 62);
		this.txtrefno.Name = "txtrefno";
		this.txtrefno.Size = new System.Drawing.Size(54, 23);
		this.txtrefno.TabIndex = 9;
		this.txtrefno.TabStop = false;
		this.txtrefno.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Enabled = false;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1088, 39);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(71, 15);
		this.label4.TabIndex = 0;
		this.label4.Text = "Supplier Ref";
		this.label4.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Enabled = false;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(1088, 125);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(58, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Company";
		this.label1.Visible = false;
		this.txtsuppwt.BackColor = System.Drawing.Color.White;
		this.txtsuppwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsuppwt.Enabled = false;
		this.txtsuppwt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtsuppwt.Location = new System.Drawing.Point(1299, 13);
		this.txtsuppwt.Name = "txtsuppwt";
		this.txtsuppwt.ReadOnly = true;
		this.txtsuppwt.Size = new System.Drawing.Size(51, 23);
		this.txtsuppwt.TabIndex = 2;
		this.txtsuppwt.TabStop = false;
		this.txtsuppwt.Visible = false;
		this.txtsuppwt.TextChanged += new System.EventHandler(txtsuppwt_TextChanged);
		this.txtsuppref.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsuppref.Enabled = false;
		this.txtsuppref.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtsuppref.Location = new System.Drawing.Point(1172, 35);
		this.txtsuppref.Name = "txtsuppref";
		this.txtsuppref.Size = new System.Drawing.Size(54, 23);
		this.txtsuppref.TabIndex = 8;
		this.txtsuppref.TabStop = false;
		this.txtsuppref.Visible = false;
		this.label6.AutoSize = true;
		this.label6.Enabled = false;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(1232, 13);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(72, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Cust Weight";
		this.label6.Visible = false;
		this.label13.AutoSize = true;
		this.label13.Enabled = false;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(1232, 69);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(44, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch";
		this.label13.Visible = false;
		this.label8.AutoSize = true;
		this.label8.Enabled = false;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1088, 85);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Stock Party";
		this.label8.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Enabled = false;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1232, 40);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 5;
		this.label11.Text = "Weight Diff";
		this.label11.Visible = false;
		this.label10.AutoSize = true;
		this.label10.Enabled = false;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(1088, 66);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(78, 15);
		this.label10.TabIndex = 8;
		this.label10.Text = "Reference No";
		this.label10.Visible = false;
		this.txtwtdiff.BackColor = System.Drawing.Color.White;
		this.txtwtdiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtdiff.Enabled = false;
		this.txtwtdiff.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtwtdiff.Location = new System.Drawing.Point(1299, 40);
		this.txtwtdiff.Name = "txtwtdiff";
		this.txtwtdiff.ReadOnly = true;
		this.txtwtdiff.Size = new System.Drawing.Size(51, 23);
		this.txtwtdiff.TabIndex = 4;
		this.txtwtdiff.TabStop = false;
		this.txtwtdiff.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Enabled = false;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1232, 96);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(44, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Project";
		this.label18.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.chkShortWeightLessFromNetWeight);
		this.panel3.Controls.Add(this.chkPropBalGrossInGrid);
		this.panel3.Controls.Add(this.btnRecordsUpdate);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 33);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1584, 24);
		this.panel3.TabIndex = 0;
		this.chkShortWeightLessFromNetWeight.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkShortWeightLessFromNetWeight.AutoSize = true;
		this.chkShortWeightLessFromNetWeight.BackColor = System.Drawing.Color.Transparent;
		this.chkShortWeightLessFromNetWeight.Cursor = System.Windows.Forms.Cursors.Arrow;
		this.chkShortWeightLessFromNetWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkShortWeightLessFromNetWeight.ForeColor = System.Drawing.Color.White;
		this.chkShortWeightLessFromNetWeight.Location = new System.Drawing.Point(1186, 3);
		this.chkShortWeightLessFromNetWeight.Margin = new System.Windows.Forms.Padding(2);
		this.chkShortWeightLessFromNetWeight.Name = "chkShortWeightLessFromNetWeight";
		this.chkShortWeightLessFromNetWeight.Size = new System.Drawing.Size(300, 19);
		this.chkShortWeightLessFromNetWeight.TabIndex = 115;
		this.chkShortWeightLessFromNetWeight.Text = "Click To Less Short Weight From Net Weight In Grid";
		this.chkShortWeightLessFromNetWeight.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
		this.chkShortWeightLessFromNetWeight.UseVisualStyleBackColor = false;
		this.chkShortWeightLessFromNetWeight.Visible = false;
		this.chkShortWeightLessFromNetWeight.CheckedChanged += new System.EventHandler(chkShortWeightLessFromNetWeight_CheckedChanged);
		this.chkPropBalGrossInGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPropBalGrossInGrid.AutoSize = true;
		this.chkPropBalGrossInGrid.BackColor = System.Drawing.Color.Transparent;
		this.chkPropBalGrossInGrid.Checked = true;
		this.chkPropBalGrossInGrid.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkPropBalGrossInGrid.Cursor = System.Windows.Forms.Cursors.Arrow;
		this.chkPropBalGrossInGrid.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPropBalGrossInGrid.ForeColor = System.Drawing.Color.White;
		this.chkPropBalGrossInGrid.Location = new System.Drawing.Point(944, 2);
		this.chkPropBalGrossInGrid.Margin = new System.Windows.Forms.Padding(2);
		this.chkPropBalGrossInGrid.Name = "chkPropBalGrossInGrid";
		this.chkPropBalGrossInGrid.Size = new System.Drawing.Size(241, 19);
		this.chkPropBalGrossInGrid.TabIndex = 19;
		this.chkPropBalGrossInGrid.Text = "Click To Prpotionate Bal Gross Wt In Grid";
		this.chkPropBalGrossInGrid.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
		this.chkPropBalGrossInGrid.UseVisualStyleBackColor = false;
		this.chkPropBalGrossInGrid.Visible = false;
		this.chkPropBalGrossInGrid.Click += new System.EventHandler(chkPropBalGrossInGrid_Click);
		this.btnRecordsUpdate.BackColor = System.Drawing.Color.FromArgb(192, 0, 0);
		this.btnRecordsUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Black;
		this.btnRecordsUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRecordsUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRecordsUpdate.ForeColor = System.Drawing.Color.White;
		this.btnRecordsUpdate.Location = new System.Drawing.Point(195, 0);
		this.btnRecordsUpdate.Name = "btnRecordsUpdate";
		this.btnRecordsUpdate.Size = new System.Drawing.Size(159, 24);
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
		this.label23.Size = new System.Drawing.Size(186, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Goods Dispatch Notes";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Checked = true;
		this.ChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBox.Cursor = System.Windows.Forms.Cursors.Arrow;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1489, 3);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(84, 20);
		this.ChkBox.TabIndex = 20;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1584, 31);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[16]
		{
			this.btnNew, this.toolStripButton1, this.toolStripSeparator5, this.btnUpdate, this.btnSave, this.btnDelete, this.toolStripSeparator4, this.btnAttachment, this.toolStripSeparator3, this.printToolStripButton,
			this.btnPrint260A, this.toolStripSeparator2, this.btnStockReport, this.BtnShortCutkeys, this.toolStripSeparator1, this.btnGenerateAvailableStock
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1584, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.ToolTipText = "Ctrl_N";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.ToolTipText = "Alt_R";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
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
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_delete_bin_16;
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
		this.printToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.printToolStripButton.Name = "printToolStripButton";
		this.printToolStripButton.Size = new System.Drawing.Size(92, 24);
		this.printToolStripButton.Text = "260-Print";
		this.printToolStripButton.Click += new System.EventHandler(printToolStripButton_Click);
		this.btnPrint260A.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint260A.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint260A.Name = "btnPrint260A";
		this.btnPrint260A.Size = new System.Drawing.Size(101, 24);
		this.btnPrint260A.Text = "260A-Print";
		this.btnPrint260A.Click += new System.EventHandler(btnPrint260A_Click);
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
		this.btnGenerateAvailableStock.Size = new System.Drawing.Size(114, 24);
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
		this.tabPage2.Size = new System.Drawing.Size(1588, 739);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(2, 152);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1584, 379);
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
		this.panel17.Controls.Add(this.ctrlGrdBar4);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel17.Location = new System.Drawing.Point(2, 531);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1584, 25);
		this.panel17.TabIndex = 48;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(7, 5);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(209, 14);
		this.label51.TabIndex = 25;
		this.label51.Text = "Detail of the Selected above row";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1536, 2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.DatagridHistoryDetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(41, 22);
		this.ctrlGrdBar4.TabIndex = 23;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.DatagridHistoryDetail.AllowEdit = (InheritableBoolean)2;
		DatagridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistoryDetail.DesignTimeLayout = DatagridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.DatagridHistoryDetail.DynamicFiltering = true;
		this.DatagridHistoryDetail.FilterMode = (FilterMode)2;
		this.DatagridHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryDetail).Location = new System.Drawing.Point(2, 556);
		((System.Windows.Forms.Control)(object)this.DatagridHistoryDetail).Name = "DatagridHistoryDetail";
		((ScrollableBase)this.DatagridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistoryDetail).Size = new System.Drawing.Size(1584, 181);
		((System.Windows.Forms.Control)(object)this.DatagridHistoryDetail).TabIndex = 1;
		this.DatagridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DatagridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label58);
		this.panel15.Controls.Add(this.ctrlGrdBar2);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(2, 127);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1584, 25);
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
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1531, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(42, 24);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel8.Controls.Add(this.groupBox2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 56);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1584, 71);
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
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1084, 66);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(948, 40);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(948, 16);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(867, 16);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(867, 40);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(796, 36);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(556, 18);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(58, 15);
		this.label53.TabIndex = 87;
		this.label53.Text = "Customer";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(448, 18);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(64, 15);
		this.label54.TabIndex = 86;
		this.label54.Text = "To Doc No";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(341, 18);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(79, 15);
		this.label55.TabIndex = 85;
		this.label55.Text = "From Doc No";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(172, 18);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(48, 15);
		this.label56.TabIndex = 84;
		this.label56.Text = "To Date";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(3, 18);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(63, 15);
		this.label57.TabIndex = 83;
		this.label57.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance170;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance179).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(559, 36);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(232, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(451, 38);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(105, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(344, 38);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(105, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(175, 38);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(167, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(6, 38);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(167, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label38);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1584, 54);
		this.panel7.TabIndex = 0;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label38.ForeColor = System.Drawing.Color.White;
		this.label38.Location = new System.Drawing.Point(5, 30);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(254, 19);
		this.label38.TabIndex = 27;
		this.label38.Text = "Goods Dispatch Notes History ";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnGdnFormHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1584, 27);
		this.toolStrip2.TabIndex = 46;
		this.toolStrip2.Text = "toolStrip2";
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
		this.btnGdnFormHistory.Click += new System.EventHandler(btnGdnFormHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1596, 768);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "InvFrmGDN";
		this.Text = "Goods Dispatch Notes";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvFrmGDN_FormClosing);
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
		this.panel13.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.tabPage4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridExpense).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAdvanceDo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combbrnch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combstockprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcompany).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistoryDetail).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
