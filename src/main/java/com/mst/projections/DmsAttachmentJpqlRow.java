package com.mst.projections;

import java.time.LocalDateTime;

/** Row of Sp_DMSAttachments_GetAllMethod @Activity='ReadAttachmentsbyRefDocumentTypeIdAndAccountId' (JPQL - same columns as DmsAttachmentRow, LocalDateTime dates). */
public interface DmsAttachmentJpqlRow {
    Integer getId();
    Integer getRefAccountId();
    Integer getDmsFoldersLabelsId();
    Integer getRefDocumentTypeId();
    Integer getRefDocumentNo();
    String getAttachment();
    LocalDateTime getEntryDate();
    Integer getEntryUser();
    LocalDateTime getModifyDate();
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
