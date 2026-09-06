package com.mst.models;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real GoldenAcedb dbo.ItemCategory table (desktop:
 * Inventory_Definition/InvDeffrmItemCatagory.cs). This is the real, top-level Item
 * Category - it replaces the earlier "ItemCategory / ItemSubCategory / ItemDef"
 * 3-level guess built before goldenAce5_25t.sql was read; the real desktop app has
 * no such hierarchy - just ItemCategory -> ItemType -> Item (see ItemType.java,
 * Item.java).
 *
 * The Revenue/CGS/Inventory GL account columns and the Asset/Depreciation columns
 * (accumulatedDepreciationAcId, DepreciationExpenseAcId, CapitalWipAcId,
 * AssetGlAcId, AssetCategoryId, usefullLifeInMonths, depriciationrate,
 * depreciationMethodScheduleId, ItemProductionStageId, ItemVarietyNatureId,
 * ItemClassGroupId) are kept as plain Ids (not JPA relations) - the real table has
 * no FK constraints on them either, and most are Fixed-Asset-module concerns not
 * yet ported.
 */
@Entity
@Table(name = "ItemCategory")
@Data
public class ItemCategory {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "CategoryCode", length = 20)
	private String categoryCode;

	@Column(name = "CategoryDescription", length = 100)
	private String categoryDescription;

	@Column(name = "SerialFrom")
	private Integer serialFrom;

	@Column(name = "SerialTo")
	private Integer serialTo;

	@Column(name = "CategoryStatus")
	private Boolean categoryStatus = true;

	/** Revenue GL account (ChartofAccount.Id) this category's sales post to. */
	@Column(name = "RevenueAccountId")
	private Integer revenueAccountId;

	/** Cost-of-goods-sold GL account (ChartofAccount.Id). */
	@Column(name = "CGSAccountId")
	private Integer cgsAccountId;

	/** Inventory (stock asset) GL account (ChartofAccount.Id). */
	@Column(name = "InventoryAccountId")
	private Integer inventoryAccountId;

	@Column(name = "accumulatedDepreciationAcId")
	private Integer accumulatedDepreciationAcId;

	@Column(name = "DepreciationExpenseAcId")
	private Integer depreciationExpenseAcId;

	@Column(name = "CapitalWipAcId")
	private Integer capitalWipAcId;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "EntryUser")
	private Integer entryUser;

	@Column(name = "ModifyDate")
	private LocalDateTime modifyDate;

	@Column(name = "ModifyUser")
	private Integer modifyUser;

	@Column(name = "OrganizationId")
	private Integer organizationId;

	@Column(name = "CompanyId")
	private Integer companyId;

	/** Self-referencing parent category Id (0/null = top level). */
	@Column(name = "InventoryParentCategoriesId")
	private Integer inventoryParentCategoriesId;

	@Column(name = "AssetGlAcId")
	private Integer assetGlAcId;

	@Column(name = "ItemClassGroupId")
	private Integer itemClassGroupId;

	@Column(name = "ItemProductionStageId")
	private Integer itemProductionStageId;

	@Column(name = "ItemVarietyNatureId")
	private Integer itemVarietyNatureId;

	@Column(name = "ExpenseMaintenanceAccountId")
	private Integer expenseMaintenanceAccountId;

	@Column(name = "usefullLifeInMonths")
	private Integer usefullLifeInMonths;

	@Column(name = "depriciationrate")
	private BigDecimal depriciationrate;

	@Column(name = "depreciationMethodScheduleId")
	private Integer depreciationMethodScheduleId;

	@Column(name = "AssetCategoryId")
	private Integer assetCategoryId;
}
