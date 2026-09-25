package com.mst.models.cmagt.dto;

import com.fasterxml.jackson.annotation.JsonAlias;
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
    /* The desktop model spells this with a leading capital, and the page posts it that way.
       Jackson derives the property name from the getter, so the posted key did not bind and
       Spring Boot's default (FAIL_ON_UNKNOWN_PROPERTIES=false) dropped it in silence.
       @JsonAlias accepts the desktop's own spelling. */
    @JsonAlias({"ShipToAddress"})
    private String shipToAddress;
    private Integer shipToAddressId = 0;
    @JsonAlias({"DeliveryToPartyId"})
    private Integer deliveryToPartyId = 0;
    private String remarksHeader;
    private String approvalRemarks;
    private String attachmentsValues;
    private String customAttachmentsValues;
    @JsonAlias({"QualitySpecifications"})
    private String qualitySpecifications;

    /* ------------------------------------------------------------------------------------
       UI-ONLY header fields.

       CmbPaymentTerm and txtDueDays are controls on the desktop form, but they are NOT
       properties of Architecture.Model.CommissionAgent.InquiryBookingMaster, so they are never
       parameters of [cmagt].[USP_inquiryBookingMaster_InsertAndUpdate]. Their only use is to
       build the single InquiryBookingPaymentSchedule row at Insert():1598-1600.

       They are carried here so the page can post what the operator chose; the master writer
       must not send them.
       ------------------------------------------------------------------------------------ */
    private Integer paymentTermId = 0;
    private Integer dueDays = 0;

    public Integer getPaymentTermId() { return paymentTermId; }
    public void setPaymentTermId(Integer paymentTermId) { this.paymentTermId = paymentTermId; }

    public Integer getDueDays() { return dueDays; }
    public void setDueDays(Integer dueDays) { this.dueDays = dueDays; }

    @JsonAlias({"InquiryBookingDetailList"})
    private List<BuyerInquiryDetailDto> inquiryBookingDetailList = new ArrayList<>();
    @JsonAlias({"InquiryBookingPartyDetailList"})
    private List<BuyerInquiryPartyDetailDto> inquiryBookingPartyDetailList = new ArrayList<>();
    /* The DAL writes FOUR child collections in one transaction
       (0544_Architecture.DAL.CommissionAgent.InquiryBookingMaster.cs:56-75). These two were
       absent from this DTO, so the rows the desktop writes for every inquiry were never sent. */
    @JsonAlias({"InquiryBookingPaymentScheduleList"})
    private List<BuyerInquiryPaymentScheduleDto> inquiryBookingPaymentScheduleList = new ArrayList<>();
    @JsonAlias({"InquiryBookingQualitySpecificationList"})
    private List<BuyerInquiryQualitySpecificationDto> inquiryBookingQualitySpecificationList = new ArrayList<>();

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

    public List<BuyerInquiryPaymentScheduleDto> getInquiryBookingPaymentScheduleList() { return inquiryBookingPaymentScheduleList; }
    public void setInquiryBookingPaymentScheduleList(List<BuyerInquiryPaymentScheduleDto> v) { this.inquiryBookingPaymentScheduleList = v; }

    public List<BuyerInquiryQualitySpecificationDto> getInquiryBookingQualitySpecificationList() { return inquiryBookingQualitySpecificationList; }
    public void setInquiryBookingQualitySpecificationList(List<BuyerInquiryQualitySpecificationDto> v) { this.inquiryBookingQualitySpecificationList = v; }

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
        @JsonAlias({"SupplierAmount"})
        private BigDecimal supplierAmount = BigDecimal.ZERO;
        private Integer packingTypeId = 0;
        private Integer packUomId = 0;
        private Integer rateUomId = 0;
        private Integer cropYearId = 0;
        private String cropYear;
        @JsonAlias({"QualitySpecifications"})
        private String qualitySpecifications;
        private String remarks;
        private Integer sortNo = 0;   /* Insert() never sets it: CLR default 0 (:1577-1620) */
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
        @JsonAlias({"SubPartyId"})
        private Integer subPartyId = 0;
        private BigDecimal itemQty = BigDecimal.ZERO;
        @JsonAlias({"Rate"})
        private BigDecimal rate = BigDecimal.ZERO;
        private Integer rateUomId = 0;
        @JsonAlias({"Amount"})
        private BigDecimal amount = BigDecimal.ZERO;
        private Integer sortNo = 0;   /* Insert() never sets it: CLR default 0 (:1577-1620) */
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

    /**
     * Architecture.Model.CommissionAgent.InquiryBookingPaymentSchedule - 9 non-virtual
     * properties, declaration order preserved, which IS the parameter list of
     * [cmagt].[USP_inquiryBookingPaymentSchedule_Insert] (GenericProvider.SetProc reflects over
     * the model).
     *
     * Insert():1596-1604 builds exactly ONE of these per inquiry: the header payment term, its
     * due days, dueBaseDate = inquiryBookingDate + dueDays, 100% of total, and dueAmount =
     * the single detail row's buyerAmount.
     */
    public static class BuyerInquiryPaymentScheduleDto {
        private String dueBaseDate;
        private BigDecimal dueAmount = BigDecimal.ZERO;
        private BigDecimal pctOfTotal = BigDecimal.ZERO;
        private Integer dueDays = 0;
        private Integer inquiryBookingMasterId = 0;
        private Integer inquiryBookingPaymentScheduleId = 0;
        private Integer paymentTermId = 0;
        private Integer sortNo = 0;
        private String remarks;

        public String getDueBaseDate() { return dueBaseDate; }
        public void setDueBaseDate(String v) { this.dueBaseDate = v; }
        public BigDecimal getDueAmount() { return dueAmount; }
        public void setDueAmount(BigDecimal v) { this.dueAmount = v; }
        public BigDecimal getPctOfTotal() { return pctOfTotal; }
        public void setPctOfTotal(BigDecimal v) { this.pctOfTotal = v; }
        public Integer getDueDays() { return dueDays; }
        public void setDueDays(Integer v) { this.dueDays = v; }
        public Integer getInquiryBookingMasterId() { return inquiryBookingMasterId; }
        public void setInquiryBookingMasterId(Integer v) { this.inquiryBookingMasterId = v; }
        public Integer getInquiryBookingPaymentScheduleId() { return inquiryBookingPaymentScheduleId; }
        public void setInquiryBookingPaymentScheduleId(Integer v) { this.inquiryBookingPaymentScheduleId = v; }
        public Integer getPaymentTermId() { return paymentTermId; }
        public void setPaymentTermId(Integer v) { this.paymentTermId = v; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer v) { this.sortNo = v; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String v) { this.remarks = v; }
    }

    /**
     * Architecture.Model.CommissionAgent.InquiryBookingQualitySpecification - 7 non-virtual
     * properties = the parameter list of
     * [cmagt].[USP_inquiryBookingQualitySpecification_Insert].
     *
     * qualityParameter is the DISPLAY name and is deliberately NOT one of the seven: it is a
     * virtual property on the desktop model, so SetProc skips it. It is carried here only
     * because Insert():1574 uses it to build the master's QualitySpecifications summary string
     * and to name the parameter in its refusal messages.
     */
    public static class BuyerInquiryQualitySpecificationDto {
        private BigDecimal rangeFrom = BigDecimal.ZERO;
        private BigDecimal rangeTo = BigDecimal.ZERO;
        private Integer inquiryBookingMasterId = 0;
        private Integer inquiryBookingQualitySpecificationId = 0;
        private Integer qualityParameterId = 0;
        private Integer sortNo = 0;
        private String remarks;
        /** virtual on the desktop model - never sent to the procedure. */
        private String qualityParameter;

        public BigDecimal getRangeFrom() { return rangeFrom; }
        public void setRangeFrom(BigDecimal v) { this.rangeFrom = v; }
        public BigDecimal getRangeTo() { return rangeTo; }
        public void setRangeTo(BigDecimal v) { this.rangeTo = v; }
        public Integer getInquiryBookingMasterId() { return inquiryBookingMasterId; }
        public void setInquiryBookingMasterId(Integer v) { this.inquiryBookingMasterId = v; }
        public Integer getInquiryBookingQualitySpecificationId() { return inquiryBookingQualitySpecificationId; }
        public void setInquiryBookingQualitySpecificationId(Integer v) { this.inquiryBookingQualitySpecificationId = v; }
        public Integer getQualityParameterId() { return qualityParameterId; }
        public void setQualityParameterId(Integer v) { this.qualityParameterId = v; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer v) { this.sortNo = v; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String v) { this.remarks = v; }
        public String getQualityParameter() { return qualityParameter; }
        public void setQualityParameter(String v) { this.qualityParameter = v; }
    }
}
