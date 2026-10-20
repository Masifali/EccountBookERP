package com.mst.projections;

import java.math.BigDecimal;

/** Row of GrnSpecialApprovalRepository.grnWeightValidationSummary (JPQL; the aggregate of USP_GrnBillWeightAndStockWeightValidations) */
public interface GrnWeightValidationRow {
    BigDecimal getDiffWeight();
    BigDecimal getEbWtTotal();
    String getDeliveryTerm();
    Integer getInwardGatePassId();
    BigDecimal getFreightDeduction();
}
