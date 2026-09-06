using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Reports;
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
namespace Architecture.WinApp.Purchase
{

public class InvFrmGRN : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtdetail = new DataTable();

	private DataTable dtPurchaseBreakup = new DataTable();

	private DataTable dtEmptyBag = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtTransporter = new DataTable();

	private DataTable dtwh = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtjob = new DataTable();

	private DataTable dtPack = new DataTable();

	private DataTable dtcity = new DataTable();

	private DataTable dtUOM = new DataTable();

	private DataTable dtItemForEmptyBag = new DataTable();

	private DataTable dtPOEmptyBagsDetail = new DataTable();

	private DataTable dtEmpType = new DataTable();

	private DataTable dtBagsCondition = new DataTable();

	private DataTable dtPrevoiusDataOfSelectedGP = new DataTable();

	private int GPID;

	private int PurchaseOrderId;

	private int PurchaseOrderNo;

	private int FreightId;

	private int RefDocumentTypeId;

	private double GpQty;

	private double TotalGrossWeight;

	private int updateDetailIndex = -1;

	public int RecId;

	private bool FormInitialized;

	private bool RefferedInInvoice;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "";

	private bool ItemSearchWithNameOrCode;

	private bool LockBreakUp;

	private bool WagesStatus;

	private bool WagesActiveOrInActive;

	private int EmptyBagsWeightCutStatus;

	private int LabWeightCutOnId;

	private double JuteBagsCut;

	private double PPBagsCut;

	private double OpenBulkCut;

	private double JuteBagsCutStock;

	private double PPBagsCutStock;

	private double OpenBulkCutStock;

	private double BillWeightAndStockWeightDifferenceTolerance;

	private bool EmptyBagsWeightCutEditable;

	private bool AddLessWeightEditable;

	private bool EmptyBagsInformationComp;

	private bool LabCompulsoryForWeighBridgeAgainstGatePurchase;

	private bool LabCompulsoryForWeighBridgeAgainstPO;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool EBWtAccordingToGrossWeight;

	private bool DeliveryTermEditableOnGrnForGatePurchase;

	private bool WeightCutEditable;

	private bool LabCompulsoryNotCheckingOnGRN;

	private bool DeductionPolicyForGrnIsOn;

	private bool FreightVoucherNotCompulsoryBeforeGrn;

	private bool ValidateGrnAndInvoiceDateWithGpDate;

	private bool ExcludeWeightShortageBusinessOnGrn;

	private DateTime GpDate = DateTime.Now;

	private bool AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt;

	private int DefaultDaysToLessFromHistoryFromDate;

	private LoadPendingGatePassForGrn LoadGatePass;

	private DataTable dtGatePassData = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel4;

	private Panel panel5;

	private Label label24;

	private Label label25;

	private Label label21;

	private Label label19;

	private TextBox txtbltyno;

	private Label label7;

	private TextBox txtvehno;

	internal UltraCombo CmbTransport;

	private Label label48;

	private TextBox txtcarramount;

	private Label label20;

	private TextBox txtsuppwt;

	private Label label6;

	private TextBox txtfctwt;

	private Label lblRecwt;

	private TextBox txtBalWeight;

	internal UltraCombo combsupplier;

	private Label label2;

	private Label label10;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private Label label5;

	internal UltraCombo CmbPackUom;

	internal UltraCombo CmbCity;

	internal UltraCombo CmbWareHouseName;

	internal UltraCombo CmbPackingType;

	internal UltraCombo CmbJobLot;

	internal UltraCombo combitem;

	private Label label30;

	private Label label27;

	private Label label22;

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

	private Label label41;

	private Label label40;

	private Label label36;

	private Panel panel7;

	internal GridEX GrdHistory;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo CmbCropYear;

	private Label label9;

	private Label label3;

	private TextBox txtwtcuttotal;

	private TextBox txtwtcut;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Label label15;

	private TextBox combgatepass;

	private Label label16;

	private TextBox txtEbWtOther;

	private Panel panel10;

	internal GridEX grd;

	private CheckBox ChkBox;

	private TextBox txtEmptyBagsTotal;

	private Panel panel6;

	internal GridEX grdGp;

	private Button Add;

	internal GridEX GrdHistoryDetail;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStrip toolStrip2;

	internal UltraCombo combvehtyp;

	internal GridEX grdEmptyBags;

	private TextBox txtGrnWeight;

	private Panel panel9;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar3;

	private TextBox txtAnaylstName;

	private ToolStripButton btnGrnFormHistory;

	private TextBox txtReceivedWeight;

	private Label lblBalwt;

	private CtrlGrdBar ctrlGrdBar4;

	private Panel panel13;

	private Panel panel12;

	private Panel panel15;

	private Panel panel14;

	private CtrlGrdBar ctrlGrdBar5;

	private Panel panel16;

	private Panel panel17;

	private Panel panel18;

	private Panel panel20;

	private Label label4;

	private Label label11;

	private Label label14;

	private GroupBox groupBox2;

	private GroupBox groupBox1;

	private TextBox txtTicketnos;

	private Label label18;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton BtnPrint;

	private ToolStripButton btnAttachment;

	private ToolStripButton BtnRefresh;

	private ToolStripButton btnDelete;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton CityDefine;

	private Label label13;

	private Label label32;

	private Label label23;

	private ToolStripButton BtnShortCutkeys;

	private Label label33;

	private TextBox txtMoisture;

	private Label label34;

	private Panel PanelBreakupMain;

	internal GridEX grdPurchaseBrakup;

	private Panel panel22;

	private Label label38;

	private Label label47;

	private Label txtAcceptedGpTotal;

	private Label txtOpenGpTotal;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnSlip257;

	private Label label39;

	private TextBox txtDifferenceWeight;

	private Label lblDiffweight;

	private Panel panel23;

	private GroupBox groupBox3;

	private Button btnshow;

	private Label label49;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label53;

	internal UltraCombo cmbSupplierNameHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton RadNotReffered;

	private RadioButton RadReffered;

	private RadioButton RadAll;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private ToolStripButton BtnUpdateEBTotalWithGrossWeight;

	internal UltraCombo txtDeliverTerm;

	private CheckBox chkSuppShortWeight;

	private CheckBox chkScaleShortWeightApply;

	private TabControl tabControl2;

	private TabPage tabDetailH;

	private TabPage tabEBH;

	internal GridEX grdHistoryEB;

	private Panel panel11;

	private Label label54;

	private Label label55;

	private Label label56;

	internal UltraCombo CmbLabNo;

	private TextBox txtWtCutOn;

	private Label label57;

	internal GridEX grdLabInfo;

	private TextBox txtremarks;

	private TextBox txtAccessWeight;

	private Label labelExcessWt;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	internal UltraCombo CmbSupplierDispatchedPreBillNo;

	private Label label8;

	private TextBox txtEbUnitStock;

	private TextBox txtEbTotalStock;

	private Label label59;

	private Label label12;

	private TextBox txtFreightDeduction;

	private Panel PanelCurrentLabResult;

	private Panel panel19;

	private Label label58;

	private CheckBox chkLabWtcutcompulsory;

	private TextBox txtLabWeightCutValue;

	private Label label60;

	private CheckBox chkLabRatecutcompulsory;

	private TextBox txtLabRateCutValue;

	private Button btnLoadGatePass;

	private Label label61;

	private GroupBox groupBox4;

	private Label label64;

	private Label label63;

	private TextBox txtAdvanceByFactory;

	private TextBox txtAdvanceByParty;

	private TextBox txtScaleCharges;

	private Label label26;

	private TextBox txtBiltyFreight;

	private Label label17;

	private Label label65;

	private TextBox txtTotalFreight;

	private TextBox txtShortageAmountFr;

	private Label label66;

	private TextBox txtDiscountAmountFr;

	private Label label67;

	private TextBox txtChargeToPartyAmountFr;

	private Label label68;

	private Label label69;

	private Label label31;

	private TextBox txtRemainingAmountFr;

	private Label label70;

	private TextBox txtOtherDeduction;

	private Label label1;

	private TextBox textBox1;

	private Label label37;

	public event Action FormInitializedAction;

