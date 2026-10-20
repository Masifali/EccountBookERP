package com.mst.projections;

/** Row of Sp_InvGRN_GetAllMethod @Activity='GetReceivedWeightAndBalanceWeight' (native). */
public interface InvGrnReceivedBalanceWeightRow {
    Double getSupplierWeight();
    Double getFactoryWeight();
    Double getReceivedWeight();
    Double getBalanceWeight();
    String getDeliverTerm();
}
