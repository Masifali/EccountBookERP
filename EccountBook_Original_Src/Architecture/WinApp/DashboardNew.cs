using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.ContractorWages;
using Architecture.BLL.DAW;
using Architecture.BLL.Themes_ERP;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.DAW;
using Architecture.Model.Themes_ERP;
using Architecture.Model.VPS;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Audit_Dashboard;
using Architecture.WinApp.Common;
using Architecture.WinApp.Configurations;
using Architecture.WinApp.Dashboard;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.PreBookingAndDelivery;
using Architecture.WinApp.Properties;
using Architecture.WinApp.SQL;
using Architecture.WinApp.Themes_ERP;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace Architecture.WinApp;

public class DashboardNew : Form
{
	private bool isDownloading;

	private bool closingForUpdate;

	private WaitForm WaitForm = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	public static bool FavouritScreens_Flag;

	private ServerCredentials Sc = new ServerCredentials();

	private bool IsVpsAttachmentsServiceOn;

	private System.Windows.Forms.Timer versionTimer = new System.Windows.Forms.Timer();

	private CancellationTokenSource _cts = new CancellationTokenSource();

	private IContainer components;

	private Panel pnlmain;

	internal Panel panel1;

	internal Button btnSettings;

	internal Button btnReport;

	internal Button btnMenuButton;

	internal ContextMenuStrip ContextMenuStrip1;

	internal ToolStripMenuItem btnAccountDefinition;

	private ToolStripMenuItem AcfrmDefCoa;

	private ToolStripMenuItem AcfrmAcAllocation;

	private ToolStripMenuItem AcfrmOpeningBalance;

	private ToolStripMenuItem AcfrmDefineBank;

	private ToolStripMenuItem btnAccountsTransaction;

	private ToolStripMenuItem PaymentVoucherNew;

	private ToolStripMenuItem ReceiptsVoucherNew;

	private ToolStripMenuItem AcfrmDefPdcManagment;

	private ToolStripMenuItem btnInvDefinition;

	private ToolStripMenuItem InvDefrmAddItem;

	private ToolStripMenuItem InvfrmPriceSchedule;

	private ToolStripMenuItem AcfrmDefineLots;

	private ToolStripMenuItem btnSupPurchase;

	private ToolStripMenuItem supfrmDefineSupplier;

	private ToolStripMenuItem PurchsaeOrder;

	private ToolStripMenuItem InvFrmGRN;

	private ToolStripMenuItem InvfrmPurchasedirectInvoice;

	private ToolStripMenuItem InvfrmPurchaseInvoice;

	private ToolStripMenuItem InvfrmPurchaseReturn;

	private ToolStripMenuItem InwardGatePass;

	private ToolStripMenuItem btnCustomerSale;

	private ToolStripMenuItem SaleOrder;

	private ToolStripMenuItem InvfrmGDN;

	private ToolStripMenuItem InvfrmSaleInvoice;

	private ToolStripMenuItem InvfrmInvSaleInvoiceDirect;

	private ToolStripMenuItem InvfrmSaleInvoiceReturn;

	private ToolStripMenuItem OutwardGatePass;

	private ToolStripMenuItem btnStockWare;

	private ToolStripMenuItem invfrmStockConversionProduction;

	private ToolStripMenuItem btnLabAnalysis;

	private ToolStripMenuItem InvLabSampleLogRegister;

	private ToolStripMenuItem InvLabAnalysisItems;

	private ToolStripMenuItem InvLabAnalysisGroup;

	private ToolStripMenuItem InvLabGroupAnalysisStandards;

	private ToolStripMenuItem InvLabSampleAnalysis;

	private ToolStripMenuItem InvLabPurchaseAnalysis;

	internal ContextMenuStrip contextMenuStrip2;

	private ToolStripMenuItem btnAcReport;

	private ToolStripMenuItem ChartOfAccount;

	private ToolStripMenuItem VoucherReport;

	private ToolStripMenuItem VoucherValidation;

	private ToolStripMenuItem ActicitySummery;

	private ToolStripMenuItem GeneralLedger;

	private ToolStripMenuItem GeneralLedgerStatment;

	private ToolStripMenuItem ReceivableAging;

	private ToolStripMenuItem TrialBalance;

	private ToolStripMenuItem Receivables;

	private ToolStripMenuItem InventoryReport;

	private ToolStripMenuItem PurchaseOrderHistory;

	private ToolStripMenuItem frmGRNHistory;

	private ToolStripMenuItem frmPurchaseInvoiceHistory;

	private ToolStripMenuItem frmSaleOrderHistory;

	private ToolStripMenuItem frmGDNHistory;

	private ToolStripMenuItem frmGatePassReport;

	private ToolStripMenuItem frmGPOutward;

	internal ContextMenuStrip contextMenuStrip3;

	private ToolStripMenuItem btnadminpanel;

	private ToolStripMenuItem btnadminpanelsysconfig;

	private ToolStripMenuItem btnadminpaneluserright;

	private PictureBox btnRefresh;

	internal ContextMenuStrip contextMenuStrip4;

	private ToolStripMenuItem lockToolStripMenuItem;

	private ToolStripMenuItem btnwages;

	private ToolStripMenuItem btnExport;

	private ToolStripMenuItem frmContractorWagesAccount;

	private ToolStripMenuItem frmContractWagesSchedule;

	private ToolStripMenuItem frmwagesBillHeader;

	private ToolStripMenuItem frmMoveOrder;

	private ToolStripMenuItem SelectedTrialBalance;

	private ToolStripMenuItem frmWeightBridgeHistory;

	private ToolStripMenuItem frmStockContractorWagesHistory;

	private ToolStripMenuItem btnExportHistory;

	private ToolStripMenuItem btnLabReports;

	private ToolStripMenuItem InvLabPurchaseRegister;

	private ToolStripMenuItem storeManagmentToolStripMenuItem;

	private ToolStripMenuItem PurchsaeOrderTrade;

	private ToolStripMenuItem InwardGatePassTrade;

	private ToolStripMenuItem InvFrmGRNTrade;

	private ToolStripMenuItem fixedAssetsToolStripMenuItem;

	private ToolStripMenuItem frmFixedAssetsCategory;

	private ToolStripMenuItem frmFixedAssetRegister;

	private ToolStripMenuItem frmOpeningStockBlancing;

	private ToolStripMenuItem frmFixedAssetRegisterHistory;

	private ToolStripMenuItem changePasswordToolStripMenuItem;

	private ToolStripMenuItem CustomerLedger;

	private ToolStripMenuItem ItemLedger;

	private ToolStripMenuItem frmSaleInvoiceHistory;

	private ToolStripMenuItem UpdateCoAllocation;

	private System.Windows.Forms.Timer timer1;

	private ToolStripMenuItem EximForwarding;

	private ToolStripMenuItem ExpfrmEformRegistration;

	private ToolStripMenuItem EximInvoice;

	private ToolStripMenuItem btnCargoManagement;

	private ToolStripMenuItem CargoBooking;

	private ToolStripMenuItem ContainerAllocation;

	private ToolStripMenuItem FrmCBDHistory;

	private ToolStripMenuItem cbdBookingTracking;

	private ToolStripMenuItem DestinationWareHouseReceipt;

	private ToolStripMenuItem frmCargoDeliveryOrderAndBill;

	private ToolStripMenuItem frmProductionJobOrder;

	private ToolStripMenuItem frmFoodProduction;

	private ToolStripMenuItem frmBillOfMaterial;

	private ToolStripMenuItem frmSupplySchedule;

	private ToolStripMenuItem PaymentByInvoiceAccount;

	private ToolStripMenuItem ReceiptByInvoiceAccount;

	private ToolStripMenuItem frmDriverBio;

	private ToolStripMenuItem btnFinanceManagement;

	private ToolStripMenuItem frmFinanceType;

	private ToolStripMenuItem frmFMLoanRegistration;

	private ToolStripMenuItem frmFMLoanAgreement;

	private ToolStripMenuItem btnUtility;

	private ToolStripMenuItem PostDatedCheqPaymentVouchers;

	private ToolStripMenuItem frmClearingBill;

	private ToolStripMenuItem PointOfSaleInvoice;

	private ToolStripMenuItem petroliumToolStripMenuItem;

	private ToolStripMenuItem PurchaseInvoicePetrol;

	private ToolStripMenuItem SaleInvoicePetrol;

	private ToolStripMenuItem PurchaseInvoiceRegister;

	private ToolStripMenuItem SaleInvoiceRegister;

	private ToolStripMenuItem StockReport;

	private ToolStripMenuItem btnCargoReports;

	private ToolStripMenuItem CbdReceivablesandReceiptsHistory;

	private ToolStripMenuItem purchaseInvoiceToolStripMenuItem;

	private ToolStripMenuItem DateLock;

	private ToolStripMenuItem btnSMSContacts;

	private ToolStripMenuItem btnScreenMessageReceivers;

	private ToolStripMenuItem btnSMSConfiquration;

	private ToolStripMenuItem btnHistory;

	private ToolStripMenuItem defineInventoryToolStripMenuItem;

	private ToolStripMenuItem btnOtherItemsDefine;

	private ToolStripMenuItem btnPartyItemAllocation;

	private ToolStripMenuItem PurchaseInvoiceTradingWithTax;

	private ToolStripMenuItem btnSaleTrading;

	private ToolStripMenuItem SaleOrderTrade;

	private ToolStripMenuItem btnTradingReports;

	private ToolStripMenuItem PurchaseInvoiceTrading;

	private ToolStripMenuItem frmDeliverySchedule;

	private ToolStripMenuItem FrmDeliveryChallan;

	private ToolStripMenuItem saleInvoiceToolStripMenuItem;

	private ToolStripMenuItem SaleInvoiceTradingWithTax;

	private ToolStripMenuItem InvfrmSaleInvoiceTrading;

	private ToolStripMenuItem SaleInvoiceTrading;

	private ToolStripMenuItem frmBillOfMaterialManufacturing;

	private ToolStripMenuItem PurchaseInvoiceTradePro;

	private ToolStripMenuItem frmWhtTaxChallanDeposit;

	private ToolStripMenuItem frmBillsPayables;

	private ToolStripMenuItem frmBillsReceivables;

	private ToolStripMenuItem ContraVoucher;

	private ToolStripMenuItem AccountsReconcilation;

	private ToolStripMenuItem AcfrmChequebookRegistration;

	private ToolStripMenuItem AccountBudget;

	private ToolStripMenuItem FrmExportSalesContract;

	private ToolStripMenuItem btnCountry;

	private ToolStripMenuItem frmRptItemList;

	private ToolStripMenuItem FrmExportSalesContractSchedule;

	private ToolStripMenuItem btnScreenRights;

	private ToolStripMenuItem SaleOrderExport;

	private ToolStripMenuItem InvfrmSaleInvoiceExport;

	private ToolStripMenuItem Acfrmfcbankreceipt;

	private ToolStripMenuItem InvLabAnalysisInProcess;

	private ToolStripMenuItem InvLabSaleAnalysis;

	private ToolStripMenuItem EximPreProductionLab;

	private ToolStripMenuItem EximShippingBookingInfo;

	private ToolStripMenuItem EximBillOfLading;

	private ToolStripMenuItem frmItemsCustomizedGroups;

	private ToolStripMenuItem frmItemsReorderSchedule;

	private ToolStripMenuItem btnSupplierDefineSystemUtilities;

	private ToolStripMenuItem shipToAddressToolStripMenuItem;

	private ToolStripMenuItem supplierLimitsToolStripMenuItem;

	private ToolStripMenuItem supplierBankDetailToolStripMenuItem;

	private ToolStripMenuItem btnShipmentHistory;

	private ToolStripMenuItem frmBillofLadingSlipandRegister;

	private ToolStripMenuItem frmExImForwardingSlipandRegister;

	private ToolStripMenuItem frmExportShipingLineBookingRpt;

	private ToolStripMenuItem FrmRptExImShipmentScheduleHistroy;

	private ToolStripMenuItem frmshippedConsignment_followup;

	private ToolStripMenuItem ExImRptFrmAdvancePaymentReceiptAndUtilization;

	private ToolStripMenuItem PayablesAging;

	private ToolStripMenuItem Payables;

	private ToolStripMenuItem ExpenseVoucher;

	private ToolStripMenuItem btnMultiCurrency;

	private ToolStripMenuItem btnPackingMaterial;

	private ToolStripMenuItem InvDeffrmWarehouse;

	private ToolStripMenuItem btnTaxation;

	private ToolStripMenuItem InvfrmAddTaxType;

	private ToolStripMenuItem InvfrmAddTaxSchedule;

	private ToolStripMenuItem InvDeffrmItemTaxSchdule;

	private ToolStripMenuItem frmTaxNotesAndGLMaping;

	private ToolStripMenuItem frmSupplierCustomerExemptionSchedule;

	private ToolStripMenuItem btnDashBoard;

	private ToolStripMenuItem SaleTaxSummary;

	private ToolStripMenuItem btnScreenDefination;

	private ToolStripMenuItem pdcReprotsToolStripMenuItem;

	private ToolStripMenuItem PdcInventoryReport;

	private ToolStripMenuItem invLookUpsToolStripMenuItem;

	private ToolStripMenuItem GeneralJournalSummeryRegister;

	private ToolStripMenuItem productionReportsToolStripMenuItem;

	private ToolStripMenuItem frmProductionPendingPurchaseInvoice;

	private ToolStripMenuItem ExportPartyDefines;

	private ToolStripMenuItem AuditDashboard;

	private ToolStripMenuItem AccountsCustomGroup;

	private ToolStripMenuItem btnDatabaseBackup;

	private ToolStripMenuItem btnStoreManagement;

	private ToolStripMenuItem frmStoreOpeningStockBalancing;

	private ToolStripMenuItem PurchaseOrderStore;

	private ToolStripMenuItem frmPurchaseDemand;

	private ToolStripMenuItem frmGSIssuance;

	private ToolStripMenuItem GatePassGeneral;

	private ToolStripMenuItem AddItemStore;

	private ToolStripMenuItem GrnStore;

	private ToolStripMenuItem ItemCategoryStore;

	private ToolStripMenuItem ItemTypeStore;

	private ToolStripMenuItem frmDepartmentRequest;

	private ToolStripMenuItem DepartmentRequestHistory;

	private ToolStripMenuItem StoreIssuenceHistory;

	private ToolStripMenuItem btnHRM;

	private ToolStripMenuItem btnPolicyManegement;

	private ToolStripMenuItem IncomeTaxPolicy;

	private ToolStripMenuItem EOBIPolicy;

	private ToolStripMenuItem GeneralPolicy;

	private ToolStripMenuItem LeaveQuotaPolicy;

	private ToolStripMenuItem ProvidentFundPolicy;

	private ToolStripMenuItem SalaryBreakupPolicy;

	private ToolStripMenuItem SocialSecurityPolicy;

	private ToolStripMenuItem DeliveryOrder;

	private ToolStripMenuItem frmSaleInvoiceDirectRegister;

	private ToolStripMenuItem DeliveryOrderPackingMaterial;

	private ToolStripMenuItem btnEmptyBagsReports;

	private ToolStripMenuItem frmStockWithSupplierHistory;

	private ToolStripMenuItem frmDeliveryOrderForApprovel;

	private ToolStripMenuItem AcFrmDashboard;

	private ToolStripMenuItem PendingVouchersForApproval;

	private ToolStripMenuItem GatePassGeneralOutward;

	private ToolStripMenuItem frmStockTransfer;

	private ToolStripMenuItem PurchaseInvoiceStoreManagement;

	private Panel panel3;

	private Label lblusername;

	private ToolStripMenuItem BankBalances;

	private ToolStripMenuItem CashBalances;

	private ToolStripMenuItem VoucherEntry;

	private ToolStripMenuItem StoreIssuanceFinancial;

	private ToolStripMenuItem btnStockReport;

	private ToolStripMenuItem frmStockReport;

	private ToolStripMenuItem InventoryTransactionReport;

	private ToolStripMenuItem RptGenrateStocks;

	private ToolStripMenuItem InventoryTransactionReportStore;

	private ToolStripMenuItem RptGenrateStocksStore;

	private Label label1;

	private ToolStripMenuItem frmExportInvoiceVoucher;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripMenuItem frmGatePassGeneral;

	private ToolStripMenuItem FCYPayablesAndReceivablesRpt;

	private ToolStripMenuItem frmSaleInvoiceReturnRegister;

	private ToolStripMenuItem FoodProductionComparisonRpt;

	private ToolStripMenuItem frmStockTranfserRegister;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripSeparator toolStripSeparator7;

	private ToolStripMenuItem PayablesandReceivablesAging;

	private ToolStripMenuItem btnEmployeeManagement;

	private ToolStripMenuItem EmployeeRegistration;

	private ToolStripMenuItem btnAttendanceManagement;

	private ToolStripMenuItem DefineShift;

	private ToolStripMenuItem ShiftLocation;

	private ToolStripMenuItem ShiftTiming;

	private ToolStripMenuItem btnHRMReports;

	private ToolStripMenuItem DailyAttendanceRpt;

	private ToolStripMenuItem DailyLateandEarlyDeparture;

	private ToolStripMenuItem genDutyRosterEmployeeWise;

	private ToolStripMenuItem genEmployeeAttendence;

	private ToolStripMenuItem MonthlyAttendanceSummary;

	private ToolStripMenuItem SupplyOrderPurchase;

	private ToolStripMenuItem MarketPurchaseOrder;

	private ToolStripMenuItem MarketGrn;

	private ToolStripMenuItem PaymentByInvoiceVoucherNew;

	private ToolStripMenuItem btnPaymentByInvoiceSummery_Detail;

	private ToolStripMenuItem FoodProductionWithValues;

	private ToolStripMenuItem frmAdvanceAdjustment;

	private ToolStripMenuItem BillOfMaterialWiseProduction;

	private ToolStripMenuItem EximLcOrder;

	private ToolStripMenuItem EximPerformaInvoice;

	private ToolStripMenuItem ExportDeliveryOrder;

	private ToolStripMenuItem frmContainerList;

	private ToolStripMenuItem ExportInvoiceRegister;

	private ToolStripMenuItem SaleContractHistory;

	private ToolStripMenuItem InventoryPayablesandReceivables;

	private ToolStripMenuItem frmPosdefineitem;

	private ToolStripMenuItem btnImport;

	private ToolStripMenuItem frmImpLcContract;

	private ToolStripMenuItem frmImpLcPurchaseContract;

	private ToolStripMenuItem frmImpInvoice;

	private ToolStripMenuItem frmImpPerformaInvoice;

	private ToolStripMenuItem frmImpGrnForwarding;

	private ToolStripMenuItem frmImpPurchaseOrder;

	private ToolStripSeparator btnBillofLadingImport;

	private ToolStripMenuItem frmImpBL;

	private ToolStripMenuItem frmImpGrnDirect;

	private ToolStripMenuItem ExportCustomerWiseComparisonSummary;

	private ToolStripMenuItem ExportComparisonSummary;

	private ToolStripMenuItem ExportDetailHistory;

	private ToolStripMenuItem frmExportSummaryRegister;

	private ToolStripMenuItem btnPayroll;

	private ToolStripMenuItem btnPayrollSalaryReport;

	private ToolStripMenuItem PayRollSalarySheetRpt;

	private ToolStripMenuItem StoreIssuanceDirect;

	private ToolStripMenuItem frmImportOrderPackingDetail;

	private ToolStripMenuItem FcyGeneralLedgerRpt;

	private ToolStripMenuItem frmFcyPaymentVoucher;

	private ToolStripMenuItem btnImportReports;

	private ToolStripMenuItem ImportContractRegister;

	private ToolStripMenuItem ImportPurchaseOrderRegister;

	private ToolStripMenuItem ImportOrderPackingDetailRegister;

	private ToolStripMenuItem ImportGrnRegister;

	private ToolStripMenuItem ImportInvoiceRegister;

	private ToolStripMenuItem btnWeightBridge;

	private ToolStripMenuItem frmWeightbridge;

	private ToolStripMenuItem WeightbridgeMannual;

	private ToolStripMenuItem EmployeeMonthlyRegister;

	private ToolStripMenuItem frmCreditNotes;

	private ToolStripMenuItem frmIncentivePolicy;

	private ToolStripMenuItem EmployeeAllowance;

	private ToolStripMenuItem EmployeeLateAdjustment;

	private ToolStripMenuItem EmployeeLoanDeduction;

	private ToolStripMenuItem EmployeeShortAdjustment;

	private ToolStripMenuItem PayrollPosting;

	private ToolStripMenuItem btnProfileManagement;

	private ToolStripMenuItem GeneralSetting;

	private ToolStripMenuItem EmployeeCategory;

	private ToolStripMenuItem Benefit;

	private ToolStripMenuItem Designation;

	private ToolStripMenuItem Department;

	private ToolStripMenuItem Section;

	private new ToolStripMenuItem Location;

	private ToolStripMenuItem TrialRegistration;

	private ToolStripMenuItem EmployeeNotice;

	private ToolStripMenuItem EmployeeApproval;

	private ToolStripMenuItem ProbationApproval;

	private ToolStripMenuItem btnDeviceManagement;

	private ToolStripMenuItem DeviceConfiguration;

	private ToolStripMenuItem PullAttendance;

	private ToolStripMenuItem Holiday;

	private ToolStripMenuItem DutyRoaster;

	private ToolStripMenuItem EmployeeRoaster;

	private ToolStripMenuItem DailyAttendance;

	private ToolStripMenuItem ManualAttendance;

	private ToolStripMenuItem btnLeaveManagement;

	private ToolStripMenuItem LeaveOpening;

	private ToolStripMenuItem EmployeeLeave;

	private ToolStripMenuItem EmployeeSpecialLeave;

	private ToolStripMenuItem EmployeeShortLeave;

	private ToolStripMenuItem LeaveClosing;

	private ToolStripMenuItem btnLoanManagement;

	private ToolStripMenuItem EmployeeLoan;

	private ToolStripMenuItem EmployeeAdvance;

	private ToolStripMenuItem btnOverTimeManagement;

	private ToolStripMenuItem OverTimeRequest;

	private ToolStripMenuItem OTRequestApproval;

	private ToolStripMenuItem EmployeeOverTime;

	private ToolStripMenuItem btnApprovalManagement;

	private ToolStripMenuItem LoanApproval;

	private ToolStripMenuItem LeaveApproval;

	private ToolStripMenuItem AdvanceApproval;

	private ToolStripMenuItem OverTimeApproval;

	private ToolStripMenuItem ShortLeaveApproval;

	private ToolStripMenuItem AttendanceApproval;

	private ToolStripMenuItem ShortHourApproval;

	private ToolStripMenuItem AllowanceApproval;

	private ToolStripMenuItem LoanDeductionApproval;

	private ToolStripMenuItem OvertimePosting;

	private ToolStripMenuItem DailyWagesPosting;

	private ToolStripMenuItem btnGlobalEmployment;

	private ToolStripMenuItem EmployeePromotion;

	private ToolStripMenuItem EmployeeIncrement;

	private ToolStripMenuItem EmployeeTransfer;

	private ToolStripMenuItem FullandFinalSettlement;

	private ToolStripMenuItem BlackListed;

	private ToolStripMenuItem ProfileType;

	private ToolStripMenuItem Profile;

	private ToolStripMenuItem SalesAnalytics;

	private ToolStripMenuItem SaleInvoiceRegisterNew;

	private ToolStripMenuItem TradeDebitorsReport;

	private ToolStripMenuItem TradeCreditorsReport;

	private ToolStripMenuItem frmDefineFixedAssets;

	private ToolStripMenuItem EmployeeHistoryRpt;

	private ToolStripMenuItem frmEvaulationDetailWagesReports;

	private ToolStripMenuItem frmFixedAssetsImportPurchaseOrder;

	private ToolStripMenuItem EmployeeGroup;

	private ToolStripMenuItem DefineReports;

	private ToolStripMenuItem PartyProcessing;

	private ToolStripMenuItem GatePassInwardPartyProcessing;

	private ToolStripMenuItem GatePassOutwardParyProcessing;

	private ToolStripMenuItem SelectedTrialBalanceAllLevel;

	private ToolStripMenuItem frmPartyProcessingDefineSupplier;

	private ToolStripMenuItem DefineReferenceParties;

	private ToolStripMenuItem DefItemCatagoryPartyProcessing;

	private ToolStripMenuItem DefPartyProcessingItemType;

	private ToolStripMenuItem DefPartProcessingAddItem;

	private ToolStripMenuItem GoodsReceivingNotesPartyProcessing;

	private ToolStripMenuItem GoodsDispatchNotesPartyProcessing;

	private ToolStripMenuItem btnPartyProcessingReports;

	private ToolStripMenuItem frmGatePassPartyProcessing;

	private ToolStripMenuItem frmGRNPartyProcessingRegister;

	private ToolStripMenuItem grmGDNPartyProcessingRegister;

	private ToolStripMenuItem GRNStorePartyProcessing;

	private ToolStripMenuItem GDNStorePartyProcessing;

	private ToolStripMenuItem frmProductionPartyProcessing;

	private ToolStripMenuItem ProductionJobOrderPartyProcessing;

	private ToolStripMenuItem frmStockReportPartyProcessing;

	private ToolStripMenuItem StockOpeningBalancePartyProcessing;

	private ToolStripMenuItem PurchaseRegisterNew;

	private ToolStripMenuItem ItemLedgerReport_Inventory;

	private ToolStripMenuItem StockReportWithAvgRates;

	private ToolStripMenuItem btnStock_Reconcilation;

	private ToolStripMenuItem ProductionPartyProcessingBill;

	private ToolStripMenuItem ExpfrmFinancialInsturment;

	private ToolStripMenuItem PullAttendanceByMachine;

	private ToolStripMenuItem GoodsDispatchNotesWithoutWeighBridge;

	private ToolStripMenuItem frmPackingMaterialTransferPartyToParty;

	private ToolStripMenuItem FrmSaleInvoiceDirectPackingMaterial;

	private ToolStripMenuItem UnApprovedVouchersDashBoard;

	private ToolStripMenuItem InvLabStandardPolicyForDeduction;

	private ToolStripMenuItem btnServices;

	private ToolStripMenuItem btndefineservices;

	private ToolStripMenuItem frmSrvCustPricingAgreement;

	private ToolStripMenuItem frmSrvSaleOrderForwarder;

	private ToolStripMenuItem frmServClearingAgentBill;

	private ToolStripMenuItem ExImClearingAgentBillDirect;

	private ToolStripMenuItem frmPreShipmentCosting;

	private ToolStripMenuItem btnServicesReport;

	private ToolStripMenuItem ServicesBillRegister;

	private ToolStripMenuItem ExImShipmentCosting;

	private ToolStripMenuItem PayablesByDueDate;

	private ToolStripMenuItem PurchaseInvoiceStoreWithOutTax;

	private ToolStripMenuItem PurchaseInvoiceStoreWithTax;

	private ToolStripMenuItem frmPurchaseInvoiceDirectStore;

	private ToolStripMenuItem PurchaseInvoiceDirectStoreWithTax;

	private ToolStripMenuItem frmPurchaseOrderWithTax;

	private ToolStripMenuItem frmSaleInvoiceDirectWithOutTax;

	private ToolStripSeparator toolStripSeparator8;

	private ToolStripMenuItem frmSaleInvoiceDirectWithTax;

	private ToolStripMenuItem frmProfitAndLoss;

	private ToolStripMenuItem frmImportCommercialInvoicebyContract;

	private ToolStripMenuItem frmBalanceSheet;

	private ToolStripMenuItem frmWeighBridgeRejectedTicketNos;

	private ToolStripMenuItem btnCPLLeaveMangement;

	private ToolStripMenuItem frmEmployeeCPLLeaveOpening;

	private ToolStripMenuItem btnCPLAttendance;

	private ToolStripMenuItem btnCPLRequest;

	private ToolStripMenuItem frmCPLApproval;

	private ToolStripMenuItem btnStocksDashBoard;

	private ToolStripMenuItem frmProductionCumPackMaterialConsumption;

	private ToolStripMenuItem frmPendingWorksRpt;

	private ToolStripMenuItem btnAccountLookupa;

	private ToolStripMenuItem btnPlAndBsSettings;

	private ToolStripMenuItem InvFrmGRNDirect;

	private ToolStripMenuItem SaleInvoicepartyStockReserve;

	private ToolStripMenuItem PurchaseInvoiceAgainstGrnDirect;

	private ToolStripMenuItem frmSaleInvoiceDirect139;

	private ToolStripMenuItem UploadFiles;

	private ToolStripMenuItem ProductionPreCostingAndJobOrder;

	private ToolStripMenuItem frmCashPaymentVoucher;

	private ToolStripMenuItem frmBankPaymentVoucher;

	private ToolStripMenuItem frmCashReceiptVoucher;

	private ToolStripMenuItem frmBankReceiptVoucher;

	private ToolStripMenuItem btnPricingScheduleForGatePurchase;

	private ToolStripMenuItem frmInventoryProfitLoss;

	private ToolStripMenuItem frmStockAdjustment;

	private ToolStripMenuItem StockAdjustmentRegister;

	private ToolStripMenuItem StoreReturn;

	private ToolStripMenuItem frmPurchaseRegisterStore;

	private ToolStripMenuItem frmPurchaseDemandRegister;

	private ToolStripMenuItem StoreIssuanceReturnRegister;

	private ToolStripMenuItem frmAccountsPrematureReceipts;

	private ToolStripMenuItem frmDayBook;

	private ToolStripMenuItem FreightVoucher;

	private Label lblTime;

	private System.Windows.Forms.Timer timer2;

	private ToolStripMenuItem EximForwardingWithoutWeighBridge;

	private ToolStripMenuItem frmBoilerConsumption;

	private ToolStripMenuItem PaddyPurchase;

	private ToolStripMenuItem frmGatePassInwardWithDetail;

	private ToolStripMenuItem btnAnalyticDashboard;

	private ToolStripMenuItem frmPaddyPurchaseAnalytics;

	private ToolStripMenuItem frmRicePurchaseAnalytics;

	private ToolStripMenuItem frmSalesAnalytics;

	private ToolStripMenuItem frmGrnForGate;

	private ToolStripMenuItem frmPurchaseInvoiceForGate;

	private ToolStripMenuItem frmPartyProcessingGrnInfo;

	private ToolStripMenuItem dummyGridToolStripMenuItem;

	private ToolStripMenuItem CostomerWiseVoucherSlip;

	private ToolStripMenuItem CommissionAgentTrade;

	private ToolStripMenuItem frmCommissionAgentOrder;

	private ToolStripMenuItem frmCommissionAgentTradeBill;

	private ToolStripMenuItem CommissionAgentTradeBill_162;

	private ToolStripMenuItem InLandFreightAgreement;

	private ToolStripMenuItem OrderManagementDashboard;

	private ToolStripMenuItem InventoryEvaluationItemLedger;

	private ToolStripMenuItem ScaleKartDefine;

	internal UltraCombo CmbNavigation;

	private Panel panel4;

	private ToolStripMenuItem btnExportForwardingDirect;

	internal Button btnFavourites;

	internal Button btnHome;

	internal ContextMenuStrip contextMenuStrip5;

	public Panel panel2;

	private Button btnNext;

	private Button btnBack;

	internal Button btnNavigateToForms;

	private ContextMenuStrip contextMenuStrip6;

	private ToolStripMenuItem btnPlBSNotes;

	private ToolStripMenuItem companyProfileToolStripMenuItem;

	private ToolStripMenuItem btnAuditLogReport;

	private ToolStripMenuItem licenseKeyToolStripMenuItem;

	private Label lblFinancialYear;

	private BackgroundWorker bw_CheckDownload;

	private PictureBox btnUpdate;

	private ToolStripMenuItem sQLQueryExecuterToolStripMenuItem;

	private Label lblBranchName;

	internal Button BtnViewRightsReport;

	private ToolStripMenuItem btnWeighBridge_WeightUpdate;

	private ToolStripMenuItem btnUserProfile;

	private ToolStripMenuItem BtnThemeForm;

	private ToolStripMenuItem btnAccountMovement;

