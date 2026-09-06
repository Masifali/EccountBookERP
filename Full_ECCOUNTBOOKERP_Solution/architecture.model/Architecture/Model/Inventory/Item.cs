using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class Item
{
	public bool ApplyExciese { get; set; }

	public bool ApplyGST { get; set; }

	public bool ApplyVAT { get; set; }

	public bool ItemStatus { get; set; }

	public bool OtherTax { get; set; }

	public bool PostState { get; set; }

	public bool IsCompany { get; set; }

	public bool IsThirdParty { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public double CostPrice { get; set; }

	public double MaxStockLevel { get; set; }

	public double OptimalStockLevel { get; set; }

	public double MinStockLevel { get; set; }

	public double PackQty { get; set; }

	public double PurchasePrice { get; set; }

	public double ReorderLevel { get; set; }

	public double ReOrderQty { get; set; }

	public double RetailPrice { get; set; }

	public double WholeSalePrice { get; set; }

	public double EmptyBagWeight { get; set; }

	public decimal WeightKgs { get; set; }

	public decimal MaxMeaurement { get; set; }

	public int LeadTimeDay { get; set; }

	public double PackSize { get; set; }

	public int PackSizeId { get; set; }

	public bool AllowMultiUom { get; set; }

	public int CropYearId { get; set; }

	public int BaseUnitId { get; set; }

	public int BranchesId { get; set; }

	public int BuyerSupplierId { get; set; }

	public int COGSGLAC { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int ServicesMasterItemId { get; set; }

	public int Id { get; set; }

	public int ItemCategoryId { get; set; }

	public int ItemClassId { get; set; }

	public int ItemTypeId { get; set; }

	public int ManufactureId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int ProjectsId { get; set; }

	public int PurchaseGLAC { get; set; }

	public int SaleGLAC { get; set; }

	public int DepreciationExpenseAcId { get; set; }

	public int accumulatedDepreciationAcId { get; set; }

	public int ExpenseMaintenanceAccountId { get; set; }

	public int CapitalWipAcId { get; set; }

	public int UOMScheduleIdCostRate { get; set; }

	public int UOMScheduleIdPurRate { get; set; }

	public int UOMScheduleIdRetailRate { get; set; }

	public int UOMScheduleIdWhsRate { get; set; }

	public int ItemQcGradeId { get; set; }

	public string AmountCalcType { get; set; }

	public string BaseCropYear { get; set; }

	public string BuyerPartNo { get; set; }

	public string HSCode { get; set; }

	public string ItemCode { get; set; }

	public string ItemCodeNew { get; set; }

	public string ItemName { get; set; }

	public string ItemNameOtherLingo { get; set; }

	public string ManufacturePartNo { get; set; }

	public string Pic1 { get; set; }

	public string Pic2 { get; set; }

	public string ProductNo { get; set; }

	public string BarcodeImage { get; set; }

	public string BarcodeNo { get; set; }

	public string ProductType { get; set; }

	public string ModelName { get; set; }

	public int ItemOriginId { get; set; }

	public string ItemAliasName { get; set; }

	public string ItemSpecification { get; set; }

	public int MaxMeaurementUnitId { get; set; }

	public int itemModalId { get; set; }

	public int UomLookUpId { get; set; }

	public int MasterItemId { get; set; }

	public int PurchaseTypeLookUpId { get; set; }

	public bool IsExpirable { get; set; }

	public bool IsDiscountable { get; set; }

	public bool IsTaxable { get; set; }

	public bool IsImport { get; set; }

	public int ShelfLife { get; set; }

	public double SaleTaxPurPercent { get; set; }

	public bool AtRetailPriceTax { get; set; }

	public bool AtPurcahsePriceTax { get; set; }

	public bool ItemWithVarient { get; set; }

	public double SaleTaxSalesPercent { get; set; }

	public double WeightSemiFinish { get; set; }

	public double WeightSemiFinishSand { get; set; }

	public double WeightFinishGoods { get; set; }

	public int MotherItemId { get; set; }

	public int RackId { get; set; }

	public virtual string ItemQcGrade { get; set; }

	public virtual bool CommOnSale { get; set; }

	public virtual bool CommOnPurchase { get; set; }

	public virtual int TaxTypeId { get; set; }

	public virtual int ItemGroupId { get; set; }

	public virtual double Equivalent { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<ItemAllocation> ItemAllocationlist { get; set; }

	public virtual List<ItemImage> ItemImageslist { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<ItemImage> ItemImagesDeletelist { get; set; }
}
