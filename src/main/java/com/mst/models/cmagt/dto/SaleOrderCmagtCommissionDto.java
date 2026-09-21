package com.mst.models.cmagt.dto;

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
    private String CommissionAgentName;
    private String AgentType;
    private String CommissionType;
    private Double RateUom;
}
