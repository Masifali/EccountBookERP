package com.mst.models.cmagt.dto;

import com.fasterxml.jackson.annotation.JsonAlias;

import lombok.Data;

import java.math.BigDecimal;

/**
 * Mirrors Architecture.Model.CommissionAgent.saleOrderEmptyBagDetail.
 * Non-virtual properties become the parameters of
 * [cmagt].[USP_saleOrderEmptyBagDetail_Insert]. PackingType is virtual (display only).
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

public class SaleOrderCmagtEmptyBagDto {

    @JsonAlias({"Rate"})

    private BigDecimal Rate = BigDecimal.ZERO;
    private BigDecimal weightCutKg = BigDecimal.ZERO;
    @JsonAlias({"PackingTypeId"})
    private Integer PackingTypeId = 0;
    private Integer purchaseOrderEmptyBagDetailId = 0;
    private Integer purchaseOrderMasterId = 0;
    private Integer saleOrderEmptyBagDetailId = 0;
    private Integer saleOrderMasterId = 0;
    private Integer sortNo = 0;
    private String remarks = "";

    /** virtual on the desktop model - display only */
    @JsonAlias({"PackingType"})
    private String PackingType;
}
