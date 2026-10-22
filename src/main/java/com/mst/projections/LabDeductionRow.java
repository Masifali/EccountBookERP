package com.mst.projections;

/** Row (native) of InvPurchaseInvoiceDetailRepository.readLabDeduction: InvLabAnalysisPurchaseHeader deduction info (Sp_InvPurchaseInvoiceDetail_Insert rate-cut compulsory check). */
public interface LabDeductionRow {
    Double getDeductionRate();
    Double getDeductionWeight();
    Boolean getRateCutCompulsory();
    Boolean getWeightCutCompulsory();
    String getDocNo();
}
