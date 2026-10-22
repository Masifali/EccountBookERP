package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InventoryEBBalancesByPartyAndItem_Rpt (single branch; #Data r.* + Company/Organization columns) */
public interface InventoryEbBalancesByPartyAndItemRow {
    String getSupplier_Name();
    String getItem_Name();
    BigDecimal getOBalance();
    BigDecimal getReceived();
    BigDecimal getIssued();
    BigDecimal getCBalance();
    String getCompCountry();
    String getCompContactPerson();
    String getCompMobileA();
    String getCompMobileB();
    String getCompMobileC();
    String getCompEmailA();
    String getCompEmailB();
    byte[] getCompLogoImage();
    String getCompanyWebsite();
    String getCompanyFaxNo();
    String getOrgReportingRemarks();
}
