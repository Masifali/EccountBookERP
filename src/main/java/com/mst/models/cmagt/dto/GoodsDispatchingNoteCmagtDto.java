package com.mst.models.cmagt.dto;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

public class GoodsDispatchingNoteCmagtDto {

    private Integer gdnBuyerDispatchMasterId;
    private Integer docNo;
    private String docDate;
    private Integer saleOrderMasterId;
    private Integer buyerId;
    private String buyerName;
    private Integer commissionAgentId;
    private String commissionAgentName;
    private String vehicleNo;
    private String biltyNo;
    private String driverName;
    private String driverMobileNo;
    private String remarksHeader;

    private Integer organizationId;
    private Integer companyId;
    private Integer branchId;
    private Integer financialYearId;
    private Integer entryUserId;
    private Integer modifyUserId;
    private Integer documentTypeId = 1055;


    /* ------------------------------------------------------------------------------------
     * The rest of the header the desktop saves - btnSave_Click, frmGoodsDispatchingNoteCmagt
     * :1877-1917. Every one of these was absent, so USP_gdnBuyerDispatchMaster_InsertAndUpdate
     * was called without them and stored its own defaults: no transporter, no cities, no
     * delivery term, no vehicle type and every freight and weight figure zero, on a document
     * that looked saved.
     * ---------------------------------------------------------------------------------- */
    private String buyerRefDocNo;
    private Integer deliverToPartyId;
    private String deliverToPartyName;
    private Integer deliverToAddressId;
    private String deliverToAddress;
    private Integer loadingCityId;
    private Integer unloadingCityId;
    private Integer transporterId;
    private String transporterName;
    private BigDecimal biltyFreight;
    private BigDecimal otherAdLesCharges;
    private BigDecimal totalFreight;
    private String freightRemarks;
    private Integer vehicleTypeId;
    private String biltyDate;
    private Integer deliveryTermId;
    private BigDecimal biltyQty;
    private BigDecimal loadWeight;
    private BigDecimal tareWeight;
    private BigDecimal scaleNetWeight;
    private BigDecimal billWeight;
    private String warningRemarks;

    private List<DetailDto> gdnBuyerDispatchDetailList = new ArrayList<>();

    public static class DetailDto {
        private Integer gdnBuyerDispatchDetailId;
        private Integer gdnBuyerDispatchMasterId;
        private Integer saleOrderDetailId;
        private Integer itemId;
        private String itemName;
        private Integer cropYearId;
        private Integer packTypeId;
        private BigDecimal weight;
        private BigDecimal qty;
        private BigDecimal rate;
        private BigDecimal amount;
        private String remarksDetail;

        public Integer getGdnBuyerDispatchDetailId() { return gdnBuyerDispatchDetailId; }
        public void setGdnBuyerDispatchDetailId(Integer gdnBuyerDispatchDetailId) { this.gdnBuyerDispatchDetailId = gdnBuyerDispatchDetailId; }
        public Integer getGdnBuyerDispatchMasterId() { return gdnBuyerDispatchMasterId; }
        public void setGdnBuyerDispatchMasterId(Integer gdnBuyerDispatchMasterId) { this.gdnBuyerDispatchMasterId = gdnBuyerDispatchMasterId; }
        public Integer getSaleOrderDetailId() { return saleOrderDetailId; }
        public void setSaleOrderDetailId(Integer saleOrderDetailId) { this.saleOrderDetailId = saleOrderDetailId; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer itemId) { this.itemId = itemId; }
        public String getItemName() { return itemName; }
        public void setItemName(String itemName) { this.itemName = itemName; }
        public Integer getCropYearId() { return cropYearId; }
        public void setCropYearId(Integer cropYearId) { this.cropYearId = cropYearId; }
        public Integer getPackTypeId() { return packTypeId; }
        public void setPackTypeId(Integer packTypeId) { this.packTypeId = packTypeId; }
        public BigDecimal getWeight() { return weight; }
        public void setWeight(BigDecimal weight) { this.weight = weight; }
        public BigDecimal getQty() { return qty; }
        public void setQty(BigDecimal qty) { this.qty = qty; }
        public BigDecimal getRate() { return rate; }
        public void setRate(BigDecimal rate) { this.rate = rate; }
        public BigDecimal getAmount() { return amount; }
        public void setAmount(BigDecimal amount) { this.amount = amount; }
        public String getRemarksDetail() { return remarksDetail; }
        public void setRemarksDetail(String remarksDetail) { this.remarksDetail = remarksDetail; }
    }

