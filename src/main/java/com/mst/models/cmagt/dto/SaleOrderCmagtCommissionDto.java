package com.mst.models.cmagt.dto;

import com.fasterxml.jackson.annotation.JsonAlias;

import lombok.Data;

import java.math.BigDecimal;

/**
 * Mirrors Architecture.Model.CommissionAgent.saleOrderCommissionDetail.
 * Non-virtual properties become the parameters of
 * [cmagt].[USP_saleOrderCommissionDetail_Insert].
 *
 * agentTypeId is the desktop's own discriminator, assigned literally in
 * frmSaleOrderCmagt.Insert(): 1 = Sub_Commission Agent, 2 = Sub_Brokery Agent.
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

public class SaleOrderCmagtCommissionDto {

    private BigDecimal commissionAmount = BigDecimal.ZERO;
    private BigDecimal commissionRate = BigDecimal.ZERO;
    private Integer agentTypeId = 0;
    private Integer commissionAgentId = 0;
    private Integer commissionTypeId = 0;
    private Integer rateUomId = 0;
    private Integer saleOrderCommissionDetailId = 0;
    private Integer saleOrderMasterId = 0;
    private Integer sortNo = 0;
    private String commissionRemarks = "";

    /** virtual on the desktop model - display only */
    @JsonAlias({"CommissionAgentName"})
    private String CommissionAgentName;
    @JsonAlias({"AgentType"})
    private String AgentType;
    @JsonAlias({"CommissionType"})
    private String CommissionType;
    @JsonAlias({"RateUom"})
    private Double RateUom;
}
