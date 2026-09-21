package com.mst.models.cmagt.dto;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

public class BuyerInquiryBookingDto {

    private Integer inquiryBookingMasterId = 0;
    private Boolean isApproved = true;
    private String approvedDate;
    private String deliveryStartDate;
    private String entryDate;
    private String expiryDate;
    private String inquiryBookingDate;
    private String modifyDate;
    private String validityDate;
    private Integer actionId = 1;
    private Integer analysisGroupId = 0;
    private Integer approvedUserId = 0;
    private Integer branchId = 0;
    private Integer buyerId = 0;
    private Integer commissionAgentId = 0;
    private Integer inquiryStatusId = 1;
    private Integer companyId = 0;
    private Integer deliveryDays = 0;
    private Integer deliveryTermId = 0;
    private Integer documentTypeId = 1050;
    private Integer entryUserId = 0;
    private Integer financialYearId = 0;
    private Integer inquiryBookingNo = 0;
    private Integer modifyUserId = 0;
    private Integer organizationId = 0;
    private Integer projectId = 0;
    private Integer validityDays = 0;
    private String shipToAddress;
    private Integer shipToAddressId = 0;
    private Integer deliveryToPartyId = 0;
    private String remarksHeader;
    private String approvalRemarks;
    private String attachmentsValues;
    private String customAttachmentsValues;
    private String qualitySpecifications;

    private List<BuyerInquiryDetailDto> inquiryBookingDetailList = new ArrayList<>();
    private List<BuyerInquiryPartyDetailDto> inquiryBookingPartyDetailList = new ArrayList<>();

    public Integer getInquiryBookingMasterId() { return inquiryBookingMasterId; }
    public void setInquiryBookingMasterId(Integer inquiryBookingMasterId) { this.inquiryBookingMasterId = inquiryBookingMasterId; }

    public Boolean getIsApproved() { return isApproved; }
    public void setIsApproved(Boolean isApproved) { this.isApproved = isApproved; }

    public String getApprovedDate() { return approvedDate; }
    public void setApprovedDate(String approvedDate) { this.approvedDate = approvedDate; }

    public String getDeliveryStartDate() { return deliveryStartDate; }
    public void setDeliveryStartDate(String deliveryStartDate) { this.deliveryStartDate = deliveryStartDate; }

    public String getEntryDate() { return entryDate; }
    public void setEntryDate(String entryDate) { this.entryDate = entryDate; }

    public String getExpiryDate() { return expiryDate; }
    public void setExpiryDate(String expiryDate) { this.expiryDate = expiryDate; }

    public String getInquiryBookingDate() { return inquiryBookingDate; }
    public void setInquiryBookingDate(String inquiryBookingDate) { this.inquiryBookingDate = inquiryBookingDate; }

    public String getModifyDate() { return modifyDate; }
    public void setModifyDate(String modifyDate) { this.modifyDate = modifyDate; }

    public String getValidityDate() { return validityDate; }
    public void setValidityDate(String validityDate) { this.validityDate = validityDate; }

    public Integer getActionId() { return actionId; }
    public void setActionId(Integer actionId) { this.actionId = actionId; }

    public Integer getAnalysisGroupId() { return analysisGroupId; }
    public void setAnalysisGroupId(Integer analysisGroupId) { this.analysisGroupId = analysisGroupId; }

    public Integer getApprovedUserId() { return approvedUserId; }
    public void setApprovedUserId(Integer approvedUserId) { this.approvedUserId = approvedUserId; }

    public Integer getBranchId() { return branchId; }
    public void setBranchId(Integer branchId) { this.branchId = branchId; }

    public Integer getBuyerId() { return buyerId; }
    public void setBuyerId(Integer buyerId) { this.buyerId = buyerId; }

    public Integer getCommissionAgentId() { return commissionAgentId; }
    public void setCommissionAgentId(Integer commissionAgentId) { this.commissionAgentId = commissionAgentId; }

    public Integer getInquiryStatusId() { return inquiryStatusId; }
    public void setInquiryStatusId(Integer inquiryStatusId) { this.inquiryStatusId = inquiryStatusId; }

    public Integer getCompanyId() { return companyId; }
    public void setCompanyId(Integer companyId) { this.companyId = companyId; }

    public Integer getDeliveryDays() { return deliveryDays; }
    public void setDeliveryDays(Integer deliveryDays) { this.deliveryDays = deliveryDays; }

    public Integer getDeliveryTermId() { return deliveryTermId; }
    public void setDeliveryTermId(Integer deliveryTermId) { this.deliveryTermId = deliveryTermId; }

