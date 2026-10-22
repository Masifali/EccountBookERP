package com.mst.projections;

import java.sql.Timestamp;

/** Row of USP_driverBiodata_GetAllMethod @Activity='ReadAll' (native). */
public interface DriverBiodataReadAllRow {
    Long getRecordNo();
    Integer getDriverBiodataId();
    Integer getDocumentTypeId();
    String getDriverName();
    String getCnicNo();
    String getCellNo();
    String getWhatsappNo();
    String getAlternateCellNo();
    String getFatherName();
    String getFatherCnicNo();
    String getDriverPicture();
    String getDriverFingerPrint();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getBranchId();
    Integer getEntryUserId();
    Timestamp getEntryDate();
    Integer getModifyUserId();
    Timestamp getModifyDate();
    Boolean getIsApproved();
    Integer getApprovedUserId();
    Timestamp getApprovedDate();
    Integer getActionId();
    Integer getRevisionNo();
    byte[] getRowVersion();
    String getAttachmentsValues();
    String getCustomAttachmentsValues();
    String getEntryUserName();
    String getModifyUserName();
    String getApprovalUserName();
}
