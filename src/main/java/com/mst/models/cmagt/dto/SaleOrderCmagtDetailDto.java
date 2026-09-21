package com.mst.models.cmagt.dto;

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
public class SaleOrderCmagtDetailDto {

    // ---- persisted (non-virtual) -> procedure parameters -------------------
    private BigDecimal fcyAmount = BigDecimal.ZERO;
    private BigDecimal ItemAmount = BigDecimal.ZERO;
    private BigDecimal itemQty = BigDecimal.ZERO;
    private BigDecimal itemRate = BigDecimal.ZERO;
    private BigDecimal itemWeight = BigDecimal.ZERO;
    private BigDecimal TaxAmount = BigDecimal.ZERO;
    private BigDecimal TaxPercent = BigDecimal.ZERO;
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
    private Integer TaxNameId = 0;

    private String cropYear = "";
    private String qualitySpecification = "";
    private String remarks = "";

    // ---- virtual (display only) - never sent to the procedure --------------
    private String inventoryParentCategory;
    private String ItemName;
    private String ItemCode;
    private String PackingType;
    private String PackUomCode;
    private Double PackUomEquivalent;
    private String RateUomCode;
    private Double RateUomEquivalent;
    private String TaxName;
}
