package com.mst.models.cmagt.dto;

import com.fasterxml.jackson.annotation.JsonAlias;

import lombok.Data;

import java.math.BigDecimal;

/**
 * Mirrors Architecture.Model.CommissionAgent.saleOrderDetail exactly.
 *
 * Every NON-VIRTUAL property of that class becomes an "@PropertyName" parameter of
 * [cmagt].[USP_saleOrderDetail_Insert] - this is not a guess, it is the documented
 * behaviour of Architecture.DAL.Common.GenericProvider.SetProc, which reflects over
 * typeof(T).GetProperties() and calls
 *     sqlCommand.Parameters.AddWithValue("@" + propertyInfo.Name, value)
 * for every property whose getter is not virtual. The virtual properties
 * (inventoryParentCategory, ItemName, ItemCode, PackingType, PackUomCode,
 * PackUomEquivalent, RateUomCode, RateUomEquivalent, TaxName) are display-only and are
 * deliberately NOT sent to the procedure - they are kept here purely so the grid can be
 * re-rendered, exactly as the desktop DataTable does.
 *
 * Field names below intentionally keep the desktop's own casing (itemQty, ItemAmount,
 * TaxPercent, ...) so the mapping to procedure parameters stays literal and auditable.
 */
@Data
/*
 * JSON BINDING NOTE
 * -----------------
 * Several fields below carry the desktop model's own spelling, which begins with a capital
 * letter (ItemAmount, TaxNameId, ShipToAddress, EBWeightDeductionTermId, ...). Jackson derives
 * a property name from the ACCESSOR, not the field, so `getItemAmount()` publishes the property
 * as "itemAmount" and a payload that posts "ItemAmount" - the name the desktop uses - does not
 * bind. Spring Boot leaves FAIL_ON_UNKNOWN_PROPERTIES off, so such a key is dropped in silence:
 * no error, no log line, just a missing value and a save that reports success.
 *
 * Every such field therefore carries @JsonAlias with the desktop's spelling, so both forms bind.
 * Aliases only ADD accepted spellings; nothing that worked before changes.
 */

public class SaleOrderCmagtDetailDto {

    // ---- persisted (non-virtual) -> procedure parameters -------------------
    private BigDecimal fcyAmount = BigDecimal.ZERO;
    @JsonAlias({"ItemAmount"})
    private BigDecimal ItemAmount = BigDecimal.ZERO;
    private BigDecimal itemQty = BigDecimal.ZERO;
    private BigDecimal itemRate = BigDecimal.ZERO;
    private BigDecimal itemWeight = BigDecimal.ZERO;
    @JsonAlias({"TaxAmount"})
    private BigDecimal TaxAmount = BigDecimal.ZERO;
    @JsonAlias({"TaxPercent"})
    private BigDecimal TaxPercent = BigDecimal.ZERO;
    @JsonAlias({"TotalAmount"})
    private BigDecimal TotalAmount = BigDecimal.ZERO;

    /** 1 = insert, 2 = update, 3 = soft delete. Assigned exactly as the desktop does. */
    private Integer actionTypeId = 1;

    private Integer cropYearId = 0;
    private Integer inquiryBookingDetailId = 0;
    private Integer inquiryBookingMasterId = 0;
    private Integer inventoryParentCategoryId = 0;
    private Integer itemId = 0;
    private Integer packingTypeId = 0;
    private Integer packUomId = 0;
    private Integer purchaseOrderDetailId = 0;
    private Integer purchaseOrderMasterId = 0;
    private Integer rateUomId = 0;
    private Integer saleOrderDetailId = 0;
    private Integer saleOrderMasterId = 0;
    private Integer sortNo = 0;
    private Integer supplierOfferDetailId = 0;
    private Integer supplierOfferId = 0;
    @JsonAlias({"TaxNameId"})
    private Integer TaxNameId = 0;

    private String cropYear = "";
    private String qualitySpecification = "";
    private String remarks = "";

    // ---- virtual (display only) - never sent to the procedure --------------
    private String inventoryParentCategory;
    @JsonAlias({"ItemName"})
    private String ItemName;
    @JsonAlias({"ItemCode"})
    private String ItemCode;
    @JsonAlias({"PackingType"})
    private String PackingType;
    @JsonAlias({"PackUomCode"})
    private String PackUomCode;
    @JsonAlias({"PackUomEquivalent"})
    private Double PackUomEquivalent;
    @JsonAlias({"RateUomCode"})
    private String RateUomCode;
    @JsonAlias({"RateUomEquivalent"})
    private Double RateUomEquivalent;
    @JsonAlias({"TaxName"})
    private String TaxName;
}
