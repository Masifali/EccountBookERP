package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_DMSAttachments_GetAllMethod @Activity='ReadById' / 'ReadAttachmentsbyRefDocumentTypeId' (native). */
public interface DmsAttachmentRow {
    Integer getId();
    Integer getRefAccountId();
    Integer getDMSFoldersLabelsId();
    Integer getRefDocumentTypeId();
    Integer getRefDocumentNo();
    String getAttachment();
    Timestamp getEntryDate();
    Integer getEntryUser();
    Timestamp getModifyDate();
    Integer getModifyUser();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getBranchId();
    String getScreenName();
    Boolean getDetailWiseAttachment();
    String getUploadedFileCustomName();
    Double getUploadedFileSizeMb();
    Integer getLineId();
    String getEntryUserName();
    String getFileCustomName();
    String getAttachmentList();
}