    // Getters and Setters
    public Integer getGdnBuyerDispatchMasterId() { return gdnBuyerDispatchMasterId; }
    public void setGdnBuyerDispatchMasterId(Integer gdnBuyerDispatchMasterId) { this.gdnBuyerDispatchMasterId = gdnBuyerDispatchMasterId; }
    public Integer getDocNo() { return docNo; }
    public void setDocNo(Integer docNo) { this.docNo = docNo; }
    public String getDocDate() { return docDate; }
    public void setDocDate(String docDate) { this.docDate = docDate; }
    public Integer getSaleOrderMasterId() { return saleOrderMasterId; }
    public void setSaleOrderMasterId(Integer saleOrderMasterId) { this.saleOrderMasterId = saleOrderMasterId; }
    public Integer getBuyerId() { return buyerId; }
    public void setBuyerId(Integer buyerId) { this.buyerId = buyerId; }
    public String getBuyerName() { return buyerName; }
    public void setBuyerName(String buyerName) { this.buyerName = buyerName; }
    public Integer getCommissionAgentId() { return commissionAgentId; }
    public void setCommissionAgentId(Integer commissionAgentId) { this.commissionAgentId = commissionAgentId; }
    public String getCommissionAgentName() { return commissionAgentName; }
    public void setCommissionAgentName(String commissionAgentName) { this.commissionAgentName = commissionAgentName; }
    public String getVehicleNo() { return vehicleNo; }
    public void setVehicleNo(String vehicleNo) { this.vehicleNo = vehicleNo; }
    public String getBiltyNo() { return biltyNo; }
    public void setBiltyNo(String biltyNo) { this.biltyNo = biltyNo; }
    public String getDriverName() { return driverName; }
    public void setDriverName(String driverName) { this.driverName = driverName; }
    public String getDriverMobileNo() { return driverMobileNo; }
    public void setDriverMobileNo(String driverMobileNo) { this.driverMobileNo = driverMobileNo; }
    public String getRemarksHeader() { return remarksHeader; }
    public void setRemarksHeader(String remarksHeader) { this.remarksHeader = remarksHeader; }

    public Integer getOrganizationId() { return organizationId; }
    public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
    public Integer getCompanyId() { return companyId; }
    public void setCompanyId(Integer companyId) { this.companyId = companyId; }
    public Integer getBranchId() { return branchId; }
    public void setBranchId(Integer branchId) { this.branchId = branchId; }
    public Integer getFinancialYearId() { return financialYearId; }
    public void setFinancialYearId(Integer financialYearId) { this.financialYearId = financialYearId; }
    public Integer getEntryUserId() { return entryUserId; }
    public void setEntryUserId(Integer entryUserId) { this.entryUserId = entryUserId; }
    public Integer getModifyUserId() { return modifyUserId; }
    public void setModifyUserId(Integer modifyUserId) { this.modifyUserId = modifyUserId; }
    public Integer getDocumentTypeId() { return documentTypeId; }
    public void setDocumentTypeId(Integer documentTypeId) { this.documentTypeId = documentTypeId; }

    public List<DetailDto> getGdnBuyerDispatchDetailList() { return gdnBuyerDispatchDetailList; }
    public void setGdnBuyerDispatchDetailList(List<DetailDto> gdnBuyerDispatchDetailList) { this.gdnBuyerDispatchDetailList = gdnBuyerDispatchDetailList; }