    public Integer getDocumentTypeId() { return documentTypeId; }
    public void setDocumentTypeId(Integer documentTypeId) { this.documentTypeId = documentTypeId; }

    public Integer getEntryUserId() { return entryUserId; }
    public void setEntryUserId(Integer entryUserId) { this.entryUserId = entryUserId; }

    public Integer getFinancialYearId() { return financialYearId; }
    public void setFinancialYearId(Integer financialYearId) { this.financialYearId = financialYearId; }

    public Integer getInquiryBookingNo() { return inquiryBookingNo; }
    public void setInquiryBookingNo(Integer inquiryBookingNo) { this.inquiryBookingNo = inquiryBookingNo; }

    public Integer getModifyUserId() { return modifyUserId; }
    public void setModifyUserId(Integer modifyUserId) { this.modifyUserId = modifyUserId; }

    public Integer getOrganizationId() { return organizationId; }
    public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }

    public Integer getProjectId() { return projectId; }
    public void setProjectId(Integer projectId) { this.projectId = projectId; }

    public Integer getValidityDays() { return validityDays; }
    public void setValidityDays(Integer validityDays) { this.validityDays = validityDays; }

    public String getShipToAddress() { return shipToAddress; }
    public void setShipToAddress(String shipToAddress) { this.shipToAddress = shipToAddress; }

    public Integer getShipToAddressId() { return shipToAddressId; }
    public void setShipToAddressId(Integer shipToAddressId) { this.shipToAddressId = shipToAddressId; }

    public Integer getDeliveryToPartyId() { return deliveryToPartyId; }
    public void setDeliveryToPartyId(Integer deliveryToPartyId) { this.deliveryToPartyId = deliveryToPartyId; }

    public String getRemarksHeader() { return remarksHeader; }
    public void setRemarksHeader(String remarksHeader) { this.remarksHeader = remarksHeader; }

    public String getApprovalRemarks() { return approvalRemarks; }
    public void setApprovalRemarks(String approvalRemarks) { this.approvalRemarks = approvalRemarks; }

    public String getAttachmentsValues() { return attachmentsValues; }
    public void setAttachmentsValues(String attachmentsValues) { this.attachmentsValues = attachmentsValues; }

    public String getCustomAttachmentsValues() { return customAttachmentsValues; }
    public void setCustomAttachmentsValues(String customAttachmentsValues) { this.customAttachmentsValues = customAttachmentsValues; }

    public String getQualitySpecifications() { return qualitySpecifications; }
    public void setQualitySpecifications(String qualitySpecifications) { this.qualitySpecifications = qualitySpecifications; }

    public List<BuyerInquiryDetailDto> getInquiryBookingDetailList() { return inquiryBookingDetailList; }
    public void setInquiryBookingDetailList(List<BuyerInquiryDetailDto> inquiryBookingDetailList) { this.inquiryBookingDetailList = inquiryBookingDetailList; }

    public List<BuyerInquiryPartyDetailDto> getInquiryBookingPartyDetailList() { return inquiryBookingPartyDetailList; }
    public void setInquiryBookingPartyDetailList(List<BuyerInquiryPartyDetailDto> inquiryBookingPartyDetailList) { this.inquiryBookingPartyDetailList = inquiryBookingPartyDetailList; }

    public static class BuyerInquiryDetailDto {
        private Integer inquiryBookingDetailId = 0;
        private Integer inquiryBookingMasterId = 0;
        private Integer itemId = 0;
        private Integer inventoryParentCategoryId = 0;
        private BigDecimal itemQty = BigDecimal.ZERO;
        private BigDecimal itemWeight = BigDecimal.ZERO;
        private BigDecimal buyerRate = BigDecimal.ZERO;
        private BigDecimal buyerAmount = BigDecimal.ZERO;
        private BigDecimal supplierRate = BigDecimal.ZERO;
        private BigDecimal supplierAmount = BigDecimal.ZERO;
        private Integer packingTypeId = 0;
        private Integer packUomId = 0;
        private Integer rateUomId = 0;
        private Integer cropYearId = 0;
        private String cropYear;
        private String qualitySpecifications;
        private String remarks;
        private Integer sortNo = 1;
        private Integer actionTypeId = 1;

        public Integer getInquiryBookingDetailId() { return inquiryBookingDetailId; }
        public void setInquiryBookingDetailId(Integer inquiryBookingDetailId) { this.inquiryBookingDetailId = inquiryBookingDetailId; }

        public Integer getInquiryBookingMasterId() { return inquiryBookingMasterId; }
        public void setInquiryBookingMasterId(Integer inquiryBookingMasterId) { this.inquiryBookingMasterId = inquiryBookingMasterId; }

        public Integer getItemId() { return itemId; }
        public void setItemId(Integer itemId) { this.itemId = itemId; }

        public Integer getInventoryParentCategoryId() { return inventoryParentCategoryId; }
        public void setInventoryParentCategoryId(Integer inventoryParentCategoryId) { this.inventoryParentCategoryId = inventoryParentCategoryId; }

        public BigDecimal getItemQty() { return itemQty; }
        public void setItemQty(BigDecimal itemQty) { this.itemQty = itemQty; }

        public BigDecimal getItemWeight() { return itemWeight; }
        public void setItemWeight(BigDecimal itemWeight) { this.itemWeight = itemWeight; }

        public BigDecimal getBuyerRate() { return buyerRate; }
        public void setBuyerRate(BigDecimal buyerRate) { this.buyerRate = buyerRate; }

        public BigDecimal getBuyerAmount() { return buyerAmount; }
        public void setBuyerAmount(BigDecimal buyerAmount) { this.buyerAmount = buyerAmount; }

        public BigDecimal getSupplierRate() { return supplierRate; }
        public void setSupplierRate(BigDecimal supplierRate) { this.supplierRate = supplierRate; }

        public BigDecimal getSupplierAmount() { return supplierAmount; }
        public void setSupplierAmount(BigDecimal supplierAmount) { this.supplierAmount = supplierAmount; }

        public Integer getPackingTypeId() { return packingTypeId; }
        public void setPackingTypeId(Integer packingTypeId) { this.packingTypeId = packingTypeId; }

        public Integer getPackUomId() { return packUomId; }
        public void setPackUomId(Integer packUomId) { this.packUomId = packUomId; }

        public Integer getRateUomId() { return rateUomId; }
        public void setRateUomId(Integer rateUomId) { this.rateUomId = rateUomId; }

        public Integer getCropYearId() { return cropYearId; }
        public void setCropYearId(Integer cropYearId) { this.cropYearId = cropYearId; }

        public String getCropYear() { return cropYear; }
        public void setCropYear(String cropYear) { this.cropYear = cropYear; }

        public String getQualitySpecifications() { return qualitySpecifications; }
        public void setQualitySpecifications(String qualitySpecifications) { this.qualitySpecifications = qualitySpecifications; }

        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }

        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }

        public Integer getActionTypeId() { return actionTypeId; }
        public void setActionTypeId(Integer actionTypeId) { this.actionTypeId = actionTypeId; }
    }

    public static class BuyerInquiryPartyDetailDto {
        private Integer inquiryBookingPartyDetailId = 0;
        private Integer inquiryBookingMasterId = 0;
        private Integer subPartyId = 0;
        private BigDecimal itemQty = BigDecimal.ZERO;
        private BigDecimal rate = BigDecimal.ZERO;
        private Integer rateUomId = 0;
        private BigDecimal amount = BigDecimal.ZERO;
        private Integer sortNo = 1;
        private String remarks;
        private Integer actionTypeId = 1;

        public Integer getInquiryBookingPartyDetailId() { return inquiryBookingPartyDetailId; }
        public void setInquiryBookingPartyDetailId(Integer inquiryBookingPartyDetailId) { this.inquiryBookingPartyDetailId = inquiryBookingPartyDetailId; }

        public Integer getInquiryBookingMasterId() { return inquiryBookingMasterId; }
        public void setInquiryBookingMasterId(Integer inquiryBookingMasterId) { this.inquiryBookingMasterId = inquiryBookingMasterId; }

        public Integer getSubPartyId() { return subPartyId; }
        public void setSubPartyId(Integer subPartyId) { this.subPartyId = subPartyId; }

        public BigDecimal getItemQty() { return itemQty; }
        public void setItemQty(BigDecimal itemQty) { this.itemQty = itemQty; }

        public BigDecimal getRate() { return rate; }
        public void setRate(BigDecimal rate) { this.rate = rate; }

        public Integer getRateUomId() { return rateUomId; }
        public void setRateUomId(Integer rateUomId) { this.rateUomId = rateUomId; }

        public BigDecimal getAmount() { return amount; }
        public void setAmount(BigDecimal amount) { this.amount = amount; }

        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }

        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }

        public Integer getActionTypeId() { return actionTypeId; }
        public void setActionTypeId(Integer actionTypeId) { this.actionTypeId = actionTypeId; }
    }
}
