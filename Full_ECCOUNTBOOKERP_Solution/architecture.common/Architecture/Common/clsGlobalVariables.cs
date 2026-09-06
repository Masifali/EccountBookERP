using System;
using System.Collections.Generic;
using System.Data;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.DAW;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.Mfg;
using Architecture.Model.Themes_ERP;

namespace Architecture.Common;

public class clsGlobalVariables
{
	public class WagesRefDocumentsStatus
	{
		public bool IsActive { get; set; }

		public int RefDocumentTypeId { get; set; }

		public string DocumentTypeDescription { get; set; }
	}

	public static FinancialYear ActiveYr = new FinancialYear();

	public static UserAccount UserAccount = new UserAccount();

	public static List<tblUserRights> ScreenViewReights = new List<tblUserRights>();

	public static List<ConfigrationsAllocation> configrationsAllocation = new List<ConfigrationsAllocation>();

	public static List<ErpFeaturesByCompany> ErpFeaturesList = new List<ErpFeaturesByCompany>();

	public static List<WagesRefDocumentsStatus> WagesRefDocumentsStatusList = new List<WagesRefDocumentsStatus>();

	public static List<ApplicationsAllocateToCompany> GetApplicationsAllocateToCompanyList = new List<ApplicationsAllocateToCompany>();

	public static List<InvDueTerms> globalPaymentTerm = new List<InvDueTerms>();

	public static List<InvPackingType> globalInvPackingType = new List<InvPackingType>();

	public static List<MultiCurrency> globalMultiCurrency = new List<MultiCurrency>();

	public static List<LookUps> GetAllLookUps_Engr = new List<LookUps>();

	public static List<PolicyDocuments> Daw_PolicyDocuments = new List<PolicyDocuments>();

	public static List<ThemeSettings> GetThemeSettings = new List<ThemeSettings>();

	public static List<GlobalAllCity> globalAllCities = new List<GlobalAllCity>();

	public static List<getGlobalBranchesAllocateToUser> globalBranchesAllocateToUser = new List<getGlobalBranchesAllocateToUser>();

	public static List<getGlobalAllAccountsWithCustomGroup> AllAccountsWithCustomGroupId = new List<getGlobalAllAccountsWithCustomGroup>();

	public static List<getGlobalAllItemsWithCustomGroup> AllItemsWithCustomGroupId = new List<getGlobalAllItemsWithCustomGroup>();

	public static List<getGlobalAllItems> getGlobalAllItems = new List<getGlobalAllItems>();

	public static List<getGlobalAllSerivesItems> getGlobalAllSerivesItems = new List<getGlobalAllSerivesItems>();

	public static List<getGlobalAllBrands> getGlobalAllBrands = new List<getGlobalAllBrands>();

	public static List<GlobalWarehousesWithBranch> globalWarehousesWithBranches = new List<GlobalWarehousesWithBranch>();

	public static List<getGlobalSupplierCustomer> globalSupplierCustomer = new List<getGlobalSupplierCustomer>();

	public static List<getGlobalAllSupplierCustomer> globalAllSupplierCustomer = new List<getGlobalAllSupplierCustomer>();

	public static List<getGlobalVendorsAndCustomersForTransporter> getGlobalVendorsAndCustomersForTransporters = new List<getGlobalVendorsAndCustomersForTransporter>();

	public static List<GloablUomSchedule> globalUomSchedule = new List<GloablUomSchedule>();

	public static List<GlobalCropYear> globalCropYear = new List<GlobalCropYear>();

	public static List<GetJobLotGlIdsandName> globalJobLot = new List<GetJobLotGlIdsandName>();

	public static List<DriverBioGlobal> globalAllDriverBioInfo = new List<DriverBioGlobal>();

	public static List<GlobalItemCondition> globalItemConditions = new List<GlobalItemCondition>();

	public static List<RacksWithWarehouseAndItem> racksWithWarehouseAndItems = new List<RacksWithWarehouseAndItem>();

	public static List<GlobalWarehouseRack> globalWarehouseRacks = new List<GlobalWarehouseRack>();

	public static List<DeliveryTerms> globalDeliveryTermList = new List<DeliveryTerms>();

	public static DateTime FirstFinancialStartDate;

	public static DateTime RunningFinancialEndDate;

	public static int DefaultNoofDecimalPointsForAmount;

	public static int DefaultNoofDecimalPointsForRate;

	public static string ServiceConnectionString;

	public static string stringFormatsingle;

	public static string stringFormatboth;

	public static string DecimalRateFormate;

	public static int DefaultNoofDecimalPointsForFcyAmount;

	public static int DefaultNoofDecimalPointsForFcyRate;

	public static string stringFormatbothForFcy;

	public static string stringFormatsingleForFcy;

	public static string DecimalFCYRateFormate;

	public static string AttachmentFolderPath;

	public static int CrystalPrintPreviewZoomDefault;

	public static DataTable dtCompLogo = new DataTable();
}
