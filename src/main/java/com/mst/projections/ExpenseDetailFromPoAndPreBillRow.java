package com.mst.projections;

import java.math.BigDecimal;

/** Row of USP_GetExpenseDetailFromPOAndPreBill (native, @Result table variable rewritten as UNION ALL). */
public interface ExpenseDetailFromPoAndPreBillRow {
    Integer getSupplierDispatchId();
    Integer getSupplierDispatchExpenseId();
    Integer getOtherItemId();
    BigDecimal getQty();
    BigDecimal getRate();
    BigDecimal getAmount();
    String getRemarks();
    Integer getGrnId();
    Integer getGrnNo();
    Integer getPoId();
    Integer getPoNo();
}
