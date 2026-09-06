package com.mst.models;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real GoldenAcedb dbo.Item table (desktop: Inventory_Definition/
 * InvDefrmAddItem.cs - the actual stockable Item/Product master). Every real column
 * is mapped 1:1, same name (camelCased), same nullability. Id is not a SQL Server
 * IDENTITY column, so ItemService assigns the next one itself (see findMaxId()),
 * same pattern as ItemCategory/ItemType/Brand/ProductType.
 *
 * Only ItemCategoryId, ItemTypeId and RackId are wired as JPA relations (to the
 * already-real ItemCategory/ItemType/Rack entities) - every other *Id-looking
 * column (BaseUnitId, PurchaseGLAC/SaleGLAC/COGSGLAC, ManufactureId,
 * BuyerSupplierId, UOMScheduleId*, MasterItemId, ItemClassId, ...) is kept as a
 * plain Integer: the real table has no FK constraints on them, and their target
 * modules (Unit of Measure, GL accounts via the still-mismapped ChartofAccount
 * table, Item Class, Item Manufacture/Buyer sub-screens) aren't ported yet.
 * ProductType here is the desktop's own free-text column on Item (NOT a link to
 * this project's ProductType master table - the real schema doesn't join them).
 */
@Entity
@Table(name = "Item")
@Data
public class Item {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "ItemCode", length = 50)
	private String itemCode;

	@Column(name = "ItemName", length = 100)
	private String itemName;

	@Column(name = "BaseUnitId")
	private Integer baseUnitId;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "ItemCategoryId")
	private ItemCategory itemCategory;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "ItemTypeId")
	private ItemType itemType;

	@Column(name = "ItemClassId")
	private Integer itemClassId;

	@Column(name = "PurchaseGLAC")
	private Integer purchaseGLAC;

	@Column(name = "SaleGLAC")
	private Integer saleGLAC;

	@Column(name = "COGSGLAC")
	private Integer cOGSGLAC;

	@Column(name = "HSCode", length = 50)
	private String hSCode;

	@Column(name = "PackQty")
	private Double packQty;

	@Column(name = "ItemStatus")
	private Boolean itemStatus;

	@Column(name = "ManufactureId")
	private Integer manufactureId;

	@Column(name = "ManufacturePartNo", length = 50)
	private String manufacturePartNo;

	@Column(name = "BuyerSupplierId")
	private Integer buyerSupplierId;

	@Column(name = "BuyerPartNo", length = 50)
	private String buyerPartNo;

	@Column(name = "ProductNo", length = 50)
	private String productNo;

	@Column(name = "MinStockLevel")
	private Double minStockLevel;

	@Column(name = "MaxStockLevel")
	private Double maxStockLevel;

	@Column(name = "OptimalStockLevel")
	private Double optimalStockLevel;

	@Column(name = "ReorderLevel")
	private Double reorderLevel;

	@Column(name = "CostPrice")
	private Double costPrice;

	@Column(name = "UOMScheduleIdCostRate")
	private Integer uOMScheduleIdCostRate;

	@Column(name = "PurchasePrice")
	private Double purchasePrice;

	@Column(name = "UOMScheduleIdPurRate")
	private Integer uOMScheduleIdPurRate;

	@Column(name = "RetailPrice")
	private Double retailPrice;

	@Column(name = "UOMScheduleIdRetailRate")
	private Integer uOMScheduleIdRetailRate;

	@Column(name = "WholeSalePrice")
	private Double wholeSalePrice;

	@Column(name = "UOMScheduleIdWhsRate")
	private Integer uOMScheduleIdWhsRate;

	@Column(name = "AmountCalcType", length = 50)
	private String amountCalcType;

	@Column(name = "ProductType", length = 50)
	private String productType;

	@Column(name = "BaseCropYear", length = 50)
	private String baseCropYear;

	@Column(name = "ApplyGST")
	private Boolean applyGST;

	@Column(name = "ApplyVAT")
	private Boolean applyVAT;

	@Column(name = "ApplyExciese")
	private Boolean applyExciese;

	@Column(name = "OtherTax")
	private Boolean otherTax;

	@Column(name = "Pic1")
	private String pic1;

	@Column(name = "Pic2")
	private String pic2;

	@Column(name = "BarcodeImage")
	private String barcodeImage;

	@Column(name = "BarcodeNo", length = 500)
	private String barcodeNo;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "EntryUser")
	private Integer entryUser;

	@Column(name = "ModifyDate")
	private LocalDateTime modifyDate;

	@Column(name = "ModifyUser")
	private Integer modifyUser;

	@Column(name = "PostDate")
	private LocalDateTime postDate;

	@Column(name = "PostUser")
	private Integer postUser;

	@Column(name = "PostState")
	private Boolean postState;

	@Column(name = "OrganizationId")
	private Integer organizationId;

	@Column(name = "CompanyId")
	private Integer companyId;

	@Column(name = "BranchesId")
	private Integer branchesId;

	@Column(name = "ProjectsId")
	private Integer projectsId;

	@Column(name = "ItemOriginId")
	private Integer itemOriginId;

	@Column(name = "ItemAliasName", length = 50)
	private String itemAliasName;

	@Column(name = "UomLookUpId")
	private Integer uomLookUpId;

	@Column(name = "ItemSpecification", length = 500)
	private String itemSpecification;

	@Column(name = "MasterItemId")
	private Integer masterItemId;

	@Column(name = "PurchaseTypeLookUpId")
	private Integer purchaseTypeLookUpId;

	@Column(name = "IsExpirable")
	private Boolean isExpirable;

	@Column(name = "ShelfLife")
	private Integer shelfLife;

	@Column(name = "IsDiscountable")
	private Boolean isDiscountable;

	@Column(name = "SaleTaxPurPercent")
	private Double saleTaxPurPercent;

	@Column(name = "AtRetailPriceTax")
	private Boolean atRetailPriceTax;

	@Column(name = "AtPurcahsePriceTax")
	private Boolean atPurcahsePriceTax;

	@Column(name = "SaleTaxSalesPercent")
	private Double saleTaxSalesPercent;

	@Column(name = "IsTaxable")
	private Boolean isTaxable;

	@Column(name = "accumulatedDepreciationAcId")
	private Integer accumulatedDepreciationAcId;

	@Column(name = "DepreciationExpenseAcId")
	private Integer depreciationExpenseAcId;

	@Column(name = "CapitalWipAcId")
	private Integer capitalWipAcId;

	@Column(name = "CropYearId")
	private Integer cropYearId;

	@Column(name = "ItemCodeNew")
	private String itemCodeNew;

	@Column(name = "ItemQcGradeId")
	private Integer itemQcGradeId;

	@Column(name = "WeightKgs", precision = 18, scale = 2)
	private BigDecimal weightKgs;

	@Column(name = "MaxMeaurement", precision = 18, scale = 2)
	private BigDecimal maxMeaurement;

	@Column(name = "MaxMeaurementUnitId")
	private Integer maxMeaurementUnitId;

	@Column(name = "ModelName", length = 300)
	private String modelName;

	@Column(name = "ItemWithVarient")
	private Boolean itemWithVarient;

	@Column(name = "ItemNameOtherLingo")
	private String itemNameOtherLingo;

	@Column(name = "IsCompany")
	private Boolean isCompany;

	@Column(name = "IsThirdParty")
	private Boolean isThirdParty;

	@Column(name = "MinRate")
	private Double minRate;

	@Column(name = "MaxRate")
	private Double maxRate;

	@Column(name = "EffectedDate")
	private LocalDateTime effectedDate;

	@Column(name = "ReOrderQty")
	private Double reOrderQty;

	@Column(name = "IsImport")
	private Boolean isImport;

	@Column(name = "LeadTimeDay")
	private Integer leadTimeDay;

	@Column(name = "AllowMultiUom")
	private Boolean allowMultiUom;

	@Column(name = "PackSizeId")
	private Integer packSizeId;

	@Column(name = "PackSize")
	private Double packSize;

	@Column(name = "MotherItemId")
	private Integer motherItemId;

	@Column(name = "itemModalId")
	private Integer itemModalId;

	@Column(name = "WeightSemiFinish")
	private Double weightSemiFinish;

	@Column(name = "WeightFinishGoods")
	private Double weightFinishGoods;

	@Column(name = "WeightSemiFinishSand")
	private Double weightSemiFinishSand;

	@Column(name = "ExpenseMaintenanceAccountId")
	private Integer expenseMaintenanceAccountId;

	@Column(name = "EmptyBagWeight")
	private Double emptyBagWeight;

	@Column(name = "ServicesMasterItemId")
	private Integer servicesMasterItemId;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "RackId")
	private Rack rack;

}