    public String getBuyerRefDocNo() { return buyerRefDocNo; }
    public void setBuyerRefDocNo(String buyerRefDocNo) { this.buyerRefDocNo = buyerRefDocNo; }
    public Integer getDeliverToPartyId() { return deliverToPartyId; }
    public void setDeliverToPartyId(Integer deliverToPartyId) { this.deliverToPartyId = deliverToPartyId; }
    public String getDeliverToPartyName() { return deliverToPartyName; }
    public void setDeliverToPartyName(String deliverToPartyName) { this.deliverToPartyName = deliverToPartyName; }
    public Integer getDeliverToAddressId() { return deliverToAddressId; }
    public void setDeliverToAddressId(Integer deliverToAddressId) { this.deliverToAddressId = deliverToAddressId; }
    public String getDeliverToAddress() { return deliverToAddress; }
    public void setDeliverToAddress(String deliverToAddress) { this.deliverToAddress = deliverToAddress; }
    public Integer getLoadingCityId() { return loadingCityId; }
    public void setLoadingCityId(Integer loadingCityId) { this.loadingCityId = loadingCityId; }
    public Integer getUnloadingCityId() { return unloadingCityId; }
    public void setUnloadingCityId(Integer unloadingCityId) { this.unloadingCityId = unloadingCityId; }
    public Integer getTransporterId() { return transporterId; }
    public void setTransporterId(Integer transporterId) { this.transporterId = transporterId; }
    public String getTransporterName() { return transporterName; }
    public void setTransporterName(String transporterName) { this.transporterName = transporterName; }
    public BigDecimal getBiltyFreight() { return biltyFreight; }
    public void setBiltyFreight(BigDecimal biltyFreight) { this.biltyFreight = biltyFreight; }
    public BigDecimal getOtherAdLesCharges() { return otherAdLesCharges; }
    public void setOtherAdLesCharges(BigDecimal otherAdLesCharges) { this.otherAdLesCharges = otherAdLesCharges; }
    public BigDecimal getTotalFreight() { return totalFreight; }
    public void setTotalFreight(BigDecimal totalFreight) { this.totalFreight = totalFreight; }
    public String getFreightRemarks() { return freightRemarks; }
    public void setFreightRemarks(String freightRemarks) { this.freightRemarks = freightRemarks; }
    public Integer getVehicleTypeId() { return vehicleTypeId; }
    public void setVehicleTypeId(Integer vehicleTypeId) { this.vehicleTypeId = vehicleTypeId; }
    public String getBiltyDate() { return biltyDate; }
    public void setBiltyDate(String biltyDate) { this.biltyDate = biltyDate; }
    public Integer getDeliveryTermId() { return deliveryTermId; }
    public void setDeliveryTermId(Integer deliveryTermId) { this.deliveryTermId = deliveryTermId; }
    public BigDecimal getBiltyQty() { return biltyQty; }
    public void setBiltyQty(BigDecimal biltyQty) { this.biltyQty = biltyQty; }
    public BigDecimal getLoadWeight() { return loadWeight; }
    public void setLoadWeight(BigDecimal loadWeight) { this.loadWeight = loadWeight; }
    public BigDecimal getTareWeight() { return tareWeight; }
    public void setTareWeight(BigDecimal tareWeight) { this.tareWeight = tareWeight; }
    public BigDecimal getScaleNetWeight() { return scaleNetWeight; }
    public void setScaleNetWeight(BigDecimal scaleNetWeight) { this.scaleNetWeight = scaleNetWeight; }
    public BigDecimal getBillWeight() { return billWeight; }
    public void setBillWeight(BigDecimal billWeight) { this.billWeight = billWeight; }
    public String getWarningRemarks() { return warningRemarks; }
    public void setWarningRemarks(String warningRemarks) { this.warningRemarks = warningRemarks; }
}
