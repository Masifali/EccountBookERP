package com.mst.projections;

import java.sql.Timestamp;

/**
 * Row of USp_InvGrnStore_Register (native; #TmpResult / #tmpDms / #Data temp-table pipeline rewritten as one
 * statement). The procedure emits BaseDocumentTypeId twice; the duplicate alias is dropped (Hibernate rejects
 * duplicated SQL aliases). DetailId is r.Id in the procedure's #Data step (kept as-is).
 */
public interface InvGrnStoreRegisterRow {
    Integer getId();
    Integer getDetailId();
    Integer getDocumentTypeId();
    Timestamp getDocDate();
    Integer getDocNo();
    Integer getInvoiceNo();
    Integer getSuppliercustomerId();
    String getCompanyName();
    String getSupplierReference();
    Timestamp getGpDate();
    Integer getGpNo();
    String getRemarksHeader();
    String getVehicleNo();
    String getVehicleType();
    String getBiltyNo();
    Integer getInwardGatePassId();
    String getReferenceDocNo();
    Integer getWagesId();
    Integer getWagesNo();
    Integer getOrderId();
    Timestamp getPoDate();
    Integer getPoNo();
    String getWareHouseName();
    Integer getItemId();
    String getItemName();
    String getCropYear();
    String getPackTypeDesc();
    String getJobLotDescription();
    Double getItemQty();
    String getUOMCode();
    Double getGrossWeight();
    Double getEBWPerUnit();
    Double getEBWTotal();
    Double getWtCut();
    Double getWtCutTotal();
    Double getAdLsWeight();
    Double getNetBillWeight();
    String getDeliveryTerm();
    Double getStockWeight();
    Double getFactoryWeight();
    Double getPartyWeight();
    String getLabReportRef();
    String getAreaCity();
    Double getFreightAmount();
    Integer getTransporterId();
    String getTransporterName();
    String getCommentsDetail();
    Timestamp getEntryDate();
    String getUserNameEusr();
    Timestamp getModifyDate();
    String getUserNameMusr();
    Integer getNoOfAttachments();
    Integer getBaseDocumentTypeId();
    String getBranchName();
    String getDocumentTypeDescription();
    String getBaseDocumentType();
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
