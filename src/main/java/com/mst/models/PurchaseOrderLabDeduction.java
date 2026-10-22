package com.mst.models;

import java.io.Serializable;
import java.math.BigDecimal;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped 1:1 onto dbo.PurchaseOrderLabDeduction (source: Sp_PurchaseOrderLabDeduction_Insert,
 * Sp_PurchaseOrder_GetAllMethod 'ReadPurchaseOrderLabDeductionByHeaderId'). Id is app-assigned: ISNULL(MAX(Id),0)+1.
 */
@Entity
@Setter
@Getter
@Table(name = "PurchaseOrderLabDeduction")
public class PurchaseOrderLabDeduction implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "InvLabAnalysisStandardDeductionPolicyHeaderId")
    private Integer invLabAnalysisStandardDeductionPolicyHeaderId;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;

    @Column(name = "PurchaseOrderDetailId")
    private Integer purchaseOrderDetailId;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "AnalysisParameterId")
    private Integer analysisParameterId;

    @Column(name = "RangeFrom")
    private BigDecimal rangeFrom;

    @Column(name = "RangeTo")
    private BigDecimal rangeTo;

    @Column(name = "DeductFrom")
    private String deductFrom;

    @Column(name = "StandardValue")
    private BigDecimal standardValue;

    @Column(name = "DeductionValue")
    private BigDecimal deductionValue;

    @Column(name = "WeightKgs")
    private BigDecimal weightKgs;
}