	public InvFrmGRN(UserAccount user)
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
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1470, 835);
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
			dtItem.Columns.Add("PODetailId", typeof(int));
			dtItem.Columns.Add("ItemWbWeight", typeof(double));
			dtItem.Columns.Add("Moisture", typeof(double));
			dtjob.Columns.Add("Id");
			dtjob.Columns.Add("JobLotDescription");
			dtPack.Columns.Add("Id");
			dtPack.Columns.Add("PackTypeDesc");
			dtwh.Columns.Add("Id");
			dtwh.Columns.Add("WareHouseName");
			dtcity.Columns.Add("Id");
			dtcity.Columns.Add("CityName");
			dtCrop.Columns.Add("Id");
			dtCrop.Columns.Add("CropYear");
			dtPurchaseBreakup.Columns.Add("Id", typeof(int));
			dtPurchaseBreakup.Columns.Add("InwardGatePassId", typeof(int));
			dtPurchaseBreakup.Columns.Add("InwardBreakupId", typeof(int));
			dtPurchaseBreakup.Columns.Add("Qty", typeof(double));
			dtPurchaseBreakup.Columns.Add("UOM", typeof(double));
			dtPurchaseBreakup.Columns.Add("GrossWeight", typeof(double));
			dtPurchaseBreakup.Columns.Add("EbWeight", typeof(double));
			dtPurchaseBreakup.Columns.Add("EBTotal", typeof(double));
			dtPurchaseBreakup.Columns.Add("NetPackSize", typeof(double));
			dtPurchaseBreakup.Columns.Add("NetWeight", typeof(double));
			dtPurchaseBreakup.Columns.Add("SuppShortWt", typeof(double));
			dtPurchaseBreakup.Columns.Add("SupplierRcvdWeight", typeof(double));
			dtPurchaseBreakup.Columns.Add("ScaleShortage", typeof(double));
			dtPurchaseBreakup.Columns.Add("BillWeight", typeof(double));
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("OrderDetailId", typeof(int));
			dtdetail.Columns.Add("OrderId", typeof(int));
			dtdetail.Columns.Add("Order", typeof(int));
			dtdetail.Columns.Add("WareHouseId", typeof(int));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemCode");
			dtdetail.Columns.Add("Item");
			dtdetail.Columns.Add("CropYearId", typeof(int));
			dtdetail.Columns.Add("JobId", typeof(int));
			dtdetail.Columns.Add("PackingTypeId", typeof(int));
			dtdetail.Columns.Add("UOMId", typeof(int));
			dtdetail.Columns.Add("UOM");
			dtdetail.Columns.Add("UOMEquivalent", typeof(double));
			dtdetail.Columns.Add("Qty", typeof(double));
			dtdetail.Columns.Add("GrossWight", typeof(double));
			dtdetail.Columns.Add("EbUnit", typeof(double));
			dtdetail.Columns.Add("EbTotal", typeof(double));
			dtdetail.Columns.Add("EbPurAgainstWeight", typeof(double));
			dtdetail.Columns.Add("LabId", typeof(int));
			dtdetail.Columns.Add("LabNo");
			dtdetail.Columns.Add("QtyForWtCut", typeof(double));
			dtdetail.Columns.Add("WtCutOnId", typeof(int));
			dtdetail.Columns.Add("WtCutOn", typeof(string));
			dtdetail.Columns.Add("WeightCutUom", typeof(double));
			dtdetail.Columns.Add("WtCut", typeof(double));
			dtdetail.Columns.Add("WtCutTotal", typeof(double));
			dtdetail.Columns.Add("AddLessWt", typeof(double));
			dtdetail.Columns.Add("ScaleShortWt", typeof(double));
			dtdetail.Columns.Add("SuppShortWt", typeof(double));
			dtdetail.Columns.Add("NetWeight", typeof(double));
			dtdetail.Columns.Add("StockEbUnit", typeof(double));
			dtdetail.Columns.Add("StockEbTotal", typeof(double));
			dtdetail.Columns.Add("StockWeight", typeof(double));
			dtdetail.Columns.Add("CityId", typeof(int));
			dtdetail.Columns.Add("FreightAmount", typeof(double));
			dtEmptyBag.Columns.Add("OrderId");
			dtEmptyBag.Columns.Add("Type");
			dtEmptyBag.Columns.Add("ItemId");
			dtEmptyBag.Columns.Add("Condition");
			dtEmptyBag.Columns.Add("RecQty", typeof(double));
			dtEmptyBag.Columns.Add("PurQty", typeof(double));
			dtEmptyBag.Columns.Add("Remarks");
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
			string documentCode = GetDocumentCode();
			DataTable gatepassData = GatepassDataDbCall();
			EmptyBagsTypeAndConditionDbCall();
			DataTable all = VehicleType.GetAll();
			DataTable dataForDropDownFromGrn = InvGrn.GetDataForDropDownFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, "46", (string)null, Conversion.ToString((object)UserAccount.BranchesId), 0);
			return new
			{
				documentNoData = documentCode,
				GatepassData = gatepassData,
				VehicleTypeData = all,
				HistoryComboData = dataForDropDownFromGrn
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
				btnDelete.Visible = false;
				ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
				ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
				ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
				ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
				ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
				ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
				ctrlGrdBar5.mGridSaveLayouts.Visible = false;
				ctrlGrdBar5.removeLayoutToolStripMenuItem.Visible = false;
				GetConfigurationsFromGlobal();
				combsupplier.ReadOnly = true;
				txtsuppwt.ReadOnly = true;
				txtebu.Text = "0";
				txtadlswt.Text = "0";
				txtebu.Enabled = false;
				txtadlswt.Enabled = false;
				txtEmptyBagsTotal.Enabled = false;
				txtwtcut.Enabled = false;
				if (EmptyBagsWeightCutEditable)
				{
					txtebu.Enabled = true;
					txtEmptyBagsTotal.Enabled = true;
				}
				if (AddLessWeightEditable)
				{
					txtadlswt.Enabled = true;
				}
				if (WeightCutEditable)
				{
					txtwtcut.Enabled = true;
					txtwtcut.ReadOnly = false;
				}
				BtnUpdateEBTotalWithGrossWeight.Visible = EBWtAccordingToGrossWeight;
				PanelBreakupMain.Enabled = RefDocumentTypeId == 105;
				txtAccessWeight.Visible = AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt;
				labelExcessWt.Visible = AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt;
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
				SupplierDtFillFromGlobal();
				TransporterDtFillFromGlobal();
				BindSupplierName();
				DeliveryTermBind();
				TransporterAcFill();
				VehicleTypesBind(result.VehicleTypeData);
				WarehousesDtFillFromGlobalAndBind();
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
				if (dtPurchaseBreakup != null && dtPurchaseBreakup.Columns.Count > 0)
				{
					AddRowInPurchaseBreakUpGrid();
					grdPurchaseBreakUpSettings();
				}
				if (dtEmptyBag != null && dtEmptyBag.Columns.Count > 0)
				{
					AddRowInvEmptyBagsGrid();
					grdEmptyBags.RetrieveStructure();
					grdEmptyBagsSettings();
				}
				dtGatePassData = result.GatepassData;
				GatepassGridFill(result.GatepassData);
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				this.FormInitializedAction?.Invoke();
				List<string> list = new List<string>();
				if (JuteBagsCutStock == 0.0)
				{
					list.Add("E.B WeightCut For JuteBags");
				}
				if (PPBagsCutStock == 0.0)
				{
					list.Add("E.B WeightCut For PPBags");
				}
				if (OpenBulkCutStock == 0.0)
				{
					list.Add("E.B WeightCut For OpenBulk");
				}
				if (list.Count > 0)
				{
					MessageBox.Show("Warning: The following configurations Of StockWeight value(s) are set to 0:\n\n" + string.Join("\n", list) + "\n\nPlease check and update them in the configuration.", "Configuration Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void PreBillNoFill(int PartyId, int OrderId)
	{
		try
		{
			DataTable dt = SupplierDispatch.SupplierDispatch_GetNoForGpandGrn(UserAccount.OrganizationId, UserAccount.CompanyId, PartyId, RecId, 0, GPID, OrderId);
			InfragisticsHelper.BindAndRetainSelection(CmbSupplierDispatchedPreBillNo, dt, "SupplierDispatchId", "SupplierDispatchNo", "Pre Bill No", AllColumns: true, Conversion.ToInt(CmbSupplierDispatchedPreBillNo.Value), insertDefaultRow: false, null, null, PurchaseOrderId > 0);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbSupplierDispatchedPreBillNo).DisplayLayout.Bands[0].Columns[6].Hidden = true;
				((UltraGridBase)CmbSupplierDispatchedPreBillNo).DisplayLayout.Bands[0].Columns[7].Hidden = true;
				CmbSupplierDispatchedPreBillNo_Leave(null, null);
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
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("Supplier", typeof(string));
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtSupplier, cmbSupplierNameHistory, "Id", "Supplier", "Supplier Name", ZeroIndex: false);
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
				CmbWareHouseName.Value = wareHouseId;
			}
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				int transporterId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("FreightInwardAc"));
				if (transporterId != 0)
				{
					CmbTransport.Value = transporterId;
				}
			}
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				if (FreightId > 0)
				{
					((Control)(object)CmbTransport).Enabled = false;
					txtcarramount.Enabled = false;
				}
				else
				{
					txtcarramount.Enabled = true;
					((Control)(object)CmbTransport).Enabled = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private void TransporterAccountDisable()
	{
		try
		{
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				if (FreightId > 0)
				{
					((Control)(object)CmbTransport).Enabled = false;
					txtcarramount.Enabled = false;
				}
				else
				{
					txtcarramount.Enabled = true;
					((Control)(object)CmbTransport).Enabled = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			JuteBagsCutStock = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeightCutForJuteBagsStock"));
			PPBagsCutStock = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeightCutForPPBagsStock"));
			OpenBulkCutStock = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeightCutForOpenBulkStock"));
			WagesActiveOrInActive = GlobalVariables_Helper.GetWagesRefDocumentsStatusById(46);
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchWithNameOrCode"));
			DeductionPolicyForGrnIsOn = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DeductionPolicyForGrnIsOn"));
			EBWtAccordingToGrossWeight = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("EBWtAccordingToGrossWeight"));
			JuteBagsCut = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeightCutForJuteBags"));
			PPBagsCut = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeightCutForPPBags"));
			OpenBulkCut = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeightCutForOpenBulk"));
			EmptyBagsWeightCutEditable = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("EmptyBagsWeightCutEditableOnGRN"));
			AddLessWeightEditable = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AddLessWeightCutEditableOnGRN"));
			WagesStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ContractorWagesCompulsoryBeforeInvoices"));
			EmptyBagsInformationComp = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("EmptyBagsInofrmationCompulsoryOnGRN"));
			LabCompulsoryForWeighBridgeAgainstGatePurchase = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("LabCompulsoryForWeighBridgeAgainstGatePurchase"));
			LabCompulsoryForWeighBridgeAgainstPO = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("LabCompulsoryForWeighBridge"));
			BillWeightAndStockWeightDifferenceTolerance = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BillWeightAndStockWeightDifferenceTolerance"));
			DeliveryTermEditableOnGrnForGatePurchase = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DeliveryTermEditableOnGrnForGatePurchase"));
			WeightCutEditable = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeightCutEditable"));
			LabCompulsoryNotCheckingOnGRN = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("LabCompulsoryNotCheckingOnGRN"));
			ExcludeWeightShortageBusinessOnGrn = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ExcludeWeightShortageBusinessOnGrn"));
			ValidateGrnAndInvoiceDateWithGpDate = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ValidateGrnAndInvoiceDateWithGpDate"));
			DocDate.Enabled = !ValidateGrnAndInvoiceDateWithGpDate;
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

	private string GetDocumentCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			return Conversion.ToInt((object)InvGrn.GenerateInvGrnCode(new InvGrn
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 46,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			})).ToString();
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
			List<getGlobalSupplierCustomer> obj = clsGlobalVariables.globalSupplierCustomer;
			dtSupplier.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				List<getGlobalSupplierCustomer> lst = (from r in obj.AsEnumerable()
					where r.PartyTypeId == 1
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
			int SupId = Conversion.ToInt(combsupplier.Value);
			if (dtSupplier.Rows.Count > 0)
			{
				InfragisticsHelper.BindAndRetainSelection(combsupplier, dtSupplier, "Id", "CompanyName", "SupplierName", AllColumns: true, SupId, insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)combsupplier).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)combsupplier).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
			else
			{
				((Control)(object)combsupplier).Text = string.Empty;
				((UltraGridBase)combsupplier).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DeliveryTermBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			dt.Rows.Add(3, "Load & PartyWeight");
			dt.Rows.Add(4, "Load & FactoryWeight");
			dt.Rows.Add(5, "Ponch & PartyWeight");
			dt.Rows.Add(6, "Ponch & FactoryWeight");
			DDL.BindDDL(dt, txtDeliverTerm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)txtDeliverTerm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				if (lst != null && lst.Count > 0)
				{
					for (int i = 0; i < lst.Count; i++)
					{
						dtTransporter.Rows.Add(lst[i].GlAccountId, lst[i].CompanyName, lst[i].PartyCode, lst[i].Id);
					}
				}
				return;
			}
			int[] excludedIds = new int[11]
			{
				2, 4, 10, 11, 12, 13, 14, 15, 20, 21,
				22
			};
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
			int Id = Conversion.ToInt(CmbTransport.Value);
			if (dtTransporter.Rows.Count > 0)
			{
				InfragisticsHelper.BindAndRetainSelection(CmbTransport, dtTransporter, "Id", "AccountTitle", "Transporter Name", AllColumns: true, Id, insertDefaultRow: true);
				if (dtTransporter.Rows.Count > 0 && ((UltraGridBase)CmbTransport).DataSource != null)
				{
					((UltraGridBase)CmbTransport).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void VehicleTypesBind(DataTable dtVehicle)
	{
		try
		{
			if (dtVehicle.Rows.Count > 0)
			{
				int Id = Conversion.ToInt(combvehtyp.Value);
				InfragisticsHelper.BindAndRetainSelection(combvehtyp, dtVehicle, "Id", "VehicleDescription", "VehicleDescription", AllColumns: true, Id, insertDefaultRow: true);
			}
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
			DatatableHelper.PopulateDataTable(dtwh, lst, (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName);
			int Id = Conversion.ToInt(CmbWareHouseName.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbWareHouseName, dtwh, "Id", "WareHouseName", "WareHouse Name", AllColumns: true, Id, insertDefaultRow: true);
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
			int Id = Conversion.ToInt(combitem.Value);
			string ValueMember = (rdSearchByName.Checked ? "ItemName" : "ItemCode");
			InfragisticsHelper.BindAndRetainSelection(combitem, dtItem, "Id", ValueMember, "Item Name", AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: true, 1);
			combitem_Leave(null, null);
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
			ItemNameBind();
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
			int[] excludedIds = new int[3] { 1, 2, 5 };
			List<InvPackingType> Finallist = lst.Where((InvPackingType r) => excludedIds.Contains(r.Id)).ToList();
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

	private void comborderno_Leave(object sender, EventArgs e)
	{
		try
		{
			DataTable dtPoDetail = new DataTable();
			dtPOEmptyBagsDetail.Clear();
			if (PurchaseOrderId > 0)
			{
				dtPoDetail = PurchaseOrder.ReadByPurchaseOrderIDNOrderItemId(PurchaseOrderId);
				dtPOEmptyBagsDetail = PurchaseOrder.GetPurchaseOrderEmptyBagsDetailByOrderId(PurchaseOrderId, "");
			}
			PurchaseOrderEmptyBagsDetailByOrderId();
			if (dtPoDetail.Rows.Count > 0)
			{
				((Control)(object)CmbCity).Text = Conversion.ToString(dtPoDetail.Rows[0]["CityArea"]);
				((Control)(object)txtDeliverTerm).Text = Conversion.ToString(dtPoDetail.Rows[0]["DeliveryTerm"]);
				CmbPackUom.Value = Conversion.ToInt(dtPoDetail.Rows[0]["OrderItemUOMId"]);
			}
			combsupplier_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmptyBagsTypeAndConditionDbCall()
	{
		try
		{
			dtEmpType = CommonServices.StaticColumnsService("PurchaseOrderEmptyBagsType");
			dtItemForEmptyBag = CommonServices.GetPackingMaterialItemsAllocateToFlow();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LabNoFillWithGpIdAndItemId(int ItemId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		try
		{
			DataTable dt = InvLabAnalysisPurchaseHeader.GetPurchaseAnalysisDataForGrnByGpIdAndItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = ItemId,
				GpId = GPID
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtLab = new DataTable();
				dtLab.Columns.Add("Id", typeof(int));
				dtLab.Columns.Add("LabNo", typeof(int));
				dtLab.Columns.Add("QtyForWtCut", typeof(double));
				dtLab.Columns.Add("WtCut", typeof(double));
				dtLab.Columns.Add("WtCutOnId", typeof(int));
				dtLab.Columns.Add("WtCutOn", typeof(string));
				dtLab.Columns.Add("WeightCutUom", typeof(double));
				dtLab.Rows.Add(dt.Rows[0]["LabId"], dt.Rows[0]["LabNo"], dt.Rows[0]["QtyForWtCut"], dt.Rows[0]["WeightCut"], dt.Rows[0]["WeightCutOnId"], dt.Rows[0]["WeightCutOn"], dt.Rows[0]["WeightCutUom"]);
				DDL.BindDDL(dtLab, CmbLabNo, "Id", "LabNo", "Lab No", ZeroIndex: false);
				((UltraGridBase)CmbLabNo).DisplayLayout.Bands[0].Columns[1].Width = 60;
				((UltraGridBase)CmbLabNo).DisplayLayout.Bands[0].Columns[2].Width = 100;
				((UltraGridBase)CmbLabNo).DisplayLayout.Bands[0].Columns[3].Width = 65;
				((UltraGridBase)CmbLabNo).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbLabNo).DisplayLayout.Bands[0].Columns[5].Width = 100;
				((UltraGridBase)CmbLabNo).Rows[0].Activate();
			}
			else
			{
				((Control)(object)CmbLabNo).Text = string.Empty;
				((UltraGridBase)CmbLabNo).DataSource = null;
			}
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
			txtMoisture.Text = Conversion.ToString((object)0);
			PackUomFromGlobalBind(Conversion.ToInt(combitem.Value));
			LabNoFillWithGpIdAndItemId(Conversion.ToInt(combitem.Value));
			if (PurchaseOrderId > 0)
			{
				int ItemId = Conversion.ToInt(combitem.Value);
				if (((UltraDropDownBase)combitem).SelectedRow != null && ItemId > 0)
				{
					double Moisture = Conversion.ToDouble(((UltraDropDownBase)combitem).SelectedRow.Cells["Moisture"].Value);
					txtMoisture.Text = Conversion.ToString((object)Moisture);
				}
			}
			double GridGrossWeight = GridEX_Helper.GetColumnSum(grd, "GrossWeight");
			double GridBillWeight = GridEX_Helper.GetColumnSum(grd, "NetWeight");
			double GridStockWeight = GridEX_Helper.GetColumnSum(grd, "StockWeight");
			WeightBusinessValidations(GridGrossWeight, GridBillWeight, GridStockWeight, IsCalculating: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDeliverTerm_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			double GridGrossWeight = GridEX_Helper.GetColumnSum(grd, "GrossWeight");
			double GridBillWeight = GridEX_Helper.GetColumnSum(grd, "NetWeight");
			double GridStockWeight = GridEX_Helper.GetColumnSum(grd, "StockWeight");
			WeightBusinessValidations(GridGrossWeight, GridBillWeight, GridStockWeight, IsCalculating: true);
			Total();
			StockWeightCalculationInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)combsupplier).ActiveRow == null || Conversion.ToInt(combsupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required");
			combsupplier.Focus();
			return false;
		}
		if (Conversion.ToInt((object)combgatepass.Text.Trim()) == 0)
		{
			MessageBox.Show("Gate pass Field is Required");
			combgatepass.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtsuppwt.Text.Trim()) == 0.0 || txtsuppwt.Text.Trim() == null)
		{
			MessageBox.Show("Supplier Weight Field is Required");
			txtsuppwt.Focus();
			return false;
		}
		if (((Control)(object)txtDeliverTerm).Text.Trim() == "" || ((Control)(object)txtDeliverTerm).Text.Trim() == "0" || ((Control)(object)txtDeliverTerm).Text.Trim() == null)
		{
			MessageBox.Show("DeliveryTerm Field is Required");
			txtDeliverTerm.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtfctwt.Text.Trim()) == 0.0 || txtfctwt.Text.Trim() == null)
		{
			MessageBox.Show("Factory Weight Field is Required");
			txtfctwt.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (RefDocumentTypeId == 41 && PurchaseOrderId == 0)
		{
			MessageBox.Show("PurchaseOrderNo Not Found.Please Check");
			return false;
		}
		if (((UltraGridBase)combitem).ActiveRow == null || Conversion.ToInt(combitem.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			combitem.Focus();
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
		if (txtqty.Text.Trim() == "" || Conversion.ToDouble((object)txtqty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("Pack Unit Field is Required");
			CmbPackUom.Focus();
			return false;
		}
		DataTable dt = (DataTable)((UltraGridBase)CmbLabNo).DataSource;
		if (dt != null && dt.Rows.Count > 0 && Conversion.ToInt(CmbLabNo.Value) == 0)
		{
			MessageBox.Show("Lab No Field is Required");
			CmbLabNo.Focus();
			return false;
		}
		if (txtgwt.Text.Trim() == "" || Conversion.ToDouble((object)txtgwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Gross Weight Field is Required");
			txtgwt.Focus();
			return false;
		}
		if (txtnetwt.Text.Trim() == "" || Conversion.ToDouble((object)txtnetwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Net Bill Weight Field is Required");
			txtnetwt.Focus();
			return false;
		}
		if (txtstockwt.Text.Trim() == "" || Conversion.ToDouble((object)txtstockwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Stock Weight Field is Required");
			txtstockwt.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWareHouseName).ActiveRow == null || Conversion.ToInt(CmbWareHouseName.Value) == 0)
		{
			MessageBox.Show("Warehouse Field is Required");
			CmbWareHouseName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCity).ActiveRow == null || Conversion.ToInt(CmbCity.Value) == 0)
		{
			MessageBox.Show("City Field is Required");
			CmbCity.Focus();
			return false;
		}
		return true;
	}

	private void grdPurchaseBreakUpSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "InwardGatePassId", "InwardBreakupId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Qty",
					(EditType)1
				},
				{
					"UOM",
					(EditType)1
				},
				{
					"EbWeight",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdPurchaseBrakup, 2, 2, hiddenColumns, LockBreakUp ? null : editableColumns);
			grdPurchaseBrakup.RootTable.Columns["SuppShortWt"].Visible = RefDocumentTypeId == 105;
			grdPurchaseBrakup.RootTable.Columns["SupplierRcvdWeight"].Visible = RefDocumentTypeId == 105;
			grdPurchaseBrakup.RootTable.Columns["BillWeight"].Visible = RefDocumentTypeId != 106;
			grdPurchaseBrakup.RootTable.Columns["ScaleShortage"].Visible = RefDocumentTypeId != 106;
			grdPurchaseBrakup.RootTable.Columns["UOM"].Caption = "Gross Pack Size";
			grdPurchaseBrakup.RootTable.Columns["EbWeight"].Caption = "Eb/ Unit";
			grdPurchaseBrakup.RootTable.Columns["EBTotal"].Caption = "EBW Total";
			grdPurchaseBrakup.RootTable.Columns["SuppShortWt"].Caption = "Supplier Shortage";
			grdPurchaseBrakup.RootTable.Columns["SupplierRcvdWeight"].Caption = "Supplier Received Weight";
			if (LockBreakUp)
			{
				txtcarramount.Enabled = true;
				return;
			}
			GridEX_Helper.AddButton(grdPurchaseBrakup, "Add", "+", 20, 0);
			GridEX_Helper.AddButton(grdPurchaseBrakup, "Delete", "X", 20, 0);
			grdPurchaseBrakup.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseBrakup_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPurchaseBrakup.UpdateData();
			GridEXRow item = grdPurchaseBrakup.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Qty" || e.Column.Key == "UOM")
				{
					if (item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["UOM"].Value.ToString() != string.Empty)
					{
						double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
						double UOM = Conversion.ToDouble((object)item.Cells["UOM"].Value.ToString());
						double SuppShortWt = Conversion.ToDouble((object)item.Cells["SuppShortWt"].Value.ToString());
						double ScaleShortage = Conversion.ToDouble((object)item.Cells["ScaleShortage"].Value.ToString());
						double GrossWeight = Qty * UOM;
						double NetWeight = 0.0;
						double SupplierRcvdWeight = 0.0;
						double BillWeight = 0.0;
						item.Cells["GrossWeight"].Value = GrossWeight;
						if (item.Cells["EbWeight"].Value.ToString() != string.Empty)
						{
							double EbWeight = Conversion.ToDouble((object)item.Cells["EbWeight"].Value.ToString());
							item.Cells["NetPackSize"].Value = UOM - Math.Abs(EbWeight);
							double EBTotal = Qty * EbWeight;
							item.Cells["EBTotal"].Value = Math.Abs(EBTotal);
							NetWeight = GrossWeight - Math.Abs(EBTotal);
							item.Cells["NetWeight"].Value = NetWeight;
							if (chkSuppShortWeight.Checked)
							{
								SupplierRcvdWeight = NetWeight - SuppShortWt;
								item.Cells["SupplierRcvdWeight"].Value = SupplierRcvdWeight;
							}
							else
							{
								SupplierRcvdWeight = NetWeight;
								item.Cells["SupplierRcvdWeight"].Value = SupplierRcvdWeight;
							}
							if (chkScaleShortWeightApply.Checked)
							{
								BillWeight = NetWeight - SuppShortWt - ScaleShortage;
								item.Cells["BillWeight"].Value = BillWeight;
							}
							else
							{
								BillWeight = NetWeight - SuppShortWt;
								item.Cells["BillWeight"].Value = BillWeight;
							}
						}
						else
						{
							item.Cells["NetPackSize"].Value = UOM;
							NetWeight = GrossWeight;
							item.Cells["NetWeight"].Value = NetWeight;
							if (chkSuppShortWeight.Checked)
							{
								SupplierRcvdWeight = NetWeight - SuppShortWt;
								item.Cells["SupplierRcvdWeight"].Value = SupplierRcvdWeight;
							}
							else
							{
								SupplierRcvdWeight = NetWeight;
								item.Cells["SupplierRcvdWeight"].Value = SupplierRcvdWeight;
							}
							if (chkScaleShortWeightApply.Checked)
							{
								BillWeight = NetWeight - SuppShortWt - ScaleShortage;
								item.Cells["BillWeight"].Value = BillWeight;
							}
							else
							{
								BillWeight = NetWeight - SuppShortWt;
								item.Cells["BillWeight"].Value = BillWeight;
							}
						}
					}
					else
					{
						item.Cells["GrossWeight"].Value = 0;
					}
				}
				else if (e.Column.Key == "EbWeight")
				{
					double Qty2 = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double UOM2 = Conversion.ToDouble((object)item.Cells["UOM"].Value.ToString());
					double SuppShortWt2 = Conversion.ToDouble((object)item.Cells["SuppShortWt"].Value.ToString());
					double ScaleShortage2 = Conversion.ToDouble((object)item.Cells["ScaleShortage"].Value.ToString());
					double GrossWeight2 = Qty2 * UOM2;
					double NetWeight2 = 0.0;
					double SupplierRcvdWeight2 = 0.0;
					double BillWeight2 = 0.0;
					if (item.Cells["EbWeight"].Value.ToString() != string.Empty)
					{
						double EbWeight2 = Conversion.ToDouble((object)item.Cells["EbWeight"].Value.ToString());
						item.Cells["NetPackSize"].Value = UOM2 - Math.Abs(EbWeight2);
						double EBTotal2 = Qty2 * EbWeight2;
						item.Cells["EBTotal"].Value = Math.Abs(EBTotal2);
						NetWeight2 = GrossWeight2 - Math.Abs(EBTotal2);
						item.Cells["NetWeight"].Value = NetWeight2;
						if (chkSuppShortWeight.Checked)
						{
							SupplierRcvdWeight2 = NetWeight2 - SuppShortWt2;
							item.Cells["SupplierRcvdWeight"].Value = SupplierRcvdWeight2;
						}
						else
						{
							SupplierRcvdWeight2 = NetWeight2;
							item.Cells["SupplierRcvdWeight"].Value = SupplierRcvdWeight2;
						}
						if (chkScaleShortWeightApply.Checked)
						{
							BillWeight2 = NetWeight2 - SuppShortWt2 - ScaleShortage2;
							item.Cells["BillWeight"].Value = BillWeight2;
						}
						else
						{
							BillWeight2 = NetWeight2 - SuppShortWt2;
							item.Cells["BillWeight"].Value = BillWeight2;
						}
					}
					else
					{
						item.Cells["NetPackSize"].Value = UOM2;
						NetWeight2 = GrossWeight2;
						item.Cells["NetWeight"].Value = NetWeight2;
						if (chkSuppShortWeight.Checked)
						{
							SupplierRcvdWeight2 = NetWeight2 - SuppShortWt2;
							item.Cells["SupplierRcvdWeight"].Value = SupplierRcvdWeight2;
						}
						else
						{
							SupplierRcvdWeight2 = NetWeight2;
							item.Cells["SupplierRcvdWeight"].Value = SupplierRcvdWeight2;
						}
						if (chkScaleShortWeightApply.Checked)
						{
							BillWeight2 = NetWeight2 - SuppShortWt2 - ScaleShortage2;
							item.Cells["BillWeight"].Value = BillWeight2;
						}
						else
						{
							BillWeight2 = NetWeight2 - SuppShortWt2;
							item.Cells["BillWeight"].Value = BillWeight2;
						}
					}
				}
			}
			grdPurchaseBrakup.UpdateData();
			SupplierShortageProportion();
			ScaleShortageProportionInBreakUpGrid();
			SupplierShortageProportionInBreakUpGrid();
			TotalEbPurchaseAgainstWeightUtilizeInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseBrakup_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPurchaseBrakup.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteRowInPurchaseBrakupGrid(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInPurchaseBreakUpGrid();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteRowInPurchaseBrakupGrid(GridEXRow r)
	{
		try
		{
			dtPurchaseBreakup.Rows[r.RowIndex].Delete();
			r.Delete();
			if (dtPurchaseBreakup.Rows.Count == 0)
			{
				AddRowInPurchaseBreakUpGrid();
			}
			SupplierShortageProportion();
			SupplierShortageProportionInBreakUpGrid();
			ScaleShortageProportionInBreakUpGrid();
			UpdateBillWeightInBreakUpGrid();
			grdPurchaseBrakup.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPurchaseBreakUpGrid()
	{
		try
		{
			dtPurchaseBreakup.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
			grdPurchaseBrakup.DataSource = dtPurchaseBreakup;
			SupplierShortageProportion();
			SupplierShortageProportionInBreakUpGrid();
			ScaleShortageProportionInBreakUpGrid();
			UpdateBillWeightInBreakUpGrid();
			grdPurchaseBrakup.UpdateData();
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
			int count = grd.GetRows().Count();
			if (DeductionPolicyForGrnIsOn && count == 1)
			{
				throw new Exception("Adding multiple rows is not allowed while the deduction policy is active.");
			}
			if (RefDocumentTypeId == 105 && grdPurchaseBrakup.RowCount > 0)
			{
				double selectedPackSize = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				if (!dtPurchaseBreakup.AsEnumerable().Any((DataRow row) => row.Field<double>("NetPackSize") == selectedPackSize))
				{
					throw new Exception("UOM " + ((Control)(object)CmbPackUom).Text + " does not exist in BreakUp Grid");
				}
			}
			if (!FormValidationDetail() || (Conversion.ToInt(CmbPackingType.Value) == 5 && Conversion.ToInt(CmbPackUom.Value) > 0 && ((UltraGridBase)CmbPackUom).ActiveRow != null && Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value) != 65.0 && MessageBox.Show("Are you sure to Add Entry, because Packing type is 'OPEN BULK' and Pack Uom not 65?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
			{
				return;
			}
			double netWeight = Conversion.ToDouble((object)txtnetwt.Text.Trim());
			double grossWeight = Conversion.ToDouble((object)txtgwt.Text.Trim());
			if (netWeight > grossWeight)
			{
				throw new Exception("NetBillWeight cannot be greater than Gross Weight. Please check.");
			}
			int orderDetailId = ((RefDocumentTypeId == 41 && ((UltraDropDownBase)combitem).SelectedRow != null) ? Conversion.ToInt(((UltraDropDownBase)combitem).SelectedRow.Cells["PODetailId"].Value) : 0);
			double qtyForWtCut = 0.0;
			int wtCutOnId = 0;
			string wtCutOn = "";
			double WeightCutUom = 0.0;
			if (((UltraDropDownBase)CmbLabNo).SelectedRow != null)
			{
				qtyForWtCut = Conversion.ToDouble(((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["QtyForWtCut"].Value);
				wtCutOnId = Conversion.ToInt(((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["WtCutOnId"].Value);
				wtCutOn = Conversion.ToString(((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["WtCutOn"].Value);
				WeightCutUom = Conversion.ToDouble(((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["WeightCutUom"].Value);
				if (chkLabWtcutcompulsory.Checked && Conversion.ToDouble((object)txtwtcut.Text.Trim()) == 0.0 && MessageBox.Show("Weight Cut is compulsory but the value is zero. Do you want to continue?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return;
				}
			}
			if (((UltraDropDownBase)CmbLabNo).SelectedRow != null && Conversion.ToInt(CmbLabNo.Value) > 0)
			{
				qtyForWtCut = Conversion.ToDouble(((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["QtyForWtCut"].Value);
				wtCutOnId = Conversion.ToInt(((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["WtCutOnId"].Value);
				wtCutOn = Conversion.ToString(((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["WtCutOn"].Value);
				WeightCutUom = Conversion.ToDouble(((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["WeightCutUom"].Value);
				if (chkLabWtcutcompulsory.Checked && Conversion.ToDouble((object)txtwtcut.Text.Trim()) == 0.0 && MessageBox.Show("Weight Cut is compulsory but the value is zero. Do you want to continue?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return;
				}
			}
			double ebUnit = Conversion.ToDouble((object)txtebu.Text);
			double ebTotal = Conversion.ToDouble((object)txtEmptyBagsTotal.Text);
			if (RefDocumentTypeId != 105 && GridEX_Helper.IsAnyCellValueHaveSameValue(grdEmptyBags, "Type", "3") && ebTotal == 0.0)
			{
				txtebu.Focus();
				string termName = DatatableHelper.GetColumnValueByIdFromDatatable<string>(dtEmpType, "Id", 3, "Type");
				throw new Exception("EmptyBags Weight is required when Empty Bags Case is " + termName);
			}
			double qty = Conversion.ToDouble((object)txtqty.Text.Trim());
			if (ebUnit > 0.0 && ebTotal == 0.0)
			{
				ebTotal = qty * ebUnit;
				txtEmptyBagsTotal.Text = ebTotal.ToString();
			}
			else if (ebUnit == 0.0 && ebTotal > 0.0)
			{
				ebUnit = ebTotal / qty;
				txtebu.Text = ebUnit.ToString();
			}
			dtdetail.Rows.Add(0, orderDetailId, PurchaseOrderId, PurchaseOrderNo, Conversion.ToInt(CmbWareHouseName.Value), Conversion.ToInt(combitem.Value), Conversion.ToString(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(CmbCropYear.Value), Conversion.ToInt(CmbJobLot.Value), Conversion.ToInt(CmbPackingType.Value), Conversion.ToInt(CmbPackUom.Value), Conversion.ToString((object)((Control)(object)CmbPackUom).Text), Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value), qty, grossWeight, ebUnit, ebTotal, 0, Conversion.ToInt(CmbLabNo.Value), Conversion.ToInt((object)((Control)(object)CmbLabNo).Text), qtyForWtCut, wtCutOnId, wtCutOn, WeightCutUom, Conversion.ToDouble((object)txtwtcut.Text.Trim()), Conversion.ToDouble((object)txtwtcuttotal.Text.Trim()), Conversion.ToDouble((object)txtadlswt.Text.Trim()), 0, 0, netWeight, Conversion.ToDouble((object)txtEbUnitStock.Text.Trim()), Conversion.ToDouble((object)txtEbTotalStock.Text.Trim()), Conversion.ToDouble((object)txtstockwt.Text.Trim()), Conversion.ToInt(CmbCity.Value), 0);
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			txtvehno.Enabled = true;
			ResetDetial();
			ScaleShortageProportion();
			SupplierShortageProportion();
			TotalEbPurchaseAgainstWeightUtilizeInGrid();
			FreightProportion();
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
			if (RefDocumentTypeId == 105 && grdPurchaseBrakup.RowCount > 0 && !dtPurchaseBreakup.AsEnumerable().Any((DataRow row) => row.Field<double>("NetPackSize") == Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value)))
			{
				throw new Exception("Uom " + ((Control)(object)CmbPackUom).Text + " does not exists in BreakUp Grid");
			}
			if (!FormValidationDetail() || (Conversion.ToInt(CmbPackingType.Value) == 5 && Conversion.ToInt(CmbPackUom.Value) > 0 && ((UltraGridBase)CmbPackUom).ActiveRow != null && Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value) != 65.0 && MessageBox.Show("Are you sure to Add Entry, because Packing type is 'OPEN BULK' and Pack Uom not 65?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
			{
				return;
			}
			Conversion.ToDouble((object)txtstockwt.Text.Trim());
			double num = Conversion.ToDouble((object)txtnetwt.Text.Trim());
			double GrossWeight = Conversion.ToDouble((object)txtgwt.Text.Trim());
			if (num > GrossWeight)
			{
				throw new Exception("NetBillWeight cannot greater than Gross Weight Please check");
			}
			if (RefDocumentTypeId == 41)
			{
				dtdetail.Rows[updateDetailIndex]["OrderDetailId"] = ((((UltraDropDownBase)combitem).SelectedRow != null) ? Conversion.ToInt(((UltraDropDownBase)combitem).SelectedRow.Cells["PODetailId"].Value) : 0);
			}
			if (((UltraDropDownBase)CmbLabNo).SelectedRow != null && Conversion.ToInt(CmbLabNo.Value) > 0)
			{
				dtdetail.Rows[updateDetailIndex]["QtyForWtCut"] = ((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["QtyForWtCut"].Value;
				dtdetail.Rows[updateDetailIndex]["WtCutOnId"] = ((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["WtCutOnId"].Value;
				dtdetail.Rows[updateDetailIndex]["WtCutOn"] = ((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["WtCutOn"].Value;
				dtdetail.Rows[updateDetailIndex]["WeightCutUom"] = ((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["WeightCutUom"].Value;
				if (chkLabWtcutcompulsory.Checked && Conversion.ToDouble((object)txtwtcut.Text.Trim()) == 0.0 && MessageBox.Show("Weight Cut is compulsory but the value is zero. Do you want to continue?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return;
				}
			}
			double EBUnit = Conversion.ToDouble((object)txtebu.Text);
			double ebTotal = Conversion.ToDouble((object)txtEmptyBagsTotal.Text);
			if (RefDocumentTypeId != 105 && GridEX_Helper.IsAnyCellValueHaveSameValue(grdEmptyBags, "Type", "3") && ebTotal == 0.0)
			{
				txtebu.Focus();
				string termName = DatatableHelper.GetColumnValueByIdFromDatatable<string>(dtEmpType, "Id", 3, "Type");
				throw new Exception("EmptyBags Weight is required when Empty Bags Case is " + termName);
			}
			if (EBUnit > 0.0 && ebTotal == 0.0)
			{
				txtEmptyBagsTotal.Text = (Conversion.ToDouble((object)txtqty.Text.Trim()) * EBUnit).ToString();
			}
			else if (EBUnit == 0.0 && ebTotal > 0.0)
			{
				txtebu.Text = (ebTotal / Conversion.ToDouble((object)txtqty.Text.Trim())).ToString();
			}
			dtdetail.Rows[updateDetailIndex]["OrderId"] = PurchaseOrderId;
			dtdetail.Rows[updateDetailIndex]["Order"] = PurchaseOrderNo;
			dtdetail.Rows[updateDetailIndex]["WareHouseId"] = Conversion.ToInt(CmbWareHouseName.Value);
			dtdetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(combitem.Value);
			dtdetail.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemCode"].Value);
			dtdetail.Rows[updateDetailIndex]["Item"] = Conversion.ToString(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemName"].Value);
			dtdetail.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(CmbCropYear.Value);
			dtdetail.Rows[updateDetailIndex]["JobId"] = Conversion.ToInt(CmbJobLot.Value);
			dtdetail.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(CmbPackingType.Value);
			dtdetail.Rows[updateDetailIndex]["UOMId"] = Conversion.ToInt(CmbPackUom.Value);
			dtdetail.Rows[updateDetailIndex]["UOM"] = Conversion.ToString((object)((Control)(object)CmbPackUom).Text);
			dtdetail.Rows[updateDetailIndex]["UOMEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
			dtdetail.Rows[updateDetailIndex]["Qty"] = Conversion.ToDouble((object)txtqty.Text);
			dtdetail.Rows[updateDetailIndex]["GrossWight"] = Conversion.ToDouble((object)txtgwt.Text);
			dtdetail.Rows[updateDetailIndex]["EbUnit"] = Conversion.ToDouble((object)txtebu.Text.Trim());
			dtdetail.Rows[updateDetailIndex]["EbTotal"] = Conversion.ToDouble((object)txtEmptyBagsTotal.Text);
			dtdetail.Rows[updateDetailIndex]["LabId"] = Conversion.ToInt(CmbLabNo.Value);
			dtdetail.Rows[updateDetailIndex]["LabNo"] = Conversion.ToString((object)((Control)(object)CmbLabNo).Text);
			dtdetail.Rows[updateDetailIndex]["WtCut"] = Conversion.ToDouble((object)txtwtcut.Text.Trim());
			dtdetail.Rows[updateDetailIndex]["WtCutTotal"] = Conversion.ToDouble((object)txtwtcuttotal.Text.Trim());
			dtdetail.Rows[updateDetailIndex]["AddLesswt"] = Conversion.ToDouble((object)txtadlswt.Text.Trim());
			dtdetail.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDouble((object)txtnetwt.Text);
			dtdetail.Rows[updateDetailIndex]["StockEbUnit"] = Conversion.ToDouble((object)txtEbUnitStock.Text.Trim());
			dtdetail.Rows[updateDetailIndex]["StockEbTotal"] = Conversion.ToDouble((object)txtEbTotalStock.Text);
			dtdetail.Rows[updateDetailIndex]["StockWeight"] = Conversion.ToDouble((object)txtstockwt.Text);
			dtdetail.Rows[updateDetailIndex]["CityId"] = Conversion.ToInt(CmbCity.Value);
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ResetDetial();
			ScaleShortageProportion();
			SupplierShortageProportion();
			TotalEbPurchaseAgainstWeightUtilizeInGrid();
			FreightProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		updateDetailIndex = -1;
		Add.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				updateDetailIndex = item.RowIndex;
				if (RefDocumentTypeId == 41)
				{
					PurchaseOrderId = Conversion.ToInt(item.Cells["OrderId"].Value);
					PurchaseOrderNo = Conversion.ToInt((object)item.Cells["Order"].Text);
					comborderno_Leave(null, null);
				}
				combitem.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				CmbWareHouseName.Value = item.Cells["WareHouseId"].Value.ToString();
				CmbCropYear.Value = item.Cells["CropYearId"].Value.ToString();
				CmbJobLot.Value = item.Cells["JobId"].Value.ToString();
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				txtqty.Text = item.Cells["Qty"].Value.ToString();
				CmbPackUom.Value = item.Cells["UOMId"].Value.ToString();
				txtgwt.Text = item.Cells["GrossWight"].Value.ToString();
				txtebu.Text = item.Cells["EbUnit"].Value.ToString();
				txtEmptyBagsTotal.Text = item.Cells["EbTotal"].Value.ToString();
				CmbLabNo.Value = Conversion.ToInt(item.Cells["LabId"].Value);
				CmbLabNo_Leave(null, null);
				txtWtCutOn.Text = Conversion.ToString(item.Cells["WtCutOn"].Value);
				txtwtcut.Text = item.Cells["WtCut"].Value.ToString();
				txtwtcuttotal.Text = item.Cells["WtCutTotal"].Value.ToString();
				txtadlswt.Text = item.Cells["AddLesswt"].Value.ToString();
				txtnetwt.Text = item.Cells["NetWeight"].Value.ToString();
				txtEbUnitStock.Text = item.Cells["StockEbUnit"].Value.ToString();
				txtEbTotalStock.Text = item.Cells["StockEbTotal"].Value.ToString();
				txtstockwt.Text = item.Cells["StockWeight"].Value.ToString();
				CmbCity.Value = item.Cells["CityId"].Value.ToString();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				combitem.Focus();
			}
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
			List<string> hiddenColumns = new List<string> { "Id", "OrderDetailId", "OrderId", "ItemId", "UOMId", "LabId", "UOMEquivalent", "WtCutOnId", "ItemCode", "" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 2, hiddenColumns);
			GridEXColumnCollection columns = grd.RootTable.Columns;
			CommonSettingInGrid(grd);
			columns["GrossWight"].EditType = (EditType)(RefDocumentTypeId != 105);
			CommonDetailSettings(grd, RefDocumentTypeId);
			GridEX_Helper.AddButton(grd, "Delete", "X", 20, 0);
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommonDetailSettings(GridEX gridEx, int RefDocTypeId)
	{
		try
		{
			GridEXColumnCollection columns = gridEx.RootTable.Columns;
			columns["FreightAmount"].Visible = RefDocTypeId == 105;
			columns["SuppShortWt"].Visible = RefDocTypeId == 105;
			columns["ScaleShortWt"].Visible = RefDocTypeId != 106;
			columns["AddLesswt"].Caption = "Add Less";
			columns["NetWeight"].Caption = "Bill Weight";
			columns["GrossWight"].Caption = "Gross Weight";
			columns["Order"].Width = 45;
			columns["WareHouseId"].Width = 120;
			columns["Item"].Width = 150;
			columns["CropYearId"].Width = 70;
			columns["JobId"].Width = 80;
			columns["PackingTypeId"].Width = 100;
			columns["UOM"].Width = 45;
			columns["Qty"].Width = 50;
			columns["GrossWight"].Width = 75;
			columns["EbUnit"].Width = 35;
			columns["EbTotal"].Width = 50;
			columns["LabNo"].Width = 35;
			columns["QtyForWtCut"].Width = 55;
			columns["WtCutOn"].Width = 50;
			columns["WeightCutUom"].Width = 65;
			columns["WtCut"].Width = 35;
			columns["WtCutTotal"].Width = 55;
			columns["AddLessWt"].Width = 40;
			columns["ScaleShortWt"].Width = Constants.InventoryConstants.WeightConstant;
			columns["SuppShortWt"].Width = Constants.InventoryConstants.WeightConstant;
			columns["NetWeight"].Width = 75;
			columns["StockWeight"].Width = 75;
			columns["StockWeight"].Width = 75;
			columns["StockEbUnit"].Width = 55;
			columns["StockEbTotal"].Width = 60;
			columns["CityId"].Width = 80;
			columns["FreightAmount"].Width = 90;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CommonSettingInGrid(GridEX grd)
	{
		try
		{
			GridEX_Helper.ComboBind(grd.RootTable.Columns["WareHouseId"], dtwh, "Id", "WareHouseName", "WareHouse Name", 130);
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
			ScaleShortageProportion();
			SupplierShortageProportion();
			FreightProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteRowInDetailGrid(GridEXRow r)
	{
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
					dtdetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
				grd.UpdateData();
				ScaleShortageProportion();
				SupplierShortageProportion();
				FreightProportion();
				return;
			}
			throw new Exception("Reset Detail first...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseOrderEmptyBagsDetailByOrderId()
	{
		try
		{
			if (RefDocumentTypeId != 41)
			{
				return;
			}
			if (dtPOEmptyBagsDetail.Rows.Count == 0)
			{
				txtebu.Text = "0";
				EmptyBagsWeightCutStatus = 0;
			}
			else
			{
				if (!btnSave.Visible || !btnSave.Enabled)
				{
					return;
				}
				dtEmptyBag.Rows.Clear();
				int packingTypeValue = Conversion.ToInt(CmbPackingType.Value);
				bool isOpenBulkPacking = packingTypeValue == 5;
				foreach (DataRow row in dtPOEmptyBagsDetail.Rows)
				{
					int type = Conversion.ToInt(row["Type"]);
					int packingTypeId = Conversion.ToInt(row["PackingTypeId"]);
					if (type == 1 || type == 2 || type == 3)
					{
						EmptyBagsWeightCutStatus = type;
					}
					if (packingTypeId == packingTypeValue)
					{
						if (Conversion.ToDouble(row["WeightCut"]) > 0.0)
						{
							txtebu.Text = Conversion.ToString(row["WeightCut"]);
						}
						else
						{
							txtebu.Text = ((packingTypeId == 2) ? Conversion.ToString((object)PPBagsCut) : Conversion.ToString((object)JuteBagsCut));
						}
					}
					if (isOpenBulkPacking)
					{
						txtebu.Text = Conversion.ToString((object)OpenBulkCut);
					}
					if (type == 1)
					{
						dtEmptyBag.Rows.Add(row["PurchaseOrderId"], type, row["ItemId"], 0, 0, 0, "");
					}
					else
					{
						double itemQty = ((GpQty > 0.0) ? (GpQty / 2.0) : 0.0);
						dtEmptyBag.Rows.Add(row["PurchaseOrderId"], type, row["ItemId"], 0, 0, itemQty, "");
					}
				}
				grdEmptyBags.DataSource = dtEmptyBag;
				grdEmptyBags.RetrieveStructure();
				grdEmptyBagsSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmptyBagsGridComboBind(GridEX grid)
	{
		try
		{
			GridEX_Helper.ComboBind(grid.RootTable.Columns["ItemId"], dtItemForEmptyBag, "ItemId", "ItemName", "Item Name", 80);
			GridEX_Helper.ComboBind(grid.RootTable.Columns["Type"], dtEmpType, "Id", "Type", "EmptyBagsType", 80);
			DataTable dtBagsCondition = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalItemConditions.Where((GlobalItemCondition r) => r.Id != 4).ToList(), (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
			GridEX_Helper.ComboBind(grid.RootTable.Columns["Condition"], dtBagsCondition, "Id", "Description", "Bags_Condition");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBagsSettings()
	{
		try
		{
			List<string> hiddenCol = new List<string> { "OrderId", "" };
			Dictionary<string, EditType> editableCol = new Dictionary<string, EditType>
			{
				{
					"OrderId",
					(EditType)0
				},
				{
					"RecQty",
					(EditType)1
				},
				{
					"PurQty",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdEmptyBags, 2, 2, hiddenCol, editableCol);
			EmptyBagsGridComboBind(grdEmptyBags);
			grdEmptyBags.RootTable.Columns["RecQty"].Caption = "Received Qty";
			grdEmptyBags.RootTable.Columns["PurQty"].Caption = "Purchase Qty";
			if (grdEmptyBags.GetRows().Length == 1)
			{
				GridEX_Helper.AddButton(grdEmptyBags, "Add", "+", 20, 0);
				grdEmptyBags.FrozenColumns = 1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdEmptyBags.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (Conversion.ToInt(r.Cells["PurQty"].Value) < 0)
				{
					r.BeginEdit();
					r.Cells["PurQty"].Value = 0.ToString();
					r.EndEdit();
					MessageBox.Show("PurQty cannot be less than Zero");
				}
				if (Conversion.ToInt(r.Cells["RecQty"].Value) < 0)
				{
					r.BeginEdit();
					r.Cells["RecQty"].Value = 0.ToString();
					r.EndEdit();
					MessageBox.Show("RecQty cannot be less than Zero");
				}
				if (e.Column.Key == "PurQty" || e.Column.Key == "Type")
				{
					EmptyBagsWeightCutStatus = Conversion.ToInt(r.Cells["Type"].Value);
					TotalEbPurchaseAgainstWeightUtilizeInGrid();
				}
				if ((Conversion.ToInt(r.Cells["Type"].Value) == 4 || Conversion.ToInt(r.Cells["Type"].Value) == 5) && e.Column.Key == "PurQty")
				{
					r.BeginEdit();
					r.Cells["PurQty"].Value = 0.ToString();
					r.EndEdit();
					MessageBox.Show("Retained or Returned Stock you cannot be purchase");
				}
				if (Conversion.ToInt(r.Cells["Type"].Value) == 2 && e.Column.Key == "RecQty")
				{
					r.Cells["RecQty"].Value = 0;
					MessageBox.Show("You cannot be add value RecQty because EmptyBagsType is Purchase Against Weight");
				}
				grdEmptyBags.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdEmptyBags_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdEmptyBags.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "Add")
			{
				if (grdEmptyBags.GetRows().Length == 1)
				{
					dtEmptyBag.Rows.Add(Conversion.ToInt(item.Cells["OrderId"].Value), Conversion.ToInt(item.Cells["Type"].Value), 0, Conversion.ToInt(item.Cells["Condition"].Value), 0, 0, "");
					grdEmptyBags.DataSource = dtEmptyBag;
				}
				grdEmptyBags.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvEmptyBagsGrid()
	{
		dtEmptyBag.Rows.Add(0, 0, 0, 0, 0, 0, "");
		grdEmptyBags.DataSource = dtEmptyBag;
	}

	private DataTable GatepassDataDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			return GatePassInward.GetByPendingGatepass(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 51,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GatepassGridFill(DataTable dtpendingGp)
	{
		try
		{
			if (dtpendingGp.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("GpSrNo", typeof(int));
				dtcol.Columns.Add("Status", typeof(string));
				dtcol.Columns.Add("FVStatus", typeof(string));
				dtcol.Columns.Add("GpDate", typeof(DateTime));
				dtcol.Columns.Add("OrderType", typeof(string));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("VarietyName", typeof(string));
				dtcol.Columns.Add("Qty", typeof(decimal));
				dtcol.Columns.Add("SupplierWeight", typeof(decimal));
				dtcol.Columns.Add("FactoryWeight", typeof(decimal));
				dtcol.Columns.Add("ReceivedWeight", typeof(decimal));
				dtcol.Columns.Add("StockWeight", typeof(decimal));
				dtcol.Columns.Add("PoAccessWeight", typeof(decimal));
				dtcol.Columns.Add("VehicleType", typeof(string));
				dtcol.Columns.Add("NetPaid", typeof(decimal));
				dtcol.Columns.Add("RefDocumentTypeId", typeof(int));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("DetailIdsCount", typeof(int));
				dtcol.Columns.Add("FreightOnCash", typeof(decimal));
				dtcol.Columns.Add("FreightId", typeof(int));
				dtcol.Columns.Add("FreightDebitAccountId", typeof(int));
				dtcol.Columns.Add("LastLabId", typeof(int));
				dtcol.Columns.Add("ScalCharges", typeof(decimal));
				dtcol.Columns.Add("AdvanceByParty", typeof(decimal));
				dtcol.Columns.Add("AdvanceByFactory", typeof(decimal));
				dtcol.Columns.Add("OtherDeduction", typeof(decimal));
				dtcol.Columns.Add("TotalFreight", typeof(decimal));
				dtcol.Columns.Add("ShortageAmount", typeof(decimal));
				dtcol.Columns.Add("DiscountAmount", typeof(decimal));
				dtcol.Columns.Add("RemainingAmount", typeof(decimal));
				dtcol.Columns.Add("ChargeToPartyAmount", typeof(decimal));
				dtcol.Columns.Add("TotalDeductionAmount", typeof(decimal));
				dtcol.Columns.Add("TotalPaidAmount", typeof(decimal));
				dtcol.Columns.Add("SupplierDispatchId", typeof(int));
				dtcol.Columns.Add("SupplierDispatchNo", typeof(int));
				dtcol.Columns.Add("FreightSpecialApprovalStatus", typeof(string));
				dtcol.Columns.Add("FreightSpecialApprovalStatusId", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow row in dtpendingGp.Rows)
				{
					string FVStatus = ((Conversion.ToInt(row["FreightId"]) == 0) ? "Pending" : "Complete");
					dtcol.Rows.Add(row["Id"], row["GpSrNo"], row["Status"], FVStatus, row["GpDate"], row["OrderType"], row["SupplierName"], row["VehicleNo"], row["BiltyNo"], row["VarietyName"], row["Qty"], row["SupplierWeight"], row["FactoryWeight"], row["ReceivedWeight"], row["StockWeight"], row["AccessWeight"], row["VehicleType"], row["NetPaid"], row["RefDocumentTypeId"], row["EntryDate"], row["EntryUser"], row["ModifyDate"], row["ModifyUser"], row["DetailIdsCount"], row["FreightOnCash"], row["FreightId"], row["FreightDebitAccountId"], row["LabId"], row["ScalCharges"], row["AdvanceByParty"], row["AdvanceByFactory"], row["OtherDeduction"], row["TotalFreight"], row["ShortageAmount"], row["DiscountAmount"], row["RemainingAmount"], row["ChargeToPartyAmount"], row["TotalDeductionAmount"], row["TotalPaidAmount"], row["SupplierDispatchId"], row["SupplierDispatchNo"], row["FreightSpecialApprovalStatus"], row["FreightSpecialApprovalStatusId"]);
				}
				grdGp.DataSource = dtcol;
				grdGp.RetrieveStructure();
				GpGridSetting();
			}
			else
			{
				grdGp.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGp_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		try
		{
			if (grdGp.RowCount <= 0)
			{
				return;
			}
			GridEXColumn AccessWeight = grdGp.RootTable.Columns["PoAccessWeight"];
			if (AccessWeight != null)
			{
				GridEXCell AccessWeightCell = e.Row.Cells[((JanusCollectionElement)AccessWeight).Index];
				if (Conversion.ToDouble(AccessWeightCell.Value) == 0.0)
				{
					AccessWeightCell.FormatStyle = new GridEXFormatStyle();
					AccessWeightCell.FormatStyle.ForeColor = Color.Green;
				}
				else if (Conversion.ToDouble(AccessWeightCell.Value) > 0.0)
				{
					AccessWeightCell.FormatStyle = new GridEXFormatStyle();
					AccessWeightCell.FormatStyle.ForeColor = Color.Red;
					AccessWeightCell.FormatStyle.FontBold = (TriState)2;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillCountFromGpGrid()
	{
		try
		{
			GridEXRow[] dataRows = grdGp.GetDataRows();
			int acceptedCount = dataRows.Count((GridEXRow row) => Conversion.ToString(row.Cells["Status"].Value) == "Accepted");
			int openCount = dataRows.Count((GridEXRow row) => Conversion.ToString(row.Cells["Status"].Value) == "Open");
			txtAcceptedGpTotal.Text = acceptedCount.ToString();
			txtOpenGpTotal.Text = openCount.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GpGridSetting()
	{
		try
		{
			FillCountFromGpGrid();
			List<string> hiddenColumns = new List<string> { "Id", "LastLabId", "DetailIdsCount", "RefDocumentTypeId", "FreightOnCash", "FreightId", "FreightDebitAccountId", "SupplierDispatchId", "FreightSpecialApprovalStatusId" };
			new Dictionary<string, EditType>
			{
				{
					"OrderId",
					(EditType)0
				},
				{
					"RecQty",
					(EditType)1
				},
				{
					"PurQty",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdGp, 2, 2, hiddenColumns);
			grdGp.RootTable.Columns["VarietyName"].Caption = "ItemName";
			grdGp.RootTable.Columns["PoAccessWeight"].Caption = "Po Excess Weight";
			grdGp.RootTable.Columns["FVStatus"].Caption = "Freight Status";
			grdGp.RootTable.Columns["GpSrNo"].ColumnType = (ColumnType)5;
			grdGp.RootTable.Groups.Add("Status");
			grdGp.RootTable.Columns["Status"].Visible = false;
			grdGp.RootTable.Columns["GpSrNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdGp.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant;
			grdGp.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grdGp.RootTable.Columns["OrderType"].Width = Constants.InventoryConstants.GpType;
			grdGp.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grdGp.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdGp.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grdGp.RootTable.Columns["VarietyName"].Width = Constants.InventoryConstants.ItemName;
			grdGp.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdGp.RootTable.Columns["SupplierWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["ReceivedWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["PoAccessWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["VehicleType"].Width = Constants.InventoryConstants.GpType;
			grdGp.RootTable.Columns["NetPaid"].Width = Constants.InventoryConstants.AmountConstant;
			grdGp.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			grdGp.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdGp.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			grdGp.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdGp.RootTable.Columns["FreightOnCash"].Width = 65;
			GridEX_Helper.AddButton(grdGp, "Load", "Load", 50, 0);
			grdGp.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGp_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGp.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "GpSrNo")
			{
				if (Conversion.ToInt(r.Cells["DetailIdsCount"].Value) > 0)
				{
					CommonServices.GatePassInwardWithDetailAndLabReport(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(Conversion.ToInt(r.Cells["Id"].Value));
				}
			}
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
				LoadGPByRow();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadGPByRow(int GpId = 0)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = ((GpId == 0) ? grdGp.CurrentRow : GridEX_Helper.GetRowByColumnNameAndValue(grdGp, "Id", GpId));
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (!btnSave.Visible || !btnSave.Enabled)
			{
				throw new Exception("Please Reset the form First...");
			}
			GpQty = 0.0;
			FreightId = 0;
			if (Conversion.ToString(r.Cells["Status"].Value) != "Accepted")
			{
				throw new Exception("Status Not Accepted Please check status");
			}
			if (!LabCompulsoryNotCheckingOnGRN && Conversion.ToInt(r.Cells["LastLabId"].Value) <= 0)
			{
				throw new Exception("Lab is pending for this gate pass. Please do lab first then Load");
			}
			label13.Text = "Goods Receiving Notes (" + Conversion.ToString((object)r.Cells["OrderType"].Text) + ")";
			RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
			if (RefDocumentTypeId == 106 && DeliveryTermEditableOnGrnForGatePurchase)
			{
				((Control)(object)txtDeliverTerm).Enabled = true;
				txtDeliverTerm.LimitToList = true;
			}
			GPID = Conversion.ToInt(r.Cells["Id"].Value);
			FreightId = Conversion.ToInt(r.Cells["FreightId"].Value);
			int FreightDebitAccountId = Conversion.ToInt(r.Cells["FreightDebitAccountId"].Value);
			txtBiltyFreight.Text = Conversion.ToString(r.Cells["NetPaid"].Value);
			txtScaleCharges.Text = Conversion.ToString(r.Cells["ScalCharges"].Value);
			txtAdvanceByParty.Text = Conversion.ToString(r.Cells["AdvanceByParty"].Value);
			txtAdvanceByFactory.Text = Conversion.ToString(r.Cells["AdvanceByFactory"].Value);
			txtOtherDeduction.Text = Conversion.ToString(r.Cells["OtherDeduction"].Value);
			txtTotalFreight.Text = Conversion.ToString(r.Cells["TotalFreight"].Value);
			txtShortageAmountFr.Text = Conversion.ToString(r.Cells["ShortageAmount"].Value);
			txtDiscountAmountFr.Text = Conversion.ToString(r.Cells["DiscountAmount"].Value);
			txtRemainingAmountFr.Text = Conversion.ToString(r.Cells["RemainingAmount"].Value);
			txtChargeToPartyAmountFr.Text = Conversion.ToString(r.Cells["ChargeToPartyAmount"].Value);
			txtFreightDeduction.Text = Conversion.ToString(r.Cells["TotalDeductionAmount"].Value);
			txtcarramount.Text = Conversion.ToString(r.Cells["TotalPaidAmount"].Value);
			if (Conversion.ToInt(r.Cells["FreightSpecialApprovalStatusId"].Value) == 1)
			{
				MessageBox.Show("The shortage allowed on the Freight Voucher exceeds the configured discount policy.\r\nTherefore, approval from an authorized person is required.\r\nPlease complete the Freight Voucher approval before proceeding further", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			txtBiltyFreight.Enabled = FreightId == 0;
			txtAdvanceByParty.Enabled = FreightId == 0;
			txtAdvanceByFactory.Enabled = FreightId == 0;
			txtFreightDeduction.Enabled = FreightId == 0;
			if (FreightDebitAccountId > 0)
			{
				CmbTransport.Value = FreightDebitAccountId;
			}
			GpQty = Conversion.ToDouble(r.Cells["Qty"].Value);
			txtReceivedWeight.Text = Conversion.ToString(r.Cells["ReceivedWeight"].Value);
			txtAccessWeight.Text = Conversion.ToString(r.Cells["PoAccessWeight"].Value);
			txtsuppwt.ReadOnly = RefDocumentTypeId != 105 || Conversion.ToDouble((object)txtReceivedWeight.Text) != 0.0;
			bool showWeightFields = RefDocumentTypeId == 105;
			Label label = lblRecwt;
			Label label2 = lblBalwt;
			TextBox textBox = txtReceivedWeight;
			bool flag2 = (txtBalWeight.Visible = showWeightFields);
			bool flag4 = (textBox.Visible = flag2);
			bool visible = (label2.Visible = flag4);
			label.Visible = visible;
			if (ValidateGrnAndInvoiceDateWithGpDate)
			{
				GpDate = Conversion.ToDateTime(r.Cells["GpDate"].Value);
				DocDate.Value = GpDate;
			}
			combgatepass.Text = Conversion.ToString(r.Cells["GpSrNo"].Value);
			if (FreightId == 0 && showWeightFields)
			{
				txtAdvanceByFactory.Enabled = false;
				txtAdvanceByFactory.Text = "0";
				txtAdvanceByParty.Enabled = false;
				txtAdvanceByParty.Text = "0";
			}
			DataTable dtBreakUp = InvGrn.ReadByGPId_InvGrnPurchaseBreakup(GPID);
			dtPurchaseBreakup.Clear();
			if (dtBreakUp.Rows.Count > 0)
			{
				LockBreakUp = true;
				foreach (DataRow row in dtBreakUp.Rows)
				{
					dtPurchaseBreakup.Rows.Add(row["Id"], row["InwardGatePassId"], row["InwardBreakupId"], row["Qty"], row["UOM"], row["GrossWeight"], row["EbWeight"], row["EBTotal"], row["NetPackSize"], row["NetWeight"], row["SupplierShortWeight"], row["SupplierRcvdWeight"], row["ScaleShortage"], row["BillWeight"]);
				}
			}
			else
			{
				foreach (DataRow row2 in GatePassInward.ReadByHeaderId_InwardPurchaseBrekup(GPID).Rows)
				{
					dtPurchaseBreakup.Rows.Add(0, row2["InwardGatePassId"], row2["Id"], row2["Qty"], row2["UOM"], row2["GrossWeight"], row2["EbWeight"], row2["EBTotal"], 0, row2["NetWeight"], 0, 0, 0, 0);
				}
			}
			if (dtPurchaseBreakup.Rows.Count == 0)
			{
				AddRowInPurchaseBreakUpGrid();
			}
			grdPurchaseBrakup.DataSource = dtPurchaseBreakup;
			grdPurchaseBrakup.RetrieveStructure();
			grdPurchaseBreakUpSettings();
			if (dtBreakUp.Rows.Count == 0)
			{
				SupplierShortageProportionInBreakUpGrid();
				ScaleShortageProportionInBreakUpGrid();
				UpdateBillWeightInBreakUpGrid();
			}
			ShowShortageCheckBoxes(RefDocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowShortageCheckBoxes(int RefDocId)
	{
		try
		{
			chkSuppShortWeight.Visible = RefDocId == 105;
			chkScaleShortWeightApply.Visible = RefDocId != 106;
			txtgwt.Enabled = RefDocumentTypeId != 105;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassRecordFill()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		try
		{
			TransporterAccountDisable();
			RefDocumentTypeId = 0;
			LabWeightCutOnId = 0;
			DataTable dtgpForGRn = GatePassInward.GetByGpNoForGRN(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt((object)GPID),
				DocumentTypeId = 51,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dtgpForGRn.Rows.Count == 0)
			{
				((Control)(object)combvehtyp).Enabled = true;
				txtvehno.Enabled = true;
				txtbltyno.Enabled = true;
				return;
			}
			((Control)(object)combvehtyp).Enabled = false;
			txtvehno.Enabled = false;
			txtbltyno.Enabled = false;
			DataRow row = dtgpForGRn.Rows[0];
			GPID = Conversion.ToInt(row["Id"]);
			((Control)(object)combvehtyp).Text = row["VehicleType"].ToString();
			txtvehno.Text = row["VehicleNo"].ToString();
			txtbltyno.Text = row["BiltyNo"].ToString();
			txtremarks.Text = row["OtherRemarks"].ToString();
			txtqty.Text = row["NoOfPackages"].ToString();
			txtsuppwt.Text = row["SupplierWeight"].ToString();
			txtfctwt.Text = row["FactoryWeight"].ToString();
			txtDifferenceWeight.Text = row["DifferenceWeight"].ToString();
			if (Conversion.ToInt(row["WareHouseName"]) > 0)
			{
				((Control)(object)CmbWareHouseName).Text = row["WareHouseName"].ToString();
			}
			RefDocumentTypeId = Conversion.ToInt(row["RefDocumentTypeId"]);
			txtAnaylstName.Text = row["AnalystName"].ToString();
			txtTicketnos.Text = row["TicketNos"].ToString();
			combsupplier.Value = Conversion.ToInt(row["SupplierCustomerId"]);
			LabWeightCutOnId = Conversion.ToInt(row["WeightCutOnId"]);
			ItemdtFillFromLab();
			ItemNameBind();
			if (RefDocumentTypeId == 41)
			{
				PurchaseOrderId = Conversion.ToInt(row["PurchaseOrderId"]);
				PurchaseOrderNo = Conversion.ToInt(row["SupplierContractCode"]);
				comborderno_Leave(null, null);
				CmbSupplierDispatchedPreBillNo.Value = Conversion.ToInt(row["SupplierDispatchId"]);
			}
			else if (RefDocumentTypeId == 105 || RefDocumentTypeId == 106)
			{
				FillWeightsAndItems(RefDocumentTypeId);
				PurchaseOrderId = 0;
				PurchaseOrderNo = 0;
			}
			Total();
			PanelBreakupMain.Enabled = RefDocumentTypeId == 105;
			combitem_Leave(null, null);
			CmbWareHouseName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillWeightsAndItems(int refDocTypeId)
	{
		double supplierWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
		double factoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
		double receivedWeight = Conversion.ToDouble((object)txtReceivedWeight.Text.Trim());
		double balWeight = 0.0;
		switch (refDocTypeId)
		{
		case 105:
			((Control)(object)txtDeliverTerm).Text = "Load";
			if (((Control)(object)txtDeliverTerm).Text == "Load")
			{
				txtGrnWeight.Text = supplierWeight.ToString();
				balWeight = supplierWeight - receivedWeight;
				txtBalWeight.Text = balWeight.ToString();
				txtgwt.Text = balWeight.ToString();
			}
			break;
		case 106:
			((Control)(object)txtDeliverTerm).Text = "Ponch";
			if (((Control)(object)txtDeliverTerm).Text == "Ponch")
			{
				balWeight = ((supplierWeight > factoryWeight) ? factoryWeight : supplierWeight);
				txtGrnWeight.Text = balWeight.ToString();
				txtBalWeight.Text = balWeight.ToString();
				txtgwt.Text = balWeight.ToString();
			}
			break;
		}
		combsupplier.ReadOnly = false;
	}

	private void combgatepass_Leave(object sender, EventArgs e)
	{
		try
		{
			if (btnSave.Visible && btnSave.Enabled)
			{
				GatePassRecordFill();
			}
			if (GPID > 0)
			{
				GetPrevoiusDataOfSelectedGP();
			}
			combsupplier_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPrevoiusDataOfSelectedGP()
	{
		try
		{
			dtPrevoiusDataOfSelectedGP.Clear();
			dtPrevoiusDataOfSelectedGP = InvGrn.GrnPrevoiusDataOfSelectedGP(UserAccount.OrganizationId, UserAccount.CompanyId, GPID, RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemdtFillFromLab()
	{
		try
		{
			dtItem.Clear();
			DataTable dtLabInfo = LabItemsBindByGpId();
			if (dtLabInfo.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow row in dtLabInfo.Rows)
			{
				dtItem.Rows.Add(row["ItemId"], row["ItemName"], row["ItemCode"], row["PoDetailId"], row["ItemWbWeight"], row["Moisture"]);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable LabItemsBindByGpId()
	{
		DataTable dtLabInfo = new DataTable();
		try
		{
			dtLabInfo = GatePassInward.getItemsFromLabOrPurchaseOrderOrAllByGpId(UserAccount.OrganizationId, UserAccount.CompanyId, GPID, 0);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return dtLabInfo;
	}

	private void btnLoadGatePass_Click(object sender, EventArgs e)
	{
		try
		{
			if (!btnSave.Visible || !btnSave.Enabled)
			{
				throw new Exception("Please Reset the form First...");
			}
			LoadGatePass = new LoadPendingGatePassForGrn(UserAccount, dtGatePassData);
			LoadGatePass.ShowDialog();
			LoadGatePass.BringToFront();
			LoadGatePassByRowFromLoader(LoadGatePass.GatePassLoadedRow);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadGatePassByRowFromLoader(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			GpQty = 0.0;
			FreightId = 0;
			if (Conversion.ToString(r.Cells["Status"].Value) != "Accepted")
			{
				throw new Exception("Status Not Accepted Please check status");
			}
			if (!LabCompulsoryNotCheckingOnGRN && Conversion.ToInt(r.Cells["LastLabId"].Value) <= 0)
			{
				throw new Exception("Lab is pending for this gate pass. Please do lab first then Load");
			}
			label13.Text = "Goods Receiving Notes (" + Conversion.ToString((object)r.Cells["OrderType"].Text) + ")";
			int refDocTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
			if (refDocTypeId == 106 && DeliveryTermEditableOnGrnForGatePurchase)
			{
				((Control)(object)txtDeliverTerm).Enabled = true;
				txtDeliverTerm.LimitToList = true;
			}
			GPID = Conversion.ToInt(r.Cells["Id"].Value);
			FreightId = Conversion.ToInt(r.Cells["FreightId"].Value);
			if (FreightId > 0)
			{
				txtcarramount.Text = Conversion.ToString(r.Cells["TotalPaidAmount"].Value);
				txtFreightDeduction.Text = Conversion.ToString(r.Cells["TotalDeductionAmount"].Value);
			}
			GpQty = Conversion.ToDouble(r.Cells["Qty"].Value);
			txtReceivedWeight.Text = Conversion.ToString(r.Cells["ReceivedWeight"].Value);
			txtAccessWeight.Text = Conversion.ToString(r.Cells["PoAccessWeight"].Value);
			txtsuppwt.ReadOnly = refDocTypeId != 105 || Conversion.ToDouble((object)txtReceivedWeight.Text) != 0.0;
			bool showWeightFields = refDocTypeId == 105;
			Label label = lblRecwt;
			Label label2 = lblBalwt;
			TextBox textBox = txtReceivedWeight;
			bool flag2 = (txtBalWeight.Visible = showWeightFields);
			bool flag4 = (textBox.Visible = flag2);
			bool visible = (label2.Visible = flag4);
			label.Visible = visible;
			if (ValidateGrnAndInvoiceDateWithGpDate)
			{
				GpDate = Conversion.ToDateTime(r.Cells["GpDate"].Value);
				DocDate.Value = GpDate;
			}
			combgatepass.Text = Conversion.ToString(r.Cells["GpSrNo"].Value);
			DataTable dtBreakUp = InvGrn.ReadByGPId_InvGrnPurchaseBreakup(GPID);
			dtPurchaseBreakup.Clear();
			if (dtBreakUp.Rows.Count > 0)
			{
				LockBreakUp = true;
				foreach (DataRow row in dtBreakUp.Rows)
				{
					dtPurchaseBreakup.Rows.Add(row["Id"], row["InwardGatePassId"], row["InwardBreakupId"], row["Qty"], row["UOM"], row["GrossWeight"], row["EbWeight"], row["EBTotal"], row["NetPackSize"], row["NetWeight"], row["SupplierShortWeight"], row["SupplierRcvdWeight"], row["ScaleShortage"], row["BillWeight"]);
				}
			}
			else
			{
				foreach (DataRow row2 in GatePassInward.ReadByHeaderId_InwardPurchaseBrekup(GPID).Rows)
				{
					dtPurchaseBreakup.Rows.Add(0, row2["InwardGatePassId"], row2["Id"], row2["Qty"], row2["UOM"], row2["GrossWeight"], row2["EbWeight"], row2["EBTotal"], 0, row2["NetWeight"], 0, 0, 0, 0);
				}
			}
			if (dtPurchaseBreakup.Rows.Count == 0)
			{
				AddRowInPurchaseBreakUpGrid();
			}
			grdPurchaseBrakup.DataSource = dtPurchaseBreakup;
			grdPurchaseBrakup.RetrieveStructure();
			grdPurchaseBreakUpSettings();
			if (dtBreakUp.Rows.Count == 0)
			{
				SupplierShortageProportionInBreakUpGrid();
				ScaleShortageProportionInBreakUpGrid();
				UpdateBillWeightInBreakUpGrid();
			}
			ShowShortageCheckBoxes(refDocTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeightBusinessValidations(double GridGrossWeight, double GridBillWeight, double GridStockWeight, bool IsCalculating = false)
	{
		double BalWeight = 0.0;
		if (!IsCalculating)
		{
			BalWeight = Conversion.ToDouble((object)txtBalWeight.Text.Trim());
		}
		double ReceivedWeight = Conversion.ToDouble((object)txtReceivedWeight.Text.Trim());
		Conversion.ToDouble((object)txtFreightDeduction.Text);
		Conversion.ToDouble((object)txtChargeToPartyAmountFr.Text);
		double SupplierWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
		double FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
		double FactoryWeightWithTolerance = FactoryWeight + BillWeightAndStockWeightDifferenceTolerance;
		double DifferenceWeight = SupplierWeight - FactoryWeightWithTolerance;
		double SupplierWeightItemWise = 0.0;
		string deliveryTerm = ((Control)(object)txtDeliverTerm).Text;
		if (deliveryTerm == "Load" || deliveryTerm == "Load & PartyWeight")
		{
			_ = 1;
		}
		else
			_ = deliveryTerm == "Ponch & PartyWeight";
		if (deliveryTerm == "Load & FactoryWeight")
		{
			_ = 1;
		}
		else
			_ = deliveryTerm == "Ponch & FactoryWeight";
		bool ValidLesserWeight = deliveryTerm == "Ponch";
		if (DeductionPolicyForGrnIsOn && RefDocumentTypeId == 41 && ValidLesserWeight)
		{
			DeductionPolicyForGrn(GridGrossWeight);
			return;
		}
		if (RefDocumentTypeId == 105)
		{
			if (!IsCalculating)
			{
				if (!(GridGrossWeight > BalWeight))
				{
					return;
				}
				throw new Exception($"GrossWeight {GridGrossWeight} cannot be greater than Balance Weight {BalWeight}");
			}
			txtGrnWeight.Text = SupplierWeight.ToString("#,##.##");
			txtBalWeight.Text = (SupplierWeight - ReceivedWeight).ToString("#,##.##");
			if (((UltraGridBase)CmbPackUom).ActiveRow != null && Conversion.ToInt(CmbPackUom.Value) > 0)
			{
				txtgwt.Text = (Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value) * Conversion.ToDouble((object)txtqty.Text)).ToString();
			}
			else
			{
				txtgwt.Text = 0.ToString();
			}
			return;
		}
		if (FreightId == 0)
		{
			ValidateWeights(GridGrossWeight, GridBillWeight, GridStockWeight, IsCalculating);
		}
		else if (DifferenceWeight < 0.0 && !ExcludeWeightShortageBusinessOnGrn)
		{
			if (IsCalculating)
			{
				txtGrnWeight.Text = SupplierWeight.ToString("#,##");
				txtBalWeight.Text = SupplierWeight.ToString("#,##");
				double ItemWeight = 0.0;
				if (((UltraGridBase)combitem).ActiveRow != null && Conversion.ToInt(combitem.Value) > 0)
				{
					ItemWeight = Conversion.ToDouble(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemWbWeight"].Value);
				}
				if (ItemWeight > 0.0)
				{
					SupplierWeightItemWise = ItemWeight / FactoryWeight * SupplierWeight;
					txtgwt.Text = SupplierWeightItemWise.ToString("#,##");
				}
				else
				{
					txtgwt.Text = SupplierWeight.ToString("#,##");
				}
			}
			else
			{
				ValidateWeights(GridGrossWeight, GridBillWeight, GridStockWeight, IsCalculating);
			}
		}
		else
		{
			ValidateWeights(GridGrossWeight, GridBillWeight, GridStockWeight, IsCalculating);
		}
		if (IsCalculating)
		{
			int ItemId = Conversion.ToInt(combitem.Value);
			TotalGrossWeight = Conversion.ToDouble((object)txtGrnWeight.Text);
			double ItemTotalWbWeight = InfragisticsHelper.GetColumnByIdTotal(combitem, "ItemWbWeight", ItemId, "Id");
			if (TotalGrossWeight == SupplierWeight)
			{
				ItemTotalWbWeight = ItemTotalWbWeight / FactoryWeight * SupplierWeight;
			}
			double GrdGrossWeight = GridEX_Helper.GetColumnSumWithWhere(grd, "GrossWight", "ItemId", Conversion.ToString((object)ItemId));
			if (btnUpdateDetail.Visible)
			{
				GrdGrossWeight -= Conversion.ToDouble(dtdetail.Rows[updateDetailIndex]["GrossWight"]);
			}
			if (ItemTotalWbWeight == 0.0)
			{
				ItemTotalWbWeight = TotalGrossWeight;
				GrdGrossWeight = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["GrossWight"], (AggregateFunction)2));
			}
			double GrossWeight = Conversion.ToDouble((object)(ItemTotalWbWeight - GrdGrossWeight));
			txtgwt.Text = GrossWeight.ToString("#,##.##");
		}
	}

	private void ValidationforQty()
	{
		try
		{
			var groupedBreakup = (from row in dtPurchaseBreakup.AsEnumerable()
				group row by new
				{
					PackUom = (row.IsNull("NetPackSize") ? 0.0 : Conversion.ToDouble((object)row.Field<double>("NetPackSize")))
				} into @group
				select new
				{
					PackUom = @group.Key.PackUom,
					TotalQty = @group.Sum((DataRow row) => (!row.IsNull("Qty")) ? row.Field<double>("Qty") : 0.0)
				}).ToList();
			foreach (var item in (from row in dtdetail.AsEnumerable()
				group row by new
				{
					PackUom = (row.IsNull("UOMEquivalent") ? 0.0 : Conversion.ToDouble((object)row.Field<double>("UOMEquivalent")))
				} into @group
				select new
				{
					PackUom = @group.Key.PackUom,
					TotalQty = @group.Sum((DataRow row) => (!row.IsNull("Qty")) ? row.Field<double>("Qty") : 0.0)
				}).ToList())
			{
				if (!dtPurchaseBreakup.AsEnumerable().Any((DataRow row) => row.Field<double>("NetPackSize") == Conversion.ToDouble((object)item.PackUom)))
				{
					throw new Exception($"Uom {item.PackUom} In Detail Grid ,does not exists in BreakUp Grid");
				}
				double QtyPreviousData = 0.0;
				if (dtPrevoiusDataOfSelectedGP.Rows.Count > 0)
				{
					QtyPreviousData = Conversion.ToDouble(dtPrevoiusDataOfSelectedGP.Compute("SUM(ItemQty)", "PackUomEquivalent = " + Conversion.ToString((object)item.PackUom)));
				}
				double QtyBreakUp = (from b in groupedBreakup
					where b.PackUom == item.PackUom
					select b.TotalQty).FirstOrDefault();
				double TotalDetailQty = item.TotalQty + QtyPreviousData;
				if (TotalDetailQty > QtyBreakUp)
				{
					if (QtyPreviousData > 0.0)
					{
						throw new Exception($"TotalQty {TotalDetailQty} (Sum Of Current Detail Grid Qty {item.TotalQty} and\nOther Grn (Of Same GatePass) Qty {QtyPreviousData}) Against PackSize {item.PackUom},\nCan't be Greater than BreakUp Grid TotaQty {QtyBreakUp}");
					}
					throw new Exception($"TotalQty {TotalDetailQty} in Detail Grid Against PackSize {item.PackUom},\nCan't be Greater than BreakUp Grid TotaQty {QtyBreakUp}");
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void DeductionPolicyForGrn(double GrossWeight)
	{
		double factoryWeight = Conversion.ToDouble((object)txtfctwt.Text);
		if (RefDocumentTypeId == 41 && DeductionPolicyForGrnIsOn && ((Control)(object)txtDeliverTerm).Text == "Ponch")
		{
			int policyId = 0;
			string policyName = "";
			double supplierWeight = Conversion.ToDouble((object)txtsuppwt.Text);
			double diffWeight = factoryWeight - supplierWeight;
			if (diffWeight > 0.0)
			{
				DataTable dt = DeductionPolicyForGrn.GetPolicyForGrn(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, UserAccount.BranchesId, DocDate.Value, diffWeight);
				if (dt.Rows.Count > 0)
				{
					policyId = Conversion.ToInt(dt.Rows[0]["PolicyTypeId"]);
					policyName = " Policy is " + Conversion.ToString(dt.Rows[0]["ConditionDescription"]);
				}
			}
			double finalBillWt = 0.0;
			switch (policyId)
			{
			case 0:
			case 1:
				finalBillWt = factoryWeight;
				break;
			case 2:
				finalBillWt = factoryWeight - diffWeight / 2.0;
				break;
			case 3:
				finalBillWt = supplierWeight;
				break;
			}
			txtBalWeight.Text = finalBillWt.ToString();
			double GrdGrossWeight = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["GrossWight"], (AggregateFunction)2));
			if (btnUpdateDetail.Visible)
			{
				GrdGrossWeight -= Conversion.ToDouble(dtdetail.Rows[updateDetailIndex]["GrossWight"]);
			}
			txtgwt.Text = Conversion.ToString((object)(finalBillWt - GrdGrossWeight));
			policyId = ((policyId == 0) ? 1 : policyId);
			if (GrossWeight > 0.0 && GrossWeight != finalBillWt)
			{
				throw new Exception($"GrossWeight {GrossWeight} Should Equal To FinalWeight {finalBillWt}...{policyName}");
			}
			if (!txtBalWeight.Visible)
			{
				lblBalwt.Visible = true;
				txtBalWeight.Visible = true;
			}
		}
		else if (DeductionPolicyForGrnIsOn)
		{
			txtBalWeight.Text = factoryWeight.ToString();
		}
	}

	private void ValidateWeights(double GridGrossWeight, double GridBillWeight, double GridStockWeight, bool IsCalculating)
	{
		double FreightDeductionAmount = Conversion.ToDouble((object)txtFreightDeduction.Text) + Conversion.ToDouble((object)txtChargeToPartyAmountFr.Text);
		Conversion.ToDouble((object)txtBalWeight.Text.Trim());
		double SupplierWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
		double FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
		double FactoryWeightWithTolerance = FactoryWeight + BillWeightAndStockWeightDifferenceTolerance;
		double DifferenceWeight = SupplierWeight - FactoryWeightWithTolerance;
		double DifferenceWeightWithoutTolerance = SupplierWeight - FactoryWeight;
		double ItemWeight = 0.0;
		double SupplierWeightItemWise = 0.0;
		double FactoryWeightItemWise = 0.0;
		string deliveryTerm = ((Control)(object)txtDeliverTerm).Text;
		bool num = deliveryTerm == "Load" || deliveryTerm == "Load & PartyWeight" || deliveryTerm == "Ponch & PartyWeight";
		bool ValidFactoryWeight = deliveryTerm == "Load & FactoryWeight" || deliveryTerm == "Ponch & FactoryWeight";
		bool ValidLesserWeight = deliveryTerm == "Ponch";
		if (num)
		{
			if (DifferenceWeight > 0.0)
			{
				if (FreightDeductionAmount > 0.0 || ExcludeWeightShortageBusinessOnGrn)
				{
					if (IsCalculating)
					{
						txtGrnWeight.Text = SupplierWeight.ToString("#,##");
						txtBalWeight.Text = SupplierWeight.ToString("#,##");
						if (((UltraGridBase)combitem).ActiveRow != null && Conversion.ToInt(combitem.Value) > 0)
						{
							ItemWeight = Conversion.ToDouble(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemWbWeight"].Value);
						}
						if (ItemWeight > 0.0)
						{
							SupplierWeightItemWise = ItemWeight / FactoryWeight * SupplierWeight;
							txtgwt.Text = SupplierWeightItemWise.ToString("#,##");
						}
						else
						{
							txtgwt.Text = SupplierWeight.ToString("#,##");
						}
					}
					else if (GridGrossWeight != SupplierWeight)
					{
						string message = $"GrossWeight {GridGrossWeight} must be equal to Supplier Weight {SupplierWeight}.\n";
						if (FreightDeductionAmount > 0.0)
						{
							message += "Freight deduction is applied.\n";
						}
						message = message + "Delivery term is " + deliveryTerm + ".";
						throw new Exception(message);
					}
				}
				else if (IsCalculating)
				{
					txtGrnWeight.Text = FactoryWeight.ToString("#,##");
					txtBalWeight.Text = FactoryWeight.ToString("#,##");
					if (((UltraGridBase)combitem).ActiveRow != null && Conversion.ToInt(combitem.Value) > 0)
					{
						ItemWeight = Conversion.ToDouble(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemWbWeight"].Value);
					}
					if (ItemWeight > 0.0)
					{
						FactoryWeightItemWise = ItemWeight;
						txtgwt.Text = FactoryWeightItemWise.ToString("#,##");
					}
					else
					{
						txtgwt.Text = FactoryWeight.ToString("#,##");
					}
				}
				else
				{
					string Msg = $"Shortage {DifferenceWeightWithoutTolerance} is Greater than Tollerance Weight {BillWeightAndStockWeightDifferenceTolerance}," + "So Freight Deduction is Required Or You Have To Make Gross Weight Equal To Facotry Weight\nAnd DeliveryTerm is " + ((Control)(object)txtDeliverTerm).Text;
					if (GridGrossWeight == SupplierWeight && GridGrossWeight != FactoryWeight)
					{
						throw new Exception(Msg);
					}
					if (GridGrossWeight != FactoryWeight)
					{
						throw new Exception(Msg);
					}
				}
			}
			else if (IsCalculating)
			{
				txtGrnWeight.Text = SupplierWeight.ToString("#,##");
				txtBalWeight.Text = SupplierWeight.ToString("#,##");
				if (((UltraGridBase)combitem).ActiveRow != null && Conversion.ToInt(combitem.Value) > 0)
				{
					ItemWeight = Conversion.ToDouble(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemWbWeight"].Value);
				}
				if (ItemWeight > 0.0)
				{
					SupplierWeightItemWise = ItemWeight / FactoryWeight * SupplierWeight;
					txtgwt.Text = SupplierWeightItemWise.ToString("#,##");
				}
				else
				{
					txtgwt.Text = SupplierWeight.ToString("#,##");
				}
			}
			else if (GridGrossWeight != SupplierWeight)
			{
				string message2 = $"GrossWeight {GridGrossWeight} must be equal to Supplier Weight {SupplierWeight}.\n";
				if (FreightDeductionAmount > 0.0)
				{
					message2 += "Freight deduction is applied.\n";
				}
				message2 = message2 + "Delivery term is " + deliveryTerm + ".";
				throw new Exception(message2);
			}
		}
		else if (ValidLesserWeight)
		{
			if (SupplierWeight > FactoryWeight)
			{
				if (IsCalculating)
				{
					txtGrnWeight.Text = FactoryWeight.ToString("#,##");
					txtBalWeight.Text = FactoryWeight.ToString("#,##");
					if (((UltraGridBase)combitem).ActiveRow != null && Conversion.ToInt(combitem.Value) > 0)
					{
						ItemWeight = Conversion.ToDouble(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemWbWeight"].Value);
					}
					if (ItemWeight > 0.0)
					{
						FactoryWeightItemWise = ItemWeight;
						txtgwt.Text = FactoryWeightItemWise.ToString("#,##");
					}
					else
					{
						txtgwt.Text = FactoryWeight.ToString("#,##");
					}
				}
				else if (GridGrossWeight != FactoryWeight)
				{
					throw new Exception($"GrossWeight {GridGrossWeight} Must be Equal To Factory Weight {FactoryWeight} Because Factory Weight is Lesser\n" + "And DeliveryTerm is " + ((Control)(object)txtDeliverTerm).Text);
				}
			}
			else if (IsCalculating)
			{
				txtGrnWeight.Text = SupplierWeight.ToString("#,##");
				txtBalWeight.Text = SupplierWeight.ToString("#,##");
				if (((UltraGridBase)combitem).ActiveRow != null && Conversion.ToInt(combitem.Value) > 0)
				{
					ItemWeight = Conversion.ToDouble(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemWbWeight"].Value);
				}
				if (ItemWeight > 0.0)
				{
					SupplierWeightItemWise = ItemWeight / FactoryWeight * SupplierWeight;
					txtgwt.Text = SupplierWeightItemWise.ToString("#,##");
				}
				else
				{
					txtgwt.Text = SupplierWeight.ToString("#,##");
				}
			}
			else if (GridGrossWeight != SupplierWeight)
			{
				throw new Exception($"GrossWeight {GridGrossWeight} Must be Equal To Supplier Weight {SupplierWeight} Because Supplier Weight is Lesser\n" + "And DeliveryTerm is " + ((Control)(object)txtDeliverTerm).Text);
			}
		}
		else
		{
			if (!ValidFactoryWeight)
			{
				return;
			}
			if (IsCalculating)
			{
				txtGrnWeight.Text = FactoryWeight.ToString("#,##");
				txtBalWeight.Text = FactoryWeight.ToString("#,##");
				if (((UltraGridBase)combitem).ActiveRow != null && Conversion.ToInt(combitem.Value) > 0)
				{
					ItemWeight = Conversion.ToDouble(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemWbWeight"].Value);
				}
				if (ItemWeight > 0.0)
				{
					FactoryWeightItemWise = ItemWeight;
					txtgwt.Text = FactoryWeightItemWise.ToString("#,##");
				}
				else
				{
					txtgwt.Text = FactoryWeight.ToString("#,##");
				}
			}
			else if (GridGrossWeight != FactoryWeight)
			{
				throw new Exception($"GrossWeight {GridGrossWeight} Must be Equal To Factory Weight {FactoryWeight} Because\n" + "DeliveryTerm is " + ((Control)(object)txtDeliverTerm).Text);
			}
		}
	}

	private void Insert()
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0788: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Expected O, but got Unknown
		//IL_0e56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5d: Expected O, but got Unknown
		//IL_0c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c54: Expected O, but got Unknown
		try
		{
			if (RecId > 0 && RefferedInInvoice)
			{
				MessageBox.Show("This Document is referred in invoice. So you can't update this record.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			int Count = grd.GetRows().Count();
			if (Count == 0)
			{
				MessageBox.Show("Grid Record Not Found");
				return;
			}
			if (DeductionPolicyForGrnIsOn && Count > 1)
			{
				throw new Exception("Multiple rows is not allowed while the deduction policy is active.");
			}
			if (!FormValidation())
			{
				return;
			}
			InvGrn GRN = new InvGrn();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				GRN.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (RefDocumentTypeId == 105)
			{
				ValidationforQty();
				double ScaleShortWeight = GridEX_Helper.GetColumnSum(grd, "ScaleShortWt");
				GridEX_Helper.GetColumnSum(grd, "SuppShortWt");
				if ((ScaleShortWeight > 0.0 && !chkScaleShortWeightApply.Checked && MessageBox.Show("You have not Apply Scale_Short_Weight Deduction In bill Weight...Are you sure to Continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) || (ScaleShortWeight > 0.0 && !chkSuppShortWeight.Checked && MessageBox.Show("You have not Apply Supplier_Short_Weight Deduction In bill Weight...Are you sure to Continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
				{
					return;
				}
			}
			if (btnSave.Visible && btnSave.Enabled && ValidateGrnAndInvoiceDateWithGpDate && Conversion.ToDateTime((object)DocDate.Value).Date != GpDate.Date)
			{
				throw new Exception("GrnDate Should be Equal to GpDate");
			}
			StockWeightCalculationInGrid();
			GRN.OrganizationId = UserAccount.OrganizationId;
			GRN.CompanyId = UserAccount.CompanyId;
			GRN.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			GRN.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			GRN.DocumentTypeId = 46;
			GRN.ScreenName = Conversion.ToString((object)ScreenName);
			GRN.ReferencePartyId = 0;
			GRN.StockPartyId = 0;
			GRN.EntryDate = DateTime.Now;
			GRN.ModifyDate = DateTime.Now;
			GRN.EntryUser = UserAccount.ID;
			GRN.ModifyUser = UserAccount.ID;
			GRN.DocDate = DocDate.Value;
			GRN.DocNo = Convert.ToInt32(txtdocno.Text.Trim());
			GRN.SupplierCustomerId = Conversion.ToInt(combsupplier.Value);
			GRN.InwardGatePassId = Conversion.ToInt((object)GPID);
			GRN.GpNo = Conversion.ToInt((object)combgatepass.Text);
			GRN.DeliveryTerm = Conversion.ToString((object)((Control)(object)txtDeliverTerm).Text.Trim());
			GRN.VehicleType = Conversion.ToString((object)((Control)(object)combvehtyp).Text);
			GRN.VehicleNo = Conversion.ToString((object)txtvehno.Text.Trim());
			GRN.BiltyNo = txtbltyno.Text.Trim();
			GRN.PartyWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
			GRN.FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			GRN.ScaleShortWeightApply = Conversion.ToBool((object)chkScaleShortWeightApply.Checked);
			GRN.SupplierShortWeightApply = Conversion.ToBool((object)chkSuppShortWeight.Checked);
			GRN.RemarksHeader = txtremarks.Text.Trim();
			GRN.SupplierDispatchId = Conversion.ToInt(CmbSupplierDispatchedPreBillNo.Value);
			GRN.BiltyFreight = Conversion.ToDouble((object)txtBiltyFreight.Text);
			GRN.ScaleCharges = Conversion.ToDouble((object)txtScaleCharges.Text);
			GRN.AdvanceByPartyFreight = Conversion.ToDouble((object)txtAdvanceByParty.Text);
			GRN.AdvanceByFactoryFreight = Conversion.ToDouble((object)txtAdvanceByFactory.Text);
			GRN.OtherFreightDeduction = Conversion.ToDouble((object)txtOtherDeduction.Text);
			if (FreightId == 0 && RefDocumentTypeId == 105 && (GRN.AdvanceByFactoryFreight > 0.0 || GRN.AdvanceByPartyFreight > 0.0))
			{
				txtAdvanceByParty.Text = "0";
				txtAdvanceByFactory.Text = "0";
				throw new Exception("In Case Of Market Purchase Advance by Factory or Advance by Party can't be greater than 0");
			}
			GRN.ShortageAmountFV = Conversion.ToDouble((object)txtShortageAmountFr.Text);
			GRN.DiscountAmountFV = Conversion.ToDouble((object)txtDiscountAmountFr.Text);
			GRN.ChargeToPartyAmountFV = Conversion.ToDouble((object)txtChargeToPartyAmountFr.Text);
			GRN.FreightDeduction = Conversion.ToDouble((object)txtFreightDeduction.Text);
			GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				GRN.TransporterId = Conversion.ToInt(CmbTransport.Value);
			}
			else
			{
				GRN.TransporterSupCustId = Conversion.ToInt(((UltraDropDownBase)CmbTransport).SelectedRow.Cells[3].Value);
				GRN.TransporterId = Conversion.ToInt(CmbTransport.Value);
			}
			if (GRN.CarriageAmount > 0.0)
			{
				if (Conversion.ToDouble((object)txtBiltyFreight.Text) <= 0.0)
				{
					MessageBox.Show("Bilty Freight field required");
					txtBiltyFreight.Focus();
					return;
				}
				if (((UltraGridBase)CmbTransport).ActiveRow == null || Conversion.ToInt(CmbTransport.Value) == 0)
				{
					MessageBox.Show("Transporter Account field required");
					CmbTransport.Focus();
					return;
				}
			}
			if (GRN.FreightDeduction > 0.0)
			{
				if (((UltraGridBase)CmbTransport).ActiveRow == null || Conversion.ToInt(CmbTransport.Value) == 0)
				{
					MessageBox.Show("Transporter Account field required");
					CmbTransport.Focus();
					return;
				}
				if (Conversion.ToDouble((object)txtBiltyFreight.Text) <= 0.0)
				{
					MessageBox.Show("Bilty Freight field required");
					txtBiltyFreight.Focus();
					return;
				}
				if (GRN.FreightDeduction > Conversion.ToDouble((object)txtBiltyFreight.Text))
				{
					MessageBox.Show("Deduction Can't be greater than Bilty Freight");
					txtFreightDeduction.Focus();
					return;
				}
				if (GRN.CarriageAmount <= 0.0 && FreightId == 0)
				{
					MessageBox.Show("Freight Amount field required");
					txtcarramount.Focus();
					return;
				}
			}
			GRN.invGrnDetail = new List<InvGrnDetail>();
			GRN.InvgrnDetailEmptyBagslist = new List<InvgrnDetailEmptyBags>();
			GRN.InvGrnPurchaseBreakUplist = new List<InvGrnPurchaseBreakUp>();
			double GrnQty = 0.0;
			double GrossWeight = 0.0;
			double EmptyBagsRecQty = 0.0;
			double EmptyBagsPurQty = 0.0;
			double TotalBIllWeight = 0.0;
			double TotalStockWeight = 0.0;
			bool OtherThanOpenBulkExist = false;
			double FreightFromGrid = 0.0;
			bool existsFOC = GridEX_Helper.IsAnyCellValueHaveSameValue(grdEmptyBags, "Type", "3");
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				InvGrnDetail detail = new InvGrnDetail();
				detail.Id = ((RecId != 0) ? Conversion.ToInt(r3.Cells["Id"].Value) : 0);
				FillGdnDetailListCommonForInsertAndDelete(detail, r3);
				if (detail.WarehouseId == 0)
				{
					throw new Exception("Warehouse Required in Detail Grid And row No: " + (r3.RowIndex + 1));
				}
				if (detail.ItemId == 0)
				{
					throw new Exception("Item Required in Detail Grid And row No: " + (r3.RowIndex + 1));
				}
				if (detail.CropYearId == 0)
				{
					throw new Exception("CropYear Required in Detail Grid And row No: " + (r3.RowIndex + 1));
				}
				if (detail.JobLotId == 0)
				{
					throw new Exception("JobLot Required in Detail Grid And row No: " + (r3.RowIndex + 1));
				}
				if (detail.PackingTypeId == 0)
				{
					throw new Exception("PackingType Required in Detail Grid And row No: " + (r3.RowIndex + 1));
				}
				if (detail.ItemUomId == 0)
				{
					throw new Exception("Uom Required in Detail Grid And row No: " + (r3.RowIndex + 1));
				}
				if (detail.ItemQty == 0.0)
				{
					throw new Exception("Qty Required in Detail Grid And row No: " + (r3.RowIndex + 1));
				}
				if (detail.GrossWeight == 0.0)
				{
					throw new Exception("GrossWeight Required in Detail Grid And row No: " + (r3.RowIndex + 1));
				}
				if (detail.NetBillWeight == 0.0)
				{
					throw new Exception("NetBillWeight Required in Detail Grid And row No: " + (r3.RowIndex + 1));
				}
				if (detail.StockWeight == 0.0)
				{
					throw new Exception("StockWeight Required in Detail Grid And row No: " + (r3.RowIndex + 1));
				}
				if (RefDocumentTypeId != 105 && existsFOC && detail.EBWTotal == 0.0)
				{
					throw new Exception("EmptyBags Weight cannot be greater than 0 when Empty Bags Case is [Purchase Against Weight] in Detail Grid And row No: " + (r3.RowIndex + 1));
				}
				if (detail.PackingTypeId != 5)
				{
					OtherThanOpenBulkExist = true;
					GrnQty += detail.ItemQty;
				}
				GrossWeight += detail.GrossWeight;
				FreightFromGrid += detail.FreightAmount;
				TotalBIllWeight += detail.NetBillWeight;
				TotalStockWeight += detail.StockWeight;
				GRN.invGrnDetail.Add(detail);
			}
			if (RefDocumentTypeId == 105)
			{
				GRN.OtherCharges = Conversion.ToDouble((object)txtcarramount.Text);
				GRN.CarriageAmount = FreightFromGrid;
			}
			WeightBusinessValidations(GrossWeight, TotalBIllWeight, TotalStockWeight);
			if (grdEmptyBags.RowCount > 0)
			{
				int EBStatus = 0;
				rows = grdEmptyBags.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(item.Cells["Type"].Value) > 0 && Conversion.ToInt(item.Cells["ItemId"].Value) > 0)
					{
						if (Conversion.ToInt(item.Cells["ItemId"].Value) == 0)
						{
							MessageBox.Show("Please Select Item First in Empty Bag Detail");
							return;
						}
						if (Conversion.ToInt(item.Cells["Type"].Value) == 0)
						{
							MessageBox.Show("Please Select Type First in Empty Bag Detail");
							return;
						}
						if (Conversion.ToInt(item.Cells["Condition"].Value) == 0)
						{
							MessageBox.Show("Please Select Bags_Condition First in Empty Bag Detail");
							return;
						}
						EmptyBagsRecQty += Conversion.ToDouble(item.Cells["RecQty"].Value);
						EmptyBagsPurQty += Conversion.ToDouble(item.Cells["PurQty"].Value);
					}
				}
				if (EmptyBagsInformationComp && OtherThanOpenBulkExist)
				{
					double EmpBagsQty2 = EmptyBagsRecQty + EmptyBagsPurQty;
					if (GrnQty != EmpBagsQty2)
					{
						throw new Exception($"Empty Bags Qty must be equal to GrnQty\nGrn Qty is {GrnQty} (Without OpenBulk) and Empty Bags Qty is {EmpBagsQty2}");
					}
				}
				rows = grdEmptyBags.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToInt(r2.Cells["ItemId"].Value) <= 0 || Conversion.ToInt(r2.Cells["Type"].Value) <= 0)
					{
						continue;
					}
					InvgrnDetailEmptyBags obj = new InvgrnDetailEmptyBags();
					obj.PurchaseOrderId = Conversion.ToInt(r2.Cells["OrderId"].Value);
					obj.TypeId = Conversion.ToInt(r2.Cells["Type"].Value);
					obj.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					obj.BagsCondition = Conversion.ToInt(r2.Cells["Condition"].Value);
					obj.ReceivedQty = Conversion.ToDouble(r2.Cells["RecQty"].Value);
					obj.PurchaseQty = Conversion.ToDouble(r2.Cells["PurQty"].Value);
					obj.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					EBStatus = obj.TypeId;
					if (obj.ReceivedQty > 0.0 || obj.PurchaseQty > 0.0)
					{
						GRN.InvgrnDetailEmptyBagslist.Add(obj);
					}
					switch (EBStatus)
					{
					case 1:
						if (EmptyBagsRecQty == 0.0 && EmptyBagsPurQty == 0.0)
						{
							throw new Exception("Received Qty Or Purchase Qty Required in EmptyBags Detail");
						}
						break;
					case 2:
					case 3:
						if (EmptyBagsPurQty == 0.0)
						{
							throw new Exception("Purchase Qty Required in EmptyBags Detail");
						}
						break;
					case 4:
					case 5:
						if (EmptyBagsRecQty == 0.0)
						{
							throw new Exception("Received Qty Required in EmptyBags Detail");
						}
						break;
					}
					if (EBStatus > 0)
					{
						double EmpBagsQty = EmptyBagsRecQty + EmptyBagsPurQty;
						if (GrnQty != EmpBagsQty)
						{
							throw new Exception("Empty Bags Quantity must be equal to GrnQty");
						}
					}
				}
			}
			if (RefDocumentTypeId == 105)
			{
				if (grdPurchaseBrakup.GetRows().Count() <= 0)
				{
					MessageBox.Show("Purchase BreakUp Required When Doing Market Purchase Entry");
					return;
				}
				rows = grdPurchaseBrakup.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvGrnPurchaseBreakUp rr = new InvGrnPurchaseBreakUp();
					rr.Id = Conversion.ToInt(r.Cells["Id"].Value);
					rr.InwardGatePassId = GRN.InwardGatePassId;
					rr.InwardBreakupId = Conversion.ToInt(r.Cells["InwardBreakupId"].Value);
					rr.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					rr.UOM = Conversion.ToDecimal(r.Cells["UOM"].Value);
					rr.GrossWeight = Conversion.ToDecimal(r.Cells["GrossWeight"].Value);
					if (rr.GrossWeight == 0m)
					{
						throw new Exception("GrossWeight not found in Empty Bags Weight Breakup grid.");
					}
					rr.EbWeight = Conversion.ToDecimal(r.Cells["EbWeight"].Value);
					rr.EBTotal = Conversion.ToDecimal(r.Cells["EBTotal"].Value);
					rr.NetPackSize = Conversion.ToDecimal(r.Cells["NetPackSize"].Value);
					rr.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					rr.SupplierShortWeight = Conversion.ToDecimal(r.Cells["SuppShortWt"].Value);
					rr.SupplierRcvdWeight = Conversion.ToDecimal(r.Cells["SupplierRcvdWeight"].Value);
					rr.ScaleShortage = Conversion.ToDecimal(r.Cells["ScaleShortage"].Value);
					rr.BillWeight = Conversion.ToDecimal(r.Cells["BillWeight"].Value);
					GRN.InvGrnPurchaseBreakUplist.Add(rr);
				}
			}
			GRN.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			GRN.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvGrn>(GRN, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			GRN.AttachmentsValues = AttachmentsValues;
			GRN.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = InvGrn.Save(GRN);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully [" + GRN.DocNo + "]");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 46, base.Name);
			}
			else
			{
				MessageBox.Show("Record Save Successfully [" + GRN.DocNo + "]");
			}
			if (WagesStatus && WagesActiveOrInActive)
			{
				frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
				frmwagesBillHeader.RefDocTypeId = 46;
				frmwagesBillHeader.RefDocId = success;
				frmwagesBillHeader.GrossWeightTotal = GrossWeight;
				frmwagesBillHeader.ShowDialog();
				frmwagesBillHeader.BringToFront();
			}
			reset();
			txtEbUnitStock.Text = "0";
			txtEbTotalStock.Text = "0";
			if (ChkBox.Checked)
			{
				GenerateReport(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
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
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Expected O, but got Unknown
		try
		{
			reset();
			RecId = ID;
			InvGrn invGrn = InvGrn.GetByID(RecId);
			if (invGrn == null)
			{
				return;
			}
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtdocno.Text = invGrn.DocNo.ToString();
			DocDate.Value = invGrn.DocDate;
			tabControl1.SelectedIndex = 0;
			combsupplier.Value = invGrn.SupplierCustomerId;
			combsupplier.ReadOnly = true;
			CmbTransport.Value = invGrn.TransporterId;
			txtsuppwt.Text = invGrn.PartyWeight.ToString();
			txtfctwt.Text = invGrn.FactoryWeight.ToString();
			txtAccessWeight.Text = invGrn.AccessWeight.ToString();
			combgatepass.Text = invGrn.GpNo.ToString();
			((Control)(object)combvehtyp).Text = invGrn.VehicleType;
			txtvehno.Text = invGrn.VehicleNo;
			txtbltyno.Text = invGrn.BiltyNo;
			txtremarks.Text = invGrn.RemarksHeader;
			txtTicketnos.Text = invGrn.TicketNos;
			((Control)(object)txtDeliverTerm).Text = Conversion.ToString((object)invGrn.DeliveryTerm);
			GPID = Conversion.ToInt((object)invGrn.InwardGatePassId);
			RefDocumentTypeId = invGrn.RefDocumentTypeId;
			FreightId = invGrn.FreightId;
			chkScaleShortWeightApply.Checked = invGrn.ScaleShortWeightApply;
			chkSuppShortWeight.Checked = invGrn.SupplierShortWeightApply;
			LabWeightCutOnId = invGrn.LabWeightCutOnId;
			txtBiltyFreight.Text = invGrn.BiltyFreight.ToString();
			txtScaleCharges.Text = invGrn.ScaleCharges.ToString();
			txtAdvanceByParty.Text = invGrn.AdvanceByPartyFreight.ToString();
			txtAdvanceByFactory.Text = invGrn.AdvanceByFactoryFreight.ToString();
			txtOtherDeduction.Text = invGrn.OtherFreightDeduction.ToString();
			txtShortageAmountFr.Text = invGrn.ShortageAmountFV.ToString();
			txtDiscountAmountFr.Text = invGrn.DiscountAmountFV.ToString();
			txtRemainingAmountFr.Text = (invGrn.ShortageAmountFV - invGrn.DiscountAmountFV).ToString();
			txtChargeToPartyAmountFr.Text = invGrn.ChargeToPartyAmountFV.ToString();
			txtFreightDeduction.Text = invGrn.FreightDeduction.ToString();
			txtcarramount.Text = invGrn.CarriageAmount.ToString();
			AttachmentsValues = invGrn.AttachmentsValues;
			CustomAttachmentsValues = invGrn.CustomAttachmentsValues;
			combsupplier_Leave(null, null);
			CmbSupplierDispatchedPreBillNo.Value = invGrn.SupplierDispatchId;
			TransporterAccountDisable();
			RefferedInInvoice = invGrn.InvoiceId > 0;
			label13.Text = "Goods Receiving Notes (" + invGrn.TransType + ")";
			if (FreightId == 0 && RefDocumentTypeId == 105)
			{
				txtAdvanceByFactory.Enabled = false;
				txtAdvanceByFactory.Text = "0";
				txtAdvanceByParty.Enabled = false;
				txtAdvanceByParty.Text = "0";
			}
			else if (FreightId > 0)
			{
				txtBiltyFreight.Enabled = false;
				txtAdvanceByParty.Enabled = false;
				txtAdvanceByFactory.Enabled = false;
				txtFreightDeduction.Enabled = false;
			}
			ItemdtFillFromLab();
			ItemNameBind();
			if (RefDocumentTypeId == 105)
			{
				txtcarramount.Text = invGrn.OtherCharges.ToString();
				lblRecwt.Visible = true;
				lblBalwt.Visible = true;
				txtReceivedWeight.Visible = true;
				txtBalWeight.Visible = true;
				DataTable dtWeight = InvGrn.GetReceivedWeightAndBalanceWeight(new InvGrn
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = invGrn.InwardGatePassId
				});
				if (dtWeight.Rows.Count > 0)
				{
					txtReceivedWeight.Text = Conversion.ToString(dtWeight.Rows[0]["ReceivedWeight"]);
					txtBalWeight.Text = Conversion.ToString(dtWeight.Rows[0]["BalanceWeight"]);
					combsupplier.ReadOnly = false;
				}
			}
			if (RefDocumentTypeId == 106)
			{
				combsupplier.ReadOnly = false;
				((Control)(object)txtDeliverTerm).Enabled = true;
				txtDeliverTerm.LimitToList = true;
			}
			dtdetail.Clear();
			FilldtDetailFromListCommonForReadById(invGrn, dtdetail);
			double GrossWeight = GridEX_Helper.GetColumnSum(grd, "GrossWight");
			if (RefDocumentTypeId != 105 && RefDocumentTypeId != 106)
			{
				txtReceivedWeight.Text = GrossWeight.ToString();
				txtBalWeight.Text = GrossWeight.ToString();
				((Control)(object)txtDeliverTerm).Text = invGrn.invGrnDetail[0].DeliveryTerm;
			}
			else if (RefDocumentTypeId == 105)
			{
				double RecWt = Conversion.ToDouble((object)txtReceivedWeight.Text) - GrossWeight;
				txtReceivedWeight.Text = RecWt.ToString();
				txtBalWeight.Text = Conversion.ToString((object)(Conversion.ToDouble((object)txtBalWeight.Text) + GrossWeight));
			}
			txtGrnWeight.Text = GrossWeight.ToString();
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			dtEmptyBag.Clear();
			FillEBDetailFromListCommonForReadById(invGrn, dtEmptyBag);
			if (dtEmptyBag.Rows.Count == 0)
			{
				AddRowInvEmptyBagsGrid();
			}
			grdEmptyBags.DataSource = dtEmptyBag;
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			dtPurchaseBreakup.Clear();
			List<InvGrnPurchaseBreakUp> invGrnPurchaseBreakUplist = invGrn.InvGrnPurchaseBreakUplist;
			if (invGrnPurchaseBreakUplist != null && invGrnPurchaseBreakUplist.Count > 0)
			{
				for (int i = 0; i < invGrn.InvGrnPurchaseBreakUplist.Count; i++)
				{
					dtPurchaseBreakup.Rows.Add(invGrn.InvGrnPurchaseBreakUplist[i].Id, invGrn.InvGrnPurchaseBreakUplist[i].InwardGatePassId, invGrn.InvGrnPurchaseBreakUplist[i].InwardBreakupId, invGrn.InvGrnPurchaseBreakUplist[i].Qty, invGrn.InvGrnPurchaseBreakUplist[i].UOM, invGrn.InvGrnPurchaseBreakUplist[i].GrossWeight, invGrn.InvGrnPurchaseBreakUplist[i].EbWeight, invGrn.InvGrnPurchaseBreakUplist[i].EBTotal, invGrn.InvGrnPurchaseBreakUplist[i].NetPackSize, invGrn.InvGrnPurchaseBreakUplist[i].NetWeight, invGrn.InvGrnPurchaseBreakUplist[i].SupplierShortWeight, invGrn.InvGrnPurchaseBreakUplist[i].SupplierRcvdWeight, invGrn.InvGrnPurchaseBreakUplist[i].ScaleShortage, invGrn.InvGrnPurchaseBreakUplist[i].BillWeight);
				}
			}
			if (dtPurchaseBreakup.Rows.Count == 0)
			{
				AddRowInPurchaseBreakUpGrid();
			}
			if (GPID > 0)
			{
				if (Conversion.ToInt((object)CommonServices.GetGrnCountAgainstGatepass(GPID)) > 1)
				{
					LockBreakUp = true;
				}
				else
				{
					LockBreakUp = false;
				}
			}
			grdPurchaseBrakup.DataSource = dtPurchaseBreakup;
			grdPurchaseBrakup.RetrieveStructure();
			grdPurchaseBreakUpSettings();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			if (GPID > 0)
			{
				GetPrevoiusDataOfSelectedGP();
			}
			ScaleShortageProportion();
			SupplierShortageProportion();
			SupplierShortageProportionInBreakUpGrid();
			ScaleShortageProportionInBreakUpGrid();
			UpdateBillWeightInBreakUpGrid();
			ShowShortageCheckBoxes(RefDocumentTypeId);
			FreightProportion();
			PanelBreakupMain.Enabled = RefDocumentTypeId == 105;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillEBDetailFromListCommonForReadById(InvGrn invGrn, DataTable dt, bool AddExtraColumns = false)
	{
		if (invGrn == null || invGrn.InvgrnDetailEmptyBagslist == null || invGrn.InvgrnDetailEmptyBagslist.Count <= 0)
		{
			return;
		}
		foreach (InvgrnDetailEmptyBags detail in invGrn.InvgrnDetailEmptyBagslist)
		{
			List<object> rowValues = new List<object> { detail.PurchaseOrderId, detail.TypeId, detail.ItemId, detail.BagsCondition, detail.ReceivedQty, detail.PurchaseQty, detail.Remarks };
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FilldtDetailFromListCommonForReadById(InvGrn invGrn, DataTable dt, bool AddExtraColumns = false)
	{
		if (invGrn == null || invGrn.invGrnDetail == null || invGrn.invGrnDetail.Count <= 0)
		{
			return;
		}
		foreach (InvGrnDetail detail in invGrn.invGrnDetail)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.PurchaseOrderDetailId, detail.PurchaseOrderId, detail.PurchaserOrderNo, detail.WarehouseId, detail.ItemId, detail.ItemCode, detail.Item, detail.CropYearId, detail.JobLotId,
				detail.PackingTypeId, detail.ItemUomId, detail.UOMCode, detail.UOM, detail.ItemQty, detail.GrossWeight, detail.EBWPerUnit, detail.EBWTotal, detail.EbPurAgainstWeight, detail.LabId,
				detail.LabNo, detail.QtyForWtCut, detail.WeightCutOnId, detail.WeightCutOn, detail.WeightCutUom, detail.WtCut, detail.WtCutTotal, detail.AdLsWeight, detail.ScaleShortWeight, detail.SupplierShortWeight,
				detail.NetBillWeight, detail.StockEbUnit, detail.StockEbTotal, detail.StockWeight, detail.CityId, detail.FreightAmount
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

	private void FillGdnDetailListCommonForInsertAndDelete(InvGrnDetail detail, GridEXRow r)
	{
		detail.PurchaseOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
		detail.PurchaseOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
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
		detail.UOM = Conversion.ToDouble(r.Cells["UomEquivalent"].Value);
		detail.ItemQty = Conversion.ToDouble(r.Cells["Qty"].Value);
		detail.GrossWeight = Conversion.ToDouble(r.Cells["GrossWight"].Value);
		detail.EBWPerUnit = Conversion.ToDouble(r.Cells["EbUnit"].Value);
		detail.EBWTotal = Conversion.ToDouble(r.Cells["EbTotal"].Value);
		detail.EbPurAgainstWeight = Conversion.ToDouble(r.Cells["EbPurAgainstWeight"].Value);
		detail.LabId = Conversion.ToInt(r.Cells["LabId"].Value);
		detail.LabReportRef = Conversion.ToString((object)r.Cells["LabNo"].Text);
		detail.QtyForWtCut = Conversion.ToDouble(r.Cells["QtyForWtCut"].Value);
		detail.WeightCutOnId = Conversion.ToInt(r.Cells["WtCutOnId"].Value);
		detail.WeightCutUom = Conversion.ToDouble(r.Cells["WeightCutUom"].Value);
		detail.WtCut = Conversion.ToDouble(r.Cells["WtCut"].Value);
		detail.WtCutTotal = Conversion.ToDouble(r.Cells["WtCutTotal"].Value);
		detail.AdLsWeight = Conversion.ToDouble(r.Cells["AddLesswt"].Value);
		detail.ScaleShortWeight = Conversion.ToDouble(r.Cells["ScaleShortWt"].Value);
		detail.SupplierShortWeight = Conversion.ToDouble(r.Cells["SuppShortWt"].Value);
		detail.NetBillWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
		detail.StockEbUnit = Conversion.ToDouble(r.Cells["StockEbUnit"].Value);
		detail.StockEbTotal = Conversion.ToDouble(r.Cells["StockEbTotal"].Value);
		detail.StockWeight = Conversion.ToDouble(r.Cells["StockWeight"].Value);
		detail.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
		detail.AreaCity = Conversion.ToString((object)r.Cells["CityId"].Text);
		detail.FreightAmount = Conversion.ToDouble(r.Cells["FreightAmount"].Value);
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		try
		{
			if (RecId > 0 && RefferedInInvoice)
			{
				MessageBox.Show("This Document is referred in invoice. So you can't delete this record.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 46,
						Id = RecId,
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
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void CityDefine_Click(object sender, EventArgs e)
	{
		new DefineCity(UserAccount).Show();
	}

	private void reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			label13.Text = "Goods Receiving Notes";
			GPID = 0;
			LabWeightCutOnId = 0;
			EmptyBagsWeightCutStatus = 0;
			PurchaseOrderId = 0;
			PurchaseOrderNo = 0;
			combsupplier.ReadOnly = true;
			((Control)(object)combsupplier).Text = string.Empty;
			((Control)(object)CmbTransport).Text = string.Empty;
			((Control)(object)CmbTransport).Enabled = true;
			txtcarramount.Text = string.Empty;
			txtcarramount.Enabled = true;
			((Control)(object)txtDeliverTerm).Text = string.Empty;
			((Control)(object)txtDeliverTerm).Enabled = false;
			txtDeliverTerm.LimitToList = false;
			combgatepass.Text = string.Empty;
			if (((UltraGridBase)combvehtyp).DataSource != null)
			{
				((UltraGridBase)combvehtyp).Rows[1].Activate();
			}
			txtvehno.Text = string.Empty;
			txtbltyno.Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtGrnWeight.Text = string.Empty;
			txtTicketnos.Text = string.Empty;
			txtAnaylstName.Text = string.Empty;
			txtBiltyFreight.Text = string.Empty;
			txtScaleCharges.Text = string.Empty;
			txtAdvanceByParty.Text = string.Empty;
			txtAdvanceByFactory.Text = string.Empty;
			txtOtherDeduction.Text = string.Empty;
			txtShortageAmountFr.Text = string.Empty;
			txtDiscountAmountFr.Text = string.Empty;
			txtRemainingAmountFr.Text = string.Empty;
			txtChargeToPartyAmountFr.Text = string.Empty;
			txtFreightDeduction.Text = string.Empty;
			txtsuppwt.ReadOnly = true;
			txtsuppwt.Text = string.Empty;
			txtfctwt.Text = string.Empty;
			txtReceivedWeight.Text = string.Empty;
			txtBalWeight.Text = string.Empty;
			txtDifferenceWeight.Text = string.Empty;
			lblDiffweight.Text = string.Empty;
			dtPrevoiusDataOfSelectedGP.Clear();
			updateDetailIndex = -1;
			((Control)(object)combitem).Text = string.Empty;
			((UltraGridBase)combitem).DataSource = null;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((UltraGridBase)CmbPackUom).DataSource = null;
			txtqty.Text = string.Empty;
			txtgwt.Text = string.Empty;
			txtebu.Text = string.Empty;
			txtEmptyBagsTotal.Text = string.Empty;
			txtadlswt.Text = string.Empty;
			txtnetwt.Text = string.Empty;
			txtstockwt.Text = string.Empty;
			txtwtcut.Text = string.Empty;
			((Control)(object)CmbLabNo).Text = string.Empty;
			((UltraGridBase)CmbLabNo).DataSource = null;
			PanelCurrentLabResult.Visible = false;
			grdLabInfo.DataSource = null;
			((Control)(object)CmbCity).Text = string.Empty;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			Add.Visible = true;
			lblBalwt.Visible = false;
			txtBalWeight.Visible = false;
			dtdetail.Rows.Clear();
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			dtEmptyBag.Rows.Clear();
			AddRowInvEmptyBagsGrid();
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			LockBreakUp = false;
			RefferedInInvoice = false;
			txtcarramount.Enabled = !LockBreakUp;
			dtPurchaseBreakup.Rows.Clear();
			AddRowInPurchaseBreakUpGrid();
			grdPurchaseBreakUpSettings();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			DocDate.Focus();
			UpdateDocumentNoUI(GetDocumentCode());
			GatepassGridFill(GatepassDataDbCall());
			FillCountFromGpGrid();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			txtgwt.Enabled = true;
			chkScaleShortWeightApply.Checked = false;
			chkSuppShortWeight.Checked = false;
			((Control)(object)CmbSupplierDispatchedPreBillNo).Text = string.Empty;
			((UltraGridBase)CmbSupplierDispatchedPreBillNo).DataSource = null;
			txtBiltyFreight.Enabled = FreightId == 0;
			txtAdvanceByParty.Enabled = FreightId == 0;
			txtAdvanceByFactory.Enabled = FreightId == 0;
			txtFreightDeduction.Enabled = FreightId == 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetial()
	{
		try
		{
			updateDetailIndex = -1;
			txtqty.Text = string.Empty;
			txtgwt.Text = string.Empty;
			txtebu.Text = string.Empty;
			txtEmptyBagsTotal.Text = string.Empty;
			txtadlswt.Text = string.Empty;
			txtnetwt.Text = string.Empty;
			txtstockwt.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((Control)(object)CmbLabNo).Text = string.Empty;
			txtwtcut.Text = "0";
			txtWtCutOn.Text = "";
			PanelCurrentLabResult.Visible = false;
			grdLabInfo.DataSource = null;
			combitem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
		txtEbUnitStock.Text = "0";
		txtEbTotalStock.Text = "0";
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string>
			{
				"BranchesAllocatedToUser", "AccountsWithCustomGroupId", "ItemsWithCustomGroupId", "PaymentTerm", "UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems", "MultiCurrency",
				"AllCities", "CropYear", "JobLot", "InvPackingType"
			};
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			TransporterDtFillFromGlobal();
			TransporterAcFill();
			VehicleTypesBind(VehicleType.GetAll());
			WarehousesDtFillFromGlobalAndBind();
			CropDtFillFromGlobalAndBind();
			JobLotDtFillFromGlobalAndBind();
			PackingTypeDtFillFromGlobalAndBind();
			CityDtFillFromGlobalAndBind();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			EmptyBagsTypeAndConditionDbCall();
			EmptyBagsGridComboBind(grdEmptyBags);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvFrmGRN_FormClosing(object sender, FormClosingEventArgs e)
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

	private void WtCutCalculations(Action<double> setResult, double WeightCut, double Qty, double GrossWeight, int LabWeightCutOnId, double WeightCutOnQty, double ItemQtyInGrd, double WeightCutUom, int ItemId, bool isTaxBox = false, GridEXRow rrr = null)
	{
		try
		{
			if (LabWeightCutOnId > 0 && WeightCutOnQty == 0.0)
			{
				WeightCutOnQty = ((!isTaxBox) ? (GridEX_Helper.GetColumnSum(grd, "Qty") + Qty) : (GridEX_Helper.GetColumnSumWithoutIndex(grd, "Qty", updateDetailIndex) + Qty));
			}
			if (grd.GetRows().Count() > 0)
			{
				if (isTaxBox)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow row in rows)
					{
						if (ItemId == Conversion.ToInt(row.Cells["ItemId"].Value) && row.RowIndex != updateDetailIndex)
						{
							double WtCutVal = Conversion.ToDouble(row.Cells["WtCut"].Value);
							if (WtCutVal != 0.0)
							{
								double QtyUtilizeForWtCut = Conversion.ToDouble(row.Cells["WtCutTotal"].Value) / WtCutVal;
								ItemQtyInGrd += QtyUtilizeForWtCut;
							}
						}
					}
				}
				else
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow row2 in rows)
					{
						if (ItemId == Conversion.ToInt(row2.Cells["ItemId"].Value) && row2.RowIndex != rrr.RowIndex)
						{
							double WtCutVal2 = Conversion.ToDouble(row2.Cells["WtCut"].Value);
							if (WtCutVal2 != 0.0)
							{
								double QtyUtilizeForWtCut2 = Conversion.ToDouble(row2.Cells["WtCutTotal"].Value) / WtCutVal2;
								ItemQtyInGrd += QtyUtilizeForWtCut2;
							}
						}
					}
				}
			}
			double result = 0.0;
			switch (LabWeightCutOnId)
			{
			case 1:
			{
				double RemainingQty = WeightCutOnQty - ItemQtyInGrd;
				if (RemainingQty > 0.0 && Qty <= RemainingQty)
				{
					result = Qty * WeightCut;
				}
				else if (RemainingQty > 0.0 && Qty >= RemainingQty)
				{
					result = RemainingQty * WeightCut;
				}
				break;
			}
			case 2:
			{
				double qty = GrossWeight / WeightCutUom;
				double RemainingQty2 = WeightCutOnQty - ItemQtyInGrd;
				if ((RemainingQty2 == 0.0 && qty <= WeightCutOnQty) || (RemainingQty2 > 0.0 && qty <= RemainingQty2))
				{
					result = qty * WeightCut;
				}
				else if (RemainingQty2 > 0.0 && RemainingQty2 <= qty)
				{
					result = RemainingQty2 * WeightCut;
				}
				break;
			}
			default:
				result = Qty * WeightCut;
				break;
			}
			setResult(result);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EbCalculations()
	{
		try
		{
			double EBUnit = 0.0;
			double EBTotal = 0.0;
			double Qty = 0.0;
			EBTotal = Conversion.ToDouble((object)txtEmptyBagsTotal.Text);
			if (txtebu.Text != string.Empty && txtebu.Text != null)
			{
				EBUnit = Conversion.ToDouble((object)txtebu.Text.Trim().ToString());
			}
			if (txtqty.Text != string.Empty && txtqty.Text != null)
			{
				Qty = Conversion.ToDouble((object)txtqty.Text.Trim().ToString());
			}
			if (EBUnit > 0.0 || EBTotal > 0.0)
			{
				if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "EbUnit")
				{
					EBTotal = EBUnit * Qty;
					txtEmptyBagsTotal.Text = Math.Round(EBTotal, 2).ToString();
				}
				else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "EbTotal")
				{
					EBUnit = ((Qty > 0.0) ? (EBTotal / Qty) : 0.0);
					txtebu.Text = Math.Round(EBUnit, 3).ToString();
				}
				else
				{
					EBTotal = ((EBUnit > 0.0 && Qty > 0.0) ? (EBUnit * Qty) : 0.0);
					txtEmptyBagsTotal.Text = Math.Round(EBTotal, 2).ToString();
				}
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
			double qty = 0.0;
			double grossWeight = 0.0;
			double ebTotal = 0.0;
			double weightCut = 0.0;
			double weightCutTotal = 0.0;
			double addLess = 0.0;
			double factoryWeight = 0.0;
			double supplierWeight = 0.0;
			double diffWeight = 0.0;
			double breakupEBTotal = 0.0;
			factoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			if (RefDocumentTypeId == 105 && grdPurchaseBrakup.GetRows().Count() > 0)
			{
				double columnSum = GridEX_Helper.GetColumnSum(grdPurchaseBrakup, "GrossWeight");
				breakupEBTotal = GridEX_Helper.GetColumnSum(grdPurchaseBrakup, "EBTotal");
				supplierWeight = columnSum - Math.Abs(breakupEBTotal);
			}
			else
			{
				supplierWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
			}
			if (((UltraGridBase)CmbPackUom).ActiveRow != null && Conversion.ToInt(CmbPackUom.Value) > 0)
			{
				Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
			}
			qty = Conversion.ToDouble((object)txtqty.Text.Trim());
			grossWeight = Conversion.ToDouble((object)txtgwt.Text.Trim());
			textBox1.Text = ((qty > 0.0) ? (grossWeight / qty) : 0.0).ToString("#,##.##");
			Conversion.ToDouble((object)txtebu.Text.Trim());
			ebTotal = Conversion.ToDouble((object)txtEmptyBagsTotal.Text);
			diffWeight = supplierWeight - (factoryWeight + BillWeightAndStockWeightDifferenceTolerance);
			if (!string.IsNullOrWhiteSpace(txtwtcut.Text))
			{
				weightCut = Conversion.ToDouble((object)txtwtcut.Text.Trim());
				int labWeightCutOnId = 0;
				double weightCutOnQty = 0.0;
				double weightCutUom = 0.0;
				if (((UltraDropDownBase)CmbLabNo).SelectedRow != null && Conversion.ToInt(CmbLabNo.Value) > 0)
				{
					labWeightCutOnId = Conversion.ToInt(((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["WtCutOnId"].Value);
					weightCutOnQty = Conversion.ToDouble(((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["QtyForWtCut"].Value);
					weightCutUom = Conversion.ToDouble(((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["WeightCutUom"].Value);
				}
				WtCutCalculations(delegate(double result)
				{
					txtwtcuttotal.Text = result.ToString();
				}, weightCut, qty, grossWeight, labWeightCutOnId, weightCutOnQty, 0.0, weightCutUom, Conversion.ToInt(combitem.Value), isTaxBox: true);
			}
			else
			{
				txtwtcuttotal.Text = "0";
			}
			weightCutTotal = Conversion.ToDouble((object)txtwtcuttotal.Text.Trim());
			addLess = (string.IsNullOrWhiteSpace(txtadlswt.Text) ? 0.0 : Conversion.ToDouble((object)txtadlswt.Text.Trim()));
			double calculatedNetWeight = ((RefDocumentTypeId == 105) ? (grossWeight - Math.Abs(addLess)) : (grossWeight - ebTotal - weightCutTotal - addLess));
			txtnetwt.Text = Math.Round(calculatedNetWeight, 2).ToString();
			if (!(supplierWeight > 0.0) || !(factoryWeight > 0.0) || !(grossWeight > 0.0))
			{
				return;
			}
			if (DeductionPolicyForGrnIsOn)
			{
				txtstockwt.Text = factoryWeight.ToString();
				return;
			}
			Conversion.ToDouble((object)txtEbUnitStock.Text);
			ebTotal = Conversion.ToDouble((object)txtEbTotalStock.Text);
			bool useSupplierWeight = true;
			string term = ((Control)(object)txtDeliverTerm).Text.Trim();
			switch (term)
			{
			case "Ponch & FactoryWeight":
			case "Load & FactoryWeight":
				useSupplierWeight = false;
				break;
			case "Ponch":
				useSupplierWeight = supplierWeight <= factoryWeight;
				break;
			}
			if ((term == "Load" || term == "Load & PartyWeight" || term == "Ponch & PartyWeight") && diffWeight > 0.0 && RefDocumentTypeId != 105 && !ExcludeWeightShortageBusinessOnGrn)
			{
				useSupplierWeight = Conversion.ToDouble((object)txtFreightDeduction.Text) + Conversion.ToDouble((object)txtChargeToPartyAmountFr.Text) > 0.0;
			}
			double baseWeight = (useSupplierWeight ? supplierWeight : factoryWeight);
			double stockWeight = ((baseWeight > 0.0) ? (factoryWeight * grossWeight / baseWeight) : 0.0);
			stockWeight -= ebTotal;
			txtstockwt.Text = Math.Round(stockWeight, 2).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalWeightCalculationForMarketPurchase()
	{
		try
		{
			if (RefDocumentTypeId == 105)
			{
				double PackUOM = 0.0;
				double Qty = 0.0;
				double GrossWeight = 0.0;
				if (((UltraGridBase)CmbPackUom).ActiveRow != null && Conversion.ToInt(CmbPackUom.Value) > 0)
				{
					PackUOM = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				}
				if (txtqty.Text != string.Empty)
				{
					Qty = Conversion.ToDouble((object)txtqty.Text.ToString());
				}
				if (txtgwt.Text != string.Empty)
				{
					GrossWeight = Conversion.ToDouble((object)txtgwt.Text.ToString());
				}
				txtgwt.Text = ((PackUOM > 0.0) ? (PackUOM * Qty) : 0.0).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockEbCalculations()
	{
		try
		{
			double EBUnit = 0.0;
			double EBTotal = 0.0;
			double Qty = 0.0;
			EBTotal = Conversion.ToDouble((object)txtEbTotalStock.Text);
			if (txtEbUnitStock.Text != string.Empty && txtEbUnitStock.Text != null)
			{
				EBUnit = Conversion.ToDouble((object)txtEbUnitStock.Text.Trim().ToString());
			}
			if (txtqty.Text != string.Empty && txtqty.Text != null)
			{
				Qty = Conversion.ToDouble((object)txtqty.Text.Trim().ToString());
			}
			if (EBUnit > 0.0 || EBTotal > 0.0)
			{
				if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "EbStockUnit")
				{
					EBTotal = EBUnit * Qty;
					txtEbTotalStock.Text = Math.Round(EBTotal, 2).ToString();
				}
				else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "EbStockTotal")
				{
					EBUnit = ((Qty > 0.0) ? (EBTotal / Qty) : 0.0);
					txtEbUnitStock.Text = Math.Round(EBUnit, 3).ToString();
				}
				else
				{
					EBTotal = ((EBUnit > 0.0 && Qty > 0.0) ? (EBUnit * Qty) : 0.0);
					txtEbTotalStock.Text = Math.Round(EBTotal, 2).ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MarkeetPurchaseFormula()
	{
		try
		{
			if ((Conversion.ToDouble((object)txtqty.Text.Trim()) == 0.0 || txtqty.Text.Trim() == string.Empty) && txtGrnWeight.Text.Trim() != string.Empty && txtGrnWeight.Text.Trim() != "0" && ((Control)(object)CmbPackUom).Text.Trim() != string.Empty)
			{
				double GrossWeight = Conversion.ToDouble((object)txtGrnWeight.Text.Trim());
				double PackUom = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				double Qty = ((PackUom > 0.0) ? (GrossWeight / PackUom) : 0.0);
				txtqty.Text = Math.Round(Qty).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculationPurchaseAgainstWeightOld()
	{
		try
		{
			grdEmptyBags.UpdateData();
			double Qty = 0.0;
			double GrossWeight = 0.0;
			double EBUnit = 0.0;
			double EBTotal = 0.0;
			double WeightCut = 0.0;
			double WeightCutTotal = 0.0;
			double AddLess = 0.0;
			double FactoryWeight = 0.0;
			double SupplierWeight = 0.0;
			double DiffWeight = 0.0;
			double ScaleShortWt = 0.0;
			double SuppShortWt = 0.0;
			double getBreakupGrossWeightTotal = 0.0;
			double getBreakupEBTotal = 0.0;
			if (txtfctwt.Text != string.Empty && txtfctwt.Text != null)
			{
				FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim().ToString());
			}
			if (RefDocumentTypeId == 105)
			{
				if (grdPurchaseBrakup.GetRows().Count() > 0)
				{
					getBreakupGrossWeightTotal = Conversion.ToDouble(grdPurchaseBrakup.GetTotal(grdPurchaseBrakup.RootTable.Columns["GrossWeight"], (AggregateFunction)2));
					getBreakupEBTotal = Conversion.ToDouble(grdPurchaseBrakup.GetTotal(grdPurchaseBrakup.RootTable.Columns["EBTotal"], (AggregateFunction)2));
					SupplierWeight = getBreakupGrossWeightTotal - Math.Abs(getBreakupEBTotal);
					if (Conversion.ToDouble((object)txtsuppwt.Text) > getBreakupGrossWeightTotal)
					{
						Conversion.ToDouble((object)txtsuppwt.Text);
					}
				}
			}
			else if (txtsuppwt.Text != string.Empty && txtsuppwt.Text != null)
			{
				SupplierWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim().ToString());
			}
			DiffWeight = SupplierWeight - (FactoryWeight + BillWeightAndStockWeightDifferenceTolerance);
			double AllocateQty = 0.0;
			double PurQty = 0.0;
			double grdQty = 0.0;
			double EBQtyLessFromBillWeight = 0.0;
			for (int i = 0; i < dtEmptyBag.Rows?.Count; i++)
			{
				if (Conversion.ToInt(dtEmptyBag.Rows[i]["Type"]) == 2)
				{
					PurQty += Conversion.ToDouble(dtEmptyBag.Rows[i]["PurQty"]);
				}
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				double TotalPurchaseAgainstWeightAddinBillWeight = 0.0;
				item.BeginEdit();
				Conversion.ToDouble(item.Cells["UOM"].Value);
				Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				GrossWeight = Conversion.ToDouble(item.Cells["GrossWight"].Value);
				EBUnit = Conversion.ToDouble(item.Cells["EbUnit"].Value);
				WeightCut = Conversion.ToDouble(item.Cells["WtCut"].Value);
				AddLess = Conversion.ToDouble(item.Cells["AddLesswt"].Value);
				ScaleShortWt = Conversion.ToDouble(item.Cells["ScaleShortWt"].Value);
				SuppShortWt = Conversion.ToDouble(item.Cells["SuppShortWt"].Value);
				if (EBUnit > 0.0)
				{
					grdQty = Conversion.ToDouble(item.Cells["Qty"].Value);
					if (PurQty > 0.0)
					{
						PurQty -= AllocateQty;
						if (PurQty > grdQty)
						{
							EBQtyLessFromBillWeight = grdQty;
						}
						else if (grdQty > PurQty)
						{
							EBQtyLessFromBillWeight = PurQty;
						}
						if (PurQty == grdQty)
						{
							EBQtyLessFromBillWeight = grdQty;
						}
						if (PurQty > 0.0)
						{
							TotalPurchaseAgainstWeightAddinBillWeight = EBQtyLessFromBillWeight * EBUnit;
							item.Cells["EbPurAgainstWeight"].Value = TotalPurchaseAgainstWeightAddinBillWeight.ToString();
						}
						else
						{
							item.Cells["EbPurAgainstWeight"].Value = 0.ToString();
						}
					}
					else
					{
						item.Cells["EbPurAgainstWeight"].Value = 0.ToString();
					}
				}
				EBTotal = Conversion.ToDouble(item.Cells["EbTotal"].Value);
				if (WeightCut > 0.0 && Qty > 0.0)
				{
					int LabWeightCutOnId = 0;
					double WeightCutOnQty = 0.0;
					double ItemQtyInGrd = 0.0;
					double WeightCutUom = 0.0;
					LabWeightCutOnId = Conversion.ToInt(item.Cells["WtCutOnId"].Value);
					WeightCutOnQty = Conversion.ToDouble(item.Cells["QtyForWtCut"].Value);
					WeightCutUom = Conversion.ToDouble(item.Cells["WeightCutUom"].Value);
					WtCutCalculations(delegate(double result)
					{
						item.Cells["WtCutTotal"].Value = result.ToString();
					}, WeightCut, Qty, GrossWeight, LabWeightCutOnId, WeightCutOnQty, ItemQtyInGrd, WeightCutUom, Conversion.ToInt(item.Cells["ItemId"].Value), isTaxBox: false, item);
					WeightCutTotal = Conversion.ToDouble(item.Cells["WtCutTotal"].Value);
				}
				else
				{
					item.Cells["WtCutTotal"].Value = 0;
					WeightCutTotal = 0.0;
				}
				double Weight = 0.0;
				if (RefDocumentTypeId == 105)
				{
					if (!chkScaleShortWeightApply.Checked)
					{
						ScaleShortWt = 0.0;
					}
					if (!chkSuppShortWeight.Checked)
					{
						SuppShortWt = 0.0;
					}
					Weight = GrossWeight - Math.Abs(AddLess) - Math.Abs(ScaleShortWt) - Math.Abs(SuppShortWt);
				}
				else if (RefDocumentTypeId == 41)
				{
					if (!chkScaleShortWeightApply.Checked)
					{
						ScaleShortWt = 0.0;
					}
					Weight = GrossWeight - WeightCutTotal - EBTotal - AddLess + TotalPurchaseAgainstWeightAddinBillWeight - Math.Abs(ScaleShortWt);
				}
				else
				{
					Weight = GrossWeight - WeightCutTotal - EBTotal - AddLess + TotalPurchaseAgainstWeightAddinBillWeight;
				}
				item.Cells["NetWeight"].Value = Math.Round(Weight, 2).ToString();
				if (SupplierWeight > 0.0 && FactoryWeight > 0.0 && GrossWeight > 0.0)
				{
					if (DeductionPolicyForGrnIsOn)
					{
						item.Cells["StockWeight"].Value = FactoryWeight.ToString();
					}
					else
					{
						EBTotal = Conversion.ToDouble(item.Cells["StockEbTotal"].Value);
						if (FreightId > 0)
						{
							if (DiffWeight >= 0.0)
							{
								double StockWt = FactoryWeight * GrossWeight / SupplierWeight;
								StockWt -= EBTotal;
								item.Cells["StockWeight"].Value = Math.Round(StockWt, 2).ToString();
							}
							else if (((Control)(object)txtDeliverTerm).Text == "Load & FactoryWeight" || ((Control)(object)txtDeliverTerm).Text == "Ponch & FactoryWeight")
							{
								double StockWt2 = FactoryWeight * GrossWeight / FactoryWeight;
								StockWt2 -= EBTotal;
								item.Cells["StockWeight"].Value = Math.Round(StockWt2, 2).ToString();
							}
							else if (((Control)(object)txtDeliverTerm).Text == "Ponch")
							{
								if (SupplierWeight > FactoryWeight)
								{
									double StockWt3 = FactoryWeight * GrossWeight / FactoryWeight;
									StockWt3 -= EBTotal;
									item.Cells["StockWeight"].Value = Math.Round(StockWt3, 2).ToString();
								}
								else
								{
									double StockWt4 = FactoryWeight * GrossWeight / SupplierWeight;
									StockWt4 -= EBTotal;
									item.Cells["StockWeight"].Value = Math.Round(StockWt4, 2).ToString();
								}
							}
							else
							{
								double StockWt5 = FactoryWeight * GrossWeight / SupplierWeight;
								StockWt5 -= EBTotal;
								item.Cells["StockWeight"].Value = Math.Round(StockWt5, 2).ToString();
							}
						}
						else if (DiffWeight >= 0.0)
						{
							double StockWt6 = FactoryWeight * GrossWeight / SupplierWeight;
							StockWt6 -= EBTotal;
							item.Cells["StockWeight"].Value = Math.Round(StockWt6, 2).ToString();
						}
						else if (FactoryWeight > SupplierWeight)
						{
							double StockWt7 = FactoryWeight * GrossWeight / SupplierWeight;
							StockWt7 -= EBTotal;
							item.Cells["StockWeight"].Value = Math.Round(StockWt7, 2).ToString();
						}
						else
						{
							double StockWt8 = FactoryWeight * GrossWeight / FactoryWeight;
							StockWt8 -= EBTotal;
							item.Cells["StockWeight"].Value = Math.Round(StockWt8, 2).ToString();
						}
					}
				}
				else
				{
					item.Cells["StockWeight"].Value = Math.Round(Weight, 2).ToString();
				}
				AllocateQty = EBQtyLessFromBillWeight;
				item.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockWeightCalculationInGrid()
	{
		try
		{
			double factoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			double freightDeduction = Conversion.ToDouble((object)txtFreightDeduction.Text) + Conversion.ToDouble((object)txtChargeToPartyAmountFr.Text);
			string deliveryTerm = ((Control)(object)txtDeliverTerm).Text.Trim();
			double supplierWeight;
			if (RefDocumentTypeId == 105 && grdPurchaseBrakup.GetRows().Any())
			{
				double columnSum = GridEX_Helper.GetColumnSum(grdPurchaseBrakup, "GrossWeight");
				double breakupEB = GridEX_Helper.GetColumnSum(grdPurchaseBrakup, "EBTotal");
				supplierWeight = columnSum - Math.Abs(breakupEB);
			}
			else
			{
				supplierWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
			}
			double diffWeight = supplierWeight - (factoryWeight + BillWeightAndStockWeightDifferenceTolerance);
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				obj.BeginEdit();
				double grossWeight = Conversion.ToDouble(obj.Cells["GrossWight"].Value);
				double ebTotal = Conversion.ToDouble(obj.Cells["StockEbTotal"].Value);
				double stockWeight = 0.0;
				if (supplierWeight > 0.0 && factoryWeight > 0.0 && grossWeight > 0.0)
				{
					if (DeductionPolicyForGrnIsOn)
					{
						stockWeight = factoryWeight;
					}
					else
					{
						bool useSupplierWeight = true;
						_ = FreightId;
						switch (deliveryTerm)
						{
						case "Ponch & FactoryWeight":
						case "Load & FactoryWeight":
							useSupplierWeight = false;
							break;
						case "Ponch":
							useSupplierWeight = supplierWeight <= factoryWeight;
							break;
						default:
							useSupplierWeight = true;
							break;
						}
						if ((deliveryTerm == "Load" || deliveryTerm == "Load & PartyWeight" || deliveryTerm == "Ponch & PartyWeight") && diffWeight > 0.0 && RefDocumentTypeId != 105 && !ExcludeWeightShortageBusinessOnGrn)
						{
							useSupplierWeight = freightDeduction > 0.0;
						}
						double baseWeight = (useSupplierWeight ? supplierWeight : factoryWeight);
						stockWeight = ((baseWeight > 0.0) ? (factoryWeight * grossWeight / baseWeight) : 0.0);
						stockWeight -= ebTotal;
					}
				}
				obj.Cells["StockWeight"].Value = stockWeight;
				obj.EndEdit();
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception("Error in StockWeightCalculationInGrid: " + ex.Message);
		}
	}

	private void TotalEbPurchaseAgainstWeightUtilizeInGrid()
	{
		try
		{
			double TotalEbPurchaseQty = GridEX_Helper.GetColumnSumWithWhere(grdEmptyBags, "PurQty", "Type", "2", CheckValue: true);
			double totalGridQty = GridEX_Helper.GetColumnSum(grd, "Qty");
			if (TotalEbPurchaseQty <= 0.0 || totalGridQty <= 0.0)
			{
				return;
			}
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				GridEXCell obj2 = obj.Cells["GrossWight"];
				double grossWeight = Conversion.ToDouble((obj2 != null) ? obj2.Value : null);
				GridEXCell obj3 = obj.Cells["Qty"];
				double Qty = Conversion.ToDouble((obj3 != null) ? obj3.Value : null);
				GridEXCell obj4 = obj.Cells["EbUnit"];
				double EbUnit = Conversion.ToDouble((obj4 != null) ? obj4.Value : null);
				GridEXCell obj5 = obj.Cells["EbTotal"];
				double ebTotal = Conversion.ToDouble((obj5 != null) ? obj5.Value : null);
				GridEXCell obj6 = obj.Cells["WtCutTotal"];
				double weightCutTotal = Conversion.ToDouble((obj6 != null) ? obj6.Value : null);
				GridEXCell obj7 = obj.Cells["NetWeight"];
				Conversion.ToDouble((obj7 != null) ? obj7.Value : null);
				GridEXCell obj8 = obj.Cells["AddLessWt"];
				double addLess = Conversion.ToDouble((obj8 != null) ? obj8.Value : null);
				double ApplyEbPurQty = Math.Min(TotalEbPurchaseQty, Qty);
				double applyEbPurAgainstWeight = ApplyEbPurQty * EbUnit;
				obj.BeginEdit();
				obj.Cells["EbPurAgainstWeight"].Value = applyEbPurAgainstWeight;
				obj.Cells["NetWeight"].Value = grossWeight - ebTotal - weightCutTotal - addLess + applyEbPurAgainstWeight;
				obj.EndEdit();
				TotalEbPurchaseQty -= ApplyEbPurQty;
				if (TotalEbPurchaseQty <= 0.0001)
				{
					break;
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception("Error in TotalEbPurchaseAgainstWeightUtilizeInGrid: " + ex.Message);
		}
	}

	private void combpcktyp_Leave(object sender, EventArgs e)
	{
		try
		{
			PurchaseOrderEmptyBagsDetailByOrderId();
			int packingType = Conversion.ToInt(CmbPackingType.Value);
			if (RefDocumentTypeId != 41)
			{
				switch (packingType)
				{
				case 1:
					txtebu.Text = ((Conversion.ToDouble((object)txtebu.Text) == 0.0) ? Conversion.ToString((object)JuteBagsCut) : txtebu.Text);
					break;
				case 2:
					txtebu.Text = ((Conversion.ToDouble((object)txtebu.Text) == 0.0) ? Conversion.ToString((object)PPBagsCut) : txtebu.Text);
					break;
				case 5:
					txtebu.Text = ((Conversion.ToDouble((object)txtebu.Text) == 0.0) ? Conversion.ToString((object)OpenBulkCut) : txtebu.Text);
					break;
				}
			}
			switch (packingType)
			{
			case 1:
				txtEbUnitStock.Text = ((Conversion.ToDouble((object)txtEbUnitStock.Text) == 0.0) ? Conversion.ToString((object)JuteBagsCutStock) : txtEbUnitStock.Text);
				break;
			case 2:
				txtEbUnitStock.Text = ((Conversion.ToDouble((object)txtEbUnitStock.Text) == 0.0) ? Conversion.ToString((object)PPBagsCutStock) : txtEbUnitStock.Text);
				break;
			case 5:
				txtEbUnitStock.Text = ((Conversion.ToDouble((object)txtEbUnitStock.Text) == 0.0) ? Conversion.ToString((object)OpenBulkCutStock) : txtEbUnitStock.Text);
				break;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowWarningOrNotWhenOpenBulk()
	{
	}

	private void combpckuom_Leave(object sender, EventArgs e)
	{
		Total();
		if (RefDocumentTypeId == 105)
		{
			TotalWeightCalculationForMarketPurchase();
			MarkeetPurchaseFormula();
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		TotalWeightCalculationForMarketPurchase();
		EbCalculations();
		StockEbCalculations();
		Total();
	}

	private void txtqty_Leave(object sender, EventArgs e)
	{
		if (RefDocumentTypeId == 105)
		{
			MarkeetPurchaseFormula();
		}
	}

	private void txtgwt_TextChanged(object sender, EventArgs e)
	{
		if (RefDocumentTypeId == 105)
		{
			EbCalculations();
		}
		Total();
	}

	private void txtgwt_Leave(object sender, EventArgs e)
	{
		if (RefDocumentTypeId == 105)
		{
			MarkeetPurchaseFormula();
		}
		double grossWeight = Conversion.ToDouble((object)txtgwt.Text.Trim());
		if (Conversion.ToInt(CmbPackingType.Value) == 5)
		{
			double packUOM = 0.0;
			if (((UltraGridBase)CmbPackUom).ActiveRow != null && Conversion.ToInt(CmbPackUom.Value) > 0)
			{
				packUOM = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
			}
			double qty = ((packUOM > 0.0) ? (grossWeight / packUOM) : 0.0);
			txtqty.Text = Math.Round(qty).ToString();
		}
		if (RefDocumentTypeId == 105)
		{
			EbCalculations();
		}
		Total();
	}

	private void txtebu_TextChanged(object sender, EventArgs e)
	{
		if (RefDocumentTypeId == 105)
		{
			EbCalculations();
		}
		Total();
	}

	private void txtEmptyBagsTotal_TextChanged(object sender, EventArgs e)
	{
		try
		{
			EbCalculations();
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtadlswt_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtadlswt_Leave(object sender, EventArgs e)
	{
		Total();
	}

	private void txtEbWtOther_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtebu_TextChanged_1(object sender, EventArgs e)
	{
		try
		{
			EbCalculations();
			Total();
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
			StockEbCalculations();
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
			StockEbCalculations();
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtadlswt_TextChanged_1(object sender, EventArgs e)
	{
		Total();
	}

	private void TotalSupplierWeight()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
			double FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			double DifferenceWeight = num - FactoryWeight;
			txtDifferenceWeight.Text = DifferenceWeight.ToString();
			if (DifferenceWeight > 0.0)
			{
				lblDiffweight.Text = "Short";
			}
			else if (DifferenceWeight < 0.0)
			{
				lblDiffweight.Text = "Excess";
			}
			else if (DifferenceWeight == 0.0)
			{
				lblDiffweight.Text = "Equal";
			}
			double GridGrossWeight = GridEX_Helper.GetColumnSum(grd, "GrossWeight");
			double GridBillWeight = GridEX_Helper.GetColumnSum(grd, "NetWeight");
			double GridStockWeight = GridEX_Helper.GetColumnSum(grd, "StockWeight");
			WeightBusinessValidations(GridGrossWeight, GridBillWeight, GridStockWeight, IsCalculating: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtsuppwt_TextChanged(object sender, EventArgs e)
	{
		TotalSupplierWeight();
		ScaleShortageProportion();
		SupplierShortageProportion();
		SupplierShortageProportionInBreakUpGrid();
	}

	private void txtfctwt_TextChanged(object sender, EventArgs e)
	{
		TotalSupplierWeight();
		ScaleShortageProportion();
		SupplierShortageProportion();
		SupplierShortageProportionInBreakUpGrid();
	}

	private void SupplierShortageProportionInBreakUpGrid()
	{
		try
		{
			double PurchaseBrakupGrossWeight = 0.0;
			if (grdPurchaseBrakup.RowCount > 0)
			{
				PurchaseBrakupGrossWeight = Conversion.ToDouble((object)grdPurchaseBrakup.GetTotal(grdPurchaseBrakup.RootTable.Columns["GrossWeight"], (AggregateFunction)2).ToString());
			}
			double SupplierWeight = Conversion.ToDouble((object)txtsuppwt.Text);
			double TotalShortage = PurchaseBrakupGrossWeight - SupplierWeight;
			if (TotalShortage > 0.0)
			{
				double ItemQty = Conversion.ToDouble((object)grdPurchaseBrakup.GetTotal(grdPurchaseBrakup.RootTable.Columns["Qty"], (AggregateFunction)2).ToString());
				if (TotalShortage > 0.0)
				{
					GridEXRow[] rows = grdPurchaseBrakup.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						double Qty = Conversion.ToDouble((object)obj.Cells["Qty"].Value.ToString());
						double Exp = ((ItemQty > 0.0) ? (TotalShortage / ItemQty * Qty) : 0.0);
						obj.Cells["SuppShortWt"].Value = Exp;
					}
				}
			}
			else
			{
				GridEXRow[] rows = grdPurchaseBrakup.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["SuppShortWt"].Value = 0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ScaleShortageProportionInBreakUpGrid()
	{
		try
		{
			double TotalShortage = Conversion.ToDouble((object)txtDifferenceWeight.Text);
			if (TotalShortage > 0.0)
			{
				double ItemQty = Conversion.ToDouble((object)grdPurchaseBrakup.GetTotal(grdPurchaseBrakup.RootTable.Columns["Qty"], (AggregateFunction)2).ToString());
				if (TotalShortage > 0.0)
				{
					GridEXRow[] rows = grdPurchaseBrakup.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						double Qty = Conversion.ToDouble((object)obj.Cells["Qty"].Value.ToString());
						double Exp = ((ItemQty > 0.0) ? (TotalShortage / ItemQty * Qty) : 0.0);
						obj.Cells["ScaleShortage"].Value = Exp;
					}
				}
			}
			else
			{
				GridEXRow[] rows = grdPurchaseBrakup.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["ScaleShortage"].Value = 0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateBillWeightInBreakUpGrid()
	{
		try
		{
			GridEXRow[] rows;
			if (grdPurchaseBrakup.RowCount > 0 && RefDocumentTypeId == 105 && !LockBreakUp)
			{
				rows = grdPurchaseBrakup.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					double GrossWeight = Conversion.ToDouble((object)r.Cells["GrossWeight"].Value.ToString());
					double EbWeight = Conversion.ToDouble((object)r.Cells["EbWeight"].Value.ToString());
					double EBTotal = Conversion.ToDouble((object)r.Cells["EBTotal"].Value.ToString());
					double SuppShortWt = Conversion.ToDouble((object)r.Cells["SuppShortWt"].Value.ToString());
					double ScaleShortage = Conversion.ToDouble((object)r.Cells["ScaleShortage"].Value.ToString());
					double NetPackSize = Conversion.ToDouble((object)r.Cells["Uom"].Value.ToString()) - Math.Abs(EbWeight);
					r.Cells["NetPackSize"].Value = NetPackSize;
					double NetWeight = GrossWeight - Math.Abs(EBTotal);
					double SupplierRcvdWeight = 0.0;
					double BillWeight = 0.0;
					if (chkSuppShortWeight.Checked)
					{
						SupplierRcvdWeight = NetWeight - SuppShortWt;
						r.Cells["SupplierRcvdWeight"].Value = SupplierRcvdWeight;
					}
					else
					{
						SupplierRcvdWeight = NetWeight;
						r.Cells["SupplierRcvdWeight"].Value = SupplierRcvdWeight;
					}
					if (chkScaleShortWeightApply.Checked)
					{
						BillWeight = SupplierRcvdWeight - ScaleShortage;
						r.Cells["BillWeight"].Value = BillWeight;
					}
					else
					{
						BillWeight = SupplierRcvdWeight;
						r.Cells["BillWeight"].Value = BillWeight;
					}
					r.EndEdit();
				}
				return;
			}
			rows = grdPurchaseBrakup.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				r2.BeginEdit();
				double GrossWeight2 = Conversion.ToDouble((object)r2.Cells["GrossWeight"].Value.ToString());
				double EbWeight2 = Conversion.ToDouble((object)r2.Cells["EbWeight"].Value.ToString());
				double EBTotal2 = Conversion.ToDouble((object)r2.Cells["EBTotal"].Value.ToString());
				double SuppShortWt2 = Conversion.ToDouble((object)r2.Cells["SuppShortWt"].Value.ToString());
				double ScaleShortage2 = Conversion.ToDouble((object)r2.Cells["ScaleShortage"].Value.ToString());
				double Uom = Conversion.ToDouble((object)r2.Cells["Uom"].Value.ToString());
				if (Conversion.ToDouble(r2.Cells["NetPackSize"].Value) == 0.0)
				{
					double NetPackSize2 = Uom - Math.Abs(EbWeight2);
					r2.Cells["NetPackSize"].Value = NetPackSize2;
				}
				double NetWeight2 = GrossWeight2 - Math.Abs(EBTotal2);
				double SupplierRcvdWeight2 = 0.0;
				double BillWeight2 = 0.0;
				if (chkSuppShortWeight.Checked)
				{
					SupplierRcvdWeight2 = NetWeight2 - SuppShortWt2;
					r2.Cells["SupplierRcvdWeight"].Value = SupplierRcvdWeight2;
				}
				else
				{
					SupplierRcvdWeight2 = NetWeight2;
					r2.Cells["SupplierRcvdWeight"].Value = SupplierRcvdWeight2;
				}
				if (chkScaleShortWeightApply.Checked)
				{
					BillWeight2 = SupplierRcvdWeight2 - ScaleShortage2;
					r2.Cells["BillWeight"].Value = BillWeight2;
				}
				else
				{
					BillWeight2 = SupplierRcvdWeight2;
					r2.Cells["BillWeight"].Value = BillWeight2;
				}
				r2.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierShortageProportion()
	{
		try
		{
			double PurchaseBrakupGrossWeight = 0.0;
			if (grdPurchaseBrakup.RowCount > 0)
			{
				PurchaseBrakupGrossWeight = Conversion.ToDouble((object)grdPurchaseBrakup.GetTotal(grdPurchaseBrakup.RootTable.Columns["GrossWeight"], (AggregateFunction)2).ToString());
			}
			double SupplierWeight = Conversion.ToDouble((object)txtsuppwt.Text);
			double TotalShortage = PurchaseBrakupGrossWeight - SupplierWeight;
			if (TotalShortage > 0.0)
			{
				double ItemQty = Conversion.ToDouble((object)grdPurchaseBrakup.GetTotal(grdPurchaseBrakup.RootTable.Columns["Qty"], (AggregateFunction)2).ToString());
				if (TotalShortage > 0.0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						double Qty = Conversion.ToDouble((object)obj.Cells["Qty"].Value.ToString());
						double Exp = ((ItemQty > 0.0) ? (TotalShortage / ItemQty * Qty) : 0.0);
						obj.Cells["SuppShortWt"].Value = Exp;
					}
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["SuppShortWt"].Value = 0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ScaleShortageProportion()
	{
		try
		{
			double TotalShortage = Conversion.ToDouble((object)txtDifferenceWeight.Text);
			double ItemQty = ((grdPurchaseBrakup.RowCount > 0) ? Conversion.ToDouble((object)grdPurchaseBrakup.GetTotal(grdPurchaseBrakup.RootTable.Columns["Qty"], (AggregateFunction)2).ToString()) : 0.0);
			if (TotalShortage > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Qty = Conversion.ToDouble((object)obj.Cells["Qty"].Value.ToString());
					double Exp = ((ItemQty > 0.0) ? (TotalShortage / ItemQty * Qty) : 0.0);
					obj.Cells["ScaleShortWt"].Value = Exp;
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["ScaleShortWt"].Value = 0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkScaleShortWeightApply_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			TotalEbPurchaseAgainstWeightUtilizeInGrid();
			SupplierShortageProportionInBreakUpGrid();
			ScaleShortageProportionInBreakUpGrid();
			UpdateBillWeightInBreakUpGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkSuppShortWeight_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			TotalEbPurchaseAgainstWeightUtilizeInGrid();
			SupplierShortageProportionInBreakUpGrid();
			ScaleShortageProportionInBreakUpGrid();
			UpdateBillWeightInBreakUpGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightProportion()
	{
		try
		{
			grd.UpdateData();
			double Freight = Conversion.ToDouble((object)txtcarramount.Text);
			double ItemQty = ((grdPurchaseBrakup.RowCount > 0) ? Conversion.ToDouble((object)grdPurchaseBrakup.GetTotal(grdPurchaseBrakup.RootTable.Columns["Qty"], (AggregateFunction)2).ToString()) : 0.0);
			if (Freight > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Qty = Conversion.ToDouble((object)obj.Cells["Qty"].Value.ToString());
					double Exp = ((ItemQty > 0.0 && Freight > 0.0) ? (Freight / ItemQty * Qty) : 0.0);
					obj.Cells["FreightAmount"].Value = Exp;
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["FreightAmount"].Value = 0;
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcarramount_Leave(object sender, EventArgs e)
	{
		try
		{
			FreightCalculations();
			if (RefDocumentTypeId == 105)
			{
				FreightProportion();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBiltyFreight_TextChanged(object sender, EventArgs e)
	{
		FreightCalculations();
	}

	private void txtAdvanceByParty_TextChanged(object sender, EventArgs e)
	{
		FreightCalculations();
	}

	private void txtAdvanceByFactory_TextChanged(object sender, EventArgs e)
	{
		FreightCalculations();
	}

	private void txtFreightDeduction_TextChanged(object sender, EventArgs e)
	{
		FreightCalculations();
	}

	private void FreightCalculations()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtBiltyFreight.Text.Trim().ToString());
			double SclaeCharges = Conversion.ToDouble((object)txtScaleCharges.Text.Trim().ToString());
			double AdvanceByParty = Conversion.ToDouble((object)txtAdvanceByParty.Text.Trim().ToString());
			double AdvanceByFactory = Conversion.ToDouble((object)txtAdvanceByFactory.Text.Trim().ToString());
			double OtherDeduction = Conversion.ToDouble((object)txtOtherDeduction.Text.Trim().ToString());
			double TotalFreight = num + SclaeCharges - (AdvanceByParty + AdvanceByFactory) - OtherDeduction;
			txtTotalFreight.Text = Math.Round(TotalFreight, 2).ToString();
			if (FreightId == 0)
			{
				double FreightDeduction = Conversion.ToDouble((object)txtFreightDeduction.Text.Trim().ToString());
				double CarrAmount = TotalFreight - FreightDeduction;
				txtcarramount.Text = Math.Round(CarrAmount, 2).ToString();
			}
			Total();
			StockWeightCalculationInGrid();
			FreightProportion();
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
	}

	private void btnGrnFormHistory_Click(object sender, EventArgs e)
	{
		try
		{
			frmGRNHistory obj = new frmGRNHistory(UserAccount);
			if (clsGlobalVariables.ScreenViewReights.Count > 0)
			{
				if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmGRNHistory").ToList()[0].Value.ToString()))
				{
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
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)cmbSupplierNameHistory).Text = string.Empty;
			GrdHistory.ClearStructure();
			GrdHistoryDetail.ClearStructure();
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
			HistoryComboBind(InvGrn.GetDataForDropDownFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, "46", (string)null, Conversion.ToString((object)UserAccount.BranchesId), 0));
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
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.BranchesId = UserAccount.BranchesId;
			grn.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.DocumentTypeId = 46;
			if (!grn.CanViewAllRecord)
			{
				grn.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			grn.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			grn.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			grn.SupplierCustomerId = Conversion.ToInt(cmbSupplierNameHistory.Value);
			if (RadReffered.Checked)
			{
				grn.ActionId = 1;
			}
			else if (RadNotReffered.Checked)
			{
				grn.ActionId = 2;
			}
			DataTable dtGridRegister = InvGrn.GetHisoty(grn);
			if (dtGridRegister.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("OrderId", typeof(int));
				dtcol.Columns.Add("OrderType", typeof(string));
				dtcol.Columns.Add("OrderNo", typeof(int));
				dtcol.Columns.Add("InvoiceNo", typeof(int));
				dtcol.Columns.Add("InwardGatePassId", typeof(int));
				dtcol.Columns.Add("ReferencePartyId", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DeliveryTerm", typeof(string));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("GpNo", typeof(int));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("WagesId", typeof(int));
				dtcol.Columns.Add("WagesNo", typeof(int));
				dtcol.Columns.Add("FactoryWeight", typeof(double));
				dtcol.Columns.Add("SuppWeight", typeof(double));
				dtcol.Columns.Add("Transporter", typeof(string));
				dtcol.Columns.Add("Freight", typeof(double));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("ApprovedDate", typeof(DateTime));
				dtcol.Columns.Add("ApprovedUser", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Columns.Add("AttachmentsCount", typeof(int));
				dtcol.Columns.Add("DetailIdsCount", typeof(int));
				dtcol.Columns.Add("RefDocumentTypeId", typeof(int));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtGridRegister.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtGridRegister.Rows[i]["Id"], dtGridRegister.Rows[i]["OrderId"], dtGridRegister.Rows[i]["OrderType"], dtGridRegister.Rows[i]["OrderNo"], dtGridRegister.Rows[i]["InvoiceNo"], dtGridRegister.Rows[i]["InwardGatePassId"], dtGridRegister.Rows[i]["ReferencePartyId"], dtGridRegister.Rows[i]["DocNo"], dtGridRegister.Rows[i]["DocDate"], dtGridRegister.Rows[i]["DocumentTypeId"], dtGridRegister.Rows[i]["DeliveryTerm"], dtGridRegister.Rows[i]["SupplierName"], dtGridRegister.Rows[i]["GpNo"], dtGridRegister.Rows[i]["VehicleNo"], dtGridRegister.Rows[i]["BiltyNo"], dtGridRegister.Rows[i]["WagesId"], dtGridRegister.Rows[i]["WagesNo"], dtGridRegister.Rows[i]["FactoryWeight"], dtGridRegister.Rows[i]["PartyWeight"], dtGridRegister.Rows[i]["Transporter"], dtGridRegister.Rows[i]["CarriageAmount"], dtGridRegister.Rows[i]["RemarksHeader"], dtGridRegister.Rows[i]["EntryDate"], dtGridRegister.Rows[i]["EntryUser"], dtGridRegister.Rows[i]["ModifyDate"], dtGridRegister.Rows[i]["ModifyUser"], dtGridRegister.Rows[i]["PostDate"], dtGridRegister.Rows[i]["ApprovedUser"], dtGridRegister.Rows[i]["NoOfAttachments"], dtGridRegister.Rows[i]["AttachmentsCount"], dtGridRegister.Rows[i]["DetailIdsCount"], dtGridRegister.Rows[i]["RefDocumentTypeId"]);
				}
				GrdHistory.DataSource = dtcol;
				GrdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				GrdHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "OrderId", "DocumentTypeId", "WagesId", "InwardGatePassId", "ReferencePartyId", "AttachmentsCount", "DetailIdsCount", "RefDocumentTypeId" };
			new Dictionary<string, EditType>
			{
				{
					"OrderId",
					(EditType)0
				},
				{
					"RecQty",
					(EditType)1
				},
				{
					"PurQty",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(GrdHistory, 2, 4, hiddenColumns);
			GrdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["OrderNo"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["WagesNo"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)GrdHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			GrdHistory.RootTable.Columns["OrderType"].Width = Constants.InventoryConstants.PackingType;
			GrdHistory.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			GrdHistory.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			GrdHistory.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			GrdHistory.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			GrdHistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			GrdHistory.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			GrdHistory.RootTable.Columns["WagesNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdHistory.RootTable.Columns["SuppWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			GrdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			GrdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			GrdHistory.RootTable.Columns["Transporter"].Width = Constants.InventoryConstants.CommissionAgent;
			GrdHistory.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.AmountConstant;
			GrdHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			GrdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			GridEX_Helper.AddButton(GrdHistory, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(GrdHistory, "Print", "Print", 40, 1);
			GrdHistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GrdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int CurrId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Column.Key == "OrderNo")
			{
				int CurrOrderId = Conversion.ToInt(GrdHistory.CurrentRow.Cells["OrderId"].Value);
				if (clsGlobalVariables.ScreenViewReights.Count > 0 && CurrOrderId > 0)
				{
					List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "PurchsaeOrder").ToList();
					if (newList.Count > 0)
					{
						if (!bool.Parse(newList[0].Value.ToString()))
						{
							MessageBox.Show("Please Check Screen Rights");
							return;
						}
						CommonServices.PurchaseOrderSlipReport203(CurrOrderId);
					}
				}
			}
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByScreenName(CurrId, base.Name);
			}
			if (e.Column.Key == "WagesNo")
			{
				int CurrWageId = Conversion.ToInt(r.Cells["WagesId"].Value);
				if (CurrWageId > 0)
				{
					CommonServices.ContractorWagesBill_SlipandRegister_002(CurrWageId);
				}
			}
			if (!(e.Column.Key == "GpNo"))
			{
				return;
			}
			int CurrGpId = Conversion.ToInt(r.Cells["InwardGatePassId"].Value);
			if (CurrGpId > 0)
			{
				if (Conversion.ToInt(r.Cells["DetailIdsCount"].Value) > 0)
				{
					CommonServices.GatePassInwardWithDetailAndLabReport(CurrGpId);
				}
				else
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(CurrGpId);
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int CurrId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (e.Column.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.GrnSlipWithSubReports(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int num = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				int RefDocTypeId = Conversion.ToInt((object)r.Cells["RefDocumentTypeId"].Value.ToString());
				InvGrn invGrn = InvGrn.GetByID(num);
				if (invGrn == null)
				{
					GrdHistoryDetail.ClearStructure();
					grdHistoryEB.ClearStructure();
					return;
				}
				DataTable dtDetailHistory = dtdetail.Clone();
				dtDetailHistory.Columns.Add("WareHouseName");
				dtDetailHistory.Columns.Add("CropYear");
				dtDetailHistory.Columns.Add("JobLot");
				dtDetailHistory.Columns.Add("PackingType");
				dtDetailHistory.Columns.Add("CityName");
				FilldtDetailFromListCommonForReadById(invGrn, dtDetailHistory, AddExtraColumns: true);
				GrdHistoryDetail.DataSource = dtDetailHistory;
				GrdHistoryDetail.RetrieveStructure();
				grdHistoryDetailSettings(RefDocTypeId);
				DataTable dtEb = dtEmptyBag.Clone();
				FillEBDetailFromListCommonForReadById(invGrn, dtEb);
				grdHistoryEB.DataSource = dtEb;
				grdHistoryEB.RetrieveStructure();
				grdHistoryEmptyBagsSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryDetailSettings(int RefDocTypeId)
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "OrderDetailId", "OrderId", "WareHouseId", "CropYearId", "JobId", "PackingTypeId", "CityId", "ItemId", "UOMId",
				"LabId", "UOMEquivalent", "WtCutOnId", "ItemCode", "EbPurAgainstWeight"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(GrdHistoryDetail, 3, 2, hiddenColumns);
			CommonDetailSettings(GrdHistoryDetail, RefDocTypeId);
			GrdHistoryDetail.RootTable.Columns["WareHouseName"].Position = GrdHistoryDetail.RootTable.Columns["WareHouseId"].Position + 1;
			GrdHistoryDetail.RootTable.Columns["CropYear"].Position = GrdHistoryDetail.RootTable.Columns["CropYearId"].Position + 1;
			GrdHistoryDetail.RootTable.Columns["JobLot"].Position = GrdHistoryDetail.RootTable.Columns["JobId"].Position + 1;
			GrdHistoryDetail.RootTable.Columns["PackingType"].Position = GrdHistoryDetail.RootTable.Columns["PackingTypeId"].Position + 1;
			GrdHistoryDetail.RootTable.Columns["CityName"].Position = GrdHistoryDetail.RootTable.Columns["CityId"].Position + 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryEmptyBagsSettings()
	{
		try
		{
			List<string> hiddenCol = new List<string> { "OrderId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistoryEB, 2, 2, hiddenCol);
			EmptyBagsGridComboBind(grdHistoryEB);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int PrintId)
	{
		try
		{
			CommonServices.GrnSlipWithSubReports(PrintId);
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
			GenerateReport(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip257_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.InwardGatePassWithWbAndLabSlip(GPID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtgwt_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtebu_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtwtcut_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtcarramount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtFromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextNumberFunction(sender, e);
	}

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextNumberFunction(sender, e);
	}

	private void txtEbUnitStock_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtEbTotalStock_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistoryDetail).Name);
			if (fs != null)
			{
				GrdHistoryDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistory).Name);
			if (fs != null)
			{
				GrdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdEmptyBags).Name);
			if (fs != null)
			{
				grdEmptyBags.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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

	private void InvFrmGRN_KeyDown(object sender, KeyEventArgs e)
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
			if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)GrdHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					GenerateReport(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
				}
				if (e.Control && e.KeyCode == Keys.H)
				{
					btnGrnFormHistory_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (BtnPrint.Visible && BtnPrint.Enabled && e.Control && e.KeyCode == Keys.P)
			{
				printToolStripButton_Click(null, null);
			}
			if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnSave_Click(null, null);
			}
			if (btnDelete.Visible && btnDelete.Enabled && e.Control && e.Alt && e.KeyCode == Keys.Delete)
			{
				btnDelete_Click(null, null);
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
					((Control)(object)grdEmptyBags).Focus();
				}
				else if (((Control)(object)grdEmptyBags).Focused)
				{
					((Control)(object)grdGp).Focus();
				}
				else if (((Control)(object)grdGp).Focused)
				{
					((Control)(object)grdPurchaseBrakup).Focus();
				}
				else
				{
					((Control)(object)grd).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				DocDate.Focus();
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
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GridEXRow item = grd.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				ScaleShortageProportion();
				SupplierShortageProportion();
				FreightProportion();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
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
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
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
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On warehouse Combo in Detail Box");
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGp.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdGp.CurrentColumn == null || !e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			if (grdGp.CurrentColumn.Key == "Load")
			{
				LoadGPByRow(Conversion.ToInt(r.Cells["Id"].Value));
			}
			else if (grdGp.CurrentColumn.Key == "GpSrNo")
			{
				if (Conversion.ToInt(r.Cells["DetailIdsCount"].Value) > 0)
				{
					CommonServices.GatePassInwardWithDetailAndLabReport(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(Conversion.ToInt(r.Cells["Id"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdEmptyBags.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdEmptyBags.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space && grdEmptyBags.CurrentColumn.Key == "Add")
			{
				if (grdEmptyBags.GetRows().Length == 1)
				{
					dtEmptyBag.Rows.Add(Conversion.ToInt(r.Cells["OrderId"].Value), Conversion.ToInt(r.Cells["Type"].Value), 0, Conversion.ToInt(r.Cells["Condition"].Value), 0, 0, "");
					grdEmptyBags.DataSource = dtEmptyBag;
				}
				grdEmptyBags.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || GrdHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (GrdHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(CurrId);
				}
				else if (GrdHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.GrnSlipWithSubReports(CurrId);
				}
				else if (GrdHistory.CurrentColumn.Key == "WagesView")
				{
					if (WagesStatus && WagesActiveOrInActive)
					{
						frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
						frmwagesBillHeader.RefDocTypeId = 46;
						frmwagesBillHeader.RefDocId = CurrId;
						frmwagesBillHeader.ShowDialog();
						frmwagesBillHeader.BringToFront();
					}
				}
				else if (GrdHistory.CurrentColumn.Key == "OrderNo")
				{
					if (clsGlobalVariables.ScreenViewReights.Count > 0)
					{
						List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "PurchsaeOrder").ToList();
						if (newList.Count > 0)
						{
							if (!bool.Parse(newList[0].Value.ToString()))
							{
								MessageBox.Show("Please Check Screen Rights");
								return;
							}
							CommonServices.PurchaseOrderSlipReport203(Conversion.ToInt(GrdHistory.CurrentRow.Cells["OrderId"].Value));
						}
					}
				}
				else if (GrdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, 217);
				}
				else if (GrdHistory.CurrentColumn.Key == "WagesNo")
				{
					CommonServices.ContractorWagesBill_SlipandRegister_002(Conversion.ToInt(r.Cells["WagesId"].Value));
				}
				else if (GrdHistory.CurrentColumn.Key == "GpNo")
				{
					if (Conversion.ToInt(r.Cells["DetailIdsCount"].Value) > 0)
					{
						CommonServices.GatePassInwardWithDetailAndLabReport(Conversion.ToInt(r.Cells["InwardGatePassId"].Value));
					}
					else
					{
						CommonServices.GatePassInwardSlipAndRegisterReport(Conversion.ToInt(r.Cells["InwardGatePassId"].Value));
					}
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

	private void grdPurchaseBrakup_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPurchaseBrakup.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdPurchaseBrakup.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPurchaseBrakup.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					GridEXRow item = grdPurchaseBrakup.CurrentRow;
					dtPurchaseBreakup.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtPurchaseBreakup.Rows.Count == 0)
					{
						AddRowInPurchaseBreakUpGrid();
					}
					SupplierShortageProportion();
					SupplierShortageProportionInBreakUpGrid();
					ScaleShortageProportionInBreakUpGrid();
					UpdateBillWeightInBreakUpGrid();
				}
				else if (grdPurchaseBrakup.CurrentColumn.Key == "Add")
				{
					AddRowInPurchaseBreakUpGrid();
					SupplierShortageProportion();
					SupplierShortageProportionInBreakUpGrid();
					ScaleShortageProportionInBreakUpGrid();
					UpdateBillWeightInBreakUpGrid();
				}
				grdPurchaseBrakup.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					GridEXRow item2 = grdPurchaseBrakup.CurrentRow;
					dtPurchaseBreakup.Rows[item2.RowIndex].Delete();
					if (item2 != null && (int)item2.RowType == 0)
					{
						item2.Delete();
					}
					if (dtPurchaseBreakup.Rows.Count == 0)
					{
						AddRowInPurchaseBreakUpGrid();
					}
					SupplierShortageProportion();
					SupplierShortageProportionInBreakUpGrid();
					ScaleShortageProportionInBreakUpGrid();
					UpdateBillWeightInBreakUpGrid();
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInPurchaseBreakUpGrid();
				SupplierShortageProportion();
				SupplierShortageProportionInBreakUpGrid();
				ScaleShortageProportionInBreakUpGrid();
				UpdateBillWeightInBreakUpGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateEBTotalWithGrossWeight_Click(object sender, EventArgs e)
	{
		try
		{
			UpdateEBTotalWithGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateEBTotalWithGrossWeight()
	{
		try
		{
			if (!EBWtAccordingToGrossWeight)
			{
				return;
			}
			double GrossWeightTotal = 0.0;
			double QtyIntoWeightTotal = 0.0;
			GridEXRow[] rows;
			if (grd.RowCount > 0)
			{
				rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					GrossWeightTotal += Conversion.ToDouble(r.Cells["GrossWight"].Value);
					QtyIntoWeightTotal += Conversion.ToDouble(r.Cells["Qty"].Value) * Conversion.ToDouble(r.Cells["UOM"].Value);
				}
			}
			if (!(GrossWeightTotal > QtyIntoWeightTotal))
			{
				return;
			}
			rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				r2.BeginEdit();
				double PackUOM = 0.0;
				double GrossWeight = 0.0;
				double EBUnit = 0.0;
				double EBTotal = 0.0;
				double WeightCutTotal = 0.0;
				double AddLess = 0.0;
				double FactoryWeight = 0.0;
				double SupplierWeight = 0.0;
				double DiffWeight = 0.0;
				if (txtfctwt.Text != string.Empty && txtfctwt.Text != null)
				{
					FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim().ToString());
				}
				if (txtsuppwt.Text != string.Empty && txtsuppwt.Text != null)
				{
					SupplierWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim().ToString());
				}
				EBUnit = Conversion.ToDouble(r2.Cells["EbUnit"].Value);
				PackUOM = Conversion.ToDouble(r2.Cells["UOM"].Value);
				GrossWeight = Conversion.ToDouble(r2.Cells["GrossWight"].Value);
				Conversion.ToDouble(r2.Cells["Qty"].Value);
				_ = GrossWeight / PackUOM;
				EBTotal = Conversion.ToDouble(r2.Cells["EbTotal"].Value);
				DiffWeight = SupplierWeight - (FactoryWeight + BillWeightAndStockWeightDifferenceTolerance);
				double PurQty = 0.0;
				double grdQty = 0.0;
				double TotalPurchaseAgainstWeightAddinBillWeight = 0.0;
				if (EBUnit > 0.0 && Conversion.ToDouble(r2.Cells["OrderId"].Value) == 0.0)
				{
					EmptyBagsWeightCutStatus = 0;
					grdQty = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["Qty"], (AggregateFunction)2));
					for (int i = 0; i < dtEmptyBag.Rows?.Count; i++)
					{
						if (Conversion.ToInt(dtEmptyBag.Rows[i]["Type"]) == 2)
						{
							PurQty += Conversion.ToDouble(dtEmptyBag.Rows[i]["PurQty"]);
						}
					}
					if (PurQty > 0.0)
					{
						double num = Math.Abs(PurQty) - grdQty;
						EmptyBagsWeightCutStatus = 2;
						TotalPurchaseAgainstWeightAddinBillWeight = num * EBUnit;
					}
				}
				WeightCutTotal = Conversion.ToDouble(r2.Cells["WtCutTotal"].Value);
				AddLess = Conversion.ToDouble(r2.Cells["AddLesswt"].Value);
				Conversion.ToDouble(r2.Cells["NetWeight"].Value);
				double Weight = 0.0;
				if (RefDocumentTypeId == 105)
				{
					Weight = GrossWeight - Math.Abs(AddLess);
				}
				else if (EmptyBagsWeightCutStatus == 2 && TotalPurchaseAgainstWeightAddinBillWeight > 0.0)
				{
					Weight = GrossWeight - WeightCutTotal - EBTotal - Math.Round(AddLess) + TotalPurchaseAgainstWeightAddinBillWeight;
				}
				else if (EmptyBagsWeightCutStatus == 2)
				{
					Weight = GrossWeight - WeightCutTotal - Math.Round(AddLess);
				}
				else if (EmptyBagsWeightCutStatus != 2)
				{
					Weight = GrossWeight - EBTotal - WeightCutTotal - Math.Round(AddLess);
				}
				r2.Cells["NetWeight"].Value = Math.Round(Weight, 2).ToString();
				if (SupplierWeight > 0.0 && FactoryWeight > 0.0 && GrossWeight > 0.0)
				{
					if (DeductionPolicyForGrnIsOn)
					{
						r2.Cells["StockWeight"].Value = FactoryWeight.ToString();
					}
					else
					{
						EBTotal = Conversion.ToDouble(r2.Cells["StockEbTotal"].Value);
						if (FreightId > 0)
						{
							if (DiffWeight >= 0.0)
							{
								double StockWt = FactoryWeight * GrossWeight / SupplierWeight;
								StockWt -= EBTotal;
								r2.Cells["StockWeight"].Value = Math.Round(StockWt, 2).ToString();
							}
							else
							{
								if (((Control)(object)txtDeliverTerm).Text == "Load" || ((Control)(object)txtDeliverTerm).Text == "Load & PartyWeight" || ((Control)(object)txtDeliverTerm).Text == "Ponch & PartyWeight")
								{
									double StockWt5 = FactoryWeight * GrossWeight / SupplierWeight;
									StockWt5 -= EBTotal;
									r2.Cells["StockWeight"].Value = Math.Round(StockWt5, 2).ToString();
								}
								if (((Control)(object)txtDeliverTerm).Text == "Ponch")
								{
									if (SupplierWeight > FactoryWeight)
									{
										double StockWt3 = FactoryWeight * GrossWeight / FactoryWeight;
										StockWt3 -= EBTotal;
										r2.Cells["StockWeight"].Value = Math.Round(StockWt3, 2).ToString();
									}
									else
									{
										double StockWt4 = FactoryWeight * GrossWeight / SupplierWeight;
										StockWt4 -= EBTotal;
										r2.Cells["StockWeight"].Value = Math.Round(StockWt4, 2).ToString();
									}
								}
								if (((Control)(object)txtDeliverTerm).Text == "Ponch & FactoryWeight" || ((Control)(object)txtDeliverTerm).Text == "Load & FactoryWeight")
								{
									double StockWt2 = FactoryWeight * GrossWeight / FactoryWeight;
									StockWt2 -= EBTotal;
									r2.Cells["StockWeight"].Value = Math.Round(StockWt2, 2).ToString();
								}
							}
						}
						else if (FreightId > 0)
						{
							if (DiffWeight >= 0.0)
							{
								double StockWt6 = FactoryWeight * GrossWeight / SupplierWeight;
								StockWt6 -= EBTotal;
								r2.Cells["StockWeight"].Value = Math.Round(StockWt6, 2).ToString();
							}
							else if (FactoryWeight > SupplierWeight)
							{
								double StockWt7 = FactoryWeight * GrossWeight / SupplierWeight;
								StockWt7 -= EBTotal;
								r2.Cells["StockWeight"].Value = Math.Round(StockWt7, 2).ToString();
							}
							else
							{
								double StockWt8 = FactoryWeight * GrossWeight / FactoryWeight;
								StockWt8 -= EBTotal;
								r2.Cells["StockWeight"].Value = Math.Round(StockWt8, 2).ToString();
							}
						}
						else
						{
							if (((Control)(object)txtDeliverTerm).Text == "Load" || ((Control)(object)txtDeliverTerm).Text == "Load & PartyWeight" || ((Control)(object)txtDeliverTerm).Text == "Ponch & PartyWeight")
							{
								double StockWt12 = FactoryWeight * GrossWeight / SupplierWeight;
								StockWt12 -= EBTotal;
								r2.Cells["StockWeight"].Value = Math.Round(StockWt12, 2).ToString();
							}
							if (((Control)(object)txtDeliverTerm).Text == "Ponch")
							{
								if (SupplierWeight > FactoryWeight)
								{
									double StockWt10 = FactoryWeight * GrossWeight / FactoryWeight;
									StockWt10 -= EBTotal;
									r2.Cells["StockWeight"].Value = Math.Round(StockWt10, 2).ToString();
								}
								else
								{
									double StockWt11 = FactoryWeight * GrossWeight / SupplierWeight;
									StockWt11 -= EBTotal;
									r2.Cells["StockWeight"].Value = Math.Round(StockWt11, 2).ToString();
								}
							}
							if (((Control)(object)txtDeliverTerm).Text == "Ponch & FactoryWeight" || ((Control)(object)txtDeliverTerm).Text == "Load & FactoryWeight")
							{
								double StockWt9 = FactoryWeight * GrossWeight / FactoryWeight;
								StockWt9 -= EBTotal;
								r2.Cells["StockWeight"].Value = Math.Round(StockWt9, 2).ToString();
							}
						}
					}
				}
				r2.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbLabNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (WeightCutEditable)
			{
				txtwtcut.Enabled = true;
				txtwtcut.ReadOnly = false;
			}
			txtLabWeightCutValue.Text = 0.ToString();
			txtLabRateCutValue.Text = Conversion.ToString((object)0);
			chkLabRatecutcompulsory.Checked = false;
			chkLabWtcutcompulsory.Checked = false;
			if (((UltraDropDownBase)CmbLabNo).SelectedRow != null && Conversion.ToInt(CmbLabNo.Value) > 0)
			{
				double wtCut = Conversion.ToDouble(((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["WtCut"].Value);
				string wtCutOn = Conversion.ToString(((UltraDropDownBase)CmbLabNo).SelectedRow.Cells["WtCutOn"].Value);
				txtwtcut.Text = wtCut.ToString();
				txtWtCutOn.Text = wtCutOn;
				DataTable dt = InvLabAnalysisPurchaseHeader.getLabAnaylsisAgainstMasterParamsById(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbLabNo.Value));
				if (dt.Rows.Count > 0)
				{
					txtLabWeightCutValue.Text = wtCut.ToString();
					txtLabRateCutValue.Text = Conversion.ToString(dt.Rows[0]["DeductionRate"]);
					chkLabRatecutcompulsory.Checked = Conversion.ToBool(dt.Rows[0]["IsRateCutCompulsory"]);
					chkLabWtcutcompulsory.Checked = Conversion.ToBool(dt.Rows[0]["IsWeightCutCompulsory"]);
					if (Conversion.ToBool(dt.Rows[0]["IsWeightCutCompulsory"]))
					{
						txtwtcut.Enabled = false;
					}
					else if (WeightCutEditable)
					{
						txtwtcut.Enabled = true;
						txtwtcut.ReadOnly = false;
					}
					PanelCurrentLabResult.Visible = true;
					DataTable dtColumns = new DataTable();
					dtColumns.Columns.Add("Parameter");
					dtColumns.Columns.Add("Result", typeof(double));
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtColumns.Rows.Add(dt.Rows[i]["AnalysisParameterDescription"], dt.Rows[i]["InAnalysisResult"]);
					}
					grdLabInfo.DataSource = dtColumns;
					grdLabInfo.RetrieveStructure();
					grdLabInfo.TotalRow = (InheritableBoolean)2;
					grdLabInfo.RootTable.Columns["Parameter"].Width = 120;
					grdLabInfo.RootTable.Columns["Result"].Width = 70;
					grdLabInfo.RootTable.Columns["Result"].TextAlignment = (TextAlignment)2;
				}
				else
				{
					PanelCurrentLabResult.Visible = false;
					grdLabInfo.DataSource = null;
				}
			}
			else
			{
				txtwtcut.Text = "0";
				txtWtCutOn.Text = "";
				grdLabInfo.DataSource = null;
				PanelCurrentLabResult.Visible = false;
			}
			if (RefDocumentTypeId == 105)
			{
				EbCalculations();
			}
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbLabNo_AfterCloseUp(object sender, EventArgs e)
	{
	}

	private void combsupplier_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(combsupplier.Value);
			if (Id > 0 || PurchaseOrderId > 0)
			{
				PreBillNoFill(Id, PurchaseOrderId);
				return;
			}
			((Control)(object)CmbSupplierDispatchedPreBillNo).Text = string.Empty;
			((UltraGridBase)CmbSupplierDispatchedPreBillNo).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplierDispatchedPreBillNo_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbSupplierDispatchedPreBillNo.Value);
			if (((UltraGridBase)CmbSupplierDispatchedPreBillNo).ActiveRow != null && Id > 0)
			{
				CellsCollection row = ((UltraDropDownBase)CmbSupplierDispatchedPreBillNo).SelectedRow.Cells;
				string vehicleNo = Conversion.ToString(row["VehicleNo"].Value);
				if (!string.IsNullOrWhiteSpace(vehicleNo) && string.IsNullOrWhiteSpace(txtvehno.Text))
				{
					txtvehno.Text = vehicleNo;
				}
				string biltyNo = Conversion.ToString(row["BiltyNo"].Value);
				if (!string.IsNullOrWhiteSpace(biltyNo) && string.IsNullOrWhiteSpace(txtbltyno.Text))
				{
					txtbltyno.Text = biltyNo;
				}
				if (FreightId == 0)
				{
					double freight = Conversion.ToDouble(row["Freight"].Value);
					if (freight > 0.0)
					{
						txtBiltyFreight.Text = freight.ToString("#,##");
					}
					double advanceFreight = Conversion.ToDouble(row["AdvanceFreight"].Value);
					if (advanceFreight > 0.0)
					{
						txtAdvanceByParty.Text = advanceFreight.ToString("#,##");
					}
				}
			}
			txtvehno.Enabled = Id == 0;
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
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
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
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
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
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Expected O, but got Unknown
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Expected O, but got Unknown
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Expected O, but got Unknown
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a8: Expected O, but got Unknown
		//IL_07eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Expected O, but got Unknown
		//IL_080c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0816: Expected O, but got Unknown
		//IL_082d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Expected O, but got Unknown
		//IL_0838: Unknown result type (might be due to invalid IL or missing references)
		//IL_0842: Expected O, but got Unknown
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_0863: Expected O, but got Unknown
		//IL_0890: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Expected O, but got Unknown
		//IL_0982: Unknown result type (might be due to invalid IL or missing references)
		//IL_098c: Expected O, but got Unknown
		//IL_0a27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a31: Expected O, but got Unknown
		//IL_0ac1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acb: Expected O, but got Unknown
		//IL_0ae2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Expected O, but got Unknown
		//IL_0b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b86: Expected O, but got Unknown
		//IL_0c63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6d: Expected O, but got Unknown
		//IL_0d29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d33: Expected O, but got Unknown
		//IL_0d76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d80: Expected O, but got Unknown
		//IL_0da2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dac: Expected O, but got Unknown
		//IL_15b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bb: Expected O, but got Unknown
		//IL_15c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d2: Expected O, but got Unknown
		//IL_15df: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e9: Expected O, but got Unknown
		//IL_1b92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9c: Expected O, but got Unknown
		//IL_1ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb3: Expected O, but got Unknown
		//IL_1efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f05: Expected O, but got Unknown
		//IL_688b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6895: Expected O, but got Unknown
		//IL_68a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_68ac: Expected O, but got Unknown
		//IL_b0d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_b0da: Expected O, but got Unknown
		//IL_b0fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_b108: Expected O, but got Unknown
		GridEXLayout grdGp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBags_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdLabInfo_DesignTimeLayout = new GridEXLayout();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
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
		GridEXLayout grdPurchaseBrakup_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Purchase.InvFrmGRN));
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryEB_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdGp = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtAcceptedGpTotal = new System.Windows.Forms.Label();
		this.txtOpenGpTotal = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grdEmptyBags = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.PanelCurrentLabResult = new System.Windows.Forms.Panel();
		this.grdLabInfo = new GridEX();
		this.panel19 = new System.Windows.Forms.Panel();
		this.label58 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnLoadGatePass = new System.Windows.Forms.Button();
		this.txtEbUnitStock = new System.Windows.Forms.TextBox();
		this.txtEbTotalStock = new System.Windows.Forms.TextBox();
		this.label59 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.chkLabRatecutcompulsory = new System.Windows.Forms.CheckBox();
		this.chkLabWtcutcompulsory = new System.Windows.Forms.CheckBox();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.txtWtCutOn = new System.Windows.Forms.TextBox();
		this.label57 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.Add = new System.Windows.Forms.Button();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbLabNo = new UltraCombo();
		this.txtMoisture = new System.Windows.Forms.TextBox();
		this.txtgwt = new System.Windows.Forms.TextBox();
		this.txtebu = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.txtLabRateCutValue = new System.Windows.Forms.TextBox();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.txtLabWeightCutValue = new System.Windows.Forms.TextBox();
		this.txtwtcut = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtadlswt = new System.Windows.Forms.TextBox();
		this.combitem = new UltraCombo();
		this.txtwtcuttotal = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtstockwt = new System.Windows.Forms.TextBox();
		this.label60 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.txtEmptyBagsTotal = new System.Windows.Forms.TextBox();
		this.CmbCropYear = new UltraCombo();
		this.txtnetwt = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.CmbCity = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.CmbWareHouseName = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.label43 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.CmbPackingType = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.txtEbWtOther = new System.Windows.Forms.TextBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.PanelBreakupMain = new System.Windows.Forms.Panel();
		this.grdPurchaseBrakup = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label38 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.txtAdvanceByFactory = new System.Windows.Forms.TextBox();
		this.txtcarramount = new System.Windows.Forms.TextBox();
		this.txtScaleCharges = new System.Windows.Forms.TextBox();
		this.txtChargeToPartyAmountFr = new System.Windows.Forms.TextBox();
		this.txtShortageAmountFr = new System.Windows.Forms.TextBox();
		this.txtOtherDeduction = new System.Windows.Forms.TextBox();
		this.txtTotalFreight = new System.Windows.Forms.TextBox();
		this.txtFreightDeduction = new System.Windows.Forms.TextBox();
		this.label64 = new System.Windows.Forms.Label();
		this.txtAdvanceByParty = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.label69 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.txtRemainingAmountFr = new System.Windows.Forms.TextBox();
		this.txtDiscountAmountFr = new System.Windows.Forms.TextBox();
		this.label66 = new System.Windows.Forms.Label();
		this.txtBiltyFreight = new System.Windows.Forms.TextBox();
		this.CmbTransport = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtDifferenceWeight = new System.Windows.Forms.TextBox();
		this.txtGrnWeight = new System.Windows.Forms.TextBox();
		this.txtTicketnos = new System.Windows.Forms.TextBox();
		this.txtfctwt = new System.Windows.Forms.TextBox();
		this.txtBalWeight = new System.Windows.Forms.TextBox();
		this.txtsuppwt = new System.Windows.Forms.TextBox();
		this.CmbSupplierDispatchedPreBillNo = new UltraCombo();
		this.txtReceivedWeight = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.lblDiffweight = new System.Windows.Forms.Label();
		this.txtAccessWeight = new System.Windows.Forms.TextBox();
		this.labelExcessWt = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.lblBalwt = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtAnaylstName = new System.Windows.Forms.TextBox();
		this.combvehtyp = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.combsupplier = new UltraCombo();
		this.combgatepass = new System.Windows.Forms.TextBox();
		this.lblRecwt = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtbltyno = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtvehno = new System.Windows.Forms.TextBox();
		this.txtDeliverTerm = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.chkSuppShortWeight = new System.Windows.Forms.CheckBox();
		this.chkScaleShortWeightApply = new System.Windows.Forms.CheckBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnSlip257 = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.CityDefine = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnUpdateEBTotalWithGrossWeight = new System.Windows.Forms.ToolStripButton();
		this.label13 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GrdHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel23 = new System.Windows.Forms.Panel();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.RadNotReffered = new System.Windows.Forms.RadioButton();
		this.RadReffered = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label49 = new System.Windows.Forms.Label();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.cmbSupplierNameHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetailH = new System.Windows.Forms.TabPage();
		this.GrdHistoryDetail = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label32 = new System.Windows.Forms.Label();
		this.tabEBH = new System.Windows.Forms.TabPage();
		this.grdHistoryEB = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label54 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label55 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnGrnFormHistory = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label37 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGp).BeginInit();
		this.panel12.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).BeginInit();
		this.panel14.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel16.SuspendLayout();
		this.panel5.SuspendLayout();
		this.PanelCurrentLabResult.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdLabInfo).BeginInit();
		this.panel19.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLabNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		this.panel4.SuspendLayout();
		this.PanelBreakupMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseBrakup).BeginInit();
		this.panel22.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierDispatchedPreBillNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtDeliverTerm).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.panel18.SuspendLayout();
		this.panel23.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).BeginInit();
		this.tabControl2.SuspendLayout();
		this.tabDetailH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel20.SuspendLayout();
		this.tabEBH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryEB).BeginInit();
		this.panel11.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1384, 792);
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
		this.tabControl1.Size = new System.Drawing.Size(1384, 792);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1376, 763);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 564);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1372, 197);
		this.panel6.TabIndex = 6;
		this.panel9.Controls.Add(this.panel13);
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(559, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(813, 197);
		this.panel9.TabIndex = 11;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdGp);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 35);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(813, 162);
		this.panel13.TabIndex = 2;
		this.grdGp.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGp).BackColor = System.Drawing.Color.White;
		grdGp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGp.DesignTimeLayout = grdGp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGp).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGp.FilterMode = (FilterMode)2;
		this.grdGp.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGp.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grdGp.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdGp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGp).Name = "grdGp";
		((ScrollableBase)this.grdGp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGp).Size = new System.Drawing.Size(813, 162);
		((System.Windows.Forms.Control)(object)this.grdGp).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdGp).TabStop = false;
		this.grdGp.TotalRow = (InheritableBoolean)1;
		this.grdGp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGp).VisualStyle = (VisualStyle)4;
		this.grdGp.FormattingRow += new RowLoadEventHandler(grdGp_FormattingRow);
		this.grdGp.ColumnButtonClick += new ColumnActionEventHandler(grdGp_ColumnButtonClick);
		this.grdGp.LinkClicked += new ColumnActionEventHandler(grdGp_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdGp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGp_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label4);
		this.panel12.Controls.Add(this.ctrlGrdBar3);
		this.panel12.Controls.Add(this.label47);
		this.panel12.Controls.Add(this.label33);
		this.panel12.Controls.Add(this.txtAcceptedGpTotal);
		this.panel12.Controls.Add(this.txtOpenGpTotal);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(813, 35);
		this.panel12.TabIndex = 1;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(6, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(364, 30);
		this.label4.TabIndex = 24;
		this.label4.Text = "This Grid show Pending Gatepases For Grn Entry,User can Load GP With Accepted Status for Entry,GP With Open Status Will Not Load";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label47.ForeColor = System.Drawing.Color.Blue;
		this.label47.Location = new System.Drawing.Point(466, 1);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(95, 19);
		this.label47.TabIndex = 0;
		this.label47.Text = "Accepted Gp";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label33.ForeColor = System.Drawing.Color.Blue;
		this.label33.Location = new System.Drawing.Point(385, 1);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(72, 19);
		this.label33.TabIndex = 0;
		this.label33.Text = "Open  Gp";
		this.txtAcceptedGpTotal.AutoSize = true;
		this.txtAcceptedGpTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAcceptedGpTotal.ForeColor = System.Drawing.Color.White;
		this.txtAcceptedGpTotal.Location = new System.Drawing.Point(491, 18);
		this.txtAcceptedGpTotal.Name = "txtAcceptedGpTotal";
		this.txtAcceptedGpTotal.Size = new System.Drawing.Size(15, 16);
		this.txtAcceptedGpTotal.TabIndex = 0;
		this.txtAcceptedGpTotal.Text = "0";
		this.txtAcceptedGpTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtOpenGpTotal.AutoSize = true;
		this.txtOpenGpTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOpenGpTotal.ForeColor = System.Drawing.Color.White;
		this.txtOpenGpTotal.Location = new System.Drawing.Point(407, 18);
		this.txtOpenGpTotal.Name = "txtOpenGpTotal";
		this.txtOpenGpTotal.Size = new System.Drawing.Size(15, 16);
		this.txtOpenGpTotal.TabIndex = 0;
		this.txtOpenGpTotal.Text = "0";
		this.txtOpenGpTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel3.Controls.Add(this.panel15);
		this.panel3.Controls.Add(this.panel14);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(559, 197);
		this.panel3.TabIndex = 10;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBags);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(0, 35);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(559, 162);
		this.panel15.TabIndex = 3;
		((ScrollableBase)this.grdEmptyBags).BackColor = System.Drawing.Color.White;
		this.grdEmptyBags.ColumnAutoResize = true;
		grdEmptyBags_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBags.DesignTimeLayout = grdEmptyBags_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBags.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Name = "grdEmptyBags";
		((ScrollableBase)this.grdEmptyBags).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Size = new System.Drawing.Size(559, 162);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).TabIndex = 0;
		this.grdEmptyBags.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBags.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBags).VisualStyle = (VisualStyle)4;
		this.grdEmptyBags.CellUpdated += new ColumnActionEventHandler(grdEmptyBags_CellUpdated);
		this.grdEmptyBags.ColumnButtonClick += new ColumnActionEventHandler(grdEmptyBags_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).KeyDown += new System.Windows.Forms.KeyEventHandler(grdEmptyBags_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label11);
		this.panel14.Controls.Add(this.ctrlGrdBar5);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(559, 35);
		this.panel14.TabIndex = 2;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(3, 5);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(240, 26);
		this.label11.TabIndex = 25;
		this.label11.Text = "This Grid show Empty Bags Info.\r\nUser Can Add Empty Bags Info Here";
		this.panel10.Controls.Add(this.panel17);
		this.panel10.Controls.Add(this.panel16);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 397);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1372, 167);
		this.panel10.TabIndex = 5;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 25);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1372, 142);
		this.panel17.TabIndex = 4;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1372, 142);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label14);
		this.panel16.Controls.Add(this.ctrlGrdBar1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1372, 25);
		this.panel16.TabIndex = 3;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 5);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(0, 15);
		this.label14.TabIndex = 26;
		this.panel5.Controls.Add(this.PanelCurrentLabResult);
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Controls.Add(this.label16);
		this.panel5.Controls.Add(this.txtEbWtOther);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 259);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1372, 138);
		this.panel5.TabIndex = 1;
		this.PanelCurrentLabResult.Controls.Add((System.Windows.Forms.Control)(object)this.grdLabInfo);
		this.PanelCurrentLabResult.Controls.Add(this.panel19);
		this.PanelCurrentLabResult.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelCurrentLabResult.Location = new System.Drawing.Point(1158, 0);
		this.PanelCurrentLabResult.Name = "PanelCurrentLabResult";
		this.PanelCurrentLabResult.Size = new System.Drawing.Size(220, 138);
		this.PanelCurrentLabResult.TabIndex = 46;
		this.PanelCurrentLabResult.Visible = false;
		((ScrollableBase)this.grdLabInfo).BackColor = System.Drawing.Color.White;
		this.grdLabInfo.ColumnAutoResize = true;
		grdLabInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdLabInfo.DesignTimeLayout = grdLabInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdLabInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdLabInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdLabInfo.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdLabInfo).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdLabInfo).Name = "grdLabInfo";
		((ScrollableBase)this.grdLabInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdLabInfo).Size = new System.Drawing.Size(220, 119);
		((System.Windows.Forms.Control)(object)this.grdLabInfo).TabIndex = 1;
		this.grdLabInfo.TotalRow = (InheritableBoolean)1;
		this.grdLabInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdLabInfo).VisualStyle = (VisualStyle)4;
		this.panel19.BackColor = System.Drawing.Color.White;
		this.panel19.Controls.Add(this.label58);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(220, 19);
		this.panel19.TabIndex = 4;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label58.ForeColor = System.Drawing.Color.Teal;
		this.label58.Location = new System.Drawing.Point(2, 3);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(130, 14);
		this.label58.TabIndex = 0;
		this.label58.Text = "Current Lab Result";
		this.groupBox2.Controls.Add(this.btnLoadGatePass);
		this.groupBox2.Controls.Add(this.txtEbUnitStock);
		this.groupBox2.Controls.Add(this.txtEbTotalStock);
		this.groupBox2.Controls.Add(this.label59);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.chkLabRatecutcompulsory);
		this.groupBox2.Controls.Add(this.chkLabWtcutcompulsory);
		this.groupBox2.Controls.Add(this.rdSearchByCode);
		this.groupBox2.Controls.Add(this.rdSearchByName);
		this.groupBox2.Controls.Add(this.txtWtCutOn);
		this.groupBox2.Controls.Add(this.label57);
		this.groupBox2.Controls.Add(this.label56);
		this.groupBox2.Controls.Add(this.Add);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLabNo);
		this.groupBox2.Controls.Add(this.txtMoisture);
		this.groupBox2.Controls.Add(this.txtgwt);
		this.groupBox2.Controls.Add(this.txtebu);
		this.groupBox2.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox2.Controls.Add(this.txtLabRateCutValue);
		this.groupBox2.Controls.Add(this.txtqty);
		this.groupBox2.Controls.Add(this.txtLabWeightCutValue);
		this.groupBox2.Controls.Add(this.txtwtcut);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.txtadlswt);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.groupBox2.Controls.Add(this.txtwtcuttotal);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.textBox1);
		this.groupBox2.Controls.Add(this.txtstockwt);
		this.groupBox2.Controls.Add(this.label60);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.txtEmptyBagsTotal);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox2.Controls.Add(this.txtnetwt);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add(this.label46);
		this.groupBox2.Controls.Add(this.label41);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.groupBox2.Controls.Add(this.label42);
		this.groupBox2.Controls.Add(this.label45);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseName);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox2.Controls.Add(this.label43);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1158, 138);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail Infomation";
		this.btnLoadGatePass.BackColor = System.Drawing.Color.Teal;
		this.btnLoadGatePass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadGatePass.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnLoadGatePass.ForeColor = System.Drawing.Color.White;
		this.btnLoadGatePass.Location = new System.Drawing.Point(720, 110);
		this.btnLoadGatePass.Margin = new System.Windows.Forms.Padding(2);
		this.btnLoadGatePass.Name = "btnLoadGatePass";
		this.btnLoadGatePass.Size = new System.Drawing.Size(122, 28);
		this.btnLoadGatePass.TabIndex = 63;
		this.btnLoadGatePass.Text = "Load Gate Pass";
		this.btnLoadGatePass.UseVisualStyleBackColor = false;
		this.btnLoadGatePass.Click += new System.EventHandler(btnLoadGatePass_Click);
		this.txtEbUnitStock.BackColor = System.Drawing.Color.White;
		this.txtEbUnitStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnitStock.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnitStock.Location = new System.Drawing.Point(493, 86);
		this.txtEbUnitStock.Name = "txtEbUnitStock";
		this.txtEbUnitStock.Size = new System.Drawing.Size(55, 23);
		this.txtEbUnitStock.TabIndex = 17;
		this.txtEbUnitStock.Tag = "EbStockUnit";
		this.txtEbUnitStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbUnitStock.TextChanged += new System.EventHandler(txtEbUnitStock_TextChanged);
		this.txtEbUnitStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnitStock_KeyPress);
		this.txtEbTotalStock.BackColor = System.Drawing.Color.White;
		this.txtEbTotalStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotalStock.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotalStock.Location = new System.Drawing.Point(549, 86);
		this.txtEbTotalStock.Name = "txtEbTotalStock";
		this.txtEbTotalStock.Size = new System.Drawing.Size(70, 23);
		this.txtEbTotalStock.TabIndex = 18;
		this.txtEbTotalStock.Tag = "EbStockTotal";
		this.txtEbTotalStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbTotalStock.TextChanged += new System.EventHandler(txtEbTotalStock_TextChanged);
		this.txtEbTotalStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbTotalStock_KeyPress);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(549, 66);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(52, 15);
		this.label59.TabIndex = 61;
		this.label59.Text = "E.b Total";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(494, 66);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(56, 15);
		this.label12.TabIndex = 62;
		this.label12.Text = "E.b / Unit";
		this.chkLabRatecutcompulsory.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkLabRatecutcompulsory.AutoSize = true;
		this.chkLabRatecutcompulsory.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.chkLabRatecutcompulsory.Enabled = false;
		this.chkLabRatecutcompulsory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkLabRatecutcompulsory.ForeColor = System.Drawing.Color.Black;
		this.chkLabRatecutcompulsory.Location = new System.Drawing.Point(479, 113);
		this.chkLabRatecutcompulsory.Margin = new System.Windows.Forms.Padding(2);
		this.chkLabRatecutcompulsory.Name = "chkLabRatecutcompulsory";
		this.chkLabRatecutcompulsory.Size = new System.Drawing.Size(179, 20);
		this.chkLabRatecutcompulsory.TabIndex = 18;
		this.chkLabRatecutcompulsory.Text = "Rate Cut Compulsory";
		this.chkLabRatecutcompulsory.UseVisualStyleBackColor = true;
		this.chkLabWtcutcompulsory.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkLabWtcutcompulsory.AutoSize = true;
		this.chkLabWtcutcompulsory.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.chkLabWtcutcompulsory.Enabled = false;
		this.chkLabWtcutcompulsory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkLabWtcutcompulsory.ForeColor = System.Drawing.Color.Black;
		this.chkLabWtcutcompulsory.Location = new System.Drawing.Point(218, 113);
		this.chkLabWtcutcompulsory.Margin = new System.Windows.Forms.Padding(2);
		this.chkLabWtcutcompulsory.Name = "chkLabWtcutcompulsory";
		this.chkLabWtcutcompulsory.Size = new System.Drawing.Size(197, 20);
		this.chkLabWtcutcompulsory.TabIndex = 18;
		this.chkLabWtcutcompulsory.Text = "Weight Cut Compulsory";
		this.chkLabWtcutcompulsory.UseVisualStyleBackColor = true;
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(261, 17);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 58;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(208, 17);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 57;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.txtWtCutOn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWtCutOn.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWtCutOn.Location = new System.Drawing.Point(143, 86);
		this.txtWtCutOn.Name = "txtWtCutOn";
		this.txtWtCutOn.Size = new System.Drawing.Size(65, 23);
		this.txtWtCutOn.TabIndex = 12;
		this.txtWtCutOn.TabStop = false;
		this.txtWtCutOn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(143, 66);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(63, 15);
		this.label57.TabIndex = 51;
		this.label57.Text = "Wt Cut On";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(72, 66);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(45, 15);
		this.label56.TabIndex = 49;
		this.label56.Text = "Lab No";
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(1030, 83);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(36, 28);
		this.Add.TabIndex = 22;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(711, 64);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(65, 19);
		this.label34.TabIndex = 47;
		this.label34.Text = "Moisture";
		this.CmbLabNo.AllowNull = (DefaultableBoolean)1;
		this.CmbLabNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLabNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLabNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLabNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLabNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbLabNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLabNo).Location = new System.Drawing.Point(72, 84);
		((System.Windows.Forms.Control)(object)this.CmbLabNo).Name = "CmbLabNo";
		((System.Windows.Forms.Control)(object)this.CmbLabNo).Size = new System.Drawing.Size(70, 26);
		((System.Windows.Forms.Control)(object)this.CmbLabNo).TabIndex = 11;
		this.CmbLabNo.AfterCloseUp += new System.EventHandler(CmbLabNo_AfterCloseUp);
		((System.Windows.Forms.Control)(object)this.CmbLabNo).Leave += new System.EventHandler(CmbLabNo_Leave);
		this.txtMoisture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoisture.Enabled = false;
		this.txtMoisture.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoisture.Location = new System.Drawing.Point(711, 86);
		this.txtMoisture.Name = "txtMoisture";
		this.txtMoisture.Size = new System.Drawing.Size(70, 23);
		this.txtMoisture.TabIndex = 20;
		this.txtMoisture.Text = "0";
		this.txtMoisture.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtgwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgwt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgwt.Location = new System.Drawing.Point(939, 38);
		this.txtgwt.Name = "txtgwt";
		this.txtgwt.Size = new System.Drawing.Size(90, 23);
		this.txtgwt.TabIndex = 8;
		this.txtgwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtgwt.TextChanged += new System.EventHandler(txtgwt_TextChanged);
		this.txtgwt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgwt_KeyPress);
		this.txtgwt.Leave += new System.EventHandler(txtgwt_Leave);
		this.txtebu.BackColor = System.Drawing.Color.White;
		this.txtebu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtebu.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtebu.Location = new System.Drawing.Point(1101, 38);
		this.txtebu.Name = "txtebu";
		this.txtebu.Size = new System.Drawing.Size(55, 23);
		this.txtebu.TabIndex = 9;
		this.txtebu.Tag = "EbUnit";
		this.txtebu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtebu.TextChanged += new System.EventHandler(txtebu_TextChanged_1);
		this.txtebu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtebu_KeyPress);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1095, 84);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(60, 26);
		this.btnCancelUpdateDetial.TabIndex = 24;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.txtLabRateCutValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLabRateCutValue.Enabled = false;
		this.txtLabRateCutValue.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLabRateCutValue.Location = new System.Drawing.Point(663, 112);
		this.txtLabRateCutValue.Name = "txtLabRateCutValue";
		this.txtLabRateCutValue.Size = new System.Drawing.Size(55, 23);
		this.txtLabRateCutValue.TabIndex = 13;
		this.txtLabRateCutValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLabRateCutValue.TextChanged += new System.EventHandler(txtebu_TextChanged);
		this.txtLabRateCutValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtwtcut_KeyPress);
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(863, 38);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(75, 23);
		this.txtqty.TabIndex = 7;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtqty.Leave += new System.EventHandler(txtqty_Leave);
		this.txtLabWeightCutValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLabWeightCutValue.Enabled = false;
		this.txtLabWeightCutValue.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLabWeightCutValue.Location = new System.Drawing.Point(420, 112);
		this.txtLabWeightCutValue.Name = "txtLabWeightCutValue";
		this.txtLabWeightCutValue.Size = new System.Drawing.Size(55, 23);
		this.txtLabWeightCutValue.TabIndex = 13;
		this.txtLabWeightCutValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLabWeightCutValue.TextChanged += new System.EventHandler(txtebu_TextChanged);
		this.txtLabWeightCutValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtwtcut_KeyPress);
		this.txtwtcut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtcut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtwtcut.Location = new System.Drawing.Point(209, 86);
		this.txtwtcut.Name = "txtwtcut";
		this.txtwtcut.Size = new System.Drawing.Size(55, 23);
		this.txtwtcut.TabIndex = 13;
		this.txtwtcut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtwtcut.TextChanged += new System.EventHandler(txtebu_TextChanged);
		this.txtwtcut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtwtcut_KeyPress);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1030, 84);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(63, 26);
		this.btnUpdateDetail.TabIndex = 23;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtadlswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtadlswt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtadlswt.Location = new System.Drawing.Point(336, 86);
		this.txtadlswt.Name = "txtadlswt";
		this.txtadlswt.Size = new System.Drawing.Size(65, 23);
		this.txtadlswt.TabIndex = 15;
		this.txtadlswt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtadlswt.TextChanged += new System.EventHandler(txtadlswt_TextChanged_1);
		this.txtadlswt.Leave += new System.EventHandler(txtadlswt_Leave);
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance124).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(175, 36);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 2;
		this.combitem.ValueChanged += new System.EventHandler(combitem_Leave);
		((System.Windows.Forms.Control)(object)this.combitem).Leave += new System.EventHandler(combitem_Leave);
		this.txtwtcuttotal.BackColor = System.Drawing.Color.White;
		this.txtwtcuttotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtcuttotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtwtcuttotal.Location = new System.Drawing.Point(265, 86);
		this.txtwtcuttotal.Name = "txtwtcuttotal";
		this.txtwtcuttotal.ReadOnly = true;
		this.txtwtcuttotal.Size = new System.Drawing.Size(70, 23);
		this.txtwtcuttotal.TabIndex = 14;
		this.txtwtcuttotal.TabStop = false;
		this.txtwtcuttotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(265, 66);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(73, 15);
		this.label9.TabIndex = 44;
		this.label9.Text = "Wt Cut Total";
		this.txtstockwt.BackColor = System.Drawing.Color.White;
		this.txtstockwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtstockwt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtstockwt.Location = new System.Drawing.Point(620, 86);
		this.txtstockwt.Name = "txtstockwt";
		this.txtstockwt.ReadOnly = true;
		this.txtstockwt.Size = new System.Drawing.Size(90, 23);
		this.txtstockwt.TabIndex = 19;
		this.txtstockwt.TabStop = false;
		this.txtstockwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(6, 115);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(193, 16);
		this.label60.TabIndex = 43;
		this.label60.Text = "Selected Lab   ---------->";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(380, 19);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(57, 15);
		this.label22.TabIndex = 41;
		this.label22.Text = "Crop Year";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(209, 66);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(44, 15);
		this.label3.TabIndex = 43;
		this.label3.Text = "Wt Cut";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(620, 66);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(79, 15);
		this.label29.TabIndex = 25;
		this.label29.Text = "Stock Weight";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(178, 19);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(67, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Item Name";
		this.txtEmptyBagsTotal.BackColor = System.Drawing.Color.White;
		this.txtEmptyBagsTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptyBagsTotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEmptyBagsTotal.Location = new System.Drawing.Point(3, 86);
		this.txtEmptyBagsTotal.Name = "txtEmptyBagsTotal";
		this.txtEmptyBagsTotal.Size = new System.Drawing.Size(68, 23);
		this.txtEmptyBagsTotal.TabIndex = 10;
		this.txtEmptyBagsTotal.Tag = "EbTotal";
		this.txtEmptyBagsTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEmptyBagsTotal.TextChanged += new System.EventHandler(txtEmptyBagsTotal_TextChanged);
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance138).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(377, 36);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 3;
		this.txtnetwt.BackColor = System.Drawing.Color.White;
		this.txtnetwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtnetwt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtnetwt.Location = new System.Drawing.Point(402, 86);
		this.txtnetwt.Name = "txtnetwt";
		this.txtnetwt.ReadOnly = true;
		this.txtnetwt.Size = new System.Drawing.Size(90, 23);
		this.txtnetwt.TabIndex = 16;
		this.txtnetwt.TabStop = false;
		this.txtnetwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(642, 19);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(77, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Packing Type";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(481, 19);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(48, 15);
		this.label27.TabIndex = 41;
		this.label27.Text = "Job/Lot";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(403, 66);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(65, 15);
		this.label46.TabIndex = 25;
		this.label46.Text = "Bill Weight";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(863, 19);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(54, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Item Qty";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(3, 19);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(67, 15);
		this.label30.TabIndex = 41;
		this.label30.Text = "Warehouse";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance145;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance151).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(782, 36);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(combpckuom_Leave);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(782, 66);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(56, 15);
		this.label28.TabIndex = 25;
		this.label28.Text = "City/Area";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(336, 66);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(62, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "Add / Less";
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(782, 84);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(248, 26);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 21;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(782, 19);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(35, 15);
		this.label42.TabIndex = 21;
		this.label42.Text = "UOM";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(3, 66);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(52, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "E.b Total";
		this.CmbWareHouseName.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouseName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Location = new System.Drawing.Point(3, 36);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Name = "CmbWareHouseName";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseName).TabIndex = 1;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(479, 36);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 4;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(939, 19);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(78, 15);
		this.label43.TabIndex = 23;
		this.label43.Text = "Gross Weight";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(1101, 18);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(56, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "E.b / Unit";
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(641, 36);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Leave += new System.EventHandler(combpcktyp_Leave);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1000, 96);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(22, 15);
		this.label16.TabIndex = 45;
		this.label16.Text = "Eb";
		this.label16.Visible = false;
		this.txtEbWtOther.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbWtOther.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEbWtOther.Location = new System.Drawing.Point(1028, 94);
		this.txtEbWtOther.Name = "txtEbWtOther";
		this.txtEbWtOther.Size = new System.Drawing.Size(57, 21);
		this.txtEbWtOther.TabIndex = 8;
		this.txtEbWtOther.TabStop = false;
		this.txtEbWtOther.Visible = false;
		this.txtEbWtOther.TextChanged += new System.EventHandler(txtEbWtOther_TextChanged);
		this.panel4.Controls.Add(this.PanelBreakupMain);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 58);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1372, 201);
		this.panel4.TabIndex = 0;
		this.PanelBreakupMain.Controls.Add((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup);
		this.PanelBreakupMain.Controls.Add(this.panel22);
		this.PanelBreakupMain.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelBreakupMain.Location = new System.Drawing.Point(1030, 0);
		this.PanelBreakupMain.Name = "PanelBreakupMain";
		this.PanelBreakupMain.Size = new System.Drawing.Size(692, 201);
		this.PanelBreakupMain.TabIndex = 20;
		((ScrollableBase)this.grdPurchaseBrakup).BackColor = System.Drawing.Color.White;
		grdPurchaseBrakup_DesignTimeLayout.LayoutString = resources.GetString("grdPurchaseBrakup_DesignTimeLayout.LayoutString");
		this.grdPurchaseBrakup.DesignTimeLayout = grdPurchaseBrakup_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPurchaseBrakup.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).Name = "grdPurchaseBrakup";
		((ScrollableBase)this.grdPurchaseBrakup).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).Size = new System.Drawing.Size(692, 175);
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).TabIndex = 0;
		this.grdPurchaseBrakup.TotalRow = (InheritableBoolean)1;
		this.grdPurchaseBrakup.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPurchaseBrakup).VisualStyle = (VisualStyle)4;
		this.grdPurchaseBrakup.CellUpdated += new ColumnActionEventHandler(grdPurchaseBrakup_CellUpdated);
		this.grdPurchaseBrakup.ColumnButtonClick += new ColumnActionEventHandler(grdPurchaseBrakup_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPurchaseBrakup_KeyDown);
		this.panel22.BackColor = System.Drawing.Color.White;
		this.panel22.Controls.Add(this.label38);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(692, 26);
		this.panel22.TabIndex = 3;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label38.ForeColor = System.Drawing.Color.Teal;
		this.label38.Location = new System.Drawing.Point(3, 5);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(311, 14);
		this.label38.TabIndex = 0;
		this.label38.Text = "Market Purchase Empty Bags Weight BreakUp";
		this.groupBox1.Controls.Add(this.groupBox4);
		this.groupBox1.Controls.Add(this.txtDifferenceWeight);
		this.groupBox1.Controls.Add(this.txtGrnWeight);
		this.groupBox1.Controls.Add(this.txtTicketnos);
		this.groupBox1.Controls.Add(this.txtfctwt);
		this.groupBox1.Controls.Add(this.txtBalWeight);
		this.groupBox1.Controls.Add(this.txtsuppwt);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo);
		this.groupBox1.Controls.Add(this.txtReceivedWeight);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.lblDiffweight);
		this.groupBox1.Controls.Add(this.txtAccessWeight);
		this.groupBox1.Controls.Add(this.labelExcessWt);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.lblBalwt);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtAnaylstName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combvehtyp);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combsupplier);
		this.groupBox1.Controls.Add(this.combgatepass);
		this.groupBox1.Controls.Add(this.lblRecwt);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label61);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtbltyno);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.txtvehno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.txtDeliverTerm);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1030, 201);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.groupBox4.Controls.Add(this.txtAdvanceByFactory);
		this.groupBox4.Controls.Add(this.txtcarramount);
		this.groupBox4.Controls.Add(this.txtScaleCharges);
		this.groupBox4.Controls.Add(this.txtChargeToPartyAmountFr);
		this.groupBox4.Controls.Add(this.txtShortageAmountFr);
		this.groupBox4.Controls.Add(this.txtOtherDeduction);
		this.groupBox4.Controls.Add(this.txtTotalFreight);
		this.groupBox4.Controls.Add(this.txtFreightDeduction);
		this.groupBox4.Controls.Add(this.label64);
		this.groupBox4.Controls.Add(this.txtAdvanceByParty);
		this.groupBox4.Controls.Add(this.label26);
		this.groupBox4.Controls.Add(this.label69);
		this.groupBox4.Controls.Add(this.label68);
		this.groupBox4.Controls.Add(this.txtRemainingAmountFr);
		this.groupBox4.Controls.Add(this.txtDiscountAmountFr);
		this.groupBox4.Controls.Add(this.label66);
		this.groupBox4.Controls.Add(this.txtBiltyFreight);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransport);
		this.groupBox4.Controls.Add(this.label1);
		this.groupBox4.Controls.Add(this.label48);
		this.groupBox4.Controls.Add(this.label65);
		this.groupBox4.Controls.Add(this.label63);
		this.groupBox4.Controls.Add(this.label31);
		this.groupBox4.Controls.Add(this.label70);
		this.groupBox4.Controls.Add(this.label67);
		this.groupBox4.Controls.Add(this.label17);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox4.Location = new System.Drawing.Point(612, 9);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(417, 188);
		this.groupBox4.TabIndex = 18;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Freight Information";
		this.txtAdvanceByFactory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAdvanceByFactory.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAdvanceByFactory.Location = new System.Drawing.Point(315, 64);
		this.txtAdvanceByFactory.Name = "txtAdvanceByFactory";
		this.txtAdvanceByFactory.Size = new System.Drawing.Size(100, 23);
		this.txtAdvanceByFactory.TabIndex = 4;
		this.txtAdvanceByFactory.TextChanged += new System.EventHandler(txtAdvanceByFactory_TextChanged);
		this.txtAdvanceByFactory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcarramount_KeyPress);
		this.txtcarramount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtcarramount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcarramount.Enabled = false;
		this.txtcarramount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcarramount.Location = new System.Drawing.Point(315, 162);
		this.txtcarramount.Name = "txtcarramount";
		this.txtcarramount.Size = new System.Drawing.Size(100, 23);
		this.txtcarramount.TabIndex = 11;
		this.txtcarramount.TextChanged += new System.EventHandler(txtcarramount_Leave);
		this.txtcarramount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcarramount_KeyPress);
		this.txtcarramount.Leave += new System.EventHandler(txtcarramount_Leave);
		this.txtScaleCharges.BackColor = System.Drawing.SystemColors.Menu;
		this.txtScaleCharges.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtScaleCharges.Enabled = false;
		this.txtScaleCharges.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtScaleCharges.Location = new System.Drawing.Point(315, 40);
		this.txtScaleCharges.Name = "txtScaleCharges";
		this.txtScaleCharges.Size = new System.Drawing.Size(100, 23);
		this.txtScaleCharges.TabIndex = 2;
		this.txtChargeToPartyAmountFr.BackColor = System.Drawing.SystemColors.Menu;
		this.txtChargeToPartyAmountFr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtChargeToPartyAmountFr.Enabled = false;
		this.txtChargeToPartyAmountFr.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtChargeToPartyAmountFr.Location = new System.Drawing.Point(315, 137);
		this.txtChargeToPartyAmountFr.Name = "txtChargeToPartyAmountFr";
		this.txtChargeToPartyAmountFr.Size = new System.Drawing.Size(100, 23);
		this.txtChargeToPartyAmountFr.TabIndex = 9;
		this.txtShortageAmountFr.BackColor = System.Drawing.SystemColors.Menu;
		this.txtShortageAmountFr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtShortageAmountFr.Enabled = false;
		this.txtShortageAmountFr.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtShortageAmountFr.Location = new System.Drawing.Point(106, 112);
		this.txtShortageAmountFr.Name = "txtShortageAmountFr";
		this.txtShortageAmountFr.Size = new System.Drawing.Size(100, 23);
		this.txtShortageAmountFr.TabIndex = 6;
		this.txtOtherDeduction.BackColor = System.Drawing.SystemColors.Menu;
		this.txtOtherDeduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherDeduction.Enabled = false;
		this.txtOtherDeduction.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherDeduction.Location = new System.Drawing.Point(106, 88);
		this.txtOtherDeduction.Name = "txtOtherDeduction";
		this.txtOtherDeduction.Size = new System.Drawing.Size(100, 23);
		this.txtOtherDeduction.TabIndex = 5;
		this.txtTotalFreight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTotalFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalFreight.Enabled = false;
		this.txtTotalFreight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalFreight.Location = new System.Drawing.Point(315, 88);
		this.txtTotalFreight.Name = "txtTotalFreight";
		this.txtTotalFreight.Size = new System.Drawing.Size(100, 23);
		this.txtTotalFreight.TabIndex = 5;
		this.txtFreightDeduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightDeduction.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFreightDeduction.Location = new System.Drawing.Point(106, 162);
		this.txtFreightDeduction.Name = "txtFreightDeduction";
		this.txtFreightDeduction.Size = new System.Drawing.Size(100, 23);
		this.txtFreightDeduction.TabIndex = 10;
		this.txtFreightDeduction.TextChanged += new System.EventHandler(txtFreightDeduction_TextChanged);
		this.txtFreightDeduction.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcarramount_KeyPress);
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(208, 68);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(111, 15);
		this.label64.TabIndex = 23;
		this.label64.Text = "Advance by Factory";
		this.txtAdvanceByParty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAdvanceByParty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAdvanceByParty.Location = new System.Drawing.Point(106, 64);
		this.txtAdvanceByParty.Name = "txtAdvanceByParty";
		this.txtAdvanceByParty.Size = new System.Drawing.Size(100, 23);
		this.txtAdvanceByParty.TabIndex = 3;
		this.txtAdvanceByParty.TextChanged += new System.EventHandler(txtAdvanceByParty_TextChanged);
		this.txtAdvanceByParty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcarramount_KeyPress);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(208, 44);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(80, 15);
		this.label26.TabIndex = 3;
		this.label26.Text = "Scale Charges";
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.Location = new System.Drawing.Point(208, 168);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(77, 15);
		this.label69.TabIndex = 3;
		this.label69.Text = "Paid Amount";
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.Location = new System.Drawing.Point(208, 141);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(90, 15);
		this.label68.TabIndex = 3;
		this.label68.Text = "Charge To Party";
		this.txtRemainingAmountFr.BackColor = System.Drawing.SystemColors.Menu;
		this.txtRemainingAmountFr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemainingAmountFr.Enabled = false;
		this.txtRemainingAmountFr.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemainingAmountFr.Location = new System.Drawing.Point(106, 137);
		this.txtRemainingAmountFr.Name = "txtRemainingAmountFr";
		this.txtRemainingAmountFr.Size = new System.Drawing.Size(100, 23);
		this.txtRemainingAmountFr.TabIndex = 8;
		this.txtDiscountAmountFr.BackColor = System.Drawing.SystemColors.Menu;
		this.txtDiscountAmountFr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountAmountFr.Enabled = false;
		this.txtDiscountAmountFr.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDiscountAmountFr.Location = new System.Drawing.Point(315, 112);
		this.txtDiscountAmountFr.Name = "txtDiscountAmountFr";
		this.txtDiscountAmountFr.Size = new System.Drawing.Size(100, 23);
		this.txtDiscountAmountFr.TabIndex = 7;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.Location = new System.Drawing.Point(3, 116);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(102, 15);
		this.label66.TabIndex = 3;
		this.label66.Text = "Shortage Amount";
		this.txtBiltyFreight.BackColor = System.Drawing.Color.White;
		this.txtBiltyFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyFreight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBiltyFreight.Location = new System.Drawing.Point(106, 40);
		this.txtBiltyFreight.Name = "txtBiltyFreight";
		this.txtBiltyFreight.Size = new System.Drawing.Size(100, 23);
		this.txtBiltyFreight.TabIndex = 1;
		this.txtBiltyFreight.TextChanged += new System.EventHandler(txtBiltyFreight_TextChanged);
		this.txtBiltyFreight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcarramount_KeyPress);
		this.CmbTransport.AllowNull = (DefaultableBoolean)1;
		this.CmbTransport.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbTransport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance115;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransport.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Location = new System.Drawing.Point(106, 13);
		((System.Windows.Forms.Control)(object)this.CmbTransport).Name = "CmbTransport";
		((System.Windows.Forms.Control)(object)this.CmbTransport).Size = new System.Drawing.Size(309, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransport).TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 92);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(96, 15);
		this.label1.TabIndex = 23;
		this.label1.Text = "Other Deduction";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(3, 19);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(67, 15);
		this.label48.TabIndex = 8;
		this.label48.Text = "Transporter";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(208, 92);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(73, 15);
		this.label65.TabIndex = 23;
		this.label65.Text = "Total Freight";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(3, 68);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(99, 15);
		this.label63.TabIndex = 23;
		this.label63.Text = "Advance by Party";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(3, 165);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(103, 15);
		this.label31.TabIndex = 3;
		this.label31.Text = "Freight Deduction";
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(3, 141);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(104, 15);
		this.label70.TabIndex = 3;
		this.label70.Text = "Remaining Freight";
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(208, 116);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(102, 15);
		this.label67.TabIndex = 3;
		this.label67.Text = "Discount Amount";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(3, 44);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(70, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Bilty Freight";
		this.txtDifferenceWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtDifferenceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDifferenceWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDifferenceWeight.Location = new System.Drawing.Point(503, 90);
		this.txtDifferenceWeight.Name = "txtDifferenceWeight";
		this.txtDifferenceWeight.ReadOnly = true;
		this.txtDifferenceWeight.Size = new System.Drawing.Size(50, 23);
		this.txtDifferenceWeight.TabIndex = 14;
		this.txtDifferenceWeight.TabStop = false;
		this.txtGrnWeight.BackColor = System.Drawing.Color.White;
		this.txtGrnWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrnWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrnWeight.Location = new System.Drawing.Point(751, 246);
		this.txtGrnWeight.Name = "txtGrnWeight";
		this.txtGrnWeight.ReadOnly = true;
		this.txtGrnWeight.Size = new System.Drawing.Size(86, 21);
		this.txtGrnWeight.TabIndex = 19;
		this.txtGrnWeight.TabStop = false;
		this.txtGrnWeight.Visible = false;
		this.txtGrnWeight.WordWrap = false;
		this.txtTicketnos.Enabled = false;
		this.txtTicketnos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTicketnos.Location = new System.Drawing.Point(503, 164);
		this.txtTicketnos.Multiline = true;
		this.txtTicketnos.Name = "txtTicketnos";
		this.txtTicketnos.Size = new System.Drawing.Size(105, 34);
		this.txtTicketnos.TabIndex = 17;
		this.txtfctwt.BackColor = System.Drawing.SystemColors.Menu;
		this.txtfctwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfctwt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtfctwt.Location = new System.Drawing.Point(503, 65);
		this.txtfctwt.Name = "txtfctwt";
		this.txtfctwt.ReadOnly = true;
		this.txtfctwt.Size = new System.Drawing.Size(105, 23);
		this.txtfctwt.TabIndex = 13;
		this.txtfctwt.TabStop = false;
		this.txtfctwt.TextChanged += new System.EventHandler(txtfctwt_TextChanged);
		this.txtBalWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtBalWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBalWeight.Location = new System.Drawing.Point(503, 115);
		this.txtBalWeight.Name = "txtBalWeight";
		this.txtBalWeight.ReadOnly = true;
		this.txtBalWeight.Size = new System.Drawing.Size(105, 23);
		this.txtBalWeight.TabIndex = 15;
		this.txtBalWeight.TabStop = false;
		this.txtsuppwt.BackColor = System.Drawing.SystemColors.Menu;
		this.txtsuppwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsuppwt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtsuppwt.Location = new System.Drawing.Point(503, 40);
		this.txtsuppwt.Name = "txtsuppwt";
		this.txtsuppwt.Size = new System.Drawing.Size(105, 23);
		this.txtsuppwt.TabIndex = 12;
		this.txtsuppwt.TabStop = false;
		this.txtsuppwt.TextChanged += new System.EventHandler(txtsuppwt_TextChanged);
		this.CmbSupplierDispatchedPreBillNo.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierDispatchedPreBillNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierDispatchedPreBillNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierDispatchedPreBillNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance90).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierDispatchedPreBillNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo).Location = new System.Drawing.Point(88, 125);
		((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo).Name = "CmbSupplierDispatchedPreBillNo";
		((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo).Size = new System.Drawing.Size(117, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo).Leave += new System.EventHandler(CmbSupplierDispatchedPreBillNo_Leave);
		this.txtReceivedWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtReceivedWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReceivedWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtReceivedWeight.Location = new System.Drawing.Point(503, 139);
		this.txtReceivedWeight.Name = "txtReceivedWeight";
		this.txtReceivedWeight.ReadOnly = true;
		this.txtReceivedWeight.Size = new System.Drawing.Size(105, 23);
		this.txtReceivedWeight.TabIndex = 16;
		this.txtReceivedWeight.TabStop = false;
		this.txtremarks.Location = new System.Drawing.Point(88, 175);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(311, 23);
		this.txtremarks.TabIndex = 10;
		this.lblDiffweight.AutoSize = true;
		this.lblDiffweight.Location = new System.Drawing.Point(557, 93);
		this.lblDiffweight.Name = "lblDiffweight";
		this.lblDiffweight.Size = new System.Drawing.Size(53, 16);
		this.lblDiffweight.TabIndex = 22;
		this.lblDiffweight.Text = "label49";
		this.txtAccessWeight.Enabled = false;
		this.txtAccessWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAccessWeight.Location = new System.Drawing.Point(289, 127);
		this.txtAccessWeight.Name = "txtAccessWeight";
		this.txtAccessWeight.Size = new System.Drawing.Size(110, 23);
		this.txtAccessWeight.TabIndex = 8;
		this.labelExcessWt.AutoSize = true;
		this.labelExcessWt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelExcessWt.Location = new System.Drawing.Point(210, 131);
		this.labelExcessWt.Name = "labelExcessWt";
		this.labelExcessWt.Size = new System.Drawing.Size(80, 15);
		this.labelExcessWt.TabIndex = 0;
		this.labelExcessWt.Text = "P.O Excess Wt";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(403, 167);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(90, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "W.B Ticket Nos.";
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(4, 177);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(62, 19);
		this.label24.TabIndex = 0;
		this.label24.Text = "Remarks";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(403, 94);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(104, 15);
		this.label39.TabIndex = 21;
		this.label39.Text = "Difference Weight";
		this.lblBalwt.AutoSize = true;
		this.lblBalwt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalwt.Location = new System.Drawing.Point(403, 119);
		this.lblBalwt.Name = "lblBalwt";
		this.lblBalwt.Size = new System.Drawing.Size(73, 15);
		this.lblBalwt.TabIndex = 21;
		this.lblBalwt.Text = "GRN Weight";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(88, 16);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(110, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(4, 20);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(285, 16);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(114, 22);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.txtAnaylstName.BackColor = System.Drawing.SystemColors.Menu;
		this.txtAnaylstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAnaylstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAnaylstName.Location = new System.Drawing.Point(88, 152);
		this.txtAnaylstName.Name = "txtAnaylstName";
		this.txtAnaylstName.ReadOnly = true;
		this.txtAnaylstName.Size = new System.Drawing.Size(311, 21);
		this.txtAnaylstName.TabIndex = 9;
		this.txtAnaylstName.TabStop = false;
		this.combvehtyp.AllowNull = (DefaultableBoolean)1;
		this.combvehtyp.AutoCompleteMode = (AutoCompleteMode)4;
		this.combvehtyp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combvehtyp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combvehtyp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance94).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combvehtyp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Location = new System.Drawing.Point(88, 97);
		((System.Windows.Forms.Control)(object)this.combvehtyp).Name = "combvehtyp";
		((System.Windows.Forms.Control)(object)this.combvehtyp).Size = new System.Drawing.Size(117, 26);
		((System.Windows.Forms.Control)(object)this.combvehtyp).TabIndex = 5;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(210, 75);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(49, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Bilty No";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(4, 155);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(78, 15);
		this.label10.TabIndex = 8;
		this.label10.Text = "AnalystName";
		this.combsupplier.AllowNull = (DefaultableBoolean)1;
		this.combsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.combsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.combsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsupplier).Location = new System.Drawing.Point(88, 41);
		((System.Windows.Forms.Control)(object)this.combsupplier).Name = "combsupplier";
		((System.Windows.Forms.Control)(object)this.combsupplier).Size = new System.Drawing.Size(311, 26);
		((System.Windows.Forms.Control)(object)this.combsupplier).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.combsupplier).Leave += new System.EventHandler(combsupplier_Leave);
		this.combgatepass.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.combgatepass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.combgatepass.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combgatepass.Location = new System.Drawing.Point(88, 71);
		this.combgatepass.Name = "combgatepass";
		this.combgatepass.ReadOnly = true;
		this.combgatepass.Size = new System.Drawing.Size(117, 23);
		this.combgatepass.TabIndex = 3;
		this.combgatepass.TabStop = false;
		this.combgatepass.TextChanged += new System.EventHandler(combgatepass_Leave);
		this.lblRecwt.AutoSize = true;
		this.lblRecwt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRecwt.Location = new System.Drawing.Point(403, 143);
		this.lblRecwt.Name = "lblRecwt";
		this.lblRecwt.Size = new System.Drawing.Size(96, 15);
		this.lblRecwt.TabIndex = 0;
		this.lblRecwt.Text = "Received Weight";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(403, 69);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(88, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Factory Weight";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(204, 20);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(57, 15);
		this.label61.TabIndex = 0;
		this.label61.Text = "Doc Date";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 75);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(76, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Gate Pass No";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(403, 44);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(93, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Supplier Weight";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(4, 47);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Supplier Name";
		this.txtbltyno.BackColor = System.Drawing.SystemColors.Menu;
		this.txtbltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbltyno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbltyno.Location = new System.Drawing.Point(289, 71);
		this.txtbltyno.Name = "txtbltyno";
		this.txtbltyno.Size = new System.Drawing.Size(110, 23);
		this.txtbltyno.TabIndex = 4;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(210, 103);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(64, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Vehicle No";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(4, 103);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(73, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Vehicle Type";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(4, 131);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(83, 15);
		this.label8.TabIndex = 8;
		this.label8.Text = "Transit Vehicle";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(403, 19);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(80, 15);
		this.label15.TabIndex = 0;
		this.label15.Text = "Delivery Term";
		this.txtvehno.BackColor = System.Drawing.Color.White;
		this.txtvehno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvehno.Location = new System.Drawing.Point(289, 99);
		this.txtvehno.Name = "txtvehno";
		this.txtvehno.Size = new System.Drawing.Size(110, 23);
		this.txtvehno.TabIndex = 6;
		this.txtDeliverTerm.AllowNull = (DefaultableBoolean)1;
		this.txtDeliverTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtDeliverTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtDeliverTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtDeliverTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.txtDeliverTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtDeliverTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtDeliverTerm).Enabled = false;
		((System.Windows.Forms.Control)(object)this.txtDeliverTerm).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.txtDeliverTerm).Location = new System.Drawing.Point(503, 13);
		((System.Windows.Forms.Control)(object)this.txtDeliverTerm).Name = "txtDeliverTerm";
		((System.Windows.Forms.Control)(object)this.txtDeliverTerm).Size = new System.Drawing.Size(105, 26);
		((System.Windows.Forms.Control)(object)this.txtDeliverTerm).TabIndex = 11;
		this.txtDeliverTerm.ValueChanged += new System.EventHandler(txtDeliverTerm_ValueChanged);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.chkSuppShortWeight);
		this.panel2.Controls.Add(this.chkScaleShortWeightApply);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.ChkBox);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1372, 56);
		this.panel2.TabIndex = 0;
		this.chkSuppShortWeight.AutoSize = true;
		this.chkSuppShortWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.chkSuppShortWeight.ForeColor = System.Drawing.Color.White;
		this.chkSuppShortWeight.Location = new System.Drawing.Point(698, 31);
		this.chkSuppShortWeight.Name = "chkSuppShortWeight";
		this.chkSuppShortWeight.Size = new System.Drawing.Size(305, 20);
		this.chkSuppShortWeight.TabIndex = 20;
		this.chkSuppShortWeight.Text = "Supplier Short Weight Deduction Apply";
		this.chkSuppShortWeight.UseVisualStyleBackColor = true;
		this.chkSuppShortWeight.CheckedChanged += new System.EventHandler(chkSuppShortWeight_CheckedChanged);
		this.chkScaleShortWeightApply.AutoSize = true;
		this.chkScaleShortWeightApply.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.chkScaleShortWeightApply.ForeColor = System.Drawing.Color.White;
		this.chkScaleShortWeightApply.Location = new System.Drawing.Point(394, 32);
		this.chkScaleShortWeightApply.Name = "chkScaleShortWeightApply";
		this.chkScaleShortWeightApply.Size = new System.Drawing.Size(285, 20);
		this.chkScaleShortWeightApply.TabIndex = 19;
		this.chkScaleShortWeightApply.Text = "Scale Short Weight Deduction Apply";
		this.chkScaleShortWeightApply.UseVisualStyleBackColor = true;
		this.chkScaleShortWeightApply.CheckedChanged += new System.EventHandler(chkScaleShortWeightApply_CheckedChanged);
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.btnNew, this.BtnRefresh, this.btnSave, this.btnUpdate, this.BtnPrint, this.btnSlip257, this.btnAttachment, this.btnDelete, this.toolStripSeparator1, this.CityDefine,
			this.BtnShortCutkeys, this.toolStripSeparator2, this.BtnUpdateEBTotalWithGrossWeight
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1372, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
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
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.ToolTipText = "Ctrl_S";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.ToolTipText = "Ctrl_U";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnPrint.Text = "211-Print";
		this.BtnPrint.ToolTipText = "Ctrl_P";
		this.BtnPrint.Click += new System.EventHandler(printToolStripButton_Click);
		this.btnSlip257.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnSlip257.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip257.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip257.Name = "btnSlip257";
		this.btnSlip257.Size = new System.Drawing.Size(102, 24);
		this.btnSlip257.Text = "257-GPSlip";
		this.btnSlip257.Click += new System.EventHandler(btnSlip257_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.CityDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.CityDefine.Name = "CityDefine";
		this.CityDefine.Size = new System.Drawing.Size(83, 24);
		this.CityDefine.Text = "Define City";
		this.CityDefine.Click += new System.EventHandler(CityDefine_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.BtnUpdateEBTotalWithGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnUpdateEBTotalWithGrossWeight.Image = Architecture.WinApp.Properties.Resources.images;
		this.BtnUpdateEBTotalWithGrossWeight.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnUpdateEBTotalWithGrossWeight.Name = "BtnUpdateEBTotalWithGrossWeight";
		this.BtnUpdateEBTotalWithGrossWeight.Size = new System.Drawing.Size(258, 24);
		this.BtnUpdateEBTotalWithGrossWeight.Text = "E.B Wt According To Gross Weight";
		this.BtnUpdateEBTotalWithGrossWeight.ToolTipText = resources.GetString("BtnUpdateEBTotalWithGrossWeight.ToolTipText");
		this.BtnUpdateEBTotalWithGrossWeight.Click += new System.EventHandler(BtnUpdateEBTotalWithGrossWeight_Click);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(3, 31);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(260, 19);
		this.label13.TabIndex = 3;
		this.label13.Text = "Goods Receiving Notes             ";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Checked = true;
		this.ChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1223, 31);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(145, 20);
		this.ChkBox.TabIndex = 18;
		this.ChkBox.Text = "Preview on save";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add(this.panel23);
		this.tabPage2.Controls.Add(this.tabControl2);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1376, 763);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.GrdHistory.AllowEdit = (InheritableBoolean)2;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistory.DynamicFiltering = true;
		this.GrdHistory.FilterMode = (FilterMode)2;
		this.GrdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(2, 151);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(1372, 308);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 44;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.SelectionChanged += new System.EventHandler(GrdHistory_SelectionChanged);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.GrdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdHistory_KeyDown);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label23);
		this.panel18.Controls.Add(this.ctrlGrdBar2);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(2, 126);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1372, 25);
		this.panel18.TabIndex = 45;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(142, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Filtered Records";
		this.panel23.Controls.Add(this.rdapproveddate);
		this.panel23.Controls.Add(this.rdmodifydate);
		this.panel23.Controls.Add(this.drdocdate);
		this.panel23.Controls.Add(this.rdentrydate);
		this.panel23.Controls.Add(this.groupBox3);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(2, 52);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1372, 74);
		this.panel23.TabIndex = 50;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(981, 45);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 13;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(981, 25);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 12;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(900, 25);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 10;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(900, 45);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 11;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.groupBox3.Controls.Add(this.RadNotReffered);
		this.groupBox3.Controls.Add(this.RadReffered);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label49);
		this.groupBox3.Controls.Add(this.RadAll);
		this.groupBox3.Controls.Add(this.label50);
		this.groupBox3.Controls.Add(this.label51);
		this.groupBox3.Controls.Add(this.label52);
		this.groupBox3.Controls.Add(this.label53);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(891, 66);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.RadNotReffered.AutoSize = true;
		this.RadNotReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadNotReffered.Location = new System.Drawing.Point(779, 43);
		this.RadNotReffered.Name = "RadNotReffered";
		this.RadNotReffered.Size = new System.Drawing.Size(108, 23);
		this.RadNotReffered.TabIndex = 8;
		this.RadNotReffered.Text = "Not Reffered";
		this.RadNotReffered.UseVisualStyleBackColor = true;
		this.RadReffered.AutoSize = true;
		this.RadReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadReffered.Location = new System.Drawing.Point(779, 25);
		this.RadReffered.Name = "RadReffered";
		this.RadReffered.Size = new System.Drawing.Size(80, 23);
		this.RadReffered.TabIndex = 7;
		this.RadReffered.Text = "Reffered";
		this.RadReffered.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(721, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 26);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label49.Location = new System.Drawing.Point(479, 19);
		this.label49.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(86, 15);
		this.label49.TabIndex = 142;
		this.label49.Text = "Supplier Name";
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(779, 7);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(44, 23);
		this.RadAll.TabIndex = 6;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label50.Location = new System.Drawing.Point(378, 19);
		this.label50.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(64, 15);
		this.label50.TabIndex = 141;
		this.label50.Text = "To Doc No";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(276, 19);
		this.label51.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(79, 15);
		this.label51.TabIndex = 140;
		this.label51.Text = "From Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(143, 19);
		this.label52.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(48, 15);
		this.label52.TabIndex = 139;
		this.label52.Text = "To Date";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label53.Location = new System.Drawing.Point(5, 20);
		this.label53.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(63, 15);
		this.label53.TabIndex = 13;
		this.label53.Text = "From Date";
		this.cmbSupplierNameHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Location = new System.Drawing.Point(479, 36);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Name = "cmbSupplierNameHistory";
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).TabIndex = 4;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(378, 38);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(276, 38);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(143, 38);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(131, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 38);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistory.TabIndex = 0;
		this.tabControl2.Controls.Add(this.tabDetailH);
		this.tabControl2.Controls.Add(this.tabEBH);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl2.Location = new System.Drawing.Point(2, 459);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1372, 302);
		this.tabControl2.TabIndex = 3;
		this.tabDetailH.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.tabDetailH.Controls.Add(this.panel20);
		this.tabDetailH.Location = new System.Drawing.Point(4, 25);
		this.tabDetailH.Name = "tabDetailH";
		this.tabDetailH.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetailH.Size = new System.Drawing.Size(1364, 273);
		this.tabDetailH.TabIndex = 0;
		this.tabDetailH.Text = "Detail";
		this.tabDetailH.UseVisualStyleBackColor = true;
		this.GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(1358, 246);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 45;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.ctrlGrdBar4);
		this.panel20.Controls.Add(this.label32);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 3);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1358, 21);
		this.panel20.TabIndex = 46;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(3, 1);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(251, 18);
		this.label32.TabIndex = 24;
		this.label32.Text = "Detail of the Selected above row";
		this.tabEBH.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryEB);
		this.tabEBH.Controls.Add(this.panel11);
		this.tabEBH.Location = new System.Drawing.Point(4, 25);
		this.tabEBH.Name = "tabEBH";
		this.tabEBH.Padding = new System.Windows.Forms.Padding(3);
		this.tabEBH.Size = new System.Drawing.Size(1364, 273);
		this.tabEBH.TabIndex = 1;
		this.tabEBH.Text = "Empty Bags Detail";
		this.tabEBH.UseVisualStyleBackColor = true;
		this.grdHistoryEB.AllowEdit = (InheritableBoolean)2;
		grdHistoryEB_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryEB.DesignTimeLayout = grdHistoryEB_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryEB).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryEB.DynamicFiltering = true;
		this.grdHistoryEB.FilterMode = (FilterMode)2;
		this.grdHistoryEB.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryEB).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryEB.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryEB).Location = new System.Drawing.Point(3, 23);
		((System.Windows.Forms.Control)(object)this.grdHistoryEB).Name = "grdHistoryEB";
		((ScrollableBase)this.grdHistoryEB).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryEB).Size = new System.Drawing.Size(1358, 247);
		((System.Windows.Forms.Control)(object)this.grdHistoryEB).TabIndex = 47;
		this.grdHistoryEB.TotalRow = (InheritableBoolean)1;
		this.grdHistoryEB.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryEB).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label54);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1358, 20);
		this.panel11.TabIndex = 48;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(5, 0);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(340, 18);
		this.label54.TabIndex = 24;
		this.label54.Text = "Empty bags Detail of the Selected above row";
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label55);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1372, 50);
		this.panel7.TabIndex = 0;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(0, 27);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(263, 19);
		this.label55.TabIndex = 24;
		this.label55.Text = "Goods Receiving Notes  History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnGrnFormHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1372, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnGrnFormHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGrnFormHistory.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnGrnFormHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGrnFormHistory.Name = "btnGrnFormHistory";
		this.btnGrnFormHistory.Size = new System.Drawing.Size(99, 24);
		this.btnGrnFormHistory.Text = "GrnHistory";
		this.btnGrnFormHistory.ToolTipText = "Crtl_H";
		this.btnGrnFormHistory.Click += new System.EventHandler(btnGrnFormHistory_Click);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(770, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdGp;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(39, 27);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(516, 4);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar5.MyGrid = this.grdEmptyBags;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(39, 27);
		this.ctrlGrdBar5.TabIndex = 24;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1329, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(39, 23);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1332, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1318, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.GrdHistoryDetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 24;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(1030, 18);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(57, 15);
		this.label37.TabIndex = 25;
		this.label37.Text = "Avg Uom";
		this.textBox1.BackColor = System.Drawing.SystemColors.Control;
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox1.Enabled = false;
		this.textBox1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.textBox1.Location = new System.Drawing.Point(1030, 38);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(70, 23);
		this.textBox1.TabIndex = 19;
		this.textBox1.TabStop = false;
		this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1384, 792);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "InvFrmGRN";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvFrmGRN_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvFrmGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGp).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.PanelCurrentLabResult.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdLabInfo).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLabNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		this.panel4.ResumeLayout(false);
		this.PanelBreakupMain.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseBrakup).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierDispatchedPreBillNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtDeliverTerm).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).EndInit();
		this.tabControl2.ResumeLayout(false);
		this.tabDetailH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.tabEBH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryEB).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
