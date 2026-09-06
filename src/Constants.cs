using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Common
{

public class Constants
{
	public class ExportConstants
	{
		public static int DocNoConstant { get; set; } = 55;


		public static int VoucherNo { get; set; } = 60;


		public static int ExportVoucherNo { get; set; } = 75;


		public static int DateConstant { get; set; } = 73;


		public static int NoOfAttachmentsConstants { get; set; } = 100;


		public static int NoOfContainers { get; set; } = 80;


		public static int ProformaNo { get; set; } = 100;


		public static int IsApproved { get; set; } = 100;


		public static int DateTimeConstantddMMM { get; set; } = 140;


		public static int CropYearConstant { get; set; } = 73;


		public static int ContractNoConstant { get; set; } = 100;


		public static int PackingTypeConstant { get; set; } = 115;


		public static int ItemNameConstants { get; set; } = 270;


		public static int PartyName { get; set; } = 170;


		public static int InvoiceNoConstant { get; set; } = 110;


		public static int AccountTitle { get; set; } = 150;


		public static int GpNoConstant { get; set; } = 50;


		public static int EFormNoConstant { get; set; } = 50;


		public static int AddressConstant { get; set; } = 100;


		public static int DoNoConstant { get; set; } = 50;


		public static int StringConstant { get; set; } = 130;


		public static int PortConstant { get; set; } = 100;


		public static int VehicleNoConstant { get; set; } = 80;


		public static int VehicleType { get; set; } = 100;


		public static int MobileNo { get; set; } = 90;


		public static int ContainerNo { get; set; } = 100;


		public static int BiltyNoConstant { get; set; } = 60;


		public static int FcyConstant { get; set; } = 60;


		public static int CarrierTypeConstant { get; set; } = 60;


		public static int FreightConstant { get; set; } = 90;


		public static int ShippingCertNoConstant { get; set; } = 90;


		public static int DriverCellNoConstant { get; set; } = 90;


		public static int DriverCnicNoConstant { get; set; } = 90;


		public static int SealNo { get; set; } = 70;


		public static int ScheduleCode { get; set; } = 90;


		public static int Days { get; set; } = 60;


		public static int DueDays { get; set; } = 60;


		public static int OverDueDays { get; set; } = 60;


		public static int CarrierMedium { get; set; } = 70;


		public static int TermConstant { get; set; } = 70;


		public static int EntryUserConstants { get; set; } = 100;


		public static int ModifyUserConstants { get; set; } = 100;


		public static int UomConstant { get; set; } = 75;


		public static int QtyConstant { get; set; } = 70;


		public static int EbUnit { get; set; } = 70;


		public static int EbTotal { get; set; } = 70;


		public static int FclConstant { get; set; } = 60;


		public static int VesselNoConstant { get; set; } = 60;


		public static int VoyageNoConstant { get; set; } = 60;


		public static int BookingCroNoConstant { get; set; } = 60;


		public static int BLNumberConstant { get; set; } = 60;


		public static int StatusConstant { get; set; } = 75;


		public static int RemarksConstants { get; set; } = 100;


		public static int WeightConstant { get; set; } = 80;


		public static int ApprovedByConstant { get; set; } = 65;


		public static int AddLessConstant { get; set; } = 80;


		public static int AmountConstant { get; set; } = 90;


		public static int AmountForReportConstant { get; set; } = 110;


		public static int LcyAmountConstant { get; set; } = 110;


		public static int PercentOfTotal { get; set; } = 60;


		public static int CommodityDetail { get; set; } = 150;


		public static int HsCode { get; set; } = 110;


		public static int JobLotConstant { get; set; } = 80;


		public static int ExRateConstant { get; set; } = 70;


		public static int CurrentStatus { get; set; } = 90;


		public static int BrandUomConstant { get; set; } = 95;


		public static int DocumentStatus { get; set; } = 100;


		public static int Country { get; set; } = 100;


		public static int Province { get; set; } = 100;


		public static int City { get; set; } = 100;


		public static int Email { get; set; } = 100;

	}

	public class InventoryConstants
	{
		public static int DocNoConstant { get; set; } = 60;


		public static int VoucherNo { get; set; } = 65;


		public static int DateConstant { get; set; } = 73;


		public static int RangeConstant { get; set; } = 73;


		public static int DateTimeConstant { get; set; } = 100;


		public static int DateTimeConstantddMMM { get; set; } = 135;


		public static int NoOfAttachmentsConstants { get; set; } = 100;


		public static int CropYearConstant { get; set; } = 73;


		public static int ContractNoConstant { get; set; } = 100;


		public static int ContainerConstant { get; set; } = 100;


		public static int AccountTitle { get; set; } = 150;


		public static int AccountCode { get; set; } = 100;


		public static int WorkStationName { get; set; } = 140;


		public static int OperationName { get; set; } = 130;


		public static int SupplierName { get; set; } = 170;


		public static int CommissionAgent { get; set; } = 150;


		public static int ReferenceParty { get; set; } = 150;


		public static int OrderNo { get; set; } = 80;


		public static int ServiceActivity { get; set; } = 100;


		public static int ItemName { get; set; } = 150;


		public static int ItemCode { get; set; } = 80;


		public static int PartyCode { get; set; } = 100;


		public static int ItemType { get; set; } = 110;


		public static int MotherItem { get; set; } = 100;


		public static int MobileNo { get; set; } = 100;


		public static int ItemClass { get; set; } = 100;


		public static int Address { get; set; } = 150;


		public static int CellNo { get; set; } = 100;


		public static int WareHouseName { get; set; } = 150;


		public static int WagesActity { get; set; } = 140;


		public static int BankName { get; set; } = 150;


		public static int ProductionStage { get; set; } = 120;


		public static int InvoiceNoConstant { get; set; } = 100;


		public static int GpType { get; set; } = 80;


		public static int GpNoConstant { get; set; } = 45;


		public static int EFormNoConstant { get; set; } = 50;


		public static int AddressConstant { get; set; } = 100;


		public static int DoNoConstant { get; set; } = 50;


		public static int ManualBillNo { get; set; } = 90;


		public static int ProductionNo { get; set; } = 90;


		public static int PlantName { get; set; } = 100;


		public static int StringConstant { get; set; } = 130;


		public static int PortConstant { get; set; } = 100;


		public static int DriverName { get; set; } = 100;


		public static int VehicleNoConstant { get; set; } = 80;


		public static int VehicleTypeConstant { get; set; } = 80;


		public static int BiltyNoConstant { get; set; } = 60;


		public static int Aflatoxins { get; set; } = 80;


		public static int FcyConstant { get; set; } = 60;


		public static int CarrierTypeConstant { get; set; } = 60;


		public static int FreightConstant { get; set; } = 90;


		public static int ShippingCertNoConstant { get; set; } = 90;


		public static int DriverCellNoConstant { get; set; } = 90;


		public static int DriverCnicNoConstant { get; set; } = 90;


		public static int SealNo { get; set; } = 70;


		public static int Days { get; set; } = 60;


		public static int DueDays { get; set; } = 60;


		public static int OverDueDays { get; set; } = 60;


		public static int CarrierMedium { get; set; } = 70;


		public static int TermConstant { get; set; } = 70;


		public static int Building { get; set; } = 80;


		public static int EntryUserConstants { get; set; } = 80;


		public static int ModifyUserConstants { get; set; } = 80;


		public static int DocumentType { get; set; } = 150;


		public static int CategoryCode { get; set; } = 100;


		public static int CategoryDescription { get; set; } = 150;


		public static int OtherDescription { get; set; } = 150;


		public static int DocumentTypeCode { get; set; } = 70;


		public static int PackingType { get; set; } = 115;


		public static int UomConstant { get; set; } = 60;


		public static int RecordNo { get; set; } = 60;


		public static int Variant { get; set; } = 65;


		public static int VariantEquivalent { get; set; } = 100;


		public static int CastingType { get; set; } = 90;


		public static int QtyConstant { get; set; } = 70;


		public static int CityName { get; set; } = 90;


		public static int FclConstant { get; set; } = 60;


		public static int VesselNoConstant { get; set; } = 60;


		public static int VoyageNoConstant { get; set; } = 60;


		public static int BookingCroNoConstant { get; set; } = 60;


		public static int BLNumberConstant { get; set; } = 60;


		public static int StatusConstant { get; set; } = 75;


		public static int RemarksConstants { get; set; } = 100;


		public static int BranchName { get; set; } = 120;


		public static int WeightConstant { get; set; } = 80;


		public static int ApprovedByConstant { get; set; } = 65;


		public static int IsApproved { get; set; } = 75;


		public static int AddLessConstant { get; set; } = 80;


		public static int DiscountType { get; set; } = 80;


		public static int AmountConstant { get; set; } = 90;


		public static int AmountForReportConstant { get; set; } = 110;


		public static int PercentOfTotal { get; set; } = 55;


		public static int CommodityDetail { get; set; } = 150;


		public static int JobLotConstant { get; set; } = 80;


		public static int ExRateConstant { get; set; } = 70;


		public static int CurrencyConstant { get; set; } = 80;


		public static int CurrentStatus { get; set; } = 90;


		public static int DocumentStatus { get; set; } = 100;


		public static int Caption { get; set; } = 100;


		public static int EbWt { get; set; } = 70;


		public static int WtCut { get; set; } = 70;

	}

	public class LabConstants
	{
		public static int AnalysisParameter { get; set; } = 140;


		public static int MinValue { get; set; } = 65;


		public static int MaxValue { get; set; } = 65;


		public static int ResultValue { get; set; } = 65;

	}
}

}