	private int currentVersionLabel
	{
		get
		{
			string Version = "";
			if (ApplicationDeployment.IsNetworkDeployed)
			{
				Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;
				Version = string.Format("Product Name: {4}, Version: {0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision, Assembly.GetEntryAssembly().GetName().Name);
			}
			else
			{
				Version ver2 = Assembly.GetExecutingAssembly().GetName().Version;
				Version = string.Format("{4}, Version: {0}.{1}.{2}.{3}", ver2.Major, ver2.Minor, ver2.Build, ver2.Revision, Assembly.GetEntryAssembly().GetName().Name);
			}
			string[] array = Version.Split('.');
			return int.Parse(array[array.Length - 1]);
		}
	}

	public string VersionLabel
	{
		get
		{
			if (ApplicationDeployment.IsNetworkDeployed)
			{
				Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;
				return string.Format("Product Name: {4}, Version: {0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision, Assembly.GetEntryAssembly().GetName().Name);
			}
			Version ver2 = Assembly.GetExecutingAssembly().GetName().Version;
			return string.Format("{4}, Version: {0}.{1}.{2}.{3}", ver2.Major, ver2.Minor, ver2.Build, ver2.Revision, Assembly.GetEntryAssembly().GetName().Name);
		}
	}

	public DashboardNew(UserAccount pUserAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
		AutoSize = true;
		base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
		base.WindowState = FormWindowState.Maximized;
	}

	private void DashboardNew_Load(object sender, EventArgs e)
	{
		try
		{
			btnadminpanel.Visible = false;
			if (UserAccount.RoleName == "Admin")
			{
				btnadminpanel.Visible = true;
				BtnViewRightsReport.Visible = true;
				BtnThemeForm.Visible = true;
			}
			clsGlobalVariables.ScreenViewReights = GetViewRightsByUserId();
			InitializeMenu();
			ScreenBindInNavigation();
			SetFinancialDates();
			clsGlobalVariables.ErpFeaturesList = GetCompanyFeaturesList();
			if (UserAccount.AppId != 5)
			{
				frmModules objForm = new frmModules(UserAccount, this);
				objForm.TopLevel = false;
				panel2.Dock = DockStyle.Fill;
				panel2.Controls.Add(objForm);
				HistoryStack.Push(objForm);
				objForm.Dock = DockStyle.Fill;
				objForm.Show();
			}
			else if (!panel2.Controls.Cast<Control>().Any((Control c) => c.Name == "BookingOfficeDashboard"))
			{
				BookingOfficeDashboard objForm2 = new BookingOfficeDashboard(UserAccount);
				objForm2.TopLevel = false;
				panel2.Dock = DockStyle.Fill;
				panel2.Controls.Add(objForm2);
				HistoryStack.Push(objForm2);
				objForm2.Dock = DockStyle.Fill;
				objForm2.Show();
			}
			clsGlobalVariables.Daw_PolicyDocuments = GetDocumentsInApprovalPolicy();
			clsGlobalVariables.WagesRefDocumentsStatusList = GetWagesRefDocumentStatuses();
			clsGlobalVariables.GetApplicationsAllocateToCompanyList = ApplicationsAllocateToUser.GetApplicationsAllocateToCompany();
			CommonServices.GetDecimalConfiguration();
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList();
			if (list != null && list.Count > 0)
			{
				clsGlobalVariables.AttachmentFolderPath = list[0].ConfigKey.ToString() + "\\";
			}
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CrystalPrintPreviewZoomDefault").ToList();
			if (lstJob != null && lstJob.Count > 0)
			{
				clsGlobalVariables.CrystalPrintPreviewZoomDefault = Conversion.ToInt((object)lstJob[0].ConfigKey);
			}
			lblusername.Text = Conversion.ToString((object)(UserAccount.FirstName + " " + UserAccount.LastName + " (" + UserAccount.RoleName + ")"));
			lblBranchName.Text = Conversion.ToString((object)UserAccount.BranchName);
			lblFinancialYear.Text = Conversion.ToString((object)clsGlobalVariables.ActiveYr.FinancialYearCode);
			label1.Text = VersionLabel.ToString();
			timer2.Start();
			Company comp = Company.GetByID(UserAccount.CompanyId);
			if (comp != null)
			{
				clsGlobalVariables.dtCompLogo.Columns.Clear();
				clsGlobalVariables.dtCompLogo.Columns.Add("ComplogoImage", typeof(byte[]));
				clsGlobalVariables.dtCompLogo.Rows.Add(comp.CompLogoImage);
			}
			bw_CheckDownload.RunWorkerAsync();
			versionTimer.Interval = (int)TimeSpan.FromMinutes(10.0).TotalMilliseconds;
			versionTimer.Tick += VersionTimer_Tick;
			versionTimer.Start();
			Task.Run(delegate
			{
				RunInBackground(_cts.Token);
			});
			btnUserProfile.Visible = true;
			IsVpsAttachmentsServiceOn = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("IsVpsAttachmentsServiceOn"));
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			UserAccount userAccount = clsGlobalVariables.UserAccount;
			object obj;
			if (userAccount == null)
			{
				obj = null;
			}
			else
			{
				UserProfile userProfileList = userAccount.UserProfileList;
				obj = ((userProfileList != null) ? userProfileList.ProfileImageFileName : null);
			}
			if (obj != null)
			{
				UserAccount userAccount2 = clsGlobalVariables.UserAccount;
				object obj2;
				if (userAccount2 == null)
				{
					obj2 = null;
				}
				else
				{
					UserProfile userProfileList2 = userAccount2.UserProfileList;
					obj2 = ((userProfileList2 != null) ? userProfileList2.ProfileImageFileName : null);
				}
				if ((string)obj2 != string.Empty)
				{
					if (!IsVpsAttachmentsServiceOn)
					{
						if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
						{
							throw new Exception("Attachment Folder Path Not Configure Properly");
						}
						Bitmap circularImage = CommonServices.MakeCircularImage(CommonServices.ResizeImage(new Bitmap(Path.Combine(clsGlobalVariables.AttachmentFolderPath, clsGlobalVariables.UserAccount.UserProfileList.ProfileImageFileName)), btnRefresh.Width, btnRefresh.Height));
						btnRefresh.Image = circularImage;
						return;
					}
					string uri = "ftp://" + Sc.ServerIp + "/" + clsGlobalVariables.UserAccount.UserProfileList.ProfileImageFileName;
					try
					{
						FtpWebRequest obj3 = (FtpWebRequest)WebRequest.Create(uri);
						obj3.Method = "RETR";
						obj3.UsePassive = true;
						obj3.UseBinary = true;
						obj3.KeepAlive = false;
						obj3.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using FtpWebResponse ftpWebResponse = (FtpWebResponse)obj3.GetResponse();
						using Stream responseStream = ftpWebResponse.GetResponseStream();
						using MemoryStream memoryStream = new MemoryStream();
						responseStream.CopyTo(memoryStream);
						_ = (double)memoryStream.Length / 1048576.0;
						memoryStream.Position = 0L;
						using MemoryStream stream = new MemoryStream();
						Image image = Image.FromStream(memoryStream);
						Bitmap bmp = new Bitmap(image);
						bmp.Save(stream, image.RawFormat);
						btnRefresh.Image = bmp;
						btnRefresh.SizeMode = PictureBoxSizeMode.CenterImage;
						image.Dispose();
						stream.Position = 0L;
						stream.Dispose();
						return;
					}
					catch (WebException webEx)
					{
						if (webEx.Response is FtpWebResponse response)
						{
							throw new Exception("FTP Error: " + response.StatusDescription + " - " + webEx.Message);
						}
						throw new Exception("Error: " + webEx.Message);
					}
					catch (Exception ex)
					{
						throw new Exception("Error: " + ex.Message);
					}
				}
			}
			Bitmap circularImage2 = CommonServices.MakeCircularImage(CommonServices.ResizeImage(new Bitmap(Resources.DummyUser), btnRefresh.Width, btnRefresh.Height));
			btnRefresh.Image = circularImage2;
			btnRefresh.SizeMode = PictureBoxSizeMode.CenterImage;
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void RunInBackground(CancellationToken token)
	{
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		if (token.IsCancellationRequested)
		{
			return;
		}
		List<string> allServices = new List<string>
		{
			"BranchesAllocatedToUser", "AccountsWithCustomGroupId", "ItemsWithCustomGroupId", "LookUpsEngr", "PaymentTerm", "UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems",
			"AllSerivesItems", "MultiCurrency", "AllCities", "CropYear", "JobLot", "InvPackingType", "AllBrands", "DriverBioInfo", "ItemConditions", "WarehouseRacks",
			"RacksWithWarehouseAndItem", "DeliveryTerm"
		};
		DatatableHelper.GlobalServicesDbCall(UserAccount, allServices, token);
		if (!token.IsCancellationRequested)
		{
			DashBoardThemeSetting();
			if (!token.IsCancellationRequested)
			{
				new Reporting().LoadReport();
			}
		}
	}

	private List<WagesRefDocumentsStatus> GetWagesRefDocumentStatuses()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		List<WagesRefDocumentsStatus> lst = new List<WagesRefDocumentsStatus>();
		try
		{
			DataTable dt = InvContractorWagesBillHeader.GetRefDocumentsForWages(0);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					WagesRefDocumentsStatus obj = new WagesRefDocumentsStatus();
					obj.RefDocumentTypeId = Conversion.ToInt(dt.Rows[i]["RefDocumentTypeId"]);
					obj.DocumentTypeDescription = Conversion.ToString(dt.Rows[i]["DocumentTypeDescription"]);
					obj.IsActive = Conversion.ToBool(dt.Rows[i]["IsActive"]);
					lst.Add(obj);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return lst;
	}

	private List<ErpFeaturesByCompany> GetCompanyFeaturesList()
	{
		List<ErpFeaturesByCompany> lst = new List<ErpFeaturesByCompany>();
		try
		{
			lst = CompanyFeatures.GetERPFeaturesByCompanyId(UserAccount.CompanyId, UserAccount.OrganizationId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return lst;
	}

	private List<PolicyDocuments> GetDocumentsInApprovalPolicy()
	{
		List<PolicyDocuments> lst = new List<PolicyDocuments>();
		try
		{
			lst = DocumentApprovalPolicy.GetDocumentsInPolicy(UserAccount.OrganizationId, UserAccount.CompanyId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return lst;
	}

	private void SetFinancialDates()
	{
		try
		{
			DataTable dt = FinancialYear.getFinancialStartAndEndDate(UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				clsGlobalVariables.FirstFinancialStartDate = Conversion.ToDateTime(dt.Rows[0]["Start_Period"]);
				clsGlobalVariables.RunningFinancialEndDate = Conversion.ToDateTime(dt.Rows[0]["End_Period"]);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void InitializeMenu()
	{
		try
		{
			ContextMenuStrip1.Items.Clear();
			contextMenuStrip2.Items.Clear();
			List<tblUserRights> mlist = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ModuleTypeId == 1 && x.Value).ToList();
			if (mlist.Count > 0)
			{
				foreach (var item2 in from x in mlist
					group x by new { x.ModuleID, x.ModuleDescription } into lg
					select new
					{
						GroupColumn = lg.Key,
						TotalCount = lg.Count()
					})
				{
					ToolStripMenuItem tsmi2 = new ToolStripMenuItem();
					tsmi2.Text = Conversion.ToString((object)item2.GroupColumn.ModuleDescription);
					List<tblUserRights> list = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ModuleID == item2.GroupColumn.ModuleID && x.Value).ToList();
					ToolStripMenuItem[] arr2 = new ToolStripMenuItem[list.Count];
					int counter2 = 0;
					foreach (tblUserRights mnu2 in list)
					{
						ToolStripMenuItem tsMenu2 = new ToolStripMenuItem();
						tsMenu2.Name = mnu2.ScreenName + "ToolStripMenuItem";
						tsMenu2.Size = new Size(180, 22);
						tsMenu2.Text = mnu2.ScreenAlias;
						tsMenu2.Tag = mnu2.ScreenID;
						arr2[counter2] = tsMenu2;
						counter2++;
						tsMenu2.Click += MenuItem_Click;
					}
					ToolStripItemCollection dropDownItems = tsmi2.DropDownItems;
					ToolStripItem[] toolStripItems = arr2;
					dropDownItems.AddRange(toolStripItems);
					ContextMenuStrip1.Items.Add(tsmi2);
				}
			}
			List<tblUserRights> mlist2 = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ModuleTypeId == 2 && x.Value).ToList();
			if (mlist2.Count > 0)
			{
				foreach (var item in from x in mlist2
					group x by new { x.ModuleID, x.ModuleDescription } into lg
					select new
					{
						GroupColumn = lg.Key,
						TotalCount = lg.Count()
					})
				{
					ToolStripMenuItem tsmi = new ToolStripMenuItem();
					tsmi.Text = Conversion.ToString((object)item.GroupColumn.ModuleDescription);
					List<tblUserRights> list2 = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ModuleID == item.GroupColumn.ModuleID && x.Value).ToList();
					ToolStripMenuItem[] arr = new ToolStripMenuItem[list2.Count];
					int counter = 0;
					foreach (tblUserRights mnu in list2)
					{
						ToolStripMenuItem tsMenu = new ToolStripMenuItem();
						tsMenu.Name = mnu.ScreenName + "ToolStripMenuItem";
						tsMenu.Size = new Size(180, 22);
						tsMenu.Text = mnu.ScreenAlias;
						tsMenu.Tag = mnu.ScreenID;
						arr[counter] = tsMenu;
						counter++;
						tsMenu.Click += MenuItem_Click;
					}
					ToolStripItemCollection dropDownItems2 = tsmi.DropDownItems;
					ToolStripItem[] toolStripItems = arr;
					dropDownItems2.AddRange(toolStripItems);
					contextMenuStrip2.Items.Add(tsmi);
				}
			}
			FavoritScreensMenu();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VersionTimer_Tick(object sender, EventArgs e)
	{
		CheckApplicationVersion();
	}

	private void CheckApplicationVersion()
	{
		int num = currentVersionLabel;
		int dbVersion = GetVersionFromDatabase();
		if (num != dbVersion)
		{
			versionTimer.Stop();
			ForceLogout();
		}
	}

	private void ForceLogout()
	{
		MessageBox.Show("A new version is available. Please update the application.", "Update Required", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		_cts.Cancel();
		Environment.Exit(0);
	}

	private int GetVersionFromDatabase()
	{
		return UserAccount.GetVersion(UserAccount.ID);
	}

	private void MenuItem_Click(object sender, EventArgs e)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		try
		{
			ToolStripMenuItem getMenu = (ToolStripMenuItem)sender;
			tblUserRights obj = clsGlobalVariables.ScreenViewReights.Find((tblUserRights x) => x.ScreenID == Convert.ToInt32(getMenu.Tag));
			if (obj.TargetUrl == null)
			{
				throw new Exception("Target URL not Configured properly... " + obj.ScreenAlias);
			}
			if (Type.GetType(obj.TargetUrl) != null)
			{
				UserAccount.UserAccountAllocationsList = new List<UserAccountAllocation>();
				UserAccountAllocation allocation = new UserAccountAllocation();
				allocation.ScreenName = obj.ScreenName;
				UserAccount.UserAccountAllocationsList.Add(allocation);
				if (Activator.CreateInstance(Type.GetType(obj.TargetUrl), UserAccount) is Form form)
				{
					form.Text = obj.ScreenAlias;
					form.Tag = obj.ScreenName;
					form.TopLevel = false;
					form.AutoScroll = true;
					form.WindowState = FormWindowState.Maximized;
					panel2.Controls.Add(form);
					HistoryStack.Push(form);
					form.Show();
				}
				return;
			}
			throw new Exception("Target URL not Configured properly... " + obj.ScreenAlias);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ScreenBindInNavigation()
	{
		try
		{
			List<tblUserRights> lst = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.Value && x.TargetUrl != null && x.TargetUrl != string.Empty).ToList();
			if (lst.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("TargetUrl");
				dt.Columns.Add("ScreenAlias");
				dt.Columns.Add("ScreenName");
				dt.Columns.Add("ModuleName");
				for (int i = 0; i < lst.Count; i++)
				{
					dt.Rows.Add(lst[i].TargetUrl, lst[i].ScreenAlias, lst[i].ScreenName, lst[i].ModuleDescription);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbNavigation, "TargetUrl", "ScreenAlias", "Search", ZeroIndex: true);
					((UltraGridBase)CmbNavigation).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbNavigation).DisplayLayout.Bands[0].Columns[1].Width = 500;
					((UltraGridBase)CmbNavigation).Rows[0].Activate();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void FavoritScreensMenu1()
	{
		try
		{
			contextMenuStrip5.Items.Clear();
			List<tblUserRights> lstFav = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.IsFavorite).ToList();
			if (lstFav.Count <= 0)
			{
				return;
			}
			ToolStripMenuItem[] arr = new ToolStripMenuItem[lstFav.Count];
			int counter = 0;
			foreach (tblUserRights mnu in lstFav)
			{
				ToolStripMenuItem tsMenu = new ToolStripMenuItem
				{
					Name = mnu.ScreenName + "ToolStripMenuItem",
					Size = new Size(180, 22),
					Text = mnu.ScreenAlias,
					Tag = mnu.ScreenID
				};
				ToolStripMenuItem tsRemoveItem = new ToolStripMenuItem
				{
					Text = "X",
					Tag = mnu.ScreenName
				};
				tsRemoveItem.Click += delegate
				{
					RemoveFavorite(mnu.ScreenName);
				};
				tsMenu.DropDownItems.Add(tsRemoveItem);
				arr[counter] = tsMenu;
				counter++;
				tsMenu.Click += MenuItem_Click;
				contextMenuStrip5.Items.Add(tsMenu);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RemoveFavorite(string ScreenName)
	{
		try
		{
			if (clsGlobalVariables.ScreenViewReights.FirstOrDefault((tblUserRights x) => x.ScreenName == ScreenName) != null)
			{
				FavoriteScreens.DeleteByScreenNameAndUserId(ScreenName, UserAccount.ID);
				clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => Conversion.ToString((object)x.ScreenName) == ScreenName && x.Value).ToList().ForEach(delegate(tblUserRights x)
				{
					x.IsFavorite = false;
				});
				FavoritScreensMenu();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error removing item: " + ex.Message);
		}
	}

	private void btnFavourites_MouseLeave(object sender, EventArgs e)
	{
	}

	private void btnFavourites_MouseHover(object sender, EventArgs e)
	{
		if (FavouritScreens_Flag)
		{
			contextMenuStrip5.Items.Clear();
			FavoritScreensMenu();
		}
		contextMenuStrip5.Show(btnFavourites, 0, btnFavourites.Height);
	}

	public void FavoritScreensMenu()
	{
		try
		{
			contextMenuStrip5.Items.Clear();
			List<tblUserRights> lstFav = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.IsFavorite).ToList();
			if (lstFav.Count <= 0)
			{
				return;
			}
			foreach (tblUserRights mnu in lstFav)
			{
				Panel obj = new Panel
				{
					Width = 250,
					Height = 22,
					BorderStyle = BorderStyle.None,
					Tag = mnu.ScreenID,
					Cursor = Cursors.Hand
				};
				Label label = new Label
				{
					Text = mnu.ScreenAlias,
					Tag = mnu.ScreenID,
					Size = new Size(150, 20),
					Location = new Point(25, 0),
					AutoSize = true,
					Font = new Font("Arial", 10f)
				};
				Button closeButton = new Button
				{
					Text = "X",
					Size = new Size(20, 20),
					Location = new Point(0, 0),
					FlatStyle = FlatStyle.Flat,
					BackColor = Color.Red,
					ForeColor = Color.White,
					Font = new Font("Tahoma", 7f, FontStyle.Bold)
				};
				closeButton.Click += delegate
				{
					RemoveFavorite(mnu.ScreenName);
				};
				obj.Controls.Add(label);
				obj.Controls.Add(closeButton);
				label.Click += delegate
				{
					FavouriteItem_Click(Conversion.ToInt(label.Tag));
				};
				ToolStripControlHost itemHost = new ToolStripControlHost(obj);
				contextMenuStrip5.Items.Add(itemHost);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FavouriteItem_Click(int ScreenId)
	{
		try
		{
			tblUserRights obj = clsGlobalVariables.ScreenViewReights.Find((tblUserRights x) => x.ScreenID == ScreenId);
			if (obj.TargetUrl == null)
			{
				throw new Exception("Target URL not Configured properly... " + obj.ScreenAlias);
			}
			if (Type.GetType(obj.TargetUrl) != null)
			{
				if (Activator.CreateInstance(Type.GetType(obj.TargetUrl), UserAccount) is Form form)
				{
					form.Text = obj.ScreenAlias;
					form.Tag = obj.ScreenName;
					form.TopLevel = false;
					form.AutoScroll = true;
					form.WindowState = FormWindowState.Maximized;
					panel2.Controls.Add(form);
					HistoryStack.Push(form);
					form.Show();
				}
				return;
			}
			throw new Exception("Target URL not Configured properly... " + obj.ScreenAlias);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private List<tblUserRights> GetViewRightsByUserId()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		List<tblUserRights> lst = new List<tblUserRights>();
		try
		{
			lst = tblUserRights.GetUserRightsForViewbyUserId(new ReportsParameters
			{
				EntryUser = UserAccount.ID,
				CompanyId = UserAccount.CompanyId,
				AppId = ((UserAccount.AppId != 3) ? UserAccount.AppId : 0)
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return lst;
	}

	private void MenuVisibleMethod()
	{
		btnAccountDefinition.Visible = false;
		AcfrmDefCoa.Visible = false;
		AcfrmAcAllocation.Visible = false;
		UpdateCoAllocation.Visible = false;
		AcfrmOpeningBalance.Visible = false;
		AccountsCustomGroup.Visible = false;
		AccountBudget.Visible = false;
		AcfrmChequebookRegistration.Visible = false;
		AcfrmDefineBank.Visible = false;
		frmAdvanceAdjustment.Visible = false;
		frmIncentivePolicy.Visible = false;
		InLandFreightAgreement.Visible = false;
		btnAccountsTransaction.Visible = false;
		ExpenseVoucher.Visible = false;
		PaymentVoucherNew.Visible = false;
		ReceiptsVoucherNew.Visible = false;
		ContraVoucher.Visible = false;
		VoucherEntry.Visible = false;
		frmWhtTaxChallanDeposit.Visible = false;
		frmBillsPayables.Visible = false;
		frmBillsReceivables.Visible = false;
		PaymentByInvoiceVoucherNew.Visible = false;
		PaymentByInvoiceAccount.Visible = false;
		ReceiptByInvoiceAccount.Visible = false;
		AcfrmDefPdcManagment.Visible = false;
		PostDatedCheqPaymentVouchers.Visible = false;
		Acfrmfcbankreceipt.Visible = false;
		frmFcyPaymentVoucher.Visible = false;
		frmExportInvoiceVoucher.Visible = false;
		StoreIssuanceFinancial.Visible = false;
		AccountsReconcilation.Visible = false;
		frmCreditNotes.Visible = false;
		frmCashPaymentVoucher.Visible = false;
		frmBankPaymentVoucher.Visible = false;
		frmCashReceiptVoucher.Visible = false;
		frmBankReceiptVoucher.Visible = false;
		frmAccountsPrematureReceipts.Visible = false;
		frmDayBook.Visible = false;
		FreightVoucher.Visible = false;
		btnLabAnalysis.Visible = false;
		InvLabSampleLogRegister.Visible = false;
		InvLabAnalysisItems.Visible = false;
		InvLabAnalysisGroup.Visible = false;
		InvLabGroupAnalysisStandards.Visible = false;
		InvLabSampleAnalysis.Visible = false;
		InvLabPurchaseAnalysis.Visible = false;
		InvLabSaleAnalysis.Visible = false;
		InvLabAnalysisInProcess.Visible = false;
		btnSupPurchase.Visible = false;
		supfrmDefineSupplier.Visible = false;
		PurchsaeOrder.Visible = false;
		MarketPurchaseOrder.Visible = false;
		SupplyOrderPurchase.Visible = false;
		InwardGatePass.Visible = false;
		InvFrmGRN.Visible = false;
		MarketGrn.Visible = false;
		InvfrmPurchaseInvoice.Visible = false;
		InvfrmPurchaseReturn.Visible = false;
		InvfrmPurchasedirectInvoice.Visible = false;
		frmOpeningStockBlancing.Visible = false;
		InvFrmGRNDirect.Visible = false;
		PurchaseInvoiceAgainstGrnDirect.Visible = false;
		PaddyPurchase.Visible = false;
		frmGatePassInwardWithDetail.Visible = false;
		frmGrnForGate.Visible = false;
		frmPurchaseInvoiceForGate.Visible = false;
		btnDashBoard.Visible = false;
		AcFrmDashboard.Visible = false;
		PendingVouchersForApproval.Visible = false;
		UnApprovedVouchersDashBoard.Visible = false;
		SalesAnalytics.Visible = false;
		frmPendingWorksRpt.Visible = false;
		btnStocksDashBoard.Visible = false;
		OrderManagementDashboard.Visible = false;
		btnAcReport.Visible = false;
		ChartOfAccount.Visible = false;
		VoucherReport.Visible = false;
		VoucherValidation.Visible = false;
		ActicitySummery.Visible = false;
		BankBalances.Visible = false;
		CashBalances.Visible = false;
		GeneralLedger.Visible = false;
		FcyGeneralLedgerRpt.Visible = false;
		CustomerLedger.Visible = false;
		GeneralLedgerStatment.Visible = false;
		Payables.Visible = false;
		PayablesAging.Visible = false;
		Receivables.Visible = false;
		ReceivableAging.Visible = false;
		TradeDebitorsReport.Visible = false;
		TradeCreditorsReport.Visible = false;
		btnPaymentByInvoiceSummery_Detail.Visible = false;
		InventoryPayablesandReceivables.Visible = false;
		FCYPayablesAndReceivablesRpt.Visible = false;
		TrialBalance.Visible = false;
		SelectedTrialBalance.Visible = false;
		PayablesandReceivablesAging.Visible = false;
		GeneralJournalSummeryRegister.Visible = false;
		AuditDashboard.Visible = false;
		SelectedTrialBalanceAllLevel.Visible = false;
		PayablesByDueDate.Visible = false;
		frmProfitAndLoss.Visible = false;
		frmBalanceSheet.Visible = false;
		CostomerWiseVoucherSlip.Visible = false;
		btnAnalyticDashboard.Visible = false;
		frmPaddyPurchaseAnalytics.Visible = false;
		frmRicePurchaseAnalytics.Visible = false;
		frmSalesAnalytics.Visible = false;
		pdcReprotsToolStripMenuItem.Visible = false;
		PdcInventoryReport.Visible = false;
		InventoryReport.Visible = false;
		PurchaseOrderHistory.Visible = false;
		frmGatePassGeneral.Visible = false;
		frmGatePassReport.Visible = false;
		frmGRNHistory.Visible = false;
		frmPurchaseInvoiceHistory.Visible = false;
		frmSaleOrderHistory.Visible = false;
		frmDeliveryOrderForApprovel.Visible = false;
		frmGPOutward.Visible = false;
		frmGDNHistory.Visible = false;
		SaleInvoiceRegisterNew.Visible = false;
		frmSaleInvoiceHistory.Visible = false;
		frmSaleInvoiceDirectRegister.Visible = false;
		frmWeightBridgeHistory.Visible = false;
		frmStockContractorWagesHistory.Visible = false;
		ItemLedger.Visible = false;
		frmRptItemList.Visible = false;
		DepartmentRequestHistory.Visible = false;
		StoreIssuenceHistory.Visible = false;
		frmSaleInvoiceReturnRegister.Visible = false;
		frmStockTranfserRegister.Visible = false;
		frmWeighBridgeRejectedTicketNos.Visible = false;
		StockAdjustmentRegister.Visible = false;
		frmPurchaseDemandRegister.Visible = false;
		frmPurchaseRegisterStore.Visible = false;
		StoreIssuanceReturnRegister.Visible = false;
		btnTradingReports.Visible = false;
		PurchaseInvoiceTrading.Visible = false;
		SaleInvoiceTrading.Visible = false;
		btnExportHistory.Visible = false;
		frmContainerList.Visible = false;
		SaleContractHistory.Visible = false;
		ExportInvoiceRegister.Visible = false;
		btnShipmentHistory.Visible = false;
		frmExImForwardingSlipandRegister.Visible = false;
		frmBillofLadingSlipandRegister.Visible = false;
		frmExportShipingLineBookingRpt.Visible = false;
		FrmRptExImShipmentScheduleHistroy.Visible = false;
		frmshippedConsignment_followup.Visible = false;
		ExImRptFrmAdvancePaymentReceiptAndUtilization.Visible = false;
		ExportCustomerWiseComparisonSummary.Visible = false;
		ExportComparisonSummary.Visible = false;
		ExportDetailHistory.Visible = false;
		frmExportSummaryRegister.Visible = false;
		ExImShipmentCosting.Visible = false;
		btnLabReports.Visible = false;
		InvLabPurchaseRegister.Visible = false;
		btnCargoReports.Visible = false;
		CbdReceivablesandReceiptsHistory.Visible = false;
		btnStockReport.Visible = false;
		frmStockReport.Visible = false;
		InventoryTransactionReport.Visible = false;
		RptGenrateStocks.Visible = false;
		InventoryTransactionReportStore.Visible = false;
		RptGenrateStocksStore.Visible = false;
		InventoryEvaluationItemLedger.Visible = false;
		btnEmptyBagsReports.Visible = false;
		frmStockWithSupplierHistory.Visible = false;
		productionReportsToolStripMenuItem.Visible = false;
		frmProductionPendingPurchaseInvoice.Visible = false;
		FoodProductionComparisonRpt.Visible = false;
		btnHRMReports.Visible = false;
		DailyAttendanceRpt.Visible = false;
		DailyLateandEarlyDeparture.Visible = false;
		genDutyRosterEmployeeWise.Visible = false;
		genEmployeeAttendence.Visible = false;
		MonthlyAttendanceSummary.Visible = false;
		EmployeeMonthlyRegister.Visible = false;
		btnPayrollSalaryReport.Visible = false;
		PayRollSalarySheetRpt.Visible = false;
		btnImportReports.Visible = false;
		ImportContractRegister.Visible = false;
		ImportPurchaseOrderRegister.Visible = false;
		ImportOrderPackingDetailRegister.Visible = false;
		ImportGrnRegister.Visible = false;
		ImportInvoiceRegister.Visible = false;
		btnInvDefinition.Visible = false;
		InvDefrmAddItem.Visible = false;
		AcfrmDefineLots.Visible = false;
		InvDeffrmWarehouse.Visible = false;
		InvfrmPriceSchedule.Visible = false;
		frmItemsCustomizedGroups.Visible = false;
		frmItemsReorderSchedule.Visible = false;
		frmPosdefineitem.Visible = false;
		btnPricingScheduleForGatePurchase.Visible = false;
		btnCustomerSale.Visible = false;
		SaleOrder.Visible = false;
		DeliveryOrder.Visible = false;
		OutwardGatePass.Visible = false;
		InvfrmGDN.Visible = false;
		InvfrmSaleInvoice.Visible = false;
		InvfrmSaleInvoiceReturn.Visible = false;
		InvfrmInvSaleInvoiceDirect.Visible = false;
		SaleInvoicepartyStockReserve.Visible = false;
		PointOfSaleInvoice.Visible = false;
		frmDriverBio.Visible = false;
		SaleOrderExport.Visible = false;
		InvfrmSaleInvoiceExport.Visible = false;
		GoodsDispatchNotesWithoutWeighBridge.Visible = false;
		frmSaleInvoiceDirect139.Visible = false;
		btnWeightBridge.Visible = false;
		frmWeightbridge.Visible = false;
		WeightbridgeMannual.Visible = false;
		InventoryReport.Visible = false;
		PurchaseOrderHistory.Visible = false;
		frmSaleOrderHistory.Visible = false;
		frmGDNHistory.Visible = false;
		frmGatePassReport.Visible = false;
		frmGPOutward.Visible = false;
		frmGRNHistory.Visible = false;
		frmSaleInvoiceHistory.Visible = false;
		frmWeightBridgeHistory.Visible = false;
		frmPurchaseInvoiceHistory.Visible = false;
		ItemLedger.Visible = false;
		frmRptItemList.Visible = false;
		frmDeliveryOrderForApprovel.Visible = false;
		frmSaleInvoiceDirectRegister.Visible = false;
		frmGatePassGeneral.Visible = false;
		frmSaleInvoiceReturnRegister.Visible = false;
		frmStockTranfserRegister.Visible = false;
		frmEvaulationDetailWagesReports.Visible = false;
		PurchaseRegisterNew.Visible = false;
		InventoryTransactionReport.Visible = false;
		btnTradingReports.Visible = false;
		PurchaseInvoiceTrading.Visible = false;
		SaleInvoiceTrading.Visible = false;
		SaleInvoiceRegisterNew.Visible = false;
		btnwages.Visible = false;
		frmContractorWagesAccount.Visible = false;
		frmContractWagesSchedule.Visible = false;
		frmwagesBillHeader.Visible = false;
		frmMoveOrder.Visible = false;
		btnDashBoard.Visible = false;
		AcFrmDashboard.Visible = false;
		PendingVouchersForApproval.Visible = false;
		btnExport.Visible = false;
		EximPerformaInvoice.Visible = false;
		EximLcOrder.Visible = false;
		EximInvoice.Visible = false;
		EximForwarding.Visible = false;
		EximBillOfLading.Visible = false;
		EximShippingBookingInfo.Visible = false;
		FrmExportSalesContract.Visible = false;
		FrmExportSalesContractSchedule.Visible = false;
		ExpfrmEformRegistration.Visible = false;
		ExportPartyDefines.Visible = false;
		ExportDeliveryOrder.Visible = false;
		EximPreProductionLab.Visible = false;
		ExpfrmFinancialInsturment.Visible = false;
		EximForwardingWithoutWeighBridge.Visible = false;
		btnExportForwardingDirect.Visible = false;
		btnExportHistory.Visible = false;
		btnShipmentHistory.Visible = false;
		frmBillofLadingSlipandRegister.Visible = false;
		frmExImForwardingSlipandRegister.Visible = false;
		frmExportShipingLineBookingRpt.Visible = false;
		FrmRptExImShipmentScheduleHistroy.Visible = false;
		frmshippedConsignment_followup.Visible = false;
		ExImRptFrmAdvancePaymentReceiptAndUtilization.Visible = false;
		btnServices.Visible = false;
		frmSrvCustPricingAgreement.Visible = false;
		frmSrvSaleOrderForwarder.Visible = false;
		frmServClearingAgentBill.Visible = false;
		ExImClearingAgentBillDirect.Visible = false;
		frmPreShipmentCosting.Visible = false;
		btndefineservices.Visible = false;
		btnServicesReport.Visible = false;
		ServicesBillRegister.Visible = false;
		fixedAssetsToolStripMenuItem.Visible = false;
		frmFixedAssetsCategory.Visible = false;
		frmFixedAssetRegister.Visible = false;
		frmFixedAssetRegisterHistory.Visible = false;
		btnCargoManagement.Visible = false;
		CargoBooking.Visible = false;
		FrmCBDHistory.Visible = false;
		ContainerAllocation.Visible = false;
		cbdBookingTracking.Visible = false;
		DestinationWareHouseReceipt.Visible = false;
		frmCargoDeliveryOrderAndBill.Visible = false;
		frmClearingBill.Visible = false;
		btnFinanceManagement.Visible = false;
		frmFinanceType.Visible = false;
		frmFMLoanRegistration.Visible = false;
		frmFMLoanAgreement.Visible = false;
		petroliumToolStripMenuItem.Visible = false;
		PurchaseInvoicePetrol.Visible = false;
		SaleInvoicePetrol.Visible = false;
		PurchaseInvoiceRegister.Visible = false;
		SaleInvoiceRegister.Visible = false;
		StockReport.Visible = false;
		btnStoreManagement.Visible = false;
		ItemCategoryStore.Visible = false;
		ItemTypeStore.Visible = false;
		AddItemStore.Visible = false;
		frmStoreOpeningStockBalancing.Visible = false;
		PurchaseOrderStore.Visible = false;
		frmPurchaseDemand.Visible = false;
		GatePassGeneral.Visible = false;
		GrnStore.Visible = false;
		PurchaseInvoiceStoreManagement.Visible = false;
		PurchaseInvoiceStoreWithTax.Visible = false;
		PurchaseInvoiceStoreWithOutTax.Visible = false;
		PurchaseInvoiceDirectStoreWithTax.Visible = false;
		frmPurchaseInvoiceDirectStore.Visible = false;
		StoreReturn.Visible = false;
		DeliveryOrderPackingMaterial.Visible = false;
		GatePassGeneralOutward.Visible = false;
		frmDepartmentRequest.Visible = false;
		frmGSIssuance.Visible = false;
		StoreIssuanceDirect.Visible = false;
		frmStockTransfer.Visible = false;
		frmPackingMaterialTransferPartyToParty.Visible = false;
		FrmSaleInvoiceDirectPackingMaterial.Visible = false;
		frmSaleInvoiceDirectWithOutTax.Visible = false;
		frmSaleInvoiceDirectWithTax.Visible = false;
		btnStockWare.Visible = false;
		frmProductionJobOrder.Visible = false;
		frmFoodProduction.Visible = false;
		invfrmStockConversionProduction.Visible = false;
		frmBillOfMaterial.Visible = false;
		FoodProductionWithValues.Visible = false;
		BillOfMaterialWiseProduction.Visible = false;
		frmBillOfMaterialManufacturing.Visible = false;
		frmProductionCumPackMaterialConsumption.Visible = false;
		ProductionPreCostingAndJobOrder.Visible = false;
		frmBoilerConsumption.Visible = false;
		btnTaxation.Visible = false;
		InvfrmAddTaxType.Visible = false;
		InvfrmAddTaxSchedule.Visible = false;
		InvDeffrmItemTaxSchdule.Visible = false;
		frmTaxNotesAndGLMaping.Visible = false;
		frmSupplierCustomerExemptionSchedule.Visible = false;
		SaleTaxSummary.Visible = false;
		btnStoreManagement.Visible = false;
		AddItemStore.Visible = false;
		frmStoreOpeningStockBalancing.Visible = false;
		PurchaseOrderStore.Visible = false;
		frmPurchaseDemand.Visible = false;
		GatePassGeneral.Visible = false;
		GrnStore.Visible = false;
		frmDepartmentRequest.Visible = false;
		DeliveryOrderPackingMaterial.Visible = false;
		GatePassGeneralOutward.Visible = false;
		frmStockTransfer.Visible = false;
		frmPurchaseInvoiceDirectStore.Visible = false;
		StoreIssuanceDirect.Visible = false;
		frmStockAdjustment.Visible = false;
		btnStockReport.Visible = false;
		frmStockReport.Visible = false;
		InventoryTransactionReportStore.Visible = false;
		ItemLedgerReport_Inventory.Visible = false;
		StockReportWithAvgRates.Visible = false;
		frmInventoryProfitLoss.Visible = false;
		pdcReprotsToolStripMenuItem.Visible = false;
		productionReportsToolStripMenuItem.Visible = false;
		frmProductionPendingPurchaseInvoice.Visible = false;
		FoodProductionComparisonRpt.Visible = false;
		btnEmptyBagsReports.Visible = false;
		frmStockWithSupplierHistory.Visible = false;
		btnHRM.Visible = false;
		btnProfileManagement.Visible = false;
		GeneralSetting.Visible = false;
		EmployeeCategory.Visible = false;
		Benefit.Visible = false;
		Designation.Visible = false;
		Department.Visible = false;
		Section.Visible = false;
		Location.Visible = false;
		ProfileType.Visible = false;
		Profile.Visible = false;
		EmployeeGroup.Visible = false;
		btnPolicyManegement.Visible = false;
		GeneralPolicy.Visible = false;
		SalaryBreakupPolicy.Visible = false;
		LeaveQuotaPolicy.Visible = false;
		EOBIPolicy.Visible = false;
		ProvidentFundPolicy.Visible = false;
		SocialSecurityPolicy.Visible = false;
		IncomeTaxPolicy.Visible = false;
		btnEmployeeManagement.Visible = false;
		TrialRegistration.Visible = false;
		EmployeeRegistration.Visible = false;
		EmployeeNotice.Visible = false;
		EmployeeApproval.Visible = false;
		ProbationApproval.Visible = false;
		btnDeviceManagement.Visible = false;
		DeviceConfiguration.Visible = false;
		PullAttendance.Visible = false;
		PullAttendanceByMachine.Visible = false;
		btnAttendanceManagement.Visible = false;
		DefineShift.Visible = false;
		ShiftLocation.Visible = false;
		ShiftTiming.Visible = false;
		Holiday.Visible = false;
		DutyRoaster.Visible = false;
		EmployeeRoaster.Visible = false;
		DailyAttendance.Visible = false;
		ManualAttendance.Visible = false;
		btnLeaveManagement.Visible = false;
		LeaveOpening.Visible = false;
		EmployeeLeave.Visible = false;
		EmployeeSpecialLeave.Visible = false;
		EmployeeShortLeave.Visible = false;
		LeaveClosing.Visible = false;
		btnCPLLeaveMangement.Visible = false;
		frmEmployeeCPLLeaveOpening.Visible = false;
		btnCPLAttendance.Visible = false;
		btnCPLRequest.Visible = false;
		btnLoanManagement.Visible = false;
		EmployeeLoan.Visible = false;
		EmployeeAdvance.Visible = false;
		btnOverTimeManagement.Visible = false;
		OverTimeRequest.Visible = false;
		OTRequestApproval.Visible = false;
		EmployeeOverTime.Visible = false;
		btnApprovalManagement.Visible = false;
		LoanApproval.Visible = false;
		LeaveApproval.Visible = false;
		AdvanceApproval.Visible = false;
		OverTimeApproval.Visible = false;
		ShortLeaveApproval.Visible = false;
		AttendanceApproval.Visible = false;
		ShortHourApproval.Visible = false;
		AllowanceApproval.Visible = false;
		LoanDeductionApproval.Visible = false;
		frmCPLApproval.Visible = false;
		btnPayroll.Visible = false;
		PayrollPosting.Visible = false;
		OvertimePosting.Visible = false;
		DailyWagesPosting.Visible = false;
		EmployeeAllowance.Visible = false;
		EmployeeLateAdjustment.Visible = false;
		EmployeeLoanDeduction.Visible = false;
		EmployeeShortAdjustment.Visible = false;
		btnGlobalEmployment.Visible = false;
		EmployeePromotion.Visible = false;
		EmployeeIncrement.Visible = false;
		EmployeeTransfer.Visible = false;
		FullandFinalSettlement.Visible = false;
		BlackListed.Visible = false;
		btnHRMReports.Visible = false;
		DailyAttendanceRpt.Visible = false;
		DailyLateandEarlyDeparture.Visible = false;
		genDutyRosterEmployeeWise.Visible = false;
		genEmployeeAttendence.Visible = false;
		MonthlyAttendanceSummary.Visible = false;
		PayRollSalarySheetRpt.Visible = false;
		btnImport.Visible = false;
		frmImpPerformaInvoice.Visible = false;
		frmImpLcContract.Visible = false;
		frmImpLcPurchaseContract.Visible = false;
		frmImpPurchaseOrder.Visible = false;
		frmImportOrderPackingDetail.Visible = false;
		frmImpGrnForwarding.Visible = false;
		frmImpInvoice.Visible = false;
		frmImpGrnDirect.Visible = false;
		btnBillofLadingImport.Visible = false;
		frmImportCommercialInvoicebyContract.Visible = false;
		btnImportReports.Visible = false;
		ImportContractRegister.Visible = false;
		ImportPurchaseOrderRegister.Visible = false;
		ImportOrderPackingDetailRegister.Visible = false;
		ImportGrnRegister.Visible = false;
		ImportInvoiceRegister.Visible = false;
		storeManagmentToolStripMenuItem.Visible = false;
		PurchsaeOrderTrade.Visible = false;
		frmPurchaseOrderWithTax.Visible = false;
		frmSupplySchedule.Visible = false;
		InwardGatePassTrade.Visible = false;
		InvFrmGRNTrade.Visible = false;
		purchaseInvoiceToolStripMenuItem.Visible = false;
		btnSaleTrading.Visible = false;
		SaleOrderTrade.Visible = false;
		frmDeliverySchedule.Visible = false;
		FrmDeliveryChallan.Visible = false;
		saleInvoiceToolStripMenuItem.Visible = false;
		PartyProcessing.Visible = false;
		GatePassInwardPartyProcessing.Visible = false;
		GatePassOutwardParyProcessing.Visible = false;
		frmPartyProcessingDefineSupplier.Visible = false;
		DefineReferenceParties.Visible = false;
		DefItemCatagoryPartyProcessing.Visible = false;
		DefPartyProcessingItemType.Visible = false;
		DefPartProcessingAddItem.Visible = false;
		GoodsReceivingNotesPartyProcessing.Visible = false;
		GoodsDispatchNotesPartyProcessing.Visible = false;
		GRNStorePartyProcessing.Visible = false;
		GDNStorePartyProcessing.Visible = false;
		frmProductionPartyProcessing.Visible = false;
		ProductionJobOrderPartyProcessing.Visible = false;
		StockOpeningBalancePartyProcessing.Visible = false;
		ProductionPartyProcessingBill.Visible = false;
		btnPartyProcessingReports.Visible = false;
		frmGatePassPartyProcessing.Visible = false;
		frmGRNPartyProcessingRegister.Visible = false;
		grmGDNPartyProcessingRegister.Visible = false;
		frmStockReportPartyProcessing.Visible = false;
		frmPartyProcessingGrnInfo.Visible = false;
		btnAccountLookupa.Visible = false;
		btnSupplierDefineSystemUtilities.Visible = false;
		btnPlAndBsSettings.Visible = false;
		DateLock.Visible = false;
		btnSMSContacts.Visible = false;
		btnScreenMessageReceivers.Visible = false;
		btnHistory.Visible = false;
		invLookUpsToolStripMenuItem.Visible = false;
		CommissionAgentTrade.Visible = false;
		frmCommissionAgentOrder.Visible = false;
		frmCommissionAgentTradeBill.Visible = false;
		CommissionAgentTradeBill_162.Visible = false;
	}

	private void PartyProcessingReports(List<tblUserRights> lst)
	{
		try
		{
			List<tblUserRights> newList = lst.Where((tblUserRights x) => x.ModuleDescription == "Party Processing Reports" && x.Value).ToList();
			if (newList.Count > 0)
			{
				btnPartyProcessingReports.Visible = true;
				newList = lst.Where((tblUserRights x) => x.ScreenName.Trim() == "frmGatePassPartyProcessing").ToList();
				frmGatePassPartyProcessing.Visible = bool.Parse(newList[0].Value.ToString());
				newList = lst.Where((tblUserRights x) => x.ScreenName.Trim() == "frmGRNPartyProcessingRegister").ToList();
				frmGRNPartyProcessingRegister.Visible = bool.Parse(newList[0].Value.ToString());
				newList = lst.Where((tblUserRights x) => x.ScreenName.Trim() == "grmGDNPartyProcessingRegister").ToList();
				grmGDNPartyProcessingRegister.Visible = bool.Parse(newList[0].Value.ToString());
				newList = lst.Where((tblUserRights x) => x.ScreenName.Trim() == "frmStockReportPartyProcessing").ToList();
				frmStockReportPartyProcessing.Visible = bool.Parse(newList[0].Value.ToString());
				newList = lst.Where((tblUserRights x) => x.ScreenName.Trim() == "frmPartyProcessingGrnInfo").ToList();
				frmPartyProcessingGrnInfo.Visible = bool.Parse(newList[0].Value.ToString());
			}
		}
		catch
		{
		}
	}

	private void btnSettings_MouseHover(object sender, EventArgs e)
	{
		contextMenuStrip3.Show(btnSettings, 0, btnSettings.Height);
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		contextMenuStrip4.Show(btnRefresh, 0, btnRefresh.Height);
	}

	private void btnMenuButton_MouseHover(object sender, EventArgs e)
	{
		ContextMenuStrip1.Show(btnMenuButton, 0, btnMenuButton.Height);
	}

	private void DashboardNew_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			if (!closingForUpdate)
			{
				if (MessageBox.Show("Close the window?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					_cts.Cancel();
					versionTimer.Stop();
					timer2.Stop();
					Environment.Exit(0);
				}
				else
				{
					e.Cancel = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnFavorits_MouseHover(object sender, EventArgs e)
	{
		ContextMenuStrip1.Show(btnMenuButton, 0, btnMenuButton.Height);
	}

	private void timer2_Tick(object sender, EventArgs e)
	{
		try
		{
			DateTime dateTime = DateTime.Now;
			lblTime.Text = dateTime.ToLongTimeString().ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DashboardNew_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			if (e.KeyData == Keys.Return && ((UltraGridBase)CmbNavigation).ActiveRow != null)
			{
				string TargetUrl = Conversion.ToString(CmbNavigation.Value);
				if (Type.GetType(TargetUrl) != null)
				{
					UserAccount.UserAccountAllocationsList = new List<UserAccountAllocation>();
					UserAccountAllocation allocation = new UserAccountAllocation();
					allocation.ScreenName = Conversion.ToString(((UltraDropDownBase)CmbNavigation).SelectedRow.Cells[2].Value);
					UserAccount.UserAccountAllocationsList.Add(allocation);
					if (Activator.CreateInstance(Type.GetType(TargetUrl), UserAccount) is Form form)
					{
						form.Text = ((Control)(object)CmbNavigation).Text;
						form.Tag = Conversion.ToString(((UltraDropDownBase)CmbNavigation).SelectedRow.Cells[2].Value);
						form.TopLevel = false;
						panel2.Controls.Add(form);
						HistoryStack.Push(form);
						form.Show();
						((Control)(object)CmbNavigation).Text = string.Empty;
						((UltraGridBase)CmbNavigation).Rows[0].Activate();
					}
				}
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.N)
			{
				CmbNavigation.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F)
			{
				btnFavourites.Focus();
				btnFavourites.TabIndex = 3;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnHome_Click(object sender, EventArgs e)
	{
		try
		{
			if (UserAccount.AppId != 5)
			{
				frmModules objForm = new frmModules(UserAccount, this);
				objForm.TopLevel = false;
				panel2.Dock = DockStyle.Fill;
				panel2.Controls.Add(objForm);
				HistoryStack.Push(objForm);
				objForm.Dock = DockStyle.Fill;
				objForm.Show();
			}
			else if (!panel2.Controls.Cast<Control>().Any((Control c) => c.Name == "BookingOfficeDashboard"))
			{
				BookingOfficeDashboard objForm2 = new BookingOfficeDashboard(UserAccount);
				objForm2.TopLevel = false;
				panel2.Dock = DockStyle.Fill;
				panel2.Controls.Add(objForm2);
				HistoryStack.Push(objForm2);
				objForm2.Dock = DockStyle.Fill;
				objForm2.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnFavourites_Click(object sender, EventArgs e)
	{
		contextMenuStrip5.Show(btnFavourites, 0, btnFavourites.Height);
	}

	private void lblusername_MouseHover(object sender, EventArgs e)
	{
		lblusername.ForeColor = Color.Blue;
	}

	private void lblusername_MouseLeave(object sender, EventArgs e)
	{
		lblusername.ForeColor = Color.White;
	}

	private void lblusername_Click(object sender, EventArgs e)
	{
		try
		{
			clsGlobalVariables.ScreenViewReights = GetViewRightsByUserId();
			InitializeMenu();
			ScreenBindInNavigation();
			DashBoardThemeSetting();
			if (UserAccount.AppId != 5)
			{
				frmModules objForm = new frmModules(UserAccount, this);
				objForm.TopLevel = false;
				panel2.Dock = DockStyle.Fill;
				panel2.Controls.Add(objForm);
				HistoryStack.Push(objForm);
				objForm.Dock = DockStyle.Fill;
				objForm.Show();
			}
			else if (!panel2.Controls.Cast<Control>().Any((Control c) => c.Name == "BookingOfficeDashboard"))
			{
				BookingOfficeDashboard objForm2 = new BookingOfficeDashboard(UserAccount);
				objForm2.TopLevel = false;
				panel2.Dock = DockStyle.Fill;
				panel2.Controls.Add(objForm2);
				HistoryStack.Push(objForm2);
				objForm2.Dock = DockStyle.Fill;
				objForm2.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnBack_Click(object sender, EventArgs e)
	{
		try
		{
			if (HistoryStack.CanNavigateBackward())
			{
				Form currentForm = HistoryStack.GetCurrentForm();
				Form previousForm = HistoryStack.GetPrevious();
				if (previousForm != null)
				{
					currentForm.Hide();
					previousForm.TopLevel = false;
					panel2.Controls.Add(previousForm);
					previousForm.BringToFront();
					previousForm.Show();
				}
				return;
			}
			throw new Exception("You Can't Go Back Further");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNext_Click(object sender, EventArgs e)
	{
		try
		{
			if (HistoryStack.CanNavigateForward())
			{
				Form currentForm = HistoryStack.GetCurrentForm();
				Form nextForm = HistoryStack.GetNext();
				if (nextForm != null)
				{
					currentForm.Hide();
					panel2.Controls.Add(nextForm);
					nextForm.TopLevel = false;
					nextForm.BringToFront();
					nextForm.Show();
				}
				return;
			}
			throw new Exception("You Can't Go Next Further");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNavigateToForms_Click(object sender, EventArgs e)
	{
		contextMenuStrip6.Items.Clear();
		HistoryOfFormsMenu();
		contextMenuStrip6.Show(btnNavigateToForms, 0, btnNavigateToForms.Height);
	}

	private void btnNavigateToForms_MouseHover(object sender, EventArgs e)
	{
		contextMenuStrip6.Items.Clear();
		HistoryOfFormsMenu();
		contextMenuStrip6.Show(btnNavigateToForms, 0, btnNavigateToForms.Height);
	}

	private void HistoryOfFormsMenu()
	{
		try
		{
			List<Form> lstForms = (from form in HistoryStack.history.AsEnumerable()
				where form.Text != "Modules" && form.Text != "Screens"
				select form).ToList();
			if (lstForms.Count <= 0)
			{
				return;
			}
			ToolStripMenuItem[] arr = new ToolStripMenuItem[lstForms.Count];
			int counter = 0;
			foreach (Form mnu in lstForms)
			{
				ToolStripMenuItem tsMenu = new ToolStripMenuItem();
				tsMenu.Size = new Size(180, 22);
				tsMenu.Text = mnu.Text;
				tsMenu.Tag = mnu.Text;
				arr[counter] = tsMenu;
				counter++;
				tsMenu.Click += HistoryFormsItem_Click;
				contextMenuStrip6.Items.Add(tsMenu);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryFormsItem_Click(object sender, EventArgs e)
	{
		try
		{
			ToolStripMenuItem getMenu = (ToolStripMenuItem)sender;
			Form obj = HistoryStack.history.Find((Form form) => form.Text == getMenu.Text);
			if (obj == null)
			{
				throw new Exception("Exception");
			}
			obj.WindowState = FormWindowState.Maximized;
			obj.TopLevel = false;
			panel2.Controls.Add(obj);
			obj.BringToFront();
			obj.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAuditLogReport_Click(object sender, EventArgs e)
	{
		try
		{
			AuditLogReport obj = new AuditLogReport(UserAccount);
			obj.TopLevel = false;
			obj.AutoScroll = true;
			panel2.Controls.Add(obj);
			obj.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void licenseKeyToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			LicenseKey obj = new LicenseKey(UserAccount);
			obj.TopLevel = false;
			obj.AutoScroll = true;
			obj.WindowState = FormWindowState.Maximized;
			panel2.Controls.Add(obj);
			obj.BringToFront();
			obj.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnScreenDefination_Click(object sender, EventArgs e)
	{
	}

	private void btnScreenRights_Click(object sender, EventArgs e)
	{
		UserRightsByCompany userRightsBy = new UserRightsByCompany(UserAccount);
		userRightsBy.TopLevel = false;
		userRightsBy.AutoScroll = true;
		panel2.Controls.Add(userRightsBy);
		userRightsBy.Show();
	}

	private void btnadminpaneluserright_Click(object sender, EventArgs e)
	{
		frmUserRights userRights = new frmUserRights(UserAccount);
		userRights.TopLevel = false;
		userRights.AutoScroll = true;
		panel2.Controls.Add(userRights);
		userRights.Show();
	}

	private void DefineReports_Click(object sender, EventArgs e)
	{
		frmReportConfig obj = new frmReportConfig(UserAccount);
		obj.TopLevel = false;
		obj.AutoScroll = true;
		panel2.Controls.Add(obj);
		obj.Show();
	}

	private void btnadminpanelsysconfig_Click(object sender, EventArgs e)
	{
		Architecture.WinApp.Configurations.Configuration configuration = new Architecture.WinApp.Configurations.Configuration(UserAccount);
		configuration.TopLevel = false;
		configuration.AutoScroll = true;
		panel2.Controls.Add(configuration);
		configuration.Show();
	}

	private void btnPlBSNotes_Click(object sender, EventArgs e)
	{
		try
		{
			BsPlSettingForm obj = new BsPlSettingForm(UserAccount);
			obj.TopLevel = false;
			obj.AutoScroll = true;
			panel2.Controls.Add(obj);
			obj.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void companyProfileToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			frmCompanyProfile obj = new frmCompanyProfile(UserAccount);
			obj.TopLevel = false;
			obj.AutoScroll = true;
			panel2.Controls.Add(obj);
			obj.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnStock_Reconcilation_Click(object sender, EventArgs e)
	{
		StockReconcilation obj = new StockReconcilation(UserAccount);
		obj.TopLevel = false;
		obj.AutoScroll = true;
		panel2.Controls.Add(obj);
		obj.Show();
	}

	private void btnDatabaseBackup_Click_1(object sender, EventArgs e)
	{
		BackUpDatabase obj = new BackUpDatabase(UserAccount);
		obj.TopLevel = false;
		obj.AutoScroll = true;
		panel2.Controls.Add(obj);
		obj.Show();
	}

	private void btnReport_MouseHover(object sender, EventArgs e)
	{
		contextMenuStrip2.Show(btnReport, 0, btnReport.Height);
	}

	private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
	{
		frmChangePassword obj = new frmChangePassword(UserAccount);
		obj.TopLevel = false;
		obj.AutoScroll = true;
		panel2.Controls.Add(obj);
		obj.BringToFront();
		obj.WindowState = FormWindowState.Normal;
		obj.StartPosition = FormStartPosition.CenterScreen;
		obj.Show();
	}

	private void lockToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			if (MessageBox.Show("Close the window?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CheckUpdate()
	{
		try
		{
			if (isDownloading)
			{
				return;
			}
			string urlVersion = getServerPath() + "/updates.txt";
			string newVersionString;
			using (WebClient client = new WebClient())
			{
				newVersionString = client.DownloadString(urlVersion).Trim();
			}
			new Version(newVersionString);
			string currentVersion = Application.ProductVersion;
			bool isUpdateAvailable = CompareVersions(newVersionString, currentVersion);
			Invoke((MethodInvoker)delegate
			{
				if (isUpdateAvailable)
				{
					label1.Text = "New Update is Available! Current Version: " + currentVersion;
					label1.ForeColor = Color.Yellow;
					btnUpdate.Visible = true;
				}
				else
				{
					label1.Text = "Current Version: " + currentVersion;
					label1.ForeColor = Color.White;
					btnUpdate.Visible = false;
				}
			});
		}
		catch (WebException)
		{
			Invoke((MethodInvoker)delegate
			{
				label1.Text = "Current Version: " + Application.ProductVersion;
				label1.ForeColor = Color.White;
				btnUpdate.Visible = false;
			});
		}
		catch (Exception)
		{
			Invoke((MethodInvoker)delegate
			{
				label1.Text = "Current Version: " + Application.ProductVersion;
				label1.ForeColor = Color.White;
				btnUpdate.Visible = false;
			});
		}
	}

	private bool CompareVersions(string newVersionString, string currentVersionString)
	{
		string[] newVersionComponents = newVersionString.Split('.');
		string[] currentVersionComponents = currentVersionString.Split('.');
		for (int i = 0; i < Math.Max(newVersionComponents.Length, currentVersionComponents.Length); i++)
		{
			int newComponent = ((i < newVersionComponents.Length) ? int.Parse(newVersionComponents[i]) : 0);
			int currentComponent = ((i < currentVersionComponents.Length) ? int.Parse(currentVersionComponents[i]) : 0);
			if (newComponent > currentComponent)
			{
				return true;
			}
			if (newComponent < currentComponent)
			{
				return false;
			}
		}
		return false;
	}

	private void bw_CheckDownload_DoWork(object sender, DoWorkEventArgs e)
	{
	}

	private void bw_CheckDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
	}

	private void Web_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
	{
		isDownloading = false;
		if (e.Error == null)
		{
			MessageBox.Show("Download completed. The application will now close for the update.");
			Process.Start("C:\\SCS\\setup.msi");
			closingForUpdate = true;
			Application.Exit();
			Environment.Exit(1);
		}
		else
		{
			MessageBox.Show("Error downloading the update: " + e.Error.Message);
		}
	}

	private void Web_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
	{
		Invoke((MethodInvoker)delegate
		{
			label1.Text = $"Downloading Updates: {e.ProgressPercentage}%";
			label1.ForeColor = Color.LimeGreen;
		});
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
	}

	private void bw_CheckDownload_DoWork_1(object sender, DoWorkEventArgs e)
	{
		try
		{
			CheckUpdate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void bw_CheckDownload_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
	{
		if (!isDownloading)
		{
			bw_CheckDownload.RunWorkerAsync();
		}
	}

	private void bw_CheckDownload_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
	}

	public string getServerPath()
	{
		string connStringReportPath = ConfigurationManager.ConnectionStrings["UpdatePath"].ConnectionString;
		if (!string.IsNullOrEmpty(connStringReportPath))
		{
			if (!connStringReportPath.EndsWith("/"))
			{
				connStringReportPath += "/";
			}
			return connStringReportPath;
		}
		throw new InvalidOperationException("The server path configuration is not set.");
	}

	private void btnUpdate_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (!isDownloading)
			{
				MessageBox.Show("Downloading Started.");
				btnUpdate.Visible = false;
				isDownloading = true;
				WebClient webClient = new WebClient();
				webClient.DownloadFileCompleted += Web_DownloadFileCompleted;
				webClient.DownloadProgressChanged += Web_DownloadProgressChanged;
				webClient.DownloadFileAsync(new Uri(getServerPath() + "/setup.msi"), "C:\\SCS\\setup.msi");
			}
		}
		catch (Exception ex)
		{
			btnUpdate.Visible = true;
			MessageBox.Show("Error initiating update download: " + ex.Message);
			isDownloading = false;
		}
	}

	private async void label1_Click(object sender, EventArgs e)
	{
		try
		{
			string apiUrl = "http://192.168.1.117:125/api/DBUService/Start";
			string apiKey = "8f155e082d363b552d235937f9132b880f1602081136cadb02f17bce75cb90d2";
			CallApiUsingHttpWebRequest(apiUrl, apiKey);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error initiating update download: " + ex.Message);
		}
	}

	public string CallApiUsingHttpWebRequest(string apiUrl, string apiKey)
	{
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
		request.Method = "GET";
		request.Headers.Add("APIKey", apiKey);
		try
		{
			using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			using StreamReader reader = new StreamReader(response.GetResponseStream());
			return reader.ReadToEnd();
		}
		catch (WebException ex)
		{
			MessageBox.Show(ex.Message);
			return null;
		}
	}

	public void DashBoardThemeSetting()
	{
		try
		{
			clsGlobalVariables.GetThemeSettings = ThemeSettings.GetThemeSettingsByScreenList(UserAccount.OrganizationId, UserAccount.CompanyId, 0, (string)null);
			ThemeSettings themeSettings = clsGlobalVariables.GetThemeSettings.FirstOrDefault((ThemeSettings row) => row.ScreenName == "DashBoardNew");
			if (themeSettings == null)
			{
				return;
			}
			panel1.BackColor = ColorTranslator.FromHtml(themeSettings.HeaderPanel_BackColor);
			panel1.ForeColor = ColorTranslator.FromHtml(themeSettings.HeaderPanel_FontColor);
			if (panel1.HasChildren)
			{
				foreach (Control control in panel1.Controls)
				{
					control.BackColor = ColorTranslator.FromHtml(themeSettings.HeaderPanel_ControlsBackColor);
					control.ForeColor = ColorTranslator.FromHtml(themeSettings.HeaderPanel_ControlsFontColor);
				}
			}
			panel3.BackColor = ColorTranslator.FromHtml(themeSettings.HeaderPanel_BackColor);
			panel3.ForeColor = ColorTranslator.FromHtml(themeSettings.HeaderPanel_FontColor);
			if (!panel3.HasChildren)
			{
				return;
			}
			foreach (Control control2 in panel3.Controls)
			{
				control2.BackColor = ColorTranslator.FromHtml(themeSettings.HeaderPanel_ControlsBackColor);
				control2.ForeColor = ColorTranslator.FromHtml(themeSettings.HeaderPanel_ControlsFontColor);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void sQLQueryExecuterToolStripMenuItem_Click(object sender, EventArgs e)
	{
		new SqlLogin().ShowDialog();
	}

	private void BtnViewRightsReport_Click(object sender, EventArgs e)
	{
		try
		{
			string ScreenName = "";
			foreach (Control control in panel2.Controls)
			{
				if (control is Form openForm)
				{
					ScreenName = openForm.Tag?.ToString();
					if (!string.IsNullOrEmpty(ScreenName))
					{
						break;
					}
				}
			}
			if (string.IsNullOrEmpty(ScreenName))
			{
				return;
			}
			tblUserRights obj1 = clsGlobalVariables.ScreenViewReights.Find((tblUserRights x) => x.ScreenName == ScreenName);
			if (obj1 != null)
			{
				if (obj1 != null && obj1.TargetUrl == null)
				{
					throw new Exception("Target URL not Configured properly... " + obj1.ScreenAlias);
				}
				if (obj1.ModuleTypeId == 2)
				{
					UserRightsByCompany userRightsByCompany = new UserRightsByCompany(UserAccount);
					userRightsByCompany.tabControl1.SelectedIndex = 3;
					userRightsByCompany.Show();
					((Control)(object)userRightsByCompany.CmbScreensTab4).Text = obj1.ScreenAlias;
					userRightsByCompany.btnShowTab4_Click(null, null);
				}
				else
				{
					UserRightsByCompany userRightsByCompany2 = new UserRightsByCompany(UserAccount);
					userRightsByCompany2.tabControl1.SelectedIndex = 2;
					userRightsByCompany2.Show();
					((Control)(object)userRightsByCompany2.CmbScreensTab3).Text = obj1.ScreenAlias;
					userRightsByCompany2.btnShowTab3_Click(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnWeighBridge_WeightUpdate_Click(object sender, EventArgs e)
	{
		WeighBridge_WeightUpdate WbUpdate = new WeighBridge_WeightUpdate(UserAccount);
		WbUpdate.TopLevel = false;
		WbUpdate.AutoScroll = true;
		panel2.Controls.Add(WbUpdate);
		WbUpdate.Show();
	}

	private void btnUserProfile_Click(object sender, EventArgs e)
	{
		UserProfileInfo obj = new UserProfileInfo();
		obj.TopLevel = false;
		obj.AutoScroll = true;
		panel2.Controls.Add(obj);
		obj.BringToFront();
		obj.WindowState = FormWindowState.Normal;
		obj.Show();
	}

	private void BtnThemeForm_Click(object sender, EventArgs e)
	{
		ThemeSelectionForm obj = new ThemeSelectionForm(UserAccount);
		obj.TopLevel = false;
		obj.AutoScroll = true;
		panel2.Controls.Add(obj);
		obj.BringToFront();
		obj.WindowState = FormWindowState.Normal;
		obj.Show();
	}

	private void btnAccountMovement_Click(object sender, EventArgs e)
	{
		try
		{
			AccountToAccountTransfer obj = new AccountToAccountTransfer(UserAccount);
			obj.StartPosition = FormStartPosition.CenterScreen;
			obj.TopLevel = false;
			obj.AutoScroll = true;
			panel2.Controls.Add(obj);
			obj.BringToFront();
			obj.WindowState = FormWindowState.Normal;
			obj.Show();
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.DashboardNew));
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnUpdate = new System.Windows.Forms.PictureBox();
		this.lblFinancialYear = new System.Windows.Forms.Label();
		this.CmbNavigation = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.lblTime = new System.Windows.Forms.Label();
		this.lblBranchName = new System.Windows.Forms.Label();
		this.lblusername = new System.Windows.Forms.Label();
		this.pnlmain = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.BtnViewRightsReport = new System.Windows.Forms.Button();
		this.btnNavigateToForms = new System.Windows.Forms.Button();
		this.btnNext = new System.Windows.Forms.Button();
		this.btnBack = new System.Windows.Forms.Button();
		this.btnFavourites = new System.Windows.Forms.Button();
		this.btnHome = new System.Windows.Forms.Button();
		this.btnRefresh = new System.Windows.Forms.PictureBox();
		this.btnSettings = new System.Windows.Forms.Button();
		this.btnReport = new System.Windows.Forms.Button();
		this.btnMenuButton = new System.Windows.Forms.Button();
		this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.btnAccountDefinition = new System.Windows.Forms.ToolStripMenuItem();
		this.AcfrmDefCoa = new System.Windows.Forms.ToolStripMenuItem();
		this.AcfrmAcAllocation = new System.Windows.Forms.ToolStripMenuItem();
		this.UpdateCoAllocation = new System.Windows.Forms.ToolStripMenuItem();
		this.AcfrmOpeningBalance = new System.Windows.Forms.ToolStripMenuItem();
		this.AccountsCustomGroup = new System.Windows.Forms.ToolStripMenuItem();
		this.AccountBudget = new System.Windows.Forms.ToolStripMenuItem();
		this.AcfrmChequebookRegistration = new System.Windows.Forms.ToolStripMenuItem();
		this.AcfrmDefineBank = new System.Windows.Forms.ToolStripMenuItem();
		this.frmAdvanceAdjustment = new System.Windows.Forms.ToolStripMenuItem();
		this.frmIncentivePolicy = new System.Windows.Forms.ToolStripMenuItem();
		this.InLandFreightAgreement = new System.Windows.Forms.ToolStripMenuItem();
		this.btnAccountsTransaction = new System.Windows.Forms.ToolStripMenuItem();
		this.frmAccountsPrematureReceipts = new System.Windows.Forms.ToolStripMenuItem();
		this.ExpenseVoucher = new System.Windows.Forms.ToolStripMenuItem();
		this.frmCashPaymentVoucher = new System.Windows.Forms.ToolStripMenuItem();
		this.frmBankPaymentVoucher = new System.Windows.Forms.ToolStripMenuItem();
		this.PaymentVoucherNew = new System.Windows.Forms.ToolStripMenuItem();
		this.frmCashReceiptVoucher = new System.Windows.Forms.ToolStripMenuItem();
		this.frmBankReceiptVoucher = new System.Windows.Forms.ToolStripMenuItem();
		this.ReceiptsVoucherNew = new System.Windows.Forms.ToolStripMenuItem();
		this.ContraVoucher = new System.Windows.Forms.ToolStripMenuItem();
		this.VoucherEntry = new System.Windows.Forms.ToolStripMenuItem();
		this.frmWhtTaxChallanDeposit = new System.Windows.Forms.ToolStripMenuItem();
		this.frmDayBook = new System.Windows.Forms.ToolStripMenuItem();
		this.FreightVoucher = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.frmBillsPayables = new System.Windows.Forms.ToolStripMenuItem();
		this.frmBillsReceivables = new System.Windows.Forms.ToolStripMenuItem();
		this.PaymentByInvoiceVoucherNew = new System.Windows.Forms.ToolStripMenuItem();
		this.PaymentByInvoiceAccount = new System.Windows.Forms.ToolStripMenuItem();
		this.ReceiptByInvoiceAccount = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.AcfrmDefPdcManagment = new System.Windows.Forms.ToolStripMenuItem();
		this.PostDatedCheqPaymentVouchers = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.Acfrmfcbankreceipt = new System.Windows.Forms.ToolStripMenuItem();
		this.frmFcyPaymentVoucher = new System.Windows.Forms.ToolStripMenuItem();
		this.frmExportInvoiceVoucher = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.StoreIssuanceFinancial = new System.Windows.Forms.ToolStripMenuItem();
		this.AccountsReconcilation = new System.Windows.Forms.ToolStripMenuItem();
		this.frmCreditNotes = new System.Windows.Forms.ToolStripMenuItem();
		this.btnInvDefinition = new System.Windows.Forms.ToolStripMenuItem();
		this.InvDefrmAddItem = new System.Windows.Forms.ToolStripMenuItem();
		this.AcfrmDefineLots = new System.Windows.Forms.ToolStripMenuItem();
		this.InvDeffrmWarehouse = new System.Windows.Forms.ToolStripMenuItem();
		this.btnPricingScheduleForGatePurchase = new System.Windows.Forms.ToolStripMenuItem();
		this.InvfrmPriceSchedule = new System.Windows.Forms.ToolStripMenuItem();
		this.frmItemsCustomizedGroups = new System.Windows.Forms.ToolStripMenuItem();
		this.frmItemsReorderSchedule = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPosdefineitem = new System.Windows.Forms.ToolStripMenuItem();
		this.btnSupPurchase = new System.Windows.Forms.ToolStripMenuItem();
		this.PaddyPurchase = new System.Windows.Forms.ToolStripMenuItem();
		this.frmGatePassInwardWithDetail = new System.Windows.Forms.ToolStripMenuItem();
		this.frmGrnForGate = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPurchaseInvoiceForGate = new System.Windows.Forms.ToolStripMenuItem();
		this.supfrmDefineSupplier = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchsaeOrder = new System.Windows.Forms.ToolStripMenuItem();
		this.MarketPurchaseOrder = new System.Windows.Forms.ToolStripMenuItem();
		this.SupplyOrderPurchase = new System.Windows.Forms.ToolStripMenuItem();
		this.InwardGatePass = new System.Windows.Forms.ToolStripMenuItem();
		this.InvFrmGRN = new System.Windows.Forms.ToolStripMenuItem();
		this.InvFrmGRNDirect = new System.Windows.Forms.ToolStripMenuItem();
		this.MarketGrn = new System.Windows.Forms.ToolStripMenuItem();
		this.InvfrmPurchaseInvoice = new System.Windows.Forms.ToolStripMenuItem();
		this.InvfrmPurchasedirectInvoice = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchaseInvoiceAgainstGrnDirect = new System.Windows.Forms.ToolStripMenuItem();
		this.InvfrmPurchaseReturn = new System.Windows.Forms.ToolStripMenuItem();
		this.frmOpeningStockBlancing = new System.Windows.Forms.ToolStripMenuItem();
		this.btnWeightBridge = new System.Windows.Forms.ToolStripMenuItem();
		this.frmWeightbridge = new System.Windows.Forms.ToolStripMenuItem();
		this.WeightbridgeMannual = new System.Windows.Forms.ToolStripMenuItem();
		this.btnCustomerSale = new System.Windows.Forms.ToolStripMenuItem();
		this.SaleOrder = new System.Windows.Forms.ToolStripMenuItem();
		this.DeliveryOrder = new System.Windows.Forms.ToolStripMenuItem();
		this.OutwardGatePass = new System.Windows.Forms.ToolStripMenuItem();
		this.frmDriverBio = new System.Windows.Forms.ToolStripMenuItem();
		this.InvfrmGDN = new System.Windows.Forms.ToolStripMenuItem();
		this.InvfrmSaleInvoice = new System.Windows.Forms.ToolStripMenuItem();
		this.InvfrmSaleInvoiceReturn = new System.Windows.Forms.ToolStripMenuItem();
		this.GoodsDispatchNotesWithoutWeighBridge = new System.Windows.Forms.ToolStripMenuItem();
		this.InvfrmInvSaleInvoiceDirect = new System.Windows.Forms.ToolStripMenuItem();
		this.SaleInvoicepartyStockReserve = new System.Windows.Forms.ToolStripMenuItem();
		this.PointOfSaleInvoice = new System.Windows.Forms.ToolStripMenuItem();
		this.SaleOrderExport = new System.Windows.Forms.ToolStripMenuItem();
		this.InvfrmSaleInvoiceExport = new System.Windows.Forms.ToolStripMenuItem();
		this.frmSaleInvoiceDirect139 = new System.Windows.Forms.ToolStripMenuItem();
		this.btnStockWare = new System.Windows.Forms.ToolStripMenuItem();
		this.frmProductionJobOrder = new System.Windows.Forms.ToolStripMenuItem();
		this.frmFoodProduction = new System.Windows.Forms.ToolStripMenuItem();
		this.invfrmStockConversionProduction = new System.Windows.Forms.ToolStripMenuItem();
		this.frmBillOfMaterial = new System.Windows.Forms.ToolStripMenuItem();
		this.frmBillOfMaterialManufacturing = new System.Windows.Forms.ToolStripMenuItem();
		this.FoodProductionWithValues = new System.Windows.Forms.ToolStripMenuItem();
		this.BillOfMaterialWiseProduction = new System.Windows.Forms.ToolStripMenuItem();
		this.frmProductionCumPackMaterialConsumption = new System.Windows.Forms.ToolStripMenuItem();
		this.ProductionPreCostingAndJobOrder = new System.Windows.Forms.ToolStripMenuItem();
		this.frmBoilerConsumption = new System.Windows.Forms.ToolStripMenuItem();
		this.btnLabAnalysis = new System.Windows.Forms.ToolStripMenuItem();
		this.InvLabAnalysisItems = new System.Windows.Forms.ToolStripMenuItem();
		this.InvLabAnalysisGroup = new System.Windows.Forms.ToolStripMenuItem();
		this.InvLabGroupAnalysisStandards = new System.Windows.Forms.ToolStripMenuItem();
		this.InvLabSampleLogRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.InvLabSampleAnalysis = new System.Windows.Forms.ToolStripMenuItem();
		this.InvLabPurchaseAnalysis = new System.Windows.Forms.ToolStripMenuItem();
		this.InvLabAnalysisInProcess = new System.Windows.Forms.ToolStripMenuItem();
		this.InvLabSaleAnalysis = new System.Windows.Forms.ToolStripMenuItem();
		this.InvLabStandardPolicyForDeduction = new System.Windows.Forms.ToolStripMenuItem();
		this.btnwages = new System.Windows.Forms.ToolStripMenuItem();
		this.frmContractorWagesAccount = new System.Windows.Forms.ToolStripMenuItem();
		this.frmContractWagesSchedule = new System.Windows.Forms.ToolStripMenuItem();
		this.frmwagesBillHeader = new System.Windows.Forms.ToolStripMenuItem();
		this.frmMoveOrder = new System.Windows.Forms.ToolStripMenuItem();
		this.btnExport = new System.Windows.Forms.ToolStripMenuItem();
		this.ExportPartyDefines = new System.Windows.Forms.ToolStripMenuItem();
		this.EximPerformaInvoice = new System.Windows.Forms.ToolStripMenuItem();
		this.EximLcOrder = new System.Windows.Forms.ToolStripMenuItem();
		this.FrmExportSalesContract = new System.Windows.Forms.ToolStripMenuItem();
		this.FrmExportSalesContractSchedule = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.ExpfrmEformRegistration = new System.Windows.Forms.ToolStripMenuItem();
		this.ExpfrmFinancialInsturment = new System.Windows.Forms.ToolStripMenuItem();
		this.EximInvoice = new System.Windows.Forms.ToolStripMenuItem();
		this.EximShippingBookingInfo = new System.Windows.Forms.ToolStripMenuItem();
		this.EximPreProductionLab = new System.Windows.Forms.ToolStripMenuItem();
		this.ExportDeliveryOrder = new System.Windows.Forms.ToolStripMenuItem();
		this.EximForwarding = new System.Windows.Forms.ToolStripMenuItem();
		this.EximForwardingWithoutWeighBridge = new System.Windows.Forms.ToolStripMenuItem();
		this.btnExportForwardingDirect = new System.Windows.Forms.ToolStripMenuItem();
		this.EximBillOfLading = new System.Windows.Forms.ToolStripMenuItem();
		this.btnHRM = new System.Windows.Forms.ToolStripMenuItem();
		this.btnProfileManagement = new System.Windows.Forms.ToolStripMenuItem();
		this.GeneralSetting = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeCategory = new System.Windows.Forms.ToolStripMenuItem();
		this.Benefit = new System.Windows.Forms.ToolStripMenuItem();
		this.Designation = new System.Windows.Forms.ToolStripMenuItem();
		this.Department = new System.Windows.Forms.ToolStripMenuItem();
		this.Section = new System.Windows.Forms.ToolStripMenuItem();
		this.Location = new System.Windows.Forms.ToolStripMenuItem();
		this.ProfileType = new System.Windows.Forms.ToolStripMenuItem();
		this.Profile = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeGroup = new System.Windows.Forms.ToolStripMenuItem();
		this.btnPolicyManegement = new System.Windows.Forms.ToolStripMenuItem();
		this.GeneralPolicy = new System.Windows.Forms.ToolStripMenuItem();
		this.SalaryBreakupPolicy = new System.Windows.Forms.ToolStripMenuItem();
		this.LeaveQuotaPolicy = new System.Windows.Forms.ToolStripMenuItem();
		this.EOBIPolicy = new System.Windows.Forms.ToolStripMenuItem();
		this.ProvidentFundPolicy = new System.Windows.Forms.ToolStripMenuItem();
		this.SocialSecurityPolicy = new System.Windows.Forms.ToolStripMenuItem();
		this.IncomeTaxPolicy = new System.Windows.Forms.ToolStripMenuItem();
		this.btnEmployeeManagement = new System.Windows.Forms.ToolStripMenuItem();
		this.TrialRegistration = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeRegistration = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeNotice = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.ProbationApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.btnDeviceManagement = new System.Windows.Forms.ToolStripMenuItem();
		this.DeviceConfiguration = new System.Windows.Forms.ToolStripMenuItem();
		this.PullAttendanceByMachine = new System.Windows.Forms.ToolStripMenuItem();
		this.PullAttendance = new System.Windows.Forms.ToolStripMenuItem();
		this.btnAttendanceManagement = new System.Windows.Forms.ToolStripMenuItem();
		this.DefineShift = new System.Windows.Forms.ToolStripMenuItem();
		this.ShiftLocation = new System.Windows.Forms.ToolStripMenuItem();
		this.ShiftTiming = new System.Windows.Forms.ToolStripMenuItem();
		this.Holiday = new System.Windows.Forms.ToolStripMenuItem();
		this.DutyRoaster = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeRoaster = new System.Windows.Forms.ToolStripMenuItem();
		this.DailyAttendance = new System.Windows.Forms.ToolStripMenuItem();
		this.ManualAttendance = new System.Windows.Forms.ToolStripMenuItem();
		this.btnLeaveManagement = new System.Windows.Forms.ToolStripMenuItem();
		this.LeaveOpening = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeShortLeave = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeLeave = new System.Windows.Forms.ToolStripMenuItem();
		this.LeaveClosing = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeSpecialLeave = new System.Windows.Forms.ToolStripMenuItem();
		this.btnCPLLeaveMangement = new System.Windows.Forms.ToolStripMenuItem();
		this.frmEmployeeCPLLeaveOpening = new System.Windows.Forms.ToolStripMenuItem();
		this.btnCPLAttendance = new System.Windows.Forms.ToolStripMenuItem();
		this.btnCPLRequest = new System.Windows.Forms.ToolStripMenuItem();
		this.btnLoanManagement = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeLoan = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeAdvance = new System.Windows.Forms.ToolStripMenuItem();
		this.btnOverTimeManagement = new System.Windows.Forms.ToolStripMenuItem();
		this.OverTimeRequest = new System.Windows.Forms.ToolStripMenuItem();
		this.OTRequestApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeOverTime = new System.Windows.Forms.ToolStripMenuItem();
		this.btnApprovalManagement = new System.Windows.Forms.ToolStripMenuItem();
		this.AdvanceApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.LoanApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.LeaveApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.frmCPLApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.OverTimeApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.ShortLeaveApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.AttendanceApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.ShortHourApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.AllowanceApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.LoanDeductionApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.btnPayroll = new System.Windows.Forms.ToolStripMenuItem();
		this.PayrollPosting = new System.Windows.Forms.ToolStripMenuItem();
		this.OvertimePosting = new System.Windows.Forms.ToolStripMenuItem();
		this.DailyWagesPosting = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeAllowance = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeLateAdjustment = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeLoanDeduction = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeShortAdjustment = new System.Windows.Forms.ToolStripMenuItem();
		this.btnGlobalEmployment = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeePromotion = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeIncrement = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeTransfer = new System.Windows.Forms.ToolStripMenuItem();
		this.FullandFinalSettlement = new System.Windows.Forms.ToolStripMenuItem();
		this.BlackListed = new System.Windows.Forms.ToolStripMenuItem();
		this.storeManagmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchsaeOrderTrade = new System.Windows.Forms.ToolStripMenuItem();
		this.frmSupplySchedule = new System.Windows.Forms.ToolStripMenuItem();
		this.InwardGatePassTrade = new System.Windows.Forms.ToolStripMenuItem();
		this.InvFrmGRNTrade = new System.Windows.Forms.ToolStripMenuItem();
		this.purchaseInvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchaseInvoiceTradingWithTax = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchaseInvoiceTradePro = new System.Windows.Forms.ToolStripMenuItem();
		this.btnSaleTrading = new System.Windows.Forms.ToolStripMenuItem();
		this.SaleOrderTrade = new System.Windows.Forms.ToolStripMenuItem();
		this.frmDeliverySchedule = new System.Windows.Forms.ToolStripMenuItem();
		this.FrmDeliveryChallan = new System.Windows.Forms.ToolStripMenuItem();
		this.saleInvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.SaleInvoiceTradingWithTax = new System.Windows.Forms.ToolStripMenuItem();
		this.InvfrmSaleInvoiceTrading = new System.Windows.Forms.ToolStripMenuItem();
		this.fixedAssetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.frmFixedAssetsCategory = new System.Windows.Forms.ToolStripMenuItem();
		this.frmDefineFixedAssets = new System.Windows.Forms.ToolStripMenuItem();
		this.frmFixedAssetRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.frmFixedAssetRegisterHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.frmFixedAssetsImportPurchaseOrder = new System.Windows.Forms.ToolStripMenuItem();
		this.btnCargoManagement = new System.Windows.Forms.ToolStripMenuItem();
		this.CargoBooking = new System.Windows.Forms.ToolStripMenuItem();
		this.FrmCBDHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.ContainerAllocation = new System.Windows.Forms.ToolStripMenuItem();
		this.cbdBookingTracking = new System.Windows.Forms.ToolStripMenuItem();
		this.DestinationWareHouseReceipt = new System.Windows.Forms.ToolStripMenuItem();
		this.frmCargoDeliveryOrderAndBill = new System.Windows.Forms.ToolStripMenuItem();
		this.frmClearingBill = new System.Windows.Forms.ToolStripMenuItem();
		this.btnFinanceManagement = new System.Windows.Forms.ToolStripMenuItem();
		this.frmFinanceType = new System.Windows.Forms.ToolStripMenuItem();
		this.frmFMLoanRegistration = new System.Windows.Forms.ToolStripMenuItem();
		this.frmFMLoanAgreement = new System.Windows.Forms.ToolStripMenuItem();
		this.btnStoreManagement = new System.Windows.Forms.ToolStripMenuItem();
		this.ItemCategoryStore = new System.Windows.Forms.ToolStripMenuItem();
		this.ItemTypeStore = new System.Windows.Forms.ToolStripMenuItem();
		this.AddItemStore = new System.Windows.Forms.ToolStripMenuItem();
		this.frmStoreOpeningStockBalancing = new System.Windows.Forms.ToolStripMenuItem();
		this.frmStockAdjustment = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.PurchaseOrderStore = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPurchaseOrderWithTax = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPurchaseDemand = new System.Windows.Forms.ToolStripMenuItem();
		this.GatePassGeneral = new System.Windows.Forms.ToolStripMenuItem();
		this.GrnStore = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchaseInvoiceStoreManagement = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchaseInvoiceStoreWithOutTax = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchaseInvoiceStoreWithTax = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPurchaseInvoiceDirectStore = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchaseInvoiceDirectStoreWithTax = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.DeliveryOrderPackingMaterial = new System.Windows.Forms.ToolStripMenuItem();
		this.GatePassGeneralOutward = new System.Windows.Forms.ToolStripMenuItem();
		this.frmDepartmentRequest = new System.Windows.Forms.ToolStripMenuItem();
		this.frmGSIssuance = new System.Windows.Forms.ToolStripMenuItem();
		this.StoreIssuanceDirect = new System.Windows.Forms.ToolStripMenuItem();
		this.frmStockTransfer = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPackingMaterialTransferPartyToParty = new System.Windows.Forms.ToolStripMenuItem();
		this.FrmSaleInvoiceDirectPackingMaterial = new System.Windows.Forms.ToolStripMenuItem();
		this.frmSaleInvoiceDirectWithOutTax = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
		this.frmSaleInvoiceDirectWithTax = new System.Windows.Forms.ToolStripMenuItem();
		this.StoreReturn = new System.Windows.Forms.ToolStripMenuItem();
		this.PartyProcessing = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPartyProcessingDefineSupplier = new System.Windows.Forms.ToolStripMenuItem();
		this.DefineReferenceParties = new System.Windows.Forms.ToolStripMenuItem();
		this.DefItemCatagoryPartyProcessing = new System.Windows.Forms.ToolStripMenuItem();
		this.DefPartyProcessingItemType = new System.Windows.Forms.ToolStripMenuItem();
		this.DefPartProcessingAddItem = new System.Windows.Forms.ToolStripMenuItem();
		this.StockOpeningBalancePartyProcessing = new System.Windows.Forms.ToolStripMenuItem();
		this.GatePassInwardPartyProcessing = new System.Windows.Forms.ToolStripMenuItem();
		this.GoodsReceivingNotesPartyProcessing = new System.Windows.Forms.ToolStripMenuItem();
		this.GRNStorePartyProcessing = new System.Windows.Forms.ToolStripMenuItem();
		this.GatePassOutwardParyProcessing = new System.Windows.Forms.ToolStripMenuItem();
		this.GoodsDispatchNotesPartyProcessing = new System.Windows.Forms.ToolStripMenuItem();
		this.GDNStorePartyProcessing = new System.Windows.Forms.ToolStripMenuItem();
		this.ProductionJobOrderPartyProcessing = new System.Windows.Forms.ToolStripMenuItem();
		this.frmProductionPartyProcessing = new System.Windows.Forms.ToolStripMenuItem();
		this.ProductionPartyProcessingBill = new System.Windows.Forms.ToolStripMenuItem();
		this.btnPartyProcessingReports = new System.Windows.Forms.ToolStripMenuItem();
		this.frmGatePassPartyProcessing = new System.Windows.Forms.ToolStripMenuItem();
		this.frmGRNPartyProcessingRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.grmGDNPartyProcessingRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.frmStockReportPartyProcessing = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPartyProcessingGrnInfo = new System.Windows.Forms.ToolStripMenuItem();
		this.petroliumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchaseInvoicePetrol = new System.Windows.Forms.ToolStripMenuItem();
		this.SaleInvoicePetrol = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchaseInvoiceRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.SaleInvoiceRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.StockReport = new System.Windows.Forms.ToolStripMenuItem();
		this.btnServices = new System.Windows.Forms.ToolStripMenuItem();
		this.btndefineservices = new System.Windows.Forms.ToolStripMenuItem();
		this.frmSrvCustPricingAgreement = new System.Windows.Forms.ToolStripMenuItem();
		this.frmSrvSaleOrderForwarder = new System.Windows.Forms.ToolStripMenuItem();
		this.frmServClearingAgentBill = new System.Windows.Forms.ToolStripMenuItem();
		this.ExImClearingAgentBillDirect = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPreShipmentCosting = new System.Windows.Forms.ToolStripMenuItem();
		this.btnServicesReport = new System.Windows.Forms.ToolStripMenuItem();
		this.ServicesBillRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.btnTaxation = new System.Windows.Forms.ToolStripMenuItem();
		this.InvfrmAddTaxType = new System.Windows.Forms.ToolStripMenuItem();
		this.InvfrmAddTaxSchedule = new System.Windows.Forms.ToolStripMenuItem();
		this.InvDeffrmItemTaxSchdule = new System.Windows.Forms.ToolStripMenuItem();
		this.frmTaxNotesAndGLMaping = new System.Windows.Forms.ToolStripMenuItem();
		this.frmSupplierCustomerExemptionSchedule = new System.Windows.Forms.ToolStripMenuItem();
		this.SaleTaxSummary = new System.Windows.Forms.ToolStripMenuItem();
		this.btnImport = new System.Windows.Forms.ToolStripMenuItem();
		this.frmImpPerformaInvoice = new System.Windows.Forms.ToolStripMenuItem();
		this.frmImpLcContract = new System.Windows.Forms.ToolStripMenuItem();
		this.frmImpLcPurchaseContract = new System.Windows.Forms.ToolStripMenuItem();
		this.frmImpPurchaseOrder = new System.Windows.Forms.ToolStripMenuItem();
		this.frmImportOrderPackingDetail = new System.Windows.Forms.ToolStripMenuItem();
		this.btnBillofLadingImport = new System.Windows.Forms.ToolStripSeparator();
		this.frmImpBL = new System.Windows.Forms.ToolStripMenuItem();
		this.frmImpGrnForwarding = new System.Windows.Forms.ToolStripMenuItem();
		this.frmImpGrnDirect = new System.Windows.Forms.ToolStripMenuItem();
		this.frmImpInvoice = new System.Windows.Forms.ToolStripMenuItem();
		this.frmImportCommercialInvoicebyContract = new System.Windows.Forms.ToolStripMenuItem();
		this.CommissionAgentTrade = new System.Windows.Forms.ToolStripMenuItem();
		this.frmCommissionAgentOrder = new System.Windows.Forms.ToolStripMenuItem();
		this.frmCommissionAgentTradeBill = new System.Windows.Forms.ToolStripMenuItem();
		this.CommissionAgentTradeBill_162 = new System.Windows.Forms.ToolStripMenuItem();
		this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.btnDashBoard = new System.Windows.Forms.ToolStripMenuItem();
		this.AcFrmDashboard = new System.Windows.Forms.ToolStripMenuItem();
		this.PendingVouchersForApproval = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPendingWorksRpt = new System.Windows.Forms.ToolStripMenuItem();
		this.btnStocksDashBoard = new System.Windows.Forms.ToolStripMenuItem();
		this.UnApprovedVouchersDashBoard = new System.Windows.Forms.ToolStripMenuItem();
		this.SalesAnalytics = new System.Windows.Forms.ToolStripMenuItem();
		this.btnAnalyticDashboard = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPaddyPurchaseAnalytics = new System.Windows.Forms.ToolStripMenuItem();
		this.frmRicePurchaseAnalytics = new System.Windows.Forms.ToolStripMenuItem();
		this.frmSalesAnalytics = new System.Windows.Forms.ToolStripMenuItem();
		this.OrderManagementDashboard = new System.Windows.Forms.ToolStripMenuItem();
		this.btnAcReport = new System.Windows.Forms.ToolStripMenuItem();
		this.ChartOfAccount = new System.Windows.Forms.ToolStripMenuItem();
		this.VoucherReport = new System.Windows.Forms.ToolStripMenuItem();
		this.VoucherValidation = new System.Windows.Forms.ToolStripMenuItem();
		this.CostomerWiseVoucherSlip = new System.Windows.Forms.ToolStripMenuItem();
		this.ActicitySummery = new System.Windows.Forms.ToolStripMenuItem();
		this.BankBalances = new System.Windows.Forms.ToolStripMenuItem();
		this.CashBalances = new System.Windows.Forms.ToolStripMenuItem();
		this.GeneralLedger = new System.Windows.Forms.ToolStripMenuItem();
		this.FcyGeneralLedgerRpt = new System.Windows.Forms.ToolStripMenuItem();
		this.CustomerLedger = new System.Windows.Forms.ToolStripMenuItem();
		this.GeneralLedgerStatment = new System.Windows.Forms.ToolStripMenuItem();
		this.Payables = new System.Windows.Forms.ToolStripMenuItem();
		this.PayablesAging = new System.Windows.Forms.ToolStripMenuItem();
		this.PayablesByDueDate = new System.Windows.Forms.ToolStripMenuItem();
		this.Receivables = new System.Windows.Forms.ToolStripMenuItem();
		this.ReceivableAging = new System.Windows.Forms.ToolStripMenuItem();
		this.TradeDebitorsReport = new System.Windows.Forms.ToolStripMenuItem();
		this.TradeCreditorsReport = new System.Windows.Forms.ToolStripMenuItem();
		this.btnPaymentByInvoiceSummery_Detail = new System.Windows.Forms.ToolStripMenuItem();
		this.InventoryPayablesandReceivables = new System.Windows.Forms.ToolStripMenuItem();
		this.FCYPayablesAndReceivablesRpt = new System.Windows.Forms.ToolStripMenuItem();
		this.TrialBalance = new System.Windows.Forms.ToolStripMenuItem();
		this.SelectedTrialBalanceAllLevel = new System.Windows.Forms.ToolStripMenuItem();
		this.SelectedTrialBalance = new System.Windows.Forms.ToolStripMenuItem();
		this.PayablesandReceivablesAging = new System.Windows.Forms.ToolStripMenuItem();
		this.GeneralJournalSummeryRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.AuditDashboard = new System.Windows.Forms.ToolStripMenuItem();
		this.frmProfitAndLoss = new System.Windows.Forms.ToolStripMenuItem();
		this.frmBalanceSheet = new System.Windows.Forms.ToolStripMenuItem();
		this.pdcReprotsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.PdcInventoryReport = new System.Windows.Forms.ToolStripMenuItem();
		this.InventoryReport = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchaseOrderHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.frmGatePassGeneral = new System.Windows.Forms.ToolStripMenuItem();
		this.frmGatePassReport = new System.Windows.Forms.ToolStripMenuItem();
		this.frmGRNHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPurchaseInvoiceHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchaseRegisterNew = new System.Windows.Forms.ToolStripMenuItem();
		this.frmSaleOrderHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.frmDeliveryOrderForApprovel = new System.Windows.Forms.ToolStripMenuItem();
		this.frmGPOutward = new System.Windows.Forms.ToolStripMenuItem();
		this.frmGDNHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.SaleInvoiceRegisterNew = new System.Windows.Forms.ToolStripMenuItem();
		this.frmSaleInvoiceHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.frmSaleInvoiceDirectRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.frmWeightBridgeHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.frmWeighBridgeRejectedTicketNos = new System.Windows.Forms.ToolStripMenuItem();
		this.frmEvaulationDetailWagesReports = new System.Windows.Forms.ToolStripMenuItem();
		this.frmStockContractorWagesHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.ItemLedger = new System.Windows.Forms.ToolStripMenuItem();
		this.btnTradingReports = new System.Windows.Forms.ToolStripMenuItem();
		this.PurchaseInvoiceTrading = new System.Windows.Forms.ToolStripMenuItem();
		this.SaleInvoiceTrading = new System.Windows.Forms.ToolStripMenuItem();
		this.frmRptItemList = new System.Windows.Forms.ToolStripMenuItem();
		this.DepartmentRequestHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.StoreIssuenceHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.frmSaleInvoiceReturnRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.frmStockTranfserRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.StockAdjustmentRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPurchaseRegisterStore = new System.Windows.Forms.ToolStripMenuItem();
		this.frmPurchaseDemandRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.StoreIssuanceReturnRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.btnExportHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.frmContainerList = new System.Windows.Forms.ToolStripMenuItem();
		this.SaleContractHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.ExportInvoiceRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.btnShipmentHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.frmExImForwardingSlipandRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.frmBillofLadingSlipandRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.frmExportShipingLineBookingRpt = new System.Windows.Forms.ToolStripMenuItem();
		this.FrmRptExImShipmentScheduleHistroy = new System.Windows.Forms.ToolStripMenuItem();
		this.frmshippedConsignment_followup = new System.Windows.Forms.ToolStripMenuItem();
		this.ExImRptFrmAdvancePaymentReceiptAndUtilization = new System.Windows.Forms.ToolStripMenuItem();
		this.ExportCustomerWiseComparisonSummary = new System.Windows.Forms.ToolStripMenuItem();
		this.ExportComparisonSummary = new System.Windows.Forms.ToolStripMenuItem();
		this.ExportDetailHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.frmExportSummaryRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.ExImShipmentCosting = new System.Windows.Forms.ToolStripMenuItem();
		this.btnLabReports = new System.Windows.Forms.ToolStripMenuItem();
		this.InvLabPurchaseRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.btnCargoReports = new System.Windows.Forms.ToolStripMenuItem();
		this.CbdReceivablesandReceiptsHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.btnStockReport = new System.Windows.Forms.ToolStripMenuItem();
		this.frmStockReport = new System.Windows.Forms.ToolStripMenuItem();
		this.InventoryTransactionReport = new System.Windows.Forms.ToolStripMenuItem();
		this.RptGenrateStocks = new System.Windows.Forms.ToolStripMenuItem();
		this.InventoryEvaluationItemLedger = new System.Windows.Forms.ToolStripMenuItem();
		this.InventoryTransactionReportStore = new System.Windows.Forms.ToolStripMenuItem();
		this.RptGenrateStocksStore = new System.Windows.Forms.ToolStripMenuItem();
		this.ItemLedgerReport_Inventory = new System.Windows.Forms.ToolStripMenuItem();
		this.StockReportWithAvgRates = new System.Windows.Forms.ToolStripMenuItem();
		this.frmInventoryProfitLoss = new System.Windows.Forms.ToolStripMenuItem();
		this.btnEmptyBagsReports = new System.Windows.Forms.ToolStripMenuItem();
		this.frmStockWithSupplierHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.productionReportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.frmProductionPendingPurchaseInvoice = new System.Windows.Forms.ToolStripMenuItem();
		this.FoodProductionComparisonRpt = new System.Windows.Forms.ToolStripMenuItem();
		this.btnHRMReports = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeHistoryRpt = new System.Windows.Forms.ToolStripMenuItem();
		this.btnPayrollSalaryReport = new System.Windows.Forms.ToolStripMenuItem();
		this.PayRollSalarySheetRpt = new System.Windows.Forms.ToolStripMenuItem();
		this.DailyAttendanceRpt = new System.Windows.Forms.ToolStripMenuItem();
		this.DailyLateandEarlyDeparture = new System.Windows.Forms.ToolStripMenuItem();
		this.genDutyRosterEmployeeWise = new System.Windows.Forms.ToolStripMenuItem();
		this.genEmployeeAttendence = new System.Windows.Forms.ToolStripMenuItem();
		this.MonthlyAttendanceSummary = new System.Windows.Forms.ToolStripMenuItem();
		this.EmployeeMonthlyRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.btnImportReports = new System.Windows.Forms.ToolStripMenuItem();
		this.ImportContractRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.ImportPurchaseOrderRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.ImportOrderPackingDetailRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.ImportGrnRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.ImportInvoiceRegister = new System.Windows.Forms.ToolStripMenuItem();
		this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.btnadminpanel = new System.Windows.Forms.ToolStripMenuItem();
		this.btnadminpanelsysconfig = new System.Windows.Forms.ToolStripMenuItem();
		this.btnadminpaneluserright = new System.Windows.Forms.ToolStripMenuItem();
		this.btnScreenRights = new System.Windows.Forms.ToolStripMenuItem();
		this.btnScreenDefination = new System.Windows.Forms.ToolStripMenuItem();
		this.DefineReports = new System.Windows.Forms.ToolStripMenuItem();
		this.btnStock_Reconcilation = new System.Windows.Forms.ToolStripMenuItem();
		this.btnPlBSNotes = new System.Windows.Forms.ToolStripMenuItem();
		this.companyProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.btnAuditLogReport = new System.Windows.Forms.ToolStripMenuItem();
		this.sQLQueryExecuterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.btnWeighBridge_WeightUpdate = new System.Windows.Forms.ToolStripMenuItem();
		this.BtnThemeForm = new System.Windows.Forms.ToolStripMenuItem();
		this.btnAccountMovement = new System.Windows.Forms.ToolStripMenuItem();
		this.btnUtility = new System.Windows.Forms.ToolStripMenuItem();
		this.btnDatabaseBackup = new System.Windows.Forms.ToolStripMenuItem();
		this.licenseKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.btnAccountLookupa = new System.Windows.Forms.ToolStripMenuItem();
		this.btnPlAndBsSettings = new System.Windows.Forms.ToolStripMenuItem();
		this.btnSupplierDefineSystemUtilities = new System.Windows.Forms.ToolStripMenuItem();
		this.shipToAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.supplierLimitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.supplierBankDetailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.defineInventoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.btnOtherItemsDefine = new System.Windows.Forms.ToolStripMenuItem();
		this.btnPartyItemAllocation = new System.Windows.Forms.ToolStripMenuItem();
		this.btnMultiCurrency = new System.Windows.Forms.ToolStripMenuItem();
		this.btnPackingMaterial = new System.Windows.Forms.ToolStripMenuItem();
		this.btnCountry = new System.Windows.Forms.ToolStripMenuItem();
		this.DateLock = new System.Windows.Forms.ToolStripMenuItem();
		this.btnSMSContacts = new System.Windows.Forms.ToolStripMenuItem();
		this.btnScreenMessageReceivers = new System.Windows.Forms.ToolStripMenuItem();
		this.btnSMSConfiquration = new System.Windows.Forms.ToolStripMenuItem();
		this.btnHistory = new System.Windows.Forms.ToolStripMenuItem();
		this.invLookUpsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ScaleKartDefine = new System.Windows.Forms.ToolStripMenuItem();
		this.contextMenuStrip4 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.lockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.UploadFiles = new System.Windows.Forms.ToolStripMenuItem();
		this.dummyGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.btnUserProfile = new System.Windows.Forms.ToolStripMenuItem();
		this.timer2 = new System.Windows.Forms.Timer(this.components);
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.contextMenuStrip5 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.contextMenuStrip6 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.bw_CheckDownload = new System.ComponentModel.BackgroundWorker();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnUpdate).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNavigation).BeginInit();
		this.pnlmain.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnRefresh).BeginInit();
		this.ContextMenuStrip1.SuspendLayout();
		this.contextMenuStrip2.SuspendLayout();
		this.contextMenuStrip3.SuspendLayout();
		this.contextMenuStrip4.SuspendLayout();
		this.panel4.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.btnUpdate);
		this.panel3.Controls.Add(this.lblFinancialYear);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbNavigation);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.lblTime);
		this.panel3.Controls.Add(this.lblBranchName);
		this.panel3.Controls.Add(this.lblusername);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1137, 37);
		this.panel3.TabIndex = 5;
		this.btnUpdate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnUpdate.Location = new System.Drawing.Point(1064, 3);
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(30, 30);
		this.btnUpdate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.btnUpdate.TabIndex = 18;
		this.btnUpdate.TabStop = false;
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click_1);
		this.lblFinancialYear.AutoSize = true;
		this.lblFinancialYear.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.lblFinancialYear.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFinancialYear.ForeColor = System.Drawing.Color.White;
		this.lblFinancialYear.Location = new System.Drawing.Point(729, 9);
		this.lblFinancialYear.Name = "lblFinancialYear";
		this.lblFinancialYear.Size = new System.Drawing.Size(104, 20);
		this.lblFinancialYear.TabIndex = 6;
		this.lblFinancialYear.Text = "Financial Year";
		this.CmbNavigation.AllowNull = (DefaultableBoolean)1;
		this.CmbNavigation.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbNavigation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbNavigation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbNavigation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbNavigation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbNavigation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.CmbNavigation.DropDownButtonDisplayStyle = (ButtonDisplayStyle)0;
		((System.Windows.Forms.Control)(object)this.CmbNavigation).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbNavigation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbNavigation).Location = new System.Drawing.Point(3, 5);
		((System.Windows.Forms.Control)(object)this.CmbNavigation).Name = "CmbNavigation";
		((System.Windows.Forms.Control)(object)this.CmbNavigation).Size = new System.Drawing.Size(241, 26);
		((System.Windows.Forms.Control)(object)this.CmbNavigation).TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(881, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(79, 20);
		this.label1.TabIndex = 3;
		this.label1.Text = "V.22.0013";
		this.label1.Click += new System.EventHandler(label1_Click);
		this.lblTime.AutoSize = true;
		this.lblTime.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.lblTime.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTime.ForeColor = System.Drawing.Color.White;
		this.lblTime.Location = new System.Drawing.Point(615, 9);
		this.lblTime.Name = "lblTime";
		this.lblTime.Size = new System.Drawing.Size(44, 20);
		this.lblTime.TabIndex = 4;
		this.lblTime.Text = "Time";
		this.lblBranchName.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.lblBranchName.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBranchName.ForeColor = System.Drawing.Color.White;
		this.lblBranchName.Location = new System.Drawing.Point(434, 2);
		this.lblBranchName.Name = "lblBranchName";
		this.lblBranchName.Size = new System.Drawing.Size(150, 34);
		this.lblBranchName.TabIndex = 0;
		this.lblBranchName.Text = "Branch\r\nName";
		this.lblBranchName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblBranchName.Click += new System.EventHandler(lblusername_Click);
		this.lblBranchName.MouseLeave += new System.EventHandler(lblusername_MouseLeave);
		this.lblBranchName.MouseHover += new System.EventHandler(lblusername_MouseHover);
		this.lblusername.AutoSize = true;
		this.lblusername.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.lblusername.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblusername.ForeColor = System.Drawing.Color.White;
		this.lblusername.Location = new System.Drawing.Point(246, 9);
		this.lblusername.Name = "lblusername";
		this.lblusername.Size = new System.Drawing.Size(83, 20);
		this.lblusername.TabIndex = 0;
		this.lblusername.Text = "UserName";
		this.lblusername.Click += new System.EventHandler(lblusername_Click);
		this.lblusername.MouseLeave += new System.EventHandler(lblusername_MouseLeave);
		this.lblusername.MouseHover += new System.EventHandler(lblusername_MouseHover);
		this.pnlmain.AutoScroll = true;
		this.pnlmain.BackColor = System.Drawing.Color.White;
		this.pnlmain.Controls.Add(this.panel1);
		this.pnlmain.Dock = System.Windows.Forms.DockStyle.Left;
		this.pnlmain.Location = new System.Drawing.Point(0, 0);
		this.pnlmain.Name = "pnlmain";
		this.pnlmain.Size = new System.Drawing.Size(55, 594);
		this.pnlmain.TabIndex = 2;
		this.panel1.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel1.Controls.Add(this.BtnViewRightsReport);
		this.panel1.Controls.Add(this.btnNavigateToForms);
		this.panel1.Controls.Add(this.btnNext);
		this.panel1.Controls.Add(this.btnBack);
		this.panel1.Controls.Add(this.btnFavourites);
		this.panel1.Controls.Add(this.btnHome);
		this.panel1.Controls.Add(this.btnRefresh);
		this.panel1.Controls.Add(this.btnSettings);
		this.panel1.Controls.Add(this.btnReport);
		this.panel1.Controls.Add(this.btnMenuButton);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(55, 594);
		this.panel1.TabIndex = 7;
		this.BtnViewRightsReport.BackColor = System.Drawing.Color.White;
		this.BtnViewRightsReport.BackgroundImage = Architecture.WinApp.Properties.Resources.icons8_user_rights_30;
		this.BtnViewRightsReport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.BtnViewRightsReport.FlatAppearance.BorderSize = 0;
		this.BtnViewRightsReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnViewRightsReport.ForeColor = System.Drawing.SystemColors.ActiveBorder;
		this.BtnViewRightsReport.Location = new System.Drawing.Point(9, 276);
		this.BtnViewRightsReport.Name = "BtnViewRightsReport";
		this.BtnViewRightsReport.Size = new System.Drawing.Size(34, 34);
		this.BtnViewRightsReport.TabIndex = 16;
		this.BtnViewRightsReport.UseVisualStyleBackColor = false;
		this.BtnViewRightsReport.Visible = false;
		this.BtnViewRightsReport.Click += new System.EventHandler(BtnViewRightsReport_Click);
		this.btnNavigateToForms.BackColor = System.Drawing.Color.Transparent;
		this.btnNavigateToForms.BackgroundImage = Architecture.WinApp.Properties.Resources.chat_history_fill;
		this.btnNavigateToForms.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btnNavigateToForms.FlatAppearance.BorderSize = 0;
		this.btnNavigateToForms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnNavigateToForms.ForeColor = System.Drawing.SystemColors.ActiveBorder;
		this.btnNavigateToForms.Location = new System.Drawing.Point(15, 238);
		this.btnNavigateToForms.Name = "btnNavigateToForms";
		this.btnNavigateToForms.Size = new System.Drawing.Size(22, 22);
		this.btnNavigateToForms.TabIndex = 16;
		this.btnNavigateToForms.UseVisualStyleBackColor = false;
		this.btnNavigateToForms.Click += new System.EventHandler(btnNavigateToForms_Click);
		this.btnNavigateToForms.MouseHover += new System.EventHandler(btnNavigateToForms_MouseHover);
		this.btnNext.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnNext.BackgroundImage = Architecture.WinApp.Properties.Resources.arrow_right_fill;
		this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnNext.ForeColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnNext.Location = new System.Drawing.Point(29, 7);
		this.btnNext.Name = "btnNext";
		this.btnNext.Size = new System.Drawing.Size(25, 23);
		this.btnNext.TabIndex = 15;
		this.btnNext.UseVisualStyleBackColor = false;
		this.btnNext.Click += new System.EventHandler(btnNext_Click);
		this.btnBack.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnBack.BackgroundImage = Architecture.WinApp.Properties.Resources.arrow_left_fill;
		this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnBack.ForeColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnBack.Location = new System.Drawing.Point(1, 7);
		this.btnBack.Name = "btnBack";
		this.btnBack.Size = new System.Drawing.Size(25, 23);
		this.btnBack.TabIndex = 14;
		this.btnBack.UseVisualStyleBackColor = false;
		this.btnBack.Click += new System.EventHandler(btnBack_Click);
		this.btnFavourites.BackColor = System.Drawing.Color.Transparent;
		this.btnFavourites.BackgroundImage = Architecture.WinApp.Properties.Resources.WhiteStar;
		this.btnFavourites.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btnFavourites.FlatAppearance.BorderSize = 0;
		this.btnFavourites.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnFavourites.ForeColor = System.Drawing.SystemColors.ActiveBorder;
		this.btnFavourites.Location = new System.Drawing.Point(12, 191);
		this.btnFavourites.Name = "btnFavourites";
		this.btnFavourites.Size = new System.Drawing.Size(28, 28);
		this.btnFavourites.TabIndex = 13;
		this.btnFavourites.UseVisualStyleBackColor = false;
		this.btnFavourites.Click += new System.EventHandler(btnFavourites_Click);
		this.btnFavourites.MouseLeave += new System.EventHandler(btnFavourites_MouseLeave);
		this.btnFavourites.MouseHover += new System.EventHandler(btnFavourites_MouseHover);
		this.btnHome.BackColor = System.Drawing.Color.Transparent;
		this.btnHome.BackgroundImage = Architecture.WinApp.Properties.Resources.home_4_fill;
		this.btnHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btnHome.FlatAppearance.BorderSize = 0;
		this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnHome.ForeColor = System.Drawing.SystemColors.ActiveBorder;
		this.btnHome.Location = new System.Drawing.Point(16, 139);
		this.btnHome.Name = "btnHome";
		this.btnHome.Size = new System.Drawing.Size(22, 22);
		this.btnHome.TabIndex = 12;
		this.btnHome.UseVisualStyleBackColor = false;
		this.btnHome.Click += new System.EventHandler(btnHome_Click);
		this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
		this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.Location = new System.Drawing.Point(0, 562);
		this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(55, 32);
		this.btnRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.btnRefresh.TabIndex = 11;
		this.btnRefresh.TabStop = false;
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnRefresh.MouseHover += new System.EventHandler(btnRefresh_Click);
		this.btnSettings.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.btnSettings.BackColor = System.Drawing.Color.Transparent;
		this.btnSettings.BackgroundImage = (System.Drawing.Image)resources.GetObject("btnSettings.BackgroundImage");
		this.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSettings.FlatAppearance.BorderSize = 0;
		this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSettings.ForeColor = System.Drawing.SystemColors.ActiveBorder;
		this.btnSettings.Location = new System.Drawing.Point(8, 503);
		this.btnSettings.Name = "btnSettings";
		this.btnSettings.Size = new System.Drawing.Size(40, 27);
		this.btnSettings.TabIndex = 2;
		this.btnSettings.UseVisualStyleBackColor = false;
		this.btnSettings.Click += new System.EventHandler(btnSettings_MouseHover);
		this.btnSettings.MouseHover += new System.EventHandler(btnSettings_MouseHover);
		this.btnReport.BackColor = System.Drawing.Color.Transparent;
		this.btnReport.BackgroundImage = (System.Drawing.Image)resources.GetObject("btnReport.BackgroundImage");
		this.btnReport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btnReport.FlatAppearance.BorderSize = 0;
		this.btnReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnReport.ForeColor = System.Drawing.SystemColors.ActiveBorder;
		this.btnReport.Location = new System.Drawing.Point(12, 83);
		this.btnReport.Name = "btnReport";
		this.btnReport.Size = new System.Drawing.Size(27, 27);
		this.btnReport.TabIndex = 1;
		this.btnReport.UseVisualStyleBackColor = false;
		this.btnReport.Click += new System.EventHandler(btnReport_MouseHover);
		this.btnReport.MouseHover += new System.EventHandler(btnReport_MouseHover);
		this.btnMenuButton.BackColor = System.Drawing.Color.Transparent;
		this.btnMenuButton.BackgroundImage = (System.Drawing.Image)resources.GetObject("btnMenuButton.BackgroundImage");
		this.btnMenuButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btnMenuButton.FlatAppearance.BorderSize = 0;
		this.btnMenuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnMenuButton.ForeColor = System.Drawing.SystemColors.ActiveBorder;
		this.btnMenuButton.Location = new System.Drawing.Point(15, 42);
		this.btnMenuButton.Name = "btnMenuButton";
		this.btnMenuButton.Size = new System.Drawing.Size(22, 22);
		this.btnMenuButton.TabIndex = 0;
		this.btnMenuButton.UseVisualStyleBackColor = false;
		this.btnMenuButton.Click += new System.EventHandler(btnMenuButton_MouseHover);
		this.btnMenuButton.MouseHover += new System.EventHandler(btnMenuButton_MouseHover);
		this.ContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[23]
		{
			this.btnAccountDefinition, this.btnAccountsTransaction, this.btnInvDefinition, this.btnSupPurchase, this.btnWeightBridge, this.btnCustomerSale, this.btnStockWare, this.btnLabAnalysis, this.btnwages, this.btnExport,
			this.btnHRM, this.storeManagmentToolStripMenuItem, this.btnSaleTrading, this.fixedAssetsToolStripMenuItem, this.btnCargoManagement, this.btnFinanceManagement, this.btnStoreManagement, this.PartyProcessing, this.petroliumToolStripMenuItem, this.btnServices,
			this.btnTaxation, this.btnImport, this.CommissionAgentTrade
		});
		this.ContextMenuStrip1.Name = "ContextMenuStrip1";
		this.ContextMenuStrip1.Size = new System.Drawing.Size(208, 510);
		this.btnAccountDefinition.AccessibleName = "";
		this.btnAccountDefinition.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.AcfrmDefCoa, this.AcfrmAcAllocation, this.UpdateCoAllocation, this.AcfrmOpeningBalance, this.AccountsCustomGroup, this.AccountBudget, this.AcfrmChequebookRegistration, this.AcfrmDefineBank, this.frmAdvanceAdjustment, this.frmIncentivePolicy,
			this.InLandFreightAgreement
		});
		this.btnAccountDefinition.Name = "btnAccountDefinition";
		this.btnAccountDefinition.Size = new System.Drawing.Size(207, 22);
		this.btnAccountDefinition.Tag = "Accounts Definition";
		this.btnAccountDefinition.Text = "Accounts Definition";
		this.AcfrmDefCoa.Name = "AcfrmDefCoa";
		this.AcfrmDefCoa.Size = new System.Drawing.Size(215, 22);
		this.AcfrmDefCoa.Text = "Chart Of Account";
		this.AcfrmAcAllocation.Name = "AcfrmAcAllocation";
		this.AcfrmAcAllocation.Size = new System.Drawing.Size(215, 22);
		this.AcfrmAcAllocation.Text = "Accounts Allocation";
		this.UpdateCoAllocation.Name = "UpdateCoAllocation";
		this.UpdateCoAllocation.Size = new System.Drawing.Size(215, 22);
		this.UpdateCoAllocation.Text = "Update COA Allocation";
		this.AcfrmOpeningBalance.Name = "AcfrmOpeningBalance";
		this.AcfrmOpeningBalance.Size = new System.Drawing.Size(215, 22);
		this.AcfrmOpeningBalance.Text = "Opening Balance";
		this.AccountsCustomGroup.Name = "AccountsCustomGroup";
		this.AccountsCustomGroup.Size = new System.Drawing.Size(215, 22);
		this.AccountsCustomGroup.Text = "Accounts CustomeGroup";
		this.AccountBudget.Name = "AccountBudget";
		this.AccountBudget.Size = new System.Drawing.Size(215, 22);
		this.AccountBudget.Text = "Accounts Budget";
		this.AcfrmChequebookRegistration.Name = "AcfrmChequebookRegistration";
		this.AcfrmChequebookRegistration.Size = new System.Drawing.Size(215, 22);
		this.AcfrmChequebookRegistration.Text = "ChequeBook Registration";
		this.AcfrmDefineBank.Name = "AcfrmDefineBank";
		this.AcfrmDefineBank.Size = new System.Drawing.Size(215, 22);
		this.AcfrmDefineBank.Text = "Define Banks";
		this.frmAdvanceAdjustment.Name = "frmAdvanceAdjustment";
		this.frmAdvanceAdjustment.Size = new System.Drawing.Size(215, 22);
		this.frmAdvanceAdjustment.Text = "Advance Adjustment";
		this.frmIncentivePolicy.Name = "frmIncentivePolicy";
		this.frmIncentivePolicy.Size = new System.Drawing.Size(215, 22);
		this.frmIncentivePolicy.Text = "Define Incentive Policy";
		this.InLandFreightAgreement.Name = "InLandFreightAgreement";
		this.InLandFreightAgreement.Size = new System.Drawing.Size(215, 22);
		this.InLandFreightAgreement.Text = "In Land Freight Agreement";
		this.btnAccountsTransaction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[30]
		{
			this.frmAccountsPrematureReceipts, this.ExpenseVoucher, this.frmCashPaymentVoucher, this.frmBankPaymentVoucher, this.PaymentVoucherNew, this.frmCashReceiptVoucher, this.frmBankReceiptVoucher, this.ReceiptsVoucherNew, this.ContraVoucher, this.VoucherEntry,
			this.frmWhtTaxChallanDeposit, this.frmDayBook, this.FreightVoucher, this.toolStripSeparator4, this.frmBillsPayables, this.frmBillsReceivables, this.PaymentByInvoiceVoucherNew, this.PaymentByInvoiceAccount, this.ReceiptByInvoiceAccount, this.toolStripSeparator5,
			this.AcfrmDefPdcManagment, this.PostDatedCheqPaymentVouchers, this.toolStripSeparator6, this.Acfrmfcbankreceipt, this.frmFcyPaymentVoucher, this.frmExportInvoiceVoucher, this.toolStripSeparator7, this.StoreIssuanceFinancial, this.AccountsReconcilation, this.frmCreditNotes
		});
		this.btnAccountsTransaction.Name = "btnAccountsTransaction";
		this.btnAccountsTransaction.Size = new System.Drawing.Size(207, 22);
		this.btnAccountsTransaction.Text = "Accounts Transaction";
		this.frmAccountsPrematureReceipts.Name = "frmAccountsPrematureReceipts";
		this.frmAccountsPrematureReceipts.Size = new System.Drawing.Size(246, 22);
		this.frmAccountsPrematureReceipts.Text = "Accounts Premature Receipts";
		this.ExpenseVoucher.Name = "ExpenseVoucher";
		this.ExpenseVoucher.Size = new System.Drawing.Size(246, 22);
		this.ExpenseVoucher.Text = "Expense Voucher";
		this.frmCashPaymentVoucher.Name = "frmCashPaymentVoucher";
		this.frmCashPaymentVoucher.Size = new System.Drawing.Size(246, 22);
		this.frmCashPaymentVoucher.Text = "Cash Payment Voucher";
		this.frmBankPaymentVoucher.Name = "frmBankPaymentVoucher";
		this.frmBankPaymentVoucher.Size = new System.Drawing.Size(246, 22);
		this.frmBankPaymentVoucher.Text = "Bank Payment Voucher";
		this.PaymentVoucherNew.Name = "PaymentVoucherNew";
		this.PaymentVoucherNew.Size = new System.Drawing.Size(246, 22);
		this.PaymentVoucherNew.Text = "Payment Voucher";
		this.frmCashReceiptVoucher.Name = "frmCashReceiptVoucher";
		this.frmCashReceiptVoucher.Size = new System.Drawing.Size(246, 22);
		this.frmCashReceiptVoucher.Text = "Cash Receipt Voucher";
		this.frmBankReceiptVoucher.Name = "frmBankReceiptVoucher";
		this.frmBankReceiptVoucher.Size = new System.Drawing.Size(246, 22);
		this.frmBankReceiptVoucher.Text = "Bank Receipt Voucher";
		this.ReceiptsVoucherNew.Name = "ReceiptsVoucherNew";
		this.ReceiptsVoucherNew.Size = new System.Drawing.Size(246, 22);
		this.ReceiptsVoucherNew.Text = "Receipt Voucher";
		this.ContraVoucher.Name = "ContraVoucher";
		this.ContraVoucher.Size = new System.Drawing.Size(246, 22);
		this.ContraVoucher.Text = "Contra Voucher";
		this.VoucherEntry.Name = "VoucherEntry";
		this.VoucherEntry.Size = new System.Drawing.Size(246, 22);
		this.VoucherEntry.Text = "Journal Voucher";
		this.frmWhtTaxChallanDeposit.Name = "frmWhtTaxChallanDeposit";
		this.frmWhtTaxChallanDeposit.Size = new System.Drawing.Size(246, 22);
		this.frmWhtTaxChallanDeposit.Text = "Wht Challan Deposit";
		this.frmDayBook.Name = "frmDayBook";
		this.frmDayBook.Size = new System.Drawing.Size(246, 22);
		this.frmDayBook.Text = "Day Book";
		this.FreightVoucher.Name = "FreightVoucher";
		this.FreightVoucher.Size = new System.Drawing.Size(246, 22);
		this.FreightVoucher.Text = "Freight Payment Voucher";
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(243, 6);
		this.frmBillsPayables.Name = "frmBillsPayables";
		this.frmBillsPayables.Size = new System.Drawing.Size(246, 22);
		this.frmBillsPayables.Text = "Bills of Payabels";
		this.frmBillsReceivables.Name = "frmBillsReceivables";
		this.frmBillsReceivables.Size = new System.Drawing.Size(246, 22);
		this.frmBillsReceivables.Text = "Bills of Receivables";
		this.PaymentByInvoiceVoucherNew.Name = "PaymentByInvoiceVoucherNew";
		this.PaymentByInvoiceVoucherNew.Size = new System.Drawing.Size(246, 22);
		this.PaymentByInvoiceVoucherNew.Text = "Payment By Invoice New";
		this.PaymentByInvoiceAccount.Name = "PaymentByInvoiceAccount";
		this.PaymentByInvoiceAccount.Size = new System.Drawing.Size(246, 22);
		this.PaymentByInvoiceAccount.Text = "Payment By Invoice";
		this.PaymentByInvoiceAccount.Visible = false;
		this.ReceiptByInvoiceAccount.Name = "ReceiptByInvoiceAccount";
		this.ReceiptByInvoiceAccount.Size = new System.Drawing.Size(246, 22);
		this.ReceiptByInvoiceAccount.Text = "Receipts By Invoice";
		this.ReceiptByInvoiceAccount.Visible = false;
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(243, 6);
		this.AcfrmDefPdcManagment.Name = "AcfrmDefPdcManagment";
		this.AcfrmDefPdcManagment.Size = new System.Drawing.Size(246, 22);
		this.AcfrmDefPdcManagment.Text = "PDC Cheques Receipts";
		this.PostDatedCheqPaymentVouchers.Name = "PostDatedCheqPaymentVouchers";
		this.PostDatedCheqPaymentVouchers.Size = new System.Drawing.Size(246, 22);
		this.PostDatedCheqPaymentVouchers.Text = "PDC Payment Voucher";
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(243, 6);
		this.Acfrmfcbankreceipt.Name = "Acfrmfcbankreceipt";
		this.Acfrmfcbankreceipt.Size = new System.Drawing.Size(246, 22);
		this.Acfrmfcbankreceipt.Text = "Fc Bank Receipts";
		this.frmFcyPaymentVoucher.Name = "frmFcyPaymentVoucher";
		this.frmFcyPaymentVoucher.Size = new System.Drawing.Size(246, 22);
		this.frmFcyPaymentVoucher.Text = "Fc Bank Payment";
		this.frmExportInvoiceVoucher.Name = "frmExportInvoiceVoucher";
		this.frmExportInvoiceVoucher.Size = new System.Drawing.Size(246, 22);
		this.frmExportInvoiceVoucher.Text = "Export Invoice Voucher";
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(243, 6);
		this.StoreIssuanceFinancial.Name = "StoreIssuanceFinancial";
		this.StoreIssuanceFinancial.Size = new System.Drawing.Size(246, 22);
		this.StoreIssuanceFinancial.Text = "Store Issuance Voucher";
		this.AccountsReconcilation.Name = "AccountsReconcilation";
		this.AccountsReconcilation.Size = new System.Drawing.Size(246, 22);
		this.AccountsReconcilation.Text = "Accounts Reconciliation";
		this.frmCreditNotes.Name = "frmCreditNotes";
		this.frmCreditNotes.Size = new System.Drawing.Size(246, 22);
		this.frmCreditNotes.Text = "Customer Incentive Credit Notes";
		this.btnInvDefinition.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.InvDefrmAddItem, this.AcfrmDefineLots, this.InvDeffrmWarehouse, this.btnPricingScheduleForGatePurchase, this.InvfrmPriceSchedule, this.frmItemsCustomizedGroups, this.frmItemsReorderSchedule, this.frmPosdefineitem });
		this.btnInvDefinition.Name = "btnInvDefinition";
		this.btnInvDefinition.Size = new System.Drawing.Size(207, 22);
		this.btnInvDefinition.Text = "Inventory Definiton";
		this.InvDefrmAddItem.Name = "InvDefrmAddItem";
		this.InvDefrmAddItem.Size = new System.Drawing.Size(257, 22);
		this.InvDefrmAddItem.Text = "Add Item";
		this.AcfrmDefineLots.Name = "AcfrmDefineLots";
		this.AcfrmDefineLots.Size = new System.Drawing.Size(257, 22);
		this.AcfrmDefineLots.Text = "Define Lots";
		this.InvDeffrmWarehouse.Name = "InvDeffrmWarehouse";
		this.InvDeffrmWarehouse.Size = new System.Drawing.Size(257, 22);
		this.InvDeffrmWarehouse.Text = "WareHouse Define";
		this.btnPricingScheduleForGatePurchase.Name = "btnPricingScheduleForGatePurchase";
		this.btnPricingScheduleForGatePurchase.Size = new System.Drawing.Size(257, 22);
		this.btnPricingScheduleForGatePurchase.Text = "Pricing Schedule For GatePurchase";
		this.InvfrmPriceSchedule.Name = "InvfrmPriceSchedule";
		this.InvfrmPriceSchedule.Size = new System.Drawing.Size(257, 22);
		this.InvfrmPriceSchedule.Text = "Price Schedule";
		this.frmItemsCustomizedGroups.Name = "frmItemsCustomizedGroups";
		this.frmItemsCustomizedGroups.Size = new System.Drawing.Size(257, 22);
		this.frmItemsCustomizedGroups.Text = "Item Customized Group";
		this.frmItemsReorderSchedule.Name = "frmItemsReorderSchedule";
		this.frmItemsReorderSchedule.Size = new System.Drawing.Size(257, 22);
		this.frmItemsReorderSchedule.Text = "ItemRecordSchedule";
		this.frmPosdefineitem.Name = "frmPosdefineitem";
		this.frmPosdefineitem.Size = new System.Drawing.Size(257, 22);
		this.frmPosdefineitem.Text = "Add Item POS";
		this.frmPosdefineitem.Visible = false;
		this.btnSupPurchase.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.PaddyPurchase, this.supfrmDefineSupplier, this.PurchsaeOrder, this.MarketPurchaseOrder, this.SupplyOrderPurchase, this.InwardGatePass, this.InvFrmGRN, this.InvFrmGRNDirect, this.MarketGrn, this.InvfrmPurchaseInvoice,
			this.InvfrmPurchasedirectInvoice, this.PurchaseInvoiceAgainstGrnDirect, this.InvfrmPurchaseReturn, this.frmOpeningStockBlancing
		});
		this.btnSupPurchase.Name = "btnSupPurchase";
		this.btnSupPurchase.Size = new System.Drawing.Size(207, 22);
		this.btnSupPurchase.Text = "Supplier Purchases";
		this.PaddyPurchase.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.frmGatePassInwardWithDetail, this.frmGrnForGate, this.frmPurchaseInvoiceForGate });
		this.PaddyPurchase.Name = "PaddyPurchase";
		this.PaddyPurchase.Size = new System.Drawing.Size(262, 22);
		this.PaddyPurchase.Text = "Paddy Purchase";
		this.frmGatePassInwardWithDetail.Name = "frmGatePassInwardWithDetail";
		this.frmGatePassInwardWithDetail.Size = new System.Drawing.Size(243, 22);
		this.frmGatePassInwardWithDetail.Text = "Gate Pass Inward For Gate";
		this.frmGrnForGate.Name = "frmGrnForGate";
		this.frmGrnForGate.Size = new System.Drawing.Size(243, 22);
		this.frmGrnForGate.Text = "Goods Receiving Notes For Gate";
		this.frmPurchaseInvoiceForGate.Name = "frmPurchaseInvoiceForGate";
		this.frmPurchaseInvoiceForGate.Size = new System.Drawing.Size(243, 22);
		this.frmPurchaseInvoiceForGate.Text = "Purchase Invoice For Gate";
		this.supfrmDefineSupplier.Name = "supfrmDefineSupplier";
		this.supfrmDefineSupplier.Size = new System.Drawing.Size(262, 22);
		this.supfrmDefineSupplier.Text = "Define Suppliers";
		this.PurchsaeOrder.Name = "PurchsaeOrder";
		this.PurchsaeOrder.Size = new System.Drawing.Size(262, 22);
		this.PurchsaeOrder.Text = "Purchase Order";
		this.MarketPurchaseOrder.Name = "MarketPurchaseOrder";
		this.MarketPurchaseOrder.Size = new System.Drawing.Size(262, 22);
		this.MarketPurchaseOrder.Text = "Market Purchase Order";
		this.SupplyOrderPurchase.Name = "SupplyOrderPurchase";
		this.SupplyOrderPurchase.Size = new System.Drawing.Size(262, 22);
		this.SupplyOrderPurchase.Text = "Supply Order";
		this.InwardGatePass.Name = "InwardGatePass";
		this.InwardGatePass.Size = new System.Drawing.Size(262, 22);
		this.InwardGatePass.Text = "Gate Pass Inward";
		this.InvFrmGRN.Name = "InvFrmGRN";
		this.InvFrmGRN.Size = new System.Drawing.Size(262, 22);
		this.InvFrmGRN.Text = "Goods Receiving Notes";
		this.InvFrmGRNDirect.Name = "InvFrmGRNDirect";
		this.InvFrmGRNDirect.Size = new System.Drawing.Size(262, 22);
		this.InvFrmGRNDirect.Text = "Goods Receving Notes Direct";
		this.MarketGrn.Name = "MarketGrn";
		this.MarketGrn.Size = new System.Drawing.Size(262, 22);
		this.MarketGrn.Text = "Market Grn";
		this.InvfrmPurchaseInvoice.Name = "InvfrmPurchaseInvoice";
		this.InvfrmPurchaseInvoice.Size = new System.Drawing.Size(262, 22);
		this.InvfrmPurchaseInvoice.Text = "Purchase Invoice";
		this.InvfrmPurchasedirectInvoice.Name = "InvfrmPurchasedirectInvoice";
		this.InvfrmPurchasedirectInvoice.Size = new System.Drawing.Size(262, 22);
		this.InvfrmPurchasedirectInvoice.Text = "Purchase Direct Invoice";
		this.PurchaseInvoiceAgainstGrnDirect.Name = "PurchaseInvoiceAgainstGrnDirect";
		this.PurchaseInvoiceAgainstGrnDirect.Size = new System.Drawing.Size(262, 22);
		this.PurchaseInvoiceAgainstGrnDirect.Text = "Purchase Invoice Against Grn Direct";
		this.InvfrmPurchaseReturn.Name = "InvfrmPurchaseReturn";
		this.InvfrmPurchaseReturn.Size = new System.Drawing.Size(262, 22);
		this.InvfrmPurchaseReturn.Text = "Purchase Return";
		this.frmOpeningStockBlancing.Name = "frmOpeningStockBlancing";
		this.frmOpeningStockBlancing.Size = new System.Drawing.Size(262, 22);
		this.frmOpeningStockBlancing.Text = "Stock Opening Balance";
		this.btnWeightBridge.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.frmWeightbridge, this.WeightbridgeMannual });
		this.btnWeightBridge.Name = "btnWeightBridge";
		this.btnWeightBridge.Size = new System.Drawing.Size(207, 22);
		this.btnWeightBridge.Text = "Weigh Bridge";
		this.frmWeightbridge.Name = "frmWeightbridge";
		this.frmWeightbridge.Size = new System.Drawing.Size(188, 22);
		this.frmWeightbridge.Text = "Weigh Bridge Auto";
		this.WeightbridgeMannual.Name = "WeightbridgeMannual";
		this.WeightbridgeMannual.Size = new System.Drawing.Size(188, 22);
		this.WeightbridgeMannual.Text = "Weigh Bridge Manual";
		this.btnCustomerSale.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.SaleOrder, this.DeliveryOrder, this.OutwardGatePass, this.frmDriverBio, this.InvfrmGDN, this.InvfrmSaleInvoice, this.InvfrmSaleInvoiceReturn, this.GoodsDispatchNotesWithoutWeighBridge, this.InvfrmInvSaleInvoiceDirect, this.SaleInvoicepartyStockReserve,
			this.PointOfSaleInvoice, this.SaleOrderExport, this.InvfrmSaleInvoiceExport, this.frmSaleInvoiceDirect139
		});
		this.btnCustomerSale.Name = "btnCustomerSale";
		this.btnCustomerSale.Size = new System.Drawing.Size(207, 22);
		this.btnCustomerSale.Text = "Customer Sales";
		this.SaleOrder.Name = "SaleOrder";
		this.SaleOrder.Size = new System.Drawing.Size(308, 22);
		this.SaleOrder.Text = "Sale Order";
		this.DeliveryOrder.Name = "DeliveryOrder";
		this.DeliveryOrder.Size = new System.Drawing.Size(308, 22);
		this.DeliveryOrder.Text = "Delivery Order";
		this.OutwardGatePass.Name = "OutwardGatePass";
		this.OutwardGatePass.Size = new System.Drawing.Size(308, 22);
		this.OutwardGatePass.Text = "Gate Pass Outward";
		this.frmDriverBio.Name = "frmDriverBio";
		this.frmDriverBio.Size = new System.Drawing.Size(308, 22);
		this.frmDriverBio.Text = "Driver Biodata";
		this.InvfrmGDN.Name = "InvfrmGDN";
		this.InvfrmGDN.Size = new System.Drawing.Size(308, 22);
		this.InvfrmGDN.Text = "Goods Dispatch Notes";
		this.InvfrmSaleInvoice.Name = "InvfrmSaleInvoice";
		this.InvfrmSaleInvoice.Size = new System.Drawing.Size(308, 22);
		this.InvfrmSaleInvoice.Text = "Sale Invoice";
		this.InvfrmSaleInvoiceReturn.Name = "InvfrmSaleInvoiceReturn";
		this.InvfrmSaleInvoiceReturn.Size = new System.Drawing.Size(308, 22);
		this.InvfrmSaleInvoiceReturn.Text = "Sale Invoice Return";
		this.GoodsDispatchNotesWithoutWeighBridge.Name = "GoodsDispatchNotesWithoutWeighBridge";
		this.GoodsDispatchNotesWithoutWeighBridge.Size = new System.Drawing.Size(308, 22);
		this.GoodsDispatchNotesWithoutWeighBridge.Text = "Goods Dispatch Notes Without WeighBridge";
		this.InvfrmInvSaleInvoiceDirect.Name = "InvfrmInvSaleInvoiceDirect";
		this.InvfrmInvSaleInvoiceDirect.Size = new System.Drawing.Size(308, 22);
		this.InvfrmInvSaleInvoiceDirect.Text = "Sale Invoice Direct";
		this.SaleInvoicepartyStockReserve.Name = "SaleInvoicepartyStockReserve";
		this.SaleInvoicepartyStockReserve.Size = new System.Drawing.Size(308, 22);
		this.SaleInvoicepartyStockReserve.Text = "Sale Invoice Party Stock Reserve";
		this.PointOfSaleInvoice.Name = "PointOfSaleInvoice";
		this.PointOfSaleInvoice.Size = new System.Drawing.Size(308, 22);
		this.PointOfSaleInvoice.Text = "POS Invoice";
		this.PointOfSaleInvoice.Visible = false;
		this.SaleOrderExport.Name = "SaleOrderExport";
		this.SaleOrderExport.Size = new System.Drawing.Size(308, 22);
		this.SaleOrderExport.Text = "Sale Order Export";
		this.SaleOrderExport.Visible = false;
		this.InvfrmSaleInvoiceExport.Name = "InvfrmSaleInvoiceExport";
		this.InvfrmSaleInvoiceExport.Size = new System.Drawing.Size(308, 22);
		this.InvfrmSaleInvoiceExport.Text = "Sale Invoice Export";
		this.InvfrmSaleInvoiceExport.Visible = false;
		this.frmSaleInvoiceDirect139.Name = "frmSaleInvoiceDirect139";
		this.frmSaleInvoiceDirect139.Size = new System.Drawing.Size(308, 22);
		this.frmSaleInvoiceDirect139.Text = "Sale Invoice Direct_139";
		this.btnStockWare.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.frmProductionJobOrder, this.frmFoodProduction, this.invfrmStockConversionProduction, this.frmBillOfMaterial, this.frmBillOfMaterialManufacturing, this.FoodProductionWithValues, this.BillOfMaterialWiseProduction, this.frmProductionCumPackMaterialConsumption, this.ProductionPreCostingAndJobOrder, this.frmBoilerConsumption });
		this.btnStockWare.Name = "btnStockWare";
		this.btnStockWare.Size = new System.Drawing.Size(207, 22);
		this.btnStockWare.Text = "Production";
		this.frmProductionJobOrder.Name = "frmProductionJobOrder";
		this.frmProductionJobOrder.Size = new System.Drawing.Size(309, 22);
		this.frmProductionJobOrder.Text = "Production Job Order";
		this.frmFoodProduction.Name = "frmFoodProduction";
		this.frmFoodProduction.Size = new System.Drawing.Size(309, 22);
		this.frmFoodProduction.Text = "Production";
		this.invfrmStockConversionProduction.Name = "invfrmStockConversionProduction";
		this.invfrmStockConversionProduction.Size = new System.Drawing.Size(309, 22);
		this.invfrmStockConversionProduction.Text = "Stock Conversion";
		this.frmBillOfMaterial.Name = "frmBillOfMaterial";
		this.frmBillOfMaterial.Size = new System.Drawing.Size(309, 22);
		this.frmBillOfMaterial.Text = "Bill Of Material";
		this.frmBillOfMaterialManufacturing.Name = "frmBillOfMaterialManufacturing";
		this.frmBillOfMaterialManufacturing.Size = new System.Drawing.Size(309, 22);
		this.frmBillOfMaterialManufacturing.Text = "Bill Of Material Manufacturing";
		this.FoodProductionWithValues.Name = "FoodProductionWithValues";
		this.FoodProductionWithValues.Size = new System.Drawing.Size(309, 22);
		this.FoodProductionWithValues.Text = "ProductionNew";
		this.BillOfMaterialWiseProduction.Name = "BillOfMaterialWiseProduction";
		this.BillOfMaterialWiseProduction.Size = new System.Drawing.Size(309, 22);
		this.BillOfMaterialWiseProduction.Text = "Production Bill Of Material Wise";
		this.frmProductionCumPackMaterialConsumption.Name = "frmProductionCumPackMaterialConsumption";
		this.frmProductionCumPackMaterialConsumption.Size = new System.Drawing.Size(309, 22);
		this.frmProductionCumPackMaterialConsumption.Text = "Production Cum PackMaterial Consumption";
		this.ProductionPreCostingAndJobOrder.Name = "ProductionPreCostingAndJobOrder";
		this.ProductionPreCostingAndJobOrder.Size = new System.Drawing.Size(309, 22);
		this.ProductionPreCostingAndJobOrder.Text = "Production Pre Costing And JobOrder";
		this.frmBoilerConsumption.Name = "frmBoilerConsumption";
		this.frmBoilerConsumption.Size = new System.Drawing.Size(309, 22);
		this.frmBoilerConsumption.Text = "Boiler Consumption";
		this.btnLabAnalysis.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.InvLabAnalysisItems, this.InvLabAnalysisGroup, this.InvLabGroupAnalysisStandards, this.InvLabSampleLogRegister, this.InvLabSampleAnalysis, this.InvLabPurchaseAnalysis, this.InvLabAnalysisInProcess, this.InvLabSaleAnalysis, this.InvLabStandardPolicyForDeduction });
		this.btnLabAnalysis.Name = "btnLabAnalysis";
		this.btnLabAnalysis.Size = new System.Drawing.Size(207, 22);
		this.btnLabAnalysis.Text = "Lab Analysis";
		this.InvLabAnalysisItems.Name = "InvLabAnalysisItems";
		this.InvLabAnalysisItems.Size = new System.Drawing.Size(305, 22);
		this.InvLabAnalysisItems.Text = "Analysis Perameter";
		this.InvLabAnalysisGroup.Name = "InvLabAnalysisGroup";
		this.InvLabAnalysisGroup.Size = new System.Drawing.Size(305, 22);
		this.InvLabAnalysisGroup.Text = "Analysis Group";
		this.InvLabGroupAnalysisStandards.Name = "InvLabGroupAnalysisStandards";
		this.InvLabGroupAnalysisStandards.Size = new System.Drawing.Size(305, 22);
		this.InvLabGroupAnalysisStandards.Text = "Group Standard Analysis";
		this.InvLabSampleLogRegister.Name = "InvLabSampleLogRegister";
		this.InvLabSampleLogRegister.Size = new System.Drawing.Size(305, 22);
		this.InvLabSampleLogRegister.Text = "Log Register";
		this.InvLabSampleAnalysis.Name = "InvLabSampleAnalysis";
		this.InvLabSampleAnalysis.Size = new System.Drawing.Size(305, 22);
		this.InvLabSampleAnalysis.Text = "Sample Analysis";
		this.InvLabPurchaseAnalysis.Name = "InvLabPurchaseAnalysis";
		this.InvLabPurchaseAnalysis.Size = new System.Drawing.Size(305, 22);
		this.InvLabPurchaseAnalysis.Text = "Purcahse Analysis";
		this.InvLabAnalysisInProcess.Name = "InvLabAnalysisInProcess";
		this.InvLabAnalysisInProcess.Size = new System.Drawing.Size(305, 22);
		this.InvLabAnalysisInProcess.Text = "In Process Analysis";
		this.InvLabSaleAnalysis.Name = "InvLabSaleAnalysis";
		this.InvLabSaleAnalysis.Size = new System.Drawing.Size(305, 22);
		this.InvLabSaleAnalysis.Text = "Sale Analysis";
		this.InvLabStandardPolicyForDeduction.Name = "InvLabStandardPolicyForDeduction";
		this.InvLabStandardPolicyForDeduction.Size = new System.Drawing.Size(305, 22);
		this.InvLabStandardPolicyForDeduction.Text = "Lab Analysis Standard Policy for Deductions";
		this.btnwages.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.frmContractorWagesAccount, this.frmContractWagesSchedule, this.frmwagesBillHeader, this.frmMoveOrder });
		this.btnwages.Name = "btnwages";
		this.btnwages.Size = new System.Drawing.Size(207, 22);
		this.btnwages.Text = "Contractor Wages";
		this.frmContractorWagesAccount.Name = "frmContractorWagesAccount";
		this.frmContractorWagesAccount.Size = new System.Drawing.Size(200, 22);
		this.frmContractorWagesAccount.Text = "Wages Account";
		this.frmContractWagesSchedule.Name = "frmContractWagesSchedule";
		this.frmContractWagesSchedule.Size = new System.Drawing.Size(200, 22);
		this.frmContractWagesSchedule.Text = "Wages Schedule";
		this.frmwagesBillHeader.Name = "frmwagesBillHeader";
		this.frmwagesBillHeader.Size = new System.Drawing.Size(200, 22);
		this.frmwagesBillHeader.Text = "Purchase / Sales Wages ";
		this.frmMoveOrder.Name = "frmMoveOrder";
		this.frmMoveOrder.Size = new System.Drawing.Size(200, 22);
		this.frmMoveOrder.Text = "Move Order";
		this.frmMoveOrder.Visible = false;
		this.btnExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[16]
		{
			this.ExportPartyDefines, this.EximPerformaInvoice, this.EximLcOrder, this.FrmExportSalesContract, this.FrmExportSalesContractSchedule, this.toolStripSeparator1, this.ExpfrmEformRegistration, this.ExpfrmFinancialInsturment, this.EximInvoice, this.EximShippingBookingInfo,
			this.EximPreProductionLab, this.ExportDeliveryOrder, this.EximForwarding, this.EximForwardingWithoutWeighBridge, this.btnExportForwardingDirect, this.EximBillOfLading
		});
		this.btnExport.Name = "btnExport";
		this.btnExport.Size = new System.Drawing.Size(207, 22);
		this.btnExport.Text = "Export";
		this.ExportPartyDefines.Name = "ExportPartyDefines";
		this.ExportPartyDefines.Size = new System.Drawing.Size(288, 22);
		this.ExportPartyDefines.Text = "Export Parties Define";
		this.EximPerformaInvoice.Name = "EximPerformaInvoice";
		this.EximPerformaInvoice.Size = new System.Drawing.Size(288, 22);
		this.EximPerformaInvoice.Text = "Performa Invoice";
		this.EximLcOrder.Name = "EximLcOrder";
		this.EximLcOrder.Size = new System.Drawing.Size(288, 22);
		this.EximLcOrder.Text = "Lc Order";
		this.FrmExportSalesContract.Name = "FrmExportSalesContract";
		this.FrmExportSalesContract.Size = new System.Drawing.Size(288, 22);
		this.FrmExportSalesContract.Text = "Sales Contract";
		this.FrmExportSalesContractSchedule.Name = "FrmExportSalesContractSchedule";
		this.FrmExportSalesContractSchedule.Size = new System.Drawing.Size(288, 22);
		this.FrmExportSalesContractSchedule.Text = "Salec Contract Schedule";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(285, 6);
		this.ExpfrmEformRegistration.Name = "ExpfrmEformRegistration";
		this.ExpfrmEformRegistration.Size = new System.Drawing.Size(288, 22);
		this.ExpfrmEformRegistration.Text = "E-Form Registration";
		this.ExpfrmFinancialInsturment.Name = "ExpfrmFinancialInsturment";
		this.ExpfrmFinancialInsturment.Size = new System.Drawing.Size(288, 22);
		this.ExpfrmFinancialInsturment.Text = "Financial Instruments";
		this.EximInvoice.Name = "EximInvoice";
		this.EximInvoice.Size = new System.Drawing.Size(288, 22);
		this.EximInvoice.Text = "Commercial Invoice";
		this.EximShippingBookingInfo.Name = "EximShippingBookingInfo";
		this.EximShippingBookingInfo.Size = new System.Drawing.Size(288, 22);
		this.EximShippingBookingInfo.Text = "ShippingBookingInfo";
		this.EximPreProductionLab.Name = "EximPreProductionLab";
		this.EximPreProductionLab.Size = new System.Drawing.Size(288, 22);
		this.EximPreProductionLab.Text = "PreProductionLotInspection";
		this.ExportDeliveryOrder.Name = "ExportDeliveryOrder";
		this.ExportDeliveryOrder.Size = new System.Drawing.Size(288, 22);
		this.ExportDeliveryOrder.Text = "Export Delivery Order";
		this.EximForwarding.Name = "EximForwarding";
		this.EximForwarding.Size = new System.Drawing.Size(288, 22);
		this.EximForwarding.Text = "Export Forwarding";
		this.EximForwardingWithoutWeighBridge.Name = "EximForwardingWithoutWeighBridge";
		this.EximForwardingWithoutWeighBridge.Size = new System.Drawing.Size(288, 22);
		this.EximForwardingWithoutWeighBridge.Text = "Export Forwarding Without Weighbridge";
		this.btnExportForwardingDirect.Name = "btnExportForwardingDirect";
		this.btnExportForwardingDirect.Size = new System.Drawing.Size(288, 22);
		this.btnExportForwardingDirect.Text = "Export Forwarding Direct";
		this.EximBillOfLading.Name = "EximBillOfLading";
		this.EximBillOfLading.Size = new System.Drawing.Size(288, 22);
		this.EximBillOfLading.Text = "Bill of Lading";
		this.btnHRM.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnProfileManagement, this.btnPolicyManegement, this.btnEmployeeManagement, this.btnDeviceManagement, this.btnAttendanceManagement, this.btnLeaveManagement, this.btnCPLLeaveMangement, this.btnLoanManagement, this.btnOverTimeManagement, this.btnApprovalManagement,
			this.btnPayroll, this.btnGlobalEmployment
		});
		this.btnHRM.Name = "btnHRM";
		this.btnHRM.Size = new System.Drawing.Size(207, 22);
		this.btnHRM.Text = "HRM";
		this.btnProfileManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.GeneralSetting, this.EmployeeCategory, this.Benefit, this.Designation, this.Department, this.Section, this.Location, this.ProfileType, this.Profile, this.EmployeeGroup });
		this.btnProfileManagement.Name = "btnProfileManagement";
		this.btnProfileManagement.Size = new System.Drawing.Size(209, 22);
		this.btnProfileManagement.Text = "Profile Management";
		this.GeneralSetting.Name = "GeneralSetting";
		this.GeneralSetting.Size = new System.Drawing.Size(180, 22);
		this.GeneralSetting.Text = "General Setting";
		this.EmployeeCategory.Name = "EmployeeCategory";
		this.EmployeeCategory.Size = new System.Drawing.Size(180, 22);
		this.EmployeeCategory.Text = "Employee Category ";
		this.Benefit.Name = "Benefit";
		this.Benefit.Size = new System.Drawing.Size(180, 22);
		this.Benefit.Text = "Benefit";
		this.Designation.Name = "Designation";
		this.Designation.Size = new System.Drawing.Size(180, 22);
		this.Designation.Text = "Designation";
		this.Department.Name = "Department";
		this.Department.Size = new System.Drawing.Size(180, 22);
		this.Department.Text = "Department";
		this.Section.Name = "Section";
		this.Section.Size = new System.Drawing.Size(180, 22);
		this.Section.Text = "Section";
		this.Location.Name = "Location";
		this.Location.Size = new System.Drawing.Size(180, 22);
		this.Location.Text = "Location";
		this.ProfileType.Name = "ProfileType";
		this.ProfileType.Size = new System.Drawing.Size(180, 22);
		this.ProfileType.Text = "Profile Type";
		this.Profile.Name = "Profile";
		this.Profile.Size = new System.Drawing.Size(180, 22);
		this.Profile.Text = "Profile";
		this.EmployeeGroup.Name = "EmployeeGroup";
		this.EmployeeGroup.Size = new System.Drawing.Size(180, 22);
		this.EmployeeGroup.Text = "Employee Group";
		this.btnPolicyManegement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.GeneralPolicy, this.SalaryBreakupPolicy, this.LeaveQuotaPolicy, this.EOBIPolicy, this.ProvidentFundPolicy, this.SocialSecurityPolicy, this.IncomeTaxPolicy });
		this.btnPolicyManegement.Name = "btnPolicyManegement";
		this.btnPolicyManegement.Size = new System.Drawing.Size(209, 22);
		this.btnPolicyManegement.Text = "Policy Management";
		this.GeneralPolicy.Name = "GeneralPolicy";
		this.GeneralPolicy.Size = new System.Drawing.Size(190, 22);
		this.GeneralPolicy.Text = "General Policy";
		this.SalaryBreakupPolicy.Name = "SalaryBreakupPolicy";
		this.SalaryBreakupPolicy.Size = new System.Drawing.Size(190, 22);
		this.SalaryBreakupPolicy.Text = "Salary Breakup Policy";
		this.LeaveQuotaPolicy.Name = "LeaveQuotaPolicy";
		this.LeaveQuotaPolicy.Size = new System.Drawing.Size(190, 22);
		this.LeaveQuotaPolicy.Text = "Leave Quota Policy";
		this.EOBIPolicy.Name = "EOBIPolicy";
		this.EOBIPolicy.Size = new System.Drawing.Size(190, 22);
		this.EOBIPolicy.Text = "E.O.B.I Policy";
		this.ProvidentFundPolicy.Name = "ProvidentFundPolicy";
		this.ProvidentFundPolicy.Size = new System.Drawing.Size(190, 22);
		this.ProvidentFundPolicy.Text = "Provident Fund Policy";
		this.SocialSecurityPolicy.Name = "SocialSecurityPolicy";
		this.SocialSecurityPolicy.Size = new System.Drawing.Size(190, 22);
		this.SocialSecurityPolicy.Text = "Social Security Policy";
		this.IncomeTaxPolicy.Name = "IncomeTaxPolicy";
		this.IncomeTaxPolicy.Size = new System.Drawing.Size(190, 22);
		this.IncomeTaxPolicy.Text = "Income Tax Policy";
		this.btnEmployeeManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.TrialRegistration, this.EmployeeRegistration, this.EmployeeNotice, this.EmployeeApproval, this.ProbationApproval });
		this.btnEmployeeManagement.Name = "btnEmployeeManagement";
		this.btnEmployeeManagement.Size = new System.Drawing.Size(209, 22);
		this.btnEmployeeManagement.Text = "Employee Management";
		this.TrialRegistration.Name = "TrialRegistration";
		this.TrialRegistration.Size = new System.Drawing.Size(192, 22);
		this.TrialRegistration.Text = "Trial Registration";
		this.EmployeeRegistration.Name = "EmployeeRegistration";
		this.EmployeeRegistration.Size = new System.Drawing.Size(192, 22);
		this.EmployeeRegistration.Text = "Employee Registration";
		this.EmployeeNotice.Name = "EmployeeNotice";
		this.EmployeeNotice.Size = new System.Drawing.Size(192, 22);
		this.EmployeeNotice.Text = "Employee Notice";
		this.EmployeeApproval.Name = "EmployeeApproval";
		this.EmployeeApproval.Size = new System.Drawing.Size(192, 22);
		this.EmployeeApproval.Text = "Employee Approval";
		this.ProbationApproval.Name = "ProbationApproval";
		this.ProbationApproval.Size = new System.Drawing.Size(192, 22);
		this.ProbationApproval.Text = "Probation Approval";
		this.btnDeviceManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.DeviceConfiguration, this.PullAttendanceByMachine, this.PullAttendance });
		this.btnDeviceManagement.Name = "btnDeviceManagement";
		this.btnDeviceManagement.Size = new System.Drawing.Size(209, 22);
		this.btnDeviceManagement.Text = "Device Management";
		this.DeviceConfiguration.Name = "DeviceConfiguration";
		this.DeviceConfiguration.Size = new System.Drawing.Size(223, 22);
		this.DeviceConfiguration.Text = "Device Configuration";
		this.PullAttendanceByMachine.Name = "PullAttendanceByMachine";
		this.PullAttendanceByMachine.Size = new System.Drawing.Size(223, 22);
		this.PullAttendanceByMachine.Text = "Pull Attendance By Machine";
		this.PullAttendance.Name = "PullAttendance";
		this.PullAttendance.Size = new System.Drawing.Size(223, 22);
		this.PullAttendance.Text = "Pull Attendance";
		this.btnAttendanceManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.DefineShift, this.ShiftLocation, this.ShiftTiming, this.Holiday, this.DutyRoaster, this.EmployeeRoaster, this.DailyAttendance, this.ManualAttendance });
		this.btnAttendanceManagement.Name = "btnAttendanceManagement";
		this.btnAttendanceManagement.Size = new System.Drawing.Size(209, 22);
		this.btnAttendanceManagement.Text = "Attendance Management";
		this.DefineShift.Name = "DefineShift";
		this.DefineShift.Size = new System.Drawing.Size(178, 22);
		this.DefineShift.Text = "Define Shift";
		this.ShiftLocation.Name = "ShiftLocation";
		this.ShiftLocation.Size = new System.Drawing.Size(178, 22);
		this.ShiftLocation.Text = "Shift Location";
		this.ShiftTiming.Name = "ShiftTiming";
		this.ShiftTiming.Size = new System.Drawing.Size(178, 22);
		this.ShiftTiming.Text = "Shift Timing";
		this.Holiday.Name = "Holiday";
		this.Holiday.Size = new System.Drawing.Size(178, 22);
		this.Holiday.Text = "Holiday";
		this.DutyRoaster.Name = "DutyRoaster";
		this.DutyRoaster.Size = new System.Drawing.Size(178, 22);
		this.DutyRoaster.Text = "Duty Roaster";
		this.EmployeeRoaster.Name = "EmployeeRoaster";
		this.EmployeeRoaster.Size = new System.Drawing.Size(178, 22);
		this.EmployeeRoaster.Text = "Employee Roaster";
		this.DailyAttendance.Name = "DailyAttendance";
		this.DailyAttendance.Size = new System.Drawing.Size(178, 22);
		this.DailyAttendance.Text = "Daily Attendance";
		this.ManualAttendance.Name = "ManualAttendance";
		this.ManualAttendance.Size = new System.Drawing.Size(178, 22);
		this.ManualAttendance.Text = "Manual Attendance";
		this.btnLeaveManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.LeaveOpening, this.EmployeeShortLeave, this.EmployeeLeave, this.LeaveClosing, this.EmployeeSpecialLeave });
		this.btnLeaveManagement.Name = "btnLeaveManagement";
		this.btnLeaveManagement.Size = new System.Drawing.Size(209, 22);
		this.btnLeaveManagement.Text = "Leave Management";
		this.LeaveOpening.Name = "LeaveOpening";
		this.LeaveOpening.Size = new System.Drawing.Size(228, 22);
		this.LeaveOpening.Text = "Leave Opening";
		this.EmployeeShortLeave.Name = "EmployeeShortLeave";
		this.EmployeeShortLeave.Size = new System.Drawing.Size(228, 22);
		this.EmployeeShortLeave.Text = "Employee Short Leave";
		this.EmployeeLeave.Name = "EmployeeLeave";
		this.EmployeeLeave.Size = new System.Drawing.Size(228, 22);
		this.EmployeeLeave.Text = "Employee Leave Request";
		this.LeaveClosing.Name = "LeaveClosing";
		this.LeaveClosing.Size = new System.Drawing.Size(228, 22);
		this.LeaveClosing.Text = "Leave Closing";
		this.EmployeeSpecialLeave.Name = "EmployeeSpecialLeave";
		this.EmployeeSpecialLeave.Size = new System.Drawing.Size(228, 22);
		this.EmployeeSpecialLeave.Text = "Employee CPL Leave Request";
		this.btnCPLLeaveMangement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.frmEmployeeCPLLeaveOpening, this.btnCPLAttendance, this.btnCPLRequest });
		this.btnCPLLeaveMangement.Name = "btnCPLLeaveMangement";
		this.btnCPLLeaveMangement.Size = new System.Drawing.Size(209, 22);
		this.btnCPLLeaveMangement.Text = "CPL Leave Management";
		this.frmEmployeeCPLLeaveOpening.Name = "frmEmployeeCPLLeaveOpening";
		this.frmEmployeeCPLLeaveOpening.Size = new System.Drawing.Size(159, 22);
		this.frmEmployeeCPLLeaveOpening.Text = "Leave Opening";
		this.btnCPLAttendance.Name = "btnCPLAttendance";
		this.btnCPLAttendance.Size = new System.Drawing.Size(159, 22);
		this.btnCPLAttendance.Text = "CPL Attendance";
		this.btnCPLRequest.Name = "btnCPLRequest";
		this.btnCPLRequest.Size = new System.Drawing.Size(159, 22);
		this.btnCPLRequest.Text = "CPL Request";
		this.btnLoanManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.EmployeeLoan, this.EmployeeAdvance });
		this.btnLoanManagement.Name = "btnLoanManagement";
		this.btnLoanManagement.Size = new System.Drawing.Size(209, 22);
		this.btnLoanManagement.Text = "Loan Management";
		this.EmployeeLoan.Name = "EmployeeLoan";
		this.EmployeeLoan.Size = new System.Drawing.Size(175, 22);
		this.EmployeeLoan.Text = "Employee Loan";
		this.EmployeeAdvance.Name = "EmployeeAdvance";
		this.EmployeeAdvance.Size = new System.Drawing.Size(175, 22);
		this.EmployeeAdvance.Text = "Employee Advance";
		this.btnOverTimeManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.OverTimeRequest, this.OTRequestApproval, this.EmployeeOverTime });
		this.btnOverTimeManagement.Name = "btnOverTimeManagement";
		this.btnOverTimeManagement.Size = new System.Drawing.Size(209, 22);
		this.btnOverTimeManagement.Text = "Over Time Management";
		this.OverTimeRequest.Name = "OverTimeRequest";
		this.OverTimeRequest.Size = new System.Drawing.Size(184, 22);
		this.OverTimeRequest.Text = "Over Time Request";
		this.OTRequestApproval.Name = "OTRequestApproval";
		this.OTRequestApproval.Size = new System.Drawing.Size(184, 22);
		this.OTRequestApproval.Text = "OT Request Approval";
		this.EmployeeOverTime.Name = "EmployeeOverTime";
		this.EmployeeOverTime.Size = new System.Drawing.Size(184, 22);
		this.EmployeeOverTime.Text = "Employee Over Time";
		this.btnApprovalManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.AdvanceApproval, this.LoanApproval, this.LeaveApproval, this.frmCPLApproval, this.OverTimeApproval, this.ShortLeaveApproval, this.AttendanceApproval, this.ShortHourApproval, this.AllowanceApproval, this.LoanDeductionApproval });
		this.btnApprovalManagement.Name = "btnApprovalManagement";
		this.btnApprovalManagement.Size = new System.Drawing.Size(209, 22);
		this.btnApprovalManagement.Text = "Approval Management";
		this.AdvanceApproval.Name = "AdvanceApproval";
		this.AdvanceApproval.Size = new System.Drawing.Size(209, 22);
		this.AdvanceApproval.Text = "Advance Approval";
		this.LoanApproval.Name = "LoanApproval";
		this.LoanApproval.Size = new System.Drawing.Size(209, 22);
		this.LoanApproval.Text = "Loan Approval";
		this.LeaveApproval.Name = "LeaveApproval";
		this.LeaveApproval.Size = new System.Drawing.Size(209, 22);
		this.LeaveApproval.Text = "Leave Approval";
		this.frmCPLApproval.Name = "frmCPLApproval";
		this.frmCPLApproval.Size = new System.Drawing.Size(209, 22);
		this.frmCPLApproval.Text = "CPL Approval";
		this.OverTimeApproval.Name = "OverTimeApproval";
		this.OverTimeApproval.Size = new System.Drawing.Size(209, 22);
		this.OverTimeApproval.Text = "Over Time Approval";
		this.ShortLeaveApproval.Name = "ShortLeaveApproval";
		this.ShortLeaveApproval.Size = new System.Drawing.Size(209, 22);
		this.ShortLeaveApproval.Text = "Short Leave Approval";
		this.AttendanceApproval.Name = "AttendanceApproval";
		this.AttendanceApproval.Size = new System.Drawing.Size(209, 22);
		this.AttendanceApproval.Text = "Attendance Approval";
		this.ShortHourApproval.Name = "ShortHourApproval";
		this.ShortHourApproval.Size = new System.Drawing.Size(209, 22);
		this.ShortHourApproval.Text = "Short Hour Approval";
		this.AllowanceApproval.Name = "AllowanceApproval";
		this.AllowanceApproval.Size = new System.Drawing.Size(209, 22);
		this.AllowanceApproval.Text = "Allowance Approval";
		this.LoanDeductionApproval.Name = "LoanDeductionApproval";
		this.LoanDeductionApproval.Size = new System.Drawing.Size(209, 22);
		this.LoanDeductionApproval.Text = "Loan Deduction Approval";
		this.btnPayroll.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.PayrollPosting, this.OvertimePosting, this.DailyWagesPosting, this.EmployeeAllowance, this.EmployeeLateAdjustment, this.EmployeeLoanDeduction, this.EmployeeShortAdjustment });
		this.btnPayroll.Name = "btnPayroll";
		this.btnPayroll.Size = new System.Drawing.Size(209, 22);
		this.btnPayroll.Text = "Payroll Management";
		this.PayrollPosting.Name = "PayrollPosting";
		this.PayrollPosting.Size = new System.Drawing.Size(222, 22);
		this.PayrollPosting.Text = "Payroll Posting";
		this.OvertimePosting.Name = "OvertimePosting";
		this.OvertimePosting.Size = new System.Drawing.Size(222, 22);
		this.OvertimePosting.Text = "Overtime Posting";
		this.DailyWagesPosting.Name = "DailyWagesPosting";
		this.DailyWagesPosting.Size = new System.Drawing.Size(222, 22);
		this.DailyWagesPosting.Text = "Daily Wages Posting";
		this.EmployeeAllowance.Name = "EmployeeAllowance";
		this.EmployeeAllowance.Size = new System.Drawing.Size(222, 22);
		this.EmployeeAllowance.Text = "Employee Allowance";
		this.EmployeeLateAdjustment.Name = "EmployeeLateAdjustment";
		this.EmployeeLateAdjustment.Size = new System.Drawing.Size(222, 22);
		this.EmployeeLateAdjustment.Text = "Employee Late Adjustment";
		this.EmployeeLoanDeduction.Name = "EmployeeLoanDeduction";
		this.EmployeeLoanDeduction.Size = new System.Drawing.Size(222, 22);
		this.EmployeeLoanDeduction.Text = "Employee Loan Deduction";
		this.EmployeeShortAdjustment.Name = "EmployeeShortAdjustment";
		this.EmployeeShortAdjustment.Size = new System.Drawing.Size(222, 22);
		this.EmployeeShortAdjustment.Text = "Employee Short Adjustment";
		this.btnGlobalEmployment.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.EmployeePromotion, this.EmployeeIncrement, this.EmployeeTransfer, this.FullandFinalSettlement, this.BlackListed });
		this.btnGlobalEmployment.Name = "btnGlobalEmployment";
		this.btnGlobalEmployment.Size = new System.Drawing.Size(209, 22);
		this.btnGlobalEmployment.Text = "Global Employment";
		this.EmployeePromotion.Name = "EmployeePromotion";
		this.EmployeePromotion.Size = new System.Drawing.Size(186, 22);
		this.EmployeePromotion.Text = "Employee Promotion";
		this.EmployeeIncrement.Name = "EmployeeIncrement";
		this.EmployeeIncrement.Size = new System.Drawing.Size(186, 22);
		this.EmployeeIncrement.Text = "Employee Increment";
		this.EmployeeTransfer.Name = "EmployeeTransfer";
		this.EmployeeTransfer.Size = new System.Drawing.Size(186, 22);
		this.EmployeeTransfer.Text = "Employee Transfer";
		this.FullandFinalSettlement.Name = "FullandFinalSettlement";
		this.FullandFinalSettlement.Size = new System.Drawing.Size(186, 22);
		this.FullandFinalSettlement.Text = "Full & Final Settlement";
		this.BlackListed.Name = "BlackListed";
		this.BlackListed.Size = new System.Drawing.Size(186, 22);
		this.BlackListed.Text = "Black Listed";
		this.storeManagmentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.PurchsaeOrderTrade, this.frmSupplySchedule, this.InwardGatePassTrade, this.InvFrmGRNTrade, this.purchaseInvoiceToolStripMenuItem });
		this.storeManagmentToolStripMenuItem.Name = "storeManagmentToolStripMenuItem";
		this.storeManagmentToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
		this.storeManagmentToolStripMenuItem.Text = "Purchase Trading";
		this.PurchsaeOrderTrade.Name = "PurchsaeOrderTrade";
		this.PurchsaeOrderTrade.Size = new System.Drawing.Size(226, 22);
		this.PurchsaeOrderTrade.Text = "Purchase Order Trading";
		this.frmSupplySchedule.Name = "frmSupplySchedule";
		this.frmSupplySchedule.Size = new System.Drawing.Size(226, 22);
		this.frmSupplySchedule.Text = "Supply Schedule";
		this.InwardGatePassTrade.Name = "InwardGatePassTrade";
		this.InwardGatePassTrade.Size = new System.Drawing.Size(226, 22);
		this.InwardGatePassTrade.Text = "GatePass Trading";
		this.InvFrmGRNTrade.Name = "InvFrmGRNTrade";
		this.InvFrmGRNTrade.Size = new System.Drawing.Size(226, 22);
		this.InvFrmGRNTrade.Text = "Goods Receipt Notes Trading";
		this.purchaseInvoiceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.PurchaseInvoiceTradingWithTax, this.PurchaseInvoiceTradePro });
		this.purchaseInvoiceToolStripMenuItem.Name = "purchaseInvoiceToolStripMenuItem";
		this.purchaseInvoiceToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
		this.purchaseInvoiceToolStripMenuItem.Text = "PurchaseInvoice";
		this.PurchaseInvoiceTradingWithTax.Name = "PurchaseInvoiceTradingWithTax";
		this.PurchaseInvoiceTradingWithTax.Size = new System.Drawing.Size(158, 22);
		this.PurchaseInvoiceTradingWithTax.Text = "With Tax";
		this.PurchaseInvoiceTradePro.Name = "PurchaseInvoiceTradePro";
		this.PurchaseInvoiceTradePro.Size = new System.Drawing.Size(158, 22);
		this.PurchaseInvoiceTradePro.Text = "InvoiceTradePro";
		this.btnSaleTrading.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.SaleOrderTrade, this.frmDeliverySchedule, this.FrmDeliveryChallan, this.saleInvoiceToolStripMenuItem });
		this.btnSaleTrading.Name = "btnSaleTrading";
		this.btnSaleTrading.Size = new System.Drawing.Size(207, 22);
		this.btnSaleTrading.Text = "Sale Trading";
		this.SaleOrderTrade.Name = "SaleOrderTrade";
		this.SaleOrderTrade.Size = new System.Drawing.Size(167, 22);
		this.SaleOrderTrade.Text = "Sale Order";
		this.frmDeliverySchedule.Name = "frmDeliverySchedule";
		this.frmDeliverySchedule.Size = new System.Drawing.Size(167, 22);
		this.frmDeliverySchedule.Text = "Delivery Schedule";
		this.FrmDeliveryChallan.Name = "FrmDeliveryChallan";
		this.FrmDeliveryChallan.Size = new System.Drawing.Size(167, 22);
		this.FrmDeliveryChallan.Text = "Delivery Challan";
		this.saleInvoiceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.SaleInvoiceTradingWithTax, this.InvfrmSaleInvoiceTrading });
		this.saleInvoiceToolStripMenuItem.Name = "saleInvoiceToolStripMenuItem";
		this.saleInvoiceToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
		this.saleInvoiceToolStripMenuItem.Text = "Sale Invoice";
		this.SaleInvoiceTradingWithTax.Name = "SaleInvoiceTradingWithTax";
		this.SaleInvoiceTradingWithTax.Size = new System.Drawing.Size(139, 22);
		this.SaleInvoiceTradingWithTax.Text = "With Tax";
		this.InvfrmSaleInvoiceTrading.Name = "InvfrmSaleInvoiceTrading";
		this.InvfrmSaleInvoiceTrading.Size = new System.Drawing.Size(139, 22);
		this.InvfrmSaleInvoiceTrading.Text = "WithOut Tax";
		this.fixedAssetsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.frmFixedAssetsCategory, this.frmDefineFixedAssets, this.frmFixedAssetRegister, this.frmFixedAssetRegisterHistory, this.frmFixedAssetsImportPurchaseOrder });
		this.fixedAssetsToolStripMenuItem.Name = "fixedAssetsToolStripMenuItem";
		this.fixedAssetsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
		this.fixedAssetsToolStripMenuItem.Text = "Fixed Assets";
		this.frmFixedAssetsCategory.Name = "frmFixedAssetsCategory";
		this.frmFixedAssetsCategory.Size = new System.Drawing.Size(189, 22);
		this.frmFixedAssetsCategory.Text = "Fixed Assets Catagory";
		this.frmDefineFixedAssets.Name = "frmDefineFixedAssets";
		this.frmDefineFixedAssets.Size = new System.Drawing.Size(189, 22);
		this.frmDefineFixedAssets.Text = "Define Fixed Assets";
		this.frmFixedAssetRegister.Name = "frmFixedAssetRegister";
		this.frmFixedAssetRegister.Size = new System.Drawing.Size(189, 22);
		this.frmFixedAssetRegister.Text = "Fixed Asset Register";
		this.frmFixedAssetRegisterHistory.Name = "frmFixedAssetRegisterHistory";
		this.frmFixedAssetRegisterHistory.Size = new System.Drawing.Size(189, 22);
		this.frmFixedAssetRegisterHistory.Text = "Fixed Assets History";
		this.frmFixedAssetsImportPurchaseOrder.Name = "frmFixedAssetsImportPurchaseOrder";
		this.frmFixedAssetsImportPurchaseOrder.Size = new System.Drawing.Size(189, 22);
		this.frmFixedAssetsImportPurchaseOrder.Text = "Import Contract";
		this.btnCargoManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.CargoBooking, this.FrmCBDHistory, this.ContainerAllocation, this.cbdBookingTracking, this.DestinationWareHouseReceipt, this.frmCargoDeliveryOrderAndBill, this.frmClearingBill });
		this.btnCargoManagement.Name = "btnCargoManagement";
		this.btnCargoManagement.ShowShortcutKeys = false;
		this.btnCargoManagement.Size = new System.Drawing.Size(207, 22);
		this.btnCargoManagement.Text = "Cargo Management";
		this.btnCargoManagement.Visible = false;
		this.CargoBooking.Name = "CargoBooking";
		this.CargoBooking.Size = new System.Drawing.Size(194, 22);
		this.CargoBooking.Text = "Cargo Booking";
		this.FrmCBDHistory.Name = "FrmCBDHistory";
		this.FrmCBDHistory.Size = new System.Drawing.Size(194, 22);
		this.FrmCBDHistory.Text = "Cargo Booking History";
		this.ContainerAllocation.Name = "ContainerAllocation";
		this.ContainerAllocation.Size = new System.Drawing.Size(194, 22);
		this.ContainerAllocation.Text = "Container Allocation";
		this.cbdBookingTracking.Name = "cbdBookingTracking";
		this.cbdBookingTracking.Size = new System.Drawing.Size(194, 22);
		this.cbdBookingTracking.Text = "Booking Tracking";
		this.DestinationWareHouseReceipt.Name = "DestinationWareHouseReceipt";
		this.DestinationWareHouseReceipt.Size = new System.Drawing.Size(194, 22);
		this.DestinationWareHouseReceipt.Text = "Warehouse Receipt";
		this.frmCargoDeliveryOrderAndBill.Name = "frmCargoDeliveryOrderAndBill";
		this.frmCargoDeliveryOrderAndBill.Size = new System.Drawing.Size(194, 22);
		this.frmCargoDeliveryOrderAndBill.Text = "Delivery Order & Bill";
		this.frmClearingBill.Name = "frmClearingBill";
		this.frmClearingBill.Size = new System.Drawing.Size(194, 22);
		this.frmClearingBill.Text = "Clearing Bill";
		this.btnFinanceManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.frmFinanceType, this.frmFMLoanRegistration, this.frmFMLoanAgreement });
		this.btnFinanceManagement.Name = "btnFinanceManagement";
		this.btnFinanceManagement.Size = new System.Drawing.Size(207, 22);
		this.btnFinanceManagement.Text = "Finance Management";
		this.frmFinanceType.Name = "frmFinanceType";
		this.frmFinanceType.Size = new System.Drawing.Size(206, 22);
		this.frmFinanceType.Text = "Finance Type";
		this.frmFMLoanRegistration.Name = "frmFMLoanRegistration";
		this.frmFMLoanRegistration.Size = new System.Drawing.Size(206, 22);
		this.frmFMLoanRegistration.Text = "Loan Facility Registration";
		this.frmFMLoanAgreement.Name = "frmFMLoanAgreement";
		this.frmFMLoanAgreement.Size = new System.Drawing.Size(206, 22);
		this.frmFMLoanAgreement.Text = "Laon Facility Agreement";
		this.btnStoreManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[22]
		{
			this.ItemCategoryStore, this.ItemTypeStore, this.AddItemStore, this.frmStoreOpeningStockBalancing, this.frmStockAdjustment, this.toolStripSeparator2, this.PurchaseOrderStore, this.frmPurchaseOrderWithTax, this.frmPurchaseDemand, this.GatePassGeneral,
			this.GrnStore, this.PurchaseInvoiceStoreManagement, this.toolStripSeparator3, this.DeliveryOrderPackingMaterial, this.GatePassGeneralOutward, this.frmDepartmentRequest, this.frmGSIssuance, this.StoreIssuanceDirect, this.frmStockTransfer, this.frmPackingMaterialTransferPartyToParty,
			this.FrmSaleInvoiceDirectPackingMaterial, this.StoreReturn
		});
		this.btnStoreManagement.Name = "btnStoreManagement";
		this.btnStoreManagement.Size = new System.Drawing.Size(207, 22);
		this.btnStoreManagement.Text = "Store Management";
		this.ItemCategoryStore.Name = "ItemCategoryStore";
		this.ItemCategoryStore.Size = new System.Drawing.Size(240, 22);
		this.ItemCategoryStore.Text = "Item Category";
		this.ItemTypeStore.Name = "ItemTypeStore";
		this.ItemTypeStore.Size = new System.Drawing.Size(240, 22);
		this.ItemTypeStore.Text = "Item Type";
		this.AddItemStore.Name = "AddItemStore";
		this.AddItemStore.Size = new System.Drawing.Size(240, 22);
		this.AddItemStore.Text = "Item Define";
		this.frmStoreOpeningStockBalancing.Name = "frmStoreOpeningStockBalancing";
		this.frmStoreOpeningStockBalancing.Size = new System.Drawing.Size(240, 22);
		this.frmStoreOpeningStockBalancing.Text = "Stock Opening";
		this.frmStockAdjustment.Name = "frmStockAdjustment";
		this.frmStockAdjustment.Size = new System.Drawing.Size(240, 22);
		this.frmStockAdjustment.Text = "Stock Adjustment";
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(237, 6);
		this.PurchaseOrderStore.Name = "PurchaseOrderStore";
		this.PurchaseOrderStore.Size = new System.Drawing.Size(240, 22);
		this.PurchaseOrderStore.Text = "Purchase Order";
		this.frmPurchaseOrderWithTax.Name = "frmPurchaseOrderWithTax";
		this.frmPurchaseOrderWithTax.Size = new System.Drawing.Size(240, 22);
		this.frmPurchaseOrderWithTax.Text = "Purchase Order With Tax";
		this.frmPurchaseDemand.Name = "frmPurchaseDemand";
		this.frmPurchaseDemand.Size = new System.Drawing.Size(240, 22);
		this.frmPurchaseDemand.Text = "Purchase Demand";
		this.GatePassGeneral.Name = "GatePassGeneral";
		this.GatePassGeneral.Size = new System.Drawing.Size(240, 22);
		this.GatePassGeneral.Text = "General GatePass Inward";
		this.GrnStore.Name = "GrnStore";
		this.GrnStore.Size = new System.Drawing.Size(240, 22);
		this.GrnStore.Text = "Goods Receiving Notes";
		this.PurchaseInvoiceStoreManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.PurchaseInvoiceStoreWithOutTax, this.PurchaseInvoiceStoreWithTax, this.frmPurchaseInvoiceDirectStore, this.PurchaseInvoiceDirectStoreWithTax });
		this.PurchaseInvoiceStoreManagement.Name = "PurchaseInvoiceStoreManagement";
		this.PurchaseInvoiceStoreManagement.Size = new System.Drawing.Size(240, 22);
		this.PurchaseInvoiceStoreManagement.Text = "Purchase Invoice";
		this.PurchaseInvoiceStoreWithOutTax.Name = "PurchaseInvoiceStoreWithOutTax";
		this.PurchaseInvoiceStoreWithOutTax.Size = new System.Drawing.Size(245, 22);
		this.PurchaseInvoiceStoreWithOutTax.Text = "Purchase Invoice";
		this.PurchaseInvoiceStoreWithTax.Name = "PurchaseInvoiceStoreWithTax";
		this.PurchaseInvoiceStoreWithTax.Size = new System.Drawing.Size(245, 22);
		this.PurchaseInvoiceStoreWithTax.Text = "Purchase Invoice With Tax";
		this.frmPurchaseInvoiceDirectStore.Name = "frmPurchaseInvoiceDirectStore";
		this.frmPurchaseInvoiceDirectStore.Size = new System.Drawing.Size(245, 22);
		this.frmPurchaseInvoiceDirectStore.Text = "Purchase Invoice Direct";
		this.PurchaseInvoiceDirectStoreWithTax.Name = "PurchaseInvoiceDirectStoreWithTax";
		this.PurchaseInvoiceDirectStoreWithTax.Size = new System.Drawing.Size(245, 22);
		this.PurchaseInvoiceDirectStoreWithTax.Text = "Purchase Invoice Direct With Tax";
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(237, 6);
		this.DeliveryOrderPackingMaterial.Name = "DeliveryOrderPackingMaterial";
		this.DeliveryOrderPackingMaterial.Size = new System.Drawing.Size(240, 22);
		this.DeliveryOrderPackingMaterial.Text = "Delivery Order Packing Material";
		this.GatePassGeneralOutward.Name = "GatePassGeneralOutward";
		this.GatePassGeneralOutward.Size = new System.Drawing.Size(240, 22);
		this.GatePassGeneralOutward.Text = "General GatePassOutward";
		this.frmDepartmentRequest.Name = "frmDepartmentRequest";
		this.frmDepartmentRequest.Size = new System.Drawing.Size(240, 22);
		this.frmDepartmentRequest.Text = "Department Request";
		this.frmGSIssuance.Name = "frmGSIssuance";
		this.frmGSIssuance.Size = new System.Drawing.Size(240, 22);
		this.frmGSIssuance.Text = "Store Issuance";
		this.StoreIssuanceDirect.Name = "StoreIssuanceDirect";
		this.StoreIssuanceDirect.Size = new System.Drawing.Size(240, 22);
		this.StoreIssuanceDirect.Text = "Store Issuance Direct";
		this.frmStockTransfer.Name = "frmStockTransfer";
		this.frmStockTransfer.Size = new System.Drawing.Size(240, 22);
		this.frmStockTransfer.Text = "Stock Transfer";
		this.frmPackingMaterialTransferPartyToParty.Name = "frmPackingMaterialTransferPartyToParty";
		this.frmPackingMaterialTransferPartyToParty.Size = new System.Drawing.Size(240, 22);
		this.frmPackingMaterialTransferPartyToParty.Text = "Packing Material Transfer";
		this.FrmSaleInvoiceDirectPackingMaterial.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.frmSaleInvoiceDirectWithOutTax, this.toolStripSeparator8, this.frmSaleInvoiceDirectWithTax });
		this.FrmSaleInvoiceDirectPackingMaterial.Name = "FrmSaleInvoiceDirectPackingMaterial";
		this.FrmSaleInvoiceDirectPackingMaterial.Size = new System.Drawing.Size(240, 22);
		this.FrmSaleInvoiceDirectPackingMaterial.Text = "Sale Invoice Direct";
		this.frmSaleInvoiceDirectWithOutTax.Name = "frmSaleInvoiceDirectWithOutTax";
		this.frmSaleInvoiceDirectWithOutTax.Size = new System.Drawing.Size(218, 22);
		this.frmSaleInvoiceDirectWithOutTax.Text = "Sale Invoice Direct";
		this.toolStripSeparator8.Name = "toolStripSeparator8";
		this.toolStripSeparator8.Size = new System.Drawing.Size(215, 6);
		this.frmSaleInvoiceDirectWithTax.Name = "frmSaleInvoiceDirectWithTax";
		this.frmSaleInvoiceDirectWithTax.Size = new System.Drawing.Size(218, 22);
		this.frmSaleInvoiceDirectWithTax.Text = "Sale Invoice Direct With Tax";
		this.StoreReturn.Name = "StoreReturn";
		this.StoreReturn.Size = new System.Drawing.Size(240, 22);
		this.StoreReturn.Text = "Return to Store";
		this.PartyProcessing.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[16]
		{
			this.frmPartyProcessingDefineSupplier, this.DefineReferenceParties, this.DefItemCatagoryPartyProcessing, this.DefPartyProcessingItemType, this.DefPartProcessingAddItem, this.StockOpeningBalancePartyProcessing, this.GatePassInwardPartyProcessing, this.GoodsReceivingNotesPartyProcessing, this.GRNStorePartyProcessing, this.GatePassOutwardParyProcessing,
			this.GoodsDispatchNotesPartyProcessing, this.GDNStorePartyProcessing, this.ProductionJobOrderPartyProcessing, this.frmProductionPartyProcessing, this.ProductionPartyProcessingBill, this.btnPartyProcessingReports
		});
		this.PartyProcessing.Name = "PartyProcessing";
		this.PartyProcessing.Size = new System.Drawing.Size(207, 22);
		this.PartyProcessing.Text = "Party Processing";
		this.frmPartyProcessingDefineSupplier.Name = "frmPartyProcessingDefineSupplier";
		this.frmPartyProcessingDefineSupplier.Size = new System.Drawing.Size(226, 22);
		this.frmPartyProcessingDefineSupplier.Text = "Define Stock Parties";
		this.DefineReferenceParties.Name = "DefineReferenceParties";
		this.DefineReferenceParties.Size = new System.Drawing.Size(226, 22);
		this.DefineReferenceParties.Text = "Define Reference Parties";
		this.DefItemCatagoryPartyProcessing.Name = "DefItemCatagoryPartyProcessing";
		this.DefItemCatagoryPartyProcessing.Size = new System.Drawing.Size(226, 22);
		this.DefItemCatagoryPartyProcessing.Text = "Define Item Category";
		this.DefPartyProcessingItemType.Name = "DefPartyProcessingItemType";
		this.DefPartyProcessingItemType.Size = new System.Drawing.Size(226, 22);
		this.DefPartyProcessingItemType.Text = "Define Item Type";
		this.DefPartProcessingAddItem.Name = "DefPartProcessingAddItem";
		this.DefPartProcessingAddItem.Size = new System.Drawing.Size(226, 22);
		this.DefPartProcessingAddItem.Text = "Define Item";
		this.StockOpeningBalancePartyProcessing.Name = "StockOpeningBalancePartyProcessing";
		this.StockOpeningBalancePartyProcessing.Size = new System.Drawing.Size(226, 22);
		this.StockOpeningBalancePartyProcessing.Text = "Stock Opening";
		this.GatePassInwardPartyProcessing.Name = "GatePassInwardPartyProcessing";
		this.GatePassInwardPartyProcessing.Size = new System.Drawing.Size(226, 22);
		this.GatePassInwardPartyProcessing.Text = "GatePass Inward";
		this.GoodsReceivingNotesPartyProcessing.Name = "GoodsReceivingNotesPartyProcessing";
		this.GoodsReceivingNotesPartyProcessing.Size = new System.Drawing.Size(226, 22);
		this.GoodsReceivingNotesPartyProcessing.Text = "Goods Receiving Notes";
		this.GRNStorePartyProcessing.Name = "GRNStorePartyProcessing";
		this.GRNStorePartyProcessing.Size = new System.Drawing.Size(226, 22);
		this.GRNStorePartyProcessing.Text = "Goods Receiving Notes Store";
		this.GatePassOutwardParyProcessing.Name = "GatePassOutwardParyProcessing";
		this.GatePassOutwardParyProcessing.Size = new System.Drawing.Size(226, 22);
		this.GatePassOutwardParyProcessing.Text = "GatePass Outward";
		this.GoodsDispatchNotesPartyProcessing.Name = "GoodsDispatchNotesPartyProcessing";
		this.GoodsDispatchNotesPartyProcessing.Size = new System.Drawing.Size(226, 22);
		this.GoodsDispatchNotesPartyProcessing.Text = "Goods Dispatch Notes";
		this.GDNStorePartyProcessing.Name = "GDNStorePartyProcessing";
		this.GDNStorePartyProcessing.Size = new System.Drawing.Size(226, 22);
		this.GDNStorePartyProcessing.Text = "Goods Dispatch Notes Store";
		this.ProductionJobOrderPartyProcessing.Name = "ProductionJobOrderPartyProcessing";
		this.ProductionJobOrderPartyProcessing.Size = new System.Drawing.Size(226, 22);
		this.ProductionJobOrderPartyProcessing.Text = "Production Job Order";
		this.frmProductionPartyProcessing.Name = "frmProductionPartyProcessing";
		this.frmProductionPartyProcessing.Size = new System.Drawing.Size(226, 22);
		this.frmProductionPartyProcessing.Text = "Production";
		this.ProductionPartyProcessingBill.Name = "ProductionPartyProcessingBill";
		this.ProductionPartyProcessingBill.Size = new System.Drawing.Size(226, 22);
		this.ProductionPartyProcessingBill.Text = "Party Processing Bill";
		this.btnPartyProcessingReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.frmGatePassPartyProcessing, this.frmGRNPartyProcessingRegister, this.grmGDNPartyProcessingRegister, this.frmStockReportPartyProcessing, this.frmPartyProcessingGrnInfo });
		this.btnPartyProcessingReports.Name = "btnPartyProcessingReports";
		this.btnPartyProcessingReports.Size = new System.Drawing.Size(226, 22);
		this.btnPartyProcessingReports.Text = "Reports";
		this.frmGatePassPartyProcessing.Name = "frmGatePassPartyProcessing";
		this.frmGatePassPartyProcessing.Size = new System.Drawing.Size(199, 22);
		this.frmGatePassPartyProcessing.Text = "GatePass Register";
		this.frmGRNPartyProcessingRegister.Name = "frmGRNPartyProcessingRegister";
		this.frmGRNPartyProcessingRegister.Size = new System.Drawing.Size(199, 22);
		this.frmGRNPartyProcessingRegister.Text = "Grn Register";
		this.grmGDNPartyProcessingRegister.Name = "grmGDNPartyProcessingRegister";
		this.grmGDNPartyProcessingRegister.Size = new System.Drawing.Size(199, 22);
		this.grmGDNPartyProcessingRegister.Text = "Gdn Register";
		this.frmStockReportPartyProcessing.Name = "frmStockReportPartyProcessing";
		this.frmStockReportPartyProcessing.Size = new System.Drawing.Size(199, 22);
		this.frmStockReportPartyProcessing.Text = "Stock Report";
		this.frmPartyProcessingGrnInfo.Name = "frmPartyProcessingGrnInfo";
		this.frmPartyProcessingGrnInfo.Size = new System.Drawing.Size(199, 22);
		this.frmPartyProcessingGrnInfo.Text = "Grn Info For Production";
		this.petroliumToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.PurchaseInvoicePetrol, this.SaleInvoicePetrol, this.PurchaseInvoiceRegister, this.SaleInvoiceRegister, this.StockReport });
		this.petroliumToolStripMenuItem.Name = "petroliumToolStripMenuItem";
		this.petroliumToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
		this.petroliumToolStripMenuItem.Text = "Petrolium";
		this.petroliumToolStripMenuItem.Visible = false;
		this.PurchaseInvoicePetrol.Name = "PurchaseInvoicePetrol";
		this.PurchaseInvoicePetrol.Size = new System.Drawing.Size(202, 22);
		this.PurchaseInvoicePetrol.Text = "PurchaseInvoice";
		this.SaleInvoicePetrol.Name = "SaleInvoicePetrol";
		this.SaleInvoicePetrol.Size = new System.Drawing.Size(202, 22);
		this.SaleInvoicePetrol.Text = "SaleInvoice";
		this.PurchaseInvoiceRegister.Name = "PurchaseInvoiceRegister";
		this.PurchaseInvoiceRegister.Size = new System.Drawing.Size(202, 22);
		this.PurchaseInvoiceRegister.Text = "PurchaseInvoiceRegister";
		this.SaleInvoiceRegister.Name = "SaleInvoiceRegister";
		this.SaleInvoiceRegister.Size = new System.Drawing.Size(202, 22);
		this.SaleInvoiceRegister.Text = "Sale Invoice Register";
		this.StockReport.Name = "StockReport";
		this.StockReport.Size = new System.Drawing.Size(202, 22);
		this.StockReport.Text = "Stock Report";
		this.btnServices.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btndefineservices, this.frmSrvCustPricingAgreement, this.frmSrvSaleOrderForwarder, this.frmServClearingAgentBill, this.ExImClearingAgentBillDirect, this.frmPreShipmentCosting, this.btnServicesReport });
		this.btnServices.Name = "btnServices";
		this.btnServices.Size = new System.Drawing.Size(207, 22);
		this.btnServices.Text = "Services";
		this.btndefineservices.Name = "btndefineservices";
		this.btndefineservices.Size = new System.Drawing.Size(186, 22);
		this.btndefineservices.Text = "Define Services";
		this.frmSrvCustPricingAgreement.Name = "frmSrvCustPricingAgreement";
		this.frmSrvCustPricingAgreement.Size = new System.Drawing.Size(186, 22);
		this.frmSrvCustPricingAgreement.Text = "Supplier ItemPricing";
		this.frmSrvSaleOrderForwarder.Name = "frmSrvSaleOrderForwarder";
		this.frmSrvSaleOrderForwarder.Size = new System.Drawing.Size(186, 22);
		this.frmSrvSaleOrderForwarder.Text = "SaleOrder Forwarder";
		this.frmServClearingAgentBill.Name = "frmServClearingAgentBill";
		this.frmServClearingAgentBill.Size = new System.Drawing.Size(186, 22);
		this.frmServClearingAgentBill.Text = "Clearing Agent Bill";
		this.ExImClearingAgentBillDirect.Name = "ExImClearingAgentBillDirect";
		this.ExImClearingAgentBillDirect.Size = new System.Drawing.Size(186, 22);
		this.ExImClearingAgentBillDirect.Text = "Services Bill - Direct";
		this.frmPreShipmentCosting.Name = "frmPreShipmentCosting";
		this.frmPreShipmentCosting.Size = new System.Drawing.Size(186, 22);
		this.frmPreShipmentCosting.Text = "PreShipment Costing";
		this.btnServicesReport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.ServicesBillRegister });
		this.btnServicesReport.Name = "btnServicesReport";
		this.btnServicesReport.Size = new System.Drawing.Size(186, 22);
		this.btnServicesReport.Text = "Reports";
		this.ServicesBillRegister.Name = "ServicesBillRegister";
		this.ServicesBillRegister.Size = new System.Drawing.Size(180, 22);
		this.ServicesBillRegister.Text = "Services Bill Register";
		this.btnTaxation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.InvfrmAddTaxType, this.InvfrmAddTaxSchedule, this.InvDeffrmItemTaxSchdule, this.frmTaxNotesAndGLMaping, this.frmSupplierCustomerExemptionSchedule, this.SaleTaxSummary });
		this.btnTaxation.Name = "btnTaxation";
		this.btnTaxation.Size = new System.Drawing.Size(207, 22);
		this.btnTaxation.Text = "Taxation";
		this.InvfrmAddTaxType.Name = "InvfrmAddTaxType";
		this.InvfrmAddTaxType.Size = new System.Drawing.Size(199, 22);
		this.InvfrmAddTaxType.Text = "Tax Types";
		this.InvfrmAddTaxSchedule.Name = "InvfrmAddTaxSchedule";
		this.InvfrmAddTaxSchedule.Size = new System.Drawing.Size(199, 22);
		this.InvfrmAddTaxSchedule.Text = "Tax Schedule";
		this.InvDeffrmItemTaxSchdule.Name = "InvDeffrmItemTaxSchdule";
		this.InvDeffrmItemTaxSchdule.Size = new System.Drawing.Size(199, 22);
		this.InvDeffrmItemTaxSchdule.Text = "Item Tax Schedule";
		this.frmTaxNotesAndGLMaping.Name = "frmTaxNotesAndGLMaping";
		this.frmTaxNotesAndGLMaping.Size = new System.Drawing.Size(199, 22);
		this.frmTaxNotesAndGLMaping.Text = "Tax Notes GL Maping";
		this.frmSupplierCustomerExemptionSchedule.Name = "frmSupplierCustomerExemptionSchedule";
		this.frmSupplierCustomerExemptionSchedule.Size = new System.Drawing.Size(199, 22);
		this.frmSupplierCustomerExemptionSchedule.Text = "Tax ExemptionSchedule";
		this.SaleTaxSummary.Name = "SaleTaxSummary";
		this.SaleTaxSummary.Size = new System.Drawing.Size(199, 22);
		this.SaleTaxSummary.Text = "Sales Tax Summery";
		this.btnImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.frmImpPerformaInvoice, this.frmImpLcContract, this.frmImpLcPurchaseContract, this.frmImpPurchaseOrder, this.frmImportOrderPackingDetail, this.btnBillofLadingImport, this.frmImpBL, this.frmImpGrnForwarding, this.frmImpGrnDirect, this.frmImpInvoice,
			this.frmImportCommercialInvoicebyContract
		});
		this.btnImport.Name = "btnImport";
		this.btnImport.Size = new System.Drawing.Size(207, 22);
		this.btnImport.Text = "Import";
		this.frmImpPerformaInvoice.Name = "frmImpPerformaInvoice";
		this.frmImpPerformaInvoice.Size = new System.Drawing.Size(321, 22);
		this.frmImpPerformaInvoice.Text = "Performa Invoice";
		this.frmImpLcContract.Name = "frmImpLcContract";
		this.frmImpLcContract.Size = new System.Drawing.Size(321, 22);
		this.frmImpLcContract.Text = "Lc Order";
		this.frmImpLcPurchaseContract.Name = "frmImpLcPurchaseContract";
		this.frmImpLcPurchaseContract.Size = new System.Drawing.Size(321, 22);
		this.frmImpLcPurchaseContract.Text = "Import Contract";
		this.frmImpPurchaseOrder.Name = "frmImpPurchaseOrder";
		this.frmImpPurchaseOrder.Size = new System.Drawing.Size(321, 22);
		this.frmImpPurchaseOrder.Text = "Import Purchase Order";
		this.frmImportOrderPackingDetail.Name = "frmImportOrderPackingDetail";
		this.frmImportOrderPackingDetail.Size = new System.Drawing.Size(321, 22);
		this.frmImportOrderPackingDetail.Text = "Import Purchase Order Packing List";
		this.btnBillofLadingImport.Name = "btnBillofLadingImport";
		this.btnBillofLadingImport.Size = new System.Drawing.Size(318, 6);
		this.frmImpBL.Name = "frmImpBL";
		this.frmImpBL.Size = new System.Drawing.Size(321, 22);
		this.frmImpBL.Text = "Bill of Lading";
		this.frmImpGrnForwarding.Name = "frmImpGrnForwarding";
		this.frmImpGrnForwarding.Size = new System.Drawing.Size(321, 22);
		this.frmImpGrnForwarding.Text = "Goods Receiving Notes";
		this.frmImpGrnDirect.Name = "frmImpGrnDirect";
		this.frmImpGrnDirect.Size = new System.Drawing.Size(321, 22);
		this.frmImpGrnDirect.Text = "Goods Receiving Notes (Without WeighBridge)";
		this.frmImpInvoice.Name = "frmImpInvoice";
		this.frmImpInvoice.Size = new System.Drawing.Size(321, 22);
		this.frmImpInvoice.Text = "Commercial Invoice";
		this.frmImportCommercialInvoicebyContract.Name = "frmImportCommercialInvoicebyContract";
		this.frmImportCommercialInvoicebyContract.Size = new System.Drawing.Size(321, 22);
		this.frmImportCommercialInvoicebyContract.Text = "Commercial Invoice by Contract";
		this.CommissionAgentTrade.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.frmCommissionAgentOrder, this.frmCommissionAgentTradeBill, this.CommissionAgentTradeBill_162 });
		this.CommissionAgentTrade.Name = "CommissionAgentTrade";
		this.CommissionAgentTrade.Size = new System.Drawing.Size(207, 22);
		this.CommissionAgentTrade.Text = "Commission Agent Trade";
		this.frmCommissionAgentOrder.Name = "frmCommissionAgentOrder";
		this.frmCommissionAgentOrder.Size = new System.Drawing.Size(218, 22);
		this.frmCommissionAgentOrder.Text = "Commission Agent Order";
		this.frmCommissionAgentTradeBill.Name = "frmCommissionAgentTradeBill";
		this.frmCommissionAgentTradeBill.Size = new System.Drawing.Size(218, 22);
		this.frmCommissionAgentTradeBill.Text = "Commission Agent Bill";
		this.CommissionAgentTradeBill_162.Name = "CommissionAgentTradeBill_162";
		this.CommissionAgentTradeBill_162.Size = new System.Drawing.Size(218, 22);
		this.CommissionAgentTradeBill_162.Text = "Commission Agent Bill_162";
		this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnDashBoard, this.btnAcReport, this.pdcReprotsToolStripMenuItem, this.InventoryReport, this.btnExportHistory, this.btnLabReports, this.btnCargoReports, this.btnStockReport, this.btnEmptyBagsReports, this.productionReportsToolStripMenuItem,
			this.btnHRMReports, this.btnImportReports
		});
		this.contextMenuStrip2.Name = "ContextMenuStrip1";
		this.contextMenuStrip2.Size = new System.Drawing.Size(177, 268);
		this.btnDashBoard.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.AcFrmDashboard, this.PendingVouchersForApproval, this.frmPendingWorksRpt, this.btnStocksDashBoard, this.UnApprovedVouchersDashBoard, this.SalesAnalytics, this.btnAnalyticDashboard, this.OrderManagementDashboard });
		this.btnDashBoard.Name = "btnDashBoard";
		this.btnDashBoard.Size = new System.Drawing.Size(176, 22);
		this.btnDashBoard.Text = "DashBoard";
		this.AcFrmDashboard.Name = "AcFrmDashboard";
		this.AcFrmDashboard.Size = new System.Drawing.Size(238, 22);
		this.AcFrmDashboard.Text = "Accounts DashBoard";
		this.PendingVouchersForApproval.Name = "PendingVouchersForApproval";
		this.PendingVouchersForApproval.Size = new System.Drawing.Size(238, 22);
		this.PendingVouchersForApproval.Text = "Approval DashBoard";
		this.frmPendingWorksRpt.Name = "frmPendingWorksRpt";
		this.frmPendingWorksRpt.Size = new System.Drawing.Size(238, 22);
		this.frmPendingWorksRpt.Text = "Pending Works Dashboard";
		this.btnStocksDashBoard.Name = "btnStocksDashBoard";
		this.btnStocksDashBoard.Size = new System.Drawing.Size(238, 22);
		this.btnStocksDashBoard.Text = "Stocks DashBoard";
		this.UnApprovedVouchersDashBoard.Name = "UnApprovedVouchersDashBoard";
		this.UnApprovedVouchersDashBoard.Size = new System.Drawing.Size(238, 22);
		this.UnApprovedVouchersDashBoard.Text = "UnApproved DashBoard";
		this.SalesAnalytics.Name = "SalesAnalytics";
		this.SalesAnalytics.Size = new System.Drawing.Size(238, 22);
		this.SalesAnalytics.Text = "Sales Analytics";
		this.btnAnalyticDashboard.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.frmPaddyPurchaseAnalytics, this.frmRicePurchaseAnalytics, this.frmSalesAnalytics });
		this.btnAnalyticDashboard.Name = "btnAnalyticDashboard";
		this.btnAnalyticDashboard.Size = new System.Drawing.Size(238, 22);
		this.btnAnalyticDashboard.Text = "Analytic Dashbaord";
		this.frmPaddyPurchaseAnalytics.Name = "frmPaddyPurchaseAnalytics";
		this.frmPaddyPurchaseAnalytics.Size = new System.Drawing.Size(67, 22);
		this.frmRicePurchaseAnalytics.Name = "frmRicePurchaseAnalytics";
		this.frmRicePurchaseAnalytics.Size = new System.Drawing.Size(67, 22);
		this.frmSalesAnalytics.Name = "frmSalesAnalytics";
		this.frmSalesAnalytics.Size = new System.Drawing.Size(67, 22);
		this.OrderManagementDashboard.Name = "OrderManagementDashboard";
		this.OrderManagementDashboard.Size = new System.Drawing.Size(238, 22);
		this.OrderManagementDashboard.Text = "Order Management Dashboard";
		this.btnAcReport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[29]
		{
			this.ChartOfAccount, this.VoucherReport, this.VoucherValidation, this.CostomerWiseVoucherSlip, this.ActicitySummery, this.BankBalances, this.CashBalances, this.GeneralLedger, this.FcyGeneralLedgerRpt, this.CustomerLedger,
			this.GeneralLedgerStatment, this.Payables, this.PayablesAging, this.PayablesByDueDate, this.Receivables, this.ReceivableAging, this.TradeDebitorsReport, this.TradeCreditorsReport, this.btnPaymentByInvoiceSummery_Detail, this.InventoryPayablesandReceivables,
			this.FCYPayablesAndReceivablesRpt, this.TrialBalance, this.SelectedTrialBalanceAllLevel, this.SelectedTrialBalance, this.PayablesandReceivablesAging, this.GeneralJournalSummeryRegister, this.AuditDashboard, this.frmProfitAndLoss, this.frmBalanceSheet
		});
		this.btnAcReport.Name = "btnAcReport";
		this.btnAcReport.Size = new System.Drawing.Size(176, 22);
		this.btnAcReport.Text = "Accounts Reports";
		this.ChartOfAccount.Name = "ChartOfAccount";
		this.ChartOfAccount.Size = new System.Drawing.Size(287, 22);
		this.ChartOfAccount.Text = "Chart Of Account";
		this.VoucherReport.Name = "VoucherReport";
		this.VoucherReport.Size = new System.Drawing.Size(287, 22);
		this.VoucherReport.Text = "Voucher Report";
		this.VoucherValidation.Name = "VoucherValidation";
		this.VoucherValidation.Size = new System.Drawing.Size(287, 22);
		this.VoucherValidation.Text = "Voucher Validation";
		this.CostomerWiseVoucherSlip.Name = "CostomerWiseVoucherSlip";
		this.CostomerWiseVoucherSlip.Size = new System.Drawing.Size(287, 22);
		this.CostomerWiseVoucherSlip.Text = "Customer Wise Voucher Slips";
		this.ActicitySummery.Name = "ActicitySummery";
		this.ActicitySummery.Size = new System.Drawing.Size(287, 22);
		this.ActicitySummery.Text = "Activity Summary";
		this.BankBalances.Name = "BankBalances";
		this.BankBalances.Size = new System.Drawing.Size(287, 22);
		this.BankBalances.Text = "Bank Balances Report";
		this.CashBalances.Name = "CashBalances";
		this.CashBalances.Size = new System.Drawing.Size(287, 22);
		this.CashBalances.Text = "Cash Balances Report";
		this.GeneralLedger.Name = "GeneralLedger";
		this.GeneralLedger.Size = new System.Drawing.Size(287, 22);
		this.GeneralLedger.Text = "General Ledger";
		this.FcyGeneralLedgerRpt.Name = "FcyGeneralLedgerRpt";
		this.FcyGeneralLedgerRpt.Size = new System.Drawing.Size(287, 22);
		this.FcyGeneralLedgerRpt.Text = "Fcy General Ledger";
		this.CustomerLedger.Name = "CustomerLedger";
		this.CustomerLedger.Size = new System.Drawing.Size(287, 22);
		this.CustomerLedger.Text = "Supplier/Customer  Ledger";
		this.GeneralLedgerStatment.Name = "GeneralLedgerStatment";
		this.GeneralLedgerStatment.Size = new System.Drawing.Size(287, 22);
		this.GeneralLedgerStatment.Text = "Account Statement";
		this.GeneralLedgerStatment.Visible = false;
		this.Payables.Name = "Payables";
		this.Payables.Size = new System.Drawing.Size(287, 22);
		this.Payables.Text = "Payables";
		this.PayablesAging.Name = "PayablesAging";
		this.PayablesAging.Size = new System.Drawing.Size(287, 22);
		this.PayablesAging.Text = "Payables Aging";
		this.PayablesByDueDate.Name = "PayablesByDueDate";
		this.PayablesByDueDate.Size = new System.Drawing.Size(287, 22);
		this.PayablesByDueDate.Text = "Payables By Due Date";
		this.Receivables.Name = "Receivables";
		this.Receivables.Size = new System.Drawing.Size(287, 22);
		this.Receivables.Text = "Receivables";
		this.ReceivableAging.Name = "ReceivableAging";
		this.ReceivableAging.Size = new System.Drawing.Size(287, 22);
		this.ReceivableAging.Text = "Receivable Aging";
		this.TradeDebitorsReport.Name = "TradeDebitorsReport";
		this.TradeDebitorsReport.Size = new System.Drawing.Size(287, 22);
		this.TradeDebitorsReport.Text = "Receivables Accounts Classification wise";
		this.TradeCreditorsReport.Name = "TradeCreditorsReport";
		this.TradeCreditorsReport.Size = new System.Drawing.Size(287, 22);
		this.TradeCreditorsReport.Text = "Payables Accounts Classification wise";
		this.btnPaymentByInvoiceSummery_Detail.Name = "btnPaymentByInvoiceSummery_Detail";
		this.btnPaymentByInvoiceSummery_Detail.Size = new System.Drawing.Size(287, 22);
		this.btnPaymentByInvoiceSummery_Detail.Text = "Payables and Receivables Aging";
		this.InventoryPayablesandReceivables.Name = "InventoryPayablesandReceivables";
		this.InventoryPayablesandReceivables.Size = new System.Drawing.Size(287, 22);
		this.InventoryPayablesandReceivables.Text = "Inventory Payables & Receivables";
		this.FCYPayablesAndReceivablesRpt.Name = "FCYPayablesAndReceivablesRpt";
		this.FCYPayablesAndReceivablesRpt.Size = new System.Drawing.Size(287, 22);
		this.FCYPayablesAndReceivablesRpt.Text = "Export Receivables & Payables";
		this.TrialBalance.Name = "TrialBalance";
		this.TrialBalance.Size = new System.Drawing.Size(287, 22);
		this.TrialBalance.Text = "Trial Balance";
		this.SelectedTrialBalanceAllLevel.Name = "SelectedTrialBalanceAllLevel";
		this.SelectedTrialBalanceAllLevel.Size = new System.Drawing.Size(287, 22);
		this.SelectedTrialBalanceAllLevel.Text = "Trial Balance Upto 5th Level";
		this.SelectedTrialBalance.Name = "SelectedTrialBalance";
		this.SelectedTrialBalance.Size = new System.Drawing.Size(287, 22);
		this.SelectedTrialBalance.Text = "Selected Trial Balance";
		this.SelectedTrialBalance.Visible = false;
		this.PayablesandReceivablesAging.Name = "PayablesandReceivablesAging";
		this.PayablesandReceivablesAging.Size = new System.Drawing.Size(287, 22);
		this.PayablesandReceivablesAging.Text = "Payables and Receivables Aging";
		this.GeneralJournalSummeryRegister.Name = "GeneralJournalSummeryRegister";
		this.GeneralJournalSummeryRegister.Size = new System.Drawing.Size(287, 22);
		this.GeneralJournalSummeryRegister.Text = "General Journal Summery";
		this.GeneralJournalSummeryRegister.Visible = false;
		this.AuditDashboard.Name = "AuditDashboard";
		this.AuditDashboard.Size = new System.Drawing.Size(287, 22);
		this.AuditDashboard.Text = "UnBalanceVoucher";
		this.frmProfitAndLoss.Name = "frmProfitAndLoss";
		this.frmProfitAndLoss.Size = new System.Drawing.Size(287, 22);
		this.frmProfitAndLoss.Text = "Profit & Loss";
		this.frmBalanceSheet.Name = "frmBalanceSheet";
		this.frmBalanceSheet.Size = new System.Drawing.Size(287, 22);
		this.frmBalanceSheet.Text = "Balance Sheet";
		this.pdcReprotsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.PdcInventoryReport });
		this.pdcReprotsToolStripMenuItem.Name = "pdcReprotsToolStripMenuItem";
		this.pdcReprotsToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
		this.pdcReprotsToolStripMenuItem.Text = "Pdc Reprots";
		this.PdcInventoryReport.Name = "PdcInventoryReport";
		this.PdcInventoryReport.Size = new System.Drawing.Size(141, 22);
		this.PdcInventoryReport.Text = "Pdc Pending";
		this.InventoryReport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[28]
		{
			this.PurchaseOrderHistory, this.frmGatePassGeneral, this.frmGatePassReport, this.frmGRNHistory, this.frmPurchaseInvoiceHistory, this.PurchaseRegisterNew, this.frmSaleOrderHistory, this.frmDeliveryOrderForApprovel, this.frmGPOutward, this.frmGDNHistory,
			this.SaleInvoiceRegisterNew, this.frmSaleInvoiceHistory, this.frmSaleInvoiceDirectRegister, this.frmWeightBridgeHistory, this.frmWeighBridgeRejectedTicketNos, this.frmEvaulationDetailWagesReports, this.frmStockContractorWagesHistory, this.ItemLedger, this.btnTradingReports, this.frmRptItemList,
			this.DepartmentRequestHistory, this.StoreIssuenceHistory, this.frmSaleInvoiceReturnRegister, this.frmStockTranfserRegister, this.StockAdjustmentRegister, this.frmPurchaseRegisterStore, this.frmPurchaseDemandRegister, this.StoreIssuanceReturnRegister
		});
		this.InventoryReport.Name = "InventoryReport";
		this.InventoryReport.Size = new System.Drawing.Size(176, 22);
		this.InventoryReport.Text = "Inventory Reports";
		this.PurchaseOrderHistory.Name = "PurchaseOrderHistory";
		this.PurchaseOrderHistory.Size = new System.Drawing.Size(281, 22);
		this.PurchaseOrderHistory.Text = "Purchase Order Register";
		this.frmGatePassGeneral.Name = "frmGatePassGeneral";
		this.frmGatePassGeneral.Size = new System.Drawing.Size(281, 22);
		this.frmGatePassGeneral.Text = "General GatePass Register";
		this.frmGatePassReport.Name = "frmGatePassReport";
		this.frmGatePassReport.Size = new System.Drawing.Size(281, 22);
		this.frmGatePassReport.Text = "Inward Gate Pass Register";
		this.frmGRNHistory.Name = "frmGRNHistory";
		this.frmGRNHistory.Size = new System.Drawing.Size(281, 22);
		this.frmGRNHistory.Text = "GRN Register";
		this.frmPurchaseInvoiceHistory.Name = "frmPurchaseInvoiceHistory";
		this.frmPurchaseInvoiceHistory.Size = new System.Drawing.Size(281, 22);
		this.frmPurchaseInvoiceHistory.Text = "Purchase Register";
		this.PurchaseRegisterNew.Name = "PurchaseRegisterNew";
		this.PurchaseRegisterNew.Size = new System.Drawing.Size(281, 22);
		this.PurchaseRegisterNew.Text = "Purchase Register New";
		this.frmSaleOrderHistory.Name = "frmSaleOrderHistory";
		this.frmSaleOrderHistory.Size = new System.Drawing.Size(281, 22);
		this.frmSaleOrderHistory.Text = "Sale Order Register";
		this.frmDeliveryOrderForApprovel.Name = "frmDeliveryOrderForApprovel";
		this.frmDeliveryOrderForApprovel.Size = new System.Drawing.Size(281, 22);
		this.frmDeliveryOrderForApprovel.Text = "Delivery Order History";
		this.frmGPOutward.Name = "frmGPOutward";
		this.frmGPOutward.Size = new System.Drawing.Size(281, 22);
		this.frmGPOutward.Text = "Outward Gate Pass Register";
		this.frmGDNHistory.Name = "frmGDNHistory";
		this.frmGDNHistory.Size = new System.Drawing.Size(281, 22);
		this.frmGDNHistory.Text = "GDN Register";
		this.SaleInvoiceRegisterNew.Name = "SaleInvoiceRegisterNew";
		this.SaleInvoiceRegisterNew.Size = new System.Drawing.Size(281, 22);
		this.SaleInvoiceRegisterNew.Text = "Sales Register New";
		this.frmSaleInvoiceHistory.Name = "frmSaleInvoiceHistory";
		this.frmSaleInvoiceHistory.Size = new System.Drawing.Size(281, 22);
		this.frmSaleInvoiceHistory.Text = "Sale Invoice Register";
		this.frmSaleInvoiceDirectRegister.Name = "frmSaleInvoiceDirectRegister";
		this.frmSaleInvoiceDirectRegister.Size = new System.Drawing.Size(281, 22);
		this.frmSaleInvoiceDirectRegister.Text = "Sale Invoice Direct Register";
		this.frmWeightBridgeHistory.Name = "frmWeightBridgeHistory";
		this.frmWeightBridgeHistory.Size = new System.Drawing.Size(281, 22);
		this.frmWeightBridgeHistory.Text = "Weigh Bridge History";
		this.frmWeighBridgeRejectedTicketNos.Name = "frmWeighBridgeRejectedTicketNos";
		this.frmWeighBridgeRejectedTicketNos.Size = new System.Drawing.Size(281, 22);
		this.frmWeighBridgeRejectedTicketNos.Text = "Move Order History For Rejected Status";
		this.frmEvaulationDetailWagesReports.Name = "frmEvaulationDetailWagesReports";
		this.frmEvaulationDetailWagesReports.Size = new System.Drawing.Size(281, 22);
		this.frmEvaulationDetailWagesReports.Text = "Wages Register";
		this.frmStockContractorWagesHistory.Name = "frmStockContractorWagesHistory";
		this.frmStockContractorWagesHistory.Size = new System.Drawing.Size(281, 22);
		this.frmStockContractorWagesHistory.Text = "Contractor Wages History";
		this.ItemLedger.Name = "ItemLedger";
		this.ItemLedger.Size = new System.Drawing.Size(281, 22);
		this.ItemLedger.Text = "Item Ledger";
		this.btnTradingReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.PurchaseInvoiceTrading, this.SaleInvoiceTrading });
		this.btnTradingReports.Name = "btnTradingReports";
		this.btnTradingReports.Size = new System.Drawing.Size(281, 22);
		this.btnTradingReports.Text = "Trading Reports";
		this.PurchaseInvoiceTrading.Name = "PurchaseInvoiceTrading";
		this.PurchaseInvoiceTrading.Size = new System.Drawing.Size(181, 22);
		this.PurchaseInvoiceTrading.Text = "Purchase Register";
		this.SaleInvoiceTrading.Name = "SaleInvoiceTrading";
		this.SaleInvoiceTrading.Size = new System.Drawing.Size(181, 22);
		this.SaleInvoiceTrading.Text = "Sale Invoice Register";
		this.frmRptItemList.Name = "frmRptItemList";
		this.frmRptItemList.Size = new System.Drawing.Size(281, 22);
		this.frmRptItemList.Text = "ItemList";
		this.DepartmentRequestHistory.Name = "DepartmentRequestHistory";
		this.DepartmentRequestHistory.Size = new System.Drawing.Size(281, 22);
		this.DepartmentRequestHistory.Text = "Departmenet Request History";
		this.StoreIssuenceHistory.Name = "StoreIssuenceHistory";
		this.StoreIssuenceHistory.Size = new System.Drawing.Size(281, 22);
		this.StoreIssuenceHistory.Text = "Store Issuance History";
		this.frmSaleInvoiceReturnRegister.Name = "frmSaleInvoiceReturnRegister";
		this.frmSaleInvoiceReturnRegister.Size = new System.Drawing.Size(281, 22);
		this.frmSaleInvoiceReturnRegister.Text = "Sale Invoice Return Register";
		this.frmStockTranfserRegister.Name = "frmStockTranfserRegister";
		this.frmStockTranfserRegister.Size = new System.Drawing.Size(281, 22);
		this.frmStockTranfserRegister.Text = "Stock Transfer Register";
		this.StockAdjustmentRegister.Name = "StockAdjustmentRegister";
		this.StockAdjustmentRegister.Size = new System.Drawing.Size(281, 22);
		this.StockAdjustmentRegister.Text = "Stock Adjustment Register";
		this.frmPurchaseRegisterStore.Name = "frmPurchaseRegisterStore";
		this.frmPurchaseRegisterStore.Size = new System.Drawing.Size(281, 22);
		this.frmPurchaseRegisterStore.Text = "Purchase Register Store";
		this.frmPurchaseDemandRegister.Name = "frmPurchaseDemandRegister";
		this.frmPurchaseDemandRegister.Size = new System.Drawing.Size(281, 22);
		this.frmPurchaseDemandRegister.Text = "Purchase Demand Register";
		this.StoreIssuanceReturnRegister.Name = "StoreIssuanceReturnRegister";
		this.StoreIssuanceReturnRegister.Size = new System.Drawing.Size(281, 22);
		this.StoreIssuanceReturnRegister.Text = "Return to Store Register";
		this.btnExportHistory.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[15]
		{
			this.frmContainerList, this.SaleContractHistory, this.ExportInvoiceRegister, this.btnShipmentHistory, this.frmExImForwardingSlipandRegister, this.frmBillofLadingSlipandRegister, this.frmExportShipingLineBookingRpt, this.FrmRptExImShipmentScheduleHistroy, this.frmshippedConsignment_followup, this.ExImRptFrmAdvancePaymentReceiptAndUtilization,
			this.ExportCustomerWiseComparisonSummary, this.ExportComparisonSummary, this.ExportDetailHistory, this.frmExportSummaryRegister, this.ExImShipmentCosting
		});
		this.btnExportHistory.Name = "btnExportHistory";
		this.btnExportHistory.Size = new System.Drawing.Size(176, 22);
		this.btnExportHistory.Text = "Export Reports";
		this.frmContainerList.Name = "frmContainerList";
		this.frmContainerList.Size = new System.Drawing.Size(318, 22);
		this.frmContainerList.Text = "Container List";
		this.SaleContractHistory.Name = "SaleContractHistory";
		this.SaleContractHistory.Size = new System.Drawing.Size(318, 22);
		this.SaleContractHistory.Text = "Sales Contract History";
		this.ExportInvoiceRegister.Name = "ExportInvoiceRegister";
		this.ExportInvoiceRegister.Size = new System.Drawing.Size(318, 22);
		this.ExportInvoiceRegister.Text = "Commercial Invoice History";
		this.btnShipmentHistory.Name = "btnShipmentHistory";
		this.btnShipmentHistory.Size = new System.Drawing.Size(318, 22);
		this.btnShipmentHistory.Text = "Shipment History";
		this.frmExImForwardingSlipandRegister.Name = "frmExImForwardingSlipandRegister";
		this.frmExImForwardingSlipandRegister.Size = new System.Drawing.Size(318, 22);
		this.frmExImForwardingSlipandRegister.Text = "Forwarding History";
		this.frmBillofLadingSlipandRegister.Name = "frmBillofLadingSlipandRegister";
		this.frmBillofLadingSlipandRegister.Size = new System.Drawing.Size(318, 22);
		this.frmBillofLadingSlipandRegister.Text = "Bill of lading History";
		this.frmExportShipingLineBookingRpt.Name = "frmExportShipingLineBookingRpt";
		this.frmExportShipingLineBookingRpt.Size = new System.Drawing.Size(318, 22);
		this.frmExportShipingLineBookingRpt.Text = "ShippingLine Booking";
		this.FrmRptExImShipmentScheduleHistroy.Name = "FrmRptExImShipmentScheduleHistroy";
		this.FrmRptExImShipmentScheduleHistroy.Size = new System.Drawing.Size(318, 22);
		this.FrmRptExImShipmentScheduleHistroy.Text = "Shipment Schedule History";
		this.frmshippedConsignment_followup.Name = "frmshippedConsignment_followup";
		this.frmshippedConsignment_followup.Size = new System.Drawing.Size(318, 22);
		this.frmshippedConsignment_followup.Text = "ShippedConsignment followup";
		this.ExImRptFrmAdvancePaymentReceiptAndUtilization.Name = "ExImRptFrmAdvancePaymentReceiptAndUtilization";
		this.ExImRptFrmAdvancePaymentReceiptAndUtilization.Size = new System.Drawing.Size(318, 22);
		this.ExImRptFrmAdvancePaymentReceiptAndUtilization.Text = "Advance Payment Receipt Utilization";
		this.ExportCustomerWiseComparisonSummary.Name = "ExportCustomerWiseComparisonSummary";
		this.ExportCustomerWiseComparisonSummary.Size = new System.Drawing.Size(318, 22);
		this.ExportCustomerWiseComparisonSummary.Text = "Export Customer Wise Comparisons Summery";
		this.ExportComparisonSummary.Name = "ExportComparisonSummary";
		this.ExportComparisonSummary.Size = new System.Drawing.Size(318, 22);
		this.ExportComparisonSummary.Text = "Export Comparisons Summery";
		this.ExportDetailHistory.Name = "ExportDetailHistory";
		this.ExportDetailHistory.Size = new System.Drawing.Size(318, 22);
		this.ExportDetailHistory.Text = "Export Detail History Report";
		this.frmExportSummaryRegister.Name = "frmExportSummaryRegister";
		this.frmExportSummaryRegister.Size = new System.Drawing.Size(318, 22);
		this.frmExportSummaryRegister.Text = "Export Summary";
		this.ExImShipmentCosting.Name = "ExImShipmentCosting";
		this.ExImShipmentCosting.Size = new System.Drawing.Size(318, 22);
		this.ExImShipmentCosting.Text = "Shipment Costing";
		this.btnLabReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.InvLabPurchaseRegister });
		this.btnLabReports.Name = "btnLabReports";
		this.btnLabReports.Size = new System.Drawing.Size(176, 22);
		this.btnLabReports.Text = "Lab Reports";
		this.InvLabPurchaseRegister.Name = "InvLabPurchaseRegister";
		this.InvLabPurchaseRegister.Size = new System.Drawing.Size(189, 22);
		this.InvLabPurchaseRegister.Text = "Lab Purchase Register";
		this.btnCargoReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.CbdReceivablesandReceiptsHistory });
		this.btnCargoReports.Name = "btnCargoReports";
		this.btnCargoReports.Size = new System.Drawing.Size(176, 22);
		this.btnCargoReports.Text = "Cargo Report";
		this.CbdReceivablesandReceiptsHistory.Name = "CbdReceivablesandReceiptsHistory";
		this.CbdReceivablesandReceiptsHistory.Size = new System.Drawing.Size(242, 22);
		this.CbdReceivablesandReceiptsHistory.Text = "Receivables & Receipts ByI nvoice";
		this.btnStockReport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.frmStockReport, this.InventoryTransactionReport, this.RptGenrateStocks, this.InventoryEvaluationItemLedger, this.InventoryTransactionReportStore, this.RptGenrateStocksStore, this.ItemLedgerReport_Inventory, this.StockReportWithAvgRates, this.frmInventoryProfitLoss });
		this.btnStockReport.Name = "btnStockReport";
		this.btnStockReport.Size = new System.Drawing.Size(176, 22);
		this.btnStockReport.Text = "Stock Reports";
		this.frmStockReport.Name = "frmStockReport";
		this.frmStockReport.Size = new System.Drawing.Size(217, 22);
		this.frmStockReport.Text = "Stock Report";
		this.InventoryTransactionReport.Name = "InventoryTransactionReport";
		this.InventoryTransactionReport.Size = new System.Drawing.Size(217, 22);
		this.InventoryTransactionReport.Text = "Inventory Transaction";
		this.RptGenrateStocks.Name = "RptGenrateStocks";
		this.RptGenrateStocks.Size = new System.Drawing.Size(217, 22);
		this.RptGenrateStocks.Text = "Stock Report With Values";
		this.RptGenrateStocks.Visible = false;
		this.InventoryEvaluationItemLedger.Name = "InventoryEvaluationItemLedger";
		this.InventoryEvaluationItemLedger.Size = new System.Drawing.Size(217, 22);
		this.InventoryEvaluationItemLedger.Text = "Inventory Item Ledger";
		this.InventoryTransactionReportStore.Name = "InventoryTransactionReportStore";
		this.InventoryTransactionReportStore.Size = new System.Drawing.Size(217, 22);
		this.InventoryTransactionReportStore.Text = "Inventory Transaction Store";
		this.RptGenrateStocksStore.Name = "RptGenrateStocksStore";
		this.RptGenrateStocksStore.Size = new System.Drawing.Size(217, 22);
		this.RptGenrateStocksStore.Text = "Stock Report Store";
		this.ItemLedgerReport_Inventory.Name = "ItemLedgerReport_Inventory";
		this.ItemLedgerReport_Inventory.Size = new System.Drawing.Size(217, 22);
		this.ItemLedgerReport_Inventory.Text = "Item Stock Ledger";
		this.StockReportWithAvgRates.Name = "StockReportWithAvgRates";
		this.StockReportWithAvgRates.Size = new System.Drawing.Size(217, 22);
		this.StockReportWithAvgRates.Text = "Current Stock Report";
		this.frmInventoryProfitLoss.Name = "frmInventoryProfitLoss";
		this.frmInventoryProfitLoss.Size = new System.Drawing.Size(217, 22);
		this.frmInventoryProfitLoss.Text = "Item Wise Proft & Loss";
		this.btnEmptyBagsReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.frmStockWithSupplierHistory });
		this.btnEmptyBagsReports.Name = "btnEmptyBagsReports";
		this.btnEmptyBagsReports.Size = new System.Drawing.Size(176, 22);
		this.btnEmptyBagsReports.Text = "Empty Bags Report";
		this.frmStockWithSupplierHistory.Name = "frmStockWithSupplierHistory";
		this.frmStockWithSupplierHistory.Size = new System.Drawing.Size(221, 22);
		this.frmStockWithSupplierHistory.Text = "Supplier Ledger Empty Bags";
		this.productionReportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.frmProductionPendingPurchaseInvoice, this.FoodProductionComparisonRpt });
		this.productionReportsToolStripMenuItem.Name = "productionReportsToolStripMenuItem";
		this.productionReportsToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
		this.productionReportsToolStripMenuItem.Text = "Production Reports";
		this.frmProductionPendingPurchaseInvoice.Name = "frmProductionPendingPurchaseInvoice";
		this.frmProductionPendingPurchaseInvoice.Size = new System.Drawing.Size(246, 22);
		this.frmProductionPendingPurchaseInvoice.Text = "Pending Invoice For Production";
		this.FoodProductionComparisonRpt.Name = "FoodProductionComparisonRpt";
		this.FoodProductionComparisonRpt.Size = new System.Drawing.Size(246, 22);
		this.FoodProductionComparisonRpt.Text = "Production Comparison Register";
		this.btnHRMReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.EmployeeHistoryRpt, this.btnPayrollSalaryReport, this.DailyAttendanceRpt, this.DailyLateandEarlyDeparture, this.genDutyRosterEmployeeWise, this.genEmployeeAttendence, this.MonthlyAttendanceSummary, this.EmployeeMonthlyRegister });
		this.btnHRMReports.Name = "btnHRMReports";
		this.btnHRMReports.Size = new System.Drawing.Size(176, 22);
		this.btnHRMReports.Text = "HRM Reports";
		this.EmployeeHistoryRpt.Name = "EmployeeHistoryRpt";
		this.EmployeeHistoryRpt.Size = new System.Drawing.Size(237, 22);
		this.EmployeeHistoryRpt.Text = "Employee Register";
		this.btnPayrollSalaryReport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.PayRollSalarySheetRpt });
		this.btnPayrollSalaryReport.Name = "btnPayrollSalaryReport";
		this.btnPayrollSalaryReport.Size = new System.Drawing.Size(237, 22);
		this.btnPayrollSalaryReport.Text = "PayRoll Reports";
		this.PayRollSalarySheetRpt.Name = "PayRollSalarySheetRpt";
		this.PayRollSalarySheetRpt.Size = new System.Drawing.Size(137, 22);
		this.PayRollSalarySheetRpt.Text = "Salary Sheet";
		this.DailyAttendanceRpt.Name = "DailyAttendanceRpt";
		this.DailyAttendanceRpt.Size = new System.Drawing.Size(237, 22);
		this.DailyAttendanceRpt.Text = "Daily Reports";
		this.DailyLateandEarlyDeparture.Name = "DailyLateandEarlyDeparture";
		this.DailyLateandEarlyDeparture.Size = new System.Drawing.Size(237, 22);
		this.DailyLateandEarlyDeparture.Text = "Daily Late and Early Departure";
		this.genDutyRosterEmployeeWise.Name = "genDutyRosterEmployeeWise";
		this.genDutyRosterEmployeeWise.Size = new System.Drawing.Size(237, 22);
		this.genDutyRosterEmployeeWise.Text = "Duty Roaster Employee Wise";
		this.genEmployeeAttendence.Name = "genEmployeeAttendence";
		this.genEmployeeAttendence.Size = new System.Drawing.Size(237, 22);
		this.genEmployeeAttendence.Text = "Employee Attendance";
		this.MonthlyAttendanceSummary.Name = "MonthlyAttendanceSummary";
		this.MonthlyAttendanceSummary.Size = new System.Drawing.Size(237, 22);
		this.MonthlyAttendanceSummary.Text = "Monthly Attendance Summary";
		this.EmployeeMonthlyRegister.Name = "EmployeeMonthlyRegister";
		this.EmployeeMonthlyRegister.Size = new System.Drawing.Size(237, 22);
		this.EmployeeMonthlyRegister.Text = "Employee Monthly Register";
		this.btnImportReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.ImportContractRegister, this.ImportPurchaseOrderRegister, this.ImportOrderPackingDetailRegister, this.ImportGrnRegister, this.ImportInvoiceRegister });
		this.btnImportReports.Name = "btnImportReports";
		this.btnImportReports.Size = new System.Drawing.Size(176, 22);
		this.btnImportReports.Text = "Import Reports";
		this.ImportContractRegister.Name = "ImportContractRegister";
		this.ImportContractRegister.Size = new System.Drawing.Size(260, 22);
		this.ImportContractRegister.Text = "Import Contract Register";
		this.ImportPurchaseOrderRegister.Name = "ImportPurchaseOrderRegister";
		this.ImportPurchaseOrderRegister.Size = new System.Drawing.Size(260, 22);
		this.ImportPurchaseOrderRegister.Text = "Import Purchase Order Register";
		this.ImportOrderPackingDetailRegister.Name = "ImportOrderPackingDetailRegister";
		this.ImportOrderPackingDetailRegister.Size = new System.Drawing.Size(260, 22);
		this.ImportOrderPackingDetailRegister.Text = "Import Purchase Order Packing List";
		this.ImportGrnRegister.Name = "ImportGrnRegister";
		this.ImportGrnRegister.Size = new System.Drawing.Size(260, 22);
		this.ImportGrnRegister.Text = "Import Grn Register";
		this.ImportInvoiceRegister.Name = "ImportInvoiceRegister";
		this.ImportInvoiceRegister.Size = new System.Drawing.Size(260, 22);
		this.ImportInvoiceRegister.Text = "Import Invoice Register";
		this.contextMenuStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnadminpanel, this.btnUtility });
		this.contextMenuStrip3.Name = "ContextMenuStrip1";
		this.contextMenuStrip3.Size = new System.Drawing.Size(155, 48);
		this.btnadminpanel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.btnadminpanelsysconfig, this.btnadminpaneluserright, this.btnScreenRights, this.btnScreenDefination, this.DefineReports, this.btnStock_Reconcilation, this.btnPlBSNotes, this.companyProfileToolStripMenuItem, this.btnAuditLogReport, this.sQLQueryExecuterToolStripMenuItem,
			this.btnWeighBridge_WeightUpdate, this.BtnThemeForm, this.btnAccountMovement
		});
		this.btnadminpanel.Name = "btnadminpanel";
		this.btnadminpanel.Size = new System.Drawing.Size(154, 22);
		this.btnadminpanel.Text = "Admin Panel";
		this.btnadminpanel.Visible = false;
		this.btnadminpanelsysconfig.Name = "btnadminpanelsysconfig";
		this.btnadminpanelsysconfig.Size = new System.Drawing.Size(228, 22);
		this.btnadminpanelsysconfig.Text = "System Configuration";
		this.btnadminpanelsysconfig.Click += new System.EventHandler(btnadminpanelsysconfig_Click);
		this.btnadminpaneluserright.Name = "btnadminpaneluserright";
		this.btnadminpaneluserright.Size = new System.Drawing.Size(228, 22);
		this.btnadminpaneluserright.Text = "User Rights";
		this.btnadminpaneluserright.Click += new System.EventHandler(btnadminpaneluserright_Click);
		this.btnScreenRights.Name = "btnScreenRights";
		this.btnScreenRights.Size = new System.Drawing.Size(228, 22);
		this.btnScreenRights.Text = "Screen Allocate To User";
		this.btnScreenRights.Click += new System.EventHandler(btnScreenRights_Click);
		this.btnScreenDefination.Name = "btnScreenDefination";
		this.btnScreenDefination.Size = new System.Drawing.Size(228, 22);
		this.btnScreenDefination.Text = "Screen Defination";
		this.btnScreenDefination.Visible = false;
		this.btnScreenDefination.Click += new System.EventHandler(btnScreenDefination_Click);
		this.DefineReports.Name = "DefineReports";
		this.DefineReports.Size = new System.Drawing.Size(228, 22);
		this.DefineReports.Text = "Define Reports";
		this.DefineReports.Click += new System.EventHandler(DefineReports_Click);
		this.btnStock_Reconcilation.Name = "btnStock_Reconcilation";
		this.btnStock_Reconcilation.Size = new System.Drawing.Size(228, 22);
		this.btnStock_Reconcilation.Text = "Stock_Reconcilation";
		this.btnStock_Reconcilation.Visible = false;
		this.btnStock_Reconcilation.Click += new System.EventHandler(btnStock_Reconcilation_Click);
		this.btnPlBSNotes.Name = "btnPlBSNotes";
		this.btnPlBSNotes.Size = new System.Drawing.Size(228, 22);
		this.btnPlBSNotes.Text = "PL & BS Notes";
		this.btnPlBSNotes.Click += new System.EventHandler(btnPlBSNotes_Click);
		this.companyProfileToolStripMenuItem.Name = "companyProfileToolStripMenuItem";
		this.companyProfileToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
		this.companyProfileToolStripMenuItem.Text = "Company Profile";
		this.companyProfileToolStripMenuItem.Click += new System.EventHandler(companyProfileToolStripMenuItem_Click);
		this.btnAuditLogReport.Name = "btnAuditLogReport";
		this.btnAuditLogReport.Size = new System.Drawing.Size(228, 22);
		this.btnAuditLogReport.Text = "Audit Log Report";
		this.btnAuditLogReport.Click += new System.EventHandler(btnAuditLogReport_Click);
		this.sQLQueryExecuterToolStripMenuItem.Name = "sQLQueryExecuterToolStripMenuItem";
		this.sQLQueryExecuterToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
		this.sQLQueryExecuterToolStripMenuItem.Text = "SQL Query Executer";
		this.sQLQueryExecuterToolStripMenuItem.Click += new System.EventHandler(sQLQueryExecuterToolStripMenuItem_Click);
		this.btnWeighBridge_WeightUpdate.Name = "btnWeighBridge_WeightUpdate";
		this.btnWeighBridge_WeightUpdate.Size = new System.Drawing.Size(228, 22);
		this.btnWeighBridge_WeightUpdate.Text = "WeighBridge_WeightsUpdate";
		this.btnWeighBridge_WeightUpdate.Click += new System.EventHandler(btnWeighBridge_WeightUpdate_Click);
		this.BtnThemeForm.Name = "BtnThemeForm";
		this.BtnThemeForm.Size = new System.Drawing.Size(228, 22);
		this.BtnThemeForm.Text = "Theme";
		this.BtnThemeForm.Click += new System.EventHandler(BtnThemeForm_Click);
		this.btnAccountMovement.Name = "btnAccountMovement";
		this.btnAccountMovement.Size = new System.Drawing.Size(228, 22);
		this.btnAccountMovement.Text = "Account Movement";
		this.btnAccountMovement.Click += new System.EventHandler(btnAccountMovement_Click);
		this.btnUtility.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.btnDatabaseBackup, this.licenseKeyToolStripMenuItem, this.btnAccountLookupa, this.btnSupplierDefineSystemUtilities, this.defineInventoryToolStripMenuItem, this.btnCountry, this.DateLock, this.btnSMSContacts, this.btnScreenMessageReceivers, this.btnSMSConfiquration,
			this.btnHistory, this.invLookUpsToolStripMenuItem, this.ScaleKartDefine
		});
		this.btnUtility.Name = "btnUtility";
		this.btnUtility.Size = new System.Drawing.Size(154, 22);
		this.btnUtility.Text = "System Utilities";
		this.btnDatabaseBackup.Name = "btnDatabaseBackup";
		this.btnDatabaseBackup.Size = new System.Drawing.Size(210, 22);
		this.btnDatabaseBackup.Text = "DataBase Backup";
		this.btnDatabaseBackup.Click += new System.EventHandler(btnDatabaseBackup_Click_1);
		this.licenseKeyToolStripMenuItem.Name = "licenseKeyToolStripMenuItem";
		this.licenseKeyToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
		this.licenseKeyToolStripMenuItem.Text = "License Key";
		this.licenseKeyToolStripMenuItem.Click += new System.EventHandler(licenseKeyToolStripMenuItem_Click);
		this.btnAccountLookupa.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnPlAndBsSettings });
		this.btnAccountLookupa.Name = "btnAccountLookupa";
		this.btnAccountLookupa.Size = new System.Drawing.Size(210, 22);
		this.btnAccountLookupa.Text = "Accounts Lookups";
		this.btnAccountLookupa.Visible = false;
		this.btnPlAndBsSettings.Name = "btnPlAndBsSettings";
		this.btnPlAndBsSettings.Size = new System.Drawing.Size(150, 22);
		this.btnPlAndBsSettings.Text = "PL_BS Settings";
		this.btnSupplierDefineSystemUtilities.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.shipToAddressToolStripMenuItem, this.supplierLimitsToolStripMenuItem, this.supplierBankDetailToolStripMenuItem });
		this.btnSupplierDefineSystemUtilities.Name = "btnSupplierDefineSystemUtilities";
		this.btnSupplierDefineSystemUtilities.Size = new System.Drawing.Size(210, 22);
		this.btnSupplierDefineSystemUtilities.Text = "SupplierDefine";
		this.btnSupplierDefineSystemUtilities.Visible = false;
		this.shipToAddressToolStripMenuItem.Name = "shipToAddressToolStripMenuItem";
		this.shipToAddressToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
		this.shipToAddressToolStripMenuItem.Text = "Ship to Address";
		this.supplierLimitsToolStripMenuItem.Name = "supplierLimitsToolStripMenuItem";
		this.supplierLimitsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
		this.supplierLimitsToolStripMenuItem.Text = "SupplierLimits";
		this.supplierBankDetailToolStripMenuItem.Name = "supplierBankDetailToolStripMenuItem";
		this.supplierBankDetailToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
		this.supplierBankDetailToolStripMenuItem.Text = "SupplierBankDetail";
		this.defineInventoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnOtherItemsDefine, this.btnPartyItemAllocation, this.btnMultiCurrency, this.btnPackingMaterial });
		this.defineInventoryToolStripMenuItem.Name = "defineInventoryToolStripMenuItem";
		this.defineInventoryToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
		this.defineInventoryToolStripMenuItem.Text = "Define Inventory";
		this.defineInventoryToolStripMenuItem.Visible = false;
		this.btnOtherItemsDefine.Name = "btnOtherItemsDefine";
		this.btnOtherItemsDefine.Size = new System.Drawing.Size(199, 22);
		this.btnOtherItemsDefine.Text = "Other Items Define";
		this.btnPartyItemAllocation.Name = "btnPartyItemAllocation";
		this.btnPartyItemAllocation.Size = new System.Drawing.Size(199, 22);
		this.btnPartyItemAllocation.Text = "Item Allocation to Party";
		this.btnMultiCurrency.Name = "btnMultiCurrency";
		this.btnMultiCurrency.Size = new System.Drawing.Size(199, 22);
		this.btnMultiCurrency.Text = "MultiCurrency Define";
		this.btnPackingMaterial.Name = "btnPackingMaterial";
		this.btnPackingMaterial.Size = new System.Drawing.Size(199, 22);
		this.btnPackingMaterial.Text = "Packing Material Define";
		this.btnCountry.Name = "btnCountry";
		this.btnCountry.Size = new System.Drawing.Size(210, 22);
		this.btnCountry.Text = "Define Country";
		this.btnCountry.Visible = false;
		this.DateLock.Name = "DateLock";
		this.DateLock.Size = new System.Drawing.Size(210, 22);
		this.DateLock.Text = "DateLock";
		this.DateLock.Visible = false;
		this.btnSMSContacts.Name = "btnSMSContacts";
		this.btnSMSContacts.Size = new System.Drawing.Size(210, 22);
		this.btnSMSContacts.Text = "SMS Contacts";
		this.btnSMSContacts.Visible = false;
		this.btnScreenMessageReceivers.Name = "btnScreenMessageReceivers";
		this.btnScreenMessageReceivers.Size = new System.Drawing.Size(210, 22);
		this.btnScreenMessageReceivers.Text = "Screen Message Receivers";
		this.btnScreenMessageReceivers.Visible = false;
		this.btnSMSConfiquration.Name = "btnSMSConfiquration";
		this.btnSMSConfiquration.Size = new System.Drawing.Size(210, 22);
		this.btnSMSConfiquration.Text = "SMS Confiquration";
		this.btnSMSConfiquration.Visible = false;
		this.btnHistory.Name = "btnHistory";
		this.btnHistory.Size = new System.Drawing.Size(210, 22);
		this.btnHistory.Text = "SMS History";
		this.btnHistory.Visible = false;
		this.invLookUpsToolStripMenuItem.Name = "invLookUpsToolStripMenuItem";
		this.invLookUpsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
		this.invLookUpsToolStripMenuItem.Text = "InvLookUps";
		this.invLookUpsToolStripMenuItem.Visible = false;
		this.ScaleKartDefine.Name = "ScaleKartDefine";
		this.ScaleKartDefine.Size = new System.Drawing.Size(210, 22);
		this.ScaleKartDefine.Text = "Scale Kart Define";
		this.ScaleKartDefine.Visible = false;
		this.contextMenuStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.lockToolStripMenuItem, this.changePasswordToolStripMenuItem, this.UploadFiles, this.dummyGridToolStripMenuItem, this.btnUserProfile });
		this.contextMenuStrip4.Name = "ContextMenuStrip1";
		this.contextMenuStrip4.Size = new System.Drawing.Size(169, 114);
		this.lockToolStripMenuItem.Name = "lockToolStripMenuItem";
		this.lockToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
		this.lockToolStripMenuItem.Text = "Lock";
		this.lockToolStripMenuItem.Click += new System.EventHandler(lockToolStripMenuItem_Click);
		this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
		this.changePasswordToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
		this.changePasswordToolStripMenuItem.Text = "Change Password";
		this.changePasswordToolStripMenuItem.Click += new System.EventHandler(changePasswordToolStripMenuItem_Click);
		this.UploadFiles.Name = "UploadFiles";
		this.UploadFiles.Size = new System.Drawing.Size(168, 22);
		this.UploadFiles.Text = "Upload Files";
		this.UploadFiles.Visible = false;
		this.dummyGridToolStripMenuItem.Name = "dummyGridToolStripMenuItem";
		this.dummyGridToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
		this.dummyGridToolStripMenuItem.Text = "Dummy Grid";
		this.dummyGridToolStripMenuItem.Visible = false;
		this.btnUserProfile.Name = "btnUserProfile";
		this.btnUserProfile.Size = new System.Drawing.Size(168, 22);
		this.btnUserProfile.Text = "User Profile";
		this.btnUserProfile.Visible = false;
		this.btnUserProfile.Click += new System.EventHandler(btnUserProfile_Click);
		this.timer2.Tick += new System.EventHandler(timer2_Tick);
		this.panel4.Controls.Add(this.panel2);
		this.panel4.Controls.Add(this.panel3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(55, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1137, 594);
		this.panel4.TabIndex = 4;
		this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.panel2.Location = new System.Drawing.Point(0, 37);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1134, 557);
		this.panel2.TabIndex = 0;
		this.contextMenuStrip5.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip5.Name = "ContextMenuStrip1";
		this.contextMenuStrip5.Size = new System.Drawing.Size(61, 4);
		this.contextMenuStrip6.Name = "contextMenuStrip6";
		this.contextMenuStrip6.Size = new System.Drawing.Size(61, 4);
		this.bw_CheckDownload.DoWork += new System.ComponentModel.DoWorkEventHandler(bw_CheckDownload_DoWork_1);
		this.bw_CheckDownload.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bw_CheckDownload_ProgressChanged);
		this.bw_CheckDownload.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_CheckDownload_RunWorkerCompleted_1);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1192, 594);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.pnlmain);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "DashboardNew";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DashboardNew_FormClosing);
		base.Load += new System.EventHandler(DashboardNew_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DashboardNew_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnUpdate).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNavigation).EndInit();
		this.pnlmain.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.btnRefresh).EndInit();
		this.ContextMenuStrip1.ResumeLayout(false);
		this.contextMenuStrip2.ResumeLayout(false);
		this.contextMenuStrip3.ResumeLayout(false);
		this.contextMenuStrip4.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
