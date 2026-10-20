package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_SupplierCustomer_GetAllMethod @Activity='ContractorHistory' (native). */
public interface ContractorHistoryRow {
    Integer getId();
    String getCompanyName();
    String getAddress1();
    String getMobilePersonal();
    String getTitle();
    String getCNIC();
    String getEmail();
    String getNTN_No();
    Boolean getStatus();
    Integer getEntryUser();
    Timestamp getEntryDate();
    String getEntryUserName();
    Integer getModifyUser();
    Timestamp getModifyDate();
    String getModifyUserName();
    String getPictureURL();
    String getEntryUserProfileImageName();
    String getModifyUserProfileImageName();
}
