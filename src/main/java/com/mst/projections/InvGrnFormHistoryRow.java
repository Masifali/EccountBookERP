package com.mst.projections;

import java.sql.Timestamp;

/**
 * Row of Sp_InvGRN_GetAllMethod @Activity='GRNFormHistory' (native; #GrnHis temp-table pipeline rewritten as a CTE
 * with OUTER APPLY sub-selects for attachments / wb ticket / invoice / wages).
 */
public interface InvGrnFormHistoryRow {
    Long getRecordNo();
    Integer getId();
    Integer getOrderId();
    String getOrderType();
    Integer getOrderNo();
    Timestamp getDocDate();
    Integer getInwardGatePassId();
    Integer getReferencePartyId();
    String getScreenName();
    Integer getDocNo();
    Integer getDocumentTypeId();
    String getDeliveryTerm();
    String getSupplierName();
    Integer getGpNo();
    String getVehicleNo();
    String getBiltyNo();
    Integer getWagesId();
    Integer getWagesNo();
    Boolean getAddWages();
    Double getFactoryWeight();
    Double getPartyWeight();
    String getTransporter();
    Double getCarriageAmount();
    String getRemarksHeader();
    Timestamp getEntryDate();
    String getEntryUser();
    Timestamp getModifyDate();
    String getModifyUser();
    Timestamp getPostDate();
    String getApprovedUser();
    String getRequestedBy();
    Integer getWbTicketId();
    String getTicketNo();
    Integer getInvoiceNo();
    Integer getNoOfAttachments();
    Integer getAttachmentsCount();
    Integer getDetailIdsCount();
    Boolean getScaleShortWeightApply();
    Boolean getSupplierShortWeightApply();
    Double getRateCut();
    String getRateCutRemarks();
    Double getAccessWeight();
    Integer getRefDocumentTypeId();
    Integer getBaseDocumentTypeId();
    String getBaseDocumentType();
}
