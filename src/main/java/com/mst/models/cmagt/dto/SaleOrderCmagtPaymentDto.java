package com.mst.models.cmagt.dto;

import com.fasterxml.jackson.annotation.JsonAlias;

import lombok.Data;

import java.math.BigDecimal;

/**
 * Mirrors Architecture.Model.CommissionAgent.saleOrderPaymentDetail.
 * Non-virtual properties become the parameters of
 * [cmagt].[USP_saleOrderPaymentDetail_Insert].
 *
 * DueDate is nullable on the desktop model (DateTime?) and is left null unless the
 * row's BaseDueDateTypeId is 1 (derived from DocDate + DueDays) or 4 (entered directly),
 * exactly as frmSaleOrderCmagt.Insert() computes it.
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

public class SaleOrderCmagtPaymentDto {

    @JsonAlias({"DueDate"})

    private String DueDate;                 // ISO yyyy-MM-dd, or null
    private BigDecimal dueAmount = BigDecimal.ZERO;
    private BigDecimal pctOfTotal = BigDecimal.ZERO;
    @JsonAlias({"BaseDueDateTypeId"})
    private Integer BaseDueDateTypeId = 0;
    @JsonAlias({"DueDays"})
    private Integer DueDays = 0;
    @JsonAlias({"PaymentTermId"})
    private Integer PaymentTermId = 0;
    private Integer saleOrderMasterId = 0;
    private Integer saleOrderPaymentDetailId = 0;
    private Integer sortNo = 0;
    private String remarks = "";

    /** virtual on the desktop model - display only */
    @JsonAlias({"PaymentTerm"})
    private String PaymentTerm;
    @JsonAlias({"BaseDateType"})
    private String BaseDateType;
}
