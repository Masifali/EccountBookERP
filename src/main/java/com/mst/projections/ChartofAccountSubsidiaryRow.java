package com.mst.projections;

/** Row of "SELECT ISNULL(SubSidiaryAcStatus,0), AccountTitle FROM ChartofAccount WHERE Id = @AccountId" in Sp_VoucherDetail_Insert. Native projection. */
public interface ChartofAccountSubsidiaryRow {
    Integer getSubSidiaryAcStatus();
    String getAccountTitle();
}
