package com.mst.models.cmagt.dto;

import com.fasterxml.jackson.annotation.JsonAlias;

import lombok.Data;

import java.math.BigDecimal;

/**
 * Mirrors Architecture.Model.CommissionAgent.saleOrderbuyerExpenseDetail.
 * Non-virtual properties become the parameters of
 * [cmagt].[USP_saleOrderBuyerExpenseDetail_Insert] (GenericProvider.SetProc convention).
 * OtherItemName is virtual on the desktop model and is therefore display-only.
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

public class SaleOrderCmagtExpenseDto {

    @JsonAlias({"Qty"})

    private BigDecimal Qty = BigDecimal.ZERO;
    private Double amount = 0d;
    private Double rate = 0d;
    @JsonAlias({"ItemId"})
    private Integer ItemId = 0;
    private Integer purchaseOrderMasterId = 0;
    private Integer purchaseOrderSupplierExpenseDetailId = 0;
    private Integer saleOrderbuyerExpenseDetailId = 0;
    private Integer saleOrderMasterId = 0;
    private Integer sortNo = 0;
    private String remarks = "";

    /** virtual on the desktop model - display only, never a procedure parameter */
    @JsonAlias({"OtherItemName"})
    private String OtherItemName;
}
