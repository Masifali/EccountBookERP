package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_PurchaseOrder_GetAllMethod @Activity='ReadPurchaseOrderLabDeductionByHeaderId' (native). */
public interface PurchaseOrderLabDeductionRow {
    Integer getId();
    Integer getInvLabAnalysisStandardDeductionPolicyHeaderId();
    Integer getPurchaseOrderId();
    Integer getPurchaseOrderDetailId();
    Integer getItemId();
    Integer getAnalysisParameterId();
    String getItemName();
    BigDecimal getRangeFrom();
    BigDecimal getRangeTo();
    String getDeductFrom();
    BigDecimal getStandardValue();
    BigDecimal getDeductionValue();
    BigDecimal getWeightKgs();
    String getAnalysisItem();
}
