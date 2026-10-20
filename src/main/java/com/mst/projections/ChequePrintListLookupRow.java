package com.mst.projections;

/** Row of the existing-cheque lookup in Sp_VoucherDetail_Insert (SELECT Id, PrintStatus FROM ChequePrintList WHERE ...). JPQL projection. */
public interface ChequePrintListLookupRow {
    Integer getId();
    Boolean getPrintStatus();
}
